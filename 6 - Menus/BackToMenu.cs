using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class BackToMenu : MonoBehaviour
{
    public GameObject loadingInterface;

    List<AsyncOperation> scenesToLoad = new List<AsyncOperation>();
    public TMP_Text loadText;
    public TMP_Text progressText;
    public Image loadingProgressBar;

    private bool once;
    private int dotCount;

    public void ButtonPressed()
    {
        HideMenu();
        once = true;
        scenesToLoad.Add(SceneManager.LoadSceneAsync(0, LoadSceneMode.Single));
        StartCoroutine(LoadingScreen());
    }

    private void HideMenu()
    {
        loadingInterface.SetActive(true);
    }
    // Start is called before the first frame update
    void Start()
    {
        loadingInterface.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (dotCount == 4)
        {
            loadText.text = "Loading";
            dotCount = 0;
        }

        if (once)
        {
            StartCoroutine(LoadTextChange());
            once = false;
        }
    }

    private IEnumerator LoadingScreen()
    {
        while (!scenesToLoad[0].isDone)
        {
            scenesToLoad[0].allowSceneActivation = false;
            float totalProgress = Mathf.Clamp01(scenesToLoad[0].progress / 0.9f);
            int g = Mathf.RoundToInt(totalProgress);
            loadingProgressBar.fillAmount = g;
            progressText.text = (g * 100f) + "%";
            if (totalProgress > 0.9f)
            {
                scenesToLoad[0].allowSceneActivation = true;
            }
            yield return null;
        }
    }

    private IEnumerator LoadTextChange()
    {
        float counter = 0;
        float waitTime = 0.3f;

        while (counter < waitTime)
        {
            counter += Time.deltaTime;
            if (counter >= waitTime)
            {
                if (dotCount < 3)
                {
                    loadText.text += ".";
                }
                dotCount++;
                once = true;
                yield break;
            }
            yield return null;
        }
    }
}
