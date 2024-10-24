using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyIconRS : MonoBehaviour
{
    public Sprite[] spElementIcon;
    public Sprite[] spJobIcon;
    public Dictionary<BaseEntity.Attribute, Sprite> dictn_ElementIcon = new();
    public Dictionary<Ally.JobClass, Sprite> dictn_jobIcon = new();

    public void SetElement()
    {
        dictn_ElementIcon ??= new();
        dictn_ElementIcon.Add(BaseEntity.Attribute.Normal, spElementIcon[0]);
        dictn_ElementIcon.Add(BaseEntity.Attribute.Fire, spElementIcon[1]);
        dictn_ElementIcon.Add(BaseEntity.Attribute.Water, spElementIcon[2]);
        dictn_ElementIcon.Add(BaseEntity.Attribute.Wind, spElementIcon[3]);
        dictn_ElementIcon.Add(BaseEntity.Attribute.Light, spElementIcon[4]);
        dictn_ElementIcon.Add(BaseEntity.Attribute.Dark, spElementIcon[5]);
    }
    public void SetJob()
    {
        dictn_jobIcon ??= new();
        dictn_jobIcon.Add(Ally.JobClass.Hero, spJobIcon[0]);
        dictn_jobIcon.Add(Ally.JobClass.Knight, spJobIcon[1]);
        dictn_jobIcon.Add(Ally.JobClass.Ranger, spJobIcon[2]);
        dictn_jobIcon.Add(Ally.JobClass.Wizard, spJobIcon[3]);
    }
}
