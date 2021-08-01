using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public static bool isChangeWeapon;
    public static Transform currentWeapon;
    public static Animator currentWeaponAnimation;

    [SerializeField]
    private float ChangeWeaponDelayTime;

    [SerializeField]
    private float ChangeWeaponEndDelayTime;

    [SerializeField]
    private Gun[] guns;

    [SerializeField]
    private MeleeWeapon[] hands;

    [SerializeField]
    private MeleeWeapon[] axes;

    [SerializeField]
    private MeleeWeapon[] pickaxes;

    //관리 차원에서 쉽게 무기 접근이 가능하도록 만듦
    private Dictionary<string, Gun> gunDictionary = new Dictionary<string, Gun>();

    private Dictionary<string, MeleeWeapon> handDictionary = new Dictionary<string, MeleeWeapon>();
    private Dictionary<string, MeleeWeapon> axeDictionary = new Dictionary<string, MeleeWeapon>();
    private Dictionary<string, MeleeWeapon> pickaxeDictionary = new Dictionary<string, MeleeWeapon>();

    //현재 무기 타입
    [SerializeField]
    private string currentWeaponType;

    [SerializeField]
    private GunController theGunController;

    [SerializeField]
    private HandController theHandController;

    [SerializeField]
    private AxeController theAxeController;

    [SerializeField]
    private PickaxeController thePickaxeController;

    private void Start()
    {
        for (int i = 0; i < guns.Length; i++)
        {
            gunDictionary.Add(guns[i].gunName, guns[i]);
        }
        for (int i = 0; i < hands.Length; i++)
        {
            handDictionary.Add(hands[i].meleeWeaponName, hands[i]);
        }
        for (int i = 0; i < axes.Length; i++)
        {
            axeDictionary.Add(axes[i].meleeWeaponName, axes[i]);
        }
        for (int i = 0; i < pickaxes.Length; i++)
        {
            pickaxeDictionary.Add(pickaxes[i].meleeWeaponName, pickaxes[i]);
        }

        //var test = axeDictionary.Values;
        //var test1 = handDictionary.Values;
        //var test2 = gunDictionary.Values;
    }

    private void Update()
    {
        if (!isChangeWeapon)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                StartCoroutine(ChangeWeaponCoroutine("HAND", "맨손"));
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                StartCoroutine(ChangeWeaponCoroutine("GUN", "SubMachineGun1"));
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                StartCoroutine(ChangeWeaponCoroutine("AXE", "Axe"));
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                StartCoroutine(ChangeWeaponCoroutine("PICKAXE", "Pickaxe"));
            }
        }
    }

    public IEnumerator ChangeWeaponCoroutine(string _type, string _name)
    {
        isChangeWeapon = true;
        currentWeaponAnimation.SetTrigger("Weapon_Out");

        yield return new WaitForSeconds(ChangeWeaponDelayTime);

        CancelPreWeaponAction();
        WeaponChange(_type, _name);

        yield return new WaitForSeconds(ChangeWeaponEndDelayTime);

        currentWeaponType = _type;
        isChangeWeapon = false;
    }

    private void WeaponChange(string _type, string _name)
    {
        if (_type == "GUN")
        {
            theGunController.GunChange(gunDictionary[_name]);
        }
        else if (_type == "HAND")
        {
            theHandController.MeleeWeaponChange(handDictionary[_name]);
        }
        else if (_type == "AXE")
        {
            theAxeController.MeleeWeaponChange(axeDictionary[_name]);
        }
        else if (_type == "PICKAXE")
        {
            thePickaxeController.MeleeWeaponChange(pickaxeDictionary[_name]);
        }
    }

    private void CancelPreWeaponAction()
    {
        switch (currentWeaponType)
        {
            case "GUN":
                theGunController.CancelFineSight();
                theGunController.CancelReload();
                GunController.isActivate = false;
                break;

            case "HAND":
                HandController.isActivate = false;
                break;

            case "AXE":
                AxeController.isActivate = false;
                break;

            case "PICKAXE":
                PickaxeController.isActivate = false;
                break;
        }
    }

    public IEnumerator WeaponInCoroutine()
    {
        isChangeWeapon = true;
        currentWeaponAnimation.SetTrigger("Weapon_Out");

        yield return new WaitForSeconds(ChangeWeaponDelayTime);

        currentWeapon.gameObject.SetActive(false);
    }

    public void WeaponOut()
    {
        isChangeWeapon = false;

        currentWeapon.gameObject.SetActive(true);
    }
}