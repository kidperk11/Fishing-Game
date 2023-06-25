using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPShadow : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public float groundOffset;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Physics.Raycast(this.transform.position, Vector3.down, out RaycastHit hit))
        {
            if (hit.transform.gameObject.CompareTag("Ground"))
            {
                spriteRenderer.enabled = true;
                if (hit.point != this.transform.position)
                {
                    this.transform.position = new Vector3(this.transform.position.x, hit.point.y + groundOffset, this.transform.position.z);
                }
            }
            else
            {
                spriteRenderer.enabled = false;
            }
        }
    }
}
