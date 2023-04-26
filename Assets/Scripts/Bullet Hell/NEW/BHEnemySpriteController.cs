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
        float distance;
        float side;
        distance = GetPlayerDistance();
        side = GetPlayerSide();

        if(side < 0f)
        {
            if (distance < 2.5)
            {
                //Right Side Back
                body.sprite = sideSprite;
                body.flipX = false;
                backRotors.SetActive(false);
                sideRotors.SetActive(true);
            }
            else if (distance > 2.5 && distance < 9)
            {
                //Right Front
                body.sprite = frontSprite;
                backRotors.SetActive(false);
                sideRotors.SetActive(false);
            }
            else// (distance > 9)
            {
                //Right Side Front
                body.sprite = sideSprite;
                rotor.transform.position = rotorRight.transform.position;
                backRotors.SetActive(false);
                sideRotors.SetActive(true);
            }
        }
        else
        {
            if (distance < 2.5)
            {
                body.sprite = sideSprite;
                rotor.transform.position = rotorRight.transform.position;
                backRotors.SetActive(false);
                sideRotors.SetActive(true);
            }
            else if (distance > 2.5f && distance < 7)
            {
                body.sprite = backSprite;
                backRotors.SetActive(true);
                sideRotors.SetActive(false);
            }
            else// (distance > 7)
            {
                body.sprite = sideSprite;
                body.flipX = true;
                rotor.transform.position = rotorLeft.transform.position;
                backRotors.SetActive(false);
                sideRotors.SetActive(true);
            }
        }





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

    public void FacePlayer(Vector3 playerPosition)
    {
        float playerSide = GetPlayerSide(playerPosition);

        if (playerSide > 0f)
        {
            body.flipX = true;
            rotor.flipX = true;
            rotor.transform.position = rotorLeft.transform.position;
        }
        else if (playerSide < 0f)
        {
            body.flipX = false;
            rotor.flipX = true;
            rotor.transform.position = rotorRight.transform.position;
        }
        else
        {
            Debug.Log(0f);
        }
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


}
