using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static bool canPlayerMove;
    public static bool isOpenInventory = false;

    public static bool isNight = false;
    public static bool isWater = false;

    public static bool isPause = false;

    private WeaponManager theWeaponManager;

    private bool flag = false;

    private void Update()
    {
        if (isOpenInventory || isPause)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            canPlayerMove = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            canPlayerMove = true;
        }

        if (isWater)
        {
            if (!flag)
            {
                StopAllCoroutines();
                StartCoroutine(theWeaponManager.WeaponInCoroutine());
                flag = true;
            }
        }
        else
        {
            if (flag)
            {
                flag = false;
                theWeaponManager.WeaponOut();
            }
        }
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        theWeaponManager = FindObjectOfType<WeaponManager>();
    }
}