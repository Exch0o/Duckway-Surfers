using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeController : MonoBehaviour
{
    public float timeMultiplier;

    public float startHour;

    public DateTime currentTime;

    public Light sunLight;

    public float sunRiseHour;
    public float sunSetHour;

    TimeSpan sunRiseTime;
    TimeSpan sunSetTime;

    public TextMeshProUGUI timeText;
    // Start is called before the first frame update
    void Start()
    {
        currentTime = DateTime.Now.Date + TimeSpan.FromHours(startHour);
        sunRiseTime = TimeSpan.FromHours(sunRiseHour);
        sunSetTime = TimeSpan.FromHours(sunSetHour);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTimeOfDay();
    }

    void UpdateTimeOfDay()
    {
        currentTime = currentTime.AddSeconds(Time.deltaTime * timeMultiplier);
        if(timeText != null)
        {
            timeText.text = currentTime.ToString("HH:mm");
        }
    }

    void RotateSun()
    {
        float sunLightRotation;

        if(currentTime.TimeOfDay > sunRiseTime && currentTime.TimeOfDay < sunSetTime)
        {
            TimeSpan sunriseToSunsetDuration = CalculateTimeDifference(sunRiseTime, sunSetTime);

        }
    }

    TimeSpan CalculateTimeDifference(TimeSpan fromTime, TimeSpan toTime)
    {
        TimeSpan difference = toTime - fromTime;

        if(difference.TotalSeconds < 0)
        {
            difference += TimeSpan.FromHours(24);
        }
        return difference;
    }
}
