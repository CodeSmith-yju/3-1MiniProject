using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Battle(UI)")]
    public GameObject player_Statbar;
    public GameObject mini_Map;
    public GameObject mini_Map_Big;
    public GameObject item_Bar;
    public ItemUse item_Slot_Prefabs;
    public GameObject party_List;
    public GameObject battleStart;
    public GameObject out_Portal;
    public GameObject next_Room_Popup;
    public GameObject room_UI;
    public GameObject item_Use_UI;
    public GameObject option_UI;
    public bool isOpenUI = false;

    [Header("Skill_Log")]
    public GameObject log_View;
    public GameObject log_Prefabs;
    public Transform log_Pos;

    [Header("Banner")]
    public GameObject def_Banner;
    public GameObject vic_Banner;
    public GameObject battle_Start_Banner;
    public GameObject battle_Ready_Banner;


    [Header("Battle_Popup")]
    public GameObject popup_Bg;
    public GameObject vic_Popup;
    public GameObject def_Popup;
    public GameObject exit_Popup;
    public GameObject alert_Popup;
    public GameObject event_Popup;
    public GameObject event_Alert_Popup;
    public GameObject check_Popup;
    public GameObject option_Popup;
    public GameObject attribute_Popup;
    public GameObject item_Check_Popup;


    [Header("Tutorial")]
    public GameObject item_Tutorial;
    public GameObject ui_Tutorial_Rest;
    public GameObject ui_Tutorial_Deploy;
    public GameObject ui_Tutorial_Box;


    [Header("Dialogue")]
    public GameObject dialogue_Box;
    public GameObject dialogue_Bg;


    [Header("Reward")]
    public GameObject reward_Popup;
    public Sprite[] reward_Icons;
    public GameObject reward_Prefab;
    public GameObject reward_Item_Prefab;

    [Header("Tooltip")]
    public Tooltip tooltip;
    public PartyDetailTooltip partyDetailTooltip;
    public Canvas cv;

    [Header("PartyDetail")]
    public PartyDetails partyDetails;
    public void ActiveBattlePartyDetail()
    {
        if (partyDetails.gameObject.activeSelf == false)
        {
            partyDetails.gameObject.SetActive(true);
            partyDetails.OpenPartyDetail();
            isOpenUI = true;
        }
        else
        {
            UnActiveBattlePartyDetail();
            isOpenUI = false;
        }
    }
    public void UnActiveBattlePartyDetail()
    {
        if (partyDetails.gameObject.activeSelf == true)
        {
            partyDetails.gameObject.SetActive(false);
            partyDetailTooltip.gameObject.SetActive(false);
            //파티디테일툴팁.gameObject.Setactive(false);
        }
    }
    private void Start()
    {
        player_Statbar.SetActive(true);
        item_Bar.SetActive(true);
        tooltip.TooltipSetting(cv.GetComponentInParent<CanvasScaler>().referenceResolution.x * 0.5f, tooltip.GetComponent<RectTransform>());
        partyDetailTooltip.TooltipSetting(cv.GetComponentInParent<CanvasScaler>().referenceResolution.x * 0.5f, partyDetailTooltip.GetComponent<RectTransform>());
        partyDetails.Init(GameUiMgr.single.lastDeparture);


        if (BattleManager.Instance.dialogue != null && BattleManager.Instance.dialogue.isTutorial) // 분수대 클릭 방지용
            isOpenUI = true;
        else
            isOpenUI = false;

    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (BattleManager.Instance.dialogue != null && dialogue_Bg.activeSelf || popup_Bg.activeSelf)
            {
                return;
            }
            else
            {
                if (!mini_Map_Big.activeSelf)
                {
                    BattleManager.Instance.room.OpenMap(true);
                }
                else
                {
                    BattleManager.Instance.room.OpenMap(false);
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!option_UI.activeSelf) 
            {
                /*                if (BattleManager.Instance.dialogue != null && dialogue_Box.activeSelf)
                                    return;*/
                if (attribute_Popup.activeSelf)
                    CancelPopup(attribute_Popup);
                else if (alert_Popup.activeSelf)
                    CancelPopup(alert_Popup);
                else if (event_Alert_Popup.activeSelf)
                    CancelPopup(event_Alert_Popup);
                else if (event_Popup.activeSelf)
                    CancelPopup(event_Popup);
                else if (next_Room_Popup.activeSelf)
                    CancelPopup(next_Room_Popup);
                else if (item_Check_Popup.activeSelf)
                    CancelPopup(item_Check_Popup);
                else if (reward_Popup.activeSelf)
                    CancelPopup(reward_Popup);
                else if (room_UI.activeSelf)
                    CancelPopup(room_UI);
                else
                {
                    OpenOption();
                }
            }
            else if (check_Popup.activeSelf)
            {
                check_Popup.SetActive(false);
            }
            else if (option_Popup.activeSelf)
            {
                option_Popup.SetActive(false);
            }
            else
            {
                CancelOption();    
            }
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            if (BattleManager.Instance.dialogue != null && dialogue_Bg.activeSelf || popup_Bg.activeSelf)
                return;
            else
                ActiveBattlePartyDetail();
        }

        tooltip.MoveTooltip();
        partyDetailTooltip.MoveTooltip();
    }

    private void FixedUpdate()
    {
        if (BattleManager.Instance._curphase == BattleManager.BattlePhase.Battle)
        {
            item_Bar.SetActive(false);
            log_View.SetActive(true);

            //SetPositionStatbar(new Vector2(0, 0), new Vector2(1, 0), new Vector2(0.5f, 0), 228, 10, player_Statbar.GetComponent<RectTransform>());

            if (!item_Bar.gameObject.activeSelf)
            {
                if (BattleManager.Instance.ui.tooltip.gameObject.activeSelf)
                {
                    BattleManager.Instance.ui.tooltip.gameObject.SetActive(false);
                }
            }
        }
        else
        {
            item_Bar.SetActive(true);
            log_View.SetActive(false);

            //SetPositionStatbar(new Vector2(0, 1), new Vector2(1, 1), new Vector2(0.5f, 1), 228, -3, player_Statbar.GetComponent<RectTransform>());
        }

        if (BattleManager.Instance._curphase == BattleManager.BattlePhase.Deploy)
        {
            battleStart.SetActive(true);
        }
        else
        {
            battleStart.SetActive(false);
        }
    }

    /*private void SetPositionStatbar(Vector2 anchorMin, Vector2 anchorMax, Vector2 pivot, float offset, float posY, RectTransform obj)
    {
        obj.anchorMin = anchorMin;
        obj.anchorMax = anchorMax;
        obj.pivot = pivot;

        Vector2 offsetMin = obj.offsetMin;
        Vector2 offsetMax = obj.offsetMax;

        offsetMin.x = offset;  // Left 설정
        offsetMax.x = -offset; // Right 설정 (음수 값으로 설정)

        obj.offsetMin = offsetMin;
        obj.offsetMax = offsetMax;

        Vector2 anchoredPosition = obj.anchoredPosition;
        anchoredPosition.y = posY;
        obj.anchoredPosition = anchoredPosition;

    }*/

    public void OpenPopup(GameObject popup)
    {
        popup_Bg.SetActive(true);
        popup.SetActive(true);
        isOpenUI = true;
    }

    public void OpenOption()
    {
        popup_Bg.SetActive(true);
        option_UI.SetActive(true);

        Canvas party_List_Deploy = option_UI.AddComponent<Canvas>();
        option_UI.AddComponent<GraphicRaycaster>();
        party_List_Deploy.additionalShaderChannels = AdditionalCanvasShaderChannels.TexCoord1;
        party_List_Deploy.overrideSorting = true;
        party_List_Deploy.sortingOrder = 2;

        if (BattleManager.Instance.dialogue != null && BattleManager.Instance.dialogue.isTutorial)
        {
            dialogue_Bg.SetActive(false);
            BattleManager.Instance.dialogue.ONOFF(false);
        }


        Time.timeScale = 0;
        isOpenUI = true;
    }

    public void CancelOption()
    {
        popup_Bg.SetActive(false);
        option_UI.SetActive(false);

        if (option_UI.GetComponent<GraphicRaycaster>() != null && option_UI.GetComponent<Canvas>() != null)
        {
            DestroyImmediate(option_UI.GetComponent<GraphicRaycaster>());
            DestroyImmediate(option_UI.GetComponent<Canvas>());
        }

        if (BattleManager.Instance.dialogue != null && BattleManager.Instance.dialogue.isTutorial)
        {
            dialogue_Bg.SetActive(true);
            BattleManager.Instance.dialogue.ONOFF(true);
        }

        Time.timeScale = 1;
        isOpenUI = false;
    }

    public void CheckPopup(int value)
    {
        popup_Bg.SetActive(true);
        check_Popup.SetActive(true);
        isOpenUI = true;

        string detail = "";
        PopUpState popupStates = PopUpState.None;

        switch (value)
        {
            case 0:  // 던전 중단 체크 팝업
                detail = "탐험을 중단 하시겠습니까?";
                popupStates = PopUpState.DungeonExit;
                break;
            case 1: // 게임 종료 체크 팝업
                detail = "게임을 종료 하시겠습니까?";
                popupStates = PopUpState.Quite;
                break;
            default:
                break;
        }

        check_Popup.GetComponent<PopUp>().SetPopUp(detail, popupStates);
    }

    public void CancelPopup(GameObject popup)
    {
        isOpenUI = false;
        if (BattleManager.Instance.dialogue != null)
        {
            if (BattleManager.Instance.dialogue.isTutorial)
            {
                if (popup.name == "Reward_Popup")
                {
                    if (popup.GetComponent<RewardPopupInit>().isBox)
                    {
                        BattleManager.Instance.tutorial.EndTutorial(17);
                    }
                    else
                    {
                        BattleManager.Instance.dialogue.ONOFF(true);
                        BattleManager.Instance.dialogue.NextDialogue();
                    }
                }
            }
        }
        
        popup.SetActive(false);
        popup_Bg.SetActive(false);
    }


    public IEnumerator Def_Banner()
    {
        CanvasGroup canvas = def_Banner.GetComponent<CanvasGroup>();

        yield return StartCoroutine(FadeTo(canvas, 0f, 1.0f, 1f));
        yield return new WaitForSeconds(1f);
        yield return StartCoroutine(FadeTo(canvas, 1f, 0f, 1f));
        CancelPopup(def_Banner);
    }


    private IEnumerator FadeTo(CanvasGroup group, float start, float targetAlpha, float duration)
    {
        float startAlpha = start;
        float timer = 0f;

        while (timer < duration)
        {
            float alpha = Mathf.Lerp(startAlpha, targetAlpha, timer / duration);
            group.alpha = alpha;
            timer += Time.deltaTime;
            yield return null;
        }

        group.alpha = targetAlpha;
    }


    public IEnumerator StartBanner(GameObject banner)
    {
        Animator ani = banner.GetComponent<Animator>();
        AnimatorStateInfo aniInfo = ani.GetCurrentAnimatorStateInfo(0);

        yield return new WaitForSeconds(aniInfo.length);
        CancelPopup(banner);
    }

    public void GenerateLog(Sprite player_portrait, string skill_Name)
    {
        Debug.Log("스킬 로그 생성");

        if (log_Pos.childCount >= 3)
        {
            Destroy(log_Pos.GetChild(0).gameObject);
        }

        GameObject log = Instantiate(log_Prefabs, log_Pos);
        SkillLogInit log_Init = log.GetComponent<SkillLogInit>();
        log_Init.Init(player_portrait, skill_Name);
    }

    public void WinSfxStop()
    {
        AudioManager.single.GetSfxPlayer(0).Stop();
    }

}
