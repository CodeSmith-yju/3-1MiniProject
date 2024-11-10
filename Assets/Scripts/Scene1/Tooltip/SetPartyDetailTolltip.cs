using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class SetPartyDetailTolltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public PartyIconState partyIconState;
    [SerializeField] PartyData partyData;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (SceneManager.GetActiveScene().name == "Town")
        {
            if (partyData != null)
            {
                GameUiMgr.single.partyDetailTooltip.gameObject.SetActive(true);
                GameUiMgr.single.partyDetailTooltip.SetupTooltip(partyIconState, partyData);
            }
        }
        else//Battle or Tutorial
        {

        }
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (SceneManager.GetActiveScene().name == "Town")
        {
            GameUiMgr.single.partyDetailTooltip.gameObject.SetActive(false);
        }
        else
        {

        }
    }

    public void SetPartyData(PartyData _partyData)
    {
        partyData = _partyData;
    }
}