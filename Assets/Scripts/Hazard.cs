using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazard : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))  // ✅ 判断 Player 层
        {
            Player player = other.GetComponent<Player>();  // ✅ 声明 player 变量
            if (player != null)
            {
                player.Die();
            }
        }
    }
}
