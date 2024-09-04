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
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingSceneController : MonoBehaviour
{
    [SerializeField] Image progressBar;
    public static string nextScene;

    public static void LoadScene(string _sceneName)
    {
        Debug.Log($"Run LoadScene: {_sceneName}");
        nextScene = _sceneName;
        SceneManager.LoadScene("LoadingScene"); // �ε� ������ �̵�
    }

    void Start()
    {
        Debug.Log($"LoadingScene Start. NextScene: {nextScene}");
        if (string.IsNullOrEmpty(nextScene))
        {
            Debug.LogError("NextScene is null or empty. Setting default to 'Town'");
            nextScene = "Town";
        }
        StartCoroutine(LoadSceneProcess());
    }

    IEnumerator LoadSceneProcess()
    {
        Debug.Log($"LoadSceneProcess started. Loading scene: {nextScene}");
        yield return null; // ù �������� ��ٸ�

        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
        op.allowSceneActivation = false;

        float timer = 0f;
        while (!op.isDone) // isDone�� true�� �� ������ �ݺ�
        {
            yield return null;
            if (op.progress < 0.9f)
            {
                progressBar.fillAmount = op.progress;
                Debug.Log($"Loading progress: {op.progress:P2}");
            }
            else
            {
                timer += Time.deltaTime;
                progressBar.fillAmount = Mathf.Lerp(0.9f, 1f, timer);
                Debug.Log($"Simulating final loading: {progressBar.fillAmount:P2}");
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