using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SetDungeonTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Button btnMy;
    public int DungeonLevelScale;
    public bool _onoff;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (btnMy.interactable == true)
        {
            GameUiMgr.single.dungeonTooltip.gameObject.SetActive(true);
            GameUiMgr.single.dungeonTooltip.SetupTooltip(DungeonLevelScale, _onoff);
        }
        else
        {
            GameUiMgr.single.dungeonTooltip.gameObject.SetActive(false);
        }
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GameUiMgr.single.dungeonTooltip.gameObject.SetActive(false);
    }
}
