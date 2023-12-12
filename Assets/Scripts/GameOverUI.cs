#pragma warning disable 0649

using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    public void Quit()
    {
        Application.Quit();
    }
    public void Retry()
    {
        int index = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(index);
    }
}
