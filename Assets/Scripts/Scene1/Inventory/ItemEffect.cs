using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//아이템마다 사용효과가 다르기때문에 이걸 추상클래스로만들어서 쓰는듯
public abstract class ItemEffect : ScriptableObject
{
    public abstract bool ExcuteRole();
}
