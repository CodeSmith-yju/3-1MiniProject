using DarkPixelRPGUI.Scripts.UI.Equipment;
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
    
    public GameObject selectedPanel;
    //Image HightRight

    [Header("Btn UI")]
    public Button ParentBtn;
    public Button ChildBtn;
    
    public void Init(Blacksmith _blacksmith, Item _item, bool _isInventory)
    {
        renovateIndex = -1;
        invenItemIndex = -1;

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
            blacksmith.sacrificeList.FirstItemMinus();

            if (blacksmith.invenCk == invenItemIndex)
            {
                blacksmith.AllInvenUnSelect();
                return;
            }

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

                blacksmith.sacrificeList.ChangeInspectionsVlue(myItem);
            }

            blacksmith.invenCk = invenItemIndex;
        }
        else//Left Side
        {
            if (selectedPanel.activeSelf == true)
            {
                selectedPanel.SetActive(false); 

                //���� ���̶���Ʈ����� ������ �ؿ� �ΰ� false �ǰ�, ������ �ٽ� true
                ParentBtn.interactable = true;
                ChildBtn.interactable = true;
            }
        }
    }
    public void Highlight()
    {
        // ���̶���Ʈ �ѱ�/����
    }
    public void SelectedOff()
    {
        if (isInventory)//Right Side
        {
            selectedPanel.SetActive(true);

            ParentBtn.interactable = false;
            ChildBtn.interactable = true;
        }
    }
    public void OnClickSelected()
    {
        Debug.Log("Clicked Btn of InSlot");
        //isInventory �� ���, �׸��� LeftTopRenovate�� Ȱ��ȭ�������ƴ���,
        if (isInventory)
        {
            //blacksmith.RenovateList[i].items.itemCode-4 = myItem.itemcode �϶� ���� �߰�.

            if (blacksmith.selectedCk != -1 && blacksmith.NowSelectedRenovateItem(blacksmith.selectedCk).itemCode-4 == myItem.itemCode )//������Ͽ��� ���õ� �������� ���� ���.
            {
                SelectedPanel();
            }
            //selectedPanel.activeSelf == true �϶�, �����ϴ� �����۸�Ͽ��� ����
        }
        else
        {
            if (blacksmith.selectedCk != -1)
            {
                //���� ���õȰ� ������, ������ ���õȰ� �� ����ġ��� 
                blacksmith.RefreshRenovate(renovateIndex);

                //���������Ѱ� üũ
                blacksmith.selectedCk = renovateIndex;
                blacksmith.ShowInspections(myItem);
            }
            else
            {
                //���õȰ� ����? ��� ����
                blacksmith.selectedCk = renovateIndex;
                blacksmith.ShowInspections(myItem);
            }
        }
        //invenItems
        //SelectedImg.SetActive(true);
        //if Area.item != null, else
        //LeftDown Area.items == this.item, Area.item = this.item

        //renoItems
        //if for (int i =0; i<RenovateItems.count; i++) { inventory.items.itemcode == RenovateItems.item.itemcode - 4 }, HightLight.ActiveTrue
    }
    public void UnSelect()
    {
        if (selectedPanel.activeSelf)
        {
            selectedPanel.SetActive(false);
        }
    }
}
