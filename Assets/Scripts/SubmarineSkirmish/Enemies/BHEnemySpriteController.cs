using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum EnemySpriteType
{
    submarine,
    fish,
    bottomFeeder
}

public class BHEnemySpriteController : MonoBehaviour
{
    [Header("Sprites")]
    public SpriteRenderer body;
    public SpriteRenderer rotor;

    [Space(5)]
    public Sprite submarineSide;
    public Sprite submarineBack;
    public Sprite submarineFront;

    [Space(5)]
    public Sprite fishSide;
    public Sprite fishBack;
    public Sprite fishFront;

    [Header("Rotors")]
    [Space(5)]
    public Transform rotorRight;
    public Transform rotorLeft;

    [Space(5)]
    public GameObject sideRotors;
    public GameObject backRotors;

    public Transform gameObject1;
    public Transform gameObject2;
    public Transform playerTransform;

    private EnemySpriteType enemyType;

    public float circleRadius
    {
        set { m_radius = value; }
    }

    public bool SpriteFlip
    {

        set { m_ignoreDistanceFlip = value; }
    }

    private bool m_ignoreDistanceFlip;
    private float m_radius;
    private float circumference;
    private float distanceHolder;
    private float sideHolder;
    private float distance;

    // Start is called before the first frame update
    void Start()
    {
        circumference = 2 * Mathf.PI * m_radius;
        GameObject playerGameObject = GameObject.FindGameObjectWithTag("Player");
        playerTransform = playerGameObject.transform;

        enemyType = EnemySpriteType.submarine;
    }

    // Update is called once per frame
    void Update()
    {
        switch (enemyType)
        {
            case EnemySpriteType.submarine:
                distance = GetPlayerDistance();


                UpdateSubmarineSprites(distance);
                break;
            case EnemySpriteType.fish:
                distance = GetPlayerDistance();
                UpdateFishSprites(distance);
                break;
        }
    }

    private void UpdateSubmarineSprites(float distance)
    {

        Sprite spriteToDisplay = null; 
        bool backRotorActive = false;
        bool sideRotorActive = false;
        bool shouldFlipX = false;
        Vector3 rotorPosition = new Vector3();

        if(distance < 2.5f)
        {

            spriteToDisplay = submarineSide;
            shouldFlipX = false;
            rotorPosition = rotorRight.transform.position;
            backRotorActive = false;
            sideRotorActive = true;

            UpdateSprite(rotorPosition, shouldFlipX, spriteToDisplay, backRotorActive, sideRotorActive);

        }
        
        if(distance >= 2.5f && distance <= 6)
        {
            float side = GetPlayerSide();
            if (side > 0f)
            {
                spriteToDisplay = submarineBack;
                backRotorActive = true;
            }
            else
            {
                spriteToDisplay = submarineFront;
                backRotorActive = false;
            }

            shouldFlipX = false;
            sideRotorActive = false;

            UpdateSprite(rotorPosition, shouldFlipX, spriteToDisplay, backRotorActive, sideRotorActive);
        }

        if(distance > 6)
        {
            spriteToDisplay = submarineSide;
            shouldFlipX = true;
            rotorPosition = rotorLeft.transform.position;
            backRotorActive = false;
            sideRotorActive = true;

            UpdateSprite(rotorPosition, shouldFlipX, spriteToDisplay, backRotorActive, sideRotorActive);
        }
    }

    private void UpdateFishSprites(float distance)
    {
        Sprite spriteToDisplay = null;
        bool shouldFlipX = false;

        if (distance < 2.5f)
        {
            spriteToDisplay = fishSide;
            shouldFlipX = false;
            UpdateSprite(shouldFlipX, spriteToDisplay);

        }

        if (distance >= 2.5f && distance <= 6)
        {
            float side = GetPlayerSide();
            if (side > 0f)
            {
                spriteToDisplay = fishBack;
            }
            else
            {
                spriteToDisplay = fishFront;
            }

            shouldFlipX = false;

            UpdateSprite(shouldFlipX, spriteToDisplay);
        }

        if (distance > 6)
        {
            spriteToDisplay = fishSide;
            shouldFlipX = true;
            UpdateSprite(shouldFlipX, spriteToDisplay);
        }
    }

    private void UpdateSprite(Vector3 rotorPosition, bool shouldFlipx, Sprite bodySprite, bool backRotor, bool sideRotor)
    {
        if (m_ignoreDistanceFlip)
        {

        }
        else
        {
            body.flipX = shouldFlipx;
            rotor.transform.position = rotorPosition;
        }

        body.sprite = bodySprite;
        backRotors.SetActive(backRotor);
        sideRotors.SetActive(sideRotor);
    }

    private void UpdateSprite(bool shouldFlipx, Sprite bodySprite)
    {
        if (m_ignoreDistanceFlip)
        {

        }
        else
        {
            body.flipX = shouldFlipx;
        }

        body.sprite = bodySprite;
    }

    private float GetPlayerDistance()
    {
        // Calculate the angle between the two gameObjects
        float angle = Vector3.Dot(gameObject1.forward, playerTransform.forward);
        angle = Mathf.Acos(angle) * Mathf.Rad2Deg;

        // Calculate the distance between the two gameObjects on the perimeter
        float distance = (angle / 360f) * circumference;

        return distance;
    }

    private float GetPlayerSide()
    {
        Vector3 heading = playerTransform.transform.position - transform.position;
        Vector3 perp = Vector3.Cross(transform.forward, heading);
        float direction = Vector3.Dot(perp, transform.up);

        return direction;
    }

    /*
     *     This is to be used when the enemy is chasing the player. 
     *     Submarine will face which direction it is moving
     * 
     */

    public void FacePlayer()
    {
        float playerSide = GetPlayerSide();

        if (playerSide > 0f)
        {
            //Right Side
            body.flipX = true;
            rotor.flipX = true;
            rotor.transform.position = rotorLeft.transform.position;
        }
        else if (playerSide < 0f)
        {
            //Right Side
            body.flipX = false;
            rotor.flipX = true;
            rotor.transform.position = rotorRight.transform.position;
        }
        else
        {
            Debug.Log(0f);
        }
    }

    public void ExposeSpecialFish()
    {
        Destroy(backRotors);
        Destroy(sideRotors);
        enemyType = EnemySpriteType.fish;
    }

}
