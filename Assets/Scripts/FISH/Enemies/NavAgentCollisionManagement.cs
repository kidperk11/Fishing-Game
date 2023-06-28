using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavAgentCollisionManagement : MonoBehaviour
{
    [Header("External References")]
    public Rigidbody rb;
    public NavMeshAgent agent;

    [Header("Collision Management")]
    public Quaternion defaultRotation;
    [SerializeField] private float maxRagdollTime;
    public bool isRagdoll;
    private float ragdollTimer;

    private void Start()
    {
        defaultRotation = transform.rotation;
    }

    private void Update()
    {
        if (isRagdoll)
        {
            ragdollTimer += Time.deltaTime;
            if (ragdollTimer >= maxRagdollTime)
            {
                ragdollTimer = 0;
                isRagdoll = false;
                EndRagdoll();
            }
        }
    }

    public void TakeKnockBack(Vector3 knockbackDirection, float knockbackForce)
    {
        agent.enabled = false;
        rb.isKinematic = false;
        rb.useGravity = true;
        rb.AddForce(knockbackDirection * knockbackForce, ForceMode.Impulse);
        isRagdoll = true;
    }

    private void EndRagdoll()
    {
        rb.isKinematic = true;
        rb.useGravity = false;
        agent.enabled = true;
        transform.rotation = defaultRotation;
    }

}
