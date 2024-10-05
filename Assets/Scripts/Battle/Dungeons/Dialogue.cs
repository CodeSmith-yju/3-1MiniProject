using System.Collections;
using TMPro;
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
    public int quest_cnt;
}


public class Dialogue : MonoBehaviour
{
    [SerializeField] private TMP_Text dialog_Text;
    [SerializeField] private TMP_Text dialog_Name;
    [SerializeField] private Image dialog_Icon;
    

    public bool isTutorial = false;
    private bool isDialogue = false;
    [SerializeField] private bool text_Done = false;
    [SerializeField] private bool isTyping = false;
    private bool isQuest = false;
    public int cnt = 0;

    [SerializeField] private dialogue[] dialogues;

    private void Awake()
    {
        isTutorial = true;
    }

    private void Start()
    {
        cnt = 0;
        ONOFF(true); //대화가 시작됨
        NextDialogue();
    }

    public void ONOFF(bool _flag)
    {
        BattleManager.Instance.ui.dialogue_Box.SetActive(_flag);
        isDialogue = _flag;

        if (cnt >= dialogues.Length)
        {
            BattleManager.Instance.ui.dialogue_Bg.SetActive(false);
        }
        
        if(isDialogue && _flag)
        {
            BattleManager.Instance.ui.dialogue_Bg.SetActive(_flag);
        }
    }

    public void NextDialogue()
    {
        if (isTyping)
        {
            isTyping = false;
        }
        else if (cnt >= dialogues.Length && text_Done)
        {
            Debug.Log("대화종료");
            isTutorial = false;
            ONOFF(false);
        }
        else
        {
            text_Done = false;
            //첫번째 대사와 첫번째 cg부터 계속 다음 cg로 진행되면서 화면에 보이게 된다. 
            StartCoroutine(Typing(dialogues[cnt].dialogue_Text));
            isQuest = dialogues[cnt].isQuest;
            if (dialogues[cnt].isPlayer)
            {
               dialog_Name.text = GameMgr.playerData[0].GetPlayerName();
            }
            else
            {
               dialog_Name.text = dialogues[cnt].dialogue_Name;
            }
            dialog_Icon.sprite = dialogues[cnt].Icon;
            cnt++; //다음 대사와 cg가 나오도록 
        }      
    }
    IEnumerator Typing(string text)
    {
        dialog_Text.text = "";
        bool isTag = false;
        isTyping = true;
        string tagBuffer = "";
        foreach (char letter in text.ToCharArray())
        {
            if (!isTyping)
            {
                TypingEnd();
                yield break;
            }

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
        isTyping = false;

        Debug.Log("타이핑 끝");
        yield break;
    }


    private void TypingEnd()
    {
        dialog_Text.text = dialogues[cnt - 1].dialogue_Text;
        text_Done = true;
    }

        // Update is called once per frame
    private void Update()
    {
        //spacebar 누를 때마다 대사가 진행되도록. 
        if (isDialogue) //활성화가 되었을 때만 대사가 진행되도록
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
            {
                if (isQuest && text_Done)
                {
                    Debug.Log("튜토리얼 시작");
                    BattleManager.Instance.tutorial.Tutorial(dialogues[cnt - 1].quest_cnt); // 튜토리얼 시작
                }
                else
                {
                    NextDialogue();
                }
               
            }
        }
    }
}
