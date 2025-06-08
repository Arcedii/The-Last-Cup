using System.Collections;
using UnityEngine;

public class CarAnimTrigger : MonoBehaviour
{
    public DirectorFinalScene director;
    public Animator carAnimator;             // Назначь в инспекторе
    public string animTriggerName = "Play";  // Имя триггера в Animator

    private bool hasActivated = false;

    void OnTriggerEnter(Collider other)
    {
        if (!hasActivated && other.CompareTag("Player"))
        {
            hasActivated = true;
            carAnimator.SetTrigger(animTriggerName); // Запускаем анимацию

            StartCoroutine(WaitForAnimationEnd());
        }
    }

    private IEnumerator WaitForAnimationEnd()
    {
        // Подожди длительность анимации (поставь точное время)
        yield return new WaitForSeconds(7.15f); // заменишь на длительность своей анимации

        director.OnAnimationFinished(); // запускаем 3-ю реплику и отключение объекта
    }
}
