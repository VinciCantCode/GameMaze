using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    // References
    [Header("References")]
    public Transform trans;
    public Transform modelTrans;
    public CharacterController characterController;
    public GameObject cam;

    // Movement
    [Header("Movement")]
    [Tooltip("Units moved per second at maximum speed.")]
    public float movespeed = 6f;

    [Tooltip("Time, in seconds, to reach maximum speed.")]
    public float timeToMaxSpeed = .26f;

    private float VelocityGainPerSecond { get { return movespeed / timeToMaxSpeed; } }

    [Tooltip("Time, in seconds, to go from maximum speed to stationary.")]
    public float timeToLoseMaxSpeed = .2f;

    private float VelocityLossPerSecond { get { return movespeed / timeToLoseMaxSpeed; } }

    [Tooltip("Multiplier for momentum when attempting to move in a direction opposite the current traveling direction.")]
    public float reverseMomentumMultiplier = 2.2f;

    private Vector3 movementVelocity = Vector3.zero;

    //Dashing
    [Header("Dashing")]
    [Tooltip("Total distance traveled during a dash.")]
    public float dashDistance = 17;
    [Tooltip("Time taken for a dash (in seconds).")]
    public float dashTime = .26f;

    private bool IsDashing
    {
        get
        {
            return (Time.time < dashBeginTime + dashTime);
        }
    }
    private Vector3 dashDirection;
    private float dashBeginTime = Mathf.NegativeInfinity;

    [Tooltip("Time after dashing finishes before it can be performed again.")]
    public float dashCooldown = 1.8f;
    private bool CanDashNow
    {
        get
        {
            return (Time.time > dashBeginTime + dashTime + dashCooldown);
        }
    }

    //Movement  
    private void Movement()
    {
        if (!IsDashing)
        {
            float moveX = Input.GetAxis("Horizontal");
            float moveZ = Input.GetAxis("Vertical");

            Vector3 moveDirection = new Vector3(moveX, 0, moveZ).normalized;

            if (moveDirection.magnitude > 0)
            {
                movementVelocity = moveDirection * movespeed;
                characterController.Move(movementVelocity * Time.deltaTime);

                Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
                modelTrans.rotation = Quaternion.Slerp(modelTrans.rotation, targetRotation, Time.deltaTime * 10f);
            }
        }
    }

    // Player script class
    private bool paused = false;
    private void Pausing()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //Toggle pause status:
            paused = !paused;
            //If we're now paused, set timeScale to 0:
            if (paused)
                Time.timeScale = 0;
            //Otherwise if we're no longer paused, revert timeScale to 1:
            else
                Time.timeScale = 1;
        }
    }

    void Update()
    {
        if (!paused)
        {
            Movement();
            Dashing();
        }
        Pausing();
        if (Input.GetKeyDown(KeyCode.T))
        {
            Die();
        }

    }


    [Header("Death and Respawning")]
    [Tooltip("How long after the player's death, in seconds, before they are respawned?")]
    public float respawnWaitTime = 2f;
    private bool dead = false;
    private Vector3 spawnPoint;
    private Quaternion spawnRotation;

    private void Start()
    {
        spawnPoint = transform.position;
        spawnRotation = modelTrans.rotation;
    }

    void OnGUI()
    {
        if (paused)
        {
            float boxWidth = Screen.width * .4f;
            float boxHeight = Screen.height * .4f;
            GUILayout.BeginArea(new Rect(
                (Screen.width * .5f) - (boxWidth * .5f),
                (Screen.height * .5f) - (boxHeight * .5f),
                boxWidth,
                boxHeight));

            if (GUILayout.Button("RESUME GAME", GUILayout.Height(boxHeight * .5f)))
            {
                paused = false;
                Time.timeScale = 1;
            }

            if (GUILayout.Button("RETURN TO MAIN MENU", GUILayout.Height(boxHeight * .5f)))
            {
                Time.timeScale = 1;
                SceneManager.LoadScene(0);
            }

            GUILayout.EndArea();
        }
    }



    private void Dashing()
    {
        if (!IsDashing && CanDashNow) //If not dashing right now, and dash is not on cooldown
        {
            if (Input.GetKey(KeyCode.Space)) //If space key is pressed
            {
                Vector3 movementDir = Vector3.zero;

                if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
                    movementDir.z = 1;
                else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
                    movementDir.z = -1;

                if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
                    movementDir.x = 1;
                else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
                    movementDir.x = -1;

                if (movementDir.x != 0 || movementDir.z != 0)
                {
                    dashDirection = movementDir;
                    dashBeginTime = Time.time;
                    movementVelocity = dashDirection * movespeed;
                    modelTrans.forward = dashDirection;
                }
            }
        }
        if (IsDashing) //If dashing
        {
            characterController.Move(dashDirection * (dashDistance / dashTime) * Time.deltaTime);
        }
    }



    public void Die()
    {
        if (dead) return;

        dead = true;
        Invoke("Respawn", respawnWaitTime);
        movementVelocity = Vector3.zero;

        enabled = false;
        characterController.enabled = false;
        modelTrans.gameObject.SetActive(false);

        dashBeginTime = Mathf.NegativeInfinity;

    }

    public void Respawn()
    {
        dead = false;
        transform.position = spawnPoint;
        modelTrans.rotation = spawnRotation;

        enabled = true;
        characterController.enabled = true;
        modelTrans.gameObject.SetActive(true);
    }

    // Detect collision with hazards
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Hazard"))
        {
            Die();
        }
    }


}


