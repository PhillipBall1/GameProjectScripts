using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FloatingText : MonoBehaviour
{
    private void Update()
    {
        float textFadeTime = 4f;
        transform.GetComponent<Image>().CrossFadeAlpha(0.1f, textFadeTime, false);
        transform.GetComponent<Image>().CrossFadeColor(Color.black, textFadeTime, false, false);
        transform.GetComponentInChildren<TMP_Text>().CrossFadeAlpha(0.1f, textFadeTime, false);
        transform.GetComponentInChildren<TMP_Text>().CrossFadeColor(Color.black, textFadeTime, false, false);
    }
}
