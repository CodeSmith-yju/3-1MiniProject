using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeInEffect : MonoBehaviour
{
    public Image fadeImage; // 페이드 효과를 적용할 이미지
    public float fadeDuration = 1.5f; // 페이드 시간

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

        // 페이드 인이 완료되면 이미지의 알파 값을 완전히 0으로 설정
        color.a = endAlpha;
        fadeImage.color = color;
        fadeImage.gameObject.SetActive(false); // 페이드 이미지 비활성화
    }

    public IEnumerator FadeOut()
    {
        fadeImage.gameObject.SetActive(true); // 페이드 이미지 활성화
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

        // 페이드 아웃이 완료되면 이미지의 알파 값을 완전히 1으로 설정
        color.a = endAlpha;
        fadeImage.color = color;
    }
    public IEnumerator FadeOutAndLoadScene()
    {
        fadeImage.gameObject.SetActive(true); // 페이드 이미지 활성화
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

        // 페이드 아웃이 완료되면 이미지의 알파 값을 완전히 1으로 설정
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
