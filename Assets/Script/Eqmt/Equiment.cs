using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Equiment   // Data Only
{
    //public abstract string Sp; or
    protected string strIconName; //��ӹ��� �ڽ�Ŭ������ ������ ���ٰ����ϰ� �ϱ�����. 
    public Equiment(string iconName)
    {
        this.strIconName = iconName;
    }
    public abstract Sprite GetIcon();//�߻�ȭ Ŭ����
}
