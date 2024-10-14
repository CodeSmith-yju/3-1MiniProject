using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BuffInit : MonoBehaviour
{
    [SerializeField] private Sprite[] buff_Icons;
    private int buff_Index;
    private Dictionary<PlayerData, PlayerStats> temp_Stats = new Dictionary<PlayerData, PlayerStats>();
    private Dictionary<int, PlayerData> playerDataMapping = new Dictionary<int, PlayerData>();
    private HashSet<Ally> buffedPlayers = new HashSet<Ally>();
    private GameObject[] players;

    // 툴팁 오브젝트 참조 (씬에 존재하는 툴팁 오브젝트에 연결)
    [SerializeField] private BuffTooltip buffTooltip;


    private void Start()
    {
        buffTooltip = FindObjectOfType<BuffTooltip>();
        players = GameObject.FindGameObjectsWithTag("Player");

        // 초기화: Ally와 PlayerData 매핑
        foreach (PlayerData data in GameMgr.playerData)
        {
            playerDataMapping[data.playerIndex] = data;
        }

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
                    if (buffedPlayers.Add(player)) // 새로운 플레이어일 때만 버프 적용
                    {
                        Buff(buff_Index, player, playerStat);
                    }
                }
                else // 플레이어가 타일에서 벗어났을 때
                {
                    if (buffedPlayers.Remove(player)) // 제거되었을 때만 버프 해제
                    {
                        RemoveBuff(player, playerStat);
                    }
                }
            }
        }
    }

    private void Buff(int index, Ally player, PlayerData player_Data)
    {
        if (!temp_Stats.ContainsKey(player_Data))
        {
            temp_Stats[player_Data] = new PlayerStats(player_Data.base_atk_Dmg, player_Data.max_Player_Hp, player_Data.max_Player_Mp);
        }


        switch (index) 
        {
            case 0: // 공격력 버프 +10%
                player.atkDmg *= 1.1f;
                break;
            case 1: // 최대 체력 버프 +20%
                float originalMaxHp = player.max_Hp;
                player.max_Hp *= 1.2f;
                player.cur_Hp = player.cur_Hp / originalMaxHp * player.max_Hp;  // 비율 맞춰 현재 체력 증가
                break;
            case 2: // 최대 마나 감소 -1 (스킬 쿨타임 감소)
                player.max_Mp -= 1;
                break;
        }
    }

    private void RemoveBuff(Ally player, PlayerData player_Data)
    {
        if (temp_Stats.ContainsKey(player_Data))
        {
            PlayerStats stats = temp_Stats[player_Data];

            player.atkDmg = stats.temp_Dmg;
            player.max_Hp = stats.temp_MaxHp;
            player.max_Mp = stats.temp_MaxMp;

            player.cur_Hp = player.cur_Hp / player.max_Hp * stats.temp_MaxHp;

            player.cur_Hp = Mathf.Min(player.cur_Hp, player.max_Hp);

            temp_Stats.Remove(player_Data);
        }
    }

    private class PlayerStats
    {
        public float temp_Dmg;
        public float temp_MaxHp;
        public float temp_MaxMp;

        public PlayerStats(float atkDmg, float maxHp, float maxMp)
        {
            temp_Dmg = atkDmg;
            temp_MaxHp = maxHp;
            temp_MaxMp = maxMp;
        }
    }


    // 마우스가 해당 오브젝트에 들어왔을 때
    private void OnMouseEnter()
    {
        ShowTooltip();
    }

    // 마우스가 해당 오브젝트에서 나갔을 때
    private void OnMouseExit()
    {
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
            default: return "알 수 없는 버프입니다.";
        }
    }



}



