using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpdateText : MonoBehaviour
{
    public TextMeshProUGUI textToUpdate;

    public void UpdateWithInt(ValueHolder textValue)
    {
        textToUpdate.text = textValue.integerValue.ToString();
    }

    public void UpdateWithFloat(ValueHolder textValue)
    {
        textToUpdate.text = textValue.floatValue.ToString();
    }

    public void UpdateWithString(ValueHolder textValue)
    {
        textToUpdate.text = textValue.stringValue;
    }
}
