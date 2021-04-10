using UnityEngine;
using UnityEngine.UI;
using System;

public class TheGameManager : MonoBehaviour
{
    public ThinkGear thinkgear;

    private float medLevel;
    private const float medBarrier = 40f;
    private float attLevel;
    private const float attBarrier = 40f;
    private const float scalar = 100f;
    private bool specialOn = false;
    private float timer = 0f;
    private float bestScoreSec = 0f;
    private const float maxYRange = 4.4f;
    private const float startYPos = -2.8f;
    private int timeS = 0;

    public Image timerBar;
    public static float maxTime = 120f;
    public static float timeLeft;

    public GameObject medCube;
    public GameObject attCube;

    public Text specialTimer;
    public Text onFireText;
    public Text bestScoreText;
    public Text medLevelText;
    public Text attLevelText;

    public Canvas gameCanvas;
    public Canvas gameOverCanvas;

    public AudioSource specialSound;


    // Start is called before the first frame update
    void Start()
    {
        SetVariables();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTimer();
        checkSpecials();
        UpdatePositions();
        UpdateTexts();
    }

    private void UpdateMeditation(int value)
    {
        medLevel = value;
    }

    private void UpdateAttention(int value)
    {
        attLevel = value;
    }

    private void SetVariables()
    {
        thinkgear = GameObject.Find("ThinkGear").GetComponent<ThinkGear>();
        thinkgear.UpdateConnectedStateEvent += () => { thinkgear.StartMonitoring(); Debug.Log("Sensor on"); };
        thinkgear.UpdateMeditationEvent += UpdateMeditation;
        thinkgear.UpdateAttentionEvent += UpdateAttention;

        timeLeft = maxTime;

        bestScoreSec = PlayerPrefs.GetInt("bestScore");

        bestScoreText.text = string.Format("Your best score: {0:00}:{1:00}", Mathf.FloorToInt(bestScoreSec / 60), Mathf.FloorToInt(bestScoreSec % 60));
    }

    private void checkSpecials()
    {
        if (medLevel >= medBarrier && attLevel >= attBarrier)
        {
            specialOn = true;
            SpecialSituation();
        }

        if ((medLevel < medBarrier || attLevel < attBarrier) && specialOn == true)
        {
            specialOn = false;
            SpecialSituationOff();
        }
    }

    private void SpecialSituation()
    {
        medCube.transform.Rotate(new Vector3(1f, 1f, 1f) * scalar * Time.deltaTime);
        attCube.transform.Rotate(new Vector3(1f, 1f, 1f) * scalar * Time.deltaTime);
        onFireText.gameObject.SetActive(true);
        specialTimer.gameObject.SetActive(true);
        timer += Time.deltaTime;
        var ts = TimeSpan.FromSeconds(timer);
        float minutes = Mathf.FloorToInt(timer / 60);
        float seconds = Mathf.FloorToInt(timer % 60);
        specialTimer.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        if (timeS < ts.Seconds)
        {
            timeS = ts.Seconds;
            specialSound.Play();
        }
        if (bestScoreSec < timer)
        {
            bestScoreSec = timer;
            bestScoreText.text = string.Format("Your best score: {0:00}:{1:00}", Mathf.FloorToInt(bestScoreSec / 60), Mathf.FloorToInt(bestScoreSec % 60));
        }
    }

    private void SpecialSituationOff()
    {
        if (PlayerPrefs.GetInt("bestScore") < bestScoreSec)
            PlayerPrefs.SetInt("bestScore", (int)bestScoreSec);
        onFireText.gameObject.SetActive(false);
        specialTimer.gameObject.SetActive(false);
        timer = 0;
        timeS = 0;
    }

    private void UpdatePositions()
    {
        float medY = (float)(((medLevel / medBarrier) * maxYRange) + startYPos);
        float attY = (float)(((attLevel / attBarrier) * maxYRange) + startYPos);

        if (medLevel < medBarrier)
            medCube.transform.position = new Vector3((float)1.5, medY, 0);
        else
            medCube.transform.position = new Vector3((float)1.5, (float)1.6, 0);

        if (attLevel < attBarrier)
            attCube.transform.position = new Vector3((float)-1.5, attY, 0);
        else
            attCube.transform.position = new Vector3((float)-1.5, (float)1.6, 0);
    }

    private void UpdateTexts()
    {
        medLevelText.text = medLevel.ToString();
        attLevelText.text = attLevel.ToString();
    }

    private void UpdateTimer()
    {
        if (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            timerBar.fillAmount = timeLeft / maxTime;
        }
        
        else
        {
            gameCanvas.gameObject.SetActive(false);
            gameOverCanvas.gameObject.SetActive(true);
        } 
    }



}
