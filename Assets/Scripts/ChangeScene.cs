using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    private int scenes, currentScene;
    void Start() {
        scenes = SceneManager.sceneCountInBuildSettings;
        currentScene = SceneManager.GetActiveScene().buildIndex;
    }

    private void OnCollisionEnter2D(Collision2D collision){
        if (collision.gameObject.tag == "Player")
            Debug.Log(currentScene + 1);
            SceneManager.LoadScene(currentScene + 1, LoadSceneMode.Single);
    }
}
