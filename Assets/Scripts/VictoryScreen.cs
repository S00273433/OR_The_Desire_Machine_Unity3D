
using UnityEngine;
using UnityEngine.SceneManagement;


public class VictoryScreen : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
   public void PlayAgain()
    {
        SceneManager.LoadSceneAsync("PlayScene");
    }
}
