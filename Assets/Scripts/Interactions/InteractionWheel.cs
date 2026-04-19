using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class InteractionWheel : MonoBehaviour
{
    public static InteractionWheel Instance { get; private set; }

    [Header("UI References")]
    [SerializeField] private GameObject wheelPanel;
    [SerializeField] private Image[] segments;
    [SerializeField] private TMPro.TextMeshProUGUI[] labels;

    [Header("Colors")]
    [SerializeField] private Color disabled = new Color(0.3f, 0.3f, 0.3f, 0.4f);
    [SerializeField] private float highlightBrightness = 1.3f;
    [SerializeField] private float dimAmount = 0.6f;

    public bool isOpen;
    private int selected = -1;
    private Interactable target;
    private bool[] segmentAvailable = new bool[4];
    private Color[] originalColors = new Color[4];
    private bool ignoreFirstFrame;

    public bool IsOpen => isOpen;

    void Awake()
    {
        Instance = this;
        wheelPanel.SetActive(false);

        for (int i = 0; i < 4; i++)
            originalColors[i] = segments[i].color;
    }

    public void Open(Interactable obj)
    {
        target = obj;
        isOpen = true;
        selected = -1;
        ignoreFirstFrame = true; // Skip the frame that opened us
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

        // Skip the frame that opened the wheel so the Space press
        // that triggered Interact() doesn't bleed through
        if (ignoreFirstFrame)
        {
            ignoreFirstFrame = false;
            return;
        }

        // Cancel
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(1))
        {
            Close();
            return;
        }

        // Tap to select
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) TrySelect(0);
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) TrySelect(1);
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) TrySelect(2);
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) TrySelect(3);

        UpdateVisuals();

        // Confirm
        if (selected >= 0 && Input.GetKeyDown(KeyCode.Space))
        {
            var entry = target.supportedActions.FirstOrDefault(e => e.action == (WheelAction)selected);

            if (!string.IsNullOrEmpty(entry.dialogue))
                DialogueDisplay.Instance.Show(entry.dialogue);

            target.ExecuteWheelAction(entry.action);
            var inv = PlayerController.Instance.GetComponent<PlayerInventory>();
            if (entry.requiredItem && entry.destroyRequired)
            {
                inv.RemoveItem(entry.requiredItem);
            }
            Close();
        }
    }

    private void TrySelect(int index)
    {
        if (segmentAvailable[index])
            selected = index;
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