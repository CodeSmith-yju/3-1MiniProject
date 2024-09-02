using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxOpen : MonoBehaviour
{
    public Animator ani;
    public GameObject mimic;
    bool isSetTutorial;
    int isMimic;

    private void Awake()
    {
        ani = GetComponent<Animator>();
    }

    private void Start()
    {
        if (!BattleManager.Instance.dialogue.isTutorial)
        {
            isMimic = Random.Range(0, 2);

            if (isMimic == 1)
            {
                gameObject.tag = "Mimic";
            }
        }
    }

    private void OnMouseDown()
    {
        if (!isSetTutorial)
        {
            ani.SetTrigger("isOpen");
        }
    }


    public void OnAniDone()
    {
        if (gameObject.CompareTag("Mimic"))
        {
            if (BattleManager.Instance.dialogue.isTutorial)
            {
                BattleManager.Instance.tutorial.EndTutorial(18);
            }

            GameObject mimics = Instantiate(mimic, gameObject.transform.parent);
            mimics.transform.position = gameObject.transform.position;
            gameObject.SetActive(false);

         
            BattleManager.Instance.room.cur_Room.gameObject.tag = "Battle";
            BattleManager.Instance.CheckRoom();

        }
        else
        {
            BattleManager.Instance.ui.OpenPopup(BattleManager.Instance.ui.reward_Popup);
            RewardPopupInit reward = BattleManager.Instance.ui.reward_Popup.GetComponent<RewardPopupInit>();
            reward.Init("»óÀÚ", true);
            if (BattleManager.Instance.dialogue.isTutorial)
                BattleManager.Instance.ui.ui_Tutorial_Box.SetActive(false);

            int gold = Random.Range(10, 101);

            GameObject gold_Obj = Instantiate(BattleManager.Instance.ui.reward_Prefab, reward.inner_Main);
            gold_Obj.GetComponent<RewardInit>().Init(BattleManager.Instance.ui.reward_Icons[0], gold + " Gold");

            BattleManager.Instance.total_Gold += gold;
            GameMgr.playerData[0].player_Gold += gold;

            gameObject.SetActive(false);
            return;
        }
    }


    public void SetTutorial(bool setTutorial)
    {
        isSetTutorial = setTutorial;
    }
 
}
