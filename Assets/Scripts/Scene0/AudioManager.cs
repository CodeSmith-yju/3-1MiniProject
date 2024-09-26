using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager single;

    [Header("#BGM")]
    public AudioClip[] bgmClips;// 배경음으로 사용될 음성파일 에셋을 담아둘 변수
    [HideInInspector] public AudioSource bgmPlayer;// 배경음성을 출력하는데 필요한 변수, private로 쓰고싶은데 왜인지안되서 일단이걸로
    
    public float bgmVolume;//배경음 크기를 조절할 변수


    [Header("#SFX")]
    public AudioClip[] sfxClips;// 효과음으로 사용될 음성파일 에셋 배열

    //public AudioClip[] 
    private AudioSource[] sfxPlayers;

    public float sfxVolume;// 효과음 크기를 조절할 변수

    //동시에 여러 사운드들이 마구잡이로 섞이기때문에 채널시스템 구축이 필요함.
    public int channels;//다량의 효과음을 낼 수 있도록 채널 개수 변수 선언
    private int channelIndex;// 재생하고있는 채널의 인덱스값이 필요함

    [Header("#Playerble SFX")]//0 = Die, 1 = Atk
    public AudioClip[] hero_sfxClip;// hero
    public AudioClip[] ranger_sfxClip;// ranger
    public AudioClip[] wizard_sfxClip;// wizard
    public AudioClip[] knight_sfxClip;// knight


    [Header("#Enemy SFX")]
    public AudioClip[] slime_sfxClip;// slime
    public AudioClip[] gobline_sfxClip;// gobline
    public AudioClip[] mimic_sfxClip;// mimic
    public AudioClip[] skelletone_sfxClip;

    public enum Sfx
    {
         
    }

    public void EnemySound(int partyIndex, int _enemyIndex, int sfx_Index)
    {
        // sfx_Index : 0 Die, 1 Attack

        /*if (sfx_Index != 0)
        {
            PlaySfxVolumeChange(partyIndex, 0.2f);
        }
        else
        {
            PlaySfxVolumeChange(partyIndex, 0.5f);
        }*/

        switch (_enemyIndex)
        {
            case 0:
                sfxPlayers[partyIndex].clip = slime_sfxClip[sfx_Index];
                sfxPlayers[partyIndex].Play();
                break;
            case 1:
                sfxPlayers[partyIndex].clip = gobline_sfxClip[sfx_Index];
                sfxPlayers[partyIndex].Play();
                break;
            case 2:
                sfxPlayers[partyIndex].clip = mimic_sfxClip[sfx_Index];
                sfxPlayers[partyIndex].Play();
                break;
            case 3:
                sfxPlayers[partyIndex].clip = skelletone_sfxClip[sfx_Index];
                sfxPlayers[partyIndex].Play();
                break;
            default:
                break;
        }
    }

    public void PlayerSound(int partyIndex, int _enemyIndex, int sfx_Index)
    {
        // sfx_Index : 0 Die, 1 Attack

/*        if (sfx_Index != 0)
        {
            PlaySfxVolumeChange(partyIndex, 0.2f);
        }
        else
        {
            PlaySfxVolumeChange(partyIndex, 0.5f);
        }*/

        switch (_enemyIndex)
        {
            case 0:
                sfxPlayers[partyIndex].clip = hero_sfxClip[sfx_Index];
                sfxPlayers[partyIndex].Play();
                break;
            case 1:
                sfxPlayers[partyIndex].clip = ranger_sfxClip[sfx_Index];
                sfxPlayers[partyIndex].Play();
                break;
            case 2:
                sfxPlayers[partyIndex].clip = wizard_sfxClip[sfx_Index];
                sfxPlayers[partyIndex].Play();
                break;
            case 3:
                sfxPlayers[partyIndex].clip = knight_sfxClip[sfx_Index];
                sfxPlayers[partyIndex].Play();
                break;
            default:
                break;
        }
    }



    void Awake()
    {
        if (single == null)
        {
            single = this;
            DontDestroyOnLoad(gameObject);
            Init();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Init()
    {
        //배경음 플레이어 초기화
        GameObject bgmObject = new GameObject("BgmPlayer");//스크립트로 새로운 오브젝트 생성, 생성할때 이름도 지정가능.
        bgmObject.transform.parent = transform;//AudioManager스크립트가 부착된 오브젝트의 자식오브젝트로 bgmPlayer오브젝트를 생성
        bgmPlayer = bgmObject.AddComponent<AudioSource>();
        bgmPlayer.clip = bgmClips[0];//0은 Title, 1은 Town, 2는 Battle, 
        bgmPlayer.playOnAwake = false;//게임이 실행되자마자 실행하는가? Yes 
        bgmPlayer.loop = true;// 음성이 반복되는가? Yes
        bgmPlayer.volume = bgmVolume;
        bgmPlayer.Play();
        

        //효과음 플레이어 초기화
        GameObject sfxObject = new GameObject("SfxPlayer");
        sfxObject.transform.parent = transform;//AudioManager스크립트가 부착된 오브젝트의 자식오브젝트로 sfxPlayer오브젝트를 생성
        sfxPlayers = new AudioSource[channels];// 효과음 플레이어를 채널 개수만큼 초기화

        for(int index = 0; index < sfxPlayers.Length; index++)
        {
            //반복문을 통해 모든 효과음 오디오소스를 생성하면서 저장
            sfxPlayers[index] = sfxObject.AddComponent<AudioSource>();
            sfxPlayers[index].playOnAwake=false;
            sfxPlayers[index].volume = sfxVolume;
        }

    }

    public void PlayBgmClipChange(int _index)
    {
        bgmPlayer.clip = bgmClips[_index];
        bgmPlayer.Play();
    }

    public void PlayBgmVolumeChange(float _val)
    {
        bgmPlayer.volume = _val / 1.5f;

        if (bgmPlayer.volume == 0)
        {
            bgmPlayer.mute = true;
        }
        else
        {
            bgmPlayer.mute = false;
        }
    }
    
    public void PlaySfxClipChange(int _index)
    {
        sfxPlayers[0].clip = sfxClips[_index];
        sfxPlayers[0].Play();
    }

    public void PlaySfxVolumeChange(int _sfxIndex, float _val)
    {
        sfxPlayers[_sfxIndex].volume = _val / 1.5f;
        sfxPlayers[0].Play();
    }

    public void PlaySfx(Sfx sfx)
    {
        for(int i = 0; i< sfxPlayers.Length; i++){
            int loopIndex = ( i + channelIndex) % sfxPlayers.Length;

            if (sfxPlayers[loopIndex].isPlaying)
                continue;

            channelIndex = loopIndex;
            sfxPlayers[loopIndex].clip = sfxClips[(int)sfx];
            sfxPlayers[loopIndex].Play();
            break;
        }
    }

    public AudioSource GetBgmPlayer()
    {
        return bgmPlayer;
    }

    public AudioSource GetSfxPlayer(int _index)
    {
        return sfxPlayers[_index];
    }

    /*public void StopClips()
    {
        bgmPlayer.Stop();
        sfxPlayers[0].Stop();
    }*/
}
