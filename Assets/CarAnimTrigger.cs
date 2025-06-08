using System.Collections;
using UnityEngine;

public class CarAnimTrigger : MonoBehaviour
{
    public DirectorFinalScene director;
    public Animator carAnimator;             // ������� � ����������
    public string animTriggerName = "Play";  // ��� �������� � Animator

    private bool hasActivated = false;

    void OnTriggerEnter(Collider other)
    {
        if (!hasActivated && other.CompareTag("Player"))
        {
            hasActivated = true;
            carAnimator.SetTrigger(animTriggerName); // ��������� ��������

            StartCoroutine(WaitForAnimationEnd());
        }
    }

    private IEnumerator WaitForAnimationEnd()
    {
        // ������� ������������ �������� (������� ������ �����)
        yield return new WaitForSeconds(7.15f); // �������� �� ������������ ����� ��������

        director.OnAnimationFinished(); // ��������� 3-� ������� � ���������� �������
    }
}
