using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("References")]
    public Transform trans;  // 🚨 这里的 trans 可能为空，建议直接使用 transform

    [Header("Stats")]
    [Tooltip("How many units the projectile will move forward per second.")]
    public float speed = 34;
    [Tooltip("The distance the projectile will travel before it comes to a stop.")]
    public float range = 70;

    private Vector3 spawnPoint;

    // === 添加的新变量 ===
    [Header("Direction")]
    [Tooltip("The direction the projectile will travel.")]
    public Vector3 moveDirection = Vector3.forward; // 默认向前方向(Z轴)

    void Start()
    {
        spawnPoint = transform.position;  // ✅ 改为 transform
    }

    void Update()
    {
        // === 修改的移动逻辑 ===
        // 原代码: trans.Translate(0, 0, speed * Time.deltaTime, Space.Self);
        trans.Translate(moveDirection.normalized * speed * Time.deltaTime, Space.World);

        // 销毁超出范围的投射物
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
