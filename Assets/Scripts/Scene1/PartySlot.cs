using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PartySlot : MonoBehaviour
{
    public int partySlotIndex;

    public PartyData partyData;
    public Ally player;
    public int index;

    public Image partyIcon;

    public string _strPartyName;
    public int intPartyCost;

    public TextMeshProUGUI text_Name;
    public TextMeshProUGUI text_Cost;
    public TextMeshProUGUI text_Lv;

    public GameObject block;
    public bool moveInChek;

    //05-22
    public Button btnMy;//��ȣ�ۿ��� �������� �߰�

    public Image classIcon;
    public Image attributeIcon;
    public void Init(PartyData _data)// PartyData (_data, _Prefab) �����ұ�.. ��Ƽ���ҽ�Mgr���� ���������ϰ� ������ ����ϴ½�����...�����
    {
        this.moveInChek = false;
        //Debug.Log("PartySlot Init");
        //Lv, Name, HP, Atk
        this.partyData = _data;
        this.partyIcon.sprite = _data.spPartyIcon;
        player = _data.player;
        index = _data.partyJobIndex;

        this._strPartyName = _data.strPartyName;
        this.text_Name.text = _strPartyName;

        this.intPartyCost = _data.cost;
        this.text_Cost.text = intPartyCost.ToString();

        this.text_Lv.text = "Lv "+_data.level.ToString();
        
        //this.classIcon.sprite = _data.jobIcon;
        //this.attributeIcon.sprite = _data.ElementalIcon;
        this.classIcon.sprite = GameUiMgr.single.entityIconRS.GetJobIcon(_data.jobClass);
        this.attributeIcon.sprite = GameUiMgr.single.entityIconRS.GetElementIcon(_data.Elemental);
    }

    public void OnClick()
    {
        AudioManager.single.PlaySfxClipChange(0);
        if (moveInChek == true)
        {
            GameUiMgr.single.RestorePartySlot(this.partySlotIndex);
            return;
        }

        if (GameUiMgr.single.ClickedPartySlot(this.partyData))
        {
            partyData.PartyDataSetMoveInCk(true);
            Debug.Log("Generate MoveInSlot");
            block.SetActive(true);
            moveInChek = true;
            btnMy.interactable = false;
        }

    }
    public void ReSetPartySlot()
    {
        this.partyData = null;
    }

    public PartyData GetPartyData()
    {
        Debug.Log("Get PartyData name: "+partyData.strPartyName);
        return this.partyData;
    }
    public void IsLoadPartySlot()
    {
        Debug.Log("***********************************Generate Load MoveInSlot");
        block.SetActive(true);
        moveInChek = true;
        btnMy.interactable = false;
    }
    public void IsLoadSetting()
    {
        block.SetActive(false);
        this.moveInChek = false;
        btnMy.interactable = true;
    }
}
