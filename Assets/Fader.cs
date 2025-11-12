using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class Fader : MonoBehaviour
{
    public Image fadeImage; // Image full screen
    public float fadeSpeed = 1f;

    [Header("Optional Loading UI")]
    public Slider loadingBar; // nếu muốn hiện progress

    private void Start()
    {
        // Fade in khi scene bắt đầu
        StartCoroutine(FadeIn());
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(FadeOutAndLoad(sceneName));
    }

    IEnumerator FadeIn()
    {
        Color c = fadeImage.color;
        c.a = 1f;
        fadeImage.color = c;

        while (c.a > 0f)
        {
            c.a -= Time.deltaTime * fadeSpeed;
            fadeImage.color = c;
            yield return null;
        }
    }

    IEnumerator FadeOutAndLoad(string sceneName)
    {
        Color c = fadeImage.color;
        c.a = 0f;
        fadeImage.gameObject.SetActive(true);

        while (c.a < 1f)
        {
            c.a += Time.deltaTime * fadeSpeed;
            fadeImage.color = c;
            yield return null;
        }

        // Load scene async
        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
        op.allowSceneActivation = false;

        while (!op.isDone)
        {
            if (loadingBar != null)
            {
                loadingBar.value = Mathf.Clamp01(op.progress / 0.9f);
            }

            if (op.progress >= 0.9f)
            {
                op.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}
