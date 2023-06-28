using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTheSnake : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {


        GetComponent<Rigidbody2D>().velocity = transform.right * 10f * Time.deltaTime;
    }
}
