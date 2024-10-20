using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler/*, IBeginDragHandler, IDragHandler, IEndDragHandler*/
{
    //public int slotnum;
    public Item item;
    public Image itemIcon;
    public bool wearChek = false;
    public bool usability = false;

    [Header("Display")]
    public TextMeshProUGUI itemStack;// 소모품 개수 표시
    public TextMeshProUGUI modifyStack;// 강화 수치 표시
    //private bool isDraging;

    public void UpdateSloutUI()
    {
        if (itemIcon != null && itemStack != null && modifyStack != null)
        {
            itemIcon.sprite = item.itemImage;
            if (item.itemType == Item.ItemType.Consumables || item.itemType == Item.ItemType.Ect)
            {
                itemStack.text = item.itemStack.ToString();
                itemStack.gameObject.SetActive(true);
            }
            else if (item.itemType == Item.ItemType.Equipment_Helmet || item.itemType == Item.ItemType.Equipment_Weapon || item.itemType == Item.ItemType.Equipment_Boots || item.itemType == Item.ItemType.Equipment_Arrmor)
            {
                if (item.modifyStack > 0)
                {
                    modifyStack.text = "+" + item.modifyStack.ToString();
                    modifyStack.gameObject.SetActive(true);
                }
            }

            if (!itemIcon.gameObject.activeSelf)
            {
                itemIcon.gameObject.SetActive(true);
            }
        }
    }
    public void RemoveSlot()
    {
        item = null;
        if (itemIcon != null)
        {
            itemIcon.gameObject.SetActive(false);
        }
        if (itemStack != null)
        {
            itemStack.gameObject.SetActive(false);
        }
        if (modifyStack != null)
        {
            modifyStack.gameObject.SetActive(false);
        }
    }
    public void CallGUISlot()
    {
        if (item.itemType != Item.ItemType.Consumables && item.itemType != Item.ItemType.Ect)
        {
            Debug.Log("Slot.cs - CallGUISlot(): "+item.itemType);// 정상작동함
/*
            GameUiMgr.single.nowSlot = this;
            GameUiMgr.single.dragIcon = GameUiMgr.single.nowSlot.itemIcon;*/
            //GameUiMgr.single.GetMoseItem(this);
            
        }
    }
    /*public void OnBeginDrag(PointerEventData eventData)
    {
        if (item != null && item.isDraggable == true)
        {
            GameUiMgr.single.OnBeginDrag(eventData);
            GameUiMgr.single.dragIndex = item.itemIndex;
            isDraging = true;
        }
    }
    public void OnDrag(PointerEventData eventData)
    {
        GameUiMgr.single.OnDrag(eventData);
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        GameUiMgr.single.OnEndDrag(eventData);
        isDraging = false;
    }*/

    //ToolTip & Item Use
    public void OnPointerEnter(PointerEventData eventData)//Move In ItemSlot
    {
        if (GameUiMgr.single.activeInventory == true /*&& isDraging == false*/)
        {
            if (item != null)
            {
                //GameUiMgr.single.dragIndex = item.itemIndex;
                if (item.itemImage != null)
                {
                    GameUiMgr.single.tooltip.gameObject.SetActive(true);
                    GameUiMgr.single.tooltip.SetupTooltip(item);
                }
                else
                    GameUiMgr.single.tooltip.gameObject.SetActive(false);
            }
        }
        else
        {
            GameUiMgr.single.tooltip.gameObject.SetActive(false);
        }
        

    }
    public void OnPointerExit(PointerEventData eventData)//Move Out ItemSlot
    {
        GameUiMgr.single.tooltip.gameObject.SetActive(false);
    }

    public void OnPointerUp(PointerEventData eventData)//Click ItemSlot
    {
        if (this.usability == true  && this.wearChek == false)//인벤토리 좌측의 장착아이템 목록이면서, 장비를 장착 중이 아닐때 상호작용불가능하게함. = 장비칸에 장비없을때 건드려지면안된다고
        {
            return;
        }

        if (item != null)
        {
            Debug.Log("Run Methode: Clicked ItemSlot");
            AudioManager.single.PlaySfxClipChange(0);
            Debug.Log("Run SFX sound index: 0");
        }
        //int||float 아이템효과 담을 변수 선언
        if (this.wearChek == true)// 장착중인 장비와 상호작용
        {
            if (!(item.itemType == Item.ItemType.Ect) && !(item.itemType == Item.ItemType.Consumables))
            {
                GameUiMgr.single.nowSlot = this;
                GameUiMgr.single.textEquipPanel.text = "장비를 해제 하시겠습니까?";//OK버튼 클릭했을때 다른효과가 나와야하는데 생각조금 더 해봐야함
                GameUiMgr.single.addEquipPanel.gameObject.SetActive(true);

                Debug.Log("Run Methode: Take Off Item, Item Code: " + this.item.itemCode); 
            }
            else
            {
                return;
            }
            return;
        }


        if (item != null )//인벤토리의 아이템과 상호작용
        {
            switch (item.itemType)
            {
                case Item.ItemType.Ect:
                    break;
                case Item.ItemType.Consumables:
                    if (item.itemCode == 0)
                    {
                        GameUiMgr.single.nowSlot = this;
                        GameUiMgr.single.popUp.SetPopUp(item.itemName+"을 사용 하시겠습니까?",PopUpState.SnPotion);
                        GameUiMgr.single.popUp.gameObject.SetActive(true);
                    }
                    break;
                default:
                    GameUiMgr.single.nowSlot = this;

                    GameUiMgr.single.textEquipPanel.text = "장비를 장착 하시겠습니까?";
                    Debug.Log("gggggggggg" + item.itemType);
                    GameUiMgr.single.addEquipPanel.gameObject.SetActive(true);
                    //클릭하면 팝업창 출력되고. 팝업창에서 확인 클릭하면 실행되어야함
                    //isDraging = true;
                    GameUiMgr.single.tooltip.gameObject.SetActive(false);
                    break;
            }

        }

        /*if (item.itemType == Item.ItemType.Equipment_Arrmor)
        {
            bool isUse = item.Use();
            if (isUse)
            {
                Inventory.single.RemoveItem(slotnum);
            }
            Debug.Log("This is Arrmor");
        }*/

    }

    public void UpdateStack()
    {
        if (item.itemStack == 0)
        {
            GameUiMgr.single.RedrawSlotUI();
        }
        if (itemStack.gameObject.activeSelf)
        {
            itemStack.text = item.itemStack.ToString();
        }
        if (modifyStack.gameObject.activeSelf)
        {
            modifyStack.text = "+"+item.modifyStack.ToString();
        }
    }
    public void WearSlotRefresh()
    {
        modifyStack.text = "0";
        modifyStack.gameObject.SetActive(false);
    }
    public void SetSlotItem(Item _item)
    {
        item = _item;
    }
}
