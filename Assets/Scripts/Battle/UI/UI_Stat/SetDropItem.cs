using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SetDropItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Item item;
    [SerializeField] Image item_Icon;
    [SerializeField] TMP_Text item_Stack;

    public void Init(Item item, int stack)
    {
        this.item = item;
        item_Icon.sprite = item.itemImage;
        item_Stack.text = stack.ToString();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //BattleManager.Instance.ui.
    }

    public void OnPointerExit(PointerEventData eventData)
    {

    }
}
