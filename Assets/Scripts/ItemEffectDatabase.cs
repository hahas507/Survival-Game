using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemEffect
{
    public string itemName; //아이템 이름

    [Tooltip("Use only HP, SP, DP, HUNGER, THIRSTY, SATISFY")]
    public string[] part; //어느 부위

    public int[] num; //수치
}

public class ItemEffectDatabase : MonoBehaviour
{
    [SerializeField]
    private ItemEffect[] itemEffects;

    [SerializeField]
    private WeaponManager theWeaponManager;

    [SerializeField]
    private SlotTooltip theSlotTooltip;

    //필요한 컴포넌트
    [SerializeField]
    private StatusController playerStatusController;

    private const string HP = "HP", SP = "SP", DP = "DP", HUNGER = "HUNGER", THIRSTY = "THIRSTY", SATISFY = "SATISFY";

    public void UseItem(Item _item)
    {
        if (_item.itemType == Item.ItemType.Equipment)
        {
            StartCoroutine(theWeaponManager.ChangeWeaponCoroutine(_item.weaponType, _item.itemName));
        }
        else if (_item.itemType == Item.ItemType.Used)
        {
            for (int x = 0; x < itemEffects.Length; x++)
            {
                if (itemEffects[x].itemName == _item.itemName)
                {
                    for (int y = 0; y < itemEffects[x].part.Length; y++)
                    {
                        switch (itemEffects[x].part[y])
                        {
                            case HP:
                                playerStatusController.IncreaseHP(itemEffects[x].num[y]);
                                break;

                            case SP:
                                playerStatusController.IncreaseSP(itemEffects[x].num[y]);
                                break;

                            case DP:
                                playerStatusController.IncreaseDP(itemEffects[x].num[y]);
                                break;

                            case HUNGER:
                                playerStatusController.IncreaseHunger(itemEffects[x].num[y]);
                                break;

                            case THIRSTY:
                                playerStatusController.IncreaseThristy(itemEffects[x].num[y]);
                                break;

                            case SATISFY:
                                break;

                            default:
                                Debug.Log("Wrong status part; use only HP, SP, DP, HUNGER, THIRSTY, SATISFY");
                                break;
                        }
                        Debug.Log("Used " + _item.itemName);
                    }
                    return;
                }
            }
            Debug.Log("itemEffectsDatabase에 일치하는 itemName이 없습니다.");
        }
    }

    public void ShowTooltip(Item _item, Vector3 _pos)
    {
        theSlotTooltip.ShowTooltip(_item, _pos);
    }

    public void HideTooltip()
    {
        theSlotTooltip.HideTooltip();
    }
}