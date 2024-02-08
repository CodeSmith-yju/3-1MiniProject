using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RescourceMgr : MonoBehaviour
{
    //�����ۿ� �ӽ÷� ���ҽ����־��µ� ���߿� ���������Ҷ� ���ҽ��������� �Ŵ���
    [SerializeField] private List<Sprite> listArmors;
    [SerializeField] private List<Sprite> listBoots;
    [SerializeField] private List<Sprite> listCaps;
    [SerializeField] private List<Sprite> listGloves;

    private static Dictionary<string, Sprite> dictArmors;//�����θ� �� ��
    private static Dictionary<string, Sprite> dictBoots;//�����θ� �� ��
    private static Dictionary<string, Sprite> dictCaps;
    private static Dictionary<string, Sprite> dictGloves;

    private void Awake()
    {
        dictArmors = new Dictionary<string, Sprite>();
        foreach(Sprite sp in listArmors)
        {
            dictArmors.Add(MakeName(sp.name), sp);
        }

        dictBoots = new Dictionary<string, Sprite>();
        foreach (Sprite sp in listBoots)
        {
            dictBoots.Add(MakeName(sp.name), sp);
        }

        dictCaps = new Dictionary<string, Sprite>();
        foreach (Sprite sp in listCaps)
        {
            dictCaps.Add(MakeName(sp.name), sp);
        }

        dictGloves = new Dictionary<string, Sprite>();
        foreach (Sprite sp in listGloves)
        {
            dictGloves.Add(MakeName(sp.name), sp);
        }

        /* foreach ���� �������ۿ��� �ϴ� for��
        for(int i =0; i< listArmors.Count; i++)
        {
            Sprite sp = listArmors[i];
            dictArmors.Add(sp.name, sp);
        }
        */
    }
    private static string MakeName(string str)
    {
        return str.ToLower().Replace(" ", "");
    }

    //static �޼���ȿ��� ���̴� ������ ���� static�̿����Ѵ�.
    public static Sprite GetArmor(string name)
    {
        name = MakeName(name);
        if (!dictArmors.TryGetValue(name, out Sprite sp)) 
        { 
            return null; 
        }
        return sp;
    }

    public static Sprite GetBoots(string name)
    {
        name = MakeName(name);
        if (!dictBoots.TryGetValue(name, out Sprite sp))
        {
            return null;
        }
        return sp;
    }

    public static Sprite GetCap(string name)
    {
        name = MakeName(name);
        if (!dictCaps.TryGetValue(name, out Sprite sp))
        {
            return null;
        }
        return sp;
    }

    public static Sprite GetGloves(string name)
    {
        name = MakeName(name);
        if (!dictGloves.TryGetValue(name, out Sprite sp))
        {
            return null;
        }
        return sp;
    }
}
