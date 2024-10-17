using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InSlot : MonoBehaviour
{
    [SerializeField] Blacksmith blacksmith;
    [SerializeField] Item myItem;
    bool isInventory;
    public int renovateIndex;
    public int invenItemIndex;

    [Header("Show UI")]
    public Image imgIcon;
    public TextMeshProUGUI textName;
    public TextMeshProUGUI textStack;

    public GameObject selectedPanel;
    public GameObject HightRight;

    [Header("Btn UI")]
    public Button ParentBtn;
    public Button ChildBtn;
    
    public void Init(Blacksmith _blacksmith, Item _item, bool _isInventory)
    {
        renovateIndex = -1;
        invenItemIndex = -1;

        blacksmith = _blacksmith;
        isInventory = _isInventory;
        DBConnector.LoadItemByCodeFromDB(_item.itemCode, ref _item.itemImage, ref _item.typeIcon);
        myItem = _item;
        imgIcon.sprite = _item.itemImage;
        textName.text = myItem.itemName;
        textStack.text = "+"+myItem.modifyStack.ToString();

        if (myItem.modifyStack < 1)
            textStack.gameObject.SetActive(false);
        else
            textStack.gameObject.SetActive(true);

        gameObject.SetActive(true);
        
        if (selectedPanel.activeSelf)
        {
            if (blacksmith.blacksmithState == BlacksmithState.Renovate)
            {
                SelectedPanel();
            }
            else
            {
                selectedPanel.SetActive(false);
            }
            
        }
        if (!isInventory)
        {
            ParentBtn.interactable = true;
            ChildBtn.interactable = false;
        }
    }

    public Item GetItem()
    {
        if (myItem == null)
        {
            Debug.Log("myItem is Null");
            return null;
        }
        return myItem;
    }

    public void SelectedPanel()
    {
        if (blacksmith.blacksmithState == BlacksmithState.Renovate)
        {
            if (isInventory)//Right Side
            {
                // 오른쪽 항목을 선택하면 왼쪽 항목의 선택을 모두 해제함
                blacksmith.AllInvenUnSelect();

                // 동일한 항목을 다시 선택한 경우 선택 해제하고 반환
                if (blacksmith.invenCk == invenItemIndex)
                {
                    Debug.Log("InventCk");
                    UnSelect();
                    blacksmith.sacrificeList.FirstItemMinus();
                    blacksmith.invenCk = -1;
                    return;
                }
                Debug.Log("Not InvenCk");
                //현재 항목 선택
                selectedPanel.SetActive(true);
                ParentBtn.interactable = true;
                ChildBtn.interactable = false;

                // sacrificeList에 선택된 항목 업데이트
                blacksmith.sacrificeList.ChangeInspectionsVlue(myItem);
                if (blacksmith.sacrificeList.inpectionRedy == true)
                {
                    blacksmith.btn_Commit.interactable = true;
                }
                else
                {
                    blacksmith.btn_Commit.interactable = false;
                }
                blacksmith.invenCk = invenItemIndex;
            }
            else//Left Side
            {
                // 왼쪽 항목을 선택하면 오른쪽 항목의 선택을 모두 해제함
                blacksmith.AllInvenUnSelect();

                // 동일한 항목을 다시 선택한 경우 선택 해제하고 반환
                if (blacksmith.selectedCk == renovateIndex)
                {
                    Debug.Log("More Click");
                    selectedPanel.SetActive(false);
                    blacksmith.selectedCk = -1;
                    return;
                }

                if (selectedPanel.activeSelf)
                {
                    selectedPanel.SetActive(true);
                }
            }
        }
        else if (blacksmith.blacksmithState == BlacksmithState.Upgrade)
        {
            if (isInventory)//Right Side
            {
                blacksmith.AllInvenUnSelect();
                //이 부분부터 새로만들어야할수도있음
                // 선택하면 우측목록리스트 선택하고
                //미리보기 만들어주고
                // 

                // 동일한 항목을 다시 선택한 경우 선택 해제하고 반환
                if (blacksmith.invenCk == invenItemIndex)
                {
                    Debug.Log("중복클릭 ");
                    blacksmith.invenCk = -1;
                    blacksmith.UnShowPreView();
                    return;
                }
                Debug.Log("Not InvenCk");
                //현재 항목 활성
                selectedPanel.SetActive(true);
                ParentBtn.interactable = true;
                ChildBtn.interactable = false;

                blacksmith.ShowPreView(myItem);

                blacksmith.invenCk = invenItemIndex;
            }
        }
        
    }
    public void Highlight(bool _onoff)
    {
        // 하이라이트 켜기/끄기
        HightRight.SetActive(_onoff);
    }
    public void UnSelect()
    {
        Debug.Log("Run UnSelect");
        if (blacksmith.blacksmithState == BlacksmithState.Renovate)
        {
            if (selectedPanel.activeSelf)
            {
                Debug.Log("active True");
                selectedPanel.SetActive(false);

                ParentBtn.interactable = false;
                ChildBtn.interactable = true;
            }
        }
        else if (blacksmith.blacksmithState == BlacksmithState.Upgrade)
        {
            selectedPanel.SetActive(false);
            Debug.Log("초기화");
           
            ParentBtn.interactable = false;
            ChildBtn.interactable = true;
        }
        
    }

    public void OnClickSelected()
    {
        Debug.Log("Clicked Btn of InSlot");

        if (blacksmith.blacksmithState == BlacksmithState.Renovate)
        {
            //isInventory 일 경우, 그리고 LeftTopRenovate가 활성화중인지아닌지,
            if (isInventory)
            {
                //blacksmith.RenovateList[i].items.itemCode-4 = myItem.itemcode 일때 재료로 추가.

                if (blacksmith.selectedCk != -1 && blacksmith.NowSelectedRenovateItem(blacksmith.selectedCk).itemCode - 4 == myItem.itemCode)//좌측목록에서 선택된 아이템이 있을 경우.
                {
                    Debug.Log("Run Clicked Selected Panel");
                    SelectedPanel();
                }
                //selectedPanel.activeSelf == true 일때, 좌측하단 아이템목록에서 제거
            }
            else
            {
                Debug.Log("Run Left Side: renovateItems");
                blacksmith.AllInvenUnSelect();
                if (blacksmith.selectedCk == renovateIndex)
                {
                    blacksmith.ReOrder();
                    blacksmith.SetOnOffLights(false, -1);
                }
                else if (blacksmith.selectedCk != -1)
                {
                    //먼저 선택된게 있으면, 기존의 선택된거 다 갈아치우고 
                    blacksmith.RefreshRenovate(renovateIndex);
                    blacksmith.SetOnOffLights(false, -1);

                    //내가선택한걸 체크
                    blacksmith.selectedCk = renovateIndex;
                    blacksmith.ShowInspections(myItem);
                    blacksmith.SetOnOffLights(true, renovateIndex);
                }
                else
                {
                    //선택된게 없다? 즉시 실행
                    blacksmith.selectedCk = renovateIndex;
                    blacksmith.ShowInspections(myItem);
                    blacksmith.SetOnOffLights(true, renovateIndex);
                }
            }
        }
        else if(blacksmith.blacksmithState == BlacksmithState.Upgrade)
        {
            Debug.Log("now state is Upgrade");
            if (isInventory)
            {
                SelectedPanel();
            }
            else
            {
                Debug.Log("Error");
                //이거는 실행될일없음 
            }
        }
        
    }
    
}
