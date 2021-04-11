using UnityEngine;
using UnityEngine.SceneManagement;

public class StartOver : MonoBehaviour
{
    public GameObject gameCanvas;
    public GameObject gameOverCanvas;

    public void RestartGame()
    {
        gameOverCanvas.SetActive(false); 
        TheGameManager.timeLeft = TheGameManager.maxTime;
        gameCanvas.SetActive(true);
    }

}
