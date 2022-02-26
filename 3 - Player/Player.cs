using System.Collections;
using System.Collections.Generic;
using UnityEngine.Animations.Rigging;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;

public class Player : MonoBehaviour
{

    #region Variables
    //Public Variables***********************************************
    public Camera cam;
    public PlayerCraft playerCraft;
    public Transform craftingParent;
    public Transform itemHolder;
    public ItemDatabaseObject ioDB;
    public TMP_Text middleWorldText;

    [Header("Building Transforms")]
    public Transform buildMenu;
    public Transform hammerMenu;
    public Transform destroyBuild;
    public Transform rotateBuild;
    public Transform upgradeBuildWood;
    public Transform upgradeBuildStone;
    public Transform upgradeBuildMetal;
    public Transform upgradeBuildHardMetal;

    [Header("HotBar Slots Active?")]
    public Transform oneTrue;
    public Transform twoTrue;
    public Transform threeTrue;
    public Transform fourTrue;
    public Transform fiveTrue;
    public Transform sixTrue;

    [Header("Status Bars")]
    public Slider healthBar;
    public TMP_Text healthText;
    public Slider foodBar;
    public TMP_Text foodText;
    public Slider waterBar;
    public TMP_Text waterText;

    [Header("Building")]
    public BuildSystem buildSystem;
    public GameObject wall;
    public GameObject ceiling;
    public GameObject foundation;
    public GameObject doorWay;

    [Header("Canvas's")]
    public Canvas inventoryCanvas;
    public Canvas menuCanvas;
    public Canvas craftMenuCanvas;
    public Canvas settingsCanvas;
    public Canvas dynamicInvCanvas;


    [Header("Inventory")]
    public InventoryObject inventory;
    public InventoryObject hotBar;
    public InventoryObject equipment;

    [Header("Item in Inventory")]
    public ItemObject wood;
    public ItemObject stone;
    public ItemObject metal;
    public ItemObject hardMetal;

    [Header("Material for Upgrade")]
    public Material selectedBuild;

    public Material woodFloor;
    public Material woodHardSide;
    public Material woodSoftSide;

    public Material stoneFloor;
    public Material stoneHardSide;
    public Material stoneSoftSide;

    public Material metalFloor;
    public Material metalSoftSide;
    public Material metalHardSide;

    public Material hardMetalFloor;
    public Material hardMetalSoftSide;
    public Material hardMetalHardSide;

    //Private Variables=========================================================

    [NonSerialized] public bool deployableEquipped;
    [NonSerialized] public bool buildingPlanEquipped;
    [NonSerialized] public bool hammerEquipped;
    [NonSerialized] public float fireSpeed;
    [NonSerialized] public int damage;
    [NonSerialized] public int nodeDamage;
    [NonSerialized] public int woodDamage;
    [NonSerialized] public ItemObject deployable;
    [NonSerialized] public int currentHealth;
    [NonSerialized] public int currentFood;
    [NonSerialized] public int currentWater;
    [NonSerialized] public Transform itemInHand;
    [NonSerialized] public ItemObject currentItem;

    private IODataBase dataBase;
    private InputManager inputManager;

    private bool check;
    private bool loseHungerAndWater;

    //HotBar Variables
    private bool pressed1 = false;
    private bool pressed2 = false;
    private bool pressed3 = false;
    private bool pressed4 = false;
    private bool pressed5 = false;
    private bool pressed6 = false;

    //StatusBars
    private int maxHealth;
    private int maxFood;
    private int maxWater;
    

    //EquipLogic
    private BoneCombiner _boneCombiner;
    private Transform _pants;
    private Transform _gloves;
    private Transform _boots;
    private Transform _chest;
    private Transform _helmet;

    //Static Variables
    public event EventHandler OnEquipChanged;
    public static bool inv;
    public static bool craftMenu;
    public static bool build;
    public static bool menuOpen;
    public static bool deployOnce;
    public static bool settingsMenuOpen;
    #endregion

    #region Start/Update/Quit

    private void Start()
    {
        inputManager = FindObjectOfType<InputManager>();
        _boneCombiner = new BoneCombiner(gameObject);
        dataBase = FindObjectOfType<IODataBase>();
        inv = false;
        craftMenu = false;
        build = false;
        menuOpen = false;
        buildMenu.gameObject.SetActive(false);
        hammerMenu.gameObject.SetActive(false);
        loseHungerAndWater = true;
        deployOnce = true;
        check = true;

        for (int i = 0; i < equipment.GetSlots.Length; i++)
        {
            equipment.GetSlots[i].onBeforeUpdated += RemoveEquipment;
            equipment.GetSlots[i].onAfterUpdated += GetItemOnEquipment;
        }

        if (MainMenu.newGame)
        {
            damage = 5;
            woodDamage = 5;
            nodeDamage = 3;
            MaxFoodAndWater();
            inventory.Clear();
            equipment.Clear();
            hotBar.Clear();
            
        }
        else
        {
            inventory.Load(); 
            equipment.Load(); 
            hotBar.Load();
            currentFood = PlayerPrefs.GetInt("CurrentFood");
            currentHealth = PlayerPrefs.GetInt("CurrentHealth");
            currentWater = PlayerPrefs.GetInt("CurrentWater");
            maxHealth = 100;
            healthBar.maxValue = maxHealth;
            maxFood = 300;
            foodBar.maxValue = maxFood;
            maxWater = 200;
            waterBar.maxValue = maxWater;
        }
        for (int i = 0; i < ioDB.itemObjects.Count; i++)
        {
            inventory.AddItem(new Item(ioDB.itemObjects[i]), ioDB.itemObjects[i].maxStackSize);
        }
    }

    void Update()
    {
        HealthBars();
        HideandShowUI();
        HotBar();
        Building();
        Interact();
    }

    private void OnApplicationQuit()
    {
        inventory.Clear();
        equipment.Clear();
        hotBar.Clear();
    }

    #endregion

    #region Interaction
    private void Interact()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        if (Physics.Raycast(ray, out hit, 1f))
        {
            if (hit.transform.parent != null)
            {
                var g = hit.transform.parent.GetComponent<Door>();
                if (g && inputManager.GetKeyDown("Interact"))
                {
                    hit.transform.parent.GetComponent<Door>().doorOpen = !hit.transform.parent.GetComponent<Door>().doorOpen;
                }
            }

            var deployable = hit.transform.GetComponent<Deployable>();
            if (deployable)
            {
                if (inputManager.GetKeyDown("Interact") && hammerEquipped)
                {
                    if (inputManager.GetKeyUp("Interact")) return;
                    //Create Loading Thing using something like a slider and setting value with Time.deltaTime
                    float waitTime = 3f;
                    float counter = 0f;

                    while (counter < waitTime)
                    {
                        counter += Time.deltaTime;
                        if (counter >= waitTime)
                        {
                            inventory.AddItem(new Item(deployable.deployedItem), 1);
                            Destroy(deployable.gameObject);
                        }
                    }
                }
            }

            //Creates Text --------------------------------------------------------------------------------------------------------------------------

            GroundItem item = hit.transform.GetComponent<GroundItem>();
            
            if (item && AllUIDown())
            {
                ChangeMiddleWorldText("Pickup: " + item.item.itemName);

                if (inputManager.GetKeyDown("Interact"))
                {
                    if (hotBar.GetEmptySlot() != null) 
                    {
                        hotBar.AddItem(new Item(item.item), item.amount);
                        Destroy(hit.transform.gameObject);
                    }
                    else
                    {
                        inventory.AddItem(new Item(item.item), item.amount);
                        Destroy(hit.transform.gameObject);
                    }
                }
            }

            DynamicInventoryObject dynamicInvObject = hit.transform.GetComponent<DynamicInventoryObject>();

            if (dynamicInvObject && AllUIDown())
            {
                ChangeMiddleWorldText("Open: " + dynamicInvObject.itemThatOpensInv.itemName);
                if (inputManager.GetKeyDown("Interact"))
                {
                    inv = true;
                    Transform dynamicUI = inventoryCanvas.transform.GetChild(2);
                    dynamicUI.gameObject.SetActive(true);
                    dynamicUI.Find("Title").GetComponent<TMP_Text>().text = dynamicInvObject.itemThatOpensInv.itemName;
                    dynamicUI.GetComponent<RectTransform>().localPosition = dynamicInvObject.canvasPosition;
                    dynamicUI.Find("slotContainer").GetComponent<StaticInterface>().inventory = dynamicInvObject.dynamicInv;
                    dynamicUI.Find("slotContainer").GetComponent<StaticInterface>().slots = dynamicInvObject.storageSlots;
                }
            }

            if(item == null && dynamicInvObject == null)
            {
                middleWorldText.gameObject.SetActive(false);
            }
        }
        else
        {
            middleWorldText.gameObject.SetActive(false);
        }
    }

    private void ChangeMiddleWorldText(string text)
    {
        middleWorldText.gameObject.SetActive(true);
        middleWorldText.text = text;
    }
    #endregion

    #region Food/Water/Health
    private void HealthBars()
    {
        healthBar.value = currentHealth;
        healthText.text = currentHealth.ToString();

        foodBar.value = currentFood;
        foodText.text = currentFood.ToString();

        waterBar.value = currentWater;
        waterText.text = currentWater.ToString();

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        if (currentFood > maxFood)
        {
            currentFood = maxFood;
        }

        if (currentWater > maxWater)
        {
            currentWater = maxWater;
        }

        if (currentHealth <= 0)
        {
            currentHealth = 0;
        }
        
        if (currentFood <= 0)
        {
            currentFood = 0;
        }
        
        if (currentWater <= 0)
        {
            currentWater = 0;
        }


        if (loseHungerAndWater)
        {
            if (currentWater > 0)
            {
                currentWater -= UnityEngine.Random.Range(1, 4);
            }
            if (currentFood > 0)
            {
                currentFood -= UnityEngine.Random.Range(1, 4);
            }

            if (currentWater == 0 || currentFood == 0)
            {
                currentHealth -= UnityEngine.Random.Range(1, 4);
            }
            if (currentWater == 0 && currentFood == 0)
            {
                currentHealth -= UnityEngine.Random.Range(3, 9);
            }

            StartCoroutine(LoseHungerAndWater());
            loseHungerAndWater = false;
        }
    }

    public void MaxFoodAndWater()
    {
        //HealthBar
        maxHealth = 100;
        currentHealth = maxHealth;
        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;

        //FoodBar
        maxFood = 300;
        currentFood = maxFood;
        foodBar.maxValue = maxFood;
        foodBar.value = currentFood;

        //WaterBar
        maxWater = 200;
        currentWater = maxWater;
        waterBar.maxValue = maxWater;
        waterBar.value = currentWater;
    }

    private IEnumerator LoseHungerAndWater()
    {
        float counter = 0;
        float waitTime = 10f;

        while (counter < waitTime)
        {
            counter += Time.deltaTime;
            if (counter >= waitTime)
            {
                loseHungerAndWater = true;
                yield break;
            }
            yield return null;
        }
    }

    #endregion

    #region HotBar
    private void AllItemsFalse()
    {
        currentItem = null;
        deployableEquipped = false;
        buildingPlanEquipped = false;
        hammerEquipped = false;
        PressedFalse();
        check = true;
        foreach(Transform child in itemHolder)
        {
            child.gameObject.SetActive(false);
        }

    }

    private void HotBar()
    {
        if (!deployableEquipped)
        {
            if (inputManager.GetKeyDown("Slot One"))
            {
                AllItemsFalse();
                pressed1 = !pressed1;
                oneTrue.gameObject.SetActive(true);
            }
            if (inputManager.GetKeyDown("Slot Two"))
            {
                AllItemsFalse();
                pressed2 = !pressed2;
                twoTrue.gameObject.SetActive(true);
            }
            if (inputManager.GetKeyDown("Slot Three"))
            {
                AllItemsFalse();
                pressed3 = !pressed3;
                threeTrue.gameObject.SetActive(true);
            }
            if (inputManager.GetKeyDown("Slot Four"))
            {
                AllItemsFalse();
                pressed4 = !pressed4;
                fourTrue.gameObject.SetActive(true);
            }
            if (inputManager.GetKeyDown("Slot Five"))
            {
                AllItemsFalse();
                pressed5 = !pressed5;
                fiveTrue.gameObject.SetActive(true);
            }
            if (inputManager.GetKeyDown("Slot Six"))
            {
                AllItemsFalse();
                pressed6 = !pressed6;
                sixTrue.gameObject.SetActive(true);
            }
        }

        if (pressed1 && check)
        {
            GetItemOnHotBar(hotBar.GetSlots[0]);
            check = false;
        }
        if (pressed2 && check)
        {
            GetItemOnHotBar(hotBar.GetSlots[1]);
            check = false;
        }
        if (pressed3 && check)
        {
            GetItemOnHotBar(hotBar.GetSlots[2]);
            check = false;
        }
        if (pressed4 && check)
        {
            GetItemOnHotBar(hotBar.GetSlots[3]);
            check = false;
        }
        if (pressed5 && check)
        {
            GetItemOnHotBar(hotBar.GetSlots[4]);
            check = false;
        }
        if (pressed6 && check)
        {
            GetItemOnHotBar(hotBar.GetSlots[5]);
            check = false;
        }
        if (!pressed1 && !pressed2 && !pressed3 && !pressed4 && !pressed5 && !pressed6)
        {
            AllItemsFalse();
        }
    }
    public void PressedFalse()
    {
        pressed1 = false;
        pressed2 = false;
        pressed3 = false;
        pressed4 = false;
        pressed5 = false;
        pressed6 = false;
        sixTrue.gameObject.SetActive(false);
        fiveTrue.gameObject.SetActive(false);
        fourTrue.gameObject.SetActive(false);
        threeTrue.gameObject.SetActive(false);
        twoTrue.gameObject.SetActive(false);
        oneTrue.gameObject.SetActive(false);
    }

    private void GetItemOnHotBar(InventorySlot slot)
    {
        if (slot.GetItemObject() == null) AllItemsFalse();

        if (slot.GetItemObject() != null)
        {
            currentItem = slot.GetItemObject();
            if (currentItem != null)
            {
                if (currentItem.equippedToHands)
                {
                    itemInHand = itemHolder.Find(currentItem.itemName);
                    itemInHand.gameObject.SetActive(true);
                    switch (currentItem.itemName)
                    {
                        case "Building Plan": buildingPlanEquipped = true; break;
                        case "Hammer": hammerEquipped = true; break;
                    }
                }
                if (currentItem.consumable)
                {
                    if (currentHealth < 100)
                    {
                        currentHealth += currentItem.healthReturn;
                    }
                    if (currentFood < maxFood)
                    {
                        currentFood += currentItem.foodReturn;
                    }
                    if (currentWater < maxWater)
                    {
                        currentWater += currentItem.waterReturn;
                    }

                    playerCraft.RemoveOneItemAndAmount(currentItem, 1);
                    AllItemsFalse();
                }
                if (currentItem.deployable)
                {
                    deployable = currentItem;
                    buildSystem.NewBuild(currentItem.deployableBP);
                    deployableEquipped = true;
                    AllItemsFalse();
                }
            }
        }
    }

    public ItemObject GetCurrentItem()
    {
        return currentItem;
    }

    #endregion

    #region Equipment
    private void GetItemOnEquipment(InventorySlot slot)
    {
        if (slot.GetItemObject() != null)
        {
            ItemObject g = slot.GetItemObject();
            switch (g.type)
            {
                case ItemType.Gloves:
                    _gloves = _boneCombiner.AddLimb(g.characterDisplay, g.boneNames);
                    _gloves.position = Vector3.zero;
                    break;

                case ItemType.Boots:
                    _boots = _boneCombiner.AddLimb(g.characterDisplay, g.boneNames);
                    _boots.position = Vector3.zero;
                    break;

                case ItemType.ChestArmor:
                    _chest = _boneCombiner.AddLimb(g.characterDisplay, g.boneNames);
                    _chest.position = Vector3.zero;
                    break;

                case ItemType.LegsArmor:
                    _pants = _boneCombiner.AddLimb(g.characterDisplay, g.boneNames);
                    _pants.position = Vector3.zero;
                    break;

                case ItemType.Helmet:
                    _helmet = _boneCombiner.AddLimb(g.characterDisplay, g.boneNames);
                    _helmet.position = Vector3.zero;
                    break;
            }
        }
    }

    private void RemoveEquipment(InventorySlot slot)
    {
        if (slot.GetItemObject() == null)
            return;
        ItemObject g = slot.GetItemObject();
        switch (g.type)
        {
            case ItemType.Gloves:
                Destroy(_gloves.gameObject);
                break;

            case ItemType.Boots:
                Destroy(_boots.gameObject);
                break;

            case ItemType.ChestArmor:
                Destroy(_chest.gameObject);
                break;

            case ItemType.LegsArmor:
                Destroy(_pants.gameObject);
                break;

            case ItemType.Helmet:
                Destroy(_helmet.gameObject);
                break;
        }
    }
    #endregion

    #region Public Get Functions
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }
    #endregion

    #region UI
    private void HideandShowUI()
    {
        
        if (LoadingScreen.done)
        {
            if (inputManager.GetKeyDown("Menu"))
            {
                if (build || inv || craftMenu)
                {
                    build = false;
                    inv = false;
                    craftMenu = false;
                }
                else
                {
                    if (settingsCanvas.GetComponent<CanvasGroup>().alpha == 1)
                    {
                        settingsCanvas.GetComponent<CanvasGroup>().alpha = 0;
                        settingsCanvas.GetComponent<CanvasGroup>().interactable = false;
                        settingsCanvas.GetComponent<CanvasGroup>().blocksRaycasts = false;
                    }
                    else
                    {
                        menuOpen = !menuOpen;
                    }
                }
            }
            if (inputManager.GetKeyDown("Inventory") && !build && !menuOpen)
            {
                inv = !inv;
                craftMenu = false;
                menuOpen = false;
            }
            if (inputManager.GetKeyDown("Craft") && !build && !menuOpen)
            {
                craftMenu = !craftMenu;
                inv = false;
                menuOpen = false;
            }
            if (menuOpen)
            {
                ChangeUI(true, false, false);
            }
            if (inv)
            {
                ChangeUI(false, false, true);
            }
            if (craftMenu)
            {
                ChangeUI(false, true, false);
            }
            if (build && !menuOpen && !craftMenu && !inv)
            {
                CursorUnLocked();
            }
            if (!craftMenu && !inv && !build && !menuOpen)
            {
                ChangeUI(false, false, false);
                CursorLocked();
            }
            else
            {
                middleWorldText.gameObject.SetActive(false);
            }
            
        }
    }

    private void ChangeUI(bool menu, bool craftMenu, bool inv)
    {
        menuCanvas.gameObject.SetActive(menu);
        
        if (inv)
        {
            inventoryCanvas.GetComponent<CanvasGroup>().alpha = 1;
        }
        else
        {
            Transform g = inventoryCanvas.transform.Find("DynamicInvUI");
            g.gameObject.SetActive(false);
            inventoryCanvas.GetComponent<CanvasGroup>().alpha = 0;
        }
        inventoryCanvas.GetComponent<CanvasGroup>().interactable = inv;
        inventoryCanvas.GetComponent<CanvasGroup>().blocksRaycasts = inv;
        
        craftMenuCanvas.gameObject.SetActive(craftMenu);

        if(!menu && !craftMenu && !inv)
        {
            CursorLocked();
        }
        else
        {
            CursorUnLocked();
        }
        if (!inv)
        {
            InfoPanel g = FindObjectOfType<InfoPanel>();
            if (g)
            {
                Destroy(g.gameObject);
            }
            GameObject[] slotPanels = GameObject.FindGameObjectsWithTag("SlotPanelActive");
            foreach (GameObject slotPanel in slotPanels)
            {
                slotPanel.gameObject.SetActive(false);
            }
        }
    }

    public bool JustMenuDown()
    {
        if(!menuOpen)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool AllUIDown()
    {
        if (!craftMenu && !inv && !build && !menuOpen)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void ResumeButton()
    {
        menuOpen = false;
    }

    #endregion

    #region Cursor State
    private void CursorLocked()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void CursorUnLocked()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
    #endregion

    #region Building
    private void Building() 
    {
        if (buildingPlanEquipped)
        {
            if (Input.GetMouseButton(1) && !buildSystem.isBuilding && !inv && !craftMenu)
            {
                build = true;
                buildMenu.gameObject.SetActive(true);
            }
            else
            {
                build = false;
                buildMenu.gameObject.SetActive(false);
            }
        }
        else
        {
            build = false;
            buildMenu.gameObject.SetActive(false);
        }
        if (hammerEquipped)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
            if (Physics.Raycast(ray, out hit, 30))
            {
                if(hit.transform != null)
                {
                    if (hit.transform.GetComponent<Building>())
                    {
                        if (Input.GetMouseButton(1) && !inv && !craftMenu)
                        {
                            build = true;
                            hammerMenu.gameObject.SetActive(true);
                            hit.transform.GetChild(2).gameObject.SetActive(true);

                            switch (hit.transform.tag)
                            {
                                case "Twig":        UpgradeBuildShow(true, true, true, true); break;
                                case "Wood":        UpgradeBuildShow(false, true, true, true); break;
                                case "Stone":       UpgradeBuildShow(false, false, true, true); break;
                                case "Metal":       UpgradeBuildShow(false, false, false, true); break;
                                case "HardMetal":   UpgradeBuildShow(false, false, false, false); break;
                                default:            UpgradeBuildShow(false, false, false, false); break;
                            }
                        }
                        else
                        {
                            MakeBuildUnSelected();
                            build = false;
                            hammerMenu.gameObject.SetActive(false);
                        }
                    }
                    else
                    {
                        MakeBuildUnSelected();
                        build = false;
                        hammerMenu.gameObject.SetActive(false);

                    }
                }
            }
        }
    }

    private void UpgradeBuildShow(bool wood, bool stone, bool metal, bool hardMetal)
    {
        upgradeBuildWood.gameObject.SetActive(wood);
        upgradeBuildStone.gameObject.SetActive(stone);
        upgradeBuildMetal.gameObject.SetActive(metal);
        upgradeBuildHardMetal.gameObject.SetActive(hardMetal);
    }

    //Hammer Buttons

    private void MakeBuildUnSelected()
    {
        Building[] g = FindObjectsOfType<Building>();
        foreach (Building h in g)
        {
            h.transform.GetChild(2).gameObject.SetActive(false);
        }
    }

    public void RotateBuild()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        if (Physics.Raycast(ray, out hit, 20.0f))
        {
            if (hit.transform.GetComponent<Building>())
            {
                hit.transform.Rotate(0, 180, 0);
            }
        }
        hammerMenu.gameObject.SetActive(false);
    }

    public void DestroyBuild()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        if (Physics.Raycast(ray, out hit, 20.0f))
        {
            if (hit.transform.GetComponent<Building>())
            {
                Destroy(hit.transform.gameObject);
            }
        }
        hammerMenu.gameObject.SetActive(false);
    }

    

    public void UpgradeToWood()
    {
        UpgradeBuild(wood, 200, 300, 150, woodFloor, woodHardSide, woodSoftSide, "Wood");
    }

    public void UpgradeToStone()
    {
        UpgradeBuild(stone, 150, 200, 100, stoneFloor, stoneHardSide, stoneSoftSide, "Stone");
    }

    public void UpgradeToMetal()
    {
        UpgradeBuild(metal, 100, 150, 75, metalFloor, metalHardSide, metalSoftSide, "Metal");
    }
    public void UpgradeToHardMetal()
    {
        UpgradeBuild(hardMetal, 30, 45, 20, hardMetalFloor, hardMetalHardSide, hardMetalSoftSide, "HardMetal");
    }

    //Build Buttons
    public void BuildFoundation()
    {
        buildSystem.NewBuild(foundation);
    }
    public void BuildWall()
    {
        buildSystem.NewBuild(wall);
    }
    public void BuildCeiling()
    {
        buildSystem.NewBuild(ceiling);
    }
    public void BuildDoorFrame()
    {
        buildSystem.NewBuild(doorWay);
    }

    private void UpgradeBuild(ItemObject materialForUpgrade, int ceilingWallCost, int foundationCost, int doorFrameCost, Material floorM, Material hardSideM, Material softSideM, string tag)
    {
        RaycastHit hit;
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        if (Physics.Raycast(ray, out hit, 20.0f))
        {
            bool hardSide = hit.transform.GetChild(0).name == "HardSide";
            bool softSide = hit.transform.GetChild(1).name == "SoftSide";
            bool nextLevel = hit.transform.GetChild(3).name == "NextLevel";

            if (hit.transform.GetComponent<Wall>() && inventory.IsItemAndAmountInInv(materialForUpgrade, ceilingWallCost))
            {
                playerCraft.RemoveOneItemAndAmount(materialForUpgrade, ceilingWallCost);
                hit.transform.tag = tag;
                hit.transform.GetComponent<Building>().onUpgrade = true;

                if (hardSide) hit.transform.GetChild(0).GetComponent<MeshRenderer>().material = hardSideM;

                if (softSide) hit.transform.GetChild(1).GetComponent<MeshRenderer>().material = softSideM;
            }
            if (hit.transform.GetComponent<Ceiling>() && inventory.IsItemAndAmountInInv(materialForUpgrade, ceilingWallCost))
            {
                playerCraft.RemoveOneItemAndAmount(materialForUpgrade, ceilingWallCost);
                hit.transform.tag = tag;
                hit.transform.GetComponent<Building>().onUpgrade = true;

                if (hardSide) hit.transform.GetChild(0).GetComponent<MeshRenderer>().material = floorM;

                if (softSide) hit.transform.GetChild(1).GetComponent<MeshRenderer>().material = softSideM;
            }
            if (hit.transform.GetComponent<Foundation>() && inventory.IsItemAndAmountInInv(materialForUpgrade, foundationCost))
            {
                if (hit.transform.childCount > 5)
                {
                    hit.transform.GetChild(3).gameObject.SetActive(true);
                    Destroy(hit.transform.GetChild(5).gameObject);
                    Destroy(hit.transform.GetChild(6).gameObject);
                    Destroy(hit.transform.GetChild(7).gameObject);
                    Destroy(hit.transform.GetChild(8).gameObject);
                }
                playerCraft.RemoveOneItemAndAmount(materialForUpgrade, foundationCost);
                hit.transform.tag = tag;
                hit.transform.GetComponent<Building>().onUpgrade = true;

                if (hardSide) hit.transform.GetChild(0).GetComponent<MeshRenderer>().material = hardSideM;

                if (softSide) hit.transform.GetChild(1).GetComponent<MeshRenderer>().material = floorM;

                if (nextLevel) hit.transform.GetChild(3).GetComponent<MeshRenderer>().material = hardSideM;
            }
            if (hit.transform.GetComponent<DoorFrame>() && inventory.IsItemAndAmountInInv(materialForUpgrade, doorFrameCost))
            {
                playerCraft.RemoveOneItemAndAmount(materialForUpgrade, doorFrameCost);
                hit.transform.tag = tag;
                hit.transform.GetComponent<Building>().onUpgrade = true;

                if (hardSide) hit.transform.GetChild(0).GetComponent<MeshRenderer>().material = hardSideM;

                if (softSide) hit.transform.GetChild(1).GetComponent<MeshRenderer>().material = softSideM;
            }
        }
        hammerMenu.gameObject.SetActive(false);
    }
    #endregion
}


/// <Ideas>
/// ----------
/// 6 Levels of Food and Water survival
/// 
/// 1 = Wood/Stone
/// 2 = Wood/Stone
/// 3 = Tin/Stone/Wood
/// 4 = Iron/Wood/Glass
/// 5 = Iron/Steel/Wood/Copper
/// 6 = "Sum Cool metal"/Copper/Steel/Wood
/// 
/// ---Food and Water Survival Levels---
///   --Water--
/// Basic Water Purifier --> Simple Water Purifier --> Large Water Purifier --> Intricate Water Purifier --> Advanced Water Purifier --> Industrial Water Purifier
///   --Food--
/// Small Campfire --> Medium Campfire --> Small Grill --> Medium Grill --> Large Grill --> Industrial Grill
/// ----------
/// 4 Levels of Creating and Smelting
/// 
/// 1 = Wood/Stone/Clay
/// 2 = Brick/Wool/Wood
/// 3 = Steel/Wool/Oil
/// 4 = "Sum Cool metal"/Steel/Wool/Oil
/// 
/// ---Smelting Levels---
/// Clay Furnace --> Brick Furnace --> Steel Furnace --> Industrial Furnace 
///-----------


/* Get FootSteps Audio

     GET Texture Player is Standing on (Use for footstep sounds)

    public Transform playerTransform;
    public Terrain t;
    
    public int posX;
    public int posZ;
    public float[] textureValues;
    
    void Start () 
      {
        t = Terrain.activeTerrain;
        playerTransform = gameObject.transform;
      }
    
    void Update()
      {
        // For better performance, move this out of update 
        // and only call it when you need a footstep.
        GetTerrainTexture();
      }
    
    public void GetTerrainTexture()
      {
        ConvertPosition(playerTransform.position);
        CheckTexture();
      }
    
    void ConvertPosition(Vector3 playerPosition)
      {
        Vector3 terrainPosition = playerPosition - t.transform.position;
    
        Vector3 mapPosition = new Vector3
        (terrainPosition.x / t.terrainData.size.x, 0,
        terrainPosition.z / t.terrainData.size.z);
    
        float xCoord = mapPosition.x * t.terrainData.alphamapWidth;
        float zCoord = mapPosition.z * t.terrainData.alphamapHeight;
    
        posX = (int)xCoord;
        posZ = (int)zCoord;
      }
    
    void CheckTexture()
      {
        float[,,] aMap = t.terrainData.GetAlphamaps (posX, posZ, 1, 1);
        textureValues[0] = aMap[0,0,0];
        textureValues[1] = aMap[0,0,1];
        textureValues[2] = aMap[0,0,2];
        textureValues[3] = aMap[0,0,3];
      }
    }

    ----------USE--------------------------
    public AudioSource source;

    public AudioClip[] stoneClips;
    public AudioClip[] dirtClips;
    public AudioClip[] sandClips;
    public AudioClip[] grassClips;
    
    AudioClip previousClip;
    
    public void PlayFootstep()
    {   
      GetTerrainTexture();
    
      if (textureValues[0] > 0)
      {
        source.PlayOneShot(GetClip(stoneClips), textureValues[0]);
      }
      if (textureValues[1] > 0)
      {
        source.PlayOneShot(GetClip(dirtClips), textureValues[1]);
      }
      if (textureValues[2] > 0)
      {
        source.PlayOneShot(GetClip(dirtClips), textureValues[2]);
      }
      if (textureValues[3] > 0)
      {
        source.PlayOneShot(GetClip(dirtClips), textureValues[3]);
      }
    }
    
    AudioClip GetClip(AudioClip[] clipArray)
      {
        int attempts = 3;
        AudioClip selectedClip = 
        clipArray[Random.Range(0, clipArray.Length - 1)];
    
        while (selectedClip == previousClip && attempts > 0)
          {
            selectedClip = 
            clipArray[Random.Range(0, clipArray.Length - 1)];
            
            attempts--;
          }
    
        previousClip = selectedClip;
        return selectedClip;
    
      }
     */
