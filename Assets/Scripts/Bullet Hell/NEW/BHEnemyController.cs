using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState
{
    Idle,
    Swim,
    Spotted,
    Pursuit,
    ReturnToIdle,
    Follow,
    Die,
    Attack,
    RadiusChange,
};

public enum EnemyMovementType
{
    Flee,
    Idle,
    Chase
};

public enum EnemyType
{
    Melee,
    Ranged
};

public class BHEnemyController : MonoBehaviour
{
    [Header("Optimization")]
    public bool vertical = false;
    public bool horizontal = false;
    public bool tracking = false;

    [Tooltip("Controls enemy attack range and spot range.")]
    [SerializeField]
    public TargetScanner playerScanner;

    [Header("Verical Movement")]
    public float verticalMoveSpeed;
    [Range(0.6f, .999f)]
    public float dragCoefficient = 0.95f;

    [Header("Chase Rules")]
    public float timeToStopPursuit;
    public bool respawnOnDeath = false;
    public int respawnLimit = 0;

    [Space(10)]
    [Header("Horizontal Movement")]
    public Transform center;
    public float radius = 2.0f;
    public float radiusSpeed = 0.5f;
    public float horizontalMoveSpeed = 80.0f;
    private Vector3 axis = Vector3.up;
    private Vector3 desiredPosition;
    float currentSpeed = 0.0f;
    float targetSpeed = 0.0f;
    float smoothTime = 0.2f;
    float currentVelocity = 0.0f;

    [Header("Enemy Attack Type")]
    public EnemyState currentState;
    public EnemyType enemyType;
    public EnemyMovementType enemyMovementType;

    [Header("Other Components")]
    public BHEnemySpriteController spriteController;
    public Rigidbody rigidBody;
    public bool overrideSpriteFlip;

    private float m_TimerSinceLostTarget = 0.0f;
    private BHPlayerController target { get { return m_Target; } }
    private BHPlayerController m_Target = null;
    public Rigidbody targetRigidBody = null;
    private bool canAttack = false;

    private void OnEnable()
    {
        spriteController.circleRadius = radius;
    }

    private void Start()
    {
        currentState = EnemyState.Swim;
        //rigidBody = GetComponent<Rigidbody>();
        transform.position = (transform.position - center.position).normalized * radius + center.position;

    }

    // Update is called once per frame
    void Update()
    {
        targetSpeed = horizontalMoveSpeed;
        spriteController.SpriteFlip = overrideSpriteFlip;
    }

    private void FixedUpdate()
    {
        switch (currentState)
        {
            case (EnemyState.Swim):
                //Runs until target is found
                Swimming();
                break;
            case (EnemyState.Spotted):
                //Spotted state only runs once
                Spotted();
                break;
            case (EnemyState.Pursuit):
                //Runs until target is lost
                Pursuit();
                break;
            case (EnemyState.ReturnToIdle):
                StopPursuit();
                break;
            case (EnemyState.RadiusChange):
                NewRadius();
                break;
        }
    }

    private void Swimming()
    {
        if (tracking)
            FindTarget();
        if (vertical)
            UpdateHeight();
        if (horizontal)
            SwimRotate();
    }

    private void Spotted()
    {
        FindTarget();
        if (target == null)
        {
            StopPursuit();
        }
        //spriteController.FacePlayer(target.transform.position);
        StartPursuit();
    }

    private void StopPursuit()
    {
        targetRigidBody = null;
        currentState = EnemyState.Swim;
        canAttack = false;
    }

    private void StartPursuit()
    {
        targetRigidBody = target.GetComponentInChildren<Rigidbody>();
        currentState = EnemyState.Pursuit;
        canAttack = true;
    }

    private void Pursuit()
    {
        FindTarget();
        if (target == null)
        {
            StopPursuit();
        }
        float side = GetPlayerSide();
        PursuitRotate(side);
        ChaseHeight();
    }

    private float GetPlayerSide()
    {
        Vector3 heading = target.transform.position - transform.position;
        Vector3 perp = Vector3.Cross(transform.forward, heading);
        float direction = Vector3.Dot(perp, transform.up);

        return direction;
    }

    private void FindTarget()
    {
        //we ignore height difference if the target was already seen
        BHPlayerController target = playerScanner.Detect(transform, m_Target == null);

        if (m_Target == null)
        {
            //we just saw the player for the first time, pick an empty spot to target around them
            if (target != null)
            {
                //Spotted state only runs once
                currentState = EnemyState.Spotted;
                m_Target = target;
            }
        }
        else
        {
            //we lost the target. they only loose the player if they move `past their detection range
            //and they didn't see the player for a given time. Not if they move out of their detectionAngle.
            if (target == null)
            {
                m_TimerSinceLostTarget += Time.deltaTime;
                //currentState = EnemyState.Swim;

                if (m_TimerSinceLostTarget >= timeToStopPursuit)
                {
                    //Lost the player, reset the target
                    m_Target = null;
                }
            }
            else
            {
                if (target != m_Target)
                {
                    m_Target = target;
                }
                m_TimerSinceLostTarget = 0.0f;
            }
        }
    }

    private void SwimRotate()
    {
        // Accelerate/decelerate towards target speed
        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref currentVelocity, smoothTime);

        transform.RotateAround(center.position, Vector3.up, currentSpeed * Time.deltaTime);
    }

    private void PursuitRotate(float targetSide)
    {
        // Accelerate/decelerate towards target speed
        //currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref currentVelocity, smoothTime);

        if (targetSide > 0f)
        {
            transform.RotateAround(center.position, Vector3.up, -horizontalMoveSpeed * Time.deltaTime);
        }
        if (targetSide < 0f)
        {
            transform.RotateAround(center.position, Vector3.up, horizontalMoveSpeed * Time.deltaTime);
        }

        spriteController.FacePlayer();
    }

    private void ChaseHeight()
    {
        float inputY = 0;

        if (rigidBody.position.y > targetRigidBody.transform.position.y)
        {
            //Below
            inputY = -1;
            overrideSpriteFlip = true;
        }
        else if (rigidBody.position.y < targetRigidBody.transform.position.y)
        {
            //Above
            inputY = 1;
            overrideSpriteFlip = true;
        }
        else
        {
            //Same Height
            inputY = 0;
            overrideSpriteFlip = false;
        }


        // Apply movement force to the Rigidbody
        rigidBody.AddForce(new Vector2(0, inputY) * verticalMoveSpeed);

        // Reduce velocity gradually when no input is given
        if (inputY == 0)
        {
            rigidBody.velocity *= dragCoefficient;
        }
    }

    private void NewRadius()
    {
        // Move enemy towards desired position
        desiredPosition = (transform.position - center.position).normalized * radius + center.position;
        transform.position = Vector3.MoveTowards(transform.position, desiredPosition, Time.deltaTime * radiusSpeed);
    }


    private void UpdateHeight()
    {
        float inputY = 0;

        if(Input.GetKey(KeyCode.U))
        {
            inputY = 1;
        }
        else if (Input.GetKey(KeyCode.J))
        {
            inputY = -1;
        }
        else
        {
            inputY = 0;
        }

        // Apply movement force to the Rigidbody
        rigidBody.AddForce(new Vector2(0, inputY) * verticalMoveSpeed);

        // Reduce velocity gradually when no input is given
        if (inputY == 0)
        {
            rigidBody.velocity *= dragCoefficient;
        }
    }

    public static void ClearConsole()
    {
        var assembly = Assembly.GetAssembly(typeof(SceneView));
        var type = assembly.GetType("UnityEditor.LogEntries");
        var method = type.GetMethod("Clear");
        method.Invoke(new object(), null);
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        playerScanner.EditorGizmo(transform);
    }
#endif
}
