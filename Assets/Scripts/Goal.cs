using UnityEngine;
using UnityEngine.SceneManagement; 

public class Goal : MonoBehaviour
{
    [SerializeField] private string nextSceneName;

    public void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.layer == 8) // Make sure Player is on Layer 8
        {
            Debug.Log("Goal Reached! Loading next scene...");
            SceneManager.LoadScene(nextSceneName);
        }
    }
}