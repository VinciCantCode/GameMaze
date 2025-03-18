using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("References")]
    public Transform trans;  

    [Header("Stats")]
    [Tooltip("How many units the projectile will move forward per second.")]
    public float speed = 34;
    [Tooltip("The distance the projectile will travel before it comes to a stop.")]
    public float range = 70;

    private Vector3 spawnPoint;

    //New Variables
    [Header("Direction")]
    [Tooltip("The direction the projectile will travel.")]
    public Vector3 moveDirection = Vector3.forward; 

    void Start()
    {
        spawnPoint = transform.position;  // Use transform instead
    }

    void Update()
    {
      
        trans.Translate(moveDirection.normalized * speed * Time.deltaTime, Space.World);

        // Destroy projectile if out of range
        if (Vector3.Distance(trans.position, spawnPoint) >= range)
        {
            ResetToSpawnPoint();
        }

    void ResetToSpawnPoint()
    {
        trans.position = spawnPoint;
        
    }

    }
}
