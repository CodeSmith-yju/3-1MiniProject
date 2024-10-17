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
                blacksmith.AllInvenUnSelect();
                //�� �κк��� ���θ������Ҽ�������
                // �����ϸ� ������ϸ���Ʈ �����ϰ�
                //�̸����� ������ְ�
                // 

                // ������ �׸��� �ٽ� ������ ��� ���� �����ϰ� ��ȯ
                if (blacksmith.invenCk == invenItemIndex)
                {
                    Debug.Log("�ߺ�Ŭ�� ");
                    blacksmith.invenCk = -1;
                    blacksmith.UnShowPreView();
                    return;
                }
                Debug.Log("Not InvenCk");
                //���� �׸� Ȱ��
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
        // ���̶���Ʈ �ѱ�/����
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
            Debug.Log("�ʱ�ȭ");
           
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
                    blacksmith.SetOnOffLights(false, -1);
                }
                else if (blacksmith.selectedCk != -1)
                {
                    //���� ���õȰ� ������, ������ ���õȰ� �� ����ġ��� 
                    blacksmith.RefreshRenovate(renovateIndex);
                    blacksmith.SetOnOffLights(false, -1);

                    //���������Ѱ� üũ
                    blacksmith.selectedCk = renovateIndex;
                    blacksmith.ShowInspections(myItem);
                    blacksmith.SetOnOffLights(true, renovateIndex);
                }
                else
                {
                    //���õȰ� ����? ��� ����
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
                //�̰Ŵ� ������Ͼ��� 
            }
        }
        
    }
    
}
