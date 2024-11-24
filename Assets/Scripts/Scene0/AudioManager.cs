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
    #region SfxClips
    [Header("#Playerble SFX")]//0 = Die, 1 = Atk, 2 = Skill
    public AudioClip[] hero_sfxClip;// hero
    public AudioClip[] ranger_sfxClip;// ranger
    public AudioClip[] wizard_sfxClip;// wizard
    public AudioClip[] knight_sfxClip;// knight
    public AudioClip[] priest_sfxClip;// Priest 
    public AudioClip[] demon_sfxClip;// Demon


    [Header("#Enemy SFX")]
    public AudioClip[] slime_sfxClip;// slime
    public AudioClip[] gobline_sfxClip;// gobline
    public AudioClip[] mimic_sfxClip;// mimic
    public AudioClip[] skelletone_sfxClip;//skelleton
    public AudioClip[] skeletonWizard_sfxClip;// Boss
    public AudioClip[] puppetHuman_sfxClip;// hero
    public AudioClip[] Golem_sfxClip;// Gollem
    public AudioClip[] Luminarch_sfxClip; // Lumina

    #endregion
    public enum Sfx
    {
         
    }

    public void EnemySound(int partyIndex, int _enemyIndex, int sfx_Index)
    {
        // sfx_Index : 0 Die, 1 Attack, 2 Skill

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
            //Slime 0
            case 0:
                sfxPlayers[partyIndex].clip = slime_sfxClip[sfx_Index];
                sfxPlayers[partyIndex].Play();
                break;
            //gobline 1
            case 1:
                if (sfx_Index == 0)
                {
                    // 현재 볼륨 저장
                    float originalVolume = sfxPlayers[partyIndex].volume;

                    // 볼륨을 12%로 줄임
                    sfxPlayers[partyIndex].volume = originalVolume * 0.5f;

                    // 클립 설정 및 재생
                    sfxPlayers[partyIndex].clip = gobline_sfxClip[sfx_Index];
                    sfxPlayers[partyIndex].Play();

                    // 볼륨 복구 작업을 위한 코루틴 실행
                    StartCoroutine(RestoreVolumeAfterPlayback(partyIndex, originalVolume));
                }
                else
                {
                    sfxPlayers[partyIndex].clip = gobline_sfxClip[sfx_Index];
                    sfxPlayers[partyIndex].Play();
                }
                break;
            //mimic 2
            case 2:
                sfxPlayers[partyIndex].clip = mimic_sfxClip[sfx_Index];
                sfxPlayers[partyIndex].Play();
                break;
            //skellotone-worrior 3
            case 3:
                if (sfx_Index == 0)
                {
                    // 현재 볼륨 저장
                    float originalVolume = sfxPlayers[partyIndex].volume;

                    // 볼륨을 25%로 줄임
                    sfxPlayers[partyIndex].volume = originalVolume * 0.5f;

                    // 클립 설정 및 재생
                    sfxPlayers[partyIndex].clip = skelletone_sfxClip[sfx_Index];
                    sfxPlayers[partyIndex].Play();

                    // 볼륨 복구 작업을 위한 코루틴 실행
                    StartCoroutine(RestoreVolumeAfterPlayback(partyIndex, originalVolume));
                }
                else
                {
                    sfxPlayers[partyIndex].clip = skelletone_sfxClip[sfx_Index];
                    sfxPlayers[partyIndex].Play();
                }
                break;
            //해골법사 4
            case 4:
                sfxPlayers[partyIndex].clip = skeletonWizard_sfxClip[sfx_Index];
                sfxPlayers[partyIndex].Play();
                break;
            //퍼핏 5
            case 5:
                sfxPlayers[partyIndex].clip = puppetHuman_sfxClip[sfx_Index];
                sfxPlayers[partyIndex].Play();
                break;
            //골렘 6
            case 6:
                sfxPlayers[partyIndex].clip = Golem_sfxClip[sfx_Index];
                sfxPlayers[partyIndex].Play();
                break;
            // 루미나 7
            case 7:
                sfxPlayers[partyIndex].clip = Luminarch_sfxClip[sfx_Index];
                sfxPlayers[partyIndex].Play();
                break;
            default:
                break;
        }
    }

    public void PlayerSound(int partyIndex, int _enemyIndex, int sfx_Index)
    {
        // sfx_Index : 0 Die, 1 Attack, 2 Skill

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
            case 4:
                sfxPlayers[partyIndex].clip = priest_sfxClip[sfx_Index];
                sfxPlayers[partyIndex].Play();
                break;
            case 5:
                sfxPlayers[partyIndex].clip = demon_sfxClip[sfx_Index];
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
            SetSfxVolumeForOtherChannels();
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
        for(int i= 0; i < sfxPlayers.Length; i++)
        {
            sfxPlayers[i].Stop();
        }

        bgmPlayer.clip = bgmClips[_index];
        bgmPlayer.Play();
    }

    public void PlayBgmVolumeChange(float _val)
    {
        Debug.Log(" Input val :" +_val);
        bgmPlayer.volume = _val / 1.5f;
        Debug.Log(" output val :" + bgmPlayer.volume);
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
        if (_index == 17)
        {
            if (sfxPlayers[0].clip.name == sfxClips[_index].name && sfxPlayers[0].isPlaying)
            {
                return;
            }
            StartCoroutine(ForceStopSfxAfterDelay(sfxPlayers[0], 0.275f));
        }
        sfxPlayers[0].clip = sfxClips[_index];
        sfxPlayers[0].Play();
    }
    private IEnumerator ForceStopSfxAfterDelay(AudioSource audioSource, float delay)
    {
        yield return new WaitForSeconds(delay);

        // 소리가 재생되고 있을 때 강제로 정지
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

    public void PlaySfxVolumeChange(int _sfxIndex, float _val)
    {
        sfxPlayers[_sfxIndex].volume = _val / 1.5f;
        sfxPlayers[0].Play();
        SetSfxVolumeForOtherChannels();
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

    public void SetSfxVolumeForOtherChannels()
    {
        Debug.Log("설정 전_sfxPlayers[0]의 볼륨: " + sfxPlayers[0].volume);
        float baseVolume = sfxPlayers[0].volume;  // sfxPlayers[0]의 볼륨을 기준으로
        float newVolume = baseVolume * 0.6f;  // 60%로 설정

        // sfxPlayers[1]부터 나머지 채널들의 볼륨을 30%로 설정
        for (int i = 1; i < sfxPlayers.Length; i++)
        {
            sfxPlayers[i].volume = newVolume;
        }

        Debug.Log($"설정 후_sfxPlayers[0]의 볼륨: {baseVolume}, 나머지 채널의 볼륨: {newVolume}");
    }

    // 볼륨 복구를 위한 코루틴
    private IEnumerator RestoreVolumeAfterPlayback(int partyIndex, float originalVolume)
    {
        // 재생 중일 때까지 대기
        yield return new WaitUntil(() => !sfxPlayers[partyIndex].isPlaying);

        // 볼륨을 원래대로 복구
        sfxPlayers[partyIndex].volume = originalVolume;
    }
}
