using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public enum StatIconState
{
    None,
    HP,
    MP,
    Def,
    Spd,
    Atk,
    AtkSpd,
    AtkRng,
}
public class SetPartyDetailTolltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Data")]
    [SerializeField] PartyDesc partyDesc;

    [Header("State")]
    public StatIconState statIconState;
    public PartyIconState partyIconState;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (SceneManager.GetActiveScene().name == "Town")
        {
            GameUiMgr.single.partyDetailTooltip.gameObject.SetActive(true);
            if (partyDesc != null)
            {
                GameUiMgr.single.partyDetailTooltip.SetUpToolTip(partyIconState, partyDesc);
            }
            else
            {
                GameUiMgr.single.partyDetailTooltip.SetUpToolTip(statIconState);
            }
            
        }
        else//Battle or Tutorial
        {
            BattleManager.Instance.ui.partyDetailTooltip.gameObject.SetActive(true);
            if (partyDesc != null)
            {
                //Debug.Log("Not Null");
                BattleManager.Instance.ui.partyDetailTooltip.SetUpToolTip(partyIconState, partyDesc);
            }
            else
            {
                //Debug.Log("Null");
                BattleManager.Instance.ui.partyDetailTooltip.SetUpToolTip(statIconState);
            }
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
            BattleManager.Instance.ui.partyDetailTooltip.gameObject.SetActive(false);
        }
    }

    public void SetPartyDesc(PartyDesc _partyDesc)
    {
        partyDesc = _partyDesc;
    }
}