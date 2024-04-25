using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyStatManager : MonoBehaviour
{
    public BaseEntity enemy;
    public Slider hp;
    public TMP_Text hp_Text;
    bool hp_Check = false;
    //public Slider mp;
    //public TMP_Text mp_Text;

    private void Start()
    {
        enemy = GetComponent<BaseEntity>();
        hp_Check = true;
    }

    private void Update()
    {
        if (hp_Check)
        {
            Debug.Log(name + " ü�� üũ��");
            hp_Text.text = $"{enemy.cur_Hp} / {enemy.max_Hp}";
            hp_Check = false;
            
        }

        if (BattleManager.Instance._curphase == BattleManager.BattlePhase.Battle)
        {
            UpdateStatus();
        }
    }


    private void UpdateStatus()
    {
        if (enemy != null && enemy.cur_Hp >= 0)
        {
            hp.value = enemy.cur_Hp / enemy.max_Hp;
            hp_Text.text = $"{enemy.cur_Hp} / {enemy.max_Hp}";
            //mp.value = player.cur_Mp / player.max_Mp;
            //mp_Text.text = $"{enemy.cur_Mp} / {enemy.max_Mp}";
        }

        if (enemy.cur_Hp <= 0 && enemy._curstate == BaseEntity.State.Death)
        {
            hp.gameObject.SetActive(false);
        }
    }


}
