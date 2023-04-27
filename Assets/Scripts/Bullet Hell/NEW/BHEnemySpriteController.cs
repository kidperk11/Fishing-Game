using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BHEnemySpriteController : MonoBehaviour
{
    [Header("Sprites")]
    public SpriteRenderer body;
    public SpriteRenderer rotor;
    public Transform rotorRight;
    public Transform rotorLeft;
    public Sprite sideSprite;
    public Sprite backSprite;
    public Sprite frontSprite;

    public GameObject sideRotors;
    public GameObject backRotors;


    public Transform gameObject1;
    public Transform gameObject2;
    public float radius;

    private float circumference;
    private float distanceHolder;
    private float sideHolder;



    // Start is called before the first frame update
    void Start()
    {
        circumference = 2 * Mathf.PI * radius;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateSprite();
    }

    private void UpdateSprite()
    {
        //Possible Solution. 
        float distance;
        float side;
        distance = GetPlayerDistance();
        Sprite spriteToDisplay = null; 
        bool backRotorActive = false;
        bool sideRotorActive = false;
        bool shouldFlipX = false;
        Vector3 rotorPosition = new Vector3();


        if(distance < 2.5f)
        {

            spriteToDisplay = sideSprite;
            shouldFlipX = false;
            rotorPosition = rotorRight.transform.position;
            backRotorActive = false;
            sideRotorActive = true;

        }
        
        if(distance >= 2.5f && distance <= 6)
        {
            side = GetPlayerSide();
            if (side > 0f)
            {
                spriteToDisplay = backSprite;
                backRotorActive = true;
            }
            else
            {
                spriteToDisplay = frontSprite;
                backRotorActive = false;
            }

            shouldFlipX = false;
            sideRotorActive = false;
        }

        if(distance > 6)
        {
            spriteToDisplay = sideSprite;
            shouldFlipX = true;
            rotorPosition = rotorLeft.transform.position;
            backRotorActive = false;
            sideRotorActive = true;
        }

        UpdateSprite(rotorPosition, shouldFlipX, spriteToDisplay, backRotorActive, sideRotorActive);
    }

    private void UpdateSprite(Vector3 rotorPosition, bool shouldFlipx, Sprite bodySprite, bool backRotor, bool sideRotor)
    {
        rotor.transform.position = rotorPosition;
        body.flipX = shouldFlipx;
        body.sprite = bodySprite;
        backRotors.SetActive(backRotor);
        sideRotors.SetActive(sideRotor);
    }

    private float GetPlayerDistance()
    {
        // Calculate the angle between the two gameObjects
        float angle = Vector3.Dot(gameObject1.forward, gameObject2.forward);
        angle = Mathf.Acos(angle) * Mathf.Rad2Deg;

        // Calculate the distance between the two gameObjects on the perimeter
        float distance = (angle / 360f) * circumference;

        return distance;
    }

    private float GetPlayerSide()
    {
        Vector3 heading = gameObject2.transform.position - transform.position;
        Vector3 perp = Vector3.Cross(transform.forward, heading);
        float direction = Vector3.Dot(perp, transform.up);

        return direction;
    }

    private float GetPlayerSide(Vector3 playerPosition)
    {
        Vector3 heading = playerPosition - transform.position;
        Vector3 perp = Vector3.Cross(transform.forward, heading);
        float direction = Vector3.Dot(perp, transform.up);

        return direction;
    }



    /*
     *     This is to be used when the enemy is chasing the player. 
     *     Submarine will face which direction it is moving
     * 
     */

    //public void FacePlayer(Vector3 playerPosition)
    //{
    //    float playerSide = GetPlayerSide(playerPosition);

    //    if (playerSide > 0f)
    //    {
    //        body.flipX = true;
    //        rotor.flipX = true;
    //        rotor.transform.position = rotorLeft.transform.position;
    //    }
    //    else if (playerSide < 0f)
    //    {
    //        body.flipX = false;
    //        rotor.flipX = true;
    //        rotor.transform.position = rotorRight.transform.position;
    //    }
    //    else
    //    {
    //        Debug.Log(0f);
    //    }
    //}

}
