using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Basket : MonoBehaviour
{
    [SerializeField] ShopSlot mySlot;
    
    public TextMeshProUGUI BasketStack;

    public void Init(ShopSlot _shopslot)
    {
        mySlot = _shopslot;
        //BasketStack.text = mySlot.Item;
    }


}
