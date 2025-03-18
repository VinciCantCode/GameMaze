using UnityEngine;
using UnityEngine.SceneManagement; // 需要导入SceneManagement

public class Goal : MonoBehaviour
{
    [SerializeField] private string nextSceneName;

    public void OnTriggerEnter(Collider other) // 需要public
    {
        if (other.gameObject.layer == 8) // 确保你的Player在Layer 8
        {
            Debug.Log("Goal Reached! Loading next scene...");
            SceneManager.LoadScene(nextSceneName);
        }
    }

}