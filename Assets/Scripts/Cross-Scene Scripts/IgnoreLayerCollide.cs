using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreLayerCollide : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] LayerMask firstLayer;
    [SerializeField] LayerMask lastLayer;

    // Start is called before the first frame update
    void Start()
    {
        Physics2D.IgnoreLayerCollision(10, 11, true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
