using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/Item")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public Sprite icon;                          // for future UI icon support
    public GameObject worldPrefab;               // spawned when placed in the world
    public float placementOffset = 0f;
}