using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class InteractionWheel : MonoBehaviour
{
    public static InteractionWheel Instance { get; private set; }

    [Header("UI References")]
    [SerializeField] private GameObject wheelPanel;
    [SerializeField] private Image[] segments;  // 4 images: [Move, Break, Place, Change]
    [SerializeField] private TMPro.TextMeshProUGUI[] labels; // Optional: 4 text labels

    [Header("Colors")]
    [SerializeField] private Color disabled = new Color(0.3f, 0.3f, 0.3f, 0.4f);
    [SerializeField] private float highlightBrightness = 1.3f;
    [SerializeField] private float dimAmount = 0.6f;

    [Header("Input")]
    [SerializeField] private float deadzone = 0.4f;

    private bool isOpen;
    private int selected = -1;
    private Interactable target;
    private bool[] segmentAvailable = new bool[4];
    private Color[] originalColors = new Color[4];

    public bool IsOpen => isOpen;

    void Awake()
    {
        Instance = this;
        wheelPanel.SetActive(false);

        // Cache the original colors set in the editor
        for (int i = 0; i < 4; i++)
            originalColors[i] = segments[i].color;
    }

    public void Open(Interactable obj)
    {
        target = obj;
        isOpen = true;
        selected = -1;
        wheelPanel.SetActive(true);

        var inv = PlayerController.Instance.GetComponent<PlayerInventory>();

        for (int i = 0; i < 4; i++)
        {
            var entry = obj.supportedActions.FirstOrDefault(e => e.action == (WheelAction)i);
            bool exists = obj.supportedActions.Any(e => e.action == (WheelAction)i);
            bool hasItem = !entry.requiredItem || inv.HasItem(entry.requiredItem);
            bool available = exists && hasItem;
            segmentAvailable[i] = available;
            segments[i].color = available ? originalColors[i] : disabled;

            if (labels != null && i < labels.Length)
            {
                labels[i].text = exists ? entry.name : "";
                labels[i].color = available ? Color.white : new Color(1, 1, 1, 0.3f);
            }
        }

        PlayerController.Instance.SetMovementLocked(true);
    }

    public void Close()
    {
        isOpen = false;
        selected = -1;
        wheelPanel.SetActive(false);
        target = null;
        PlayerController.Instance.SetMovementLocked(false);
    }

    void Update()
    {
        if (!isOpen) return;

        // Cancel
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(1))
        {
            Close();
            return;
        }

        // Read raw input (same axes your PlayerController uses)
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Vector2 dir = new Vector2(h, v);

        if (dir.magnitude > deadzone)
        {
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            if (angle < 0) angle += 360f;
            selected = AngleToIndex(angle);

            // Don't allow selecting disabled segments
            if (!segmentAvailable[selected]) selected = -1;
        }
        else
        {
            selected = -1;
        }

        UpdateVisuals();

        // Confirm
        if (selected >= 0 && Input.GetKeyDown(KeyCode.Space))
        {
            var entry = target.supportedActions.FirstOrDefault(e => e.action == (WheelAction)selected);

            if (!string.IsNullOrEmpty(entry.dialogue))
                DialogueDisplay.Instance.Show(entry.dialogue);

            target.ExecuteWheelAction(entry.action);
            var inv = PlayerController.Instance.GetComponent<PlayerInventory>();
            if (entry.requiredItem && entry.destroyRequired) {
                inv.RemoveItem(entry.requiredItem);
            }
            Close();
        }
    }

    /// Maps input angle to segment index.
    /// Adjust these ranges to match your wheel's visual layout.
    private int AngleToIndex(float angle)
    {
        //       0
        //       |
        //  3 ---+--- 1
        //       |
        //       2

        if (angle >= 45f  && angle < 135f)  return 0; // Top
        if (angle >= 315f || angle < 45f)   return 1; // Right
        if (angle >= 225f && angle < 315f)  return 2; // Bottom
        return 3;                                       // Left
    }
    private void UpdateVisuals()
    {
        for (int i = 0; i < 4; i++)
        {
            if (!segmentAvailable[i]) continue;
            bool sel = (i == selected);
            segments[i].color = sel
                ? originalColors[i] * highlightBrightness
                : originalColors[i] * dimAmount;
        }
    }
}