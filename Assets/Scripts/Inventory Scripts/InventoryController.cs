using UnityEngine;

/// <summary>
/// Singleton that manages inventory navigation mode.
/// When active, movement keys scroll through slots and
/// player movement is suspended.
/// </summary>
public class InventoryController : MonoBehaviour
{
    public static InventoryController Instance { get; private set; }

    [Header("Input")]
    [SerializeField] private string navigateLeftKey = "a";
    [SerializeField] private string navigateRightKey = "d";
    [SerializeField] private string confirmKey = "f";
    [SerializeField] private string cancelKey = "e";

    private PlayerInventory _inventory;
    private InventoryUI _inventoryUI;
    private PlacementPoint _activePlacementPoint;
    private int _selectedSlot = 0;
    private bool _isActive = false;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    void Update()
    {   
        if (!_isActive) return;

        PlayerController.Instance.SetMovementLocked(true);

        if (Input.GetKeyDown(navigateLeftKey))
            Navigate(-1);
        else if (Input.GetKeyDown(navigateRightKey))
            Navigate(1);
        else if (Input.GetKeyDown(confirmKey))
            ConfirmPlacement();
        else if (Input.GetKeyDown(cancelKey))
            EndPlacement();
    }

    /// <summary>Called by PlacementPoint when the player interacts with it.</summary>
    public void BeginPlacement(PlacementPoint point)
    {
        _inventory = PlayerController.Instance.GetComponent<PlayerInventory>();
        _inventoryUI = FindObjectOfType<InventoryUI>();
        _activePlacementPoint = point;
        _selectedSlot = FirstOccupiedSlot();

        if (_selectedSlot < 0)
        {
            Debug.Log("No items to place.");
            return;
        }

        _isActive = true;
        PlayerController.Instance.SetMovementLocked(true);
        _inventoryUI.SetNavigating(true, _selectedSlot);
    }

    private void Navigate(int direction)
    {
        int size = _inventory.inventorySize;
        for (int i = 1; i <= size; i++)
        {
            int next = (_selectedSlot + direction * i + size) % size;
            if (_inventory.slots[next] != null)
            {
                _selectedSlot = next;
                _inventoryUI.SetNavigating(true, _selectedSlot);
                return;
            }
        }
    }

    private void EndPlacement()
    {
        _isActive = false;
        _activePlacementPoint = null;
        PlayerController.Instance.SetMovementLocked(false);
        _inventoryUI.SetNavigating(false, -1);
    }

    // Called when the player "confirms" by pressing navigateLeft/Right and landing,
    // but since we place immediately on selection we place during Navigate instead.
    private void ConfirmPlacement()
    {   
        ItemData item = _inventory.slots[_selectedSlot];
        _activePlacementPoint.OnItemPlaced(item);
        EndPlacement();
    }

    private int FirstOccupiedSlot()
    {
        for (int i = 0; i < _inventory.slots.Count; i++)
            if (_inventory.slots[i] != null) return i;
        return -1;
    }
}