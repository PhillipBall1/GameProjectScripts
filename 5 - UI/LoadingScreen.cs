using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoadingScreen : MonoBehaviour
{
    public SpawnAnimals animals;
    public NodeSpawner nodes;
    public TreeSpawner trees;
    public TMP_Text loadingText;
    public TMP_Text progressText;
    public Transform loadingScreen;
    public Image fillAmount;

    public static bool done = false;

    private float loadingValueTogether;
    private float loadingValueCurrent;

    // Start is called before the first frame update
    void Start()
    {
        loadingText.text = "Spawning Trees";
        //IF NEW GAME
        loadingScreen.GetComponent<CanvasGroup>().alpha = 1;
        StartCoroutine(CheckDone());
        StartCoroutine(ClosePanel());
    }

    // Update is called once per frame
    void Update()
    {
        if (!TreeSpawner.treesDone)
        {
            loadingValueTogether = trees.treePooler2.poolAmount + 
                                   trees.treePooler3.poolAmount + 
                                   trees.treePooler4.poolAmount + 
                                   trees.treePooler5.poolAmount +
                                   trees.treePooler6.poolAmount +
                                   trees.treePooler7.poolAmount;

            loadingValueCurrent = trees.treeAmount2 + 
                                  trees.treeAmount3 + 
                                  trees.treeAmount4 + 
                                  trees.treeAmount5 + 
                                  trees.treeAmount6 + 
                                  trees.treeAmount7;
        }
        else if (!NodeSpawner.nodesDone && TreeSpawner.treesDone)
        {
            loadingValueTogether = nodes.stonePooler.poolAmount + 
                                   nodes.metalPooler.poolAmount;

            loadingValueCurrent = nodes.metalAmount + 
                                  nodes.stoneAmount;
        }
        else if (!SpawnAnimals.animalsDone && TreeSpawner.treesDone && NodeSpawner.nodesDone)
        {
            loadingValueTogether = animals.bearPoolerrr.poolAmount +
                                   animals.wolf1Poolerr.poolAmount +
                                   animals.wolf2Poolerr.poolAmount +
                                   animals.boarPoolerrr.poolAmount +
                                   animals.deerMPoolerr.poolAmount +
                                   animals.deerFPoolerr.poolAmount +
                                   animals.cow1Poolerrr.poolAmount +
                                   animals.cow2Poolerrr.poolAmount +
                                   animals.cow3Poolerrr.poolAmount +
                                   animals.rabbitPooler.poolAmount +
                                   animals.sheepPoolerr.poolAmount +
                                   animals.foxPoolerrrr.poolAmount;

            loadingValueCurrent = animals.bearAmounttt +
                                  animals.wolf1Amountt +
                                  animals.wolf2Amountt +
                                  animals.boarAmounttt +
                                  animals.deerMAmountt +
                                  animals.deerFAmountt +
                                  animals.cow1Amounttt +
                                  animals.cow2Amounttt +
                                  animals.cow3Amounttt +
                                  animals.rabbitAmount +
                                  animals.sheepAmountt +
                                  animals.foxAmountttt; 
        }


        float calcPercent = loadingValueCurrent / loadingValueTogether;
        fillAmount.fillAmount = calcPercent;
        progressText.text = Mathf.RoundToInt(calcPercent * 100).ToString() + "%";
        if (!done)
        {
            loadingScreen.gameObject.SetActive(true);
        }
    }

    private IEnumerator CheckDone()
    {
        while (true)
        {
            yield return new WaitForSeconds(2f);
            if (NodeSpawner.nodesDone && TreeSpawner.treesDone && SpawnAnimals.animalsDone)
            {
                done = true;
                break;
            }
        }
    }

    private IEnumerator ClosePanel()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.2f);
            if (done)
            {
                loadingScreen.GetComponent<CanvasGroup>().alpha = 0;
                break;
            }
            else
            {
                if (!TreeSpawner.treesDone)
                {
                    loadingText.text = "Spawning Trees";
                }
                else if (!NodeSpawner.nodesDone && TreeSpawner.treesDone && !SpawnAnimals.animalsDone)
                {
                    loadingText.text = "Spawning Nodes";
                } 
                else if (!SpawnAnimals.animalsDone && TreeSpawner.treesDone && NodeSpawner.nodesDone)
                {
                    loadingText.text = "Spawning Animals";
                }    
            }
        }
    }
}
