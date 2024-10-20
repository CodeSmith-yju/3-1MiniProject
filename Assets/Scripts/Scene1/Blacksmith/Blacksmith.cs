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
    public SacrificeList sacrificeList;// 좌측상단 목록클릭시 얘가 활성, 해당아이템정보로 덮어씀

    [SerializeField] GameObject Renovates;
    [SerializeField] List<InSlot> renovateItems;
    [SerializeField] List<InSlot> invenItems;

    [SerializeField] Transform renovatetr;
    [SerializeField] Transform inventr;

    [Header("Upgrade")]
    public SacrificeList upgradesMaterials;//좌측하단에 표시될 강화재료 목록

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
        //씬최초실행시 한번
        upgradesMaterials.gameObject.SetActive(false);
        MakeRenovateItems();
        //OpenBlacksmith();
    }
    /// <summary>
    /// 3초 동안 오브젝트를 활성화했다가 비활성화하는 코루틴
    /// </summary>
    public void CommitAnimation()
    {
        StartCoroutine(ActivateAndDeactivateObject());
    }
    private IEnumerator ActivateAndDeactivateObject()
    {
        // 1. 오브젝트 활성화
        commitAni.SetActive(true);
        // 3. 3초 대기
        yield return new WaitForSeconds(2.1f);
        
        // 4. 오브젝트 비활성화
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
        }
        else if (blacksmithState == BlacksmithState.Upgrade)
        {
            // 장비강화및 초기화
            for (int i = 0; i < upgradesMaterials.inspections.Length; i++)
            {
                switch (i)
                {
                    case 0:
                        Debug.Log("아이템 강화에 필요한 아이템개수: " + upgradesMaterials.inspections[i].cnt + "");
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
                        int stk = upgradesMaterials.inspections[i].cnt; // 제거할 아이템의 개수
                        Debug.Log("아이템 강화에 필요한 아이템개수: " + upgradesMaterials.inspections[i].cnt + "");
                        for (int j = 0; j < Inventory.Single.items.Count; j++)
                        {
                            // 아이템 코드가 동일할 경우
                            if (Inventory.Single.items[j].itemCode == upgradesMaterials.inspections[i].GetItem().itemCode + 4)
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
                        if (GameMgr.playerData[0].player_Gold >= upgradesMaterials.inspections[i].cnt)
                        {
                            GameMgr.playerData[0].player_Gold -= upgradesMaterials.inspections[i].cnt;
                            Debug.Log("아이템 강화에 필요한 골드: " + upgradesMaterials.inspections[i].cnt);
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
            //Item _item = new Item().GenerateRandomItem(upgradesMaterials.inspections[0].GetItem().itemCode);
            //Inventory.Single.AddItem(_item.UpgradeModifyPowerSet(_item));
            Debug.Log("제작된 아이템: " + Inventory.Single.items[Inventory.Single.items.Count - 1].itemPower);
        }
        CommitAnimation();
        GameUiMgr.single.GoldChanger();
    }
    public void OpenBlacksmith()
    {
        btn_Upgrade.interactable = true;
        btn_Renovate.interactable = false;
        // 상태 체크
        blacksmithState = BlacksmithState.Renovate;
        Renovates.SetActive(true);
        upgrades.SetActive(false);
        sacrificeList.gameObject.SetActive(false);
        NowGold();
        // TODO: invenList Add. (foreach Inventory.items[i].PK != BlackSmithSlot.item.PK) 문으로 기존invenList와비교하여 Destroy or As it is 
        ReOrder();
        // 최초실행 = 리스트가 없다면 초기화 == invenItems ??= new List<InSlot>();
        SetOnOffLights(false, -1);
        invenItems ??= new List<InSlot>();
        int _cnt = 0;
        if (blacksmithState == BlacksmithState.Renovate)
        {
            // 기존의 리스트가 존재한다면.
            if (invenItems.Count > 0)
            {
                List<string> invenItemPKs = invenItems.Select(slot => slot.GetItem().PrimaryCode).ToList();

                // Inventory의 아이템들과 비교하여 일치하는 경우 invenItemPKs에서 제거
                for (int j = Inventory.Single.items.Count - 1; j >= 0; j--) // 역순으로 순회
                {
                    if (!(Inventory.Single.items[j].itemType == Item.ItemType.Consumables || Inventory.Single.items[j].itemType == Item.ItemType.Ect))
                    {
                        _cnt++;
                    }

                    if (invenItemPKs.Contains(Inventory.Single.items[j].PrimaryCode))
                    {
                        // invenItemPKs에서 해당 아이템 PK 제거
                        invenItemPKs.Remove(Inventory.Single.items[j].PrimaryCode);
                    }
                }
                if (_cnt != invenItems.Count)
                {
                    Debug.Log("invenItem의 개수가 인벤토리와 일치하지않습니다. 새로 생성합니다.");
                    MakeInvenItems();
                }
                // invenItemPKs가 비었는지 확인
                if (invenItemPKs.Count == 0)
                {
                    Debug.Log("invenItemPKs가 비어있습니다. 새로 생성하지 않습니다.");
                    return; // 새로 생성하지 않음
                }
                else
                {
                    Debug.Log("invenItemPKs가 비어있지 않습니다. 새로 생성합니다.");
                    MakeInvenItems(); // 새로 생성
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
        // 상태 체크
        if (blacksmithState != BlacksmithState.Upgrade)
            blacksmithState = BlacksmithState.Upgrade;
        Debug.Log("++++++++Now State: " + blacksmithState);

        //화면 초기화
        upgradeInvenItems ??= new List<InSlot>();
        Renovates.SetActive(false);
        upgrades.SetActive(true);
        ReOrder();
        NowGold();
        int _cnt = 0;

        // 무결성 확인 및 오브젝트 생성
        if (blacksmithState == BlacksmithState.Upgrade)
        {
            if (upgradeInvenItems.Count > 0)// 기존의 리스트가 존재한다면.
            {
                List<string> upgradeInvenItemPKs = upgradeInvenItems.Select(slot => slot.GetItem().PrimaryCode).ToList();

                // Inventory의 아이템들과 비교하여 일치하는 경우 invenItemPKs에서 제거
                for (int j = Inventory.Single.items.Count - 1; j >= 0; j--) // 역순으로 순회
                {
                    if (!(Inventory.Single.items[j].itemType == Item.ItemType.Consumables || Inventory.Single.items[j].itemType == Item.ItemType.Ect))
                    {
                        _cnt++;
                    }

                    if (upgradeInvenItemPKs.Contains(Inventory.Single.items[j].PrimaryCode))
                    {
                        // invenItemPKs에서 해당 아이템 PK 제거
                        upgradeInvenItemPKs.Remove(Inventory.Single.items[j].PrimaryCode);
                    }
                }

                if (_cnt != upgradeInvenItems.Count)
                {
                    Debug.Log("UpgradeInvenItem의 개수가 인벤토리와 일치하지않습니다. 새로 생성합니다.");
                    MakeUpgradeInvenItems();
                }

                //기존의 리스트와 새로 만들어야할 리스트가 동일한지 확인
                if (upgradeInvenItemPKs.Count == 0)
                {
                    Debug.Log("UpgradeInvenItemPKs가 비어있습니다. 새로 생성하지 않습니다.");
                    return; // 새로 생성하지 않음
                }
                else
                {
                    // 아이템 개수가 다르면 무조건 새로 만들어야 함
                    MakeUpgradeInvenItems();
                }
            }
            else//새로 만드는 코드
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
            Debug.Log("기존 리스트 파괴 및 초기화 후 새로 생성");
            foreach (var slot in invenItems)
            {
                slot.gameObject.SetActive(false);
                Destroy(slot.gameObject); // 슬롯 오브젝트를 파괴
            }
            invenItems.Clear(); // 리스트 비우기
        }
        else
            Debug.Log("최초 실행 or 버그");

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
        if (upgradeInvenItems != null)
        {
            Debug.Log("기존 리스트 파괴 및 초기화 후 새로 생성");
            foreach (var slot in upgradeInvenItems)
            {
                slot.gameObject.SetActive(false);
                Destroy(slot.gameObject); // 슬롯 오브젝트를 파괴
            }
            upgradeInvenItems.Clear(); // 리스트 비우기
        }
        else
            Debug.Log("최초 실행 or 버그");

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
            Debug.Log("업그레이드 를위한 재료아이템 무결성 검사중");
            upgradesMaterials.gameObject.SetActive(true);

            for (int i = 0; i < upgradesMaterials.inspections.Length; i++)
            {
                Debug.Log("강화재료 배치중: "+i+"회");
                /*if (i == 0)
                {
                    if (upgradesMaterials.inspections[i].GetItem() == null)
                    {
                        Debug.Log("최초실행");
                        upgradesMaterials.inspections[i].Init(_item);
                    }
                    else
                    {
                        Debug.Log("중복실행");
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
