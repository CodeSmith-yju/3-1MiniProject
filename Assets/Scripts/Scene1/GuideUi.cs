using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GuideState
{
    Stamina,
    Shop,
    Renovate,
    Upgrade,
    None
}
public class GuideUi : MonoBehaviour
{
    [SerializeField] GameObject obj_Gui;
    [SerializeField] GameObject obj_Contrast;
    [SerializeField] Image img_Guide;
    [SerializeField] Sprite[] obj_GuideSprites;

    public int _cnt = 0;
    public GuideState guideState = GuideState.None;

    public void Refresh_GuideImg()
    {
        obj_Gui.SetActive(false);
        img_Guide.gameObject.SetActive(false);
        obj_Contrast.SetActive(false);
    }
    public string ChangeGuideImg(GuideState _guideState)
    {
        if (!obj_Gui.activeSelf)
        {
            obj_Gui.SetActive(true);
        }
        if (_guideState == GuideState.None)
        {
            Refresh_GuideImg();
            return string.Empty;
        }
        if (!obj_Contrast.activeSelf)
        {
            obj_Contrast.SetActive(true);
        }
        if (!img_Guide.gameObject.activeSelf)
        {
            img_Guide.gameObject.SetActive(true);
        }
        img_Guide.sprite = obj_GuideSprites[(int)_guideState];
        return string.Empty;
    }
}
