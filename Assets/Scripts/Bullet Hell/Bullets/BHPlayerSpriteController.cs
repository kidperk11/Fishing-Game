using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BHPlayerSpriteController : MonoBehaviour
{
    public SpriteRenderer submarine;
    public SpriteRenderer rotor;
    public Transform rotorRight;
    public Transform rotorLeft;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FlipSprite(float direction)
    {
        if (direction > 0)
        {
            submarine.flipX = true;
            rotor.transform.position = rotorLeft.transform.position;
            rotor.flipX = true;
        }
        else if (direction < 0)
        {
            submarine.flipX = false;
            rotor.transform.position = rotorRight.transform.position;
            rotor.flipX = false;
        }
    }

}
