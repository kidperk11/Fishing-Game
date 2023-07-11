using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public enum EnemyState
{
    Idle,
    Swim,
    Leader,
    Follower,
    Spotted,
    Pursuit,
    ReturnToIdle,
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
    [Header("Debugging")]
    public bool vertical = false;
    public bool horizontal = false;
    public bool tracking = false;
    public float angularPosition = -80f;
    public bool updateEnemyPlacement;

    [Tooltip("Controls enemy attack range and spot range.")]
    public TargetScanner playerScanner;
    public bool displayScanner;

    [Header("Verical Movement")]
    public float verticalMoveSpeed;
    [Range(0.6f, .999f)]
    public float dragCoefficient = 0.95f;

    // Swimming/Bobbing movement
    public float sinWaveSpeed = 5f;           // Speed of vertical movement
    public float sinWaveRange = .3f;           // Range of vertical movement
    private float crissCrossTime = 0f;          // Timer for oscillation


    [Header("Chase Rules")]
    public float timeToStopPursuit;
    public bool respawnOnDeath = false;
    public int respawnLimit = 0;

    [Header("Horizontal Movement")]
    public Transform center;
    public float radius = 2.0f;
    public float radiusSpeed = 0.5f;
    public float horizontalMoveSpeed = 80.0f;

    [Header("Enemy Attack Type")]
    public EnemyState currentState;
    public EnemyType enemyType;
    public EnemyMovementType enemyMovementType;

    [Header("Other Components")]
    public BHEnemySpriteController spriteController;
    public Rigidbody rigidBody;
    public bool overrideSpriteFlip;
    public GameObject followTarget;
    public Rigidbody targetRigidBody = null;

    //Movement information
    private Vector3 axis = Vector3.up;
    private Vector3 desiredPosition;
    internal Vector3 offset;
    private float currentSpeed = 0.0f;
    private float targetSpeed = 0.0f;
    private float smoothTime = 0.2f;
    private float currentVelocity = 0.0f;
    private float m_TimerSinceLostTarget = 0.0f;
    internal Rigidbody rigidBodyToFollow = null;

    //Tracking information
    private BHPlayerController target { get { return m_Target; } }
    private BHPlayerController m_Target = null;

    private void OnEnable()
    {
        if(spriteController != null)
            spriteController.circleRadius = radius;
        if (center == null)
            center = GameObject.FindGameObjectWithTag("AtlantisCenter").transform;
    }

    private void Start()
    {

        //Once set, tranform.forward will always face the center
        transform.LookAt(center);
        transform.position = (transform.position - center.position).normalized * radius + center.position;

        horizontalMoveSpeed = UnityEngine.Random.Range(40, 80);
    }

    void Update()
    {

        targetSpeed = horizontalMoveSpeed;
        if(spriteController != null)
            spriteController.SpriteFlip = overrideSpriteFlip;

        if(updateEnemyPlacement)
        {
            PlaceEnemyOnCircle();
        }
    }

    private void FixedUpdate()
    {
        switch (currentState)
        {
            case (EnemyState.Idle):
                IdleLoop();
                break;

            case (EnemyState.Leader):
                //Runs until target is found, Breaks up the entire school
                if (!vertical)
                    vertical = true;
                if (!horizontal)
                    horizontal = true;
                LeaderLoop();
                break;

            case (EnemyState.Follower):
                //Runs until target is found, Breaks away from the school leader
                FollowerLoop();
                break;

            case (EnemyState.Swim):
                //Runs until target is found
                SwimmingLoop();
                break;

            case (EnemyState.Spotted):
                //Spotted state only runs once
                Spotted();
                break;

            case (EnemyState.Pursuit):
                //Runs until target is lost
                PursuitLoop();
                break;

            case (EnemyState.ReturnToIdle):
                StopPursuit();
                break;

            case (EnemyState.RadiusChange):
                MoveToNewRadius();
                break;
        }
    }

    private void LeaderLoop()
    {
        if (tracking)
            SearchForTarget();
        if (vertical)
            SwimHeight();
        if (horizontal)
            SwimRotate();
    }

    private void IdleLoop()
    {

        if (horizontal)
            SwimRotate();
    }

    private void FollowerLoop()
    {
        if (tracking)
            SearchForTarget();

        FollowSchoolLeader();
    }

    private void SwimmingLoop()
    {
        if (tracking)
            SearchForTarget();
        if (vertical)
            SwimHeight();
        if (horizontal)
            SwimRotate();
    }

    private void Spotted()
    {
        SearchForTarget();
        if (target == null)
        {
            StopPursuit();
        }
        //spriteController.FacePlayer(target.transform.position);
        StartPursuit();
    }

    private void PursuitLoop()
    {
        SearchForTarget();
        if (target == null)
        {
            StopPursuit();
        }
        float side = GetPlayerSide();
        PursuitRotate(side);
        ChaseHeight();
    }

    private void StopPursuit()
    {
        targetRigidBody = null;
        currentState = EnemyState.Swim;
    }

    private void MoveToNewRadius()
    {
        Vector3 newRadius = new Vector3();
        //Debug.Log("Non normalized: " + (transform.position - center.position));
        //Debug.Log("Normalized: " + (transform.position - center.position).normalized);
        // Move enemy towards desired position
        desiredPosition = (transform.position - center.position).normalized * radius + center.position;
        newRadius = desiredPosition - transform.position;
        //newRadius = Vector3.MoveTowards(transform.position, desiredPosition, Time.deltaTime * radiusSpeed);
        transform.position += newRadius;

    }

    private void SearchForTarget()
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

    private void StartPursuit()
    {
        targetRigidBody = target.GetComponentInChildren<Rigidbody>();
        currentState = EnemyState.Pursuit;
    }

    private float GetPlayerSide()
    {
        Vector3 heading = target.transform.position - transform.position;
        Vector3 perp = Vector3.Cross(transform.forward, heading);
        float direction = Vector3.Dot(perp, transform.up);

        return direction;
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

    private void SwimHeight()
    {
        float inputY = 0;

        crissCrossTime += Time.deltaTime;

        float verticalMovement = Mathf.Sin((crissCrossTime * sinWaveSpeed) + (Mathf.PI / 2)) * sinWaveRange;

        // Apply movement force to the Rigidbody
        rigidBody.AddForce(new Vector2(0, verticalMovement) * verticalMoveSpeed);


        // Reduce velocity gradually when no input is given
        if (inputY == 0)
        {
            rigidBody.velocity *= dragCoefficient;
        }
    }

    private void FollowSchoolLeader()
    {
        BHFishPositionManager positions = rigidBodyToFollow.GetComponent<BHFishPositionManager>();

        transform.position = positions.markerList[0].position + offset;
        transform.rotation = positions.markerList[0].rotation;
        positions.markerList.RemoveAt(0);
    }


    private void PlaceEnemyOnCircle( )
    {
        //transform.forward = zePlayer.transform.forward;

        // Convert angle to radians
        float radians = angularPosition * Mathf.Deg2Rad;

        // Calculate position on the circumference
        float x = radius * Mathf.Cos(radians);
        float y = 0f;
        float z = radius * Mathf.Sin(radians);

        // Set the object's position
        transform.position = new Vector3(x, y, z);
        transform.LookAt(center.transform);

        updateEnemyPlacement = false;
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if(displayScanner)
            playerScanner.EditorGizmo(transform);
    }
#endif
}
