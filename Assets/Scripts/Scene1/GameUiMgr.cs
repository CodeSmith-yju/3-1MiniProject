﻿//using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUiMgr : MonoBehaviour/*, IBeginDragHandler, IDragHandler, IEndDragHandler*/
{
    public static GameUiMgr single;
    private GameObject scanObject;//상호작용중인 오브젝트의 정보를 받아오 변수, PlayerAction에서 스캔할 오브젝트정보를 받아오기때문에 얘는 인스펙터에서 안 보여도된다.
    
    [Header("TalkPanel")]
    [SerializeField] private Image imgTalkPnel;// 대화창 표시유무를 위해 변수 선언
    public bool isActionTalk;// 대화창의 활성화 유무를 판별하기위한 변수
    public Image imgPortrait;// 초상화 이미지를 관리할 변수
    public TextMeshProUGUI talkName;
    public GameObject objNpcInner;//06-15 Add
    [Header("TalkMgr")]
    public TalkMgr talkMgr;// 대화 매니저를 변수로 선언하여 대화매니저의 함수에 접근 할 수 있게함.
    public int talkIndex;

    [Header("TextEffect")]
    public TypeEffect typeTextEffect;

    [Header("Player DESC")]
    //GameMgr.PlayerData.NAME/Level 을 인계받아 화면에 출력할 TMP변수
    public TextMeshProUGUI tmp_PlayerName;
    public TextMeshProUGUI tmp_PlayerLevle;
    public TextMeshProUGUI tmp_PlayerGold;
    public TextMeshProUGUI tmp_PlayerPartyTabGold;

    public Slider s_HP;
    public Slider s_SN;
    public Slider s_EXP;

    public TextMeshProUGUI tmp_PlayerRating;
    [Header("Button")]
    public GameObject mainButton;// 클릭할 메인 버튼
    public GameObject[] subButtons;// 클릭하면 펼쳐질 서브버튼들.
    private bool areSubButtonsVisible = false;// 메인버튼을 클릭해서 얘가 true가 되면 서브버튼이 보여짐 
    //[SerializeField] private Image img_Portrait;
    //[SerializeField] private Sprite[] ary_sp_Portrait;
    [SerializeField] private GameObject objSubButtonFrame;
    
    [Header("VideoOption")]
    [SerializeField] private VideoOption videoOption_S1;

    //Minimap
    private bool bigMinimapChek;
    [Header("Minimap")]
    [SerializeField] private Camera miniCamera;
    [SerializeField] private RenderTexture minimapRanderTexture;
    [SerializeField] private RawImage rimgSmall;
    [SerializeField] private RawImage rimgBig;
    [SerializeField] private Image smallMap;
    [SerializeField] private Image bigMap;

    //Player Desc
    [Header("Desc_Player")]
    [SerializeField] private Image PlayerDesc;
    private bool DescCheck;

    //MenuSet
    [Header("Mnue Set")]
    //public GameObject menuSet;

    [Header("Player Options")]
    public GameObject player;
    public PlaceState nowPlayerPlace;
    public Transform[] arySpawnPoint;

    [Header("Quest Manager")]
    public QuestMgr questMgr;//퀘스트 번호를 가져올 퀘스트 매니저 변수 생성
    public TextMeshProUGUI questTitle;
    public TextMeshProUGUI questDesc;
    //06-18 퀘스트 Id, Index가 정상저장이되지않는상태이기때문에 이를 해결하기위한 추가변수 도입
    public static int quest_Id = 0;
    public static int quest_Index = 0;

    [Header("Invnetory")]
    [SerializeField] private GameObject inventory_panel;
    [HideInInspector] public bool activeInventory = false;
    //03-31 variable Inventoty - try.4LocalDataStore
    public Slot[] slots;
    public Transform slotHolder;// 인벤토리의 아이템슬롯 오브젝트가 들어가는 부모 오브젝트 위치
    //04-21 Inventory Slot Drag items
    public Image dragIcon;
    public Slot nowSlot;
    public Slot[] targetSlots;
    public int dragIndex;
    //04-22
    public Image addEquipPanel;
    public Button btn_YesEquipAdd;
    public Button btn_NoEquipAdd;
    public bool equipmnet;
    //Add
    public TextMeshProUGUI textEquipPanel;

    [Header("ToolTip")]
    public Tooltip tooltip;
    public PartyTooltip partytooltip;
    public PartyDetailTooltip partyDetailTooltip;

    public DungeonTooltip dungeonTooltip;
    public Canvas cv;
    //Vector3 lodingPosition;// player Position

    [Header("player State")]
    public float player_Max_HP;
    public float player_Cur_HP;
    public float player_Max_SN;
    public float player_Cur_SN;
    private float player_Max_MP;
    private float player_Cur_MP;

    public int playerGold;
    public int playerLevel;

    private float player_Atk_Speed;
    private float player_Atk_Range;
    private float player_Base_Atk_Dmg;
    private float player_Max_EXP;
    private float player_Cur_EXP;

    //04-25 Tutorial Quest ActionIndex
    private bool wearEquipment = false;

    //05-12 PartyList
    [Header("PartyDetails")]
    public PartyDetails partyDetails;
    [Header("Party Bord")]
    public GameObject panelPartyBoard;// 파티 게시판오브젝트
    public PartyIconRS entityIconRS;
    [SerializeField] private List<PartySlot> poolPartySlot = new(); // 파티게시판의 Body에 해당하는 고용가능한 파티원 리스트 이거수정해야할수도있음
    [SerializeField] private List<PartyData> listPartyData = new();// 실제파티원들 정보가 저장되어야함
    
    //05-21 ClickedPartySlot -> Add Buttom PartySlot 
    public List<PartyData> partyData;// 얘로 파티원데이터생성해서 집어넣음 왜인지 아직 파악못했는데 Slot자체로하려니까 오류가남
    public List<PartySlot> poolMoveInSlot = new(); // 파티게시판의 Buttom에 해당하는 고용파티원 명단 리스트

    public GameObject partyPrefab; // 새로운 슬롯을 생성할 때 사용할 프리팹, 부모 transform은 transfrom.parent를 사용하는것으로 사용안함
    public GameObject playerPrefab;//플레이어 프리펩넣을곳 이걸로 파티고용리스트 0번 요소에 상호작용 불가능한 플레이어이미지를 고정으로 넣어 줄 것.
    //05-14
    public List<GameObject> objListPlayable;// 파티보드에서 출력될 실제 플레이어블 캐릭터 리소스 데이터를 여기에 임시로 등록

    //파티보드 하단에 출력되는 파티원 인원수 카운팅 / 고용할 파티원목록에 포함된 파티원 들 몸값 텍스트 반영
    public TextMeshProUGUI textPartyCount;
    public TextMeshProUGUI textPartyPrice;
    public int partyPrice;
    
// PoolMoveInSlot에 PartyData가 있을경우 여기에 담아서 고용완료 목록에 추가되어 Battle씬의 PartyList가 얘를 참조하게됨 || 슬롯이가지고있는 PartyData에 포함된 Prefab을 가져가는형식
    public List<PartySlot> lastDeparture;

    public GameObject blockedPartyBord;
    public Button btn_PartyCommit;

    public PartyNameSetting partyNameSetting;

    //06-16 던전입장용변수
    public bool isDungeon = false;

    //활성화된 ui창이있으면 강제로 종료하게하기위한 변수
    public bool uiEventCk = true;

    //특정 ui가 활성화되어있을때 캐릭터 이동을 막는 변수
    public bool move_doit = true;

    [Header("Shop UI")]
    [SerializeField] ShopMgr shopMgr;
    [SerializeField] Blacksmith blacksmith;
    public GameObject SnB;

    [Header("PopUp")]
    public PopUp popUp;

    [Header("Dungeon_Level")]
    public GameObject dungeon_Level_Ui;
    public float dungeon_Level_Scale = 1;
    public int dungeon_Level;
    public Button[] level_Buttons;

    public Sprite question;

    public void SetLevel(int level)
    {
        dungeon_Level = level;

        switch (level)
        {
            case 0:
                dungeon_Level_Scale = 0.75f;
                break;
            case 1:
                dungeon_Level_Scale = 1;
                break;
            case 2:
                dungeon_Level_Scale = 1.25f;
                break;
            case 3:
                dungeon_Level_Scale = 1.5f;
                break;
        }
    }
    private void TutoDungeonQuestCheck()
    {
        int dungeonCheckCheser = GameMgr.single.GetPlayerDifficulty();
        if (isDungeon)
        {
            // level_Buttons[0]은 튜토리얼 버튼이므로 나머지 버튼을 비활성화
            for (int i = 0; i < level_Buttons.Length; i++)
            {
                if (i == 0) // 인덱스 0은 튜토리얼 버튼
                {
                    level_Buttons[i].interactable = true; // 튜토리얼 버튼은 활성화 상태
                    level_Buttons[i].GetComponent<CanvasGroup>().alpha = 1.0f;
                }
                else
                {
                    level_Buttons[i].interactable = false; // 나머지 버튼은 비활성화 상태
                    level_Buttons[i].GetComponent<CanvasGroup>().alpha = 0.5f;
                }
            }
        }
        else
        {
            for (int i = 0; i < level_Buttons.Length; i++)
            {
                if (i == 0) // 인덱스 0은 튜토리얼 버튼
                {
                    level_Buttons[i].interactable = false;
                    level_Buttons[i].GetComponent<CanvasGroup>().alpha = 0.5f;
                }
                else if (i == 4)
                {
                    Debug.Log("??? btn");
                    SetDungeonTooltip sd = level_Buttons[4].GetComponent<SetDungeonTooltip>();

                    if (GameMgr.single.GetPlayerDifficulty() > 51)
                    {
                        Debug.Log("LasgDungeon 클리어 이후");

                        level_Buttons[i].gameObject.SetActive(true);
                        level_Buttons[i].gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "악마의 미궁";
                        RectTransform rt = dungeon_Level_Ui.GetComponent<RectTransform>();
                        rt.sizeDelta = new Vector2(400, 500);
                        rt.anchoredPosition = new Vector2(0, 24);

                        sd.Change_ScaleAndBoolean(3, true);
                    }
                    else
                    {
                        sd.Change_ScaleAndBoolean(3, false);
                    }
                }
                if(dungeonCheckCheser >= 8)
                {
                    if (dungeonCheckCheser == 8)
                    {
                        dungeonCheckCheser = 20;
                    }
                    if (dungeonCheckCheser == 23) //|| dungeonCheckCheser == 33 || dungeonCheckCheser == 43)
                    {
                        dungeonCheckCheser = 30;
                    }
                    if (dungeonCheckCheser == 33)
                    {
                        dungeonCheckCheser = 40;
                    }

                    if (0 < i &&i < dungeonCheckCheser/10)
                    {
                        level_Buttons[i].interactable = true;
                        level_Buttons[i].GetComponent<CanvasGroup>().alpha = 1.0f;
                    }
                    else
                    {
                        level_Buttons[i].interactable = false;
                        level_Buttons[i].GetComponent<CanvasGroup>().alpha = 0.5f;
                    }
                }
            }
        }
    }
    public void OpenDungeonUi()
    {
        dungeon_Level_Ui.SetActive(true);
        TutoDungeonQuestCheck();
    }
    public void EnterDungeon()
    {
        popUp.SetPopUp("던전에 입장 하시겠습니까?",  PopUpState.Dungeon);
        popUp.gameObject.SetActive(true);
    }
    public void MoveInDungeon()
    {
        AudioManager.single.PlaySfxClipChange(4);
        Debug.Log("던전 입장");
        GameSave();

        // 튜토리얼때는 튜토리얼을 제외한 나머지 버튼은 잠겨 있어야함.
        if (isDungeon)
        {
            LoadingSceneController.LoadScene("Tutorial");
            //LoadingSceneController.LoadScene("Battle");
            isDungeon = false;
        }
        else
        {
            LoadingSceneController.LoadScene("Battle");
        }
    }
    private void Awake()
    {
        single = this;
    }
    public void AddItemTest()
    {
        /*for (int i = 0; i < ItemResources.instance.DBItems.Count; i++)
        {
            Debug.Log("AddItem");
            //아 버그 왜 생기는거냐 진짜 소모아이템생성로직에 문제가있는데
            //기존 아이템생성방식의 한계점을발견하여 아래와 같이 수정함
            //int r = Random.Range(0, 20);
            Item _item = ItemResources.instance.DBItems[i];

            Inventory.Single.AddItem(_item); // 인벤토리에 아이템 추가,
            Inventory.Single.AddItem(_item);

            Debug.Log("Make A New Test Item Code: " + _item.itemCode);
        }*/
        for (int i = 0; i < 20; i++)
        {
            Item _item = new Item().GenerateRandomItem(i);

            if (i == 7)
            {
                _item = new Item().GenerateRandomItem(0);
            }
            if (i > 15 || i==7)
            {
                Inventory.Single.AddItem(_item);
                Inventory.Single.AddItem(_item);
                Inventory.Single.AddItem(_item);

                Inventory.Single.AddItem(_item);
                Inventory.Single.AddItem(_item);
                Inventory.Single.AddItem(_item);

                Inventory.Single.AddItem(_item);
            }
            Inventory.Single.AddItem(_item);
        }
        GameMgr.playerData[0].player_Gold += 4000;
        RedrawSlotUI();
    }
    public void ValueUpdate()
    {
        int ran = UnityEngine.Random.Range(0, 2);
        if (ran == 0)
        {
            playerGold += 15;
            player_Max_HP += 15;
            player_Max_SN += 15;
            player_Cur_EXP += 2;

            Debug.Log("player gold: " + playerGold);
            Debug.Log("player max hp: " + player_Max_HP);
            Debug.Log("player max sn: " + player_Max_SN);
            Debug.Log("player cur sn: " + player_Cur_EXP);
        }
        else
        {
            playerGold -= 1;
            player_Cur_HP -= 1;
            player_Cur_SN -= 1;

            Debug.Log("player gold: " + playerGold);
            Debug.Log("player cur hp: " + player_Cur_HP);
            Debug.Log("player cur sn: " + player_Cur_SN);
        }

        //SliderChange();
    }

    private void Start()
    {
        //03-31 Start Inventory - try.4
        inventory_panel.SetActive(activeInventory);
        Inventory.Single.onSlotCountChange += SlotChange;
        slots = slotHolder.GetComponentsInChildren<Slot>();
        Inventory.Single.onChangeItem += RedrawSlotUI;
        shopMgr.RefreshShopItems();
        if (GameMgr.single.LoadChecker() == true)
        {
            GameLoad();

            SetPlayerDatas();

            //TargetSlotsRefresh();
            GameUiMgr.single.slots = GameUiMgr.single.slotHolder.GetComponentsInChildren<Slot>();
            Inventory.Single.onChangeItem += RedrawSlotUI;

            /*if (GameUiMgr.single.shopCleaner)
            {
                Debug.Log("상점 새로고침 ing...");
                GameUiMgr.single.shopCleaner = false;
                GameUiMgr.single.shopMgr.RefreshShopItems();
                GameUiMgr.single.GameSave();
            }*/

            /*//Debug.Log("ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ");
            //Debug.Log("게임데이터를 불러온 다음입니다.");

            //Debug.Log("PlayerData - QID: " + GameUiMgr.single.questMgr.questId);
            //Debug.Log("PlayerData - AID: " + GameUiMgr.single.questMgr.questActionIndex);
            //Debug.Log("NowGold: " + GameMgr.playerData[0].player_Gold);
            //Debug.Log("Now_SN" + GameMgr.playerData[0].cur_Player_Sn);
            //Debug.Log("Now_HP" + GameMgr.playerData[0].cur_Player_Hp);

            //Debug.Log("Now_Lv" + GameMgr.playerData[0].player_level);
            //Debug.Log("Now_cur - exp" + GameMgr.playerData[0].player_cur_Exp);
            //Debug.Log("Now_max - exp" + GameMgr.playerData[0].player_max_Exp);*/

            Debug.Log("Load Type: " + GameMgr.single.LoadChecker());
        }
        else
        {
            //shopMgr.RefreshShopItems();
            Debug.Log("지금은 게임 로드중이 아닙니다.");
        }

        Debug.Log("GameMgr.single.tutorial: " + GameMgr.single.tutorial);
        //06-14 BGM
        AudioManager.single.PlayBgmClipChange(1);

        imgTalkPnel.gameObject.SetActive(false);// NPC대화창 시작할때 꺼줌
        objSubButtonFrame.SetActive(true);//서브버튼 목록 시작할때 켜줌
        HideSubButtons();//서브버튼 하위 목록 시작할때 꺼줌
        OffVideoOption_S1();//게임옵션 설정창 시작할때 꺼줌
        panelPartyBoard.SetActive(false);//05-12 파티창 게임시작할때 꺼줌
        SnB.SetActive(false);//09-30
        bigMinimapChek = true;
        smallMap.gameObject.SetActive(true);
        bigMap.gameObject.SetActive(false);
        popUp.gameObject.SetActive(false);
        PlayerDesc.gameObject.SetActive(true);
        dungeonTooltip.gameObject.SetActive(false);
        DescCheck = true;


        OneTimeRun();

        EquipSlotSetting();// 씬 실행 시 각 장비 슬롯에 해당하는 아이템 타입을 직접 지정해줌

        //05-21 
        partyNameSetting.RefreshiNameList();
        entityIconRS.SetAllIcons();
        RefreshiPartyBord();

        //04-22
        addEquipPanel.gameObject.SetActive(false);
        // Yes 버튼에 클릭 이벤트 리스너 추가
        btn_YesEquipAdd.onClick.AddListener(OnYesButtonClick);

        // No 버튼에 클릭 이벤트 리스너 추가
        btn_NoEquipAdd.onClick.AddListener(OnNoButtonClick);

        questDesc.text = questMgr.CheckQuest();

        SetPlayerDatas();//PlayerData[0]의 데이터를가져와서 데이터 저장하고 Dsce/각종 게이지 슬라이더/골드 변동사항 반영

        //튜토리얼던전 클리어 여부 확인하여 접수원 배치 변경하는 코드
        TutorialDungeonClear();//이거이제 필요없어질거임
        //Tooltip
        SetTooltip();
        TargetSlotsRefresh();
        if (GameMgr.single.GetShopClean())
        {
            Debug.Log("---------------------------상점 새로고침중인ㄷ ㅔ스 *----------");
            GameMgr.single.SetShopClean (false);
            GameUiMgr.single.shopMgr.RefreshShopItems();
        }
        else
        {
            Debug.Log("---------------------------상점 새로고침이 안되는 ㄷ ㅔ스 *----------");
        }

        UpdatePlayerRankAndQuestText((PlayerDifficulty)GameMgr.single.GetPlayerDifficulty());
    }

    //03-31 Method Inventory - try.4
    public void SlotChange(int val)// slotChange 에서 slot의 slotNum을 차례대로 부여함
    {
        if (Inventory.Single.items.Count != 0)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                //slots[i].slotnum = i;

                if (i < Inventory.Single.items.Count) // 인벤토리에 아이템이 있을 때만 버튼을 활성화
                    slots[i].GetComponent<Button>().interactable = true;
                else
                    slots[i].GetComponent<Button>().interactable = false;
            }
        }
        
    }

    public void OneTimeRun()
    {
        if (GameMgr.single.LoadChecker() == false)
        {
            AddSlot();
        }
    }
    void AddSlot()
    {
        //인벤토리 칸 세팅할때 나는 설정 안 만져서 그런지 이걸로 인벤토리 한번 활성화 시켜주지않으면 이상하게 동작하는거 확인.
        Inventory.Single.SlotCnt += 5;
    }

    public void RedrawSlotUI()// 08-14 수정
    {
        if (SceneManager.GetActiveScene().name.Equals("Town"))
        {
            // 모든 슬롯 초기화
            for (int i = 0; i < slots.Length; i++)
            {
                slots[i].RemoveSlot();
            }

            // 아이템 개수만큼만 슬롯 업데이트
            int itemCount = Mathf.Min(Inventory.Single.items.Count, slots.Length);
            for (int i = 0; i < itemCount; i++)
            {
                slots[i].item = Inventory.Single.items[i];
                //DBConnector.LoadItemByCodeFromDB(slots[i].item.itemCode, ref slots[i].item.itemImage, ref slots[i].item.typeIcon);
                //slots[i].item.itemIndex = i;

                /*if (i < ItemResources.instance.itemRS.Count && slots[i].name == ItemResources.instance.itemRS[i].itemName)
                {
                    slots[i].item.itemCode = ItemResources.instance.itemRS[i].itemCode;
                }*/

                slots[i].UpdateSloutUI();
            }
        }
    }

    private void Update()
    {
        // Minimap 
        if (Input.GetKeyDown(KeyCode.M))
        {
            ChangeRanderTextur();
            MinimapInteraction();
        }
        // Player Desc
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (DescCheck == true)
            {
                PlayerDesc.gameObject.SetActive(false);
                DescCheck = false;
            }
            else
            {
                PlayerDesc.gameObject.SetActive(true);
                DescCheck = true;
            }

        }

        // Sub Menu Set
        if (Input.GetButtonDown("Cancel"))
        {
            //여기에이제 파티상태창 끄고, 뭐끄고 끄고끄고
            /*if (menuSet.activeSelf)
            {
                menuSet.SetActive(false);
                uiEventCk = true;
            }
            else
            {*/
            ToggleSubButtons();
            if (objSubButtonFrame.activeSelf)
            {
                ToggleSubButtons();
            }
            if (inventory_panel.activeSelf)
            {
                ActiveInventory();
                tooltip.gameObject.SetActive(false);
            }
            if (panelPartyBoard.activeSelf)
            {
                UnActiveParty();
            }
            if (shopMgr.gameObject.activeSelf)
            {
                ActiveShop();
            }
            if (blacksmith.gameObject.activeSelf)
            {
                ActiveBlackSmith();
            }
            if (partyDetails.gameObject.activeSelf)
            {
                UnActivePartyDetail();
            }
            
            /*else
            {
                menuSet.SetActive(true);
                uiEventCk = false;
            }*/
        }

        //Inventory
        if (Input.GetKeyDown(KeyCode.I))
        {
            ActiveInventory();
        }
        //Tooltip
        tooltip.MoveTooltip();
        partytooltip.MoveTooltip();
        partyDetailTooltip.MoveTooltip();

        /*Debug.Log("x:" + player.transform.position.x);시발시발시발
        Debug.Log("y:" + player.transform.position.y);*/

        //PartyPanel
        if (Input.GetKeyDown(KeyCode.P))// && blockedPartyBord.activeSelf)
        {
            /*if (questMgr.questId > 30 || questMgr.questId == 30 && questMgr.questActionIndex >= 1 )
                 ActiveParty();*/
            if (GameMgr.single.GetPlayerDifficulty() > 4)
            {
                ActivePartyDetail();
            }
            else
            {
                Debug.LogWarning("파티 상태창 개방 조건 미 충족");
            }
        }

        //Ui Event Action
        if (!uiEventCk)
        {
            if (panelPartyBoard.activeSelf)
            {
                //panelPartyBoard.SetActive(false);
                UnActiveParty();
            }
            if (activeInventory == true)
            {
                inventory_panel.SetActive(false);
            }
            if (blacksmith.gameObject.activeSelf)
            {
                blacksmith.gameObject.SetActive(false);
            }
            if (shopMgr.gameObject.activeSelf)
            {
                shopMgr.gameObject.SetActive(false);
            }
            if (partyDetails.gameObject.activeSelf)
            {
                UnActivePartyDetail();
            }

            uiEventCk = false;
        }


    }
    void SetTooltip()
    {
        tooltip.TooltipSetting(cv.GetComponentInParent<CanvasScaler>().referenceResolution.x * 0.5f, tooltip.GetComponent<RectTransform>());
        partytooltip.TooltipSetting(cv.GetComponentInParent<CanvasScaler>().referenceResolution.x * 0.5f, partytooltip.GetComponent<RectTransform>());
        partyDetailTooltip.TooltipSetting(cv.GetComponentInParent<CanvasScaler>().referenceResolution.x * 0.5f, partyDetailTooltip.GetComponent<RectTransform>());
    }
    #region MinimapMethod
    private void ChangeRanderTextur()
    {
        int screenWidth = Screen.width;
        int screenHeight = Screen.height;

        if (bigMinimapChek)
        {
            minimapRanderTexture.Release(); // 기존 텍스처 해제
            minimapRanderTexture.width = screenWidth; // 너비 변경
            minimapRanderTexture.height = screenHeight; // 높이 변경
            minimapRanderTexture.Create(); // 변경된 크기로 새로 생성
        }
        else
        {
            minimapRanderTexture.Release(); // 기존 텍스처 해제
            minimapRanderTexture.width = 1024; // 너비 변경
            minimapRanderTexture.height = 1024; // 높이 변경
            minimapRanderTexture.Create(); // 변경된 크기로 새로 생성
        }

    }
    private void MinimapInteraction()
    {
        if (bigMinimapChek)
        {
            smallMap.gameObject.SetActive(false);
            bigMap.gameObject.SetActive(bigMinimapChek);
            RawAlphaChange(rimgSmall, 0f);
            RawAlphaChange(rimgBig, 1f);


            Rect newRect = new Rect(0f, 0f, 1.2f, 1.2f);
            miniCamera.rect = newRect;

            bigMinimapChek = false;
        }
        else
        {
            smallMap.gameObject.SetActive(true);
            bigMap.gameObject.SetActive(bigMinimapChek);
            RawAlphaChange(rimgSmall, 1f);
            RawAlphaChange(rimgBig, 0f);

            Rect newRect = new Rect(0f, 0f, 2f, 2f);
            miniCamera.rect = newRect;

            bigMinimapChek = true;
        }
    }
    private void RawAlphaChange(RawImage rimg, float alpha)
    {
        Color color = rimg.color;
        color.a = alpha;
        rimg.color = color;
    }
    #endregion
    #region ToggleBtn
    public void ToggleSubButtons()
    {
        areSubButtonsVisible = !areSubButtonsVisible;

        if (areSubButtonsVisible)
        {
            ShowSubButtons();
        }
        else
        {
            HideSubButtons();
        }
    }
    private void ShowSubButtons()
    {
        foreach (GameObject subButton in subButtons)
        {
            subButton.gameObject.SetActive(true);
        }
    }
    public void HideSubButtons()
    {
        foreach (GameObject subButton in subButtons)
        {
            subButton.gameObject.SetActive(false);
        }
    }
    #endregion
    private void SetPlayerDatas()
    {
        tmp_PlayerName.text = GameMgr.playerData[0].NAME;
        playerGold = GameMgr.playerData[0].player_Gold;
        playerLevel = GameMgr.playerData[0].player_level;

        tmp_PlayerLevle.text = "Lv." + GameMgr.playerData[0].player_level .ToString();
        tmp_PlayerGold.text = GameMgr.playerData[0].player_Gold .ToString();

        this.player_Max_HP = GameMgr.playerData[0].max_Player_Hp;
        this.player_Cur_HP = GameMgr.playerData[0].cur_Player_Hp;

        this.player_Max_SN = GameMgr.playerData[0].max_Player_Sn;
        this.player_Cur_SN = GameMgr.playerData[0].cur_Player_Sn;

        this.player_Max_MP = GameMgr.playerData[0].max_Player_Mp;
        this.player_Cur_MP = GameMgr.playerData[0].cur_Player_Mp;

        this.player_Atk_Speed = GameMgr.playerData[0].atk_Speed;
        this.player_Atk_Range = GameMgr.playerData[0].atk_Range;
        this.player_Base_Atk_Dmg = GameMgr.playerData[0].base_atk_Dmg;
        this.player_Max_EXP = GameMgr.playerData[0].player_max_Exp;
        this.player_Cur_EXP = GameMgr.playerData[0].player_cur_Exp;
        

        //s_HP.value = this.player_Cur_HP / this.player_Max_HP;
        //s_SN.value = this.player_Cur_SN / this.player_Max_SN;
        //s_EXP.value = this.player_Cur_EXP / this.player_Max_EXP;

        SliderChange();
    }
    public void SliderChange()
    {
        s_HP.value = GameMgr.playerData[0].cur_Player_Hp / GameMgr.playerData[0].max_Player_Hp;//this.player_Cur_HP / this.player_Max_HP;
        s_SN.value = GameMgr.playerData[0].cur_Player_Sn / GameMgr.playerData[0].max_Player_Sn;
        s_EXP.value = GameMgr.playerData[0].player_cur_Exp / GameMgr.playerData[0].player_max_Exp;

        if (GameMgr.playerData[0].cur_Player_Sn < 15)
        {
            string hexColor = "#D26100";
            Image fillImage = s_SN.fillRect.GetComponent<Image>();
            if (ColorUtility.TryParseHtmlString(hexColor, out Color color))
            {
                fillImage.color = color; // 이미지 색상 변경
            }
        }
        else
        {
            string hexColor = "#22D200";
            Image fillImage = s_SN.fillRect.GetComponent<Image>();
            if (ColorUtility.TryParseHtmlString(hexColor, out Color color))
            {
                fillImage.color = color; // 이미지 색상 변경
            }

        }

        GoldChanger();
    }
    public void GoldChanger()
    {
        tmp_PlayerGold.text = GameMgr.playerData[0].player_Gold.ToString();
        tmp_PlayerPartyTabGold.text = GameMgr.playerData[0].player_Gold.ToString();

        if (GameMgr.playerData[0].player_Gold <= 0)
        {
            tmp_PlayerGold.text = "0";
            tmp_PlayerPartyTabGold.text = "0";
        }
    }
    public void TalkAction(GameObject scanObj)
    {
        
        scanObject = scanObj;
        ObjectData objectData = scanObject.GetComponent<ObjectData>();// Ray가 스캔했을때  LayerMask가 Obejct인 오브젝트가 부착중인 ObecjtData를  Ray가 오브젝트를 스캔 했을 때만 추출해서 TossTalkData메서드의 매개변수로 사용함.
        if (objectData.isNpc)
        {
            objNpcInner.SetActive(true);
        }
        else
        {
            objNpcInner.SetActive(false);
        }
        TossTalkData(objectData.id, objectData.isNpc);
        //Debug.Log(objectData.id.ToString());// 04-23 Debug
        imgTalkPnel.gameObject.SetActive(isActionTalk);// isActionTalk의 true/false 상태를 따라가기때문에 이렇게 작성해주면 코드 깔끔해짐 
    }

    private void TossTalkData(int scanObj_ID, bool scanObj_isNpc)
    {
        //Set Talk Data
        int questTalkIndex = 0;
        string talkData = "";

        int questDifficultyChser = GameMgr.single.GetPlayerDifficulty();
        // isAnim
        if (typeTextEffect.isAnim)
        {
            typeTextEffect.SetMsg("");
            return;
        }
        else
        {
            questTalkIndex = questMgr.GetQuestTalkIndex(scanObj_ID);
            talkData = talkMgr.GetTalk(scanObj_ID + questTalkIndex, talkIndex);
        }

        //End Talk
        if (talkData == null)
        {
            //is Tutorol Event
            if (questMgr.receptionist[1].activeSelf && questMgr.questId == 40)
            {
                //튜토리얼던전을 클리어했고, 튜토리얼던전 클리어시 활성화되는 접수원1이 활성화 상태이면서, 모의던전클리어 퀘스트(Qid 40)를 진행중일때.
                tmp_PlayerRating.text = "견습 모험가";
            }

            if (scanObj_isNpc)
            {
                Debug.Log("ID : " + scanObj_ID);
            }
            if (scanObj_ID == 1000)//대화 끝났으면 가이드제거
            {
                if (questMgr.questId == 10)
                {
                    talkMgr.guideUi.ChangeGuideImg(GuideState.None);
                }
                else if (questMgr.questId == 40)
                {
                    if (questMgr.questActionIndex == 1)
                    {
                        Debug.Log("-------------------40, 1, 8, easybefor Talk End");
                        if (questDifficultyChser == 8)
                        {
                            UpdatePlayerRankAndQuestText((PlayerDifficulty)questDifficultyChser);
                        }
                    }
                }
                else if (questMgr.questId == 50)
                {
                    if (questMgr.questActionIndex == 0)
                    {
                        Debug.Log("-------------------50, 0, 8, easybefor Talk End");
                        if (questDifficultyChser == 20)
                        {
                            GameMgr.single.SetPlayerDifficulty(20);
                        }
                    }

                    if (questMgr.questActionIndex == 1)
                    {
                        UpdatePlayerRankAndQuestText((PlayerDifficulty)questDifficultyChser);
                    }
                }
                else if (questMgr.questId == 60)
                {
                    if (questMgr.questActionIndex == 0)
                    {
                        Debug.Log("-------------------60, 0, , Talk End");
                        if (questDifficultyChser == 23)
                        {
                            GameMgr.single.SetPlayerDifficulty(31);//nomal Start
                        }
                    }
                    else// aindex == 1
                    {
                        Debug.Log("-------------------60, 1, Talk End");
                    }
                }
                else if (questMgr.questId == 70)
                {
                    if (questMgr.questActionIndex == 0)
                    {
                        Debug.Log("-------------------70, 0, Talk End");
                        if (questDifficultyChser == 33)//Last MainQuest Start
                        {
                            GameMgr.single.SetPlayerDifficulty(51);
                            UpdatePlayerRankAndQuestText((PlayerDifficulty)51);
                        }
                    }
                    else
                    {
                        Debug.Log("-------------------70, 1, Talk End");
                    }
                }
            }
            else if (scanObj_ID == 2000)
            {
                if (questMgr.questId == 50)
                {
                    if (questMgr.questActionIndex == 1 && questDifficultyChser == 22)
                    {
                        Debug.Log("-------------------50, 1, 2000, easy after Talk End");

                        GameMgr.single.SetPlayerDifficulty(23);
                        UpdatePlayerRankAndQuestText((PlayerDifficulty)23);
                    }
                }
                else if (questMgr.questId == 60)
                {
                    if (questMgr.questActionIndex == 1 && questDifficultyChser == 32)
                    {
                        GameMgr.single.SetPlayerDifficulty(33);
                        UpdatePlayerRankAndQuestText((PlayerDifficulty)33);
                    }
                }
                else if( questMgr.questId == 70)
                {
                    if (questDifficultyChser == 52)
                    {
                        GameMgr.single.SetPlayerDifficulty(53);

                        isActionTalk = true;
                        uiEventCk = false;
                        talkIndex++;
                        UpdatePlayerRankAndQuestText((PlayerDifficulty)53);
                        questMgr.questId = 80;
                        questMgr.questActionIndex = 0;
                        GameSave();
                        LoadingSceneController.LoadScene("Title");
                    }
                }
            }
            
            if (scanObj_ID == 4000)
            {
                Debug.Log("상점 npc와 대화종료되면 상점/대장간 ui오픈");
                if (questMgr.questId >= 50)
                {
                    SnB.SetActive(true);
                    move_doit = false;
                }
                
            }
            //Debug.Log("NulltalkData // ToosTalkData: " + scanObj_ID); // 04 -23 Debug
            /*if (AllEquipChek())
            {
                questMgr.CheckQuest(scanObj_ID);
                return;
            }*/

            isActionTalk = false;
            uiEventCk = true;//06-16 Add

            talkIndex = 0;
            questDesc.text = questMgr.CheckQuest(scanObj_ID);

            questDifficultyChser = GameMgr.single.GetPlayerDifficulty();

            if (questMgr.questId == 40)
            {
                if (questMgr.questActionIndex == 1)
                {
                    if (questDifficultyChser == 5)
                    {
                        GameMgr.single.SetPlayerDifficulty(6);
                    }
                }
            }
            else if (questMgr.questId == 50)
            {
                Debug.Log("---------- is 50 QC end");
                if (questMgr.questActionIndex == 0)
                {
                    Debug.Log("---------- is 0 QC end");
                    /*if (questDifficultyChser == 20)
                    {
                        Debug.Log("---------- is 8 is 20 end");
                        GameMgr.single.SetPlayerDifficulty(questDifficultyChser);
                    }*/
                }
                else if (questMgr.questActionIndex == 1)
                {
                    GameMgr.single.GetPlayerDifficulty();
                    Debug.Log(""+ questDifficultyChser);
                    if (questDifficultyChser == 21)
                    {
                        Debug.Log("Easy Dungeon Quest Start & Easy Dungeon is not Clear");
                        UpdatePlayerRankAndQuestText((PlayerDifficulty)questDifficultyChser);
                    }
                }
            }
            if (questMgr.questId == 60)
            {
                /*if (questMgr.questActionIndex == 0)
                {
                    if (questDifficultyChser == 23)
                    {
                        GameMgr.single.SetPlayerDifficulty(30);
                    }
                }
                else if (questMgr.questActionIndex == 1)
                {
                    if (questDifficultyChser == 30)
                    {
                        UpdatePlayerRankAndQuestText((PlayerDifficulty)questDifficultyChser);
                    }
                }*/
            }

            if (questDifficultyChser == 4)
            {
                UpdatePlayerRankAndQuestText((PlayerDifficulty)questDifficultyChser);
            }
            else if (questDifficultyChser == 20 && questDifficultyChser < 30)
            {
                UpdatePlayerRankAndQuestText((PlayerDifficulty)questDifficultyChser);
            }
            else if (questDifficultyChser > 30 && questDifficultyChser < 33)
            {
                UpdatePlayerRankAndQuestText((PlayerDifficulty)questDifficultyChser);
            }else if (questDifficultyChser == 51 )
            {
                UpdatePlayerRankAndQuestText((PlayerDifficulty)51);
            }
            return;
        }

        //Continue Talk
        if (scanObj_isNpc)
        {
            //Debug.Log("ContinueTalk // ToosTalkData: " + scanObj_ID);// 04-23 Debug
            typeTextEffect.SetMsg(talkData.Split(':')[0]);// .Split()  ':' 구분자 : 를 통하여 문자열을 배열로 나눠주는 함수

            //show Portrait
            imgPortrait.sprite = talkMgr.GetPortrait(scanObj_ID, int.Parse(talkData.Split(':')[1]));
            imgPortrait.color = new Color(1, 1, 1, 1);// npc가 맞으면 초상화이미지 활성화
            SetTalkName(imgPortrait.sprite);
        }
        else
        {
            //Debug.Log("else ContinueTalk // ToosTalkData: " + scanObj_ID); // 04 -23 Debug
            typeTextEffect.SetMsg(talkData);
            imgPortrait.color = new Color(1, 1, 1, 0);// npc가 아니면 초상화이미지 비활성화
            talkName.gameObject.SetActive(false);
        }

        isActionTalk = true;
        uiEventCk = false;
        talkIndex++;

        if (scanObj_ID == 1000 && questMgr.questId == 10)// 튜토리얼 가이드 팝업 활성화
        {
            switch (talkIndex)
            {
                case 4:
                    talkMgr.guideUi.ChangeGuideImg(GuideState.Stamina);
                    break;
                case 6:
                    talkMgr.guideUi.ChangeGuideImg(GuideState.Shop);
                    break;
                case 9:
                    talkMgr.guideUi.ChangeGuideImg(GuideState.Renovate);
                    break;
                case 11:
                    talkMgr.guideUi.ChangeGuideImg(GuideState.Upgrade);
                    break;
                default:
                    break;
            }
        }
    }
    //06- 11 Add
    private void SetTalkName(Sprite _sp)
    {
        talkName.gameObject.SetActive(true);
        talkName.text = talkMgr.dictTalkName[_sp];
    }

    #region Local_Save_Load_Method
    /*public void GameSave()
    {
        Debug.Log("Run SaveData");
        *//*if (menuSet.activeSelf)
        {
            menuSet.SetActive(false);
        }*//*

        List<Item> saveInventoryItem = new();
        List<Item> saveWearItem = new();

        foreach (Item item in Inventory.Single.items)
        {
            saveInventoryItem.Add(item);
        }

        for (int i = 0; i < targetSlots.Length; i++)
        {
            if (targetSlots[i].wearChek == true && targetSlots[i].item != null)
            {
                saveWearItem.Add(targetSlots[i].item);
            }
        }

        List<Item> saveShopItems = new();
        foreach (var _item in GameUiMgr.single.shopMgr.GetShopSlots())
        {
            saveShopItems.Add(_item.GetItem());
        }

        SaveData gameSaveData = new SaveData(GameMgr.playerData[0].GetPlayerName(), GameMgr.playerData[0].player_level, GameMgr.playerData[0].player_Gold, GameUiMgr.single.questMgr.questId, GameUiMgr.single.questMgr.questActionIndex,
            GameMgr.playerData[0].max_Player_Hp, GameMgr.playerData[0].cur_Player_Hp, GameMgr.playerData[0].max_Player_Sn, GameMgr.playerData[0].cur_Player_Sn, GameMgr.playerData[0].max_Player_Mp, GameMgr.playerData[0].cur_Player_Mp ,
            GameMgr.playerData[0].atk_Speed, GameMgr.playerData[0].atk_Range, GameMgr.playerData[0].base_atk_Dmg ,
            GameMgr.playerData[0].player_max_Exp, GameMgr.playerData[0].player_cur_Exp , 
            saveInventoryItem, saveWearItem, saveShopItems);
        SaveSystem.Save(gameSaveData, "save");

        //  Player DayCount, Player Inventory, Player Desc (Stat, Name, Job, Gold ... ect)
    }
    public void GameLoad()
    {
        SaveData loadData = SaveSystem.Load("save");

        //Load Player Data => save_001.x, save_001.y, save_001.questId, save_001.QuestActionIndex 

        GameMgr.single.OnSelectPlayer(loadData.playerName);

        Debug.Log("PlayerDatas: "+GameMgr.playerData.Count);
        *//*Vector3 lodingPosition = new Vector3(loadData.playerX, loadData.playerY);
        player.transform.position = lodingPosition;*//*
        //Debug.Log("load x, y: "+loadData.playerX +", "+ loadData.playerY);
        //SetNowPosition(loadData.playerX, loadData.playerY);

        GameMgr.playerData[0].max_Player_Hp = loadData.p_max_hp;
        GameMgr.playerData[0].cur_Player_Hp = loadData.p_cur_hp;

        GameMgr.playerData[0].max_Player_Sn = loadData.p_max_sn;
        GameMgr.playerData[0].cur_Player_Sn = loadData.p_cur_sn;

        GameMgr.playerData[0].max_Player_Mp = loadData.p_max_mp;
        GameMgr.playerData[0].cur_Player_Mp = loadData.p_cur_mp;

        GameMgr.playerData[0].player_max_Exp = loadData.p_max_Exp;
        GameMgr.playerData[0].player_cur_Exp = loadData.p_cur_Exp;


        GameMgr.playerData[0].player_Gold = loadData.p_gold;
        GameMgr.playerData[0].player_level = loadData.p_level;


        GameMgr.playerData[0].atk_Speed = loadData.p_atk_speed;
        GameMgr.playerData[0].atk_Range = loadData.p_atk_range;
        GameMgr.playerData[0].base_atk_Dmg = loadData.p_base_atk_Dmg;

        GameMgr.playerData[0].listInventory = loadData.listInven;
        GameMgr.playerData[0].listEquipment = loadData.listEquip;

        LoadInventory(loadData.listInven);
        LoadEquipment(loadData.listEquip);

        if (this.questMgr.questId <= 40)
        {
            questMgr.questId = loadData.questId;
            questMgr.questActionIndex = loadData.questActionIndex;
        }
        else
        {
            Debug.Log("이거 왜 넣었더라");
        }
        questMgr.ControlQuestObejct();
        //GetNowPositon();

        GameUiMgr.single.shopMgr.ReLoadShopItems(loadData.shops);
    }*/
    #endregion
    #region PlayerPosition
    public void SetNowPosition(float x, float y)
    {
        //Debug.Log("set x, y: " + x + ", " + y);
        //lodingPosition.x = x;
        //lodingPosition.y = y;
    }
    public void GetNowPositon()
    {
        //Debug.Log("get x, y: " + lodingPosition.x + ", " + lodingPosition.y);
        //player.transform.position = lodingPosition;
        //Debug.Log("P x, y: " + player.transform.position.x + ", " + player.transform.position.y);
    }
    #endregion

    public void GameSave()//SavePlayerDataToDB()
    {
        Debug.Log("Run SaveData (DB Save)");

        // Inventory 및 Equipment 데이터를 저장할 리스트를 초기화
        List<Item> saveInventoryItem = new();
        List<Item> saveWearItem = new();
        GameMgr.playerData[0].listPartyDatas.Clear();
        GameMgr.playerData[0].listPartyDeparture.Clear();
        // Inventory 및 Equipment의 아이템정보를 저장
        foreach (Item item in Inventory.Single.items)
        {
            saveInventoryItem.Add(item);
        }
        for (int i = 0; i < targetSlots.Length; i++)
        {
            if (targetSlots[i].wearChek == true && targetSlots[i].item != null)
            {
                saveWearItem.Add(targetSlots[i].item);
            }
        }

        if (GameMgr.single.saveState == SaveState.TownSave)
        {
            for (int i = 0; i < GameUiMgr.single.poolPartySlot.Count; i++)
            {
                PartyData pd = GameUiMgr.single.poolPartySlot[i].GetPartyData();
                GameMgr.playerData[0].listPartyDatas.Add(pd);
            }
            for (int i = 0; i < GameUiMgr.single.lastDeparture.Count; i++)
            {
                PartyData pd = GameUiMgr.single.lastDeparture[i].GetPartyData();
                GameMgr.playerData[0].listPartyDeparture.Add(pd);
            }
        }
        // Shop에 있는 아이템들 저장
        List<Item> saveShopItems = new();
        foreach (var _item in GameUiMgr.single.shopMgr.GetShopSlots())
        {
            saveShopItems.Add(_item.GetItem());
        }

        // SaveData 객체 생성
        SaveData gameSaveData = new SaveData(GameMgr.playerData[0].GetPlayerName(), GameMgr.playerData[0].player_level, GameMgr.playerData[0].player_Gold, GameUiMgr.single.questMgr.questId, GameUiMgr.single.questMgr.questActionIndex,
                GameMgr.playerData[0].max_Player_Hp, GameMgr.playerData[0].cur_Player_Hp, GameMgr.playerData[0].max_Player_Sn, GameMgr.playerData[0].cur_Player_Sn, GameMgr.playerData[0].max_Player_Mp, GameMgr.playerData[0].cur_Player_Mp,
                GameMgr.playerData[0].atk_Speed, GameMgr.playerData[0].atk_Range, GameMgr.playerData[0].base_atk_Dmg,
                GameMgr.playerData[0].player_max_Exp, GameMgr.playerData[0].player_cur_Exp, GameMgr.single.tutorial, GameMgr.playerData[0].defensePoint,
                saveInventoryItem, saveWearItem, saveShopItems,
                GameMgr.playerData[0].listPartyDatas, GameMgr.playerData[0].listPartyDeparture, GameMgr.playerData[0].GetPlayerDataDifficulty()
                );

        // SaveData를 DB에 저장
        DBConnector.SaveToDB(gameSaveData, DBConnector.GetUID());
        Debug.Log("Save Success: Data saved to DB.");
    }
    /*public void GameLoad() //LoadPlayerDataFromDB()
    {
        Debug.Log("Run LoadData (DB Load)");

        // DB에서 SaveData 불러오기
        SaveData loadData = DBConnector.LoadFromDB(DBConnector.GetUID());

        if (loadData == null)
        {
            Debug.LogError("No saved data found in DB for userID: " + DBConnector.GetUID());
            return;
        }

        // 불러온 데이터를 적용
        GameMgr.single.OnSelectPlayer(loadData.playerName);

        Debug.Log("PlayerDatas: " + GameMgr.playerData.Count);

        GameMgr.playerData[0].max_Player_Hp = loadData.p_max_hp;
        GameMgr.playerData[0].cur_Player_Hp = loadData.p_cur_hp;

        GameMgr.playerData[0].max_Player_Sn = loadData.p_max_sn;
        GameMgr.playerData[0].cur_Player_Sn = loadData.p_cur_sn;

        GameMgr.playerData[0].max_Player_Mp = loadData.p_max_mp;
        GameMgr.playerData[0].cur_Player_Mp = loadData.p_cur_mp;

        GameMgr.playerData[0].player_max_Exp = loadData.p_max_Exp;
        GameMgr.playerData[0].player_cur_Exp = loadData.p_cur_Exp;

        GameMgr.playerData[0].player_Gold = loadData.p_gold;
        GameMgr.playerData[0].player_level = loadData.p_level;

        GameMgr.playerData[0].atk_Speed = loadData.p_atk_speed;
        GameMgr.playerData[0].atk_Range = loadData.p_atk_range;
        GameMgr.playerData[0].base_atk_Dmg = loadData.p_base_atk_Dmg;

        GameMgr.playerData[0].listInventory = loadData.listInven;
        GameMgr.playerData[0].listEquipment = loadData.listEquip;

        // Load Inventory and Equipment
        LoadInventory(loadData.listInven);
        LoadEquipment(loadData.listEquip);

        if (GameUiMgr.single.questMgr.questId <= 40)
        {
            GameUiMgr.single.questMgr.questId = loadData.questId;
            GameUiMgr.single.questMgr.questActionIndex = loadData.questActionIndex;
        }

        GameUiMgr.single.questMgr.ControlQuestObejct();

        // Shop 아이템 로드
        GameUiMgr.single.shopMgr.ReLoadShopItems(loadData.shops);

        Debug.Log("Load Success: Data loaded from DB.");
    }*/
    public void GameLoad() //최종수정
    {
        Debug.Log("Run LoadData (DB Load)");

        // DB에서 SaveData 불러오기
        SaveData loadData = DBConnector.LoadFromDB(DBConnector.GetUID());

        if (loadData == null)
        {
            Debug.LogError("No saved data found in DB for userID: " + DBConnector.GetUID());
            return;
        }

        // 플레이어 데이터 적용
        GameMgr.single.OnSelectPlayer(loadData.playerName);

        // 플레이어 기본 정보 적용
        GameMgr.playerData[0].max_Player_Hp = loadData.p_max_hp;
        GameMgr.playerData[0].cur_Player_Hp = loadData.p_cur_hp;
        GameMgr.playerData[0].max_Player_Sn = loadData.p_max_sn;
        GameMgr.playerData[0].cur_Player_Sn = loadData.p_cur_sn;
        GameMgr.playerData[0].max_Player_Mp = loadData.p_max_mp;
        GameMgr.playerData[0].cur_Player_Mp = loadData.p_cur_mp;
        GameMgr.playerData[0].player_max_Exp = loadData.p_max_Exp;
        GameMgr.playerData[0].player_cur_Exp = loadData.p_cur_Exp;
        GameMgr.playerData[0].player_Gold = loadData.p_gold;
        GameMgr.playerData[0].player_level = loadData.p_level;
        GameMgr.playerData[0].atk_Speed = loadData.p_atk_speed;
        GameMgr.playerData[0].atk_Range = loadData.p_atk_range;
        GameMgr.playerData[0].base_atk_Dmg = loadData.p_base_atk_Dmg;
        GameMgr.playerData[0].listPartyDatas.AddRange(loadData.listPartyData);
        GameMgr.playerData[0].listPartyDeparture.AddRange(loadData.listPartyDeparture);

        GameMgr.playerData[0].defensePoint = loadData.p_defensePoint;
        // 인벤토리 및 장비 데이터 로드
        LoadInventory(loadData.listInven);
        RedrawSlotUI();
        LoadEquipment(loadData.listEquip);
        RedrawSlotUI();
        // 퀘스트 데이터 로드
        GameMgr.single.tutorial = loadData.tutorialClear;

        GameUiMgr.single.questMgr.questId = loadData.questId;
        GameUiMgr.single.questMgr.questActionIndex = loadData.questActionIndex;

        GameUiMgr.single.questMgr.ControlQuestObejct();

        // 상점 아이템 로드
        GameUiMgr.single.shopMgr.ReLoadShopItems(loadData.shops);
        GameMgr.single.SetPlayerDifficulty((int)loadData.p_playerDifficulty);
        Debug.Log("Load Success: Data loaded from DB.");
    }


    public void OnVideoOption_S1()
    {
        if (videoOption_S1.gameObject.activeSelf)
        {
            videoOption_S1.gameObject.SetActive(false);
        }
        else
        {
            videoOption_S1.gameObject.SetActive(true);
        }
    }
    public void OffVideoOption_S1()
    {
        //videoOption_S1.gameObject.SetActive(false);
    }
    public void ActiveInventory()
    {
        if (questMgr.questId >= 20 && !shopMgr.gameObject.activeSelf)
        {
            activeInventory = !activeInventory;
            if (activeInventory)
            {
                AudioManager.single.PlaySfxClipChange(6);
            }
            inventory_panel.SetActive(activeInventory);
            if (activeInventory == false)
            {
                for (int i = 0; i < Inventory.Single.items.Count; i++)
                {
                    tooltip.gameObject.SetActive(false);
                }
            }
        }
        else
        {
            Debug.Log("퀘스트ID가 20미만");
        }
        tooltip.gameObject.SetActive(false);
    }

    public void OnClickedQuite()
    {
        popUp.SetPopUp("게임을 종료 하시겠습니까?", PopUpState.Quite);
        if (GameUiMgr.single.popUp.gameObject.activeSelf == false)
        {
            GameUiMgr.single.popUp.gameObject.SetActive(true);
            Debug.Log("Run if");
        }
/*#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();*/

    }
    public void OnClickedGameSave()
    {
        popUp.SetPopUp("게임을 저장 하시겠습니까?", PopUpState.GameSave);
        if (GameUiMgr.single.popUp.gameObject.activeSelf == false)
        {
            GameUiMgr.single.popUp.gameObject.SetActive(true);
            Debug.Log("Run if");
        }
    }
    public void OnYesButtonClick()
    {
        AudioManager.single.PlaySfxClipChange(0);
        Debug.Log("Run SFX sound index: 0");
        /*if (isDungeon)//이제사용안함
        {
            AudioManager.single.PlaySfxClipChange(4);
            Debug.Log("던전 입장");
            GameSave();
            //SceneManager.LoadScene("Battle");//아니면여기에 던전에입장하시겠습니까? 예, 아니오, Wall, 값을 넣고 던져서 예누르면 wall로 텔포,아니오누르면 그냥 retrun하게하는식으로하면~ 야매 맵이동구현 뚝딲
            LoadingSceneController.LoadScene("Battle");
            isDungeon = false;
            return;
        }*/

        if (nowSlot.wearChek && nowSlot.GetComponent<Button>().interactable == true)
        {
            //장착해제 Sound
            AudioManager.single.PlaySfxClipChange(2);
            Debug.Log("Run SoundEffect: Equip On/Off");

            TakeOffItem(ref nowSlot);

            AllEquipChek();
            //RedrawSlotUI();
            addEquipPanel.gameObject.SetActive(false);
            return;
        }

        equipmnet = true;
        Debug.Log("AddEquip Name: " + nowSlot.item.itemName);
        Debug.Log("AddEquip Type: " + nowSlot.item.itemType);

        WearEquipment();
        AudioManager.single.PlaySfxClipChange(2);
        Debug.Log("Run SoundEffect: Equip On/Off");
        if (AllEquipChek() && questMgr.questId == 20)
        {
            questMgr.questActionIndex = 1;
        }
        addEquipPanel.gameObject.SetActive(false);
    }
    public void OnNoButtonClick()
    {
        AudioManager.single.PlaySfxClipChange(0);
        Debug.Log("Run SFX sound index: 0");

        equipmnet = false;
        addEquipPanel.gameObject.SetActive(false);
    }
    public void EquipSlotSetting()
    {
        for (int i = 0; i < targetSlots.Length; i++)
        {
            targetSlots[i].usability = true;
            switch (i)
            {
                case 0:
                    targetSlots[i].item.itemType = Item.ItemType.Equipment_Helmet;
                    break;
                case 1:
                    targetSlots[i].item.itemType = Item.ItemType.Equipment_Arrmor;
                    break;
                case 2:
                    targetSlots[i].item.itemType = Item.ItemType.Equipment_Weapon;
                    break;
                case 3:
                    targetSlots[i].item.itemType = Item.ItemType.Equipment_Boots;
                    break;
                default:
                    break;
            }
        }
    } 
    void TargetSlotsRefresh()
    {
        for (int i = 0; i < targetSlots.Length; i++)
        {
            Debug.Log("장착슬롯이있습니다.");
            if (targetSlots[i].item.itemName != string.Empty)
            {
                Debug.Log("Item 있습니다");
                DBConnector.LoadItemByCodeFromDB(targetSlots[i].item.itemCode, ref targetSlots[i].item.itemImage, ref targetSlots[i].item.typeIcon);
                targetSlots[i].itemIcon.sprite = targetSlots[i].item.itemImage;
                if (targetSlots[i].item.modifyStack > 0)
                {
                    Debug.Log("강화수치가있는데왜 안뜨는건데 시ㅣ발아");
                    targetSlots[i].modifyStack.text = "+" + targetSlots[i].item.modifyStack.ToString();
                    targetSlots[i].modifyStack.gameObject.SetActive(true);
                }
            }
        }
    }
    /*
     public void WearEquipment()
    {
        string pk = "";
        // 현재 선택된 슬롯의 아이템을 복제하여 대상 슬롯에 추가
        for (int i = 0; i < targetSlots.Length; i++)
        {
            // 현재 슬롯의 아이템 타입이 일치하는지 확인
            if (targetSlots[i].item.itemType == nowSlot.item.itemType)
            {
                // 장착된 아이템이 있는 경우
                if (targetSlots[i].item.itemName != string.Empty)
                {
                    // 장착된 장비 해제 및 인벤토리에 추가
                    ApplyEquipPower(false, targetSlots[i].item);

                    // 이미 존재하는 아이템을 중복해서 추가하는 것을 방지
                    if (!Inventory.Single.items.Contains(targetSlots[i].item))
                    {
                        Inventory.Single.AddItem(targetSlots[i].item);
                    }
                }

                Debug.Log("Success Equip Add: " + nowSlot.item.itemName);

                // 아이템 복제 및 설정
                pk = nowSlot.item.PrimaryCode;
                Item clonedItem = new Item
                {
                    itemCode = nowSlot.item.itemCode,
                    itemName = nowSlot.item.itemName,
                    itemType = nowSlot.item.itemType,
                    itemTitle = nowSlot.item.itemTitle,
                    itemImage = nowSlot.item.itemImage,
                    itemPrice = nowSlot.item.itemPrice,
                    itemPower = nowSlot.item.itemPower,
                    itemDesc = nowSlot.item.itemDesc,
                    itemStack = nowSlot.item.itemStack,
                    modifyStack = nowSlot.item.modifyStack,
                    PrimaryCode = nowSlot.item.PrimaryCode,
                    typeIcon = nowSlot.item.typeIcon,
                };

                // 타겟 슬롯에 아이템 설정
                targetSlots[i].item = clonedItem;
                targetSlots[i].itemIcon.sprite = nowSlot.itemIcon.sprite;
                targetSlots[i].itemIcon.gameObject.SetActive(true);
                targetSlots[i].wearChek = true;
                targetSlots[i].GetComponent<Button>().interactable = true;

                // UI 업데이트
                targetSlots[i].UpdateSloutUI();

                // 장비 효과 적용
                ApplyEquipPower(true, nowSlot.item);

                break;
            }
        }

        // 사용한 아이템 제거
        for (int i = 0; i < Inventory.Single.items.Count; i++)
        {
            if (Inventory.Single.items[i].PrimaryCode.Equals(pk))
            {
                Inventory.Single.RemoveItem(Inventory.Single.items[i]);
                break;
            }
        }

        // 슬롯 UI 업데이트
        RedrawSlotUI();

        // 현재 슬롯 비우기
        nowSlot = null;

        // 장비 창 닫기
        addEquipPanel.gameObject.SetActive(false);
    }

     *///GPT가 작성해준 디버깅코드 적용은나중에해보겠음 개힘들다진짜...
    public void WearEquipment()
    {
        string pk = "";
        // 현재 선택된 슬롯의 아이템을 복제하여 대상 슬롯에 추가
        for (int i = 0; i < targetSlots.Length; i++)
        {
            if (targetSlots[i].item.itemType == nowSlot.item.itemType)
            {
                if (targetSlots[i].item.itemName != string.Empty)
                {
                    //장착된 장비가 있을 경우, 장착해제하고, 인벤토리에 해당아이템 옮기는 조건문 09-22 Add
                    ApplyEquipPower(false, targetSlots[i].item);
                    Inventory.Single.AddItem(targetSlots[i].item);
                }
                Debug.Log("Success Equip Add: " + nowSlot.item.itemName);
                pk = nowSlot.item.PrimaryCode;
                // 아이템 복제
                //Item clonedItem = nowSlot.item;
                Item clonedItem = new()
                {
                    itemCode = nowSlot.item.itemCode,
                    itemName = nowSlot.item.itemName,
                    itemType = nowSlot.item.itemType,
                    itemTitle = nowSlot.item.itemTitle,
                    itemImage = nowSlot.item.itemImage,
                    itemPrice = nowSlot.item.itemPrice,
                    itemPower = nowSlot.item.itemPower,
                    itemDesc = nowSlot.item.itemDesc,
                    itemStack = nowSlot.item.itemStack,
                    modifyStack = nowSlot.item.modifyStack,
                    PrimaryCode = nowSlot.item.PrimaryCode,
                    typeIcon = nowSlot.item.typeIcon,
                };
                // 아이템 설정
                targetSlots[i].item = clonedItem;

                // 아이콘 설정
                targetSlots[i].itemIcon.sprite = nowSlot.itemIcon.sprite;
                targetSlots[i].itemIcon.gameObject.SetActive(true);
                targetSlots[i].wearChek = true;
                targetSlots[i].GetComponent<Button>().interactable = true;
                
                //UI 설정
                targetSlots[i].UpdateSloutUI();

                ApplyEquipPower(targetSlots[i].wearChek, nowSlot.item);// == ApplyEquipPower(targetSlots[i], clonedItem);
                break;
            }
        }
        // 사용한 아이템 제거 
        for (int i = 0; i < Inventory.Single.items.Count; i++)
        {
            if (Inventory.Single.items[i].PrimaryCode.Equals(pk))
            {
                Inventory.Single.RemoveItem(Inventory.Single.items[i]);
                break;
            }
        }
        //inventory.RemoveItem(item);
        RedrawSlotUI();

        nowSlot = null;

        //addEquipPanel.gameObject.SetActive(false);
    }
    public void TakeOffItem(ref Slot _Slot)
    {
        //매개변수로 넘겨받은 슬롯의 아이템으로 새 아이템을 생성하여.
        Item livingItem = new()
        {
            itemCode = _Slot.item.itemCode,
            itemName = _Slot.item.itemName,
            itemType = _Slot.item.itemType,
            itemImage = _Slot.item.itemImage,
            itemPrice = _Slot.item.itemPrice,
            itemPower = _Slot.item.itemPower,
            itemTitle = _Slot.item.itemTitle,
            itemDesc = _Slot.item.itemDesc,
            itemStack = _Slot.item.itemStack,
            modifyStack = _Slot.item.modifyStack,
            typeIcon = _Slot.item.typeIcon,
            PrimaryCode = _Slot.item.PrimaryCode,
        };

        //일단 장착해제
        _Slot.wearChek = false;//슬롯의 장비가 빠졌으니 fasle로 바꿔줌
        ApplyEquipPower(_Slot.wearChek, livingItem);

        //현재 슬롯의 아이템 지우기
        _Slot.item = new Item
        {
            itemType = livingItem.itemType
        };

        switch (_Slot.item.itemType)
        {
            case Item.ItemType.Equipment_Helmet:
                _Slot.itemIcon.sprite = ItemResources.instance.iconRS[1];
                break;
            case Item.ItemType.Equipment_Arrmor:
                _Slot.itemIcon.sprite = ItemResources.instance.iconRS[2];
                break;
            case Item.ItemType.Equipment_Weapon:
                _Slot.itemIcon.sprite = ItemResources.instance.iconRS[3];
                break;
            case Item.ItemType.Equipment_Boots:
                _Slot.itemIcon.sprite = ItemResources.instance.iconRS[4];
                break;
            default:
                _Slot.itemIcon.sprite = ItemResources.instance.iconRS[0];
                break;
        }
        //_Slot.usability = true;//까먹을까봐 넣어둠 내가 클릭한 슬롯의 주소값을 참조하고있을(확실하진 않은데 그간 경험상 맞을거임) _Slot의 item들과 wearChek만 수정해주면되서 얘는 시작할때 건드려둔거 안 건드려도됨.

        //다시 장착할때 필요한 기본설정 초기화
        //_Slot.item.itemType = livingItem.itemType;//이부분이제 초기화코드넣어서 필요없을거임
        _Slot.GetComponent<Button>().interactable = false;
        _Slot.WearSlotRefresh();

        //인벤토리에 장착 해제한 아이템 추가 후 인벤토리 새로그리기

        /*if (livingItem.itemName != string.Empty)
        {
            if (Inventory.Single.AddItem(livingItem))
            {
                Debug.Log("/??? : " + livingItem.itemName);
                nowSlot = null;
                return;
            }
            else
            {
                Debug.Log("버그발생?");
            }
        }*/

        Inventory.Single.AddItem(livingItem);
        RedrawSlotUI();
        Debug.Log("/??? : " + livingItem.itemName);
        nowSlot = null;
    }

    public void ApplyEquipPower(bool _onoff, Item _equip)//07-20 Fix
    {
        float equipPower;

        if (_onoff == true)
        {
            equipPower = _equip.itemPower;
            Debug.Log("장착: " + equipPower + _onoff);
        }
        else
        {
            equipPower = -1 * (_equip.itemPower);
            Debug.Log("장착해제: " + equipPower + _onoff);
        }

        Debug.Log("Now EquipItem Power: " + _equip.itemPower);

        if (equipPower > 0)
        {
            switch (_equip.itemType)
            {
                case Item.ItemType.Equipment_Helmet:
                    Debug.Log("장착전 HP: " + GameMgr.playerData[0].max_Player_Hp);
                    GameMgr.playerData[0].max_Player_Hp += equipPower;
                    GameMgr.playerData[0].cur_Player_Hp += equipPower;

                    Debug.Log("장착후 HP: " + GameMgr.playerData[0].max_Player_Hp);
                    break;
                case Item.ItemType.Equipment_Arrmor:
                    //Debug.Log("장착전 Range: " + GameMgr.playerData[0].atk_Range);
                    Debug.Log("장착전 Def: " + GameMgr.playerData[0].defensePoint);
                    GameMgr.playerData[0].defensePoint += (int)equipPower;

                    //Debug.Log("장착후 Range: " + GameMgr.playerData[0].atk_Range);
                    Debug.Log("장착후 Def: " + GameMgr.playerData[0].defensePoint);
                    break;
                case Item.ItemType.Equipment_Weapon:
                    Debug.Log("장착전 Dmg: " + GameMgr.playerData[0].base_atk_Dmg);
                    GameMgr.playerData[0].base_atk_Dmg += equipPower;

                    Debug.Log("장착후 Dmg: " + GameMgr.playerData[0].base_atk_Dmg);
                    break;
                case Item.ItemType.Equipment_Boots:
                    Debug.Log("장착전 SPD: " + GameMgr.playerData[0].atk_Speed);
                    GameMgr.playerData[0].atk_Speed += equipPower;

                    Debug.Log("장착후 SPD: " + GameMgr.playerData[0].atk_Speed);
                    break;
                /*case Item.ItemType.Consumables:
                    break;
                case Item.ItemType.Ect:
                    break;*/
                default:
                    break;
            }
        }
        else
        {
            switch (_equip.itemType)
            {
                case Item.ItemType.Equipment_Helmet:
                    Debug.Log("장비 해제 전 HP: " + GameMgr.playerData[0].max_Player_Hp);
                    GameMgr.playerData[0].max_Player_Hp += equipPower;
                    GameMgr.playerData[0].cur_Player_Hp += equipPower;

                    Debug.Log("장비 해제 후 HP: " + GameMgr.playerData[0].max_Player_Hp);
                    break;
                case Item.ItemType.Equipment_Arrmor:
                    Debug.Log("장착전 Def: " + GameMgr.playerData[0].defensePoint);
                    GameMgr.playerData[0].defensePoint += (int)equipPower;
                    Debug.Log("장착후 Def: " + GameMgr.playerData[0].defensePoint);
                    break;
                case Item.ItemType.Equipment_Weapon:
                    Debug.Log("장비 해제 전 Dmg: " + GameMgr.playerData[0].base_atk_Dmg);
                    GameMgr.playerData[0].base_atk_Dmg += equipPower;

                    Debug.Log("장비 해제 후 Dmg: " + GameMgr.playerData[0].base_atk_Dmg);
                    break;
                case Item.ItemType.Equipment_Boots:
                    Debug.Log("장비 해제 전 SPD: " + GameMgr.playerData[0].atk_Speed);
                    GameMgr.playerData[0].atk_Speed += equipPower;

                    Debug.Log("장비 해제 후 SPD: " + GameMgr.playerData[0].atk_Speed);
                    break;
                /*case Item.ItemType.Consumables:
                    break;
                case Item.ItemType.Ect:
                    break;*/
                default:
                    break;
            }
        }


    }
    public bool AllEquipChek()
    {
        int sum = 0;
        for (int i = 0; i < targetSlots.Length; i++)
        {
            if (targetSlots[i].wearChek == true)
            {
                sum++;
            }
        }

        if (sum == (targetSlots.Length))
        {
            if (questMgr.questId < 30)
            {
                questMgr.SetReceptionist(1);
                GameMgr.single.SetPlayerDifficulty(3);
                UpdatePlayerRankAndQuestText((PlayerDifficulty)GameMgr.single.GetPlayerDifficulty());
                Debug.Log("장비 장착 퀘스트 조건 충족");
            }
            //tutorial Quest - wearEquipment
            if (wearEquipment != true)
            {
                wearEquipment = true;
                return true;
            }
            return false;
        }
        else
        {
            if (questMgr.questId < 30)
            {
                questMgr.SetReceptionist(0);
                GameMgr.single.SetPlayerDifficulty(2);
                UpdatePlayerRankAndQuestText((PlayerDifficulty)GameMgr.single.GetPlayerDifficulty());
                Debug.Log("장비 장착 퀘스트 조건 미충족");
            }
        }
        return false;
    }
    public void TutorialDungeonClear()
    {
        if (GameMgr.single.tutorial)
        {
            questMgr.SetReceptionist(1);
            Debug.Log("튜토리얼 던전 클리어 GameMgr.single.tutorial: " + GameMgr.single.tutorial);
        }
        
    }
    #region DBConnect_Load_Method

    //최종수정 2개LoadInventory LoadEquipment
    public void LoadInventory(List<Item> _items)
    {
        Inventory.Single.items.Clear();
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].RemoveSlot();  // 슬롯 초기화
        }

        if (_items == null || _items.Count == 0)
        {
            Debug.Log("No inventory items to load.");
            return;
        }

        // 인벤토리 아이템 추가
        for (int i = 0; i < _items.Count; i++)
        {
            if (_items[i] != null)
            {
                DBConnector.LoadItemByCodeFromDB(_items[i].itemCode,ref _items[i].itemImage, ref _items[i].typeIcon);
                Inventory.Single.items.Add(_items[i]);
                Debug.Log($"Inventory Item Loaded: {_items[i].itemName}");
            }
            else
            {
                Debug.LogWarning("Null item found in inventory list.");
            }
        }
        /*for (int i = 0; i < Inventory.Single.items.Count; i++)
        {
            DBConnector.LoadItemByCodeFromDB(Inventory.Single.items[i].itemCode, ref Inventory.Single.items[i].itemImage, ref Inventory.Single.items[i].typeIcon);
        }*/

    }
    public void LoadEquipment(List<Item> _items)
    {
        if (_items == null || _items.Count == 0)
        {
            Debug.Log("No equipment items to load.");
            return;
        }

        // 슬롯 수와 아이템 수가 맞는지 확인
        if (_items.Count != targetSlots.Length)
        {
            Debug.LogWarning("Loaded equipment items count does not match targetSlots length.");
            return;
        }

        // 장비 슬롯에 아이템 추가
        for (int i = 0; i < targetSlots.Length; i++)
        {
            if (_items[i] != null && _items[i].itemType == targetSlots[i].item?.itemType)
            {
                targetSlots[i].item = _items[i];
                if (targetSlots[i].itemIcon != null)
                {
                    if (_items[i].itemImage != null)
                    {
                        DBConnector.LoadItemByCodeFromDB(_items[i].itemCode, ref _items[i].itemImage, ref _items[i].typeIcon);
                        targetSlots[i].itemIcon.sprite = _items[i].itemImage;
                        targetSlots[i].itemIcon.gameObject.SetActive(true);
                    }
                }

                //targetSlots[i].UpdateSloutUI();
                targetSlots[i].wearChek = true;

                //Debug.Log($"Equipment Loaded: {_items[i].itemName}");
            }
            else
            {
                Debug.LogWarning($"Item in slot {i} is null or type mismatch.");
            }
        }
    }

    /*public void LoadInventory(List<Item> _items)
    {
        Inventory.Single.items.Clear();
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].RemoveSlot();
        }

        if (_items == null || _items.Count == 0) // 예외 처리 추가
        {
            Debug.Log("No inventory items to load.");
            return;
        }

        for (int i = 0; i < _items.Count; i++)
        {
            Inventory.Single.items.Add(_items[i]);
        }

        RedrawSlotUI();
    }
    public void LoadEquipment(List<Item> _items)
    {
        if (_items == null || _items.Count == 0)
        {
            Debug.Log("No equipment items to load.");
            return;
        }

        if (_items.Count != targetSlots.Length)
        {
            Debug.LogWarning("Loaded equipment items count does not match targetSlots length.");
            return;
        }

        // 현재 선택된 슬롯의 아이템을 복제하여 대상 슬롯에 추가
        for (int i = 0; i < targetSlots.Length; i++)
        {
            if (_items[i] != null && targetSlots[i].item != null && targetSlots[i].item.itemType == _items[i].itemType)
            {
                Debug.Log("Success Equip Add: " + _items[i].itemName);

                // 아이콘 설정
                targetSlots[i].itemIcon.sprite = _items[i].itemImage;
                targetSlots[i].itemIcon.gameObject.SetActive(true);
                targetSlots[i].wearChek = true;
                // 아이템 설정
                targetSlots[i].item = _items[i];
            }
        }

        RedrawSlotUI();
    }*/
    #endregion
    #region Local_Load_Methods
    /*public void LoadInventory(List<Item> _items)
    {
        Inventory.Single.items.Clear();
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].RemoveSlot();
        }

        if (_items == null)
            return;
        for (int i = 0; i < _items.Count; i++)
        {
            Inventory.Single.items.Add(_items[i]);
        }

        RedrawSlotUI();
    }*/
    /*public void LoadEquipment(List<Item> _items)
    {
        //if (_items == null || _items.Count == 0 || _items.Count != targetSlots.Length)
        if (_items == null || _items.Count == 0)
        {
            // 만약 _items 리스트가 null이거나 비어있거나 targetSlots과의 길이가 일치하지 않으면 로드를 진행하지 않고 종료합니다.
            Debug.Log("exeption");
            return;
        }

        // 현재 선택된 슬롯의 아이템을 복제하여 대상 슬롯에 추가
        for (int i = 0; i < targetSlots.Length; i++)
        {
            if (targetSlots[i].item.itemType == _items[i].itemType)
            {
                Debug.Log("Success Equip Add: " + _items[i].itemName);

                // 아이콘 설정
                targetSlots[i].itemIcon.sprite = _items[i].itemImage;
                targetSlots[i].itemIcon.gameObject.SetActive(true);
                targetSlots[i].wearChek = true;
                // 아이템 설정
                targetSlots[i].item = _items[i];
            }
        }
        // 사용한 아이템 제거 
        RedrawSlotUI();
    }*/
    #endregion
    //05-12 PartyPanel
    public void ActiveParty()
    {
        if (panelPartyBoard.activeSelf == false)
        {
            panelPartyBoard.SetActive(true);
            AudioManager.single.PlaySfxClipChange(6);
        }
    }
    public void UnActiveParty()
    {
        if (panelPartyBoard.activeSelf == true)
        {
            panelPartyBoard.SetActive(false);
            //AudioManager.single.PlaySfxClipChange(6);
            partytooltip.gameObject.SetActive(false);
        }
    }
    public void ActivePartyDetail()
    {
        if (partyDetails.gameObject.activeSelf == false)
        {
            partyDetails.gameObject.SetActive(true);
            partyDetails.OpenPartyDetail();
        }
        else
        {
            UnActivePartyDetail();
        }
    }
    public void UnActivePartyDetail()
    {
        if (partyDetails.gameObject.activeSelf == true)
        {
            partyDetails.gameObject.SetActive(false);
            partyDetailTooltip.gameObject.SetActive(false);
            //파티디테일툴팁.gameObject.Setactive(false);
        }
    }

    public void ActiveShop()
    {
        if (SnB.activeSelf)
            SnB.SetActive(false);

        if (!shopMgr.gameObject.activeSelf && !activeInventory)
        {
            shopMgr.gameObject.SetActive(true);
            shopMgr.OpenTap(ShopState.BUY);
        }
        else
        {
            shopMgr.gameObject.SetActive(false);
            MoveDoitCk(true);
        }
    }
    public void ActiveBlackSmith()
    {
        if (SnB.activeSelf)
            SnB.SetActive(false);

        if (!blacksmith.gameObject.activeSelf && !activeInventory)
        {
            blacksmith.gameObject.SetActive(true);
            blacksmith.OpenBlacksmith();
        }
        else
        {
            blacksmith.gameObject.SetActive(false);
            MoveDoitCk(true);
        }
    }
    public void MoveDoitCk(bool _onoff)
    {
        move_doit = _onoff;
    }

    public void RefreshiPartyBord()
    {
        blockedPartyBord.SetActive(false);
        btn_PartyCommit.interactable = true;
        //활성화된 슬롯 비 활성화
        foreach (var _slot in poolPartySlot)
        {
            _slot.gameObject.SetActive(false);
            _slot.text_Cost.gameObject.SetActive(true);
            _slot.text_Name.gameObject.SetActive(false);
            //Debug.Log("PartyBordSlots Active: False");
        }
        foreach (var _slot in poolMoveInSlot)
        {
            _slot.gameObject.SetActive(false);
            _slot.text_Cost.gameObject.SetActive(false);
            _slot.text_Name.gameObject.SetActive(true);
            //Debug.Log("MoveInSlots Active: False");
        }
        //MoveInSlot 초기화
        PartyListInPlayer(GetPlayerPrefab());

        //05-23 고용리스트 텍스트 관리
        RefreshiEmploy();

        //06-13 PartyAdd
        OnClickCreateParty();//파티보드 초기화 될 때마다 목록 생성

        // 세이브된 기존의 파티 보드의 데이터가 존재한다면 해당 데이터를 슬롯에 추가해서 활성화, 이거는 다른곳으로 옮기는게 나을듯?
        /*foreach (var nowPartyBord in listPartyData)
        {
            CreatePartySlot(nowPartyBord);
        }*/

    }
    public void CreatePartySlot(PartyData _partyData)
    {
        int activeCount = poolPartySlot.FindAll(s => s.gameObject.activeSelf).Count;
        if (activeCount >= 16)
        {
            return;
        }

        PartySlot partySlot = poolPartySlot.Find(s => !s.gameObject.activeSelf); // 비활성화된 오브젝트 있으면 반환하는 코드
        if (partySlot == null)
        {
            GameObject go = Instantiate(partyPrefab, poolPartySlot[0].transform.parent);
            partySlot = go.GetComponent<PartySlot>();
            poolPartySlot.Add(partySlot);
        }

        partySlot.Init(_partyData);

        partySlot.partySlotIndex = (activeCount +1);
        partySlot.partyData.index = partySlot.partySlotIndex;

        Debug.Log("생성 번호: "+activeCount);
        partySlot.gameObject.SetActive(true);//활성화
    }

    public void OnClickCreateParty()// 모집가능파티원리스트 생성 메서드
    {
        if (GameMgr.playerData[0].listPartyDatas.Count == 0)//l || GameMgr.playerData[0].listPartyDatas!= null && GameMgr.playerData[0].listPartyDatas.Count < 2)
        {
            Debug.Log("---------------------------저장해놓은 파티원목록이 없습니다.");
            for (int i = 0; i < objListPlayable.Count; i++)
            {
                int ran = Random.Range(1, 10);
                PartyData newParty = new(objListPlayable[i], ran);

                Debug.Log("Btn 각 직업별 파티 영입가능인원 생성 ");

                CreatePartySlot(newParty);
                listPartyData.Add(newParty);
            }
            int random = Random.Range(0, 14);

            for (int i = 0; i < random; i++)
            {
                // 0부터 10 사이의 정수 난수 생성 (10은 포함되지 않음)
                int ran = Random.Range(1, 10);
                PartyData newParty = new(objListPlayable[Random.Range(0, objListPlayable.Count)], ran);

                Debug.Log("Btn 파티 영입가능인원 생성 ");

                CreatePartySlot(newParty);
                listPartyData.Add(newParty);// 고용가능 파티원목록리스트를 저장, 여기에서 저장했으니까 씬 넘어갈때 Clier해서 비워줘야겠지??
            }
        }
        else
        {
            Debug.Log("---------------------------저장해놓은 파티원목록이 존재합니다."+ GameMgr.playerData[0].listPartyDatas.Count);
            bool blockedOK = false;
            if (GameMgr.playerData[0].listPartyDeparture != null && GameMgr.playerData[0].listPartyDeparture.Count > 1)//저장된 고용목록이있다면 여기서동작
            {
                Debug.Log("++++++++++++++++++++++++++++++++++++++저장해놓은 고용목록이 존재합니다.");
                for (int i = 0; i < GameMgr.playerData[0].listPartyDatas.Count; i++)//기존에 만들어둔 파티보드의 목록을 생성
                {
                    //Debug.Log("i: " + i);
                    PartyData newParty = new (objListPlayable[GameMgr.playerData[0].listPartyDatas[i].GetPlayerbleObjIndex()], GameMgr.playerData[0].listPartyDatas[i].level);
                    newParty.LoadAttribute(GameMgr.playerData[0].listPartyDatas[i]);
                    newParty.SetPartyCost(GameMgr.playerData[0].listPartyDatas[i].cost);
                    newParty.moveInCk = GameMgr.playerData[0].listPartyDatas[i].moveInCk;
                    listPartyData.Add(newParty);
                    CreatePartySlot(newParty);
                }
                for (int j = 0; j < poolPartySlot.Count; j++)
                {
                    if (poolPartySlot[j].GetPartyData().moveInCk)//저장된고용목록을전부받아다가 newSlots에저장하고 체크표시해주고
                    {
                        Debug.Log("저장된고용목록의 정보를 확인중...");
                        ClickedPartySlot(poolPartySlot[j].GetPartyData());
                        poolPartySlot[j].IsLoadPartySlot();
                        blockedOK = true;
                    }
                }
                if (blockedOK)
                {
                    PartyListInPlayer(GetPlayerPrefab());
                    if (questMgr.questId == 30 && questMgr.questActionIndex == 1)
                        questMgr.SetReceptionist(1);
                    blockedPartyBord.SetActive(true);
                    btn_PartyCommit.interactable = false;
                    int battleIndex = 1;
                    foreach (PartySlot _slot in poolMoveInSlot)
                    {
                        if (_slot.partySlotIndex != 0 && _slot.partyData != null)
                        {
                            _slot.partyData.partyJobIndex = battleIndex++;
                            lastDeparture.Add(_slot);

                            PlayerData _pd = new(
                                _slot.partyData.partyJobIndex,

                                _slot.partyData.partyHp,
                                _slot.partyData.partyMp,

                                _slot.partyData.partyAtkSpd,
                                _slot.partyData.partyAtkRange,
                                _slot.partyData.partyAtk,

                                _slot.partyData.level,
                                _slot.partyData.strPartyName,
                                _slot.partyData.able_Skill,
                                _slot.partyData.isMelee,

                                _slot.partyData.partyDefense,

                                _slot.partyData.jobType,
                                _slot.partyData.Elemental
                                );
                            //_pd.partySlotData = _slot.partyData;// 06-05 수정

                            GameMgr.playerData.Add(_pd);
                            //_pd.playerAttribute = _slot.partyData.Elemental;
                            //_slot.partyData.obj_Data.GetComponent<Ally>().Init(_pd.playerIndex, _pd);
                            Debug.Log("최종파티원LV: " + _slot.partyData.level + ", 파티인덱스 :" + _slot.partyData.partyJobIndex);
                        }

                    }

                    partyDetails.Init(lastDeparture);
                }
            }
            else
            {
                Debug.Log("(++++++++++++++++++++++++++++++++++++++저장해놓은 고용목록이 없습니다.");
                for (int i = 0; i < GameMgr.playerData[0].listPartyDatas.Count; i++)
                {
                    PartyData newParty = new(objListPlayable[GameMgr.playerData[0].listPartyDatas[i].GetPlayerbleObjIndex()], GameMgr.playerData[0].listPartyDatas[i].level);
                    newParty.LoadAttribute(GameMgr.playerData[0].listPartyDatas[i]);
                    newParty.SetPartyCost(GameMgr.playerData[0].listPartyDatas[i].cost);
                    listPartyData.Add(newParty);
                    CreatePartySlot(newParty);
                    for (int j = 0; j < poolPartySlot.Count; j++)
                    {
                        /*if (poolPartySlot[i].moveInChek == true)//여기까지 기존에 저장되어있는 파티데이터 가져와서 파티보드에 표시
                        {
                            poolPartySlot[i].block.SetActive(true);
                            poolPartySlot[i].moveInChek = true;
                            poolPartySlot[i].btnMy.interactable = false;
                            ClickedPartySlot(poolPartySlot[i].partyData);
                        }*/
                        poolPartySlot[i].moveInChek = false;
                    }
                }
            }
        }
        
    }
    
    /*public bool ClickedPartySlot(PartyData _partyData)
    {
        int activeCount = poolMoveInSlot.FindAll(s => s.gameObject.activeSelf).Count;
        if (activeCount >= 4)
        {
            return false;
        }

        // 비활성화된 오브젝트 있으면 반환하는 코드
        PartySlot partySlot = poolMoveInSlot.Find(s => !s.gameObject.activeSelf);
        if (partySlot == null)
        {
            GameObject go = Instantiate(partyPrefab, poolMoveInSlot[0].transform.parent);
            partySlot = go.GetComponent<PartySlot>();
            poolMoveInSlot.Add(partySlot);
        }

        partySlot.Init(_partyData);
        
        partySlot.partySlotIndex = _partyData.index;// 06-04 여기수정
        partySlot.moveInChek = true;
        partySlot.btnMy.interactable = true;

        partySlot.gameObject.SetActive(true);//활성화
        partySlot.text_Cost.gameObject.SetActive(false);
        partySlot.text_Name.gameObject.SetActive(true);

        RefreshiEmploy();
        //listPartyData.Add(_partyData); 시발 이거 왜 안 보임?

        return true;
    }


    public void RestorePartySlot(int _index)
    {
        Debug.Log("Rrestore Index: " + _index);
        foreach (var _slot in poolMoveInSlot)
        {
            if (_slot.partySlotIndex == _index)
            {
                Debug.Log("equal Index: " + _slot.partySlotIndex);
                _slot.gameObject.SetActive(false);

                poolPartySlot[_index].block.SetActive(false);
                poolPartySlot[_index].moveInChek = false;
                poolPartySlot[_index].btnMy.interactable = true;
                
                _slot.ReSetPartySlot();
                //listPartyData.Remove(poolPartySlot[_index].partyData); 시발
                RefreshiEmploy();
                *//*poolMoveInSlot[_index].gameObject.SetActive(false);//비활성화 
                poolPartySlot[_index].block.SetActive(false);
                poolPartySlot[_index].moveInChek = false;*//*
            }
        }
    }*/
    public bool ClickedPartySlot(PartyData _partyData)
    {
        int activeCount = poolMoveInSlot.FindAll(s => s.gameObject.activeSelf).Count;
        if (activeCount >= 4)
        {
            return false;
        }

        // 비활성화된 오브젝트 있으면 반환하는 코드
        PartySlot partySlot = poolMoveInSlot.Find(s => !s.gameObject.activeSelf);
        if (partySlot == null)
        {
            GameObject go = Instantiate(partyPrefab, poolMoveInSlot[0].transform.parent);
            partySlot = go.GetComponent<PartySlot>();
            poolMoveInSlot.Add(partySlot);
        }

        partySlot.Init(_partyData);

        partySlot.partySlotIndex = _partyData.index; // 여기 수정
        partySlot.moveInChek = true;
        partySlot.btnMy.interactable = true;

        partySlot.gameObject.SetActive(true); // 활성화
        partySlot.text_Cost.gameObject.SetActive(false);
        partySlot.text_Name.gameObject.SetActive(true);

        RefreshiEmploy();

        return true;
    }

    public void RestorePartySlot(int _index)
    {
        Debug.Log("Restore Index: " + _index);
        foreach (var _slot in poolMoveInSlot)
        {
            if (_slot.partySlotIndex == _index)
            {
                Debug.Log("Equal Index: " + _slot.partySlotIndex);
                _slot.gameObject.SetActive(false);

                var correspondingSlot = poolPartySlot.Find(s => s.partySlotIndex == _index);
                if (correspondingSlot != null)
                {
                    correspondingSlot.block.SetActive(false);
                    correspondingSlot.moveInChek = false;
                    correspondingSlot.btnMy.interactable = true;
                }
                _slot.ReSetPartySlot();
                RefreshiEmploy();
            }
        }

        foreach(var _slot in poolPartySlot)
        {
            _slot.classIcon.gameObject.SetActive(true);
        }
    }
    public void RefreshiEmploy()
    {
        int countEmploy = 0;
        int sum = 0;
        foreach (PartySlot _slot in poolMoveInSlot)
        {
            if (_slot.partyData != null)
            {
                countEmploy++;
                sum += _slot.intPartyCost;
            }
        }
        textPartyCount.text = "파티원\n " + countEmploy + " / 4"; // countEmploy +" / 4";
        textPartyPrice.text = "금액\n "+ sum + "\n골드";//금액\n 121\n골드
        partyPrice = sum;
    }

    public void PartyCommitBtn()
    {
        popUp.gameObject.transform.GetChild(0).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(600,300);
        popUp.SetPopUp("파티 모집을 \n완료하시겠습니까?", PopUpState.PartyCommit);
    }
    public void EmploymentCompleted()
    {
        PartyListInPlayer(GetPlayerPrefab());

        int battleIndex = 1;
        Debug.Log("고용 전: 현재자금"+ GameMgr.playerData[0].player_Gold);
        Debug.Log("파티원가격: " + partyPrice);
        if ((GameMgr.playerData[0].player_Gold - partyPrice) < 0 )
        {
            Debug.Log("골드 부족");
            AudioManager.single.PlaySfxClipChange(7);
            return; //버튼눌렀는데 골드가 부족하면 실행안됨
        }
        //06-16
        if (questMgr.questId == 30 && questMgr.questActionIndex == 1)
        {
            questMgr.SetReceptionist(1);
        }

        blockedPartyBord.SetActive(true);
        btn_PartyCommit.interactable = false;
        AudioManager.single.PlaySfxClipChange(3);
        GameMgr.playerData[0].player_Gold -= partyPrice;
        GoldChanger();
        Debug.Log("고용 완료: 현재자금" + GameMgr.playerData[0].player_Gold);

        foreach (PartySlot _slot in poolMoveInSlot)
        {
            if (_slot.partySlotIndex != 0 &&_slot.partyData != null)
            {
                _slot.partyData.partyJobIndex = battleIndex++;
                lastDeparture.Add(_slot);

                PlayerData _pd = new(
                    _slot.partyData.partyJobIndex,

                    _slot.partyData.partyHp,
                    _slot.partyData.partyMp,

                    _slot.partyData.partyAtkSpd,
                    _slot.partyData.partyAtkRange,
                    _slot.partyData.partyAtk,

                    _slot.partyData.level,
                    _slot.partyData.strPartyName,

                    _slot.partyData.able_Skill,
                    _slot.partyData.isMelee,
                    _slot.partyData.partyDefense,
                    _slot.partyData.jobType,
                    _slot.partyData.Elemental
                    );
                //_pd.partySlotData = _slot.partyData;// 06-05 수정

                GameMgr.playerData.Add(_pd);
                //_pd.playerAttribute = _slot.partyData.Elemental;

                //_slot.partyData.obj_Data.GetComponent<Ally>().Init(_pd.playerIndex, _pd);
                Debug.Log("최종파티원LV: " + _slot.partyData.level + ", 파티인덱스 :" + _slot.partyData.partyJobIndex);
            }
            
        }

        //여기수정중
        partyDetails.Init(lastDeparture);

        UnEablePartyBoard();
    }
    void EablePartyBoard()//일단만들어만뒀는데 이건쓸일없을듯
    {
        for (int i = 0; i < poolPartySlot.Count; i++)
        {
            poolPartySlot[i].EnebleBtn();
        }
    }
    void UnEablePartyBoard()
    {
        for (int i = 0; i < poolPartySlot.Count; i++)
        {
            poolPartySlot[i].UnEnebleBtn();
        }
    }

    public GameObject GetPlayerPrefab()
    {
        return playerPrefab;
    }

    public void PartyListInPlayer(GameObject playerPrefab)
    {
        lastDeparture.Clear();

        //게임이 최초로 시작될때, lastDepatuar[0]에 PlayerPartyData를 가진 MoveInSlot[0]에 || PartyData를 Player[0]의 Data로 채워서 Slot을만들어 MoveInSlot에 Add하고 
        PartyData pd = new(playerPrefab, GameMgr.playerData[0].player_level);// 초기화 

        poolMoveInSlot[0].partyData = pd;
        poolMoveInSlot[0].gameObject.SetActive(true);
        poolMoveInSlot[0].partyIcon.sprite = playerPrefab.GetComponent<SpriteRenderer>().sprite;
        poolMoveInSlot[0].text_Name.text = GameMgr.playerData[0].GetPlayerName();
        poolMoveInSlot[0].text_Lv.text = "Lv"+GameMgr.playerData[0].player_level.ToString();
        poolMoveInSlot[0].classIcon.sprite = GameUiMgr.single.entityIconRS.GetJobIcon(Ally.Class.Melee);

        listPartyData.Add(poolMoveInSlot[0].partyData);
        lastDeparture.Add(poolMoveInSlot[0]);

        poolMoveInSlot[0].btnMy.interactable = false;
        partyDetails.Init(lastDeparture);//파티디테일 수정 PartyDetail.Cs에서 플레이어혼자만있을때도 보여지게하기위함.  
    }

    public void ChangePlayerPlace(PlaceState _playerState)// 플레이어 스폰 포인트(= arySpawnPoint) 값을 사전에 인스펙터창에서 등록하여 enum값과 통일시켜주어서 State값으로 이동하는기능 
    {
        player.transform.position = arySpawnPoint[((int)_playerState)].position;

        MovePlayerPlace((int)_playerState);
    }
    public void MovePlayerPlace(int _stateIndex)
    {
        nowPlayerPlace = (PlaceState)_stateIndex;
    }

    public void OpenShop()
    {
        if (!shopMgr.gameObject.activeSelf)
        {
            ActiveShop();
        }
    }
    public void OpenBlackSmith()
    {
        if (!blacksmith.gameObject.activeSelf)
        {
            ActiveBlackSmith();
        }

    }
    public PartyDetails GetPartyDetails()
    {
        return partyDetails;
    }

    public void SetAdventurerRateText(string _text)
    {
        tmp_PlayerRating.text = _text;
    }
    public void SetQuestBoardText(string _text, bool _onoff)
    {
        Debug.Log("Run: ChangeQuestBoardText: "+_text);
        questDesc.gameObject.transform.parent.gameObject.transform.parent.gameObject.transform.parent.gameObject.SetActive(_onoff);
        questDesc.text = _text;
    }
    public void SetQuestTitleText(string _text)
    {
        questTitle.text = _text;
    }

    public void UpdatePlayerRankAndQuestText(PlayerDifficulty _playerDifficulty)
    {
        switch (_playerDifficulty)
        {
            case PlayerDifficulty.None:
                break;
            case PlayerDifficulty.Tutorial_Before:
                Debug.Log("GameStart");
                break;
            case PlayerDifficulty.Tutorial_WearEquipBefore:
                questMgr.SetQuestICon(0, 1);// ...
                SetQuestBoardText("장비를 착용하고 다시 말을 걸어보자", true);
                break;
            case PlayerDifficulty.Tutorial_WearEquipAfter:
                questMgr.SetQuestICon(0, 2);// ?
                SetQuestBoardText("장비를 모두 장착했다 접수원에게 보고하자", true);
                //접수원 !를 ?로 변경, 화살표 띄워주기.SetPlayerDifficulty(5);
                break;

            case PlayerDifficulty.Tutorial_PartyBefor:
                questMgr.SetQuestICon(0, 1);// ...
                SetQuestBoardText("게시판에서 파티원을 모집하자", true);
                Debug.LogWarning("이거실행되고있는거아니였냐");
                break;
            case PlayerDifficulty.Tutorial_PartyAfter:
                questMgr.SetQuestICon(0, 2);// ?
                SetQuestBoardText("파티원을 모집했다 접수원에게 보고하자", true);
                break;

            case PlayerDifficulty.Tutorial_DungeonBefor:
                questMgr.SetQuestICon(0, 1);
                SetQuestBoardText("모의 전투에서 승리하자", true);
                break;
            case PlayerDifficulty.Tutorial_DungeonClearAndNotTalk:
                questMgr.SetQuestICon(0, 2);// ?
                SetQuestBoardText("모의전투를 클리어했다 접수원에게 보고하자", true);
                break;
            case PlayerDifficulty.Tutorial_After:
                SetQuestTitleText("새로운 퀘스트");
                break;

            case PlayerDifficulty.Easy_Before:
                questMgr.SetReceptionist(0);
                questMgr.SetQuestICon(0, 0);
                SetQuestTitleText("새로운 퀘스트");
                SetQuestBoardText("접수원과 이야기하자", true);
                break;
            case PlayerDifficulty.Easy_Start:
                questMgr.SetReceptionist(0);
                questMgr.SetQuestICon(0, 1);
                SetQuestTitleText("승급 퀘스트");
                SetQuestBoardText("하급 던전 클리어 0/1", true);
                break;
            case PlayerDifficulty.Easy_DungeonClearAndNotTalk:
                questMgr.SetReceptionist(1);
                questMgr.SetQuestICon(0, 2);
                SetQuestTitleText("승급 퀘스트");
                SetQuestBoardText("하급 던전 클리어 1/1", true);
                break;

            case PlayerDifficulty.Easy_After://중급 모험가
                questMgr.SetReceptionist(0);
                questMgr.SetQuestICon(0, 0);

                SetQuestTitleText("새로운 퀘스트");
                SetQuestBoardText("접수원과 이야기하자", true);
                break;
            case PlayerDifficulty.Normal_Start://중급 모험가
                questMgr.SetReceptionist(0);
                questMgr.SetQuestICon(0, 1);

                SetQuestTitleText("승급 퀘스트");
                SetQuestBoardText("중급 던전 클리어 0/1", true);
                break;
            case PlayerDifficulty.Normal_DungeonClearAndNotTalk://중급 모험가
                questMgr.SetQuestICon(0, 2);

                SetQuestTitleText("승급 퀘스트");
                SetQuestBoardText("중급 던전 클리어 1/1", true);
                break;
            case PlayerDifficulty.Normal_After://상급 모험가
                questMgr.SetReceptionist(0);
                questMgr.SetQuestICon(0, 0);

                SetQuestTitleText("새로운 퀘스트");
                SetQuestBoardText("접수원과 이야기하자", true);
                break;
            case PlayerDifficulty.Hard_Start:
                break;
            case PlayerDifficulty.Hard_DungeonClearAndNotTalk:
                break;
            case PlayerDifficulty.Hard_After:
                break;
            case PlayerDifficulty.Final_Befor:
                break;
            case PlayerDifficulty.Final_Start://상급 모험가
                questMgr.SetReceptionist(0);
                questMgr.SetQuestICon(0, 1);

                SetQuestTitleText("탐험 퀘스트");
                SetQuestBoardText("??? 던전 클리어 0/1", true);
                questMgr.LastDungeonPotalFirstActive(true);
                level_Buttons[4].gameObject.SetActive(true);
                break;
            case PlayerDifficulty.Final_DungeonClearAndNotTalk:
                questMgr.SetReceptionist(1);
                questMgr.SetQuestICon(0, 2);
                SetQuestTitleText("탐험 퀘스트");
                SetQuestBoardText("악마의 미궁 \n클리어 1/1", true);
                break;
            case PlayerDifficulty.Final_After:
                questMgr.SetReceptionist(1);
                questMgr.SetQuestICon(-1, 0);
                SetQuestTitleText("탐험 전문가");
                SetQuestBoardText("탐험 전문가", false);
                break;
            case PlayerDifficulty.GameEnd:
                break;
            default:
                break;
        }

        if ((int)_playerDifficulty > 7 && (int)_playerDifficulty < 23)
        {
            Debug.Log("23인데 왜 안바뀌는거냐고 씨ㅣ발아" + (int)_playerDifficulty + _playerDifficulty);
            Debug.Log("초급따리");
            SetAdventurerRateText("초급 모험가");
        }
        else if ((int)_playerDifficulty > 22 && (int)_playerDifficulty < 33)
        {
            Debug.Log("중급따리");
            SetAdventurerRateText("중급 모험가");
        }
        else if ((int)_playerDifficulty > 32 && (int)_playerDifficulty < 52)
        {
            Debug.Log("상급땃쥐");
            SetAdventurerRateText("상급 모험가");
        }
        else if((int)_playerDifficulty > 51)
        {
            level_Buttons[4].gameObject.GetComponent<SetDungeonTooltip>()._onoff = true;
            SetAdventurerRateText("특급 모험가");
        }
        Debug.Log("현재 호출당시의 숫자 상황" + (int)_playerDifficulty + _playerDifficulty);
    }
}
public enum PlaceState
{
    Guild,
    Town,
    Act
}