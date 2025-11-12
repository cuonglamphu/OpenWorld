using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance { get; private set; }

    [Header("Loading UI Elements")]
    public Image fader;
    public Slider loadingBar;
    public Text loadingText;
    public float fadeDuration = 1f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
       fader.enabled = false;
    }

 
    public void LoadScene(int sceneID)
    {
        StartCoroutine(LoadSceneAsync(sceneID));
    }

    private IEnumerator LoadSceneAsync(int sceneId)
    {
        fader.enabled = true;
        loadingBar.gameObject.SetActive(false);

        // Fade in (màn hình dần tối)
        float t = 0f;
        Color c = fader.color;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            c.a = Mathf.Lerp(0f, 1f, t / fadeDuration);
            fader.color = c;
            yield return null;
        }
        c.a = 1f;
        fader.color = c;

        // 3. Hiện loading bar
        loadingBar.gameObject.SetActive(true);
        loadingBar.value = 0f;


        // Load scene bất đồng bộ
        AsyncOperation op = SceneManager.LoadSceneAsync(sceneId);
        op.allowSceneActivation = false;

        // Cập nhật thanh loading
        while (op.progress < 0.9f)
        {

            loadingBar.value = op.progress;
            yield return null;
        }

        loadingBar.value = 1f;
      

        // Scene xong, kích hoạt
        op.allowSceneActivation = true;

        // ẩn Fader
        fader.enabled = false;
        loadingBar.gameObject.SetActive(false);
    }
}
