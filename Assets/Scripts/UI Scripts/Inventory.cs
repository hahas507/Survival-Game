using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static bool inventoryActivated = false;

    [SerializeField]
    private GameObject go_InventoryBase;

    [SerializeField]
    private GameObject go_SlotParent;

    private GunController theGunController;
    private SlotTooltip theSlotTooltip;

    private Slot[] slots;

    public Slot[] GetSlots()
    {
        return slots;
    }

    [SerializeField]
    private Item[] items;

    public void LoadToInven(int _arrNum, string _itemName, int _itemNum)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i].itemName == _itemName)
            {
                slots[_arrNum].AddItem(items[i], _itemNum);
            }
        }
    }

    private void Start()
    {
        slots = go_SlotParent.GetComponentsInChildren<Slot>();
        theGunController = FindObjectOfType<GunController>();
        theSlotTooltip = FindObjectOfType<SlotTooltip>();
    }

    private void Update()
    {
        TryOpenInventory();
    }

    private void TryOpenInventory()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            inventoryActivated = !inventoryActivated;

            if (inventoryActivated)
            {
                theGunController.CancelFineSight();
                OpenInvenvtory();
            }
            else
            {
                theSlotTooltip.HideTooltip();
                CloseInventory();
            }
        }
    }

    private void CloseInventory()
    {
        GameManager.isOpenInventory = false;
        go_InventoryBase.SetActive(false);
    }

    private void OpenInvenvtory()
    {
        GameManager.isOpenInventory = true;
        go_InventoryBase.SetActive(true);
    }

    public void AcquireItem(Item _item, int _count = 1)
    {
        if (Item.ItemType.Equipment != _item.itemType)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].item != null)
                {
                    if (slots[i].item.itemName == _item.itemName)
                    {
                        slots[i].SetSlotCount(_count);
                        return;
                    }
                }
            }
        }

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)
            {
                slots[i].AddItem(_item, _count);
                return;
            }
        }
    }
}