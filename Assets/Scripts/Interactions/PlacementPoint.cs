using System;
using UnityEngine;

[RequireComponent(typeof(Interactable))]
public class PlacementPoint : MonoBehaviour
{
    [Serializable]
    public class PlacementRule
    {
        public ItemData item;
        public string[] idsToDestroy;
        public string[] idsToReveal;

        [Tooltip("If set, this item appears on the present version of this point when placed in the past.")]
        public ItemData presentItem;
    }

    [SerializeField] private string pointID;
    [SerializeField] private string presentPointID;
    [SerializeField] private string pastPointID;
    [SerializeField] private ItemData preplacedItem;
    [SerializeField] private PlacementRule[] rules;
    [SerializeField] private LayerMask groundLayer;

    private Interactable _interactable;
    private ItemData _placedItem = null;
    private PlacementRule _activeRule = null;
    private GameObject _spawnedObject = null;
    private bool _isPreplaced = false;

    void Awake()
    {
        _interactable = GetComponent<Interactable>();
    }

    void Start()
    {
        var mgr = SceneStateManager.Instance;
        ItemData savedItem = mgr.GetPlacedItem(pointID);

        if (savedItem != null)
        {
            _placedItem = savedItem;
            _activeRule = Array.Find(rules, r => r.item != null && r.item == savedItem);
            SpawnObject(_placedItem);
        }
        else if (preplacedItem != null && !mgr.IsPickedUp(pointID))
        {
            _placedItem = preplacedItem;
            _activeRule = Array.Find(rules, r => r.item != null && r.item == preplacedItem);
            _isPreplaced = true;
            SpawnObject(_placedItem);
            ApplyRule(_activeRule);
        }

        RefreshWheel();
    }

    private void RefreshWheel()
    {
        // _interactable.supportedActions.Clear();
        _interactable.onTopAction.RemoveAllListeners();
        _interactable.onBottomAction.RemoveAllListeners();

        if (_placedItem != null)
        {
            _interactable.supportedActions.Add(new ActionEntry { action = WheelAction.Top, name = "Pick Up", dialogue = _placedItem.dialogue });
            _interactable.supportedActions.Add(new ActionEntry { action = WheelAction.Bottom, name = "Place" });
            _interactable.onTopAction.AddListener(PickUp);
            _interactable.onBottomAction.AddListener(() => InventoryController.Instance.BeginPlacement(this));
        }
        else
        {
            _interactable.supportedActions.Add(new ActionEntry { action = WheelAction.Bottom, name = "Place" });
            _interactable.onBottomAction.AddListener(() => InventoryController.Instance.BeginPlacement(this));
        }
    }

    public void OnItemPlaced(ItemData item)
    {
        if (_placedItem != null)
        {
            PlayerController.Instance.GetComponent<PlayerInventory>().AddItem(_placedItem);
            DestroyObject();
            ReverseRule(_activeRule);
            ClearCounterpartIDs();
            SceneStateManager.Instance.ClearPlacedItem(pointID);
            _placedItem = null;
            _activeRule = null;
            _isPreplaced = false;
        }

        _placedItem = item;
        _activeRule = Array.Find(rules, r => r.item == item);
        _isPreplaced = false;
        SceneStateManager.Instance.SetPlacedItem(pointID, item);
        PlayerController.Instance.GetComponent<PlayerInventory>().RemoveItem(item);
        SpawnObject(item);
        ApplyRule(_activeRule);
        SetCounterpartID(_activeRule);
        RefreshWheel();
    }

    private void PickUp()
    {
        if (_placedItem == null) return;

        PlayerController.Instance.GetComponent<PlayerInventory>().AddItem(_placedItem);
        DestroyObject();
        ReverseRule(_activeRule);
        ClearCounterpartIDs();

        var mgr = SceneStateManager.Instance;
        if (_isPreplaced) mgr.MarkPickedUp(pointID);
        else mgr.ClearPlacedItem(pointID);

        _placedItem = null;
        _activeRule = null;
        _isPreplaced = false;
        RefreshWheel();
    }

    private void ApplyRule(PlacementRule rule)
    {
        if (rule == null) return;
        var mgr = SceneStateManager.Instance;
        foreach (var id in rule.idsToDestroy)
        {
            mgr.MarkDestroyed(id);
            var obj = mgr.Find(id);
            if (obj != null) Destroy(obj.gameObject);
        }
        foreach (var id in rule.idsToReveal)
        {
            mgr.MarkSpawned(id);
            var obj = mgr.Find(id);
            if (obj != null) obj.RevealPersistent();
        }
    }

    private void ReverseRule(PlacementRule rule)
    {
        if (rule == null) return;
        var mgr = SceneStateManager.Instance;
        foreach (var id in rule.idsToDestroy) mgr.UnmarkDestroyed(id);
        foreach (var id in rule.idsToReveal) mgr.UnmarkSpawned(id);
    }

    private void SetCounterpartID(PlacementRule rule)
    {
        if (string.IsNullOrEmpty(presentPointID)) return;
        if (rule?.presentItem != null)
            SceneStateManager.Instance.SetPlacedItem(presentPointID, rule.presentItem);
        else
            SceneStateManager.Instance.ClearPlacedItem(presentPointID);
    }

    private void ClearCounterpartIDs()
    {
        var mgr = SceneStateManager.Instance;
        if (!string.IsNullOrEmpty(presentPointID)) mgr.ClearPlacedItem(presentPointID);
        if (!string.IsNullOrEmpty(pastPointID)) mgr.ClearPlacedItem(pastPointID);
    }

    private void SpawnObject(ItemData item)
    {
        if (item?.worldPrefab != null)
            _spawnedObject = Instantiate(item.worldPrefab, GetSurfacePosition(item), item.worldPrefab.transform.localRotation);
    }

    private void DestroyObject()
    {
        if (_spawnedObject != null) { Destroy(_spawnedObject); _spawnedObject = null; }
    }

    private Vector3 GetSurfacePosition(ItemData item)
    {
        Vector3 origin = transform.position + Vector3.up * 10f;
        float surfaceY = transform.position.y;
        if (Physics.Raycast(origin, Vector3.down, out RaycastHit hit, 20f, groundLayer))
            surfaceY = hit.point.y;
        return new Vector3(transform.position.x + item.placementOffset.x, surfaceY + item.placementOffset.y, transform.position.z + item.placementOffset.z);
    }
}