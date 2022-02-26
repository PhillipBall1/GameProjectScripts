using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShowFPS : MonoBehaviour
{
    public TMP_Text fpsText;
    private float deltaTime;
    void Update()
    {
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
        float fps = 1.0f / deltaTime;
        if(fpsText != null)
        {
            fpsText.text = Mathf.Ceil(fps).ToString();
        }
    }
}
