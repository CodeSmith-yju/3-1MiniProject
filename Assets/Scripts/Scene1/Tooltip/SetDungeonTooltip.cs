using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SetDungeonTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public int DungeonLevelScale;

    public void OnPointerEnter(PointerEventData eventData)
    {
        GameUiMgr.single.dungeonTooltip.gameObject.SetActive(true);
        GameUiMgr.single.dungeonTooltip.SetupTooltip(DungeonLevelScale);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GameUiMgr.single.dungeonTooltip.gameObject.SetActive(false);
    }
}
