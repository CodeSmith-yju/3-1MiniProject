using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyStatManager : MonoBehaviour
{
    public Enemy enemy;
    public Slider hp;
    public TMP_Text hp_Text;
    bool hp_Check = false;
    public Image attribute_Icon;

    private void Start()
    {
        enemy = GetComponent<Enemy>();
        hp_Check = true;

        attribute_Icon.sprite = GameUiMgr.single.entityIconRS.GetElementIcon(enemy.attribute);
    }

    private void Update()
    {
        if (hp_Check)
        {
            Debug.Log(name + " Ã¼·Â Ã¼Å©µÊ");
            hp_Text.text = $"{enemy.cur_Hp.ToString("0.##")}/{enemy.max_Hp.ToString("0.##")}";
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
            hp_Text.text = $"{enemy.cur_Hp.ToString("0.##")}/{enemy.max_Hp.ToString("0.##")}";
        }

        if (enemy.cur_Hp <= 0 && enemy._curstate == BaseEntity.State.Death)
        {
            hp.gameObject.SetActive(false);
        }
    }
}
