using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;
using UnityEngine.UI;
using static Unity.VisualScripting.Metadata;

public class Blacksmith : MonoBehaviour
{
    [SerializeField] private InSlot Prefab;

    [SerializeField] private List<InSlot> renovateItems;
    [SerializeField] private List<InSlot> invenItems;

    [SerializeField] private Transform renovatetr;
    [SerializeField] private Transform inventr;
    bool samenessCk;

    public int selectedCk = -1;
    public int invenCk = -1;
    public SacrificeList sacrificeList;// ������� ���Ŭ���� �갡 Ȱ��, �ش������������ ���

    public Button btn_Commit;

    private void Start()
    {
        btn_Commit.interactable = false;
        //�����ʽ���� �ѹ�
        sacrificeList.gameObject.SetActive(false);
        MakeRenovateItems();
        //OpenBlacksmith();
    }
    void OpenBlacksmith()
    {
        samenessCk = false;
        // TODO: invenList Add. (foreach Inventory.items[i].PK != BlackSmithSlot.item.PK) ������ ����invenList�ͺ��Ͽ� Destroy or As it is 

        // ���ʽ��� = ����Ʈ�� ���ٸ� �ʱ�ȭ == invenItems ??= new List<InSlot>();
        if (invenItems == null)
        {
            invenItems = new List<InSlot>();
        }

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
        // TODO: revnovateItems
    }

    void Refresh()
    {
        int cnt = invenItems.Count;
        for (int i = 0; i < cnt; i++)
        {
            invenItems[i].gameObject.SetActive(false);
            Destroy(invenItems[i]);
        }
        invenItems.Clear();
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
            //�ϴ� ��� ��µǰ� �ߴµ� ���������Ѿ������� �ڵ常�ȴٴ��� �ϴ½����� ���� �ʿ�
            //if (Inventory.Single.items[i].itemType != Item.ItemType.Consumables && Inventory.Single.items[i].itemType != Item.ItemType.Ect)
            // RS[8 ~ 11] ���� �Ϲ����
            if (Inventory.Single.items[i].itemCode > 7 && Inventory.Single.items[i].itemCode < 12)
            {
                InSlot slot = Instantiate(Prefab, inventr);

                // ������ ���� �ʱ�ȭ
                slot.Init(this, Inventory.Single.items[i], true);

                // ������ ������ ����Ʈ�� �߰�
                invenItems.Add(slot);

                slot.invenItemIndex = invenItems.Count - 1;
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
            
            // ������ ������ ����Ʈ�� �߰�
            renovateItems.Add(slot);

            // �߰� ���� ����
            slot.renovateIndex = renovateItems.Count - 1;
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
    }
    public Item NowSelectedRenovateItem(int _index)
    {
        return renovateItems[_index].GetItem();
    }
    //TODO: LeftTop Side one = Renovate
    //TODO: LeftTop Side two = Enhance
    //TODO: Click(OneTab || Two Tab)
    //TODO: LeftBottom Side View List

}
