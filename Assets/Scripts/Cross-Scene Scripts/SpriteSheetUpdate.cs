using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteSheetUpdate : MonoBehaviour
{

    [SerializeField] Sprite[] sprites = null;
    [SerializeField] public int currentSprite = 0;

    private SpriteRenderer spriteRenderer = null;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprites[currentSprite];
    }

    public void IncrementSprite()
    {
        currentSprite += 1;
        spriteRenderer.sprite = sprites[currentSprite];
    }

    public void DecrementSprite()
    {
        currentSprite -= 1;
        spriteRenderer.sprite = sprites[currentSprite - 1];
    }

    public void NewSpriteIndex(int spriteIndex)
    {
        currentSprite = spriteIndex;
        spriteRenderer.sprite = sprites[spriteIndex];
    }
}
