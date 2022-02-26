using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotTemplateButton : MonoBehaviour
{
    public void Clicked()
    {
        GameObject[] slotPanels = GameObject.FindGameObjectsWithTag("SlotPanelActive");
        foreach(GameObject slotPanel in slotPanels)
        {
            slotPanel.gameObject.SetActive(false);
        }
        if (Input.GetMouseButton(0))
        {
            
            //popup info !
        }
        else if (Input.GetMouseButton(1))
        {

        }

    }
}
