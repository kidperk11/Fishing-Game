using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Mini Game", menuName = "Rapid Routine/New Mini Game")]
public class MiniGameScriptableObject : ScriptableObject
{
    public GameObject miniGamePrefab;

    [TextArea(1, 5)]
    public string transitionText;

    public Sprite background;
}
