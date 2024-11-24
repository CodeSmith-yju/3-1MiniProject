using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager single;

    [Header("#BGM")]
    public AudioClip[] bgmClips;// ��������� ���� �������� ������ ��Ƶ� ����
    [HideInInspector] public AudioSource bgmPlayer;// ��������� ����ϴµ� �ʿ��� ����, private�� ��������� �������ȵǼ� �ϴ��̰ɷ�
    
    public float bgmVolume;//����� ũ�⸦ ������ ����


    [Header("#SFX")]
    public AudioClip[] sfxClips;// ȿ�������� ���� �������� ���� �迭

    //public AudioClip[] 
    private AudioSource[] sfxPlayers;

    public float sfxVolume;// ȿ���� ũ�⸦ ������ ����

    //���ÿ� ���� ������� �������̷� ���̱⶧���� ä�νý��� ������ �ʿ���.
    public int channels;//�ٷ��� ȿ������ �� �� �ֵ��� ä�� ���� ���� ����
    private int channelIndex;// ����ϰ��ִ� ä���� �ε������� �ʿ���
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
                    // ���� ���� ����
                    float originalVolume = sfxPlayers[partyIndex].volume;

                    // ������ 12%�� ����
                    sfxPlayers[partyIndex].volume = originalVolume * 0.5f;

                    // Ŭ�� ���� �� ���
                    sfxPlayers[partyIndex].clip = gobline_sfxClip[sfx_Index];
                    sfxPlayers[partyIndex].Play();

                    // ���� ���� �۾��� ���� �ڷ�ƾ ����
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
                    // ���� ���� ����
                    float originalVolume = sfxPlayers[partyIndex].volume;

                    // ������ 25%�� ����
                    sfxPlayers[partyIndex].volume = originalVolume * 0.5f;

                    // Ŭ�� ���� �� ���
                    sfxPlayers[partyIndex].clip = skelletone_sfxClip[sfx_Index];
                    sfxPlayers[partyIndex].Play();

                    // ���� ���� �۾��� ���� �ڷ�ƾ ����
                    StartCoroutine(RestoreVolumeAfterPlayback(partyIndex, originalVolume));
                }
                else
                {
                    sfxPlayers[partyIndex].clip = skelletone_sfxClip[sfx_Index];
                    sfxPlayers[partyIndex].Play();
                }
                break;
            //�ذ���� 4
            case 4:
                sfxPlayers[partyIndex].clip = skeletonWizard_sfxClip[sfx_Index];
                sfxPlayers[partyIndex].Play();
                break;
            //���� 5
            case 5:
                sfxPlayers[partyIndex].clip = puppetHuman_sfxClip[sfx_Index];
                sfxPlayers[partyIndex].Play();
                break;
            //�� 6
            case 6:
                sfxPlayers[partyIndex].clip = Golem_sfxClip[sfx_Index];
                sfxPlayers[partyIndex].Play();
                break;
            // ��̳� 7
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
        //����� �÷��̾� �ʱ�ȭ
        GameObject bgmObject = new GameObject("BgmPlayer");//��ũ��Ʈ�� ���ο� ������Ʈ ����, �����Ҷ� �̸��� ��������.
        bgmObject.transform.parent = transform;//AudioManager��ũ��Ʈ�� ������ ������Ʈ�� �ڽĿ�����Ʈ�� bgmPlayer������Ʈ�� ����
        bgmPlayer = bgmObject.AddComponent<AudioSource>();
        bgmPlayer.clip = bgmClips[0];//0�� Title, 1�� Town, 2�� Battle, 
        bgmPlayer.playOnAwake = false;//������ ������ڸ��� �����ϴ°�? Yes 
        bgmPlayer.loop = true;// ������ �ݺ��Ǵ°�? Yes
        bgmPlayer.volume = bgmVolume;
        bgmPlayer.Play();
        

        //ȿ���� �÷��̾� �ʱ�ȭ
        GameObject sfxObject = new GameObject("SfxPlayer");
        sfxObject.transform.parent = transform;//AudioManager��ũ��Ʈ�� ������ ������Ʈ�� �ڽĿ�����Ʈ�� sfxPlayer������Ʈ�� ����
        sfxPlayers = new AudioSource[channels];// ȿ���� �÷��̾ ä�� ������ŭ �ʱ�ȭ

        for(int index = 0; index < sfxPlayers.Length; index++)
        {
            //�ݺ����� ���� ��� ȿ���� ������ҽ��� �����ϸ鼭 ����
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

        // �Ҹ��� ����ǰ� ���� �� ������ ����
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
        Debug.Log("���� ��_sfxPlayers[0]�� ����: " + sfxPlayers[0].volume);
        float baseVolume = sfxPlayers[0].volume;  // sfxPlayers[0]�� ������ ��������
        float newVolume = baseVolume * 0.6f;  // 60%�� ����

        // sfxPlayers[1]���� ������ ä�ε��� ������ 30%�� ����
        for (int i = 1; i < sfxPlayers.Length; i++)
        {
            sfxPlayers[i].volume = newVolume;
        }

        Debug.Log($"���� ��_sfxPlayers[0]�� ����: {baseVolume}, ������ ä���� ����: {newVolume}");
    }

    // ���� ������ ���� �ڷ�ƾ
    private IEnumerator RestoreVolumeAfterPlayback(int partyIndex, float originalVolume)
    {
        // ��� ���� ������ ���
        yield return new WaitUntil(() => !sfxPlayers[partyIndex].isPlaying);

        // ������ ������� ����
        sfxPlayers[partyIndex].volume = originalVolume;
    }
}
