using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BoxOpen : MonoBehaviour
{
    public Animator ani;
    public GameObject mimic;
    int isMimic;
    bool isOpening = false;

    private void Awake()
    {
        ani = GetComponent<Animator>();
    }

    private void Start()
    {
        if (SceneManager.GetActiveScene().name != "Tutorial") 
        {
            isMimic = Random.Range(0, 2);
            Debug.Log(isMimic + " = 1 : �̹�, 0 : ����");

            if (isMimic == 1)
            {
                gameObject.tag = "Mimic";
            }
        }
    }

    private void OnMouseDown()
    {
        if (!isOpening)
        {
            ani.SetTrigger("isOpen");
            isOpening = true;
        }
    }


    public void OnAniDone()
    {
        if (gameObject.CompareTag("Mimic"))
        {
            if (BattleManager.Instance.dialogue != null)
            {
                if (BattleManager.Instance.dialogue.isTutorial)
                {
                    BattleManager.Instance.tutorial.EndTutorial(18);
                }
            }
            
            GameObject mimics = Instantiate(mimic, gameObject.transform.parent);
            mimics.transform.position = gameObject.transform.position;
            gameObject.SetActive(false);

         
            BattleManager.Instance.room.cur_Room.gameObject.tag = "Battle";
            BattleManager.Instance.ChangePhase(BattleManager.BattlePhase.Deploy);

        }
        else
        {
            BattleManager.Instance.ui.OpenPopup(BattleManager.Instance.ui.reward_Popup);
            RewardPopupInit reward = BattleManager.Instance.ui.reward_Popup.GetComponent<RewardPopupInit>();
            reward.Init("����", true);
            if (BattleManager.Instance.dialogue != null)
            {
                if (BattleManager.Instance.dialogue.isTutorial)
                    BattleManager.Instance.ui.ui_Tutorial_Box.SetActive(false);
            }
            

            int gold = Random.Range(150, 250);

            GameObject gold_Obj = Instantiate(BattleManager.Instance.ui.reward_Prefab, reward.inner_Gold_Exp);
            gold_Obj.GetComponent<RewardInit>().Init(BattleManager.Instance.ui.reward_Icons[0], gold + " Gold");

            BattleManager.Instance.total_Gold += gold;

            gameObject.SetActive(false);
            return;
        }
    }
}
