using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData //����ȿ� ������ ���, ������ ��� �� �� ������ �����Ͱ� ����. ���⼱ red, blue ���� ĳ�����̸��̵� ��
{
    public readonly string NAME;// �����ڸ޼���ȿ��� �����Ҽ��ִµ�, ���Ŀ��� �ٲܼ� ���� ��.

    public PlayerData(string name)
    {
        this.NAME = name;
    }
}
