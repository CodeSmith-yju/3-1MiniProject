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

    [Header("Debug")]
    [SerializeField]private Button slotMy;
    public void Init(Item _item, ShopState _state)
    {
        slotMy = gameObject.GetComponent<Button>();

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
    }

    public void DrawBuyShop(Item _item)//���� ������ ǥ�� �ʱ�ȭ
    {
        if (_item.itemType == Item.ItemType.Consumables || _item.itemType == Item.ItemType.Ect)// �Ҹ�ǰ�� ��� ǥ��
        {
            if (_item.itemStack > 0)
            {
                textItemStack.text = _item.itemStack.ToString();
                textItemStack.gameObject.SetActive(true);
                if (imgUse.gameObject.activeSelf)
                {
                    UseImgSet(false);
                }
            }
            else
            {
                textItemStack.gameObject.SetActive(false);
                if (state != ShopState.SOLDOUT)
                {
                    state = ShopState.SOLDOUT;
                    UseImgSet(false);
                    SoldOut(true);
                }
                return;
            }
        }
        else // ���������� ��� ǥ��
        {
            if (_item.modifyStack > 0)
            {
                textItemModifyStack.text = "+"+_item.modifyStack.ToString();
                textItemModifyStack.gameObject.SetActive(true);
            }
            else
                textItemModifyStack.gameObject.SetActive(false);

            if (_item.itemStack <= 0)
            {
                if (state != ShopState.SOLDOUT)
                {
                    state = ShopState.SOLDOUT;
                    UseImgSet(false);
                    SoldOut(true);
                }
            }

        }
    }
    public Item GetItem()
    {
        /*if (item.itemType != Item.ItemType.Consumables)
        {
            Debug.Log("�ù߿� �ٸ��������̵��İ� �̸�: "+item.itemName + " ��ȭ: "+item.modifyStack + " ����: "+item.itemPower + " ��ȭ: "+item.itemPrice);
        }*/
        return item;
    }

    public void InBasket()
    {
        bool yesAdd = true;
        // �������� �ν��Ͻ�ȭ�Ͽ� ShopSlot�� ����
        BasketBox basket = PoolBasketBox(shopMgr.baskets);
        if (basket == null)
        {
            basket = Instantiate(shopMgr.prBasketBox, shopMgr.trBasketBox);
            yesAdd = false;
        }
        // ������ ���� �ʱ�ȭ
        basket.Init(this);
        basket.shopMgr = shopMgr;
        // ������ ������ ����Ʈ�� �߰�
        if (!yesAdd)
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
    public void SoldOut(bool active)
    {
        if (active)
        {
            slotMy.interactable = false;
            imgSlodOut.gameObject.SetActive(true);
        }
        else
        {
            slotMy.interactable = true;
            imgSlodOut.gameObject.SetActive(false);
        }
    }

    public void SoldNow(int _cnt)
    {
        item.itemStack -= _cnt;
        DrawBuyShop(item);
    }
}