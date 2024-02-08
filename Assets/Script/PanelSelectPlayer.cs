using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PanelSelectPlayer : MonoBehaviour
{
    // �갡 �߾��������̶� item_P ���� ���� ������ item_P�� �����ϰ� ���������� ����� GMgr ���� �Ѱ��ش�.
    //�̱���
    public static PanelSelectPlayer single { get; private set; }
    //ĳ���� ������Ʈ
    [SerializeField] private List<Item_Player> listeItem;
    //�����ϱ� ��ư ������Ʈ
    [SerializeField] private Button btnStart;

    // �ȳ� �޼���
    [SerializeField] private TextMeshProUGUI textAlert;
    private void Awake()
    {
        single = this;

        ONFocusePlayerItem(null);

        /*
        //ĳ���� ������ ��ü ���� ��Ȱ��ȭ
        foreach (var item in listeItem)
        {
            item.OnUnselect();
        }
        //�����ϱ� ��ư ��Ȱ��ȭ
        btnStart.gameObject.SetActive(false); //�����ϸ� ����
        //�ȳ� �޼��� Ȱ��ȭ
        textAlert.gameObject.SetActive(true);//�����ϸ� ����
        */
    }

    private Item_Player focusePlayerItem = null;
    public void ONFocusePlayerItem(Item_Player clicked)
    {
        this.focusePlayerItem = clicked;
        foreach(var item in listeItem)
        {
            if (item == clicked)
                item.Onselect();
            else 
                item.OnUnselect();
        }

        // tip - btn On && text off
        btnStart.gameObject.SetActive(this.focusePlayerItem != null);
        textAlert.gameObject.SetActive(this.focusePlayerItem == null);
    }

    public void OnStartGame()// �̰� �����ϸ� ���ӸŴ������� �����ͺ����ߵ�
    {
        // ���� ����
        if (focusePlayerItem == null) return;
        //���� �Ŵ������� ������ ������ ��
        GameManager.single.OnSelectPlayer(focusePlayerItem.GetName());
    }
}
