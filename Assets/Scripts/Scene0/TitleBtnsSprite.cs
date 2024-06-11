using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TitleBtnsSprite : MonoBehaviour, IPointerEnterHandler, IPointerUpHandler, IPointerExitHandler
{
    public MainMenuMgr mainMenuMgr;

    [SerializeField] private Image img;
    [SerializeField] private Button my;
    private bool isPointerOver = false; // ���콺�� ��ư ���� �ִ��� ���θ� �����ϴ� ����
    public List<AudioClip> audioSources = new();
    private void Update()
    {
        // ���콺�� ��ư ���� ���� �ʰ�, ���콺�� ���� ���°� �ƴ� ���
        /*if (!isPointerOver && Input.GetMouseButton(0))
        {
            OnPointerExit(null); // OnPointerExit ȣ��
        }*/

        if (Input.GetMouseButton(0))
        {
            if (isPointerOver == true)
            {
                img.sprite = mainMenuMgr.TitleBtnSprites[2];// Ŭ��
            }
            else
            {
                img.sprite = mainMenuMgr.TitleBtnSprites[0];//�⺻
            }
        }
        else
        {
            Debug.Log("Return Update");
            return;
        }

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isPointerOver = true;
        img.sprite = mainMenuMgr.TitleBtnSprites[1];//ȣ��
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("Up");
        img.sprite = mainMenuMgr.TitleBtnSprites[0];
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("Exit");
        isPointerOver = false;
        img.sprite = mainMenuMgr.TitleBtnSprites[0];
    }
}
