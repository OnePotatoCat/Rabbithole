using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverCanvas : MonoBehaviour
{
    [SerializeField] CanvasGroup gameOverCanvas;

    public void Retry()
    {
        SceneManager.LoadScene(1);
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        Destroy(SavedKid.instance.gameObject);
        SceneManager.LoadScene(0);
    }
}
