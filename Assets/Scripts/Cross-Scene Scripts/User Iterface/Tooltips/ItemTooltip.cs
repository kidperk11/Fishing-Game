using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemTooltip : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI titleText = null;
    [SerializeField] TextMeshProUGUI bodyText = null;


    public void Setup( )//InventoryItem item)
    {
        titleText.text = "I enjoy Potatos";
        bodyText.text = "I enjoy Potatos";
            //titleText.text = item.GetDisplayName();
            //bodyText.text = item.GetDescription();
    }
}
