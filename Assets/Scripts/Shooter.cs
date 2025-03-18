using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    [Header("References")]
    public Transform spawnPoint;
    public GameObject projectilePrefab;

    [Header("Stats")]
    [Tooltip("Time, in seconds, between the firing of each projectile.")]
    public float fireRate = 1;
    private float lastFireTime = 0;

    void Update()
    {
        if (Time.time >= lastFireTime + fireRate)
        {
            lastFireTime = Time.time;
            Instantiate(projectilePrefab, spawnPoint.position, spawnPoint.rotation);
        }
    }
}

