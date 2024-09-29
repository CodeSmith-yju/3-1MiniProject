using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeInEffect : MonoBehaviour
{
    public Image fadeImage; // ���̵� ȿ���� ������ �̹���
    public float fadeDuration = 1.5f; // ���̵� �ð�

    private Coroutine currentCoroutine;

    void Start()
    {
        fadeImage.gameObject.SetActive(true);
        StartFadeIn();
    }

    public void StartFadeIn()
    {
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }
        currentCoroutine = StartCoroutine(FadeIn());
    }

    public void StartFadeOut()
    {
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }
        currentCoroutine = StartCoroutine(FadeOut());
    }

    public IEnumerator FadeIn()
    {
        Color color = fadeImage.color;
        float startAlpha = 1.0f;
        float endAlpha = 0.0f;
        float elapsedTime = 0.0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / fadeDuration);
            fadeImage.color = color;
            yield return null;
        }

        // ���̵� ���� �Ϸ�Ǹ� �̹����� ���� ���� ������ 0���� ����
        color.a = endAlpha;
        fadeImage.color = color;
        fadeImage.gameObject.SetActive(false); // ���̵� �̹��� ��Ȱ��ȭ
    }

    public IEnumerator FadeOut()
    {
        fadeImage.gameObject.SetActive(true); // ���̵� �̹��� Ȱ��ȭ
        Color color = fadeImage.color;
        float startAlpha = 0f;
        float endAlpha = 1f;
        float elapsedTime = 0.0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / fadeDuration);
            fadeImage.color = color;
            yield return null;
        }

        // ���̵� �ƿ��� �Ϸ�Ǹ� �̹����� ���� ���� ������ 1���� ����
        color.a = endAlpha;
        fadeImage.color = color;
    }
    public IEnumerator FadeOutAndLoadScene()
    {
        fadeImage.gameObject.SetActive(true); // ���̵� �̹��� Ȱ��ȭ
        Color color = fadeImage.color;
        float startAlpha = 0f;
        float endAlpha = 1f;
        float elapsedTime = 0.0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / fadeDuration);
            fadeImage.color = color;
            yield return null;
        }

        // ���̵� �ƿ��� �Ϸ�Ǹ� �̹����� ���� ���� ������ 1���� ����
        color.a = endAlpha;
        fadeImage.color = color;
        GameMgr.single.OnSelectPlayer(GameMgr.single.input_Name);
    }

    public void FadeON()
    {
        StartFadeIn();
    }

    public void FadeOFFAndLoadScene()
    {
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }
        currentCoroutine = StartCoroutine(FadeOutAndLoadScene());
    }
}
