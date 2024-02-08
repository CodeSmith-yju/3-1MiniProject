using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //�г�, ĳ���� ���� �� �갡 �����Ѵ�. ���ӸŴ����� �ϳ����־���Ѵ� = �÷��̾� = �̱��� �����ؼ� �޸𸮿� �÷��߰���? || ���� �Ŵ����� �ֻ����� �ٿ��� �Ѵ�.
    public static GameManager single { get; private set; }//get�� public �ϰ��Ҽ������� set�� ���⼭�� �� �� �ִ�.
    public static PlayerData playerData { get; private set; }//��ü���ӿ��� �ϳ��� �ִ°�

    [SerializeField] PanelSelectPlayer panel_SelectPlayer;
    [SerializeField] Inventory_Mgr inventoryMgr;
    //������ ó����

    private void Awake()
    {
        single = this; //�ʴ� �̰Ÿ� ��� ???

        playerData = null;// �÷��̾� ������ �ʱ�ȭ

        panel_SelectPlayer.gameObject.SetActive(true);//�����ϸ� ĳ���ͻ��� on, �κ��丮 off
        inventoryMgr.gameObject.SetActive(false);
    }

    public bool OnSelectPlayer(string name)
    {
        playerData = new PlayerData(name);  // �÷��̾���͸� �����Ҷ��� �̸����־��ֱ�� ��Ģ���������� name�־��ش�.
        //Debug.Log(name);
        bool succ = playerData != null;
        if (!succ) { return false; }

        //ĳ���� ���� ����
        panel_SelectPlayer.gameObject.SetActive(false);//ĳ���� ���� �� ���ӽ��� ��  ĳ���� ����â off �κ��丮 on
        inventoryMgr.gameObject.SetActive(true);

        return playerData != null;
    }
}
