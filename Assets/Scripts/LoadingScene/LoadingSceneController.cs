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

        ///SceneManager.LoadScene �޼���� ���������� �ش� ��ũ��Ʈ�� ������ ���� ��� �ҷ����� �� ������ �ٸ� �۾��� �Ұ���
        ///LoadSceneAsync �޼��带 ����ϰԵǸ� �񵿱� ������� Scene�� �ҷ����� ���߿� �ٸ� �۾��� ����
        ///���� �ҷ����� �����Ȳ�� LoadSceneAsync�Լ��� AsyncOperation Ÿ������ ��ȯ����.
        AsyncOperation op =  SceneManager.LoadSceneAsync(nextScene);

        /// ���� �񵿱������� ȣ���Ҷ�, ���� �ε��� ������ �ڵ����� �ش� ������ �̵��� �������� �����ϴ� ����
        /// false�� ���� 90%������ �ε��� ���·� ���, �̷��� �ϴ� ������
        /// �ǹ������� ���¹����� ���� ���ҽ����� �ҷ����»�Ȳ�� ����µ� �̰� ���صΰ� �׳� �Ѿ�� �̹��� �������� ����
        op.allowSceneActivation = false;

        float timer = 0f;
        while (op.isDone)// AsyncOperation���� op�� ���ε��� �������϶� == �� �ε��� 90%���� �������� ��Ȳ�̶�� �ݺ�
        {
            //�� �ݺ����� �ѹ� ����ɶ����� ����Ƽ �������� ������� �Ѱ���, �̰� ���ϸ� - �ݺ��������������� ȭ�鰻���� �� �� == ����� ������ ������ ȭ�鿡 ��ȯ���� ����
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
        SceneManager.LoadScene("LoadingScene"); // �ε� ������ �̵�
    }

    void Start()
    {
        StartCoroutine(LoadSceneProcess());
    }

    IEnumerator LoadSceneProcess()
    {
        Debug.Log($"LoadSceneProcess started. Loading scene: {nextScene}");
        yield return null; // ù �������� ��ٸ�

        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
        op.allowSceneActivation = false;

        float progress = 0f;
        float targetProgress = 0f;
        float progressSpeed = 1f; // ���α׷��� �ٰ� �Ų����� �������� �ӵ�
        int i = 0;
        while (!op.isDone)
        {
            yield return null;
            i++;
            // �񵿱� �ε� ���� ��Ȳ�� 0~0.99 ������ ����
            progress = op.progress * 1.1f; // 0~0.99 ������ ����
            targetProgress = Mathf.Clamp01(progress); // 0~1 ������ ����

            // ���α׷��� ���� fillAmount�� ��ǥ ������ �Ų����� ����
            progressBar.fillAmount = Mathf.MoveTowards(progressBar.fillAmount, targetProgress, progressSpeed * Time.deltaTime);

            if (i%200 == 0)
            {
                TextUpdate(loadingText);
            }

            Debug.Log($"Loading progress: {progressBar.fillAmount:P2}");

            // �ε��� �Ϸ�Ǹ� �� ��ȯ ���
            if (progressBar.fillAmount >= 0.99f)
            {
                loadingText.text = "Loading Now...";
                progressBar.fillAmount = 1f; // ���������� 100%�� ����
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