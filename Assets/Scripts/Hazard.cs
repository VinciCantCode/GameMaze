using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazard : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))  // Check if colliding with Player layer
        {
            Player player = other.GetComponent<Player>();  // Get Player component
            if (player != null)
            {
                player.Die();
            }
        }
    }
}
