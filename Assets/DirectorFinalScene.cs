using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DirectorFinalScene : MonoBehaviour
{
    [Header("Настройки звука")]
    public AudioSource audioSource;
    public AudioClip replic1;
    public AudioClip replic2;
    public AudioClip replic3;

    public GameObject ExitBut;

    [Header("UI для затемнения")]
    public Image fadePanel;

    public float fadeDuration = 2f;   // Длительность затемнения

    [Header("Игрок")]
    public GameObject player;
    public GameObject FinalCutSceneCamera;// Ссылка на игрока

    private PlayerMovement playerMovement;    // Скрипт игрока

    [Header("Объекты сцены")]
    public GameObject Monster;
    public GameObject PoliceCar;

    public GameObject MonstrJump;  // Добавляем новый объект
    public AudioSource screamAudioSource;    // Отдельный AudioSource для крика
    public AudioClip screamClip;


    private bool hasStarted = false;

    void OnEnable()
    {
        if (!hasStarted)
        {
            Monster.SetActive(true);
            PoliceCar.SetActive(true);
            MonstrJump.SetActive(false);  // Чтобы изначально был выключен
            hasStarted = true;

            // Обеспечиваем, что fadePanel прозрачна в начале
            if (fadePanel != null)
                SetFadeAlpha(0f);

            if (player != null)
                playerMovement = player.GetComponent<PlayerMovement>();

            StartCoroutine(PlayInitialReplicas());

            ExitBut.SetActive(false);
            
        }
    }

    private IEnumerator PlayInitialReplicas()
    {
        if (audioSource == null) yield break;

        if (replic1 != null)
        {
            audioSource.clip = replic1;
            audioSource.Play();
            yield return new WaitForSeconds(replic1.length + 2f);
        }

        if (replic2 != null)
        {
            audioSource.clip = replic2;
            audioSource.Play();
            yield return new WaitForSeconds(replic2.length + 2f);
        }
    }

    // Этот метод вызывается после завершения анимации
    public void OnAnimationFinished()
    {
        StartCoroutine(PlayThirdReplica());
    }

    private IEnumerator PlayThirdReplica()
    {
        if (Monster != null)
        {
            yield return new WaitForSeconds(7.15f);
            Monster.SetActive(false);
        }

        if (audioSource != null && replic3 != null)
        {
            audioSource.clip = replic3;
            audioSource.Play();

            yield return new WaitForSeconds(replic3.length);
        }

        if (MonstrJump != null)
        {
            MonstrJump.SetActive(true);

            if (screamAudioSource != null && screamClip != null)
            {
                screamAudioSource.clip = screamClip;
                screamAudioSource.Play();
            }
        }

        // Отключаем скрипт PlayerMovement после 3-й фразы
        if (playerMovement != null)
        {
            playerMovement.enabled = false;
        }

        if (fadePanel != null)
        {
            // Сначала затемнение
            yield return StartCoroutine(FadeIn());

            ExitBut.SetActive(true);
            player.SetActive(true);
            Monster.SetActive(false);
            FinalCutSceneCamera.SetActive(true);
            MonstrJump.SetActive(false);

            // Затем пауза (по желанию, например 1 секунда)
            yield return new WaitForSeconds(1f);

            // Потом раззатемнение
            yield return StartCoroutine(FadeOut());
        }


    }

    private IEnumerator FadeIn()
    {
        float elapsed = 0f;
        Color c = fadePanel.color;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsed / fadeDuration);
            fadePanel.color = new Color(c.r, c.g, c.b, alpha);
            yield return null;
        }
        fadePanel.color = new Color(c.r, c.g, c.b, 1f);
    }

    private IEnumerator FadeOut()
    {
        float elapsed = 0f;
        Color c = fadePanel.color;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Clamp01(1f - (elapsed / fadeDuration));
            fadePanel.color = new Color(c.r, c.g, c.b, alpha);
            yield return null;
        }
        fadePanel.color = new Color(c.r, c.g, c.b, 0f);
    }


    private void SetFadeAlpha(float alpha)
    {
        if (fadePanel != null)
        {
            Color c = fadePanel.color;
            fadePanel.color = new Color(c.r, c.g, c.b, alpha);
        }
    }

    public void DisableMonster()
    {
        if (Monster != null)
        {
            Monster.SetActive(false);
        }
    }

    public void ExitFromGame()
    {
        Application.Quit();

    }

}