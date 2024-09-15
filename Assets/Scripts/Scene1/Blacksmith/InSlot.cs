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

    public void OnClickSelected()
    {
        Debug.Log("Clicked Btn_InSlot");
        //isInventory �� ���, �׸��� LeftTopRenovate�� Ȱ��ȭ�������ƴ���,
        if (isInventory)
        {
            SelectedPanel();
            //blacksmith.RenovateList[i].items.itemCode-4 = myItem.itemcode �϶� ���� �߰�.
            //selectedPanel.activeSelf == true �϶�, �����ϴ� �����۸�Ͽ��� ����
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
