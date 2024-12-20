using UnityEngine;
using UnityEngine.EventSystems;

public class SetIconTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public PartyIconState partyIconState;

    public void OnPointerEnter(PointerEventData eventData)
    {
        GameUiMgr.single.partytooltip.gameObject.SetActive(true);
        GameUiMgr.single.partytooltip.SetupTooltip(partyIconState, gameObject.transform.parent.gameObject.transform.parent.gameObject.transform.parent.GetComponent<PartySlot>().partyData);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GameUiMgr.single.partytooltip.gameObject.SetActive(false);
    }
}
