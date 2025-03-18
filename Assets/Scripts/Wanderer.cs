using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wanderer : MonoBehaviour
{

    private enum State
    {
        Idle,
        Rotating,
        Moving
    }

    private State state = State.Idle;
    [HideInInspector] public WanderRegion region;

    [Header("References")]
    public Transform trans;
    public Transform modelTrans;

    [Header("Stats")]
    public float movespeed = 18;
    [Tooltip("Minimum wait time before retargeting again.")]
    public float minRetargetInterval = 4.4f;
    [Tooltip("Maximum wait time before retargeting again.")]
    public float maxRetargetInterval = 6.2f;
    [Tooltip("Time in seconds taken to rotate after targeting, before moving begins.")]
    public float rotationTime = .6f;
    [Tooltip("Time in seconds after rotation finishes before movement starts.")]
    public float postRotationWaitTime = .3f;

    private Vector3 currentTarget; // Position we're currently targeting
    private Quaternion initialRotation; // Our rotation when we first retargeted
    private Quaternion targetRotation; // The rotation we're aiming to reach
    private float rotationStartTime; // Time.time at which we started rotating

    // Called on Start and invokes itself again after each call.
    // Each invoke will wait a random time within the retarget interval.
    void Retarget()
    {
        currentTarget = region.GetRandomPointWithin(); // Get a random target point
        initialRotation = modelTrans.rotation; // Save current rotation
        targetRotation = Quaternion.LookRotation((currentTarget - trans.position).normalized); // Calculate target rotation
        state = State.Rotating; // Set state to Rotating
        rotationStartTime = Time.time; // Mark rotation start time
        Invoke("BeginMoving", rotationTime + postRotationWaitTime); // Invoke BeginMoving after rotationTime + postRotationWaitTime
    }

    // Called by Retarget to initiate movement.
    void BeginMoving()
    {
        modelTrans.rotation = targetRotation; // Ensure facing targetRotation
        state = State.Moving; // Set state to Moving
    }


    void Start()
    {

        Retarget();
    }

    // Update is called once per frame
    void Update()
    {
        if (state == State.Moving)
        {
            // Measure the distance we're moving this frame:
            float delta = movespeed * Time.deltaTime;
            // Move towards the target by the delta:
            trans.position = Vector3.MoveTowards(trans.position, currentTarget, delta);
            // Become idle and invoke the next Retarget once we hit the point:
            if (trans.position == currentTarget)
            {
                state = State.Idle;
                Invoke("Retarget", Random.Range(minRetargetInterval, maxRetargetInterval));
            }
        }
        else if (state == State.Rotating)
        {
            // Measure the time we've spent rotating so far, in seconds:
            float timeSpentRotating = Time.time - rotationStartTime;
            // Rotate from initialRotation towards targetRotation:
            modelTrans.rotation = Quaternion.Slerp(initialRotation, targetRotation, timeSpentRotating / rotationTime);
        }
    }
}
