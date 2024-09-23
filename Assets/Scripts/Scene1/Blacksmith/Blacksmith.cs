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
    public SacrificeList sacrificeList;// 좌측상단 목록클릭시 얘가 활성, 해당아이템정보로 덮어씀

    public Button btn_Commit;

    private void Start()
    {
        btn_Commit.interactable = false;
        //씬최초실행시 한번
        sacrificeList.gameObject.SetActive(false);
        MakeRenovateItems();
        //OpenBlacksmith();
    }
    void OpenBlacksmith()
    {
        samenessCk = false;
        // TODO: invenList Add. (foreach Inventory.items[i].PK != BlackSmithSlot.item.PK) 문으로 기존invenList와비교하여 Destroy or As it is 

        // 최초실행 = 리스트가 없다면 초기화 == invenItems ??= new List<InSlot>();
        if (invenItems == null)
        {
            invenItems = new List<InSlot>();
        }

        // 기존의 리스트가 존재한다면.
        if (invenItems.Count > 0)
        {
            //기존의 리스트와 새로 만들어야할 리스트가 동일한지 확인
            if (Inventory.Single.items.Count == invenItems.Count)
            {
                // 일단 동일하다고 가정하고 시작
                samenessCk = true;

                for (int i = 0; i < invenItems.Count; i++)
                {
                    if (invenItems[i].GetItem().PrimaryCode != Inventory.Single.items[i].PrimaryCode)
                    {
                        // 다른 아이템이 발견되면 false로 설정
                        samenessCk = false; 
                        break; // 차이가 발견되면 더 이상 비교할 필요가 없음
                    }
                }

                if (!samenessCk)
                {
                    // 기존 아이템을 모두 삭제하고 리스트 초기화
                    Refresh();
                }
            }
            else
            {
                // 아이템 개수가 다르면 무조건 새로 만들어야 함
                Refresh();
            }
        }
        //새로 만드는 코드
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
            //기존거 다시 선택한거면
            sacrificeList.gameObject.SetActive(false);
            AllInvenUnSelect();
        }/*
        else 곰곰히 생각해보니 이건 없어도 상관없는 구조인듯
        {
            //기존에 선택된거 있으면 그거 선택을 비활성화
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
            //일단 장비만 출력되게 했는데 개조가능한아이템의 코드만된다던가 하는식으로 수정 필요
            //if (Inventory.Single.items[i].itemType != Item.ItemType.Consumables && Inventory.Single.items[i].itemType != Item.ItemType.Ect)
            // RS[8 ~ 11] 까지 일반장비
            if (Inventory.Single.items[i].itemCode > 7 && Inventory.Single.items[i].itemCode < 12)
            {
                InSlot slot = Instantiate(Prefab, inventr);

                // 생성된 슬롯 초기화
                slot.Init(this, Inventory.Single.items[i], true);

                // 생성된 슬롯을 리스트에 추가
                invenItems.Add(slot);

                slot.invenItemIndex = invenItems.Count - 1;
            }
        }
    }

    void MakeRenovateItems()
    {
        for (int i = 8; i < 12; i++)
        {
            //12 ~ 15 = 강화장비
            //16 ~ 19 = 강화장비재료
            InSlot slot = Instantiate(Prefab, renovatetr);

            // 생성된 슬롯 초기화
            slot.Init(this, ItemResources.instance.itemRS[i + 4], false);
            
            // 생성된 슬롯을 리스트에 추가
            renovateItems.Add(slot);

            // 추가 정보 대입
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
