using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour
{
    public TextMeshProUGUI textName;
    public TextMeshProUGUI textTitle;
    public TextMeshProUGUI textDesc;
    //public TextMeshProUGUI textPower;
    public Image imgIcon;

    private float canvaseWidth;
    private RectTransform tooltipRect;

    public void SetupTooltip(string _name, string _title, string _desc,Sprite _img)
    {
        textName.text = _name;

        textTitle.text = _title;

        textDesc.text = _desc;

        imgIcon.sprite = _img;
    }

    /*public void SetupTooltip2(string name, string desc1, string desc2, int power, Sprite img)
    {
        textName.text = name;
        textDesc1.text = desc1;
        textDesc2.text = desc2;

        if (power == 0)
        {
            textPower.gameObject.SetActive(false);
        }
        else
        {
            textPower.text = power.ToString();
            textPower.gameObject.SetActive(true);
        }

        imgIcon.sprite = img;
    }*/
    private void Start()
    {
        canvaseWidth = GetComponentInParent<CanvasScaler>().referenceResolution.x * 0.5f;
        tooltipRect = GetComponent<RectTransform>();
    }
    private void Update()
    {
        MoveTooltip();
    }

    private void MoveTooltip()
    {
        transform.position = Input.mousePosition;
        // 04-15 ToolTip
        tooltipRect = GetComponent<RectTransform>();

        if (tooltipRect.anchoredPosition.x + tooltipRect.sizeDelta.x > canvaseWidth)
            tooltipRect.pivot = new Vector2(1, 0);
        else
            tooltipRect.pivot = new Vector2(0, 0);
    }
}
