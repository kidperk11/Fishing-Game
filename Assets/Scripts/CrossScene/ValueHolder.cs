using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValueHolder : MonoBehaviour
{
    public int integerValue;
    public float floatValue;
    public bool booleanValue;
    public string stringValue;

    public void UpdateInteger(int newValue)
    {
        integerValue += newValue;
    }

    public void UpdateFloat(float newValue)
    {
        floatValue += newValue;
    }

    public void UpdateBoolean(bool newValue)
    {
        booleanValue = newValue;
    }

    public void UpdateString(string newValue)
    {
        stringValue = newValue;
    }

}
