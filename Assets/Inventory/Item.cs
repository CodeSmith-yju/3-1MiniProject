using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using UnityEngine;

[System.Serializable]
public class Item
{
    public enum ItemType//04-01 Add
    {
        Equipment_Arrmor,
        Equipment_Boots,
        Equipment_Helmet,
        Equipment_Weapon,
        Consumables,
        Ect
    }
    public ItemType itemType;

    // ������ ���� & ��ȭ��ġ stack ���� �߰�
    public int itemStack = 0;
    public int modifyStack = 0;
    
    public string itemName;
    public Sprite itemImage;
    public Sprite typeIcon;
    public List<ItemEffect> efts;
    //04-01 Add
    public string itemTitle;
    public string itemDesc;

    public int itemIndex;
    //04-22
    public bool isDraggable;
    //05-08
    public int itemCode;
    //06
    public float itemPower;
    //08
    public int itemPrice;// �����۰���
    //09
    public string PrimaryCode;

    /*public Item(string name, ItemType type, int stack, int modify, float power, int price, string title, string desc)
    {
        Init(name, type, stack, modify, power, price, title, desc);
    }

    // Init �޼���
    public void Init(string name, ItemType type, int stack, int modify, float power, int price, string title, string desc)
    {
        itemName = name;
        itemType = type;
        itemStack = stack;
        modifyStack = modify;
        itemPower = power;
        itemPrice = price;
        itemTitle = title;
        itemDesc = desc;

        // ���� �ڵ� ����
        PrimaryCode = GenerateUniqueCode(12);
    }*/

    public Item()//�����ڸ� ���� ������ ���� �� ������ PrimaryCode ����
    {
        PrimaryCode = GenerateUniqueCode(12);
    }
    public string GetNewPK(int _12)
    {
        return GenerateUniqueCode(12);
    }
    private string GenerateUniqueCode(int length) // 12���� �̻��� ������ ���ڿ��� �����ϴ� �޼���
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        StringBuilder code = new StringBuilder();
        System.Random random = new System.Random();

        for (int i = 0; i < length; i++)
        {
            code.Append(chars[random.Next(chars.Length)]);
        }

        return code.ToString();
    }
    public string GetItemUniqueCode()
    {
        Debug.Log("���� ���� �ҷ�����: "+PrimaryCode);
        return PrimaryCode.ToString();
    }
    public bool Use()
    {
        bool isUsed = false;
        
        foreach (ItemEffect eft in efts)
        {
            isUsed = eft.ExcuteRole();
        }

        return isUsed;
    }
    //���� ���������� ����Ҷ� ���� �޼��带 ����� �������ϴµ�..?
    public Item UpgradeModifyPowerSet(Item _item)
    {
        if (_item.modifyStack > 0)
        {
            switch (_item.modifyStack)
            {
                /*case 0:
                    break;*/
                default:
                    _item.modifyStack++;
                    _item.itemPower = ItemResources.instance.itemRS[_item.itemCode].itemPower + (ItemResources.instance.itemRS[_item.itemCode].itemPower)*0.5f*_item.modifyStack;
                    _item.itemPrice = ItemResources.instance.itemRS[_item.itemCode].itemPrice + (int)((ItemResources.instance.itemRS[_item.itemCode].itemPrice * 0.1)*_item.modifyStack);
                    break;
            }
        }
        return _item;
    }
    public void IconSet(Item _item)
    {
        switch (_item.itemType)
        {
            case ItemType.Equipment_Arrmor://Range 40
                _item.typeIcon = Inventory.Single.typeIconRS[0];
                break;
            case ItemType.Equipment_Boots://Atk spd 07
                _item.typeIcon = Inventory.Single.typeIconRS[1];
                break;
            case ItemType.Equipment_Helmet://HP 43
                _item.typeIcon = Inventory.Single.typeIconRS[2];
                break;
            case ItemType.Equipment_Weapon://Atk 72
                _item.typeIcon = Inventory.Single.typeIconRS[3];
                break;
            case ItemType.Consumables:// + 11
                _item.typeIcon = Inventory.Single.typeIconRS[4];
                break;
            case ItemType.Ect:// 29
                _item.typeIcon = Inventory.Single.typeIconRS[5];
                break;
            default:
                break;
        }
    }
}

