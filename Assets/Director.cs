using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class Director : MonoBehaviour
{
    public Volume globalVolume;
    private Vignette vignette;

    public GameObject Player;
    public GameObject CutSceneCamera;

    public GameObject CoffeCup;
    
    void Start()
    {
        CutSceneCamera.SetActive(true);
        Player.SetActive(false);
        BoxCollider boxCollider = CoffeCup.GetComponent<BoxCollider>();
        boxCollider.enabled = false;

        if (globalVolume.profile.TryGet<Vignette>(out vignette))
        {
            // Запускаем корутину для анимации
            StartCoroutine(AnimateVignette());
        }
    }

    void Update()
    {
        
    }

    public void SceneOne()
    {

    }

    private System.Collections.IEnumerator AnimateVignette()
    {
        float duration = 4.0f; // Длительность анимации в секундах
        float elapsedTime = 0f;

        // Начальное значение Intensity
        vignette.intensity.value = 1.0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            // Плавное изменение от 1 до 0.3 с использованием Lerp
            vignette.intensity.value = Mathf.Lerp(1.0f, 0.3f, t);
            yield return null;
        }

        // Убеждаемся, что значение точно установлено в 0.3
        vignette.intensity.value = 0.3f;
        CutSceneCamera.SetActive(false);
        Player.SetActive(true);
    }
}
