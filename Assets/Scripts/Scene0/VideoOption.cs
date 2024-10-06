using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class VideoOption : MonoBehaviour
{
    private List<Resolution> resolutions = new List<Resolution>();// 컴퓨터의 모니터가 지원하는 해상도는 Scrren.resolutions 라는 배열에 들어있다, 따라서 Resolution타입의 List를 생성하여 모니터가 지원하는 화면 해상도 정보를 저장 할 수 있는 배열을 생성한다. 
    private FullScreenMode screenMode;

    public Dropdown resolutionDropDwon;// 이 드롭다운형식의 변수 resolutionDropDwon이 List resolutions의 값을 받아갈 것. Dropdown형식의 변수를 사용하려면 using UI 해야됨.
    private int resolutionNum;
    public Toggle btnFullScreen;
    private int dropdwonOptionValue;// 처음 시작했을 때 드롭다운의 선택된 값이 초기화되어있지 않으니 현재 해상도의 값과 해상도 목록을 비교해서 드롭다운의 벨류값을 변경해준다.

    [Header("#Sound Sliders")]
    public Slider bgmSlider;
    public Slider sfxSlider;
    /*public void InitUI()
    {
        DefaultCompareRate();

        for (int i = 0; i < Screen.resolutions.Length; i++)// 컴퓨터의 모니터가 지원하는 화면 해상도 정보를 전부 resolutions 배열에 저장한다. 
        {
            if (Screen.resolutions[i].refreshRateRatio.ToString() == "60" )
            {
                if (Screen.resolutions[i].width == 1280 && Screen.resolutions[i].height == 720)
                {
                    resolutions.Add(Screen.resolutions[i]);
                }else

                if (Screen.resolutions[i].width == 1366 && Screen.resolutions[i].height == 768)
                {
                    resolutions.Add(Screen.resolutions[i]);
                }else

                if (Screen.resolutions[i].width == 1920 && Screen.resolutions[i].height == 1080)
                {
                    resolutions.Add(Screen.resolutions[i]);
                }

            }
        }
        resolutionDropDwon.options.Clear();// 기존의 드롭다운 변수에 어떤 값이 들어있을수도있으니 초기화 해 준다.

        dropdwonOptionValue = 0;

        foreach (Resolution item in resolutions)    //Resolution 데이터타입의 변수 item에 배열변수 resolutions에 저장된 값을 대입하는 반복문
        {
            Dropdown.OptionData option = new Dropdown.OptionData();// Dropdown의 Option 목록 리스트는 OptionData형식으로 되어있기 때문에, OptionData형식의 객체 option을 생성하고
            option.text = item.width + " x " + item.height + " " + item.refreshRateRatio + "hz";// 객체 option의 text변수에 해상도 값을 넣어 준 뒤에
            resolutionDropDwon.options.Add(option);// 목록에 추가한다.

            if (item.width == Screen.width && item.height == Screen.height)
            {
                resolutionDropDwon.value = dropdwonOptionValue;// 실행되었을때의 해상도값에 해당하는 드롭다운의 벨류값으로 맞추어준다.
            }
            dropdwonOptionValue++;
        }
        resolutionDropDwon.RefreshShownValue();// 목록의 내용이 변경되었으므로 새로고침을 해 준다.

        btnFullScreen.isOn = Screen.fullScreenMode.Equals(FullScreenMode.FullScreenWindow) ? true : false ;
    }*/
    private void Start()
    {
        if (GameMgr.single.selectedResolution == -1)
        {
            Screen.SetResolution(1920, 1080, FullScreenMode.Windowed);// 게임 시작 시 1920*1080 창모드 실행
        }

        InitUI();
        SetSliderComponent();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5))
        {
            // 현재 화면 모드가 창 모드인지 전체화면인지 확인
            if (Screen.fullScreenMode == FullScreenMode.Windowed)
            {
                // 창 모드일 경우 전체화면으로 전환 (현재 해상도 유지)
                Screen.SetResolution(Screen.width, Screen.height, FullScreenMode.FullScreenWindow);
                Debug.Log("Switched to Fullscreen mode");
            }
            else
            {
                // 전체화면일 경우 창 모드로 전환 (현재 해상도 유지)
                Screen.SetResolution(Screen.width, Screen.height, FullScreenMode.Windowed);
                Debug.Log("Switched to Windowed mode");
            }

            // UI 업데이트
            InitUI();
        }
    }
    public void DropBoxOptionChange(int ChangeValue)// 이 메서드가 pulbic이여야만 OnValueChanged에서 Dynamic Int 항목을 선택 할 수 있음.
    {
        resolutionNum = ChangeValue;// DropBoxOptionChange 메서드가 매개변수로 받은 값을 resolutionNum이 Dynamic Int옵션으로 받아서 옵션을 적용하는 형식인듯. 
    }
    public void BtnFullScreen(bool isFull)// 토글 버튼
    {
        screenMode = isFull ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;// 참이면 전체화면, 거짓이면 창 모드
    }
    public void InitUI()
    {
        resolutionDropDwon.onValueChanged.AddListener(DropBoxOptionChange);
        Debug.Log("Run InitUI : VideoOption");

        // 해상도 설정
        resolutions ??= new List<Resolution>();// 리스트 초기화

        /*if (resolutions.Count == 0)//기존의 해상도추가코드, 정상 동작함
        {
            for (int i = 0; i < Screen.resolutions.Length; i++)// 컴퓨터의 모니터가 지원하는 화면 해상도 정보를 전부 resolutions 배열에 저장한다. 
            {
                if (Screen.resolutions[i].refreshRateRatio.ToString() == "60")
                {
                    if (Screen.resolutions[i].width == 1280 && Screen.resolutions[i].height == 720)
                    {
                        resolutions.Add(Screen.resolutions[i]);
                    }
                    else

                    if (Screen.resolutions[i].width == 1366 && Screen.resolutions[i].height == 768)
                    {
                        resolutions.Add(Screen.resolutions[i]);
                    }
                    else

                    if (Screen.resolutions[i].width == 1920 && Screen.resolutions[i].height == 1080)
                    {
                        resolutions.Add(Screen.resolutions[i]);
                    }

                }
            }
        }*/
        if (resolutions.Count == 0)// 해상도 중복 추가 방지
        {
            for (int i = 0; i < Screen.resolutions.Length; i++)
            {
                var res = Screen.resolutions[i];
                if (!resolutions.Exists(r => r.width == res.width && r.height == res.height))
                {
                    if (res.refreshRateRatio.numerator == 60 &&
                        (res.width == 1280 && res.height == 720 ||
                         res.width == 1366 && res.height == 768 ||
                         res.width == 1920 && res.height == 1080))
                    {
                        resolutions.Add(res);
                    }
                }
            }
        }


        // 디버그 로그로 확인: Screen.resolutions의 길이를 확인
        Debug.Log("******************Screen.resolutions.Length: " + Screen.resolutions.Length);

        // 드롭다운 옵션 설정
        resolutionDropDwon.ClearOptions(); // 드롭다운 초기화
        List<string> options = new List<string>();

        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Count; i++)
        {
            string option = $"{resolutions[i].width} x {resolutions[i].height}";
            options.Add(option);

            if (resolutions[i].width == Screen.width && resolutions[i].height == Screen.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropDwon.AddOptions(options);

        if (GameMgr.single.selectedResolution != -1 && GameMgr.single.selectedResolution < resolutions.Count)
        {
            resolutionDropDwon.value = GameMgr.single.selectedResolution;
        }
        else
        {
            resolutionDropDwon.value = currentResolutionIndex;
        }

        resolutionDropDwon.RefreshShownValue();

        resolutionNum = currentResolutionIndex;

        // 저장된 전체 화면 여부 설정
        btnFullScreen.isOn = Screen.fullScreen;
    }
    public void BtnOKClick()
    {
        // resolutionNum이 유효한 값인지 확인
        if (resolutionNum < 0 || resolutionNum >= resolutions.Count)
        {
            Debug.LogError("Selected resolution index is out of range.");
            return;  // resolutionNum이 범위를 벗어나면 실행을 중단
        }

        // 해상도 및 전체화면 설정 저장
        int width = resolutions[resolutionNum].width;
        int height = resolutions[resolutionNum].height;
        bool isFullScreen = screenMode == FullScreenMode.FullScreenWindow;

        // 설정을 화면에 반영
        Screen.SetResolution(width, height, screenMode);

        GameMgr.single.selectedResolution = resolutionNum;
        Debug.Log("*******************\nGameMgr.single.selectedResolution : "+GameMgr.single.selectedResolution+ "resolutionNum: "+ resolutionNum);
        // 볼륨 설정 저장
        ComitSliderVolume();
    }
    void ChangeBGMVolume(float value)
    {
        bgmSlider.value = value;
    }
    void ChangeSFXVolume(float value)
    {
        sfxSlider.value = value;
    }
    void SetSliderComponent()
    {
        bgmSlider.onValueChanged.AddListener(ChangeBGMVolume);
        sfxSlider.onValueChanged.AddListener(ChangeSFXVolume);

        bgmSlider.minValue = 0.0f;
        bgmSlider.maxValue = 1.5f;

        sfxSlider.minValue = 0.0f;
        sfxSlider.maxValue = 1.5f;

        bgmSlider.value = AudioManager.single.bgmPlayer.volume * 1.5f;
        sfxSlider.value = AudioManager.single.GetSfxPlayer(0).volume * 1.5f;
    }
    public void ComitSliderVolume()
    {
        AudioManager.single.PlayBgmVolumeChange(bgmSlider.value);
        AudioManager.single.PlaySfxVolumeChange(0, sfxSlider.value);

        SaveAudioSettings();
    }

    public void LoadAudioSettings()
    {
        // 저장된 볼륨 값 불러오기 (값이 없으면 최대치 1.5f로 설정)
        float savedBgmVolume = PlayerPrefs.GetFloat("BgmVolume", 1.5f);
        float savedSfxVolume = PlayerPrefs.GetFloat("SfxVolume", 1.5f);

        // 불러온 값을 슬라이더와 AudioManager에 적용
        bgmSlider.value = savedBgmVolume;
        AudioManager.single.PlayBgmVolumeChange(savedBgmVolume);

        sfxSlider.value = savedSfxVolume;
        AudioManager.single.PlaySfxVolumeChange(0, savedSfxVolume);

        Debug.Log($"Loaded BGM Volume: {savedBgmVolume}, SFX Volume: {savedSfxVolume}");
    }
    
    public void SaveAudioSettings()
    {
        PlayerPrefs.SetFloat("BgmVolume", bgmSlider.value);  // BGM 볼륨 저장
        PlayerPrefs.SetFloat("SfxVolume", sfxSlider.value);  // SFX 볼륨 저장
        PlayerPrefs.Save();  // PlayerPrefs에 저장
        Debug.Log("Audio settings saved.");
    }
    #region ResolutionSave/Load
    public void SaveResolutionSettings(int width, int height, bool isFullScreen)
    {
        PlayerPrefs.SetInt("ScreenWidth", width);
        PlayerPrefs.SetInt("ScreenHeight", height);
        PlayerPrefs.SetInt("FullScreen", isFullScreen ? 1 : 0);  // 전체화면: 1, 창모드: 0
        PlayerPrefs.Save();  // 저장
        Debug.Log($"Resolution saved: {width}x{height}, Fullscreen: {isFullScreen}");
    }
    public bool LoadResolutionSettings()
    {
        // 저장된 해상도와 전체화면 설정이 있는지 확인
        if (PlayerPrefs.HasKey("ScreenWidth") && PlayerPrefs.HasKey("ScreenHeight") && PlayerPrefs.HasKey("FullScreen"))
        {
            int savedWidth = PlayerPrefs.GetInt("ScreenWidth");
            int savedHeight = PlayerPrefs.GetInt("ScreenHeight");
            bool isFullScreen = PlayerPrefs.GetInt("FullScreen") == 1;

            // 저장된 해상도와 전체화면 설정 적용
            Screen.SetResolution(savedWidth, savedHeight, isFullScreen);
            Debug.Log($"Resolution loaded: {savedWidth}x{savedHeight}, Fullscreen: {isFullScreen}");

            return true;  // 저장된 데이터가 있음
        }

        return false;  // 저장된 데이터가 없음
    }
    #endregion

}
