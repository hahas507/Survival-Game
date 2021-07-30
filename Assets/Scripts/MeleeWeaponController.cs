using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MeleeWeaponController : MonoBehaviour
//abstract 미완성
{
    //현재 장착된 Hand형 타입 무기
    [SerializeField]
    protected MeleeWeapon currentMeleeWeapon;

    protected bool isAttack = false;
    protected bool isSwing = false;

    protected RaycastHit hitInfo;

    protected void TryAttack()
    {
        if (!Inventory.inventoryActivated)
        {
            if (Input.GetButton("Fire1"))
            {
                if (!isAttack)
                {
                    //코루틴 실행
                    StartCoroutine(AttackCoroutine());
                }
            }
        }
    }

    protected IEnumerator AttackCoroutine()
    {
        isAttack = true;
        currentMeleeWeapon.anim.SetTrigger("Attack");

        yield return new WaitForSeconds(currentMeleeWeapon.attackDelayA);
        isSwing = true; //공격 활성화 시점

        StartCoroutine(HitCoroutine());

        yield return new WaitForSeconds(currentMeleeWeapon.attackDelayB);
        isSwing = false;

        yield return new WaitForSeconds(currentMeleeWeapon.attackDelay - currentMeleeWeapon.attackDelayA - currentMeleeWeapon.attackDelayB);

        isAttack = false;
    }

    protected abstract IEnumerator HitCoroutine();

    protected bool CheckObject()
    {
        if (Physics.Raycast(transform.position, transform.forward, out hitInfo, currentMeleeWeapon.range))
        {
            return true;
        }
        return false;
    }

    //완성함수이지만 추가 편집이 가능한 함수 virtual
    public virtual void MeleeWeaponChange(MeleeWeapon _melee)
    {
        if (WeaponManager.currentWeapon != null)
        {
            WeaponManager.currentWeapon.gameObject.SetActive(false);
        }

        currentMeleeWeapon = _melee;
        WeaponManager.currentWeapon = currentMeleeWeapon.GetComponent<Transform>();
        WeaponManager.currentWeaponAnimation = currentMeleeWeapon.anim;

        currentMeleeWeapon.transform.localPosition = Vector3.zero;
        currentMeleeWeapon.gameObject.SetActive(true);
    }
}