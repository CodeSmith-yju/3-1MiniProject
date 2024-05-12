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

    public PartySlot(PartyData _data)// PartyData (_data, _Prefab) �����ұ�.. ��Ƽ���ҽ�Mgr���� ���������ϰ� ������ ����ϴ½�����...�����
    {
        this.partyData = _data;
        this.partyIcon.sprite = _data.spPartyIcon;
        this.text_Name.text = _data.strName;
        this.text_Cost.text = _data.cost.ToString();
    }

    public void OnClick()
    {
        //if (true){ } ���� ����ٰ� PlayerList���� < 3 �϶��� �޼���� ����ǰ� �ƴϸ� return ~   
        block.SetActive(true);
        //TODO: Ŭ���Ǿ����� PlayerPartyList�� �� partyData�� �Ѱ� Add�� �ְ�
    }
}
