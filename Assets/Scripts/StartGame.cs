using UnityEngine;

public class StartGame : MonoBehaviour
{
    private GameObject startCanvas;
    private GameObject gameCanvas;
    private GameObject gameOverCanvas;

    // Start is called before the first frame update
    void Start()
    {
        startCanvas = GameObject.Find("StartCanvas");
        gameCanvas = GameObject.Find("GameCanvas");
        gameOverCanvas = GameObject.Find("NewCanvas");
        gameCanvas.SetActive(false);
        gameOverCanvas.SetActive(false);
    }

    public void onClick()
    {
        startCanvas.SetActive(false);
        gameCanvas.SetActive(true);
    }
}
