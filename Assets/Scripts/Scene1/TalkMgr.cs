using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TalkMgr : MonoBehaviour // 대화 데이터를 관리할 매니저 스크립트
{
    private Dictionary<int, string[]> dictTalkData;//오브젝트 id를 받으면 문장(대사)이 들어있는배열(대화)을 반환하는 변수
    private Dictionary<int, Sprite> dictPortraitSprite;// 초상화 데이터를 저장할 딕셔너리 <초상화에 해당하는 npc id, 해당초상화 이미지 << 초상화 스프라이트를 저장하는 배열의 요소가 들어갈 것>
    public Sprite[] aryPortraitSprite;// 초상화 스프라이트를 저장할 배열

    public Dictionary<Sprite, string> dictTalkName = new();
    private void Awake()
    {
        dictTalkData = new Dictionary<int, string[]>(); // 초기화
        dictPortraitSprite = new Dictionary<int, Sprite>();
        //GenerateTalkData();// 만들어진 오브젝트들의 상호작용 대사를 호출
    }

    private void GenerateTalkData()// 오브젝트들의 상호작용 대사를 만들어서 스크립트 실행할때 호출되게 함
    {
        //Talk Data - NPC A: 1000, NPC B: 2000, BOX: 100, 
        //dictTalkData.Add(1000, new string[] { "안녕! :0", "이 곳에 처음 왔구나?:1", "개쩌는 김치피자탕수육을 만들어 주렴:2" });// 하나의 대화에는 여러 문장이 있으므로 배열로 선언
        dictTalkData.Add(1000, new string[] { GameMgr.playerData[0].GetPlayerName()+"님!:0", "오셨군요 :0" });// 하나의 대화에는 여러 문장이 있으므로 배열로 선언
        dictTalkData.Add(2000, new string[] { GameMgr.playerData[0].GetPlayerName() + "님!:0", "오셨군요 :0" });
        //dictTalkData.Add(1000, new string[] { "안녕!" + GameMgr.playerData.NAME + ":0", "이 곳에 처음 왔구나?:1", "개쩌는 김치피자탕수육을 만들어주렴:2" });// 하나의 대화에는 여러 문장이 있으므로 배열로 선언
        //dictTalkData.Add(2000, new string[] { GameMgr.playerData.NAME+"! :0","던전마을로 가는거야? :1", "몸조심해! :2" });
        dictTalkData.Add(5000, new string[] { "상자" });
        dictTalkData.Add(6000, new string[] { "표지판" });
        dictTalkData.Add(7000, new string[] { "문"});
        dictTalkData.Add(8000, new string[] { "던전"});

        //dictTalkData.Add(01 + 1000, new string[] { "안녕! :0", "이 곳에 처음 왔구나?:1", "나는 루나라고해!:2" });
        dictTalkData.Add(10 + 1000, new string[] {
            "견습 모험가님 안녕하세요. 9급 모험가 시험에 응시하러 오셨군요. :0",
            "저희가 준비한 모의전투에 승리하면 \n 모험가 길드에 가입할 수 있습니다. :0",
            "모의 전투에 필요한 기본 장비를 지급 해드릴테니 \n 다시 대화를 걸어주세요 :0",
        });
        dictTalkData.Add(11 + 2000, new string[] { 
            "여기 기본 장비 4종을 지급해드렸으니 착용하고 다시 와주세요. :0", 
            "(장비를 착용하고 다시 오자.) :1" // 여기까지는 정상구현 완료.
        });


        // QestRange-20, NPC-1000
        dictTalkData.Add(20 + 1000, new string[] { "인벤토리는 키보드의 I키 혹은 하단의 가방 아이콘을 통해 열 수 있습니다. :0", "장비는 클릭을 통하여 착용 할 수 있습니다. :0" });
        // QestRange-20, NPC-2000
        dictTalkData.Add(21 + 2000, new string[] { "장비를 전부 착용하셨군요! :0", 
            "다음은 파티원을 모집하는 방법을 알려드릴테니 다시 대화를 걸어주세요. :0" 
        });

        // QestRange-30, NPC-1000
        dictTalkData.Add(30 + 1000, new string[] {
            "모험가님 오른쪽의 게시판이나 키보드의 P 키를 통해 파티원을 모집할 수 있습니다.:0",
            "모의 전투에서 파티원 모집에 사용되는 금액은 전투 종료 후 복구되니 걱정하지 마세요.:0",
            " (게시판으로 이동해서 파티원을 모집하고 돌아오자.) :1"
        });
        dictTalkData.Add(31 + 1000, new string[] {
            "파티원을 모집하는 방법은 우측의 게시판을 이용하거나 키보드의 P 키를 누르시면 됩니다. :0",
            "모의 전투에서 파티원 모집에 사용되는 금액은 전투 종료 후 복구되니 걱정하지 마세요.:0"
        });
        // QestRange-30, NPC-2000
        dictTalkData.Add(31 + 2000, new string[] {
            "파티원을 모아오셨군요!:0",
            "다음은 모의 전투에 대해서 알려드릴 테니 다시 대화를 걸어 주세요.:0"
        });

        // QestRange-40, NPC-1000
        dictTalkData.Add(40 + 1000, new string[] {
            "모험가님 우측에 있는 포탈로 입장하면 모의 전투를 진행할 수 있습니다. :0",
            "우측의 게시판이나 키보드의 P 키를 통해 파티원을 모집하고 전투를 진행하세요.:0",
            " (포탈로 이동해서 모의 전투를 하고 돌아오자.) :1"
        });
        dictTalkData.Add(41 + 1000, new string[] {
            "(포탈로 이동해서 모의 전투를 하고 돌아오자.) :1"
        });
        // QestRange-40, NPC-2000
        dictTalkData.Add(40 + 2000, new string[] {
            "던전을 클리어하고 오셨군요! \n이제부터 "+GameMgr.playerData[0].GetPlayerName()+" 모험가님은 정식으로 9급 모험가가 되셨습니다.:0",
            "앞으로도 "+GameMgr.playerData[0].GetPlayerName()+" 9급 모험가님의 \n활약을 기대하겠습니다.:0"
        });


        dictTalkData.Add(50 + 1000, new string[] {
            "앞으로도"+ GameMgr.playerData[0].GetPlayerName()+"님의 멋진 활약 기대하겠습니다. :0"
        });
        dictTalkData.Add(51 + 1000, new string[] {
            "앞으로도"+ GameMgr.playerData[0].GetPlayerName()+"님의 멋진 활약 기대하겠습니다. :0"
        });

        dictTalkData.Add(50 + 2000, new string[] {
            "앞으로도"+ GameMgr.playerData[0].GetPlayerName()+"님의 멋진 활약 기대하겠습니다. :0"
        });


        /*dictTalkData.Add(40 + 1000, new string[] {
            " 체력 회복을 위한 물약을 지급해 드렸으니 사용하고 다시 와주세요 :0",
            " (I키로 인벤토리를 열고 물약을 사용하자.) :0"
        });*/
        /*dictTalkData.Add(41 + 1000, new string[] {
            " (인벤토리를 열고 물약을 먹은 뒤 이야기하자.) :0"
        });
        // QestRange-40, NPC-2000
        dictTalkData.Add(41 + 2000, new string[] {
            "[Player]님 저희 모험가 길드에 가입한 것을 축하드립니다. :1",
            "앞으로도 [Player]님의 멋진 활약 기대하겠습니다. :2"
        });*/

        //dictTalkData.Add(20 + 2000, new string[] { "찾으면 꼭 가져다줘 :1"});
        //dictTalkData.Add(20 + 9000, new string[] { "책을 발견했다." });

        //Portrait Data, 0:Idel, 1: Talk, 2: Happy, 3: Angry
        // (내가 지정한 Npc의 ID + NPC상태에 따른 변수), 스프라이트배열 aryPortraitSprite에 저장된 스프라이트 이미지 << 이건 추후에 배열번호가아니라 배열에저장된 스프라이트 이름으로 주는식으로 변경할수있을듯 
        dictPortraitSprite.Add(1000 + 0, aryPortraitSprite[0]);// Idel
        dictPortraitSprite.Add(1000 + 1, aryPortraitSprite[8]);

        dictPortraitSprite.Add(2000 + 0, aryPortraitSprite[0]);// Idel
        dictPortraitSprite.Add(2000 + 1, aryPortraitSprite[8]);

        dictPortraitSprite.Add(2000 + 2, aryPortraitSprite[2]);
        dictPortraitSprite.Add(2000 + 3, aryPortraitSprite[3]);
    }
    private void Start()
    {
        GenerateTalkData();// 만들어진 오브젝트들의 상호작용 대사를 호출

        dictTalkName.Clear();

        dictTalkName.Add(aryPortraitSprite[0], "접수원");
        dictTalkName.Add(aryPortraitSprite[8], GameMgr.playerData[0].GetPlayerName());//GameMgr.playerData[0].GetPlayerName()
    }

    public string GetTalk(int objectID, int talkDataIndex)// 지정된 대화 문장을 반환하는 함수
    {
        //Dictionary에 Key가 있는지 검사하는 함수
        if (!dictTalkData.ContainsKey(objectID))
        {
            if (!dictTalkData.ContainsKey(objectID - objectID%10))
                return GetTalk(objectID - objectID % 100, talkDataIndex);// Get First Talk
            else
                return GetTalk(objectID - objectID % 10, talkDataIndex);// Get First Quest Talk
        }

        if (talkDataIndex == dictTalkData[objectID].Length)
            return null;
        else
            return dictTalkData[objectID][talkDataIndex];

    }
    public Sprite GetPortrait(int npcId, int portraitIndex)// 딕셔너리에 저장된 스프라이트를 지정하여 반환시키는 메서드
    {
        return dictPortraitSprite[npcId + portraitIndex];
    }
}
