using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QTEActivationBox : MonoBehaviour
{
    public QTETickerController ticker;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("FPSTicker"))
        {
            Debug.Log("The ticker has entered an activation box");
            ticker.currentActivationBox = this;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("FPSTicker"))
        {
            ticker.currentActivationBox = null;
        }
    }
}
