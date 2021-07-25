using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    //필요한 컴포넌트
    [SerializeField]
    private GunController theGunController;

    private Gun currentGun;

    //HUD on/off
    [SerializeField]
    private GameObject go_BulletHUD;

    //총알 개수 텍스트에 반영
    [SerializeField]
    private Text[] textBullet;

    private void Update()
    {
        CheckBullet();
    }

    private void CheckBullet()
    {
        currentGun = theGunController.GetGun();
        textBullet[0].text = currentGun.carryBulletCount.ToString();
        textBullet[1].text = currentGun.reloadBulletCount.ToString();
        textBullet[2].text = currentGun.currentBulletCount.ToString();
    }
}