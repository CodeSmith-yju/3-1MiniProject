using System.Collections;
using System.Collections.Generic;
using System.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatManager : MonoBehaviour
{
    public BaseEntity player;
    public Slider hp;
    public TMP_Text hp_Text;
    public Slider mp;
    public TMP_Text mp_Text;
    public Image player_Icon;
    public GameObject entry_Check;
    public GameObject dead_Check;
    bool isDeloy = false;

    public void InitStat(BaseEntity player, Sprite icon)
    {
        this.player = player;
        this.player_Icon.sprite = icon;
        isDeloy = false;
    }

    private void Start()
    {
        // ��ġ�� �÷��̾��� ���� ���̵�� ���� ���̵� ���� ����â�� ã�Ƽ� ������Ʈ�� ����(ü��, ���� ��)�� �־��� (�̸� �����տ� Id�� ����������� -> ������� ���� ����ؾߵ� �κ�)
       // FindUnitStat();
    }

    private void OnEnable()
    {
        //FindUnitStat();
    }

/*    private void FindUnitStat()
    {
        foreach (GameObject obj in BattleManager.Instance.deploy_Player_List)
        {
            BaseEntity entity = obj.GetComponent<BaseEntity>();
            if (entity.entity_id == player.entity_id)
            {
                player = entity;
                Debug.Log("���� �ֱ� " + obj);
                isDeloy = true;
                break;
            }
            else
            {
                isDeloy = false;
            }
        }
    }
*/
    private void Update()
    {
        if (BattleManager.Instance._curphase == BattleManager.BattlePhase.Battle)
        {
            UpdateStatus();
        }
    }

    private void UpdateStatus()
    {
        if (isDeloy && player != null && player.cur_Hp >= 0) 
        {
            entry_Check.SetActive(false);
            hp.value = player.cur_Hp / player.max_Hp;
            hp_Text.text = $"{player.cur_Hp} / {player.max_Hp}";
            mp_Text.text = $"{player.cur_Mp} / {player.max_Mp}";
            mp.value = player.cur_Mp / player.max_Mp;
            dead_Check.SetActive(false);
        }
        else if (!isDeloy && player != null && player.cur_Hp >= 0)
        {
            entry_Check.SetActive(true);
        }

        if (player != null && player.cur_Hp <= 0 && player._curstate == BaseEntity.State.Death)
        {
            entry_Check.SetActive(false);
            dead_Check.SetActive(true);
        }
    }

}
