using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public TextMeshProUGUI slotText;
    public Image iconImage;              // optional — wire up in prefab if you want icons
    public Image background;             // assign the slot's background Image in the prefab

    private int _slotIndex;
    private InventoryUI _owner;

    public void Init(int index, InventoryUI owner)
    {
        _slotIndex = index;
        _owner = owner;
    }

    public void SetHighlighted(bool highlighted)
    {
        // if (background != null)
        //     background.color = highlighted ? Color.yellow : Color.white;

        slotText.color = highlighted ? Color.yellow : Color.white;
    }

    public void SetSlot(ItemData item)
    {
        slotText.text = item != null ? item.itemName : "";

        if (iconImage != null)
            iconImage.sprite = item != null ? item.icon : null;
    }

}