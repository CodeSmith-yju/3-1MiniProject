using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldItems : MonoBehaviour
{
    public Item item;
    public SpriteRenderer image;

    public void SetItem(Item _item)
    {
        item.itemName = _item.itemName;
        item.itemImage = _item.itemImage;
        item.itemType = _item.itemType;
        //04-01 Add
        item.itemTitle = _item.itemTitle;
        item.itemDesc = _item.itemDesc;

        image.sprite = _item.itemImage;
    }

    public Item GetItem()
    {
        Debug.Log("GetItem"+item.itemName);
        return item;
    }
    public void DestroyItem()
    {
        Destroy(gameObject);
    }
}
