using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum BlacksmithState
{
    None,
    Renovate,
    Upgrade
}
public class Blacksmith : MonoBehaviour
{
    public BlacksmithState blacksmithState = BlacksmithState.None;

    [SerializeField] InSlot Prefab;
    [SerializeField] Preview preview;

    [Header("Renovate")]
    public SacrificeList sacrificeList;// ������� ���Ŭ���� �갡 Ȱ��, �ش������������ ���

    [SerializeField] GameObject Renovates;
    [SerializeField] List<InSlot> renovateItems;
    [SerializeField] List<InSlot> invenItems;

    [SerializeField] Transform renovatetr;
    [SerializeField] Transform inventr;

    [Header("Upgrade")]
    public SacrificeList upgradesMaterials;//�����ϴܿ� ǥ�õ� ��ȭ��� ���

    [SerializeField] GameObject upgrades;
    [SerializeField] List<InSlot> upgradeInvenItems;
    [SerializeField] Transform upgradeInventr;

    [SerializeField] GameObject commitAni;
    [Header("***")]
    public int selectedCk = -1;
    public int invenCk = -1;

    public Button btn_Renovate;
    public Button btn_Upgrade;
    public Button btn_Commit;
    public TextMeshProUGUI tmp_nowGold;
    private void Start()
    {
        btn_Commit.interactable = false;
        //�����ʽ���� �ѹ�
        upgradesMaterials.gameObject.SetActive(false);
        MakeRenovateItems();
        //OpenBlacksmith();
    }
    /// <summary>
    /// 3�� ���� ������Ʈ�� Ȱ��ȭ�ߴٰ� ��Ȱ��ȭ�ϴ� �ڷ�ƾ
    /// </summary>
    public void CommitAnimation()
    {
        StartCoroutine(ActivateAndDeactivateObject());
    }
    private IEnumerator ActivateAndDeactivateObject()
    {
        // 1. ������Ʈ Ȱ��ȭ
        commitAni.SetActive(true);
        // 3. 3�� ���
        yield return new WaitForSeconds(2.1f);
        
        // 4. ������Ʈ ��Ȱ��ȭ
        commitAni.SetActive(false);
        btn_Commit.interactable = false;
        AllInvenUnSelect();
        Refresh();
        OpenUpgrade();
        btn_Commit.interactable = false;
        AudioManager.single.PlaySfxClipChange(3);
        yield break;
    }

    public void OnClickCommit()
    {
        if (blacksmithState == BlacksmithState.Renovate)
        {
            for (int i = 0; i < sacrificeList.inspections.Length; i++)
            {
                switch (i)
                {
                    case 0:
                        for (int j = 0; j < Inventory.Single.items.Count; j++)
                        { 
                            if (Inventory.Single.items[j].PrimaryCode.Equals(sacrificeList.inspections[i].ItemPK))
                            {
                                Inventory.Single.RemoveItem(Inventory.Single.items[j]);
                                break;
                            }
                        }
                        break;
                    case 1:
                        int stk = sacrificeList.inspections[i].cnt; // ������ �������� ����
                        Debug.Log("\n\n������ ��ȭ�� �ʿ��� �����۰���: " + sacrificeList.inspections[i].cnt + "\n\n");
                        for (int j = 0; j < Inventory.Single.items.Count; j++)
                        {
                            // ������ �ڵ尡 ������ ���
                            if (Inventory.Single.items[j].itemCode == sacrificeList.inspections[i].GetItem().itemCode + 4)
                            {
                                // ���� �������� stack�� ������ �������� ���ų� ���� ���
                                if (Inventory.Single.items[j].itemStack >= stk)
                                {
                                    // stk ����ŭ ������ ����
                                    for (int k = 0; k < stk; k++)
                                    {
                                        Inventory.Single.RemoveItem(Inventory.Single.items[j]);
                                    }
                                    break; // �������� �� ���������Ƿ� ������ ����
                                }
                                else
                                {
                                    // ���� stack�� ������ ��� ���� stk���� ������ �� �ִ� ��ŭ ����
                                    stk -= Inventory.Single.items[j].itemStack;
                                    for (int k = 0; k < Inventory.Single.items[j].itemStack; k++)
                                    {
                                        Inventory.Single.RemoveItem(Inventory.Single.items[j]);

                                        // stk�� 0�� �Ǹ� ��� ������ ����
                                        if (--stk == 0)
                                        {
                                            break;
                                        }
                                    }

                                    // stk�� 0�� �Ǹ� ��ü ������ ����
                                    if (stk == 0)
                                    {
                                        break;
                                    }
                                }
                            }
                        }

                        break;
                    case 2:
                        if (GameMgr.playerData[0].player_Gold >= sacrificeList.inspections[i].cnt)
                        {
                            GameMgr.playerData[0].player_Gold -= sacrificeList.inspections[i].cnt;
                            Debug.Log("\n\n������ ��ȭ�� �ʿ��� ���: "+ sacrificeList.inspections[i].cnt+"\n\n");
                        }
                        else
                        {
                            Debug.Log("�������ѵ���?");
                        }
                        break;

                    default:
                        Debug.Log("�����ε���");
                        break;
                }
            }

            Inventory.Single.AddItem(ItemResources.instance.itemRS[
                renovateItems[selectedCk].GetItem().itemCode
                ]);
        }
        else if (blacksmithState == BlacksmithState.Upgrade)
        {
            // ���ȭ�� �ʱ�ȭ
            for (int i = 0; i < upgradesMaterials.inspections.Length; i++)
            {
                switch (i)
                {
                    case 0:
                        Debug.Log("������ ��ȭ�� �ʿ��� �����۰���: " + upgradesMaterials.inspections[i].cnt + "");
                        for (int j = 0; j < Inventory.Single.items.Count; j++)
                        {
                            if (Inventory.Single.items[j].PrimaryCode.Equals(upgradesMaterials.inspections[i].ItemPK))
                            {
                                Debug.Log("Remove UpgradeMaterials Item: "+ upgradesMaterials.inspections[i].GetItem().itemName);
                                Item upgradedItem = Inventory.Single.items[j].UpgradeModifyPowerSet(Inventory.Single.items[j]);
                                Inventory.Single.AddItem(upgradedItem);
                                Inventory.Single.RemoveItem(Inventory.Single.items[j]);
                                break;
                            }
                        }
                        break;
                    case 1:
                        int stk = upgradesMaterials.inspections[i].cnt; // ������ �������� ����
                        Debug.Log("������ ��ȭ�� �ʿ��� �����۰���: " + upgradesMaterials.inspections[i].cnt + "");
                        for (int j = 0; j < Inventory.Single.items.Count; j++)
                        {
                            // ������ �ڵ尡 ������ ���
                            if (Inventory.Single.items[j].itemCode == upgradesMaterials.inspections[i].GetItem().itemCode + 4)
                            {
                                // ���� �������� stack�� ������ �������� ���ų� ���� ���
                                if (Inventory.Single.items[j].itemStack >= stk)
                                {
                                    // stk ����ŭ ������ ����
                                    for (int k = 0; k < stk; k++)
                                    {
                                        Inventory.Single.RemoveItem(Inventory.Single.items[j]);
                                    }
                                    break; // �������� �� ���������Ƿ� ������ ����
                                }
                                else
                                {
                                    // ���� stack�� ������ ��� ���� stk���� ������ �� �ִ� ��ŭ ����
                                    stk -= Inventory.Single.items[j].itemStack;
                                    for (int k = 0; k < Inventory.Single.items[j].itemStack; k++)
                                    {
                                        Inventory.Single.RemoveItem(Inventory.Single.items[j]);

                                        // stk�� 0�� �Ǹ� ��� ������ ����
                                        if (--stk == 0)
                                        {
                                            break;
                                        }
                                    }

                                    // stk�� 0�� �Ǹ� ��ü ������ ����
                                    if (stk == 0)
                                    {
                                        break;
                                    }
                                }
                            }
                        }

                        break;
                    case 2:
                        if (GameMgr.playerData[0].player_Gold >= upgradesMaterials.inspections[i].cnt)
                        {
                            GameMgr.playerData[0].player_Gold -= upgradesMaterials.inspections[i].cnt;
                            Debug.Log("������ ��ȭ�� �ʿ��� ���: " + upgradesMaterials.inspections[i].cnt);
                        }
                        else
                        {
                            Debug.Log("�������ѵ���?");
                        }
                        break;
                    default:
                        Debug.Log("�����ε���");
                        break;
                }
            }
            //Item _item = new Item().GenerateRandomItem(upgradesMaterials.inspections[0].GetItem().itemCode);
            //Inventory.Single.AddItem(_item.UpgradeModifyPowerSet(_item));
            Debug.Log("���۵� ������: " + Inventory.Single.items[Inventory.Single.items.Count - 1].itemPower);
        }
        CommitAnimation();
        GameUiMgr.single.GoldChanger();
    }
    public void OpenBlacksmith()
    {
        btn_Upgrade.interactable = true;
        btn_Renovate.interactable = false;
        // ���� üũ
        blacksmithState = BlacksmithState.Renovate;
        Renovates.SetActive(true);
        upgrades.SetActive(false);
        sacrificeList.gameObject.SetActive(false);
        NowGold();
        // TODO: invenList Add. (foreach Inventory.items[i].PK != BlackSmithSlot.item.PK) ������ ����invenList�ͺ��Ͽ� Destroy or As it is 
        ReOrder();
        // ���ʽ��� = ����Ʈ�� ���ٸ� �ʱ�ȭ == invenItems ??= new List<InSlot>();
        SetOnOffLights(false, -1);
        invenItems ??= new List<InSlot>();
        int _cnt = 0;
        if (blacksmithState == BlacksmithState.Renovate)
        {
            // ������ ����Ʈ�� �����Ѵٸ�.
            if (invenItems.Count > 0)
            {
                List<string> invenItemPKs = invenItems.Select(slot => slot.GetItem().PrimaryCode).ToList();

                // Inventory�� �����۵�� ���Ͽ� ��ġ�ϴ� ��� invenItemPKs���� ����
                for (int j = Inventory.Single.items.Count - 1; j >= 0; j--) // �������� ��ȸ
                {
                    if (!(Inventory.Single.items[j].itemType == Item.ItemType.Consumables || Inventory.Single.items[j].itemType == Item.ItemType.Ect))
                    {
                        _cnt++;
                    }

                    if (invenItemPKs.Contains(Inventory.Single.items[j].PrimaryCode))
                    {
                        // invenItemPKs���� �ش� ������ PK ����
                        invenItemPKs.Remove(Inventory.Single.items[j].PrimaryCode);
                    }
                }
                if (_cnt != invenItems.Count)
                {
                    Debug.Log("invenItem�� ������ �κ��丮�� ��ġ�����ʽ��ϴ�. ���� �����մϴ�.");
                    MakeInvenItems();
                }
                // invenItemPKs�� ������� Ȯ��
                if (invenItemPKs.Count == 0)
                {
                    Debug.Log("invenItemPKs�� ����ֽ��ϴ�. ���� �������� �ʽ��ϴ�.");
                    return; // ���� �������� ����
                }
                else
                {
                    Debug.Log("invenItemPKs�� ������� �ʽ��ϴ�. ���� �����մϴ�.");
                    MakeInvenItems(); // ���� ����
                }
            }
            else
            {
                MakeInvenItems();
            }
        }
    }
    public void OpenUpgrade()
    {
        btn_Upgrade.interactable = false;
        btn_Renovate.interactable = true;
        // ���� üũ
        if (blacksmithState != BlacksmithState.Upgrade)
            blacksmithState = BlacksmithState.Upgrade;
        Debug.Log("++++++++Now State: " + blacksmithState);

        //ȭ�� �ʱ�ȭ
        upgradeInvenItems ??= new List<InSlot>();
        Renovates.SetActive(false);
        upgrades.SetActive(true);
        ReOrder();
        NowGold();
        int _cnt = 0;

        // ���Ἲ Ȯ�� �� ������Ʈ ����
        if (blacksmithState == BlacksmithState.Upgrade)
        {
            if (upgradeInvenItems.Count > 0)// ������ ����Ʈ�� �����Ѵٸ�.
            {
                List<string> upgradeInvenItemPKs = upgradeInvenItems.Select(slot => slot.GetItem().PrimaryCode).ToList();

                // Inventory�� �����۵�� ���Ͽ� ��ġ�ϴ� ��� invenItemPKs���� ����
                for (int j = Inventory.Single.items.Count - 1; j >= 0; j--) // �������� ��ȸ
                {
                    if (!(Inventory.Single.items[j].itemType == Item.ItemType.Consumables || Inventory.Single.items[j].itemType == Item.ItemType.Ect))
                    {
                        _cnt++;
                    }

                    if (upgradeInvenItemPKs.Contains(Inventory.Single.items[j].PrimaryCode))
                    {
                        // invenItemPKs���� �ش� ������ PK ����
                        upgradeInvenItemPKs.Remove(Inventory.Single.items[j].PrimaryCode);
                    }
                }

                if (_cnt != upgradeInvenItems.Count)
                {
                    Debug.Log("UpgradeInvenItem�� ������ �κ��丮�� ��ġ�����ʽ��ϴ�. ���� �����մϴ�.");
                    MakeUpgradeInvenItems();
                }

                //������ ����Ʈ�� ���� �������� ����Ʈ�� �������� Ȯ��
                if (upgradeInvenItemPKs.Count == 0)
                {
                    Debug.Log("UpgradeInvenItemPKs�� ����ֽ��ϴ�. ���� �������� �ʽ��ϴ�.");
                    return; // ���� �������� ����
                }
                else
                {
                    // ������ ������ �ٸ��� ������ ���� ������ ��
                    MakeUpgradeInvenItems();
                }
            }
            else//���� ����� �ڵ�
            {
                MakeUpgradeInvenItems();
            }
        }
    }
    void Refresh()
    {
        NowGold();

        if (blacksmithState == BlacksmithState.Renovate)
        {
            MakeInvenItems();
        }
        else if(blacksmithState == BlacksmithState.Upgrade)
        {
            MakeUpgradeInvenItems();
        }
        
    }
    public void RefreshRenovate(int _index)
    {
        if (selectedCk == _index)
        {
            //������ �ٽ� �����ѰŸ�
            sacrificeList.gameObject.SetActive(false);
            AllInvenUnSelect();
        }/*
        else ������ �����غ��� �̰� ��� ������� �����ε�
        {
            //������ ���õȰ� ������ �װ� ������ ��Ȱ��ȭ
            //renovateItems[selectedCk];
        }*/

    }
    public void AllInvenUnSelect()
    {
        if (blacksmithState == BlacksmithState.Renovate)
        {
            for (int i = 0; i < invenItems.Count; i++)
            {
                invenItems[i].UnSelect();
                /*invenItems[i].selectedPanel.SetActive(false);
                invenItems[i].ParentBtn.interactable = false;
                invenItems[i].ChildBtn.interactable = true;*/
            }
        }
        else if(blacksmithState == BlacksmithState.Upgrade)
        {
            for (int i = 0; i < upgradeInvenItems.Count; i++)
            {
                upgradeInvenItems[i].UnSelect();
            }
            
        }
        
    }

    void MakeInvenItems()
    {
        if (invenItems != null)
        {
            Debug.Log("���� ����Ʈ �ı� �� �ʱ�ȭ �� ���� ����");
            foreach (var slot in invenItems)
            {
                slot.gameObject.SetActive(false);
                Destroy(slot.gameObject); // ���� ������Ʈ�� �ı�
            }
            invenItems.Clear(); // ����Ʈ ����
        }
        else
            Debug.Log("���� ���� or ����");

        for (int i = 0; i < Inventory.Single.items.Count; i++)
        {
            //if (Inventory.Single.items[i].itemType != Item.ItemType.Consumables && Inventory.Single.items[i].itemType != Item.ItemType.Ect)
            // RS[8 ~ 11] ���� �Ϲ����
            if (Inventory.Single.items[i].itemCode > 7 && Inventory.Single.items[i].itemCode < 12)
            {
                InSlot slot = Instantiate(Prefab, inventr);

                Item _item = new()
                {
                    itemCode = Inventory.Single.items[i].itemCode,
                    itemName = Inventory.Single.items[i].itemName,
                    itemType = Inventory.Single.items[i].itemType,
                    itemImage = Inventory.Single.items[i].itemImage,
                    itemPrice = Inventory.Single.items[i].itemPrice,
                    itemPower = Inventory.Single.items[i].itemPower,
                    itemDesc = Inventory.Single.items[i].itemDesc,
                    itemStack = Inventory.Single.items[i].itemStack,
                    modifyStack = Inventory.Single.items[i].modifyStack,
                    typeIcon = Inventory.Single.items[i].typeIcon,
                    PrimaryCode = Inventory.Single.items[i].PrimaryCode,
                };

                // ������ ���� �ʱ�ȭ
                slot.Init(this, _item, true);
                slot.invenItemIndex = invenItems.Count;
                // ������ ������ ����Ʈ�� �߰�
                invenItems.Add(slot);
            }
        }
    }
    void MakeUpgradeInvenItems()
    {
        if (upgradeInvenItems != null)
        {
            Debug.Log("���� ����Ʈ �ı� �� �ʱ�ȭ �� ���� ����");
            foreach (var slot in upgradeInvenItems)
            {
                slot.gameObject.SetActive(false);
                Destroy(slot.gameObject); // ���� ������Ʈ�� �ı�
            }
            upgradeInvenItems.Clear(); // ����Ʈ ����
        }
        else
            Debug.Log("���� ���� or ����");

        for (int i = 0; i < Inventory.Single.items.Count; i++)
        {
            //if (Inventory.Single.items[i].itemType != Item.ItemType.Consumables && Inventory.Single.items[i].itemType != Item.ItemType.Ect)
            // RS[8 ~ 11] ���� �Ϲ����
            if (Inventory.Single.items[i].itemCode > 11 && Inventory.Single.items[i].itemCode < 16)
            {
                InSlot slot = Instantiate(Prefab, upgradeInventr);

                Item _item = new()
                {
                    itemCode = Inventory.Single.items[i].itemCode,
                    itemName = Inventory.Single.items[i].itemName,
                    itemType = Inventory.Single.items[i].itemType,
                    itemImage = Inventory.Single.items[i].itemImage,
                    itemPrice = Inventory.Single.items[i].itemPrice,
                    itemPower = Inventory.Single.items[i].itemPower,
                    itemDesc = Inventory.Single.items[i].itemDesc,
                    itemStack = Inventory.Single.items[i].itemStack,
                    modifyStack = Inventory.Single.items[i].modifyStack,
                    typeIcon = Inventory.Single.items[i].typeIcon,
                    PrimaryCode = Inventory.Single.items[i].PrimaryCode,
                };

                // ������ ���� �ʱ�ȭ
                slot.Init(this, _item, true);
                slot.invenItemIndex = upgradeInvenItems.Count;
                // ������ ������ ����Ʈ�� �߰�
                upgradeInvenItems.Add(slot);
            }
        }
    }

    void MakeRenovateItems()
    {
        for (int i = 8; i < 12; i++)
        {
            //12 ~ 15 = ��ȭ���
            //16 ~ 19 = ��ȭ������
            InSlot slot = Instantiate(Prefab, renovatetr);

            // ������ ���� �ʱ�ȭ
            slot.Init(this, ItemResources.instance.itemRS[i + 4], false);

            slot.renovateIndex = renovateItems.Count;
            
            // ������ ������ ����Ʈ�� �߰�
            renovateItems.Add(slot);
        }
    }

    public void ShowInspections(Item _item)
    {
        if (blacksmithState == BlacksmithState.Renovate)
        {
            sacrificeList.gameObject.SetActive(true);
            for (int i = 0; i < sacrificeList.inspections.Length; i++)
            {
                sacrificeList.inspections[i].Init(_item);
            }
            sacrificeList.ChangeInspectionsVlue(null);

            if (sacrificeList.AllCk() == true)
            {
                btn_Commit.interactable = true;
            }
        }
        else if(blacksmithState == BlacksmithState.Upgrade)
        {
            Debug.Log("���׷��̵� ������ �������� ���Ἲ �˻���");
            upgradesMaterials.gameObject.SetActive(true);

            for (int i = 0; i < upgradesMaterials.inspections.Length; i++)
            {
                Debug.Log("��ȭ��� ��ġ��: "+i+"ȸ");
                /*if (i == 0)
                {
                    if (upgradesMaterials.inspections[i].GetItem() == null)
                    {
                        Debug.Log("���ʽ���");
                        upgradesMaterials.inspections[i].Init(_item);
                    }
                    else
                    {
                        Debug.Log("�ߺ�����");
                        upgradesMaterials.inspections[i].Refresh();
                        upgradesMaterials.inspections[i].Init(_item);
                    }
                }*/
                //upgradesMaterials.inspections[i].Init(_item);
                upgradesMaterials.inspections[i].Init(_item);
            }
            upgradesMaterials.ChangeInspectionsVlue(_item);

            if (upgradesMaterials.AllCk() == true)
            {
                btn_Commit.interactable = true;
            }
            else
            {
                btn_Commit.interactable = false;
            }
        }
    }
    public Item NowSelectedRenovateItem(int _index)
    {
        return renovateItems[_index].GetItem();
    }
    public Item NowSelectedUpgradeItem(int _index)
    {
        return upgradeInvenItems[_index].GetItem();
    }
    public void ReOrder()
    {
        selectedCk = -1;
        invenCk = -1;

        if (blacksmithState == BlacksmithState.Renovate)
        {
            sacrificeList.gameObject.SetActive(false);
        }
        else if(blacksmithState == BlacksmithState.Upgrade)
        {
            preview.gameObject.SetActive(false);
            upgradesMaterials.gameObject.SetActive(false);
        }
        
    }
    public void SetOnOffLights(bool _onoff, int _index)
    {
        Debug.Log("Off Highlight index : "+_index +" type is :" + _onoff);

        if (_onoff)
        {
            if (_index == -1)
            {
                Debug.Log("Error on -1");
            }
            else
            {
                renovateItems[_index].Highlight(true);
            }
        }
        else
        {
            if (_index == -1)
            {
                for (int i = 0; i < renovateItems.Count; i++)
                {
                    renovateItems[i].Highlight(false);
                }

            }
            else
            {
                renovateItems[_index].Highlight(false);
            }
        }
        
    }
    void NowGold()
    {
        tmp_nowGold.text = GameMgr.playerData[0].player_Gold.ToString();
    }

    public void ShowPreView(Item _item)
    {
        preview.gameObject.SetActive(true);
        preview.Init(_item);
        ShowInspections(_item);
    }
    public void UnShowPreView()
    {
        preview.gameObject.SetActive(false);
        upgradesMaterials.gameObject.SetActive(false);
        upgradesMaterials.ChangeInspectionsVlue(null);
        //ShowInspections(_item);
    }
    public void Test()
    {
        //upgradesMaterials.inspections.refresh();
    }
}
