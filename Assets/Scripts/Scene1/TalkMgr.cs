﻿using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TalkMgr : MonoBehaviour // 대화 데이터를 관리할 매니저 스크립트
{
    private Dictionary<int, string[]> dictTalkData;//오브젝트 id를 받으면 문장(대사)이 들어있는배열(대화)을 반환하는 변수
    private Dictionary<int, Sprite> dictPortraitSprite;// 초상화 데이터를 저장할 딕셔너리 <초상화에 해당하는 npc id, 해당초상화 이미지 << 초상화 스프라이트를 저장하는 배열의 요소가 들어갈 것>
    public Sprite[] aryPortraitSprite;// 초상화 스프라이트를 저장할 배열

    public Dictionary<Sprite, string> dictTalkName = new();

    public GuideUi guideUi;
    private void Awake()
    {
        dictTalkData = new Dictionary<int, string[]>(); // 초기화
        dictPortraitSprite = new Dictionary<int, Sprite>();
        guideUi.Refresh_GuideImg();
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
        dictTalkData.Add(4000, new string[] { "물건 사러 왔어? 아니면 개조? :1"});
        dictTalkData.Add(5000, new string[] { "상자" });
        dictTalkData.Add(6000, new string[] { "표지판" });
        dictTalkData.Add(7000, new string[] { "문"});
        dictTalkData.Add(8000, new string[] { "던전"});

        //dictTalkData.Add(01 + 1000, new string[] { "안녕! :0", "이 곳에 처음 왔구나?:1", "나는 루나라고해!:2" })
        /*
        "견습 모험가님 안녕하세요. \n9급 모험가 시험에 응시하러 오셨군요. :0",
        "저희가 준비한 모의전투에 승리하시면 \n정식으로 모험가 길드에 가입할 수 있습니다. :0",
        "모의 전투에 필요한 기본 장비를 지급 해드릴테니 \n다시 대화를 걸어주세요 :0",
         */

        dictTalkData.Add(10 + 1000, new string[] {
            "견습 모험가님 안녕하세요. \n9급 모험가 시험에 응시하러 오셨군요. :0",
            "저희가 준비한 모의전투에 승리하시면 \n정식으로 모험가 길드에 가입하실 수 있습니다. :0",
            "모의전투에 앞서, 모험가님께서 반드시 숙지하셔야 하는 \n중요한 안내 사항을 설명드리겠습니다. :0",

           "스태미나는 상점에서 판매하는 포션으로만 회복 가능합니다. \n일정 수치 이하로 떨어지게 된다면 \n모험가증에 경고가 표시되니 유의해 주세요. :0",
            "스태미나가 부족한 상태로 던전에 입장하면 과로로 인해 \n최대 소지금의 10%를 잃고, 다음 날까지 회복이 지연됩니다. :0",

            "스태미나 포션은 대장장이에게서 구매 가능합니다. \n단, 9급 모험가 자격이 없으면 구매할 수 없으니 유의해 주세요. :0",
            "또한, 아이템을 판매하실 때는 \n원가의 70%만 지급되오니 참고해 주시기 바랍니다. :0",
            "장비에는 일반 장비와 특수 장비가 있으며, \n일반 장비는 상점에 주기적으로 입고 됩니다.:0",

            "개조 장비는 던전 재료와 일반 장비를 대장장이에게 가져가시면 \n일정의 수고비를 받고 개조해 드립니다. \n 개조하실 장비는 장비 해제 후 요청해 주시기 바랍니다.:0",
            "일반 장비는 강화할 수 없으나, \n던전에서 얻은 재료로 개조된 장비는 강화가 가능합니다. :0",

            "강화는 실패하지 않지만, 단계가 높아질수록 \n소모되는 재화가 많아지므로 참고해 주세요. \n강화할 장비는 반드시 장비 해제 후 요청해 주시기 바랍니다. :0",
            "모의 전투에 필요한 기본 장비를 준비해드릴테니, \n다시 대화를 걸어주세요. :0"
            /*"스태미나는 상점에서 판매하는 스태미나 포션으로만 회복 가능합니다. \n스태미나가 일정 수치 이하로 떨어지면 모험가증에 경고가 표시됩니다. :0",
            "스태미나가 부족한 상태에서 던전에 입장하면 과로로 인해 쓰러질 수 있습니다. \n최대 소지금의 10%를 잃고, 다음 날이 되어야 회복됩니다. :0", 

            "스태미나 포션을 구매하시려면 특정 위치에 있는 NPC를 찾아가셔야 하지만 \n9급 모험가 자격을 갖추지 못하면 구매가 불가능합니다. :0", 
            "또한, 아이템을 판매할 때는 원가의 70%만 받을 수 있으니 주의하세요. :0", 
            
            "장비에는 일반 장비와 특수 장비가 있습니다. \n일반 장비는 상점에서 주기적으로 구매할 수 있습니다. :0", 
            "개조 장비는 던전에서 얻은 재료와 일반 장비를 대장장이 NPC에게 가져가면 \n일정 수고비를 받고 개조해 드립니다. 장비를 개조할 때는 장비를 해제해두세요. :0",
             
            "일반 장비는 강화할 수 없지만, \n던전에서 얻은 재료로 개조한 장비는 강화가 가능합니다. :0", 
            "강화는 실패하지 않지만, 단계가 높아질수록 소모하는 재화가 많아집니다. \n강화할 장비는 반드시 장비 해제 후 강화 요청을 하세요. :0",
            
            "모의 전투에 필요한 기본 장비를 지급해드릴테니 \n다시 대화를 걸어주세요. :0",*/
        });
        dictTalkData.Add(11 + 2000, new string[] { 
            "여기 기본 장비 4종을 지급해드렸으니 착용하고 다시 와주세요. :0",
            "(장비를 착용하고 다시 오자.) :1" // 여기까지는 정상구현 완료.
        });


        // QestRange-20, NPC-1000
        dictTalkData.Add(20 + 1000, new string[] { "인벤토리는 키보드의 I키 혹은 하단의 가방 아이콘을 통해 열 수 있습니다. :0", "장비는 클릭을 통하여 착용 할 수 있습니다. :0" });
        // QestRange-20, NPC-2000
        dictTalkData.Add(21 + 2000, new string[] { "장비를 전부 착용하셨군요! :0", 
            "다음은 파티원을 모집하는 방법을 알려드릴테니 \n다시 대화를 걸어주세요. :0" 
        });

        // QestRange-30, NPC-1000
        dictTalkData.Add(30 + 1000, new string[] {
            "모험가님 오른쪽의 게시판이나 키보드의 P 키를 통해 \n파티원을 모집할 수 있으며, 파티원은 최대 3명 입니다.:0",
            " (게시판으로 이동해서 파티원을 모집하고 돌아오자.) :1"
        });
        dictTalkData.Add(31 + 1000, new string[] {
            "파티원을 모집하는 방법은 우측의 게시판을 이용하거나 \n키보드의 P 키를 누르시면 됩니다. :0"
        });
        // QestRange-30, NPC-2000
        dictTalkData.Add(31 + 2000, new string[] {
            "파티원을 모아오셨군요!:0",
            "이번 전투에 한해서 \n던전에서 사용할 수 있는 포션을 지급해 드렸어요. :0",
            "다음은 모의 전투에 대해서 알려드릴 테니 \n다시 대화를 걸어 주세요.:0"
        });

        // QestRange-40, NPC-1000
        dictTalkData.Add(40 + 1000, new string[] {
            "모험가님 우측에 있는 포탈로 입장하면 \n모의 전투를 진행할 수 있습니다. :0",
            "포션을 잘 활용하여 모의 전투에서 승리하세요. :0",
            " (포탈로 이동해서 모의 전투를 하고 돌아오자.) :1"
        });
        dictTalkData.Add(41 + 1000, new string[] {
            "포션을 잘 활용하여 모의 전투에서 승리하세요. :0",
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
        // aryPortraitSprite[0] = 접수원, [1] = player, [2] = blacksmith
        dictPortraitSprite.Add(1000 + 0, aryPortraitSprite[0]);// Idel
        dictPortraitSprite.Add(1000 + 1, aryPortraitSprite[1]);

        dictPortraitSprite.Add(2000 + 0, aryPortraitSprite[0]);// Idel
        dictPortraitSprite.Add(2000 + 1, aryPortraitSprite[1]);

        /*dictPortraitSprite.Add(2000 + 2, aryPortraitSprite[0]);
        dictPortraitSprite.Add(2000 + 3, aryPortraitSprite[0]);*/

        for (int i = 0; i < 4; i++)
        {
            dictPortraitSprite.Add(4000 + i, aryPortraitSprite[2]);
        }
        
    }
    private void Start()
    {
        GenerateTalkData();// 만들어진 오브젝트들의 상호작용 대사를 호출

        dictTalkName.Clear();

        dictTalkName.Add(aryPortraitSprite[0], "접수원");
        dictTalkName.Add(aryPortraitSprite[2], "대장장이");
        dictTalkName.Add(aryPortraitSprite[1], GameMgr.playerData[0].GetPlayerName());//GameMgr.playerData[0].GetPlayerName()
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
