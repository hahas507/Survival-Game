using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeController : MeleeWeaponController
{
    public static bool isActivate = false;

    private void Start()
    {
        WeaponManager.currentWeapon = currentMeleeWeapon.GetComponent<Transform>();
        WeaponManager.currentWeaponAnimation = currentMeleeWeapon.anim;
    }

    protected void Update()
    {
        if (isActivate)
        {
            TryAttack();
        }
    }

    protected override IEnumerator HitCoroutine()
    {
        while (isSwing)
        {
            if (CheckObject())
            {
                isSwing = false;
                Debug.Log(hitInfo.transform.name);
            }
            yield return null;
        }
    }

    public override void MeleeWeaponChange(MeleeWeapon _melee)
    {
        base.MeleeWeaponChange(_melee);
        isActivate = true;
    }
}