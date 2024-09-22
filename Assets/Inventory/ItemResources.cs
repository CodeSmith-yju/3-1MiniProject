using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemResources : MonoBehaviour
{
    public static ItemResources instance;
    public List<Item> itemRS = new();
    public List<Sprite> iconRS = new();
    private void Awake()
    {
        instance = this;
    }


    /*public void IconSet(Item _item)
    {
        switch (_item.itemType)
        {
            case Item.ItemType.Equipment_Arrmor://Range
                _item.typeIcon = typeIconRS[0];
                break;
            case Item.ItemType.Equipment_Boots://Atk spd
                _item.typeIcon = typeIconRS[1];
                break;
            case Item.ItemType.Equipment_Helmet://HP
                _item.typeIcon = typeIconRS[2];
                break;
            case Item.ItemType.Equipment_Weapon://Atk
                _item.typeIcon = typeIconRS[3];
                break;
            case Item.ItemType.Consumables:// +
                _item.typeIcon = typeIconRS[4];
                break;
            case Item.ItemType.Ect:// 
                _item.typeIcon = typeIconRS[5];
                break;
            default:
                break;
        }
    }*/

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
}