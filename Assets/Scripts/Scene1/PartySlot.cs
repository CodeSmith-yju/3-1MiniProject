using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PartySlot : MonoBehaviour
{
    public int partySlotIndex;

    public PartyData partyData;

    public Image partyIcon;
    public TextMeshProUGUI text_Name;
    public TextMeshProUGUI text_Cost;

    public GameObject block;
    public bool moveInChek;
    public void Init(PartyData _data)// PartyData (_data, _Prefab) �����ұ�.. ��Ƽ���ҽ�Mgr���� ���������ϰ� ������ ����ϴ½�����...�����
    {
        Debug.Log("PartySlot Init");
        //Lv, Name, HP, Atk
        this.partyData = _data;
        this.partyIcon.sprite = _data.spPartyIcon;

        //this.text_Name.text = _data.strName;
        this.text_Cost.text = _data.cost.ToString();
    }

    public void OnClick()
    {
        if (moveInChek == false)
        {
            block.SetActive(true);

            moveInChek = true;
            //TODO: Ŭ���Ǿ����� PlayerPartyList�� �� partyData�� �Ѱ� Add�� �ְ�, ���α׷�����
            GameUiMgr.single.ClickedPartySlot(this.partyData);
            //���⿡��  ��Ƽ���Կ� Add���ִµ� ���� �����ϴ� ����̾ƴ϶� ���Կ� �� ������ �״�� ������� �����θ���鼭 Block�� false�� ���ְ� �� ���¿��� �ٽ� Ŭ���Ǹ� ����Ʈ���������鼭 �� ��ü�� block�� false�� ���ֱ�.
        }
        else
        {
            //�ϴ� MoveIn Slot List�� PartySlot�� Ŭ���Ǹ� �ش� MoveInSlot List�� Ȱ��ȭ�� ������ ��Ȱ��ȭ/���� �ǰ�
            GameUiMgr.single.poolMoveInSlot[partySlotIndex].gameObject.SetActive(false);
            GameUiMgr.single.poolMoveInSlot.Remove(GameUiMgr.single.poolMoveInSlot[partySlotIndex]);
            //Body�� PartyBordSlot�� �ٽ� ��ȣ�ۿ밡���ϰ� �Ǿ����.
            block.SetActive(false);
        }

    }
    public void BaseStat()
    {
        
    }
}
