﻿using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemTooltip : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI titleText = null;
    [SerializeField] TextMeshProUGUI bodyText = null;


    public void Setup( InventoryItem3D item)
    {
        titleText.text = item.GetDisplayName();
        bodyText.text = item.GetDescription();
    }
}
