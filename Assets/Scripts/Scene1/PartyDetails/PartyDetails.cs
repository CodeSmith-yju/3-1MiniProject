using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PartyDetails : MonoBehaviour
{
    /*public List<GameObject> Btns = new();
    public Transform tf_Content;
    public TextMeshProUGUI text;*/


    public void Init(PartyData _pd)//GuiMgr의 List<PartySlot>인 lastDefartual의 partyData를통해 목록을만들것.
    {
        //if (Btns.Count < 1)
    }

    public void OpenCharactorDetails(PartyData _pd)
    {

    }

    public void Test()
    {
        //gmaeObject.SetActive(false);
        Debug.Log("외부영역 클릭");
    }
}
