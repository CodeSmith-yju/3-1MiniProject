using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyNameSetting : MonoBehaviour
{
    // 기사, 궁수, 마법사의 이름 리스트
    public List<string> knightNames;
    public List<string> archerNames;
    public List<string> mageNames;
    public List<string> priestNames;
    public List<string> demonNames;
    

    /* //한국어이름도 일단 만들어뒀음
        knightNames = new List<string> { "알프레드", "에드먼드", "해럴드", "제프리", "월터", "가이", "레이먼드", "오스윈", "아이보", "휴" };
        archerNames = new List<string> {"에일린", "길다", "이소벨", "엘레노어", "콜린", "레이놀드", "아델라", "에디스", "오스카", "앨런"};
        mageNames = new List<string> { "마이클", "리처드", "에드워드", "가브리엘", "토머스", "알렉산더", "제이콥", "크리스토퍼", "필립", "도미닉", };
    */
    public string GetRandomName(List<string> nameList)// 이름 리스트에서 랜덤하게 하나 선택하는 함수
    {
        if (nameList.Count == 0)
        {
            Debug.LogWarning("이름을 더 이상 선택할 수 없습니다.");
            return "Unknown"; // 리스트에 이름이 없을 경우 기본값 반환
        }

        // 랜덤한 인덱스 선택
        int randomIndex = Random.Range(0, nameList.Count);

        // 랜덤으로 선택된 이름을 저장
        string selectedName = nameList[randomIndex];

        // 선택된 이름을 리스트에서 제거
        nameList.RemoveAt(randomIndex);
        //Debug.Log("반환되어 리스트에서 제거됨:" + selectedName);
        // 선택된 이름 반환
        return selectedName;
    }
    public void RefreshiNameList()
    {
        /// <summary>
        /// public변수로 선언하면 유니티 인스펙터창에보이는 SerialLize시스템때문에 자동으로 null이 아니라 비어있는 배열로 초기화되어서 그런다고함
        /// nameList ??= new List<string> { "A", "B", "C" }; 이런거 못쓴다는 뜻. 
        /// 비어있는 리스트로 초기화되었기때문에 null검사 하는 복합할당 검사문(" ??= ")안먹히고, Count가 0인배열이라 Count검사해야됨.
        /// </summary>
        if (knightNames.Count < 10)
        {
            knightNames.Clear();
            knightNames = new List<string> { "Alaric", "Gawain", "Roland", "Lancelot", "Arthas", "Tristan", "Bedivere", "Galahad", "Percival", "Kay" };
        }
        if (archerNames.Count < 10)
        {
            archerNames.Clear();
            archerNames = new List<string> { "Robin", "Elduin", "Faolan", "Nimue", "Ayla", "Vara", "Rylai", "Orin", "Shara", "Elandra" };
        }
        if (mageNames.Count < 10)
        {
            mageNames.Clear();
            mageNames = new List<string> { "Merlin", "Gandalf", "Morgana", "Zatanna", "Aegis", "Althas", "Luna", "Arcanis", "Solara", "Faye" };
        }
        if (priestNames.Count < 10)
        {
            priestNames.Clear();
            priestNames = new List<string> { "Aldric", "Benedict", "Elspeth", "Ishara", "Tobias", "Lucius", "Cyril", "Anselm", "Seraphine", "Eldora" };
        }
        if (demonNames.Count < 10)
        {
            demonNames.Clear();
            demonNames = new List<string> { "Garruk", "Thorne", "Drex", "Magnus", "Borin", "Huldan", "Grimnir", "Varg", "Ragnar", "Krogan" };
        }
    }
}
