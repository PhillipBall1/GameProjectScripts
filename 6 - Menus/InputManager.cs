using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;
using TMPro;

public class InputManager : MonoBehaviour
{
    public Transform changeBindTransform;
    Dictionary<string, KeyCode> actions;
    Dictionary<string, TMP_Text> buttonToLabel;
    public TMP_Text holder;

    private void Awake()
    {
        actions = new Dictionary<string, KeyCode>();
        buttonToLabel = new Dictionary<string, TMP_Text>();

        if (PlayerPrefs.GetInt("InputSaved") == 70000)
        {
            //Movement
            actions["Jump"] = (KeyCode)PlayerPrefs.GetInt("Jump");
            actions["Sprint"] = (KeyCode)PlayerPrefs.GetInt("Sprint");
            actions["Crouch"] = (KeyCode)PlayerPrefs.GetInt("Crouch");

            //Hotbar
            actions["Slot One"] = (KeyCode)PlayerPrefs.GetInt("Slot One");
            actions["Slot Two"] = (KeyCode)PlayerPrefs.GetInt("Slot Two");
            actions["Slot Three"] = (KeyCode)PlayerPrefs.GetInt("Slot Three");
            actions["Slot Four"] = (KeyCode)PlayerPrefs.GetInt("Slot Four");
            actions["Slot Five"] = (KeyCode)PlayerPrefs.GetInt("Slot Five");
            actions["Slot Six"] = (KeyCode)PlayerPrefs.GetInt("Slot Six");

            //InGameUI
            actions["Menu"] = (KeyCode)PlayerPrefs.GetInt("Menu");
            actions["Inventory"] = (KeyCode)PlayerPrefs.GetInt("Inventory");
            actions["Craft"] = (KeyCode)PlayerPrefs.GetInt("Craft");

            //InGameActions
            actions["Interact"] = (KeyCode)PlayerPrefs.GetInt("Interact");
            actions["Map"] = (KeyCode)PlayerPrefs.GetInt("Map");
        }
        else
        {
            //Movement
            actions["Jump"] = KeyCode.Space;
            actions["Sprint"] = KeyCode.LeftShift;
            actions["Crouch"] = KeyCode.LeftControl;

            //Hotbar
            actions["Slot One"] = KeyCode.Alpha1;
            actions["Slot Two"] = KeyCode.Alpha2;
            actions["Slot Three"] = KeyCode.Alpha3;
            actions["Slot Four"] = KeyCode.Alpha4;
            actions["Slot Five"] = KeyCode.Alpha5;
            actions["Slot Six"] = KeyCode.Alpha6;

            //InGameUI
            actions["Menu"] = KeyCode.Escape;
            actions["Inventory"] = KeyCode.Tab;
            actions["Craft"] = KeyCode.Q;

            //InGameActions
            actions["Interact"] = KeyCode.E;
            actions["Map"] = KeyCode.M;
        }
    }

    private void Start()
    {
        string[] buttonNames = GetButtonNames();
        GameObject[] buttons = GameObject.FindGameObjectsWithTag("SettingsButton");
        
        for(int i = 0; i < buttonNames.Length; i++)
        {
            string bn;
            bn = buttonNames[i];

            TMP_Text keyText = buttons[i].GetComponentInChildren<TMP_Text>();
            buttonToLabel[bn] = keyText;
            keyText.text = GetKeyNameButton(bn);
            //ReplaceKeyTexts(GetKeyNameButton(bn), keyText);

            Button keyButton = buttons[i].GetComponent<Button>();
            keyButton.onClick.AddListener(() => { StartRebind(bn); });
            keyButton.onClick.AddListener(() => { OpenChangeBindScreen(bn, keyText); });
           
        }
    }

    void Update()
    {
        if(buttonToRebind != null)
        {
            
            if (Input.anyKeyDown && !Input.GetMouseButtonDown(0) && !Input.GetMouseButtonDown(1))
            {
                foreach (KeyCode kc in Enum.GetValues(typeof(KeyCode)))
                {
                    if (Input.GetKeyDown(kc))
                    {
                        bool isBound = false;
                        
                        holder.SetText(kc.ToString());
                        for(int i = 0; i < buttonToLabel.Values.Count; i++)
                        {     
                            if (buttonToLabel.Values.ElementAt(i).text == holder.text)
                            {
                                isBound = true;
                                ChangeBindText();
                                break;
                            }
                        }
                        if (!isBound)
                        {
                            changeBindTransform.gameObject.SetActive(false);
                            SetButtonForKey(buttonToRebind, kc);
                            buttonToLabel[buttonToRebind].text = kc.ToString();
                            //ReplaceKeyTexts(kc.ToString(), buttonToLabel[buttonToRebind]);
                            buttonToRebind = null;
                            break;
                        }  
                    }
                }
            }
        }
    }

    public bool GetKeyDown(string actionName)
    {
        if (actions.ContainsKey(actionName) == false)
        {
            Debug.Log("InputManager::GetKeyDown -- No Button Named: " + actionName);
            return false;
        }
        return Input.GetKeyDown(actions[actionName]);
    }

    public bool GetKey(string actionName)
    {
        if (actions.ContainsKey(actionName) == false)
        {
            Debug.Log("InputManager::GetKeyDown -- No Button Named: " + actionName);
            return false;
        }
        return Input.GetKey(actions[actionName]);
    }

    public bool GetKeyUp(string actionName)
    {
        if (actions.ContainsKey(actionName) == false)
        {
            Debug.Log("InputManager::GetKeyUp -- No Button Named: " + actionName);
            return false;
        }
        return Input.GetKeyUp(actions[actionName]);
    }

    public string[] GetButtonNames()
    {
        return actions.Keys.ToArray();
    }

    public string GetKeyNameButton( string actionName)
    {
        if (actions.ContainsKey(actionName) == false)
        {
            Debug.Log("InputManager::GetKeyNameButton -- No Button Named: " + actionName);
            return "N/A";
        }
        return actions[actionName].ToString();
    }
    string buttonToRebind = null;
    public void StartRebind(string buttonName)
    {
        buttonToRebind = buttonName;
    }

    public void SetButtonForKey(string buttonName, KeyCode kc)
    {
        actions[buttonName] = kc;
    }

    public void OpenChangeBindScreen(string buttonName, TMP_Text keyName)
    {
        changeBindTransform.gameObject.SetActive(true);
        Transform firstChild = changeBindTransform.GetChild(1);
        Transform secondChild = changeBindTransform.GetChild(2);
        Transform thirdChild = changeBindTransform.GetChild(3);
        firstChild.GetComponent<TMP_Text>().text = "Currently Changing:   " + buttonName;
        secondChild.GetComponent<TMP_Text>().text = "Current Bind \n" + keyName.text;
        thirdChild.GetComponent<TMP_Text>().text = "Press Any Key \n To Confirm";
    }

    public void ChangeBindText()
    {
        changeBindTransform.gameObject.SetActive(true);
        Transform thirdChild = changeBindTransform.GetChild(3);
        thirdChild.GetComponent<TMP_Text>().text = "Bind Already Exists";
    }

    public void ReplaceKeyTexts(string mainContainer, TMP_Text textToChange)
    {
        if (mainContainer.Contains("Alpha"))
        {
            string ss = mainContainer.Substring(mainContainer.Length - 1);
            switch (ss)
            {
                case "1": textToChange.text = "1"; break;
                case "2": textToChange.text = "2"; break;
                case "3": textToChange.text = "3"; break;
                case "4": textToChange.text = "4"; break;
                case "5": textToChange.text = "5"; break;
                case "6": textToChange.text = "6"; break;
                case "7": textToChange.text = "7"; break;
                case "8": textToChange.text = "8"; break;
                case "9": textToChange.text = "9"; break;
                case "0": textToChange.text = "0"; break;
            }
        }
        if (mainContainer.Contains("Left"))
        {
            if (mainContainer.Length == 9)
            {
                textToChange.text = "Shift";
            }
            if (mainContainer.Length == 11)
            {
                textToChange.text = "Ctrl";
            }
        }
        if (mainContainer == "Escape")
        {
            textToChange.text = "Esc";
        }
        if (mainContainer.Contains("Right"))
        {
            if (mainContainer.Length == 10)
            {
                textToChange.text = "Shift";
            }
            if (mainContainer.Length == 12)
            {
                textToChange.text = "Ctrl";
            }
        }
    }

    public void SaveCurrentBinds()
    {
        PlayerPrefs.SetInt("InputSaved", 70000);
        PlayerPrefs.SetInt("Jump", (int)actions["Jump"]);
        PlayerPrefs.SetInt("Sprint", (int)actions["Sprint"]);
        PlayerPrefs.SetInt("Crouch", (int)actions["Crouch"]);

        PlayerPrefs.SetInt("Slot One", (int)actions["Slot One"]);
        PlayerPrefs.SetInt("Slot Two", (int)actions["Slot Two"]);
        PlayerPrefs.SetInt("Slot Three", (int)actions["Slot Three"]);
        PlayerPrefs.SetInt("Slot Four", (int)actions["Slot Four"]);
        PlayerPrefs.SetInt("Slot Five", (int)actions["Slot Five"]);
        PlayerPrefs.SetInt("Slot Six", (int)actions["Slot Six"]);

        PlayerPrefs.SetInt("Menu", (int)actions["Menu"]);
        PlayerPrefs.SetInt("Inventory", (int)actions["Inventory"]);
        PlayerPrefs.SetInt("Craft", (int)actions["Craft"]);

        PlayerPrefs.SetInt("Interact", (int)actions["Interact"]);
        PlayerPrefs.SetInt("Map", (int)actions["Map"]);
    }
}
