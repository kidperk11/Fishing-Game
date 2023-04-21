using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubmarineAnimationController : MonoBehaviour
{
    public SpriteRenderer submarineSprite;
    public SpriteRenderer rotorSprite;
    public Transform rotorPositionRight;
    public Transform rotorPositionLeft;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Flip(float flipValue)
    {
        if (flipValue > 0)
        {
            submarineSprite.flipX = true;
            rotorSprite.transform.position = rotorPositionLeft.transform.position;
            rotorSprite.flipX = true;
        }
        else if (flipValue < 0)
        {
            submarineSprite.flipX = false;
            rotorSprite.transform.position = rotorPositionRight.transform.position;
            rotorSprite.flipX = false;
        }
    }
}
