using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;



public class MainMenu : MonoBehaviour
{
    public GameObject loadingInterface;
    public GameObject menu;

    public Camera menuCam;
    private bool left;
    private bool right;
    private bool once;
    private float g;
    private float dotCount;

    List<AsyncOperation> scenesToLoad = new List<AsyncOperation>();
    public TMP_Text loadText;
    public TMP_Text progressText;
    public Image loadingProgressBar;

    public static bool mainMenuOpen;

    public static bool newGame;

    public void PlayPressed()
    {
        HideMenu();
        once = true;
        newGame = true;
        mainMenuOpen = false;
        scenesToLoad.Add(SceneManager.LoadSceneAsync(1, LoadSceneMode.Single));
        StartCoroutine(LoadingScreen());
    }

    public void LoadPressed()
    {
        if (PlayerPrefs.GetInt("Saved") == 1)
        {
            HideMenu();
            once = true;
            newGame = false;
            mainMenuOpen = false;
            scenesToLoad.Add(SceneManager.LoadSceneAsync(1, LoadSceneMode.Single));
            StartCoroutine(LoadingScreen());
        }
        else
        {
            //Display nothing saved UI Panel
        }
    }

    public void HideMenu()
    {
        menu.SetActive(false);
        loadingInterface.SetActive(true);
    }


    // Start is called before the first frame update
    void Start()
    {
        newGame = false;
        loadingInterface.SetActive(false);
        menu.SetActive(true);
        dotCount = 0;
        loadText.text = "Loading Scene";
        mainMenuOpen = true;
        right = true;
        menuCam.transform.position = new Vector3(-555.8f, 26.51f, -84.1f);
        g = menuCam.transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        Cursor.lockState = CursorLockMode.None;
        
        if(dotCount == 4)
        {
            loadText.text = "Loading";
            dotCount = 0;
        }

        if (once)
        {
            StartCoroutine(LoadTextChange());
            once = false;
        }
        if (g < -555)
        {
            right = true;
            left = false;
        }
        if (g > -732)
        {
            left = true;
            right = false;
        }
        if (right)
        {
            g += 1 * Time.deltaTime;
        }
        if (left)
        {
            g -= 1 * Time.deltaTime;
        }
        menuCam.transform.position = new Vector3(g, 26.51f, -84.1f);
    }

    private IEnumerator LoadingScreen()
    {
        scenesToLoad[0].allowSceneActivation = false;
        while (!scenesToLoad[0].isDone)
        {
            float totalProgress = Mathf.Clamp01(scenesToLoad[0].progress / 0.9f);
            int g = Mathf.RoundToInt(totalProgress);
            loadingProgressBar.fillAmount = g;
            progressText.text = (g * 100f) + "%";
            if(totalProgress >= 0.9f)
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
                if(dotCount < 3)
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

    public void ExitGame()
    {
        Application.Quit();
    }
}
