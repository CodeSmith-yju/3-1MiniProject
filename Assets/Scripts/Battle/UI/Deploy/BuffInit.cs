using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BuffInit : MonoBehaviour
{
    [SerializeField] private Sprite[] buff_Icons;
    private int buff_Index;
    private Dictionary<int, PlayerData> playerDataMapping = new Dictionary<int, PlayerData>();
    [SerializeField] private GameObject[] players;
    private HashSet<Ally> buffedPlayers = new HashSet<Ally>();

    // 툴팁 오브젝트 참조 (씬에 존재하는 툴팁 오브젝트에 연결)
    [SerializeField] private BuffTooltip buffTooltip;
    //private bool buff_Check = false;


    private void Start()
    {
        buffTooltip = FindObjectOfType<BuffTooltip>();
        players = GameObject.FindGameObjectsWithTag("Player");

        // 초기화: Ally와 PlayerData 매핑
        foreach (PlayerData data in GameMgr.playerData)
        {
            playerDataMapping[data.playerIndex] = data;
        }

        //buff_Check = false;
    }

    public void Init(int index)
    {
        transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = buff_Icons[index];
        this.buff_Index = index;
    }



    private void Update()
    {
        if (BattleManager.Instance._curphase == BattleManager.BattlePhase.Deploy)
        {
            CheckPlayerDistances();
        }

        if (BattleManager.Instance._curphase == BattleManager.BattlePhase.Battle)
        {
            if (buffTooltip.transform.GetChild(0).gameObject.activeSelf)
                HideTooltip();
        }
    }


    // 매 프레임마다 플레이어들과의 거리 체크
    private void CheckPlayerDistances()
    {
        foreach (GameObject playerObj in players)
        {
            Ally player = playerObj.GetComponent<Ally>();
            if (player == null) continue;

            // 해당 플레이어의 PlayerData 가져오기
            if (playerDataMapping.TryGetValue(player.entity_index, out PlayerData playerStat))
            {
                float distance = Vector3.Distance(transform.position, player.transform.position);

                if (distance < 0.1f) // 플레이어가 버프 타일 위에 있을 때
                {
                    if (!buffedPlayers.Contains(player))
                    {
                        Buff(buff_Index, player, playerStat);
                        buffedPlayers.Add(player);
                        //buff_Check = true;
                        break;
                    }
                }
                else // 플레이어가 타일에서 벗어났을 때
                {
                    if (buffedPlayers.Contains(player))
                    {
                        //buff_Check = false;
                        RemoveBuff(player, playerStat);
                        buffedPlayers.Remove(player);
                        break;
                    }
                }
            }
        }
    }

    private void Buff(int index, Ally player, PlayerData player_Data)
    {

        switch (index) 
        {
            case 0: // 공격력 버프 +10%
                player.atkDmg *= 1.1f;
                player_Data.base_atk_Dmg = player.atkDmg;
                Debug.Log($"{player.name}의 공격력 버프 적용: {player.atkDmg}");
                break;
            case 1: // 최대 체력 버프 +20%
                float originalMaxHp = player.max_Hp;
                player.max_Hp *= 1.2f;
                float healthRatio = (originalMaxHp > 0) ? player.cur_Hp / originalMaxHp : 0;
                player.cur_Hp = Mathf.Clamp(healthRatio * player.max_Hp, 0, player.max_Hp);
                player_Data.max_Player_Hp = player.max_Hp;
                player_Data.cur_Player_Hp = player.cur_Hp;
                Debug.Log($"{player.name}의 최대체력 버프 적용: {player.max_Hp}");
                break;
            case 2: // 최대 마나 감소 -1 (스킬 쿨타임 감소)
                player.max_Mp -= 1;
                player_Data.max_Player_Mp = player.max_Mp;
                Debug.Log($"{player.name}의 마나 버프 적용: {player.max_Mp}");
                break;
            case 3: // 방어력 버프 +25 (10% 피해 감소)
                player.def_Point += 25;
                player_Data.defensePoint = player.def_Point;
                break;
        }
    }

    private void RemoveBuff(Ally player, PlayerData player_Data)
    {
        if (BattleManager.Instance.temp_Stats.TryGetValue(player_Data, out BattleManager.PlayerStats stats))
        {
            float healthRatio = (player.max_Hp > 0) ? player.cur_Hp / player.max_Hp : 1;

            player.atkDmg = stats.temp_Dmg;
            player.max_Hp = stats.temp_MaxHp;
            player.max_Mp = stats.temp_MaxMp;
            player.def_Point = stats.temp_Dp;

            // 체력 비율에 따라 현재 체력 조정 및 클램핑
            player.cur_Hp = Mathf.Clamp(healthRatio * stats.temp_MaxHp, 0, stats.temp_MaxHp);

            player_Data.base_atk_Dmg = stats.temp_Dmg;
            player_Data.max_Player_Mp = stats.temp_MaxMp;
            player_Data.max_Player_Hp = stats.temp_MaxHp;
            player_Data.cur_Player_Hp = player.cur_Hp;
            player_Data.defensePoint = stats.temp_Dp;


            //temp_Stats.Remove(player);
            //BattleManager.Instance.temp_Stats.Remove(player_Data);
        }
    }

    // 마우스가 해당 오브젝트에 들어왔을 때
    private void OnMouseEnter()
    {
        if (BattleManager.Instance._curphase != BattleManager.BattlePhase.Battle)
        {
            if (BattleManager.Instance.dialogue != null)
            {
                if (!BattleManager.Instance.ui.popup_Bg.activeSelf && !BattleManager.Instance.ui.dialogue_Bg.activeSelf && !BattleManager.Instance.ui.item_Use_UI.activeSelf && !BattleManager.Instance.ui.isOpenUI)
                    ShowTooltip();
            }
            else
            {
                if (!BattleManager.Instance.ui.popup_Bg.activeSelf && !BattleManager.Instance.ui.item_Use_UI.activeSelf && !BattleManager.Instance.ui.isOpenUI)
                    ShowTooltip();
            }
        }
        else
        {
            return;
        }
    }

    // 마우스가 해당 오브젝트에서 나갔을 때
    private void OnMouseExit()
    {
        if (buffTooltip.transform.GetChild(0).gameObject.activeSelf)
            HideTooltip();
    }

    private void OnMouseOver()
    {
        if (buffTooltip.transform.GetChild(0).gameObject.activeSelf)
            UpdateTooltipPosition();
    }

    private void ShowTooltip()
    {
        string description = GetBuffDescription(buff_Index);
        Sprite icon = buff_Icons[buff_Index];
        buffTooltip.Init(description, icon); // 툴팁 초기화
        buffTooltip.transform.GetChild(0).gameObject.SetActive(true); // 툴팁 표시
    }

    private void UpdateTooltipPosition()
    {
        if (buffTooltip.transform.GetChild(0).gameObject.activeSelf)
        {
            // 기본 마우스 위치에 오프셋 추가 (오른쪽 위로 이동)
            Vector3 tooltipPosition = Input.mousePosition + new Vector3(20, -20, 0);

            // 툴팁의 RectTransform 가져오기
            RectTransform tooltipRect = buffTooltip.transform.GetChild(0).GetComponent<RectTransform>();

            // 피벗을 오른쪽 위로 설정
            tooltipRect.pivot = new Vector2(0, 1);

            // 화면 경계를 고려한 위치 조정
            float clampedX = Mathf.Clamp(tooltipPosition.x, 0, Screen.width - tooltipRect.rect.width);
            float clampedY = Mathf.Clamp(tooltipPosition.y, 0, Screen.height - tooltipRect.rect.height);

            // 조정된 위치로 툴팁 배치
            buffTooltip.transform.position = new Vector3(clampedX, clampedY, 0);
        }
    }


    private void HideTooltip()
    {
        buffTooltip.transform.GetChild(0).gameObject.SetActive(false);
    }

    private string GetBuffDescription(int index)
    {
        switch (index)
        {
            case 0: return "공격력 +10%";
            case 1: return "최대 체력 +20%";
            case 2: return "최대 마나 -1";
            case 3: return "방어력 +25 (받는 피해 -10%)";
            default: return "알 수 없는 버프입니다.";
        }
    }



}



