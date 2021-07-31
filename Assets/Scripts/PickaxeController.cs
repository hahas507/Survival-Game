using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickaxeController : MeleeWeaponController
{
    public static bool isActivate = false;

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
                if (hitInfo.transform.tag == "Rock")
                {
                    hitInfo.transform.GetComponent<Rock>().Mining();
                }
                else if (hitInfo.transform.tag == "NPC")
                {
                    hitInfo.transform.GetComponent<Pig>().Damage(/*currentMeleeWeapon.damage*/1, transform.position);
                }
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