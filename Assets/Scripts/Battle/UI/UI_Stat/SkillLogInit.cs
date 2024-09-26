using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillLogInit : MonoBehaviour
{
    [SerializeField] Image portrait;
    [SerializeField] TMP_Text skill_Name;


    public void Init(Sprite portrait, string skill_Name)
    {
        this.portrait.sprite = portrait;
        this.skill_Name.text = skill_Name;
    }

    private void Start()
    {
        StartCoroutine(SkillLogTimer());
    }

    IEnumerator SkillLogTimer()
    {
        yield return new WaitForSeconds(3f);

        Destroy(gameObject);
    }

}
