using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ColliderEvents : MonoBehaviour
{
    [Header("Collision Tag to Check")]

    [TagSelector]
    public string objectTag;

    [Space(15)]
    public UnityEvent OnContact;
    private Collider collision3D;
    private Collider2D collision2D;

    private void OnTriggerEnter(Collider other)
    {
        collision3D = other;
        OnContact.Invoke();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision2D = collision;
        OnContact.Invoke();
    }

    public void DestroyItem()
    {
        if(collision2D.gameObject.tag == objectTag)
        {
            Destroy(collision2D.gameObject);
            collision3D = null;
        }
        
    }
}
