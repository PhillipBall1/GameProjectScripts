using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Animations.Rigging;
using UnityEngine.VFX;
using System;

public class PlayerAttack : MonoBehaviour
{
    public bool debugRecoil;
    public bool debugWeapons;
    public Transform spine;
    public Animator anim;
    public Camera cam;
    public ObjectPooler groundImpact;
    public ObjectPooler aiImpact;
    public ObjectPooler bulletHole;

    public IODataBase dataBase;

    public PlayerCraft pC;

    public AudioSource hitMarker;
    public AudioSource headShotHitmarker;

    public AudioSource gunClick;
    public AudioSource gunShot;
    public AudioSource gunReload;


    private float nextTimeToFire = 0f;
    private float nextTimeToSwing = 0f;
    private Player player;
    [NonSerialized]
    public bool block;
    [NonSerialized]
    public bool canSwing;
    private bool reloading = false;

    private int recoilIndex;

    public float upRecoil;
    public float sideRecoil;

    private bool currentlyShooting = false;

    public GameObject floatingText;
    private GameObject popupHolder;
    

    // Start is called before the first frame update
    void Start()
    {
        popupHolder = GameObject.FindGameObjectWithTag("PopupParent");
        canSwing = true;
        player = transform.GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (debugRecoil)
        {
            Debug.Log("Up Recoil: " + upRecoil);
            Debug.Log("Side Recoil: " + sideRecoil);
            Debug.Log("Recoil Index: " + recoilIndex);
        }
        if(player.AllUIDown() && LoadingScreen.done)
        {
            Attack();
        }

        if(player.GetCurrentItem() == null)
        {
            reloading = false;
            anim.SetLayerWeight(anim.GetLayerIndex("Attack Layer"), 0);
            anim.SetLayerWeight(anim.GetLayerIndex("Tool Layer"), 0);
        }
        else if (player.GetCurrentItem().weaponItem)
        {
            anim.SetLayerWeight(anim.GetLayerIndex("Attack Layer"), 1);
            anim.SetLayerWeight(anim.GetLayerIndex("Tool Layer"), 0);
            SetAnimations(player.GetCurrentItem().animator);
        }
        else if (player.GetCurrentItem().toolItem)
        {
            anim.SetLayerWeight(anim.GetLayerIndex("Tool Layer"), 1);
            anim.SetLayerWeight(anim.GetLayerIndex("Attack Layer"), 0);
            SetAnimations(player.GetCurrentItem().animator);
        }
    }

    private void Attack()
    {
        if (player.GetCurrentItem() != null)
        {
            #region Tool Item
            if (player.GetCurrentItem().toolItem && Input.GetMouseButtonDown(0))
            {
                if (Time.time >= nextTimeToSwing)
                {
                    if (player.GetCurrentItem().swingRight) StartCoroutine(ToolSwing(true, false, true, false));

                    if (player.GetCurrentItem().swingDown) StartCoroutine(ToolSwing(false, true, false, true));

                    nextTimeToSwing = Time.time + 1f / player.GetCurrentItem().toolSwingSpeed;
                }
            }
            else
            {
                SetToolAnimBools(false, false, false, false);
            }
            #endregion

            #region Weapon Item
            if (player.GetCurrentItem().weaponItem)
            {
                Recoil();
                bool first = true;
                if (Input.GetMouseButton(1) && !reloading)
                {

                    if (!currentlyShooting)
                    {
                        SetGunAnimBools(true, false, false);
                    }
                    bool tryingToShootSemi = Input.GetMouseButtonDown(0);
                    if (player.GetCurrentItem().semiAutomatic && tryingToShootSemi && Time.time >= nextTimeToFire)
                    {

                        if (player.GetCurrentItem().currentAmmoAmount > 0)
                        {
                            nextTimeToFire = Time.time + 1f / player.GetCurrentItem().fireSpeed;
                            currentlyShooting = true;
                            Shoot();
                            player.GetCurrentItem().currentAmmoAmount -= 1;
                            recoilIndex += 1;
                        }
                        else
                        {
                            currentlyShooting = false;
                            recoilIndex = 0;
                            gunClick.clip = player.GetCurrentItem().gunClick;
                            gunClick.Play();
                        }
                        
                    }
                    else if (!tryingToShootSemi)
                    {
                        currentlyShooting = false;
                        recoilIndex = 0;
                    }
                    bool tryingToShootAuto = Input.GetMouseButton(0);
                    if (!player.GetCurrentItem().semiAutomatic && tryingToShootAuto && Time.time >= nextTimeToFire)
                    {
                        if (player.GetCurrentItem().currentAmmoAmount > 0)
                        {
                            nextTimeToFire = Time.time + 1f / player.GetCurrentItem().fireSpeed;
                            Shoot();
                            currentlyShooting = true;
                            player.GetCurrentItem().currentAmmoAmount -= 1;
                            recoilIndex += 1;
                        }
                        else
                        {
                            currentlyShooting = false;
                            recoilIndex = 0;
                            gunClick.clip = player.GetCurrentItem().gunClick;
                            gunClick.Play();

                        }
                        
                    }
                    else if (!tryingToShootAuto)
                    {
                        currentlyShooting = false;
                        recoilIndex = 0;
                    }

                }
                else
                {
                    first = false;
                }

                if (player.GetCurrentItem().currentAmmoAmount != player.GetCurrentItem().maxAmmo && Input.GetKeyDown(KeyCode.R))
                {
                    if (player.GetCurrentItem().singleReload)
                    {
                        StartCoroutine(SingleReload(player.GetCurrentItem().ammoType));
                    }
                    else if (!player.GetCurrentItem().singleReload)
                    {
                        StartCoroutine(MagReload(player.GetCurrentItem().ammoType));
                    }
                }

                if (!first || reloading)
                {
                    if (cam.fieldOfView < 90)
                    {
                        cam.fieldOfView += 3;
                    }
                    recoilIndex = 0;
                    //StartAnimation("WeaponIdle");
                    if (!reloading)
                    {
                        SetGunAnimBools(false, false, true);
                    }
                    else
                    {
                        anim.SetTrigger("reloading");
                    }
                }
                else
                {
                    if (cam.fieldOfView > 70)
                    {
                        cam.fieldOfView -= 5;
                    }    
                }

            }
            else
            {
                cam.fieldOfView = 90;
            }
#endregion

            //BLOCKING
            if (Input.GetMouseButton(1) && canSwing && player.AllUIDown())
            {

            }
        }
    }

    private IEnumerator ToolSwing(bool swingRight, bool swingDown, bool hitRight, bool hitDown)
    {
        SetToolAnimBools(swingRight, swingDown, false, false);
        yield return new WaitForSeconds(player.GetCurrentItem().toolTimeBetweenSwingAndHit);
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, player.GetCurrentItem().toolLength))
        {
            Farmables farmable = hit.transform.GetComponent<Farmables>();
            if (farmable)
            {
                if (farmable.tree)
                {
                    //instantiate texture on tree

                    Farm(player.GetCurrentItem().woodDamage, dataBase.wood, farmable);
                }
                if (farmable.node)
                {
                    //instantiate texture on node

                    if (farmable.objName == "Metal") Farm(player.GetCurrentItem().nodeDamage, dataBase.copper, farmable);

                    if (farmable.objName == "Stone") Farm(player.GetCurrentItem().nodeDamage, dataBase.stone, farmable);
                }
            }

            HitBoxCheck hitBox = hit.transform.GetComponent<HitBoxCheck>();
            if (hitBox != null)
            {
                if (hitBox.tag == "Legs")
                {
                    hitBox.transform.root.GetComponent<GlobalAIController>().TakeDamage(player.GetCurrentItem().weaponDamage / 2);
                    hitMarker.Play();
                }
                if (hitBox.tag == "Body")
                {
                    hitBox.transform.root.GetComponent<GlobalAIController>().TakeDamage(player.GetCurrentItem().weaponDamage);
                    hitMarker.Play();
                }
                if (hitBox.tag == "Head")
                {
                    hitBox.transform.root.GetComponent<GlobalAIController>().TakeDamage(player.GetCurrentItem().weaponDamage * player.GetCurrentItem().criticalStrikeMultiplier);
                    headShotHitmarker.Play();
                }
            }

            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * player.GetCurrentItem().impactForce);
            }
            if (hit.collider != null)
            {
                SetToolAnimBools(false, false, hitRight, hitDown);
            }
        }
    }

    #region Reload
    private IEnumerator SingleReload(ItemObject ammoType)
    {
        while (true)
        {
            
            if (player.GetCurrentItem().currentAmmoAmount != player.GetCurrentItem().maxAmmo)
            {
                if (player.inventory.IsItemInInventory(ammoType))
                {
                    reloading = true;
                    gunReload.clip = player.GetCurrentItem().gunReload;
                    gunReload.Play();
                    yield return new WaitForSeconds(player.GetCurrentItem().reloadSpeed);
                    pC.RemoveOneItemAndAmount(ammoType, 1);
                    player.GetCurrentItem().currentAmmoAmount += 1;
                }
                else
                {
                    reloading = false;
                    anim.ResetTrigger("reloading");
                    break;
                }
            }
            else
            {
                reloading = false;
                anim.ResetTrigger("reloading");
                break;
            }
        }
    }
    private IEnumerator MagReload(ItemObject ammoType)
    {
        while (true)
        {
            int difference = player.GetCurrentItem().maxAmmo - player.GetCurrentItem().currentAmmoAmount;
            if (player.inventory.IsItemAndAmountInInv(ammoType, difference))
            {
                reloading = true;
                gunReload.clip = player.GetCurrentItem().gunReload;
                gunReload.Play();
                yield return new WaitForSeconds(player.GetCurrentItem().reloadSpeed);
                pC.RemoveOneItemAndAmount(ammoType, difference);
                player.GetCurrentItem().currentAmmoAmount = player.GetCurrentItem().maxAmmo;
                reloading = false;
                anim.ResetTrigger("reloading");
                break;
            }
            else if (player.inventory.IsItemInInventory(ammoType))
            {
                reloading = true;
                gunReload.clip = player.GetCurrentItem().gunReload;
                gunReload.Play();
                yield return new WaitForSeconds(player.GetCurrentItem().reloadSpeed);
                int amountInSlot = player.inventory.AmountInSlot(ammoType);
                pC.RemoveOneItemAndAmount(ammoType, amountInSlot);
                player.GetCurrentItem().currentAmmoAmount += amountInSlot;
                reloading = false;
                anim.ResetTrigger("reloading");
                break;
            }
            else
            {
                Debug.Log("No Ammo");
                break;
            }
        }
    }
    #endregion

    #region Shooting
    private void Shoot()
    {
        //StartAnimation("WeaponFire");
        SetGunAnimBools(false, true, false);
        Transform muzzleFlash = player.itemInHand.Find("MuzzleFlash");
        if (muzzleFlash != null)
        {
            muzzleFlash.GetComponent<VisualEffect>().Play();
        }
        gunShot.clip = player.GetCurrentItem().gunShot;
        gunShot.Play();

        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit))
        {
            HitBoxCheck hitBox = hit.transform.GetComponent<HitBoxCheck>();
            if (hitBox != null)
            {
                if (hitBox.tag == "Legs")
                {
                    hitBox.transform.root.GetComponent<GlobalAIController>().TakeDamage(player.GetCurrentItem().weaponDamage / 2);
                    hitMarker.Play();
                }
                if (hitBox.tag == "Body")
                {
                    hitBox.transform.root.GetComponent<GlobalAIController>().TakeDamage(player.GetCurrentItem().weaponDamage);
                    hitMarker.Play();
                }
                if (hitBox.tag == "Head")
                {
                    hitBox.transform.root.GetComponent<GlobalAIController>().TakeDamage(player.GetCurrentItem().weaponDamage * player.GetCurrentItem().criticalStrikeMultiplier);
                    headShotHitmarker.Play();
                }
            }

            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * player.GetCurrentItem().impactForce);
            }

            GameObject groundImp = groundImpact.GetPooledObject();
            GameObject livingImp = aiImpact.GetPooledObject();
            GameObject bulletH = bulletHole.GetPooledObject();

            if (groundImp != null && hitBox == null)
            {
                groundImp.transform.position = hit.point;
                groundImp.transform.rotation = Quaternion.LookRotation(hit.normal);
                groundImp.SetActive(true);
                groundImp.GetComponent<ParticleSystem>().Play();
                StartCoroutine(SetInactive(groundImp, 2f));
            }
            if (livingImp != null && hitBox != null)
            {
                livingImp.transform.position = hit.point;
                livingImp.transform.rotation = Quaternion.LookRotation(hit.normal);
                livingImp.SetActive(true);
                livingImp.GetComponent<ParticleSystem>().Play();
                StartCoroutine(SetInactive(livingImp, 2f));
            }
            if (bulletH != null && hit.transform.tag != "Terrain")
            {
                bulletH.transform.position = hit.point;
                bulletH.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
                bulletH.SetActive(true);
                StartCoroutine(SetInactive(bulletH, 6f));
            }

        }

    }

    private void Recoil()
    {
        if (recoilIndex == player.GetCurrentItem().maxAmmo)
        {
            recoilIndex = 0;
        }

        upRecoil = recoilIndex * player.GetCurrentItem().recoilYStep;
        sideRecoil = player.GetCurrentItem().recoilXPattern[recoilIndex];
    }
    #endregion

    IEnumerator SetInactive(GameObject g, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        g.SetActive(false);
    }

    private void SetGunAnimBools(bool aiming, bool firing, bool idle)
    {
        anim.SetBool("isAiming", aiming);
        anim.SetBool("isFiring", firing);
        anim.SetBool("isGunIdle", idle);
    }
    private void SetToolAnimBools(bool rightSwing, bool swingDown, bool hitRight, bool hitDown)
    {
        anim.SetBool("rightSwing", rightSwing);
        anim.SetBool("downSwing", swingDown);
        anim.SetBool("rightSwingHit", hitRight);
        anim.SetBool("downSwingHit", hitDown);
    }

    private void SetAnimations(AnimatorOverrideController cont)
    {
        anim.runtimeAnimatorController = cont;
    }

    private void Farm(int damage, ItemObject item, Farmables farmed)
    {
        if (damage >= 0)
        {
            if (farmed.objName.Contains("tree"))
            {
                //Spawn texture on tree
            }
            else
            {
                //spawn dust / particles
            }
            farmed.health -= damage;
            player.inventory.AddItem(new Item(item), damage);

            GameObject f = Instantiate(floatingText, popupHolder.transform);
            if (farmed.objName.Contains("Tree"))
            {
                f.GetComponentInChildren<TextMeshProUGUI>().text = "+ " + damage + " " + "Wood";
            }
            else if (farmed.objName == "Stone")
            {
                f.GetComponentInChildren<TextMeshProUGUI>().text = "+ " + damage + " " + "Stone";
            }
            else if (farmed.objName == "Metal")
            {
                f.GetComponentInChildren<TextMeshProUGUI>().text = "+ " + damage + " " + "Metal";
            }
            Destroy(f.gameObject, 4f);
        }
    }
}
