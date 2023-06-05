using UnityEngine;
using UnityEngine.UI;
using TMPro;



/// <summary>
/// When a item that can be stacked has 2 or more in the inventory, 
/// display an indicator with the ammount in the stack
/// </summary>
[RequireComponent(typeof(Image))]
public class InventoryItemIcon : MonoBehaviour
{
    // CONFIG DATA
    [SerializeField] GameObject textContainer = null;
    [SerializeField] TextMeshProUGUI itemNumber = null;

    // PUBLIC

    public void SetItem(InventoryItem3D item)
    {
        SetItem(item, 0);
    }

    public void SetItem(InventoryItem3D item, int number)
    {
        var iconImage = GetComponent<Image>();
        if (item == null)
        {
            iconImage.enabled = false;
        }
        else
        {
            iconImage.enabled = true;
            iconImage.sprite = item.GetIcon();
        }

        if (itemNumber)
        {
            if (number <= 1)
            {
                textContainer.SetActive(false);
            }
            else
            {
                textContainer.SetActive(true);
                itemNumber.text = number.ToString();
            }
        }
    }
}
