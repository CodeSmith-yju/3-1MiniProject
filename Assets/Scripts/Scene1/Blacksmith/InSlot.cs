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
        DBConnector.LoadItemByCodeFromDB(_item.itemCode, ref _item.itemImage, ref _item.typeIcon);
        myItem = _item;
        imgIcon.sprite = _item.itemImage;
        textName.text = myItem.itemName;
        
        gameObject.SetActive(true);
        
        if (selectedPanel.activeSelf)
        {
            SelectedPanel();
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
                // ������ �׸��� �����ϸ� ���� �׸��� ������ ��� ������
                blacksmith.AllInvenUnSelect();

                // ������ �׸��� �ٽ� ������ ��� ���� �����ϰ� ��ȯ
                if (blacksmith.invenCk == invenItemIndex)
                {
                    Debug.Log("InventCk");
                    UnSelect();
                    blacksmith.sacrificeList.FirstItemMinus();
                    blacksmith.invenCk = -1;
                    return;
                }
                Debug.Log("Not InvenCk");
                //���� �׸� ����
                selectedPanel.SetActive(true);
                ParentBtn.interactable = true;
                ChildBtn.interactable = false;

                // sacrificeList�� ���õ� �׸� ������Ʈ
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
                // ���� �׸��� �����ϸ� ������ �׸��� ������ ��� ������
                blacksmith.AllInvenUnSelect();

                // ������ �׸��� �ٽ� ������ ��� ���� �����ϰ� ��ȯ
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
                //�� �κк��� ���θ������Ҽ�������
                // �����ϸ� ������ϸ���Ʈ �����ϰ�
                //�̸����� ������ְ�
                // 

                // ������ �׸��� �ٽ� ������ ��� ���� �����ϰ� ��ȯ
                if (blacksmith.invenCk == invenItemIndex)
                {
                    Debug.Log("InventCk");
                    UnSelect();
                    blacksmith.sacrificeList.FirstItemMinus();
                    blacksmith.invenCk = -1;
                    return;
                }
                Debug.Log("Not InvenCk");
                //���� �׸� ����
                selectedPanel.SetActive(true);
                ParentBtn.interactable = true;
                ChildBtn.interactable = false;

                // sacrificeList�� ���õ� �׸� ������Ʈ
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
        }
        
    }
    public void Highlight()
    {
        // ���̶���Ʈ �ѱ�/����
    }
    public void UnSelect()
    {
        Debug.Log("Run UnSelect");
        if (selectedPanel.activeSelf)
        {
            Debug.Log("active True ");
            selectedPanel.SetActive(false);

            ParentBtn.interactable = false;
            ChildBtn.interactable = true;
        }
    }

    public void OnClickSelected()
    {
        Debug.Log("Clicked Btn of InSlot");

        if (blacksmith.blacksmithState == BlacksmithState.Renovate)
        {
            //isInventory �� ���, �׸��� LeftTopRenovate�� Ȱ��ȭ�������ƴ���,
            if (isInventory)
            {
                //blacksmith.RenovateList[i].items.itemCode-4 = myItem.itemcode �϶� ���� �߰�.

                if (blacksmith.selectedCk != -1 && blacksmith.NowSelectedRenovateItem(blacksmith.selectedCk).itemCode - 4 == myItem.itemCode)//������Ͽ��� ���õ� �������� ���� ���.
                {
                    Debug.Log("Run Clicked Selected Panel");
                    SelectedPanel();
                }
                //selectedPanel.activeSelf == true �϶�, �����ϴ� �����۸�Ͽ��� ����
            }
            else
            {
                Debug.Log("Run Left Side: renovateItems");
                blacksmith.AllInvenUnSelect();
                if (blacksmith.selectedCk == renovateIndex)
                {
                    blacksmith.ReOrder();
                }
                else if (blacksmith.selectedCk != -1)
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
        }
        else if(blacksmith.blacksmithState == BlacksmithState.Upgrade)
        {

        }
        
    }
    
}
