using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapPointLogic : MonoBehaviour
{
    private LayerMask buildingLayer;
    private bool filled;
    // Start is called before the first frame update
    void Start()
    {
        buildingLayer = LayerMask.GetMask("Building");
    }

    // Update is called once per frame
    void Update()
    {
        filled = Physics.CheckSphere(transform.position, 0.1f, buildingLayer);

        if (filled)
        {
            GetComponent<SphereCollider>().enabled = false;
        }
        else
        {
            GetComponent<SphereCollider>().enabled = true;
        }
    }
}
