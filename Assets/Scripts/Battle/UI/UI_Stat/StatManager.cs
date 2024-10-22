using System.Collections;
using System.Collections.Generic;
using System.Data;
using TMPro;
using Unity.Jobs.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class StatManager : MonoBehaviour
{
    public PlayerData player;

    [Header("Player_Stat")]
    public Slider hp;
    public Slider mp;

    [Header("Player_Text")]
    public TMP_Text hp_Text;
    public TMP_Text mp_Text;
    public TMP_Text level_Text;
    public TMP_Text name_Text;

    [Header("Image")]
    public Image player_Icon;
    public GameObject dead_Check;
    public Image attribute_Icon;

    public bool isDead;

    public void InitStat(PlayerData player, Sprite portrait, int level, string name)
    {
        this.player = player;
        player_Icon.sprite = portrait;
        attribute_Icon.sprite = GameUiMgr.single.entityIconRS.dictn_ElementIcon[player.playerAttribute];

        this.level_Text.text = level.ToString();
        this.name_Text.text = name;
    }

    private void Start()
    {
        UnitStat();
    }
    
    private void UnitStat()
    {
        foreach (PlayerData data in GameMgr.playerData)
        {
            if (data.playerIndex == player.playerIndex)
            {
                UpdateStatus();
                break;
            }
        }
    }

    private void Update()
    {
        if (player != null && !isDead)
            UpdateStatus();
    }

    public void UpdateStatus()
    {
        // 실시간으로 HP, MP 업데이트
        hp.value = player.cur_Player_Hp / player.max_Player_Hp;
        hp_Text.text = $"{player.cur_Player_Hp.ToString("0.##")}/{player.max_Player_Hp.ToString("0.##")}";
        mp.value = player.cur_Player_Mp / player.max_Player_Mp;
        mp_Text.text = $"{player.cur_Player_Mp}/{player.max_Player_Mp}";

        if (player.cur_Player_Hp > 0)
        {
            dead_Check.SetActive(false);
            isDead = false;
        }
        else
        {
            dead_Check.SetActive(true);
            isDead = true;
        }
    }
}
