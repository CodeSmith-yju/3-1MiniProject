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

        if (stack == 1)
        {
            item_Stack.gameObject.SetActive(false);
        }
        else
        {
            item_Stack.gameObject.SetActive(true);
        }

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //BattleManager.Instance.ui.tooltip.SetupTooltip(item.itemName, item.itemTitle, item.itemDesc, item.itemImage);
        BattleManager.Instance.ui.tooltip.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        BattleManager.Instance.ui.tooltip.gameObject.SetActive(false);
    }
}
