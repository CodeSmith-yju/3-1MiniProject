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
    public TextMeshProUGUI myGoldText;

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
    private int[] numbers = { 0, 6, 8, 9, 10, 11 };

    // ���� ���� ��� �Լ�
    public int GetRandomNumber()
    {
        // Random.Range�� 0���� numbers.Length - 1������ �ε����� ��ȯ
        int randomIndex = UnityEngine.Random.Range(0, numbers.Length);
        return numbers[randomIndex];
    }
    public void RefreshShopItems()
    {
        //Debug.Log("Make Shop Items");
        //�� �Ʒ��� for�� ���������� GameUIMgr�� �������ҰŰ��� ���⼭�����ϱ⿣����? ����� ����̸�����
        for (int i = 0; i < 4; i++)
        {
            // ���� �������� ����
            int randomIndex = GetRandomNumber();
            if (i == 0)
                randomIndex = 0;

            Item originalItem = ItemResources.instance.itemRS[randomIndex];
            

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
                typeIcon = originalItem.typeIcon,
            };

            if (newItem.itemType == Item.ItemType.Consumables)
            {
                newItem.itemStack = Random.Range(1, 10);
            }/*
            else
            {
                newItem.modifyStack = Random.Range(1, 4);
                newItem.UpgradeModifyPowerSet(newItem);
            }*/

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
    public void ReLoadShopItems(List<Item> _loadShopitem)//��������
    {
        if (_loadShopitem == null || _loadShopitem.Count == 0)
        {
            Debug.LogWarning("No shop items to load.");
            return;
        }

        // ���� ���� �ʱ�ȭ
        if (shopSlots.Count > 0)
        {
            foreach (var slot in shopSlots)
            {
                Destroy(slot.gameObject);
            }
            shopSlots.Clear();
        }

        Debug.Log("Run ReLoad shopItem");

        for (int i = 0; i < _loadShopitem.Count; i++)
        {
            DBConnector.LoadItemByCodeFromDB(_loadShopitem[i].itemCode, ref _loadShopitem[i].itemImage, ref _loadShopitem[i].typeIcon);
            ShopSlot slot = Instantiate(slotPrefab, tfBuy);
            slot.ShopMgrSet(this);
            slot.Init(_loadShopitem[i], ShopState.BUY);
            slot.shopIndex = i;
            shopSlots.Add(slot);

            Debug.Log($"Shop Item Loaded: {_loadShopitem[i].itemName}");
        }
    }
    /*public void ReLoadShopItems(List<Item> _loadShopitem)
    {
        // ���� ó�� �߰�
        if (_loadShopitem == null || _loadShopitem.Count == 0)
        {
            Debug.LogWarning("No shop items to load.");
            return;
        }

        if (shopSlots.Count > 0)
        {
            Debug.Log("Existing shop slots found.");
            // ���⿡�� shopSlots�� �ʱ�ȭ�ϰų� �ʿ��ϴٸ� ó���� �� ����.
        }

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
    }*/

    /*public void ReLoadShopItems(List<Item> _loadShopitem)
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
    }*/
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
                PrimaryCode = Inventory.Single.items[i].PrimaryCode,
                typeIcon = originalItem.typeIcon,
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
        GoldSet();
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

                if (baskets[i].stack == playerShopItems[baskets[i].BasketShopIndex()].GetItem().itemStack)// && baskets[i].stack > 1)
                {
                    //�κ��丮�� �������� ���ŵǾ�� �ϴ� ��Ȳ
                    foreach (var _item in Inventory.Single.items)
                    {
                        if (_item.PrimaryCode == playerShopItems[baskets[i].BasketShopIndex()].GetItem().PrimaryCode)
                        {
                            baskets[i].GetBasketShopSlot().gameObject.SetActive(false);
                            //Inventory.Single.RemoveItem(_item);
                            RemoveItems.Add(_item);
                            /*for (int j = 0; j < baskets[i].GetBasketShopSlot().GetItem().itemStack; j++)
                            {
                                RemoveItems.Add(_item);
                            }*/
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
                }//�̻����ִµ����� �Ƹ� �ȿð���
            }
            
            for (int i = 0; i < RemoveItems.Count; i++)
            {
                int _stack = 0;
                if (RemoveItems[i].itemStack > 1)
                {
                    _stack = RemoveItems[i].itemStack;
                    for (int j = 0; j < _stack; j++)
                    {
                        Inventory.Single.RemoveItem(RemoveItems[i]);
                    }
                }
                else
                    Inventory.Single.RemoveItem(RemoveItems[i]);
            }
            RemoveItems.Clear();
            //�ǸŰ� ���������� ����� ��Ȳ.
            GameMgr.playerData[0].player_Gold += basketsPrice;
            RefeshBaskets();
        }

        GameUiMgr.single.RedrawSlotUI();
        //�ŷ��� ����Ǹ� ����� �ݿ��Ǿ�� ��.
        Debug.Log("�ŷ� ���� �� ���: " + GameMgr.playerData[0].player_Gold);
        GameUiMgr.single.SliderChange();
        GoldSet();
        GameUiMgr.single.GoldChanger();
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
    void GoldSet()
    {
        myGoldText.text = GameMgr.playerData[0].player_Gold.ToString();
    }
}
