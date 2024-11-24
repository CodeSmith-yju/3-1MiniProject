using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Experimental.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
    private static BattleManager instance = null;
    [Header("Manager")]
    public ObjectManager pool;
    public MapManager room;
    public UIManager ui;
    public Dialogue dialogue;
    public TutorialManager tutorial;


    [Header("Battle")]
    public List<GameObject> party_List = new List<GameObject>();
    public List<GameObject> deploy_Player_List = new List<GameObject>();
    public List<GameObject> deploy_Enemy_List = new List<GameObject>();
    public GameObject deploy_area;
    public GameObject unit_deploy_area;
    private bool isFirstEnter;
    private bool battleEnded = false;
    public int event_Stack = 0;
    private bool event_Trigger = false;
    private bool poison_Check = false;
    //public bool isMapDone = false;

    [Header("BuffTile")]
    public Dictionary<PlayerData, PlayerStats> base_Stats = new Dictionary<PlayerData, PlayerStats>();
    public Dictionary<PlayerData, PlayerStats> temp_Stats = new Dictionary<PlayerData, PlayerStats>();
    public bool buff_On = false;

    [Header("Stage")]
    public float dungeon_Level_Scale;

    [Header("Reward")]
    public float exp_Cnt;
    public int gold_Cnt;
    public List<Item> drop_Item = new List<Item>();

    [Header("Total_Reward")]
    public int total_Gold;
    public float total_Exp;
    public List<Item> total_Drop_Item = new List<Item>();

    public static BattleManager Instance
    {
        get
        {
            if (instance == null)
            {
                return null;
            }
            return instance;
        }

    }

    // 스텟을 버프 받기 전의 스텟을 저장 (버프타일)
    public class PlayerStats
    {
        public float temp_Dmg;
        public float temp_MaxHp;
        public float temp_MaxMp;
        public float temp_AtkSpd;
        public int temp_Dp;

        // 버프 타일용
        public PlayerStats(float atkDmg, float maxHp, float maxMp, int dp)
        {
            temp_Dmg = atkDmg;
            temp_MaxHp = maxHp;
            temp_MaxMp = maxMp;
            temp_Dp = dp;
        }

        // 샘물 이벤트용
        public PlayerStats(float atkDmg, float maxHp, float maxMp, float atkSpd, int dp)
        {
            temp_Dmg = atkDmg;
            temp_MaxHp = maxHp;
            temp_MaxMp = maxMp;
            temp_AtkSpd = atkSpd;
            temp_Dp = dp;
        }
    }



    public enum BattlePhase
    {
        Start, // 방 진입 상태 (방의 종류 체크)
        Deploy, // 전투방 입장 후 배치 단계 상태
        Rest, // 전투방이 아닐 때 
        Battle, // 배치 단계에서 배틀 시작 버튼을 눌러 배틀이 시작된 상태
        End // 적이 다 죽었거나, 아군이 다 사망했을 경우 <- 현재는 배치한 파티원이 다 죽으면 끝나는 방식이라 만약 배치를 하지 않은 파티원이 있으면 재시작? 혹은 아예 다 배치되도록 해야할 듯
    }


    public BattlePhase _curphase;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        dungeon_Level_Scale = GameUiMgr.single.dungeon_Level_Scale;

        room = FindObjectOfType<MapManager>();
        isFirstEnter = true;
        
        for (int i = 0; i < GameUiMgr.single.lastDeparture.Count; i++)
        {
            party_List.Add(GameUiMgr.single.lastDeparture[i].partyData.obj_Data);

            // 캐릭터가 가지고 있는 원래 스텟 데이터 (유동이 있는 스텟만 저장함)
            base_Stats[GameMgr.playerData[i]] = new PlayerStats(GameMgr.playerData[i].base_atk_Dmg, 
                GameMgr.playerData[i].max_Player_Hp, 
                GameMgr.playerData[i].max_Player_Mp, 
                GameMgr.playerData[i].atk_Speed,
                GameMgr.playerData[i].defensePoint
                );

            // 디버프, 버프로 유동적으로 이용하는 플레이어 스텟 데이터
            temp_Stats.Add(GameMgr.playerData[i], new PlayerStats(
            base_Stats[GameMgr.playerData[i]].temp_Dmg,
            base_Stats[GameMgr.playerData[i]].temp_MaxHp,
            base_Stats[GameMgr.playerData[i]].temp_MaxMp,
            base_Stats[GameMgr.playerData[i]].temp_AtkSpd,
            base_Stats[GameMgr.playerData[i]].temp_Dp
            ));
        }

        if (GameUiMgr.single.questMgr.questId == 40)
        {
            Debug.Log("NOW QUESTID 40 GOLD: " + GameMgr.playerData[0].player_Gold);
            GameMgr.playerData[0].player_Gold = 1500;
        }

        event_Stack = 0;

        // 소모품 아이템 체크 후 아이템 바에 생성
        SetItem();
    }

    private void Start()
    {
        //ChangePhase(BattlePhase.Start); // 방 체크 (맵 매니저에서 하도록 함)
        AudioManager.single.PlayBgmClipChange(2);
    }

    public IEnumerator BattleReady() // 전투 방일 시 실행되는 메서드
    {
        deploy_area = GameObject.FindGameObjectWithTag("Deploy");
        unit_deploy_area = GameObject.FindGameObjectWithTag("Wait");
        ui.OpenPopup(ui.battle_Ready_Banner);
        yield return StartCoroutine(ui.StartBanner(ui.battle_Ready_Banner));
        yield return new WaitForSeconds(0.15f);

        buff_On = true; // 버프 타일 활성화
        if (event_Trigger)
        {
            EventOccurs(UnityEngine.Random.Range(0, 4));
            event_Trigger = false;
        }
           
        Enemy[] entity = FindObjectsOfType<Enemy>(); // 몬스터를 찾음
        battleEnded = false;

        deploy_area.SetActive(true);
        unit_deploy_area.SetActive(true);

        foreach (GameObject ally in deploy_Player_List)
        {
            EntityDrag drag = ally.GetComponent<EntityDrag>();

            drag.enabled = true;
        }

        int sfx_StartIndex = party_List.Count + 1;

        foreach (Enemy obj in entity)
        {
            obj.sfx_Index = sfx_StartIndex;
            sfx_StartIndex++;

            NavMeshAgent nav = obj.GetComponent<NavMeshAgent>();

            if (obj.gameObject != null)
            {
                deploy_Enemy_List.Add(obj.gameObject);
            }

            if (nav != null)
            {
                nav.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
            }
        }
    }

    private void Update()
    {
        if (_curphase == BattlePhase.Battle && (deploy_Player_List.Count == 0 || deploy_Enemy_List.Count == 0))
        {
            Debug.Log("다 죽음");
            ChangePhase(BattlePhase.End);
        }
    }


    public void ChangePhase(BattlePhase phase)
    {
        _curphase = phase;

        switch (phase) // 각 상태에 진입 했을 때 실행되는 switch문
        {
            case BattlePhase.Start:
                if (room.isMoveDone || isFirstEnter)
                {
                    CheckRoom();
                }
                break;
            case BattlePhase.Rest:
                ui.out_Portal.SetActive(true);
                ui.out_Portal.GetComponent<FadeEffect>().fadeout = true;

                foreach (GameObject ally in deploy_Player_List)
                {
                    EntityDrag drag = ally.GetComponent<EntityDrag>();

                    drag.enabled = false;
                }

                break;
            case BattlePhase.Deploy:
                if (ui.out_Portal.activeSelf)
                    ui.out_Portal.GetComponent<FadeEffect>().fadein = true;

                if (event_Stack != 0)
                {
                    float event_Chance = event_Stack * 3f;

                    event_Chance = Mathf.Clamp(event_Chance, 0f, 50f);

                    float random_Event = UnityEngine.Random.Range(0, 100f);

                    if (random_Event < event_Chance)
                    {
                        event_Trigger = true;
                        event_Stack = 0;
                    }
                }

                StartCoroutine(BattleReady());
                break;
            case BattlePhase.Battle:
                break;
            case BattlePhase.End:
                

                StartCoroutine(EndBattle());
                break;
        }
    }

    public void BattleStartButton()
    {
        StartCoroutine(BattleStart());
    }


    public IEnumerator BattleStart()
    {
        if (deploy_Player_List.Count == 0)
        {
            ui.OpenPopup(ui.alert_Popup);
            ui.alert_Popup.GetComponent<TitleInit>().Init("최소 한명의 파티원을 배치를 해야 합니다.");
            yield break;
        }
        else
        {
            AudioManager.single.PlaySfxClipChange(7);
            ui.OpenPopup(ui.battle_Start_Banner);
            yield return StartCoroutine(ui.StartBanner(ui.battle_Start_Banner));
            yield return new WaitForSeconds(0.15f);

            Debug.Log("배틀 시작");
            ChangePhase(BattlePhase.Battle);
            deploy_area = GameObject.FindGameObjectWithTag("Deploy");
            unit_deploy_area = GameObject.FindGameObjectWithTag("Wait");
            deploy_area.SetActive(false);
            unit_deploy_area.SetActive(false);

            if (_curphase == BattlePhase.Battle)
            {
                BaseEntity[] entity = FindObjectsOfType<BaseEntity>(); // 활성화 된 플레이어나 몬스터를 찾아서 리스트에 넣음

                foreach (BaseEntity obj in entity)
                {
                    NavMeshAgent nav = obj.GetComponent<NavMeshAgent>();
                    EntityDrag drag = obj.GetComponent<EntityDrag>();

                    if (nav != null)
                    {
                        nav.obstacleAvoidanceType = ObstacleAvoidanceType.HighQualityObstacleAvoidance;
                    }
                    if (drag != null)
                    {
                        drag.enabled = false; // 배틀 시작 시에는 플레이어들이 드래그가 안되도록 방지.
                    }
                }

                // 배틀 시작 시 방 안에 있는 적들의 정보의 경험치량, 골드, 드랍 유무를 계산해서 임시변수에 넣음;
                foreach (GameObject enemy in deploy_Enemy_List)
                {
                    Enemy enemy_Obj = enemy.GetComponent<Enemy>();

                    float enemy_Exp = enemy_Obj.exp_Cnt;
                    int enemy_gold = enemy_Obj.gold_Cnt;
                    bool enemy_Item_Drop = enemy_Obj.item_Drop_Check;

                   
                    exp_Cnt += enemy_Exp;
                    gold_Cnt += enemy_gold;

                    if (enemy_Item_Drop)
                    {
                        drop_Item.Add(enemy.GetComponent<Enemy>().GetItemDropTable());
                    }

                    Debug.Log("얻을 수 있는 경험치 량 : " + exp_Cnt);
                    Debug.Log("얻을 수 있는 골드 : " + gold_Cnt);
                }

                if (poison_Check)
                {
                    StartCoroutine(Poison());
                }

            }
        }
    }

    // 중독 이벤트 메서드 (5초마다 체력 감소)
    private IEnumerator Poison()
    {
        while (poison_Check)
        {
            foreach (GameObject player in deploy_Player_List)
            {
                Ally player_Ally = player.GetComponent<Ally>();

                if (player_Ally != null && player_Ally.cur_Hp > 0)
                {
                    // 체력 감소
                    player_Ally.cur_Hp -= 3;

                    // 체력이 0 이하로 떨어지면 행동을 멈추기
                    if (player_Ally.cur_Hp <= 0)
                    {
                        player_Ally.cur_Hp = 0; 
                    }
                }
            }
            yield return new WaitForSeconds(5f);
        }
    }

    private void PlayingResetStats()
    {
        foreach (GameObject player_Obj in deploy_Player_List)
        {
            Ally player = player_Obj.GetComponent<Ally>();
            PlayerData playerData = GameMgr.playerData[player.entity_index];

            // base_Stats에서 해당 플레이어의 원래 스탯 찾기
            if (base_Stats.TryGetValue(playerData, out PlayerStats stats) && playerData.cur_Player_Hp > 0)
            {
                // 원래 스탯 복원
                float healthRatio = (player.max_Hp > 0) ? player.cur_Hp / player.max_Hp : 1;

                playerData.base_atk_Dmg = stats.temp_Dmg;
                playerData.max_Player_Hp = stats.temp_MaxHp;
                playerData.max_Player_Mp = stats.temp_MaxMp;
                player.cur_Mp = 0;
                playerData.cur_Player_Mp = player.cur_Mp;
                playerData.atk_Speed = stats.temp_AtkSpd;
                playerData.defensePoint = stats.temp_Dp;

                player.cur_Hp = Mathf.Clamp(healthRatio * stats.temp_MaxHp, 0, stats.temp_MaxHp);
                playerData.cur_Player_Hp = player.cur_Hp;

                Debug.Log(player_Obj.name + " 스텟 초기화");

                // temp_Stats 초기화
                ResetTempStats(playerData);
            }
        }
    }

    private void EndResetStats()
    {
        foreach (GameObject player_Obj in deploy_Player_List)
        {
            Ally player = player_Obj.GetComponent<Ally>();

            PlayerData playerData = GameMgr.playerData[player.entity_index];

            // base_Stats에서 해당 플레이어의 원래 스탯 찾기
            if (base_Stats.TryGetValue(playerData, out PlayerStats stats))
            {

                playerData.base_atk_Dmg = stats.temp_Dmg;
                playerData.max_Player_Hp = stats.temp_MaxHp;
                playerData.cur_Player_Hp = playerData.max_Player_Hp;
                playerData.max_Player_Mp = stats.temp_MaxMp;
                player.cur_Mp = 0;
                playerData.cur_Player_Mp = player.cur_Mp;
                playerData.atk_Speed = stats.temp_AtkSpd;
                playerData.defensePoint = stats.temp_Dp;
            }
        }
    }

    private void ResetTempStats(PlayerData playerData)
    {
        if (base_Stats.TryGetValue(playerData, out PlayerStats baseStats))
        {
            // base_Stats 값을 temp_Stats로 복사
            if (temp_Stats.ContainsKey(playerData))
            {
                temp_Stats[playerData].temp_Dmg = baseStats.temp_Dmg;
                temp_Stats[playerData].temp_MaxHp = baseStats.temp_MaxHp;
                temp_Stats[playerData].temp_MaxMp = baseStats.temp_MaxMp;
                temp_Stats[playerData].temp_AtkSpd = baseStats.temp_AtkSpd;
            }
        }
    }

    private IEnumerator EndBattle()
    {
        if (_curphase == BattlePhase.End && !battleEnded)
        {
            poison_Check = false;
            StopCoroutine(Poison());
             

            if (deploy_Player_List.Count == 0)
            {
                // 던전 종료 시 원래 스텟으로 리셋
                EndResetStats();
                AudioManager.single.PlaySfxClipChange(10);
                ui.OpenPopup(ui.def_Banner);
                yield return StartCoroutine(ui.Def_Banner());
                yield return new WaitForSeconds(0.15f);
                ui.OpenPopup(ui.def_Popup);
            }
            else if (deploy_Enemy_List.Count == 0)
            {
                // 게임 중 버프 타일로 증가되기 전 스텟으로 리셋
                PlayingResetStats();   
                ui.OpenPopup(ui.vic_Banner);
                AudioManager.single.PlaySfxClipChange(9);
                yield return StartCoroutine(ui.StartBanner(ui.vic_Banner));
                yield return new WaitForSeconds(0.15f);

                
                if (!ui.out_Portal.activeSelf)
                {
                    ui.out_Portal.SetActive(true);
                    ui.out_Portal.GetComponent<FadeEffect>().fadeout = true;
                }

                ui.OpenPopup(ui.reward_Popup);
                

                Debug.Log("얻은 경험치 : " + exp_Cnt);
                RewardPopupInit popup = ui.reward_Popup.GetComponent<RewardPopupInit>();
                popup.Init("전투 승리", false);


                GameObject gold_Obj = Instantiate(ui.reward_Prefab, popup.inner_Gold_Exp);
                gold_Obj.GetComponent<RewardInit>().Init(ui.reward_Icons[0], gold_Cnt + " Gold");
                total_Gold += gold_Cnt;

                GameObject exp_Obj = Instantiate(ui.reward_Prefab, popup.inner_Gold_Exp);
                exp_Obj.GetComponent<RewardInit>().Init(ui.reward_Icons[1], exp_Cnt.ToString("0.##") + " Exp");
                total_Exp += exp_Cnt;


                // 아이템 코드로 중복 드랍된 아이템 찾고 그룹화
                SetIconDropItem(popup, drop_Item);

                foreach (Item item in drop_Item)
                {
                    // 드랍된 아이템들을 총 드랍 아이템 리스트에 넣기
                    total_Drop_Item.Add(item);
                }

                if (!room.FindRoom(room.cur_Room.gameObject).isBoss)
                {
                    ChangePhase(BattlePhase.Rest);
                }

                if (dialogue == null && tutorial == null) // 튜토리얼 전투에서는 이벤트 스택이 오르지 않음
                {
                    event_Stack++;
                }
            }


            if (deploy_Enemy_List.Count == 0 && room.FindRoom(room.cur_Room.gameObject).isBoss)
            {
                EndResetStats(); // 보스 처치 시 스텟을 리셋

                if (AudioManager.single.GetBgmPlayer().isPlaying)
                    AudioManager.single.GetBgmPlayer().Stop();

                if (!ui.out_Portal.activeSelf)
                {
                    ui.out_Portal.SetActive(true);
                    ui.out_Portal.GetComponent<FadeEffect>().fadeout = true;
                }

                Debug.Log("버튼 생성");
                DestroyImmediate(ui.out_Portal.GetComponent<Button>());
                ui.out_Portal.AddComponent<Button>().onClick.AddListener(() => TotalReward());
            }

            BaseEntity[] unit = FindObjectsOfType<BaseEntity>();

            foreach (BaseEntity obj in unit)
            {
                Ally ally = obj as Ally;
                if (ally != null)
                    ally.UpdateCurrentHPMPToSingle();
                Destroy(obj.gameObject);

                foreach (Transform arrow_Obj in pool.obj_Parent)
                {
                    Destroy(arrow_Obj.gameObject);
                }

                pool.Poolclear();
            }

            deploy_Player_List.Clear();
            deploy_Enemy_List.Clear();

        }

        exp_Cnt = 0;
        gold_Cnt = 0;
        drop_Item.Clear();
        battleEnded = true;

        int battleDifficultyChaser = GameMgr.single.GetPlayerDifficulty();
        if (SceneManager.GetActiveScene().name == "Tutorial")
        {
            if (battleDifficultyChaser < 7)
            {
                GameMgr.single.SetPlayerDifficulty(7);
                GameMgr.single.tutorial = true;
            }
        }
        else// easy, nomal, hard, lastBoss
        {
            if (dungeon_Level_Scale == 0.75f)
            {
                if (battleDifficultyChaser == 21)
                {
                    GameMgr.single.SetPlayerDifficulty(22);
                }
            }
            else if(dungeon_Level_Scale == 1f)
            {
                if (battleDifficultyChaser == 31)
                {
                    GameMgr.single.SetPlayerDifficulty(32);
                }
            }
            else if (dungeon_Level_Scale == 1.25f)
            {

            }
            else
            {
                //lastBoss
            }
        }
        
    }


    public void TotalReward()
    {
        if (_curphase == BattlePhase.End)
        {
            Debug.Log("실행됨");
            if (deploy_Enemy_List.Count == 0 && room.FindRoom(room.cur_Room.gameObject).isBoss)
            {
                // 던전 종료 시 원래 스텟으로 리셋
                

                if (!ui.out_Portal.activeSelf)
                {
                    ui.out_Portal.SetActive(true);
                    ui.out_Portal.GetComponent<FadeEffect>().fadeout = true;
                }

                // 총 골드, 경험치를 얻음
                GameMgr.playerData[0].player_Gold += total_Gold;
                GameMgr.playerData[0].GetPlayerExp(total_Exp);

                // 얻은 아이템들을 인벤토리에 추가
                foreach (Item item in total_Drop_Item)
                {
                    Inventory.Single.AddItem(item);
                }

                ui.OpenPopup(ui.vic_Popup);
                RewardPopupInit popup = ui.vic_Popup.GetComponent<RewardPopupInit>();

                DestroyRewardPopup();

                GameObject gold_Obj = Instantiate(ui.reward_Prefab, popup.inner_Gold_Exp);
                gold_Obj.GetComponent<RewardInit>().Init(ui.reward_Icons[0], total_Gold + " Gold");

                GameObject exp_Obj = Instantiate(ui.reward_Prefab, popup.inner_Gold_Exp);
                exp_Obj.GetComponent<RewardInit>().Init(ui.reward_Icons[1], total_Exp.ToString("0.##") + " Exp");

                SetIconDropItem(popup, total_Drop_Item);

            }
        }
    }

    // 방 종류 체크 메서드
    public void CheckRoom()
    {
        // 전 방에 배치된 유닛들 제거
        if (deploy_Player_List != null)
        {
            Ally[] unit = FindObjectsOfType<Ally>();

            if (unit != null)
            {
                foreach (Ally obj in unit)
                {
                    if (obj != null)
                        obj.UpdateCurrentHPMPToSingle();
                    Destroy(obj.gameObject);

                    foreach (Transform arrow_Obj in pool.obj_Parent)
                    {
                        if (arrow_Obj != null)
                            Destroy(arrow_Obj.gameObject);
                    }

                    pool.Poolclear();
                }
            }
        }

        if (AudioManager.single.GetSfxPlayer(9).isPlaying)
            AudioManager.single.GetSfxPlayer(9).Stop();

        deploy_Player_List.Clear();
        deploy_Enemy_List.Clear();

        unit_deploy_area = GameObject.FindGameObjectWithTag("Wait");
        PlacementUnit(); // 어떤 방이든 유닛을 소환 시키도록 함.

        if (room.cur_Room.tag == "Battle")
        {
            Debug.Log("전투 방입니다.");

            if (room.FindRoom(room.cur_Room.gameObject).isBoss)
            {
                AudioManager.single.PlayBgmClipChange(3);
            }
            ChangePhase(BattlePhase.Deploy);
        }
        else
        {
            ChangePhase(BattlePhase.Rest);
            Debug.Log("휴식");
        }
    }

    private void EventOccurs(int event_Value)
    {
        ui.OpenPopup(ui.event_Alert_Popup);
        TitleInit titleInit = ui.event_Alert_Popup.GetComponent<TitleInit>();

        // 플레이어들의 각종 스텟이 감소하는 이벤트를 만듦 (현재 전투 방에서만 발동)
        switch (event_Value) 
        {
            case 0:
                // 5초마다 현재 체력이 지속적으로 감소
                titleInit.Init("'중독' 디버프가 걸립니다.\n(5초마다 지속적으로 체력 -3)");

                // 전투 시 5초마다 지속적으로 체력 -3씩 감소하도록 트리거
                poison_Check = true;

                break;
            case 1:
                // 공격력 감소
                titleInit.Init("'취약' 디버프가 걸립니다.\n(공격력 -10%)");

                foreach (GameObject player in deploy_Player_List)
                {
                    Ally player_Ally = player.GetComponent<Ally>();

                    if (player_Ally != null && player_Ally.cur_Hp > 0)
                    {
                        PlayerData playerData = GameMgr.playerData[player_Ally.entity_index];

                        if (temp_Stats.TryGetValue(playerData, out PlayerStats stats))
                        {
                            player_Ally.atkDmg *= 0.9f;
                            playerData.base_atk_Dmg = player_Ally.atkDmg;

                            stats.temp_Dmg = playerData.base_atk_Dmg;
                        }
                        
                    }
                }
                break;
            case 2:
                // 공격속도 감소
                titleInit.Init("'마비' 디버프가 걸립니다.\n(공격속도 -10%)");

                foreach (GameObject player in deploy_Player_List)
                {
                    Ally player_Ally = player.GetComponent<Ally>();

                    if (player_Ally != null && player_Ally.cur_Hp > 0)
                    {
                        PlayerData playerData = GameMgr.playerData[player_Ally.entity_index];

                        if (temp_Stats.TryGetValue(playerData, out PlayerStats stats))
                        {
                            player_Ally.atkSpd *= 0.9f;
                            playerData.atk_Speed = player_Ally.atkSpd;

                            stats.temp_AtkSpd = playerData.atk_Speed;
                        }

                    }
                }


                break;
            case 3:
                // 최대 MP ( 스킬 쿨타임 증가 ) 증가
                titleInit.Init("'마나결핍' 디버프가 걸립니다.\n(스킬 사용 시 필요 마나 +2)");

                foreach (GameObject player in deploy_Player_List)
                {
                    Ally player_Ally = player.GetComponent<Ally>();

                    if (player_Ally != null && player_Ally.cur_Hp > 0)
                    {
                        PlayerData playerData = GameMgr.playerData[player_Ally.entity_index];

                        if (temp_Stats.TryGetValue(playerData, out PlayerStats stats))
                        {
                            player_Ally.max_Mp += 2;
                            playerData.max_Player_Mp = player_Ally.max_Mp;

                            stats.temp_MaxMp = playerData.max_Player_Mp;
                        }

                    }
                }
                break;
            case 4:
                // 방어력 감소
                titleInit.Init("'약화' 디버프가 걸립니다.\n(방어력 -15 [받는 피해 +6%])");

                foreach (GameObject player in deploy_Player_List)
                {
                    Ally player_Ally = player.GetComponent<Ally>();

                    if (player_Ally != null && player_Ally.cur_Hp > 0)
                    {
                        PlayerData playerData = GameMgr.playerData[player_Ally.entity_index];

                        if (temp_Stats.TryGetValue(playerData, out PlayerStats stats))
                        {
                            player_Ally.def_Point -= 15;
                            playerData.defensePoint = player_Ally.def_Point;

                            stats.temp_Dp = playerData.defensePoint;
                        }

                    }
                }
                break;
        }
    }


    // 배치 단계일때 죽은 파티원을 제외한 나머지 파티원을 배치
    private void PlacementUnit()
    {
        Debug.Log("작동하나 체크");

        Tilemap deployTilemap = unit_deploy_area.GetComponent<Tilemap>();

        BoundsInt bounds = deployTilemap.cellBounds;
        int unit_Cnt = 0;

        foreach (Vector3Int position in bounds.allPositionsWithin)
        {
            if (unit_Cnt >= party_List.Count)
            {
                break;
            }

            if (deployTilemap.HasTile(position) && CanPlace(position))
            {
                if (GameMgr.playerData[unit_Cnt].cur_Player_Hp <= 0)
                {
                    unit_Cnt++;
                    continue;
                }

                // 그 외 유닛들은 생성 하도록 함.
                if (GameMgr.playerData[unit_Cnt].cur_Player_Hp > 0)
                {
                    Debug.Log("작동하나 체크 유닛 생성" + unit_Cnt);
                    Vector3 worldPos = deployTilemap.GetCellCenterWorld(position);

                    GameObject unit = Instantiate(party_List[unit_Cnt], worldPos, Quaternion.identity);
                    unit.GetComponent<Ally>().InitStat(GameMgr.playerData[unit_Cnt].playerIndex);

                    deploy_Player_List.Add(unit);

                    unit_Cnt++; // 다음 유닛 배치하기 위한 인덱스 값
                }
            }
        }
    }

    // 배치 가능한지 체크
    private bool CanPlace(Vector3Int position)
    {
        Collider2D[] colliders = Physics2D.OverlapPointAll(unit_deploy_area.GetComponent<Tilemap>().GetCellCenterWorld(position));

        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                return false; // 이미 해당 위치에 유닛이 있으면 배치할 수 없습니다.
            }
        }

        return true;
    }


    // 던전이 끝나면 마을로 돌아가는 메서드
    public void ReturnToTown()
    {
        total_Exp = 0;
        total_Gold = 0;
        total_Drop_Item.Clear();

        Debug.Log("Now_cur - exp" + GameMgr.playerData[0].player_cur_Exp);
        Debug.Log("Now_max - exp" + GameMgr.playerData[0].player_max_Exp);

        Debug.Log("PlayerData - QID: " + GameUiMgr.single.questMgr.questId);
        Debug.Log("PlayerData - AID: " + GameUiMgr.single.questMgr.questActionIndex);
        Debug.Log("NowGold: " + GameMgr.playerData[0].player_Gold);
        Debug.Log("Now_SN" + GameMgr.playerData[0].cur_Player_Sn);
        Debug.Log("Now_HP" + GameMgr.playerData[0].cur_Player_Hp);

        Debug.Log("Now_Lv" + GameMgr.playerData[0].player_level);
        Debug.Log("Now_cur - exp" + GameMgr.playerData[0].player_cur_Exp);
        Debug.Log("Now_max - exp" + GameMgr.playerData[0].player_max_Exp);

        Debug.Log("Load Type: " + GameMgr.single.LoadChecker());

        GameMgr.playerData[0].cur_Player_Sn -= 15;
        GameMgr.playerData[0].cur_Player_Hp = GameMgr.playerData[0].max_Player_Hp;

        if (!GameMgr.single.GetShopClean())
        {
            GameMgr.single.SetShopClean(true);
        }

        GameUiMgr.single.GameSave();
        GameMgr.single.IsGameLoad(true);
        Debug.Log("Load Type: " + GameMgr.single.LoadChecker());

        LoadingSceneController.LoadScene("Town");
    }

    // 보상 팝업 내용물 초기화
    public void DestroyRewardPopup()
    {
        RewardPopupInit popup = ui.reward_Popup.GetComponent<RewardPopupInit>();

        foreach (Transform child in popup.inner_Gold_Exp.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (Transform item_Child in popup.inner_Item.transform)
        {
            Destroy(item_Child.gameObject);
        }

    }

    public void EventSelect(bool check)
    {
        if (check)
        {
            // 전체 체력 증가 및 이벤트 스택 증가
            foreach(PlayerData player in GameMgr.playerData)
            {
                if (player.cur_Player_Hp > 0)
                {
                    if (player.cur_Player_Hp + 20 >= player.max_Player_Hp)
                    {
                        player.cur_Player_Hp = player.max_Player_Hp;
                    }
                    else
                    {
                        player.cur_Player_Hp += 20;
                    }
                }
            }

            ui.CancelPopup(ui.event_Popup);

            if (dialogue == null && tutorial == null)
            {
                event_Stack += 2;

                ui.OpenPopup(ui.alert_Popup);
                TitleInit titleInit = ui.alert_Popup.GetComponent<TitleInit>();
                titleInit.Init("무슨 일이 일어날 것 같습니다.");
            }
        }
        else
        {
            ui.CancelPopup(ui.event_Popup);
        }

        if (dialogue != null && dialogue.isTutorial)
        {
            tutorial.EndTutorial(21);
        }
    }


    // 인벤토리 아이템 불러오기
    private void SetItem()
    {
        int index = 0;
        for (int i = 0; i < 10; i++)
        {
            ItemUse iia = Instantiate(ui.item_Slot_Prefabs, ui.item_Bar.transform.GetChild(0));

            // 생성된 슬롯 초기화
            Item item = SetInnerItem(ref index);

            iia.Init(item);

            // 생성된 슬롯을 리스트에 추가
            //Inner.IiaList.Add(iia);
        }

    }

    Item SetInnerItem(ref int _index)
    {
        for (int i = _index; i < Inventory.Single.items.Count; i++)
        {
            if (Inventory.Single.items[i].itemType == Item.ItemType.Consumables)
            {
                if (Inventory.Single.items[i].itemCode == 6 || Inventory.Single.items[i].itemCode == 1)
                {
                    _index = i + 1;
                    return Inventory.Single.items[i];
                }
            }
        }
        return null;
    }

    // 드랍 아이템들을 아이템 코드로 그룹화 하여 아이콘을 생성하는 메서드 (LinQ 사용)
    public void SetIconDropItem(RewardPopupInit popup, List<Item> drop_List)
    {
        var item_Cnt = drop_List
                    .GroupBy(item => item.itemCode)
                    .Select(group => new { item = group.First(), count = group.Count() })
                    .ToList();

        if (drop_List.Count == 0)
        {
            popup.null_Item_Text.gameObject.SetActive(true);
            return;
        }
        else
        {
            popup.null_Item_Text.gameObject.SetActive(false);
            // 그룹화 된 아이템들을 아이콘으로 생성
            foreach (var drop in item_Cnt)
            {
                if (drop != null)
                {
                    GameObject item_Obj = Instantiate(ui.reward_Item_Prefab, popup.inner_Item);
                    item_Obj.GetComponent<SetDropItem>().Init(drop.item, drop.count);

                }
            }
        }
    }



}
