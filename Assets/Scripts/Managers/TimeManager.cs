using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TimeManager : SingleTon<TimeManager>
{
    public TextMeshProUGUI TimeText;
    public TextMeshProUGUI DayText;
    public System.DateTime gameStartDate = new System.DateTime(2021, 1, 1);
    private float timeScale = 20f; // 1실제초 = 60게임분

    public float Time_M;
    public float Time_H;

    public bool IsNight;

    public float StartTime;

    private void Start() {
        gameStartDate = gameStartDate.AddHours(6f);
        gameStartDate = gameStartDate.AddMinutes(0f);

        StartTime = Time.time;
    }
    
    void Update()
    {
        if(SceneManager.GetActiveScene().name == "GamePlayScene") {
            gameStartDate = gameStartDate.AddMinutes(Time.deltaTime * timeScale);
            TimeText.text = gameStartDate.ToString("HH:mm");
            DayText.text = gameStartDate.ToString("dd") + " DAY";
        }
    }

    public bool IsNightTime()
    {
        // 예시: 밤 시간을 18시부터 6시로 가정
        return gameStartDate.Hour >= 18 || gameStartDate.Hour < 6;
    }

    public bool IsDayTime()
    {
        // 예시: 낮 시간을 6시부터 18시로 가정
        return gameStartDate.Hour >= 6 && gameStartDate.Hour < 18;
    }

    public string ElapsedTime() {
        float EndTime = Time.time - StartTime;

        string timetext = "";

        int m_count = 0;
        int s_count = 0;

        m_count = (int)EndTime / 60;
        s_count = (int)EndTime % 60;

        timetext = $"{m_count}분  {s_count}초";

        return timetext;
    }
}
