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
