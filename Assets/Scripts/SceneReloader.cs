using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneReloader : MonoBehaviour
{
    public static SceneReloader instance;
    [SerializeField] private GameObject transitionPanelPrefab;
    private Image transitionPanel;
    const float TRANSITION_TIME = 0.5f;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private IEnumerator Start()
    {
        VolumeProfile profile = FindObjectOfType<Volume>().profile;
        profile.TryGet(out ColorAdjustments ca);
        
        ca.contrast.Override(-100);
        yield return new WaitForSecondsRealtime(0.2f);

        float timer = TRANSITION_TIME;
        while (timer > 0)
        {
            ca.contrast.Override(-100 * timer / TRANSITION_TIME);
            timer -= Time.unscaledDeltaTime;
            yield return new WaitForEndOfFrame();
        }
    }

    public void Reload()
    {
        StopCoroutine(ReloadCoroutine());
        StartCoroutine(ReloadCoroutine());
    }

    private IEnumerator ReloadCoroutine()
    {
        VolumeProfile profile = FindObjectOfType<Volume>().profile;
        profile.TryGet(out ColorAdjustments ca);

        float timer = 0;
        while (timer < TRANSITION_TIME)
        {
            ca.contrast.Override(-100 * timer / TRANSITION_TIME);
            timer += Time.unscaledDeltaTime;
            yield return new WaitForEndOfFrame();
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private IEnumerator ReloadWhitedCoroutine()
    {
        if (transitionPanel == null)
        {
            transitionPanel = Instantiate(transitionPanelPrefab).GetComponentInChildren<Image>();
            DontDestroyOnLoad(transitionPanel.transform.parent.gameObject);
        }

        float timer = 0f;
        float velocity = 0f;
        while (timer < TRANSITION_TIME)
        {
            var color = transitionPanel.color;
            color.a = Mathf.SmoothDamp(color.a, 1f, ref velocity, TRANSITION_TIME);
            transitionPanel.color = color;
            timer += Time.unscaledDeltaTime;
            yield return new WaitForEndOfFrame();
        }
        var targetColor = transitionPanel.color; targetColor.a = 1f; transitionPanel.color = targetColor;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
