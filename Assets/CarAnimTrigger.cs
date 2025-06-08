using System.Collections;
using UnityEngine;

public class CarAnimTrigger : MonoBehaviour
{
    public DirectorFinalScene director;
    public Animator carAnimator;             // ������� � ����������
    public string animTriggerName = "Play";  // ��� �������� � Animator

    private bool hasActivated = false;

    private bool monsterDisabled = false;

    void Update()
    {
        AnimatorStateInfo state = carAnimator.GetCurrentAnimatorStateInfo(0);
        if (state.IsName("PoliceCarAnim"))
        {
            if (state.normalizedTime >= 0.477f && !monsterDisabled)
            {
                monsterDisabled = true;
                director.DisableMonster();  // �������� ����� � DirectorFinalScene
            }
        }
    }



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
