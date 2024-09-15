using DarkPixelRPGUI.Scripts.UI.Equipment;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InSlot : MonoBehaviour
{
    [SerializeField] Blacksmith blacksmith;
    [SerializeField] Item myItem;
    bool isInventory;

    [Header("Show UI")]
    public Image imgIcon;
    public TextMeshProUGUI textName;
    
    public GameObject selectedPanel;
    //Image HightRight

    [Header("Btn UI")]
    public Button ParentBtn;
    public Button ChildBtn;
    
    public void Init(Blacksmith _blacksmith, Item _item, bool _isInventory)
    {
        blacksmith = _blacksmith;
        isInventory = _isInventory;
        myItem = _item;
        imgIcon.sprite = _item.itemImage;
        textName.text = myItem.itemName;
        
        gameObject.SetActive(true);
        
        if (selectedPanel.activeSelf)
        {
            SelectedPanel();
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
        if (isInventory)//Right Side
        {
            if (selectedPanel.activeSelf == true)
            {
                selectedPanel.SetActive(false);

                ParentBtn.interactable = false;
                ChildBtn.interactable = true;
            }
            else
            {
                selectedPanel.SetActive(true);

                ParentBtn.interactable = true;
                ChildBtn.interactable = false;
            }
        }
        else//Left Side
        {
            if (selectedPanel.activeSelf == true)
            {
                selectedPanel.SetActive(false); 

                //만약 하이라이트기능이 켜지면 밑에 두개 false 되고, 꺼지면 다시 true
                ParentBtn.interactable = true;
                ChildBtn.interactable = true;
            }
        }
    }
    public void Highlight()
    {
        // 하이라이트 켜기/끄기
    }

    public void OnClickSelected()
    {
        Debug.Log("Clicked Btn_InSlot");
        //isInventory 일 경우, 그리고 LeftTopRenovate가 활성화중인지아닌지,
        if (isInventory)
        {
            SelectedPanel();
            //blacksmith.RenovateList[i].items.itemCode-4 = myItem.itemcode 일때 재료로 추가.
            //selectedPanel.activeSelf == true 일때, 좌측하단 아이템목록에서 제거
        }
        else
        {

        }
        //invenItems
        //SelectedImg.SetActive(true);
        //if Area.item != null, else
        //LeftDown Area.items == this.item, Area.item = this.item

        //renoItems
        //if for (int i =0; i<RenovateItems.count; i++) { inventory.items.itemcode == RenovateItems.item.itemcode - 4 }, HightLight.ActiveTrue
    }
}
