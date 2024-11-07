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
    public Sprite[] spElementIcon;
    public Sprite[] spJobIcon;
    public Sprite[] spPortraitIcon;
    [Header("Enemy, Attack, Skill Icons")]
    public Sprite[] spEnemyIcon;
    public Sprite[] spAttackIcon;
    public Sprite[] spSkillIcon;

    [Header("Dictionarys")]
    public Dictionary<BaseEntity.Attribute, Sprite> dictn_ElementIcon = new();
    public Dictionary<Ally.Class, Sprite> dictn_jobIcon = new();
    public Dictionary<Ally.Job, Sprite> dictn_portratiIcon = new();

    public Dictionary<IconEnemy, Sprite> dictn_enemyIcon = new();
    public Dictionary<Ally.Job, Sprite> dictn_attackIcon = new();
    public Dictionary<Ally.Job, Sprite> dictn_skillIcon = new();

    public void SetAllIcons()
    {
        SetElement();
        SetJob();
        SetPortrait();

        SetEnemyIcon();
        SetAttack();
        SetSkill();
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
        //dictn_jobIcon.Add(Ally.Class.Support, spJobIcon[3]);
    }
    void SetPortrait()
    {
        dictn_portratiIcon ??= new();
        dictn_portratiIcon.Add(Ally.Job.Hero, spPortraitIcon[0]);
        dictn_portratiIcon.Add(Ally.Job.Knight, spPortraitIcon[1]);
        dictn_portratiIcon.Add(Ally.Job.Ranger, spPortraitIcon[2]);
        dictn_portratiIcon.Add(Ally.Job.Wizard, spPortraitIcon[3]);
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
    }
    void SetSkill()
    {
        dictn_skillIcon ??= new();
        dictn_skillIcon.Add(Ally.Job.Hero, spSkillIcon[0]);
        dictn_skillIcon.Add(Ally.Job.Knight, spSkillIcon[1]);
        dictn_skillIcon.Add(Ally.Job.Ranger, spSkillIcon[2]);
        dictn_skillIcon.Add(Ally.Job.Wizard, spSkillIcon[3]);
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
        return dictn_portratiIcon[_job];
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
}
