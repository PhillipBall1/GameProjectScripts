using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSetFalse : MonoBehaviour
{
    public float secondsToSetFalse;

    private void Update()
    {
        if (transform.gameObject.activeInHierarchy)
        {
            float counter = 0;
            float waitTime = secondsToSetFalse;
            if (counter < waitTime)
            {
                counter += Time.deltaTime;
                if (counter >= waitTime)
                {
                    gameObject.SetActive(false);
                }
            }
        }
    }
}
