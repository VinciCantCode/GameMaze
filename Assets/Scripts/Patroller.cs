using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patroller : MonoBehaviour
{
    public float moveSpeed = 5f;

    [SerializeField] private Transform[] arrayOfTransforms;
    // Array to store patrol points
    private int currentPointIndex = 0; // Index of the current point

    //Kill player even when they are stationary
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                player.Die();
            }
        }
    }

    void Start()
    {
        // Accessing the first Transform in the array
        Transform first = arrayOfTransforms[0];

        if (arrayOfTransforms == null || arrayOfTransforms.Length == 0)
        {
            Debug.LogError("arrayOfTransforms is not set or empty!");
        }
        else
        {
            Debug.Log("arrayOfTransforms has " + arrayOfTransforms.Length + " elements.");
        }

        transform.position = arrayOfTransforms[0].position;


    }

    void Update()
    {
        // Check if array is not empty and index is valid
        if (arrayOfTransforms != null && arrayOfTransforms.Length > 0 && currentPointIndex >= 0 && currentPointIndex < arrayOfTransforms.Length)
        {
            // Move towards the current point
            Transform targetPoint = arrayOfTransforms[currentPointIndex];
            transform.position = Vector3.MoveTowards(transform.position, targetPoint.position, Time.deltaTime * moveSpeed);

            // Check if arrived at current point
            if (transform.position == targetPoint.position)
            {
                // Switch to the next point
                currentPointIndex = (currentPointIndex + 1) % arrayOfTransforms.Length;
            }
        }
    }
}
