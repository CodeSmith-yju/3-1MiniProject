using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

//��� Ŭ������ 1���ۿ��ȵ����� �߻�Ŭ������ ��� ���� �� �ִ�.
public class Item_Player : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private CanvasGroup _group;
    [SerializeField] private string playerName;
    [SerializeField] private TextMeshProUGUI textName;

    private void Awake()
    {
        textName.text = playerName;
        OnUnselect();
    }
    //ȭ���� ������ ���� Ŭ���� �� �� ���·� �ʱ�ȭ ���־���Ѵ�.
    public void OnUnselect() //�������� �׷���ó���Ϸ��� ����ٳ�����ǰ��� 
    {
        _group.alpha = 0.1f;
    }

    public string GetName() => playerName;

    public void Onselect() //�������� �׷���ó���Ϸ��� ����ٳ�����ǰ��� 
    {
        _group.alpha = 1f;
    }

    public void OnPointerClick(PointerEventData eventData) //�߻�Ŭ����
    {
        //Onselect(); //Ŭ��
        PanelSelectPlayer.single.ONFocusePlayerItem(this);
    }
}
