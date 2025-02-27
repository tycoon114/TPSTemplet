using UnityEngine;
using System.Collections;

public class UIController : MonoBehaviour
{
    public CanvasGroup Copyrights;
    public CanvasGroup Logo;
    public CanvasGroup Back;
    public float fadeDuration = 2f;  // ���̵� ���� �ð�

    void Start()
    {
        //Copyrights.alpha = 1; // ���� �� �޽��� ǥ��
        StartCoroutine(FadeOutMessage());

    }

    IEnumerator FadeOutMessage()
    {
        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            Copyrights.alpha = Mathf.Lerp(1f, 0f, timer / fadeDuration);
            Logo.alpha = Mathf.Lerp(1f, 0f, timer / fadeDuration);
            Back.alpha = Mathf.Lerp(1f, 0f, timer / fadeDuration);
            yield return null;
        }
        Copyrights.alpha = 0f;
        Logo.alpha = 0f;
        Back.alpha = 0f;
        Copyrights.gameObject.SetActive(false); // �޽��� ��Ȱ��ȭ
    }
}
