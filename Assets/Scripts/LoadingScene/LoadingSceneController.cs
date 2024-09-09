/*using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingSceneController : MonoBehaviour
{
    [SerializeField] Image progressBar;

    public static string nextScene;

    public static void LoadScene(string _sceneName)
    {
        Debug.Log("Run LoadScene");
        nextScene = _sceneName;
        SceneManager.LoadScene(nextScene);
    }
    // Start is called before the first frame update
    void Start()
    {
        if (nextScene != null)
        {
            nextScene = "Town";
        }
        StartCoroutine(LoadSceneProcess());
    }

    IEnumerator LoadSceneProcess()
    {
        Debug.Log("Run Load Progress");

        ///SceneManager.LoadScene 메서드는 동기방식으로 해당 스크립트가 부착된 씬을 모두 불러오기 전 까지는 다른 작업이 불가능
        ///LoadSceneAsync 메서드를 사용하게되면 비동기 방식으로 Scene을 불러오는 도중에 다른 작업이 가능
        ///씬을 불러오는 진행상황은 LoadSceneAsync함수가 AsyncOperation 타입으로 반환해줌.
        AsyncOperation op =  SceneManager.LoadSceneAsync(nextScene);

        /// 씬을 비동기방식으로 호출할때, 씬의 로딩이 끝나면 자동으로 해당 씬으로 이동할 것인지를 설정하는 변수
        /// false는 씬을 90%까지만 로드한 상태로 대기, 이렇게 하는 이유는
        /// 실무에서는 에셋번들을 통해 리소스들을 불러오는상황이 생기는데 이거 안해두고 그냥 넘어가면 이미지 깨질수도 있음
        op.allowSceneActivation = false;

        float timer = 0f;
        while (op.isDone)// AsyncOperation변수 op가 씬로딩을 진행중일때 == 씬 로딩이 90%까지 차지않은 상황이라면 반복
        {
            //이 반복문이 한번 실행될때마다 유니티 엔진에게 제어권을 넘겨줌, 이거 안하면 - 반복문끝나기전까지 화면갱신이 안 됨 == 진행바 게이지 변경이 화면에 반환되지 않음
            yield return null;
            if (op.progress < 0.9f)
            {
                progressBar.fillAmount = op.progress;
            }
            else
            {
                timer += Time.unscaledDeltaTime;
                progressBar.fillAmount = Mathf.Lerp(0.9f, 1f, timer);
                if (progressBar.fillAmount >= 1f)
                {
                    op.allowSceneActivation = true;
                    Debug.Log("Load Progress Complete!");
                    yield break;
                }
            }
        }

    }
}
*/

using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingSceneController : MonoBehaviour
{
    [SerializeField] Image progressBar;
    [SerializeField] TextMeshProUGUI loadingText;
    public static string nextScene;

    public static void LoadScene(string _sceneName)
    {
        Debug.Log($"Run LoadScene: {_sceneName}");
        nextScene = _sceneName;
        SceneManager.LoadScene("LoadingScene"); // 로딩 씬으로 이동
    }

    void Start()
    {
        StartCoroutine(LoadSceneProcess());
    }

    IEnumerator LoadSceneProcess()
    {
        Debug.Log($"LoadSceneProcess started. Loading scene: {nextScene}");
        yield return null; // 첫 프레임을 기다림

        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
        op.allowSceneActivation = false;

        float progress = 0f;
        float targetProgress = 0f;
        float progressSpeed = 1f; // 프로그레스 바가 매끄럽게 차오르는 속도
        int i = 0;
        while (!op.isDone)
        {
            yield return null;
            i++;
            // 비동기 로딩 진행 상황을 0~0.99 범위로 조정
            progress = op.progress * 1.1f; // 0~0.99 범위로 조정
            targetProgress = Mathf.Clamp01(progress); // 0~1 범위로 제한

            // 프로그레스 바의 fillAmount를 목표 값으로 매끄럽게 조정
            progressBar.fillAmount = Mathf.MoveTowards(progressBar.fillAmount, targetProgress, progressSpeed * Time.deltaTime);

            if (i%200 == 0)
            {
                TextUpdate(loadingText);
            }

            Debug.Log($"Loading progress: {progressBar.fillAmount:P2}");

            // 로딩이 완료되면 씬 전환 허용
            if (progressBar.fillAmount >= 0.99f)
            {
                loadingText.text = "Loading Now...";
                progressBar.fillAmount = 1f; // 최종적으로 100%로 설정
                op.allowSceneActivation = true;
                Debug.Log("Load Progress Complete!");
                yield break;
            }
        }
    }


    void TextUpdate(TextMeshProUGUI _tmp)
    {
        string s = "";
        if (_tmp.text == null)
        {
            s = "Loading Now";
        }
        else if (_tmp.text == "Loading Now...")
        {
            s = "Loading Now";
        }
        else if (_tmp.text == "Loading Now..")
        {
            s = "Loading Now...";
        }
        else if (_tmp.text == "Loading Now.")
        {
            s = "Loading Now..";
        }
        else if (_tmp.text == "Loading Now")
        {
            s = "Loading Now.";
        }
        loadingText.text = s;
    }

}