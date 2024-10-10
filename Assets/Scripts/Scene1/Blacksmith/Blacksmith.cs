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
    public SacrificeList sacrificeList;// 좌측상단 목록클릭시 얘가 활성, 해당아이템정보로 덮어씀

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
        //씬최초실행시 한번
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
                        int stk = sacrificeList.inspections[i].cnt; // 제거할 아이템의 개수
                        Debug.Log("\n\n아이템 강화에 필요한 아이템개수: " + sacrificeList.inspections[i].cnt + "\n\n");
                        for (int j = 0; j < Inventory.Single.items.Count; j++)
                        {
                            // 아이템 코드가 동일할 경우
                            if (Inventory.Single.items[j].itemCode == sacrificeList.inspections[i].GetItem().itemCode + 4)
                            {
                                // 현재 아이템의 stack이 제거할 개수보다 많거나 같을 경우
                                if (Inventory.Single.items[j].itemStack >= stk)
                                {
                                    // stk 수만큼 아이템 제거
                                    for (int k = 0; k < stk; k++)
                                    {
                                        Inventory.Single.RemoveItem(Inventory.Single.items[j]);
                                    }
                                    break; // 아이템을 다 제거했으므로 루프를 종료
                                }
                                else
                                {
                                    // 현재 stack이 부족할 경우 남은 stk에서 제거할 수 있는 만큼 제거
                                    stk -= Inventory.Single.items[j].itemStack;
                                    for (int k = 0; k < Inventory.Single.items[j].itemStack; k++)
                                    {
                                        Inventory.Single.RemoveItem(Inventory.Single.items[j]);

                                        // stk가 0이 되면 즉시 루프를 종료
                                        if (--stk == 0)
                                        {
                                            break;
                                        }
                                    }

                                    // stk가 0이 되면 전체 루프를 종료
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
                            Debug.Log("\n\n아이템 강화에 필요한 골드: "+ sacrificeList.inspections[i].cnt+"\n\n");
                        }
                        else
                        {
                            Debug.Log("골드부족한데스?");
                        }
                        break;

                    default:
                        Debug.Log("버그인데스");
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
            // 장비강화및 초기화

        }

        NowGold();
    }
    public void OpenBlacksmith()
    {
        // 상태 체크
        blacksmithState = BlacksmithState.Renovate;
        Renovates.SetActive(true);
        upgrades.SetActive(false);
        sacrificeList.gameObject.SetActive(false);
        NowGold();
        samenessCk = false;
        // TODO: invenList Add. (foreach Inventory.items[i].PK != BlackSmithSlot.item.PK) 문으로 기존invenList와비교하여 Destroy or As it is 
        
        // 최초실행 = 리스트가 없다면 초기화 == invenItems ??= new List<InSlot>();
        invenItems ??= new List<InSlot>();

        if (blacksmithState == BlacksmithState.Renovate)
        {
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
        }
    }
    public void OpenUpgrade()
    {
        // 상태 체크
        if (blacksmithState != BlacksmithState.Upgrade)
            blacksmithState = BlacksmithState.Upgrade;
        Debug.Log("++++++++Now State: " + blacksmithState);

        //화면 초기화
        Renovates.SetActive(false);
        upgrades.SetActive(true);
        preview.gameObject.SetActive(false);
        NowGold();

        // 리스트 초기화
        samenessCk = false;
        upgradeInvenItems ??= new List<InSlot>();

        // 무결성 확인 및 오브젝트 생성
        if (blacksmithState == BlacksmithState.Upgrade)
        {
            // 기존의 리스트가 존재한다면.
            if (upgradeInvenItems.Count > 0)
            {
                //기존의 리스트와 새로 만들어야할 리스트가 동일한지 확인
                if (Inventory.Single.items.Count == upgradeInvenItems.Count)
                {
                    // 일단 동일하다고 가정하고 시작
                    samenessCk = true;

                    for (int i = 0; i < upgradeInvenItems.Count; i++)
                    {
                        if (upgradeInvenItems[i].GetItem().PrimaryCode != Inventory.Single.items[i].PrimaryCode)
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
            //if (Inventory.Single.items[i].itemType != Item.ItemType.Consumables && Inventory.Single.items[i].itemType != Item.ItemType.Ect)
            // RS[8 ~ 11] 까지 일반장비
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

                // 생성된 슬롯 초기화
                slot.Init(this, _item, true);
                slot.invenItemIndex = invenItems.Count;
                // 생성된 슬롯을 리스트에 추가
                invenItems.Add(slot);
            }
        }
    }
    void MakeUpgradeInvenItems()
    {
        for (int i = 0; i < Inventory.Single.items.Count; i++)
        {
            //if (Inventory.Single.items[i].itemType != Item.ItemType.Consumables && Inventory.Single.items[i].itemType != Item.ItemType.Ect)
            // RS[8 ~ 11] 까지 일반장비
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

                // 생성된 슬롯 초기화
                slot.Init(this, _item, true);
                slot.invenItemIndex = upgradeInvenItems.Count;
                // 생성된 슬롯을 리스트에 추가
                upgradeInvenItems.Add(slot);
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

            slot.renovateIndex = renovateItems.Count;
            
            // 생성된 슬롯을 리스트에 추가
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
