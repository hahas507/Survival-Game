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
    private MeleeWeapon[] melee;

    //관리 차원에서 쉽게 무기 접근이 가능하도록 만듦
    private Dictionary<string, Gun> gunDictionary = new Dictionary<string, Gun>();

    private Dictionary<string, MeleeWeapon> meleeDictionary = new Dictionary<string, MeleeWeapon>();

    //현재 무기 타입
    [SerializeField]
    private string currentWeaponType;

    [SerializeField]
    private GunController theGunController;

    [SerializeField]
    private HandController theHandController;

    private void Start()
    {
        for (int i = 0; i < guns.Length; i++)
        {
            gunDictionary.Add(guns[i].gunName, guns[i]);
        }
        for (int i = 0; i < melee.Length; i++)
        {
            meleeDictionary.Add(melee[i].MeleeWeaponName, melee[i]);
        }

        var test = gunDictionary.Values;
        var test2 = meleeDictionary.Values;
        Debug.Log(test);
        Debug.Log(test2);
    }

    private void Update()
    {
        if (!isChangeWeapon)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                StartCoroutine(ChangeWeaponCoroutine("MELEE", "HAND"));
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                StartCoroutine(ChangeWeaponCoroutine("GUN", "SubMachineGun1"));
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
        else if (_type == "MELEE")
        {
            theHandController.HandChange(meleeDictionary[_name]);
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

            case "MELEE":
                HandController.isActivate = false;
                break;
        }
    }
}