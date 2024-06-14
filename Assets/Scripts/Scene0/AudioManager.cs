using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager single;

    [Header("#BGM")]
    public AudioClip[] bgmClips;// ��������� ���� �������� ������ ��Ƶ� ����
    private AudioSource bgmPlayer;// ��������� ����ϴµ� �ʿ��� ����
    
    public float bgmVolume;//����� ũ�⸦ ������ ����


    [Header("#SFX")]
    public AudioClip[] sfxClips;// ȿ�������� ���� �������� ���� �迭
    private AudioSource[] sfxPlayers;

    public float sfxVolume;// ȿ���� ũ�⸦ ������ ����

    //���ÿ� ���� ������� �������̷� ���̱⶧���� ä�νý��� ������ �ʿ���.
    public int channels;//�ٷ��� ȿ������ �� �� �ֵ��� ä�� ���� ���� ����
    private int channelIndex;// ����ϰ��ִ� ä���� �ε������� �ʿ���


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
        bgmPlayer.playOnAwake = true;//������ ������ڸ��� �����ϴ°�? Yes 
        bgmPlayer.loop = true;// ������ �ݺ��Ǵ°�? Yes
        bgmPlayer.volume = bgmVolume;
        bgmPlayer.clip = bgmClips[0];//0�� Title, 1�� Town, 2�� Battle, 

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

    public void PlayBgmChange(int _index)
    {
        bgmPlayer.clip = bgmClips[_index];
        bgmPlayer.Play();
    }

    public void PlayBgmVolumeChange(float _val)
    {
        bgmPlayer.volume = _val;
    }
    
    public void PlaySfxChange(int _index)
    {
        sfxPlayers[0].clip = sfxClips[_index];
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
}
