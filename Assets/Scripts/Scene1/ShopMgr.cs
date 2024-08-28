using DarkPixelRPGUI.Scripts.UI.Equipment;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopMgr : MonoBehaviour
{
    [SerializeField] List<ShopSlot> shopSlots;
    [SerializeField] List<ShopSlot> playerShopItems;

    [SerializeField] ShopSlot slotPrefab;
    public ShopState shopState;

    [Header("Tab")]
    [SerializeField] Button buyTab;
    [SerializeField] Button sellTab;

    [Header("Pannel")]
    public GameObject buyPannel;
    public GameObject sellPannel;

    public Transform tfBuy;
    public Transform tfSell;

    [Header("��ٱ���")]
    public Transform trBasketBox;
    public BasketBox prBasketBox;

    public List<BasketBox> baskets;

    public TextMeshProUGUI calcPrice;
    public TextMeshProUGUI PayText;

    public int basketsPrice;
    //[SerializeField] Button btnBuy;// TODO: Add SoundEv 

    private void Start()
    {
        shopState = ShopState.BUY;
        OpenTap(shopState);
        RefreshShopItems();
    }

    public void RefreshShopItems()
    {
        Debug.Log("Make Shop Items");
        //�� �Ʒ��� for�� ���������� GameUIMgr�� �������ҰŰ��� ���⼭�����ϱ⿣����? ����� ����̸�����
        for (int i = 0; i < 12; i++)
        {
            // ���� �������� ����
            Item originalItem = ItemResources.instance.itemRS[Random.Range(1, 3)];

            // ���ο� ������ �ν��Ͻ��� ����
            Item newItem = new Item
            {
                itemCode = originalItem.itemCode,
                itemName = originalItem.itemName,
                itemType = originalItem.itemType,
                itemImage = originalItem.itemImage,
                itemPrice = originalItem.itemPrice,
                itemPower = originalItem.itemPower,
                itemDesc = originalItem.itemDesc,
                itemStack = originalItem.itemStack,
                modifyStack = originalItem.modifyStack
            };

            if (newItem.itemType == Item.ItemType.Consumables)
            {
                newItem.itemStack = Random.Range(1, 10);
            }
            else
            {
                newItem.modifyStack = Random.Range(1, 4);
                newItem.ModifyPowerSet(newItem);
            }

            // �������� �ν��Ͻ�ȭ�Ͽ� ShopSlot�� ����
            ShopSlot slot = Instantiate(slotPrefab, tfBuy);

            // ������ ���� �ʱ�ȭ
            slot.Init(newItem, ShopState.BUY);

            slot.shopIndex = i;
            slot.ShopMgrSet(this);
            // ������ ������ ����Ʈ�� �߰�
            shopSlots.Add(slot);
        }
    }

    void SetSellItems()
    {
        // ������ playerShopItems ������ ��� ��Ȱ��ȭ
        if (playerShopItems.Count > 0)
        {
            for (int i = 0; i < playerShopItems.Count; i++)
            {
                playerShopItems[i].gameObject.SetActive(false);
            }
        }

        List<ShopSlot> inactiveSlots = new List<ShopSlot>(playerShopItems);
        playerShopItems.Clear();

        // �κ��丮�� ��� �������� ������ ���Կ� �߰�
        for (int i = 0; i < Inventory.Single.items.Count; i++)
        {
            Item originalItem = ItemResources.instance.itemRS[Inventory.Single.items[i].itemCode];

            Item _item = new Item
            {
                itemCode = originalItem.itemCode,
                itemName = originalItem.itemName,
                itemType = originalItem.itemType,
                itemImage = originalItem.itemImage,
                itemPrice = originalItem.itemPrice,
                itemPower = originalItem.itemPower,
                itemDesc = originalItem.itemDesc,
                itemStack = Inventory.Single.items[i].itemStack,
                modifyStack = Inventory.Single.items[i].modifyStack
            };

            // ��Ȱ��ȭ�� ������ �����ϰų�, ���ο� ������ ����
            ShopSlot sellSlot = PoolShopSlot(inactiveSlots);
            if (sellSlot == null)
            {
                sellSlot = Instantiate(slotPrefab, tfSell);
            }
            // ���� �ʱ�ȭ
            sellSlot.Init(_item, ShopState.SELL);
            sellSlot.shopIndex = i;
            sellSlot.ShopMgrSet(this);

            playerShopItems.Add(sellSlot);
        }
        inactiveSlots.Clear();
    }

    ShopSlot PoolShopSlot(List<ShopSlot> _s)
    {
        foreach (ShopSlot slot in _s)
        {
            if (!slot.gameObject.activeSelf)
            {
                slot.gameObject.SetActive(true);
                return slot;
            }
        }
        return null;
    }

    void OpenTap(ShopState _state)// isOpen == true '�Ǹ�'�� Ȱ��, isOpen == false '����'�� Ȱ��
    {
        basketsPrice = 0;
        for (int i = 0; i < baskets.Count; i++)
        {
            baskets[i].gameObject.SetActive(false);
        }

        if (_state == ShopState.BUY)
        {
            buyTab.interactable = false;
            sellTab.interactable = true;

            //TODO: Buy ImageObejct Active, Sell ImageObject unActive
            buyPannel.SetActive(true);
            sellPannel.SetActive(false);

            PayText.text = "����";
            calcPrice.text = basketsPrice.ToString();
        }
        else if (_state == ShopState.SELL)
        {
            buyTab.interactable = true;
            sellTab.interactable = false;

            buyPannel.SetActive(false);
            sellPannel.SetActive(true);

            SetSellItems();

            PayText.text = "�Ǹ�";
            calcPrice.text = basketsPrice.ToString();

        }

        /*if (baskets.Count > 0)
        {
            for (int i = 0; i < baskets.Count; i++)
            {
                if (_state == ShopState.BUY)
                {
                    shopSlots[baskets[i].BasketShopIndex()].UseImgSet(false);
                }
                else
                {
                    playerShopItems[baskets[i].BasketShopIndex()].UseImgSet(false);
                }
            }
        }*/
        if (baskets.Count > 0)
        {
            for (int i = 0; i < baskets.Count; i++)
            {
                int basketIndex = baskets[i].BasketShopIndex();
                if (_state == ShopState.BUY && basketIndex < shopSlots.Count)
                {
                    shopSlots[basketIndex].UseImgSet(false);
                }
                else if (_state == ShopState.SELL && basketIndex < playerShopItems.Count)
                {
                    playerShopItems[basketIndex].UseImgSet(false);
                }
            }
        }

    }

    public List<ShopSlot> GetShopSlots()
    {
        return shopSlots;
    }
    public void SetShopSlots(ShopSlot _shopSlot)
    {
        shopSlots.Add(_shopSlot);
    }

    public void CalcPrice(int _price)
    {
        basketsPrice += _price;
        calcPrice.text = basketsPrice.ToString();
    }

    // Tab Open
    public void ClickBuyTab() { OpenTap(ShopState.BUY); }
    public void ClickSellTab() { OpenTap(ShopState.SELL); }

    public void OnClickPayBtn()
    {
        if (shopState == ShopState.BUY)// ����
        {
            if (basketsPrice <= 1500)//basketsPrice <= GameMgr.playerData[0].player_Gold
            {
                //TODO: baskets �����۵��� Inventory�� ����, �ش� shopSlots[i].soldOut.Active(true), shopSlots.Clear();
                //shopSlots[baskets[i].BasketShopIndex()].GetItem().itemType == Item.ItemType.Consumables || shopSlots[baskets[i].BasketShopIndex()].GetItem().itemType == Item.ItemType.Ect
                for (int i = 0; i < baskets.Count; i++)
                {
                    if (baskets[i].stack == 0)
                    {
                        continue;
                    }

                    //������ �������� ���Ű��ɰ�����ŭ ����, ���� �� ������ 0�ϰ�� SoldOut
                    int lastStack = shopSlots[baskets[i].BasketShopIndex()].GetItem().itemStack - baskets[i].stack;
                    shopSlots[baskets[i].BasketShopIndex()].GetItem().itemStack = baskets[i].stack;

                    Inventory.Single.AddItem(shopSlots[baskets[i].BasketShopIndex()].GetItem());//�߰�
                    if (lastStack == 0)
                    {
                        shopSlots[baskets[i].BasketShopIndex()].SoldOut(true);
                    }
                    else
                    {
                        shopSlots[baskets[i].BasketShopIndex()].GetItem().itemStack = lastStack;
                    }
                }
                //����
                Inventory.Single.SortingStackItems();
            }
            else
            {
                //TODO: ���Ž��� or �׳� ��ٱ��ϱݾ� > ������� �̸� ��ư��ü�� Ȱ��ȭ �ȵǰ� �ؾ��ҵ�.
                Debug.Log("���� ����");
            }
        }
        else if (shopState == ShopState.SELL)// �Ǹ�
        {
            // 
        }
    }
}
