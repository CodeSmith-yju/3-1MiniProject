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
    public Button btnMy;//상호작용을 막기위해 추가

    public Image classIcon;
    public Image attributeIcon;
    public void Init(PartyData _data)// PartyData (_data, _Prefab) 으로할까.. 파티리소스Mgr만들어서 프리펩등록하고 아이콘 등록하는식으로...고민중
    {
        this.moveInChek = false;
        Debug.Log("PartySlot Init");
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

        this.classIcon.sprite = _data.jobIcon;
        this.attributeIcon.sprite = _data.ElementalIcon;
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
}
