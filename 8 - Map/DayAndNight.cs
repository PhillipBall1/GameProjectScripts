using System.Collections;
using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using TMPro;

public class DayAndNight : MonoBehaviour
{
    //Save These
    [Range(1, 24)]
    public float timeOfDay;

    public static int dayNumber;
    //------------------------------

    public Light sunLight;
    public Light moonLight;
    public Volume skyVolume;
    private PhysicallyBasedSky sky;
    public AnimationCurve starsCurve;

    public TMP_Text dayText;
    public TMP_Text timeText;
    public float orbitSpeed = 1;


    private bool once;
    private bool once1;
    public bool isNight;

    private float day;
    private int minutes;

    private float g = 0;

    private void Start()
    {
        skyVolume.profile.TryGet(out sky);
        if (MainMenu.newGame)
        {
            dayNumber = 0;
            timeOfDay = 5.8f;
        }
        else
        {
            //dayNumber = PlayerPrefs.GetInt("CurrentDay");
            //timeOfDay = PlayerPrefs.GetFloat("CurrentTime");
            dayNumber = 0;
            timeOfDay = 5.8f;
        }
        once = false;
        once1 = false;
        dayText.alpha = 0;
    }

    private void OnValidate()
    {
        skyVolume.profile.TryGet(out sky);
        UpdateTime();
    }

    private void Update()
    {
        if (LoadingScreen.done)
        {
            CalcTime();
        }
    }

    private void CalcTime()
    {
        timeOfDay += Time.deltaTime * orbitSpeed;

        UpdateTime();

        if (timeOfDay > 24) timeOfDay = 0;

        //Increase Alpha
        if (once) dayText.alpha += 0.004f;

        if (dayText.alpha >= 1.2f)
        {
            once = false;
            once1 = true;
        }

        //Decrease Alpha
        if (once1) dayText.alpha -= 0.005f;

        if (dayText.alpha <= 0) once1 = false;

        if (timeOfDay >= 5.995 && timeOfDay <= 6.005)
        {
            if (!once)
            {
                //Play a cool short theme
                dayNumber += 1;
                dayText.text = "Day " + dayNumber.ToString();
                once = true;
            }
        }

        day = timeOfDay / 24;
        float dayNormalized = day % 1;

        float hoursPerDay = 24f;
        float minutePerHour = 60f;

        float f = Mathf.Floor(timeOfDay);

        minutes = (int)Mathf.Floor(((dayNormalized * hoursPerDay) % 1) * minutePerHour);

        if (f > 12)
        {
            int g = (int)f - 12;
            timeText.text = g + ":" + minutes.ToString("00") + " pm";
        }
        else
        {
            timeText.text = f + ":" + minutes.ToString("00") + " am";
        }
    }

    private void UpdateTime()
    {
        float alpha = timeOfDay / 24f;
        float sunRot = Mathf.Lerp(-90, 270, alpha);
        float moonRot = sunRot - 180;
        sky.spaceEmissionMultiplier.value = starsCurve.Evaluate(alpha);
        g += 0.01f;
        if(g >= 360)
        {
            g = 0;
        }
        Vector3 f = new Vector3(g, 0, 0);
        sky.spaceRotation = new Vector3Parameter(f);
        sunLight.transform.rotation = Quaternion.Euler(sunRot, 0, 0);
        moonLight.transform.rotation = Quaternion.Euler(moonRot, 0, 0);
        CheckNightOrDay();
    }

    private void CheckNightOrDay()
    {
        if (isNight)
        {
            if(moonLight.transform.eulerAngles.x > 180)
            {
                StartDay();
            }
        }
        else
        {
            if (sunLight.transform.eulerAngles.x > 180)
            {
                StartNight();
            }
        }
    }

    private void StartDay()
    {
        isNight = false;
        sunLight.shadows = LightShadows.Hard;
        moonLight.shadows = LightShadows.None;
  
    }
    private void StartNight()
    {
        isNight = true;
        sunLight.shadows = LightShadows.None;
        moonLight.shadows = LightShadows.Hard;
    }
}