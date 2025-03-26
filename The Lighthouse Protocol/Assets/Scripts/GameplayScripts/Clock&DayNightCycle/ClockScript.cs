using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ClockScript : MonoBehaviour
{
    [Header("Clock UI")]

    [SerializeField] private TMP_Text clockText;
    private float elapsedTime;

    [Header("Hours in a day on the planet")]
    [SerializeField] private float timeInADay = 86400f;
    [Header("how fast time goes")]
    [SerializeField] private float timeScale = 2.0f;

    [Header("Alarms")]
    [SerializeField] private float playSoundAtTime;//when in the timer sound needs to be played
    [SerializeField] private AudioSource AlarmSound;
    // Start is called before the first frame update
    void Start()
    {
        elapsedTime = 12 * 3600f;
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime * timeScale;
        elapsedTime %= timeInADay;

        UpdateClockUI();
    }

    void UpdateClockUI()
    {
        int hours = Mathf.FloorToInt(elapsedTime / 3600f);
        int minutes = Mathf.FloorToInt((elapsedTime - hours * 3600f) / 60f);
        int seconds = Mathf.FloorToInt((elapsedTime - hours * 3600f) - minutes *60f);

        string ClockString = string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);
        clockText.text = ClockString;
    }


}
