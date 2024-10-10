using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditorInternal.Profiling.Memory.Experimental;
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
    bool samenessCk;

    [Header("Upgrade")]
    public SacrificeList upgradesMaterials;

    [SerializeField] GameObject upgrades;
    [SerializeField] List<InSlot> upgradeInvenItems;
    [SerializeField] Transform upgradeInventr;


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

            OpenBlacksmith();
            AllInvenUnSelect();
            ReOrder();

            Refresh();
            
        }
        else if (blacksmithState == BlacksmithState.Upgrade)
        {
            // ���ȭ�� �ʱ�ȭ

        }

        NowGold();
    }
    public void OpenBlacksmith()
    {
        // ���� üũ
        blacksmithState = BlacksmithState.Renovate;
        Renovates.SetActive(true);
        upgrades.SetActive(false);
        sacrificeList.gameObject.SetActive(false);
        NowGold();
        samenessCk = false;
        // TODO: invenList Add. (foreach Inventory.items[i].PK != BlackSmithSlot.item.PK) ������ ����invenList�ͺ��Ͽ� Destroy or As it is 
        
        // ���ʽ��� = ����Ʈ�� ���ٸ� �ʱ�ȭ == invenItems ??= new List<InSlot>();
        invenItems ??= new List<InSlot>();

        if (blacksmithState == BlacksmithState.Renovate)
        {
            // ������ ����Ʈ�� �����Ѵٸ�.
            if (invenItems.Count > 0)
            {
                //������ ����Ʈ�� ���� �������� ����Ʈ�� �������� Ȯ��
                if (Inventory.Single.items.Count == invenItems.Count)
                {
                    // �ϴ� �����ϴٰ� �����ϰ� ����
                    samenessCk = true;

                    for (int i = 0; i < invenItems.Count; i++)
                    {
                        if (invenItems[i].GetItem().PrimaryCode != Inventory.Single.items[i].PrimaryCode)
                        {
                            // �ٸ� �������� �߰ߵǸ� false�� ����
                            samenessCk = false;
                            break; // ���̰� �߰ߵǸ� �� �̻� ���� �ʿ䰡 ����
                        }
                    }

                    if (!samenessCk)
                    {
                        // ���� �������� ��� �����ϰ� ����Ʈ �ʱ�ȭ
                        Refresh();
                    }
                }
                else
                {
                    // ������ ������ �ٸ��� ������ ���� ������ ��
                    Refresh();
                }
            }
            //���� ����� �ڵ�
            if (!samenessCk)
            {
                MakeInvenItems();
            }
        }
    }
    public void OpenUpgrade()
    {
        // ���� üũ
        if (blacksmithState != BlacksmithState.Upgrade)
            blacksmithState = BlacksmithState.Upgrade;
        Debug.Log("++++++++Now State: " + blacksmithState);

        //ȭ�� �ʱ�ȭ
        Renovates.SetActive(false);
        upgrades.SetActive(true);
        preview.gameObject.SetActive(false);
        NowGold();

        // ����Ʈ �ʱ�ȭ
        samenessCk = false;
        upgradeInvenItems ??= new List<InSlot>();

        // ���Ἲ Ȯ�� �� ������Ʈ ����
        if (blacksmithState == BlacksmithState.Upgrade)
        {
            // ������ ����Ʈ�� �����Ѵٸ�.
            if (upgradeInvenItems.Count > 0)
            {
                //������ ����Ʈ�� ���� �������� ����Ʈ�� �������� Ȯ��
                if (Inventory.Single.items.Count == upgradeInvenItems.Count)
                {
                    // �ϴ� �����ϴٰ� �����ϰ� ����
                    samenessCk = true;

                    for (int i = 0; i < upgradeInvenItems.Count; i++)
                    {
                        if (upgradeInvenItems[i].GetItem().PrimaryCode != Inventory.Single.items[i].PrimaryCode)
                        {
                            // �ٸ� �������� �߰ߵǸ� false�� ����
                            samenessCk = false;
                            break; // ���̰� �߰ߵǸ� �� �̻� ���� �ʿ䰡 ����
                        }
                    }

                    if (!samenessCk)
                    {
                        // ���� �������� ��� �����ϰ� ����Ʈ �ʱ�ȭ
                        Refresh();
                    }
                }
                else
                {
                    // ������ ������ �ٸ��� ������ ���� ������ ��
                    Refresh();
                }
            }
            //���� ����� �ڵ�
            if (!samenessCk)
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
            int cnt = invenItems.Count;
            for (int i = 0; i < cnt; i++)
            {
                invenItems[i].gameObject.SetActive(false);
                Destroy(invenItems[i]);
            }
            invenItems.Clear();
        }
        else if(blacksmithState == BlacksmithState.Upgrade)
        {
            int cnt = upgradeInvenItems.Count;
            for (int i = 0; i < cnt; i++)
            {
                upgradeInvenItems[i].gameObject.SetActive(false);
                Destroy(upgradeInvenItems[i]);
            }
            upgradeInvenItems.Clear();
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
        for (int i = 0; i < invenItems.Count; i++)
        {
            invenItems[i].UnSelect();
            /*invenItems[i].selectedPanel.SetActive(false);

            invenItems[i].ParentBtn.interactable = false;
            invenItems[i].ChildBtn.interactable = true;*/
        }
    }

    void MakeInvenItems()
    {
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
    public Item NowSelectedRenovateItem(int _index)
    {
        return renovateItems[_index].GetItem();
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
        upgradesMaterials.ChangeInspectionsVlue(_item);
    }
    public void UnShowPreView()
    {
        preview.gameObject.SetActive(false);
    }
}
