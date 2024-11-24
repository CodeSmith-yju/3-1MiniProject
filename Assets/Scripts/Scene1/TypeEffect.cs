using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TypeEffect : MonoBehaviour
{
    
    private string targetMsg;// 실제 출력될 대화내용을 가지고있다가 TMP에 하나씩뿌려줄 문자열변수 
    [SerializeField]private TextMeshProUGUI msgText;//대화창에 표시될 TMP
    private int charIndex;
    private float interval;

    public bool isAnim;//현재 대화애니메이션이 진행중인지 확인할 변수
    [Header("Anima Speed")]
    public int charPerSeconds;// 텍스트 대화 속도
    [Header("Cursor")]
    public GameObject endCursor;

    public void SetMsg(string msg)
    {
        AudioManager.single.PlaySfxClipChange(15);
        if (isAnim)
        {
            msgText.text = targetMsg;
            CancelInvoke();//지금 돌고있는 인보크함수가 꺼짐
            EffectEnd();
        }
        else
        {
            targetMsg = msg;
            EffectStart();
        }
    }

    private void EffectStart()
    {
        AudioManager.single.PlaySfxClipChange(15);
        msgText.text = "";
        charIndex = 0;
        endCursor.SetActive(false);

        //Start Text OutPut Animation
        interval = (1.0f / charPerSeconds);
        Debug.Log(interval);

        isAnim = true;

        Invoke("EffectNow", interval);//출력되는 글자에 딜레이 줌
    }

    private void EffectNow()
    {
        AudioManager.single.PlaySfxClipChange(15);
        if (msgText.text == targetMsg)
        {
            EffectEnd();
            return;
        }
        msgText.text += targetMsg[charIndex];
        charIndex++;

        Invoke("EffectNow", interval);//출력되는 글자에 딜레이 줌
    }

    private void EffectEnd()
    {
        isAnim = false;
        endCursor.SetActive(true);

        AudioManager.single.PlaySfxClipChange(15);
    }
}
