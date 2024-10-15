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

    public string itemName;

    // ������ ���� & ��ȭ��ġ stack ���� �߰�
    public int itemStack = 0;
    public int modifyStack = 0;

    public Sprite itemImage;
    public Sprite typeIcon;

    //04-01 Add
    public string itemTitle;
    public string itemDesc;

    public int itemIndex;
    //05-08
    public int itemCode;
    //06
    public float itemPower;
    //08
    public int itemPrice;// �����۰���
    //09
    public string PrimaryCode;

    // Init ������
    public Item(int code, ItemType type, string name, int stack, int modify, float power, int price, string title, string desc, int index) : this()
    {
        itemCode = code;
        Init(code, type, name, stack, modify, power, price, title, desc, index);
    }
    // Init �޼���
    void Init(int code, ItemType type, string name, int stack, int modify, float power, int price, string title, string desc, int index)
    {
        itemCode = code;
        itemType = type;
        itemName = name;

        itemStack = stack;
        modifyStack = modify;

        itemPower = power;
        itemPrice = price;

        itemTitle = title;
        itemDesc = desc;

        itemIndex = index;
    }

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
        //Debug.Log("���� ���� �ҷ�����: "+PrimaryCode);
        return PrimaryCode.ToString();
    }

    //���� ���������� ����Ҷ� ���� �޼��带 ����� �������ϴµ�..?
    public Item UpgradeModifyPowerSet(Item _item)
    {
        if (_item.modifyStack > -1)
        {
            _item.modifyStack++;
            _item.itemPower = ItemResources.instance.itemRS[_item.itemCode].itemPower + (ItemResources.instance.itemRS[_item.itemCode].itemPower) * 0.5f * _item.modifyStack;
            _item.itemPrice = ItemResources.instance.itemRS[_item.itemCode].itemPrice + (int)((ItemResources.instance.itemRS[_item.itemCode].itemPrice * 0.1) * _item.modifyStack);
            /*switch (_item.modifyStack)
            {
                case 0:
                    break
                default:
                    _item.modifyStack++;
                    _item.itemPower = ItemResources.instance.itemRS[_item.itemCode].itemPower + (ItemResources.instance.itemRS[_item.itemCode].itemPower) * 0.5f * _item.modifyStack;
                    _item.itemPrice = ItemResources.instance.itemRS[_item.itemCode].itemPrice + (int)((ItemResources.instance.itemRS[_item.itemCode].itemPrice * 0.1) * _item.modifyStack);
                    break
            }*/
        }
        return _item;
    }
    public float GetPreViewPower(Item _item)
    {
        int _i = _item.modifyStack;
        if (_i < 1)
        {
            _i = 2;
        }
        else
        {
            _i += 2;
        }
        float power = 0f;
        power = _item.itemPower + _item.itemPower * 0.5f * _i;
        Debug.Log("Befor: "+_item.itemPower + " / After: "+ power);
        return power;
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

    public Item GenerateRandomItem(int r)
    {
        Item _item = new()
        {
            itemCode = ItemResources.instance.itemRS[r].itemCode,
            itemName = ItemResources.instance.itemRS[r].itemName,
            itemType = ItemResources.instance.itemRS[r].itemType,
            itemTitle = ItemResources.instance.itemRS[r].itemTitle,
            itemImage = ItemResources.instance.itemRS[r].itemImage,
            itemPrice = ItemResources.instance.itemRS[r].itemPrice,
            itemPower = ItemResources.instance.itemRS[r].itemPower,
            itemDesc = ItemResources.instance.itemRS[r].itemDesc,
            itemStack = ItemResources.instance.itemRS[r].itemStack,
            modifyStack = ItemResources.instance.itemRS[r].modifyStack,
            typeIcon = ItemResources.instance.itemRS[r].typeIcon,
            //PK�� �ڵ������ǰ� ����
        };

        return _item;
    }
}

