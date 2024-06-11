using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Experimental.Rendering;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class dialogue
{
    public string dialogue_Name;
    public Sprite Icon;
    [TextArea] public string dialogue_Text;
    public bool isPlayer;
    public bool isQuest;
}


public class Dialogue : MonoBehaviour
{
    [SerializeField] private GameObject dialogue_Box;
    [SerializeField] private TMP_Text dialog_Text;
    [SerializeField] private TMP_Text dialog_Name;
    [SerializeField] private Image dialog_Icon;
    public GameObject dialogue_Bg;

    private bool isDialogue = false;
    private bool text_Done = false;
    public bool isQuest = false;
    public int cnt = 0;

    [SerializeField] private dialogue[] dialogues;

    private void Start()
    {
        cnt = 0;
        ONOFF(true); //��ȭ�� ���۵�
    }

    public void ONOFF(bool _flag)
    {
        dialogue_Box.SetActive(_flag);
        isDialogue = _flag;

        if (!isQuest && _flag == false)
        {
            dialogue_Bg.SetActive(_flag);
        }
        
        if(_flag)
        {
            dialogue_Bg.SetActive(_flag);
            NextDialogue();
        }
    }

    public void NextDialogue()
    {
        text_Done = false;
        isQuest = dialogues[cnt].isQuest;
        //ù��° ���� ù��° cg���� ��� ���� cg�� ����Ǹ鼭 ȭ�鿡 ���̰� �ȴ�. 
        StartCoroutine(Typing(dialogues[cnt].dialogue_Text));
        dialog_Name.text = dialogues[cnt].dialogue_Name;
        /*        if (dialogues[cnt].isPlayer)
                {
                    dialog_Name.text = GameMgr.playerData[0].GetPlayerName();
                }
                else
                {
                    dialog_Name.text = dialogues[cnt].dialogue_Name;
                }*/

        dialog_Icon.sprite = dialogues[cnt].Icon;
        cnt++; //���� ���� cg�� �������� 
    }

    IEnumerator Typing(string text)
    {
        dialog_Text.text = "";
        bool isTag = false;
        string tagBuffer = "";
        foreach (char letter in text.ToCharArray())
        {
            if (letter == '<')
            {
                isTag = true;
            }

            if (isTag)
            {
                tagBuffer += letter;
                if (letter == '>')
                {
                    isTag = false;
                    dialog_Text.text += tagBuffer;
                    tagBuffer = "";
                }
            }
            else
            {
                dialog_Text.text += letter;
                yield return new WaitForSeconds(0.05f);
            }
        }

        text_Done = true;
        yield break;
    }


    // Update is called once per frame
    void Update()
    {
        //spacebar ���� ������ ��簡 ����ǵ���. 
        if (isDialogue) //Ȱ��ȭ�� �Ǿ��� ���� ��簡 ����ǵ���
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                //��ȭ�� ���� �˾ƾ���.
                if (cnt < dialogues.Length && text_Done && !isQuest) NextDialogue(); //���� ��簡 �����
                else if (isQuest)
                {
                    ONOFF(false);
                    BattleManager.Instance.Tutorial(cnt);
                }
                else if (cnt >= dialogues.Length) ONOFF(false); //��簡 ����
                
            }
        }
    }
}
