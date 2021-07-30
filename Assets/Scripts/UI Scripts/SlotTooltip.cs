using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotTooltip : MonoBehaviour
{
    [SerializeField]
    private GameObject go_Base;

    [SerializeField]
    private Text txt_ItemName;

    [SerializeField]
    private Text txt_ItemDescription;

    [SerializeField]
    private Text txt_ItemHowToUse;

    public void ShowTooltip(Item _item, Vector3 _pos)
    {
        go_Base.SetActive(true);
        _pos += new Vector3(go_Base.GetComponent<RectTransform>().rect.width * 0.5f, -go_Base.GetComponent<RectTransform>().rect.height * 0.5f, 0);
        go_Base.transform.position = _pos;
        txt_ItemName.text = _item.itemName;
        txt_ItemDescription.text = _item.itemDescription;

        if (_item.itemType == Item.ItemType.Equipment)
        {
            txt_ItemHowToUse.text = "Right Click to Equipt";
        }
        else if (_item.itemType == Item.ItemType.Used)
        {
            txt_ItemHowToUse.text = "Right Click to Eat";
        }
        else
        {
            txt_ItemDescription.text = "";
        }
    }

    public void HideTooltip()
    {
        go_Base.SetActive(false);
    }
}