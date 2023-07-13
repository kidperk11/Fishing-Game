using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuImageRotation : MonoBehaviour
{
    public Image imageComponent;
    public float switchTime = 1f;
    public Sprite[] sprites;

    private int currentIndex = 0;

    private void Start()
    {
        StartCoroutine(SwitchImages());
    }

    private IEnumerator SwitchImages()
    {
        while (true)
        {
            imageComponent.sprite = sprites[currentIndex];
            currentIndex = (currentIndex + 1) % sprites.Length;
            yield return new WaitForSeconds(switchTime);
        }
    }
}
