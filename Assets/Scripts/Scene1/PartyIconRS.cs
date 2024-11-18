using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum IconEnemy
{
    Goblin,
    Slime,
    Purple_Slime,
    Golem,
    Skeleton,
    Puppet_Human,
    FireGolem,
    IceGolem
}
public class PartyIconRS : MonoBehaviour
{

    [Header("Elemnetal, JobClass, Poartrait Icons")]
    [SerializeField] Sprite[] spElementIcon;
    [SerializeField] Sprite[] spJobIcon;
    [SerializeField] Sprite[] spPortraitIcon;
    [SerializeField] Sprite[] spDoublePortraitIcon;
    public Sprite[] spStandingIcon;
    [Header("Enemy, Attack, Skill Icons")]
    [SerializeField] Sprite[] spEnemyIcon;
    [SerializeField] Sprite[] spAttackIcon;
    [SerializeField] Sprite[] spSkillIcon;
    [SerializeField] Sprite[] spDoubleSkillIcon;
    public Sprite[] spStatIcon;//Hp, Mp, atk, atkspd, atkrng, def, spd

    [Header("Dictionarys")]
    [SerializeField] Dictionary<BaseEntity.Attribute, Sprite> dictn_ElementIcon = new();
    [SerializeField] Dictionary<Ally.Class, Sprite> dictn_jobIcon = new();
    [SerializeField] Dictionary<Ally.Job, Sprite> dictn_portratIcon = new();
    [SerializeField] Dictionary<Ally.Job, Sprite> dictn_doubleportratIcon = new();

    [SerializeField] Dictionary<IconEnemy, Sprite> dictn_enemyIcon = new();
    [SerializeField] Dictionary<Ally.Job, Sprite> dictn_attackIcon = new();
    [SerializeField] Dictionary<Ally.Job, Sprite> dictn_skillIcon = new();
    [SerializeField] Dictionary<Ally.Job, Sprite> dictn_double_skillIcon = new();

    public void SetAllIcons()
    {
        SetElement();
        SetJob();
        SetPortrait();
        SetDoublePortrait();

        SetEnemyIcon();
        SetAttack();
        SetSkill();
        SetDoubleSkill();
    }

    void SetElement()
    {
        dictn_ElementIcon ??= new();
        dictn_ElementIcon.Add(BaseEntity.Attribute.Normal, spElementIcon[0]);
        dictn_ElementIcon.Add(BaseEntity.Attribute.Fire, spElementIcon[1]);
        dictn_ElementIcon.Add(BaseEntity.Attribute.Water, spElementIcon[2]);
        dictn_ElementIcon.Add(BaseEntity.Attribute.Wind, spElementIcon[3]);
        dictn_ElementIcon.Add(BaseEntity.Attribute.Light, spElementIcon[4]);
        dictn_ElementIcon.Add(BaseEntity.Attribute.Dark, spElementIcon[5]);
    }
    void SetJob()
    {
        dictn_jobIcon ??= new();
        dictn_jobIcon.Add(Ally.Class.Melee, spJobIcon[0]);
        dictn_jobIcon.Add(Ally.Class.Tank, spJobIcon[1]);
        dictn_jobIcon.Add(Ally.Class.Range, spJobIcon[2]);
        dictn_jobIcon.Add(Ally.Class.Support, spJobIcon[3]);
    }
    void SetPortrait()
    {
        dictn_portratIcon ??= new();
        dictn_portratIcon.Add(Ally.Job.Hero, spPortraitIcon[0]);
        dictn_portratIcon.Add(Ally.Job.Knight, spPortraitIcon[1]);
        dictn_portratIcon.Add(Ally.Job.Ranger, spPortraitIcon[2]);
        dictn_portratIcon.Add(Ally.Job.Wizard, spPortraitIcon[3]);
        dictn_portratIcon.Add(Ally.Job.Priest, spPortraitIcon[4]);
        dictn_portratIcon.Add(Ally.Job.Demon, spPortraitIcon[5]);
    }
    void SetDoublePortrait()
    {
        dictn_doubleportratIcon ??= new();
        //dictn_doubleportratIcon.Add(Ally.Job.Hero, spDoublePortraitIcon[0]);
        //dictn_doubleportratIcon.Add(Ally.Job.Knight, spDoublePortraitIcon[1]);
        //dictn_doubleportratIcon.Add(Ally.Job.Ranger, spDoublePortraitIcon[2]);
        //dictn_doubleportratIcon.Add(Ally.Job.Wizard, spDoublePortraitIcon[3]);
        //dictn_doubleportratIcon.Add(Ally.Job.Priest, spDoublePortraitIcon[4]);
        dictn_doubleportratIcon.Add(Ally.Job.Demon, spDoublePortraitIcon[0]);
    }
    void SetEnemyIcon()
    {
        dictn_enemyIcon ??= new();
        dictn_enemyIcon.Add(IconEnemy.Goblin, spEnemyIcon[0]);
        dictn_enemyIcon.Add(IconEnemy.Slime, spEnemyIcon[1]);
        dictn_enemyIcon.Add(IconEnemy.Purple_Slime, spEnemyIcon[2]);

        dictn_enemyIcon.Add(IconEnemy.Golem, spEnemyIcon[3]);
        dictn_enemyIcon.Add(IconEnemy.Skeleton, spEnemyIcon[4]);
        dictn_enemyIcon.Add(IconEnemy.Puppet_Human, spEnemyIcon[5]);
        dictn_enemyIcon.Add(IconEnemy.FireGolem, spEnemyIcon[6]);
        dictn_enemyIcon.Add(IconEnemy.IceGolem, spEnemyIcon[7]);
    }
    void SetAttack()
    {
        dictn_attackIcon ??= new();
        dictn_attackIcon.Add(Ally.Job.Hero, spAttackIcon[0]);
        dictn_attackIcon.Add(Ally.Job.Knight, spAttackIcon[1]);
        dictn_attackIcon.Add(Ally.Job.Ranger, spAttackIcon[2]);
        dictn_attackIcon.Add(Ally.Job.Wizard, spAttackIcon[3]);
        //dictn_attackIcon.Add(Ally.Job.Priest, spAttackIcon[4]);
    }
    void SetSkill()
    {
        dictn_skillIcon ??= new();
        dictn_skillIcon.Add(Ally.Job.Hero, spSkillIcon[0]);
        dictn_skillIcon.Add(Ally.Job.Knight, spSkillIcon[1]);
        dictn_skillIcon.Add(Ally.Job.Ranger, spSkillIcon[2]);
        dictn_skillIcon.Add(Ally.Job.Wizard, spSkillIcon[3]);
        dictn_skillIcon.Add(Ally.Job.Priest, spSkillIcon[4]);
        dictn_skillIcon.Add(Ally.Job.Demon, spSkillIcon[5]);
        Debug.Log("스킬아이콘추가해!!!!!!!!!!!!");
    }
    void SetDoubleSkill()
    {
        dictn_double_skillIcon ??= new();
        dictn_double_skillIcon.Add(Ally.Job.Demon, spDoubleSkillIcon[0]);
    }

    public Sprite GetElementIcon(BaseEntity.Attribute _elemnental)
    {
        return dictn_ElementIcon[_elemnental];
    }
    public Sprite GetJobIcon(Ally.Class _classType)
    {
        return dictn_jobIcon[_classType];
    }
    public Sprite GetPortraitIcon(Ally.Job _job)
    {
        return dictn_portratIcon[_job];
    }
    public Sprite GetDoublePortraitIcon(Ally.Job _job)
    {
        return dictn_doubleportratIcon[_job];
    }

    public Sprite GetEnemyIcon(IconEnemy _enemyIcon)
    {
        return dictn_enemyIcon[_enemyIcon];
    }
    public Sprite GetAttackIcon(Ally.Job _job)
    {
        return dictn_attackIcon[_job];
    }
    public Sprite GetSkillIcon(Ally.Job _job)
    {
        return dictn_skillIcon[_job];
    }
    public Sprite GetDoubleSkill(Ally.Job _job)
    {
        return dictn_double_skillIcon[_job];
    }

    public Sprite GetStandingIcon(int _index)//0 = Knight, 1 = Mage
    {
        return spStandingIcon[_index];
    }
}
