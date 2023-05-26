using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ColliderEvents : MonoBehaviour
{
    public UnityEvent OnContact;
    private Collider collision;

    private void OnTriggerEnter(Collider other)
    {
        collision = other;
        OnContact.Invoke();
    }

    public void DestroyItem()
    {
        Destroy(collision.gameObject);
        collision = null;
    }
}
