using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum ShopState
{
    BUY,
    SELL,
    SOLDOUT
}
public class ShopSlot : MonoBehaviour
{
    [SerializeField] private Item item;
    [SerializeField] private ShopState state;
    [SerializeField] private ShopMgr shopMgr;
    public int slotPirce = 0;
    public int shopIndex = 0;

    public TextMeshProUGUI textItemPrice;
    public TextMeshProUGUI textItemStack;
    public TextMeshProUGUI textItemModifyStack;
    public Image imgSlodOut;
    public Image imgUse;

    public Image slotIcon;

    private Button slotMy;
    public void Init(Item _item, ShopState _state)
    {
        item = _item;
        state = _state;

        //Debug.Log($"Item Price: {item.itemPrice}");
        Debug.Log("���ݻ����� �������� ����: " + _item.itemStack);

        slotPirce = item.itemPrice;
        if (state == ShopState.SELL)
        {
            slotPirce = (int)(slotPirce * 0.7);
        }
        slotIcon.sprite = item.itemImage;
        textItemPrice.text = slotPirce.ToString();
        DrawBuyShop(item);

        slotMy = GetComponent<Button>();
    }

    public void DrawBuyShop(Item _item)//���� ������ ǥ�� �ʱ�ȭ
    {
        if (_item.itemType == Item.ItemType.Consumables || _item.itemType == Item.ItemType.Ect)
        {
            // �Ҹ�ǰ�� ��� ǥ��
            if (_item.itemStack > 0)
            {
                textItemStack.text = _item.itemStack.ToString();
                textItemStack.gameObject.SetActive(true);
            }
            else
            {
                textItemStack.gameObject.SetActive(false);
                if (state != ShopState.SOLDOUT)
                {
                    state = ShopState.SOLDOUT;
                    imgSlodOut.gameObject.SetActive(true);
                }
                return;
            }
        }
        else
        {
            // ���������� ��� ǥ��
            if (_item.modifyStack > 0)
            {
                textItemModifyStack.text = "+"+_item.modifyStack.ToString();
                textItemModifyStack.gameObject.SetActive(true);
            }
            else
                textItemModifyStack.gameObject.SetActive(false);
        }
    }
    public Item GetItem()//���� '�Ǹ�'������ ǥ�� �ʱ�ȭ
    {
        return item;
    }

    public void InBasket()
    {
        // �������� �ν��Ͻ�ȭ�Ͽ� ShopSlot�� ����
        BasketBox basket = PoolBasketBox(shopMgr.baskets);
        if (basket == null)
        {
            basket = Instantiate(shopMgr.prBasketBox, shopMgr.trBasketBox);
        }
        // ������ ���� �ʱ�ȭ
        basket.Init(this);
        basket.shopMgr = shopMgr;
        // ������ ������ ����Ʈ�� �߰�
        shopMgr.baskets.Add(basket);

        UseImgSet(true);

        //���� ���
        shopMgr.CalcPrice(slotPirce);
    }
    BasketBox PoolBasketBox(List<BasketBox> _b)
    {
        foreach (BasketBox _box in _b)
        {
            if (!_box.gameObject.activeSelf)
            {
                _box.gameObject.SetActive(true);
                return _box;
            }
        }
        return null;
    }

    public void ShopMgrSet(ShopMgr _shopMgr)
    {
        shopMgr = _shopMgr;
    }

    public void UseImgSet(bool active)
    {
        if (active)
        {
            slotMy.interactable = false;
            imgUse.gameObject.SetActive(true);
        }
        else
        {
            slotMy.interactable = true;
            imgUse.gameObject.SetActive(false);
        }
    }
}
