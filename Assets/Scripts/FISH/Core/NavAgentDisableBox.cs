using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavAgentDisableBox : MonoBehaviour
{
    public MeshRenderer mesh;
    private void Start()
    {
        mesh.enabled = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            NavMeshAgent enemyAgent = other.GetComponent<NavMeshAgent>();
            if(enemyAgent != null)
            {
                Destroy(enemyAgent);
            }
        }
    }
}
