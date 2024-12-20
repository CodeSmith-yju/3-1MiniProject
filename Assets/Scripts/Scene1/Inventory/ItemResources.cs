using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemResources : MonoBehaviour
{
    public static ItemResources instance;
    public List<Item> itemRS = new();//LocalType
    public List<Sprite> itemSpriteRS = new();
    public List<Sprite> iconRS = new();


    [Header("DB Items")]
    public List<Item> DBItems = new();
    private void Awake()
    {
        instance = this;
        DBItems ??= new();
        for (int i = 0; i < 20; i++)
        {
            DBItems.Add(DBConnector.LoadItemFromDB(i));
        }
        setTypeIcons();
        //여기밑으로 주석치면 InsertDB 정상동작함
        itemRS.Clear();
        itemRS.AddRange(DBItems);

        for (int i = 0; i < itemRS.Count; i++)
        {
            itemRS[i].itemImage = itemSpriteRS[i];
        }
    }
    void setTypeIcons()//Local_Icon_Set
    {
        for (int i = 0; i < itemRS.Count; i++)
        {
            itemRS[i].PrimaryCode = itemRS[i].GetNewPK(12);
            //Debug.Log("===================================================" + itemRS[i].itemType);
            IconSet(itemRS[i]);
        }
    }

    public void IconSet(Item _item)
    {
        switch (_item.itemType)
        {
            case Item.ItemType.Equipment_Arrmor://Range -> Defens
                _item.typeIcon = Inventory.Single.typeIconRS[0];
                break;
            case Item.ItemType.Equipment_Boots://Atk spd
                _item.typeIcon = Inventory.Single.typeIconRS[1];
                break;
            case Item.ItemType.Equipment_Helmet://HP
                _item.typeIcon = Inventory.Single.typeIconRS[2];
                break;
            case Item.ItemType.Equipment_Weapon://Atk
                _item.typeIcon = Inventory.Single.typeIconRS[3];
                break;
            case Item.ItemType.Consumables:// +
                _item.typeIcon = Inventory.Single.typeIconRS[4];
                break;
            case Item.ItemType.Ect:// 
                _item.typeIcon = Inventory.Single.typeIconRS[5];
                break;
            default:
                break;
        }
    }

    public Sprite SetTpyeIcon(Item _item)
    {
        Sprite sp = null;
        switch (_item.itemType)
        {
            case Item.ItemType.Equipment_Arrmor://Range
                sp = Inventory.Single.typeIconRS[0];
                break;
            case Item.ItemType.Equipment_Boots://Atk spd
                sp = Inventory.Single.typeIconRS[1];
                break;
            case Item.ItemType.Equipment_Helmet://HP
                sp = Inventory.Single.typeIconRS[2];
                break;
            case Item.ItemType.Equipment_Weapon://Atk
                sp = Inventory.Single.typeIconRS[3];
                break;
            case Item.ItemType.Consumables:// +
                sp = Inventory.Single.typeIconRS[4];
                break;
            case Item.ItemType.Ect:// 
                sp = Inventory.Single.typeIconRS[5];
                break;
            default:
                break;
        }
        return sp;
    }

    //FieldItem Prefab copy code
    /*
    public GameObject fieldItemPrefab;
    public Vector3[] pos;

    private void Start()
    {
        for(int i = 0; i < 6; i++)
        {
            GameObject go = Instantiate(fieldItemPrefab, pos[i], Quaternion.identity);
            go.GetComponent<FieldItems>().SetItem(itemRS[Random.Range(0, 3)]);
        }
    }
    */

    public void DB_ITEM_TEST()
    {
        for (int i = 0; i < itemRS.Count; i++)
        {
            if (DBConnector.InsertItemToDB(itemRS[i]))
            {
                Debug.Log("True");
            }
            else
            {
                Debug.Log("False");
            }
        }
    }
}