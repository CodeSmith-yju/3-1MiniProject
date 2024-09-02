using DarkPixelRPGUI.Scripts.UI.Equipment;
using System.Collections.Generic;
using TMPro;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

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

    private void Awake()
    {
        shopSlots = new();
    }
    private void Start()
    {
        if (baskets == null)
            baskets = new List<BasketBox>();

        shopState = ShopState.BUY;
        OpenTap(shopState);
        //RefreshShopItems();
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
                modifyStack = originalItem.modifyStack,
            };

            if (newItem.itemType == Item.ItemType.Consumables)
            {
                newItem.itemStack = Random.Range(1, 10);
            }
            else
            {
                newItem.modifyStack = Random.Range(1, 4);
                newItem.UpgradeModifyPowerSet(newItem);
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

    public void ReLoadShopItems(List<Item> _loadShopitem)
    {
        if (shopSlots.Count > 0)
            Debug.Log("�ƴ� �����а� �����ִٰ�?");
        Debug.Log("Run ReLoad shopItem");
        for (int i = 0; i < _loadShopitem.Count; i++)
        {
            ShopSlot slot = Instantiate(slotPrefab, tfBuy);
            Debug.Log("���� �������� �����Ͽ� ��ġ");
            // ������ ���� �ʱ�ȭ
            slot.ShopMgrSet(this);
            slot.Init(_loadShopitem[i], ShopState.BUY);
            slot.shopIndex = i;

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
                itemDesc = originalItem.itemDesc,
                itemPower = Inventory.Single.items[i].itemPower,
                itemPrice = Inventory.Single.items[i].itemPrice,
                itemStack = Inventory.Single.items[i].itemStack,
                modifyStack = Inventory.Single.items[i].modifyStack,
                PrimaryCode = Inventory.Single.items[i].PrimaryCode
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

    public void OpenTap(ShopState _state)// isOpen == true '�Ǹ�'�� Ȱ��, isOpen == false '����'�� Ȱ��
    {
        shopState = _state;

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

        if (shopState == ShopState.BUY)
        {
            if (basketsPrice <= GameMgr.playerData[0].player_Gold)
            {
                PayText.GetComponentInParent<Button>().interactable = true;
            }
            else
            {
                PayText.GetComponentInParent<Button>().interactable = false;
            }
        }
        
    }

    // Tab Open
    public void ClickBuyTab() { OpenTap(ShopState.BUY); }
    public void ClickSellTab() { OpenTap(ShopState.SELL); }

    public void OnClickPayBtn()
    {
        if (shopState == ShopState.BUY)// ����
        {
            if (basketsPrice <= GameMgr.playerData[0].player_Gold)//basketsPrice <= GameMgr.playerData[0].player_Gold
            {
                //TODO: baskets �����۵��� Inventory�� ����, �ش� shopSlots[i].soldOut.Active(true), shopSlots.Clear();
                //shopSlots[baskets[i].BasketShopIndex()].GetItem().itemType == Item.ItemType.Consumables || shopSlots[baskets[i].BasketShopIndex()].GetItem().itemType == Item.ItemType.Ect
                for (int i = 0; i < baskets.Count; i++)
                {
                    if (baskets[i].stack == 0 || !baskets[i].gameObject.activeSelf)
                    {
                        continue;
                    }

                    if (baskets[i].GetBasketShopSlot().GetItem().itemType == Item.ItemType.Consumables || baskets[i].GetBasketShopSlot().GetItem().itemType == Item.ItemType.Ect)
                    {
                        for (int j = 0; j < baskets[i].stack; j++)
                        {
                            //Item item = ItemResources.instance.itemRS[baskets[i].GetBasketShopSlot().GetItem().itemCode];
                            Item item = baskets[i].GetBasketShopSlot().GetItem();
                            Inventory.Single.AddItem(item);
                        }
                    }
                    else
                    {
                        Inventory.Single.AddItem(baskets[i].GetBasketShopSlot().GetItem());
                    }
                    baskets[i].GetBasketShopSlot().SoldNow(baskets[i].stack); // ���ŵȸ�ŭ�� stack ����
                }
                //���Ű� ���������� ����� ��Ȳ.
                GameMgr.playerData[0].player_Gold -= basketsPrice;
                RefeshBaskets();
            }
            else
            {
                //TODO: ���Ž��� or �׳� ��ٱ��ϱݾ� > ������� �̸� ��ư��ü�� Ȱ��ȭ �ȵǰ� �ؾ��ҵ�.
                Debug.Log("���� ����");
            }
        }
        else if (shopState == ShopState.SELL)// �Ǹ�
        {
            List<Item> RemoveItems = new();
            for (int i = 0; i < baskets.Count; i++)
            {
                if (baskets[i].stack == 0 || !baskets[i].gameObject.activeSelf)
                {
                    continue;
                }

                if (baskets[i].stack == playerShopItems[baskets[i].BasketShopIndex()].GetItem().itemStack)
                {
                    //�κ��丮�� �������� ���ŵǾ�� �ϴ� ��Ȳ
                    foreach (var _item in Inventory.Single.items)
                    {
                        if (_item.PrimaryCode == playerShopItems[baskets[i].BasketShopIndex()].GetItem().PrimaryCode)
                        {
                            baskets[i].GetBasketShopSlot().gameObject.SetActive(false);
                            //Inventory.Single.RemoveItem(_item);
                            RemoveItems.Add(_item);
                            Debug.Log("Remove Item :"+_item.itemName);
                            continue;
                        }
                        Debug.Log("not Remove Item :" + _item.itemName);
                    }
                }
                else
                {
                    //�κ��丮�� �������� stack�� ����Ǵ� ��Ȳ

                    foreach (var _item in Inventory.Single.items)
                    {
                        if (_item == playerShopItems[baskets[i].BasketShopIndex()].GetItem())
                        {
                            _item.itemStack -= baskets[i].stack;
                            return;
                        }
                    }
                    baskets[i].GetBasketShopSlot().SoldNow(baskets[i].stack); // ���ŵȸ�ŭ�� stack ����
                }
            }
            for (int i = 0; i < RemoveItems.Count; i++)
            {
                Inventory.Single.RemoveItem(RemoveItems[i]);
            }
            RemoveItems.Clear();
            //�ǸŰ� ���������� ����� ��Ȳ.
            GameMgr.playerData[0].player_Gold += basketsPrice;

            RefeshBaskets();
        }


        //�ŷ��� ����Ǹ� ����� �ݿ��Ǿ�� ��.
        Debug.Log("�ŷ� ���� �� ���: " + GameMgr.playerData[0].player_Gold);
        GameUiMgr.single.SliderChange();

    }

    void RefeshBaskets()
    {
        basketsPrice = 0;
        for (int i = 0; i < baskets.Count; i++)
        {
            baskets[i].gameObject.SetActive(false);
            baskets[i].GetBasketShopSlot().UseImgSet(false);
            calcPrice.text = basketsPrice.ToString();
        }
    }
}
