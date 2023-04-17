using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControllerAimAssist : MonoBehaviour
{
    public GameObject enemy;
    public Camera fpsCam;

    public PlayerInput playerInput;

    

    public float turnSpeed;
    private void FixedUpdate()
    {
        if(playerInput.currentControlScheme == "Gamepad")
        {
            Ray ray = fpsCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hit;
            Vector3 targetPoint;

            if (Physics.SphereCast(ray, 1, out hit, 10f))
            {
                if (hit.transform.gameObject.CompareTag("Enemy"))
                {
                    Vector3 enemyDirection = enemy.transform.position - fpsCam.transform.position;

                    //Note from Unity Documentation on Vector3.Dot:
                    //The dot product is a float value equal to the magnitudes of the two vectors multiplied together and then multiplied by the cosine of the angle between them.
                    float onRight = Vector3.Dot(enemyDirection, fpsCam.transform.right);



                    //Left
                    if (onRight < 0)
                    {
                        //float newRot = fpsCam.transform.rotation.y - 0.05;
                        //fpsCam.transform.rotation += Quaternion.Euler(0, newRot, 0);
                    }

                    //Right
                    if (onRight > 0)
                    {

                    }
                }
            }

        } 
    }
}
