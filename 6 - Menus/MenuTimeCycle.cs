using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using TMPro;

public class MenuTimeCycle : MonoBehaviour
{
    [Range(0, 24)]
    public float timeOfDay;
    public float orbitSpeed = 1;
    public Light sun;
    public Light moon;
    public Volume skyVolume;

    private bool isNight;
    private bool once;
    private bool once1;

    private PhysicallyBasedSky sky;

    private float day;

    private float minutes;
    private void Start()
    {
        once = false;
        once1 = false;
        skyVolume.profile.TryGet(out sky);
        timeOfDay = 7.4f;
    }

    private void OnValidate()
    {
        skyVolume.profile.TryGet(out sky);
        UpdateTime();
    }

    private void Update()
    {
        timeOfDay += Time.deltaTime * orbitSpeed;

        if (timeOfDay > 24) timeOfDay = 0;

        UpdateTime();
    }

    private void UpdateTime()
    {
        float alpha = timeOfDay / 24f;
        float sunRot = Mathf.Lerp(-90, 270, alpha);
        float moonRot = sunRot - 180;
        sun.transform.rotation = Quaternion.Euler(sunRot, 0, 0);
        moon.transform.rotation = Quaternion.Euler(moonRot, 0, 0);

        CheckNightOrDay();
    }

    private void CheckNightOrDay()
    {
        if (isNight)
        {
            if (moon.transform.rotation.eulerAngles.x > 180)
            {
                StartDay();
                sky.spaceEmissionMultiplier.value = 0;
            }
        }
        if (!isNight)
        {
            if (sun.transform.rotation.eulerAngles.x > 180)
            {
                StartNight();
                sky.spaceEmissionMultiplier.value = 0.4f;
            }
        }
    }

    private void StartDay()
    {
        isNight = false;
        sun.shadows = LightShadows.Soft;
        moon.shadows = LightShadows.None;
    }
    private void StartNight()
    {
        isNight = true;
        sun.shadows = LightShadows.None;
        moon.shadows = LightShadows.Soft;
    }
}
