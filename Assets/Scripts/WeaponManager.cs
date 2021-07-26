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
    private Hand[] hands;

    //관리 차원에서 쉽게 무기 접근이 가능하도록 만듦
    private Dictionary<string, Gun> gunDictionary = new Dictionary<string, Gun>();

    private Dictionary<string, Hand> handDictionary = new Dictionary<string, Hand>();

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
        for (int i = 0; i < hands.Length; i++)
        {
            handDictionary.Add(hands[i].handName, hands[i]);
        }
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
            theHandController.HandChange(handDictionary[_name]);
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
        }
    }
}