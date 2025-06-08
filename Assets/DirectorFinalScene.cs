using System.Collections;
using UnityEngine;

public class DirectorFinalScene : MonoBehaviour
{
    [Header("Настройки звука")]
    public AudioSource audioSource;
    public AudioClip replic1;
    public AudioClip replic2;
    public AudioClip replic3;

    [Header("Объекты сцены")]
    public GameObject Monster;
    public GameObject PoliceCar;
    

    private bool hasStarted = false;

    void OnEnable()
    {
        if (!hasStarted)
        {
            Monster.SetActive(true);
            PoliceCar.SetActive(true);
            hasStarted = true;
            StartCoroutine(PlayInitialReplicas());
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
        // Отключение объекта перед репликой
        if (Monster != null)
        {
            yield return new WaitForSeconds(7.15f); // Подстрой паузу под нужный момент в анимации
            Monster.SetActive(false);
        }

        if (audioSource != null && replic3 != null)
        {
            audioSource.clip = replic3;
            audioSource.Play();
        }
    }

    public void DisableMonster()
    {
        if (Monster != null)
        {
            Monster.SetActive(false);
        }
    }

}
