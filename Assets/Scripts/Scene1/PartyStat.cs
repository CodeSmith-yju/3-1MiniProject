using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyStat : MonoBehaviour
{
    public static PartyStat single;

    private EntityStat stat;
    private void Awake()
    {
        single = this;
    }
 /*   public void GenratePartyStat()
    {
        foreach (listPartyData)
        {
            switch (listPartyData.partyJobIndex)
            {
                case 1: Ranger();
                    isMelee = false; // �ӽ÷� ���� ���ְ� �Ȱ��� ������� ���� ���Ŀ� ����ü�� �߻��ϴ� ������� �ٲ� ����  
                    break;
            }
        }
        Debug.Log("���ò��� ����");
    }
    public void CreateParty(PartyData _partyData)
    {
        // ���� id, �ִ� HP, �ִ� MP, ���ݷ�, ���ݼӵ�, ���ݻ�Ÿ� ������ �ʱ�ȭ
        *//*stat = new EntityStat
            (1, 25, 5, 2, 1, 8, false);*//*

        GameUiMgr.single.objListPlayable[1].GetComponent<BaseEntity>().entity_id = _partyData.partyJobIndex;
        entity_id = _partyData.partyJobIndex;// ���� ǥ�õ� ��Ƽ�� ����Ʈ������� �����ص� �Ǵµ�?
        max_Hp = _partyData.partyHp;
        cur_Hp = max_Hp;
        max_Mp = _partyData.partyMp;
        cur_Mp = 0;
        atkDmg = _partyData.partyAtk;
        SetAttackSpeed(_partyData.partyAtkSpd);
        atkRange = _partyData.partyAtkRange;
        able_Skill = _partyData.partyAbleSkill;
        
    }
    public void qusehd(int _lv)
    {
        if (��Ƽ������.������Ʈ == State.Knight)
        {
            maxhp = maxhp + (maxhp * 0.1 * _lv);
        }
    }*/

}
