using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildSystem : MonoBehaviour
{
    private Terrain terrain;
    private Collider terrainCollider;
    public Camera cam;
    public LayerMask layer;
    public LayerMask groundMask;
    private GameObject previewGameObject = null;
    private Preview previewScript = null;

    public float stickTolerance = 0.5f;
    public Transform player;
    public bool isBuilding = false;
    private bool pauseBuilding = false;
    private bool deployable;
    private Player players;
    private bool once;
    private float g = 0;

    private void Start()
    {
        g = 0;
        players = FindObjectOfType<Player>();
        once = true;
    }
    void Update()
    {
        terrain = GetClosestCurrentTerrain(player.position);
        terrainCollider = terrain.GetComponent<TerrainCollider>();

        if (previewGameObject != null)
        {
            if (previewScript.isDeployable)
            {
                deployable = true;
            }
            else
            {
                deployable = false;
            }
        }

        if(previewGameObject != null)
        {
            if (Input.GetKeyDown(KeyCode.R))//Rotate
            {
                g += 90;
                previewGameObject.transform.rotation = Quaternion.Euler(0, g, 0);
            }
            if (Input.GetMouseButtonDown(1) && !deployable)//Cancel Non Deployable
            {
                CancelBuild();
            }
            if (Input.GetKeyDown(KeyCode.Tab) && !deployable)
            {
                CancelBuild();
            }

            if (Input.GetKeyDown(KeyCode.Tab) && deployable)//Cancel Deployable
            {
                players.deployableEquipped = false;
                CancelBuild();
            }
            if (Input.GetMouseButtonDown(1) && deployable)
            {
                players.deployableEquipped = false;
                CancelBuild();
            }

            if (Input.GetMouseButtonDown(0) && isBuilding)//Build
            {
                if (previewScript.GetSnapped() && !deployable)
                {
                    PlaceBuild();
                }
                if (previewScript.GetSnapped() && deployable)
                {
                    PlaceDeployable();
                }
                else
                {
                    //Not Snapped
                }
            }
        }

        if (isBuilding)
        {
            if (pauseBuilding)
            {
                float mouseX = Input.GetAxis("Mouse X");
                float mouseY = Input.GetAxis("Mouse Y");
                if (Mathf.Abs(mouseX) >= stickTolerance || Mathf.Abs(mouseY) >= stickTolerance)
                {
                    pauseBuilding = false;
                }
            }
            else
            {
                DoBuildRay();
            }
        }
    }

    public void NewBuild(GameObject build)
    {
        previewGameObject = Instantiate(build, new Vector3(0,-5000,0), Quaternion.identity);
        previewScript = previewGameObject.GetComponent<Preview>();
        isBuilding = true;
    }

    private void CancelBuild()
    {
        Destroy(previewGameObject);
        previewGameObject = null;
        previewScript = null;
        isBuilding = false;
    }

    private void PlaceBuild()
    {
        previewScript.Place();
    }
    private void PlaceDeployable()
    {
        previewScript.Place();
        previewGameObject = null;
        previewScript = null;
        isBuilding = false;
    }

    public void PauseBuild(bool value)
    {
        pauseBuilding = value;
    }
    private void DoBuildRay()
    {
        if(previewGameObject != null)
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 2.5f;
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePos);

            if (previewScript.isFreeDeployable)
            {
                RaycastHit hit;
                RaycastHit hit2;
                var ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));


                if(Physics.Raycast(ray, out hit, 2.5f, groundMask))
                {
                    if(hit.collider.tag != "Terrain" && hit.collider.tag != "Player")
                    {
                        previewGameObject.transform.position = hit.point;
                        previewGameObject.transform.rotation = Quaternion.Euler(0, g, 0);
                    }
                    else
                    {
                        previewGameObject.transform.position = worldPosition;
                    }
                }
                else if (terrainCollider.Raycast(ray, out hit2, 2.5f))
                {
                    previewGameObject.transform.position = hit2.point;
                    previewGameObject.transform.rotation = Quaternion.FromToRotation(transform.up, hit2.normal) * transform.rotation;
                }
                else
                {
                    previewGameObject.transform.position = worldPosition;
                }
            }
            else
            {
                previewGameObject.transform.position = worldPosition;
            }
        }
    }


    Terrain GetClosestCurrentTerrain(Vector3 playerPos)
    {
        Terrain[] terrains = Terrain.activeTerrains;

        if (terrains.Length == 0)
            return null;

        if (terrains.Length == 1)
            return terrains[0];

        float lowDist = (terrains[0].GetPosition() - playerPos).sqrMagnitude;
        var terrainIndex = 0;

        for (int i = 1; i < terrains.Length; i++)
        {
            Terrain terrain = terrains[i];
            Vector3 terrainPos = terrain.GetPosition();

            var dist = (terrainPos - playerPos).sqrMagnitude;
            if (dist < lowDist)
            {
                lowDist = dist;
                terrainIndex = i;
            }
        }
        return terrains[terrainIndex];
    }
}
/*
 else if (terrainCollider.Raycast(ray, out hit2, 2.5f))
                {
                    
                }
 */