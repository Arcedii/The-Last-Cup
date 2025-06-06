using UnityEngine;
using UnityEngine.UI;
using static Unity.Burst.Intrinsics.X86.Avx;

public class PlayerTake : MonoBehaviour
{
    [Header("Settings")]
    public float rayDistance = 2.0f; 

    [Header("UI Elements")]
    public Text interactionText;

    public GameObject LidInHand;
    public GameObject Lid;
    public GameObject CupInHand;
    public GameObject Cup;

    public Animation CupAnim;
    public Animation LidAnim;

    public float animationDuration = 1.0f;

    void Update()
    {
       
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        
        interactionText.text = "";

        if (Physics.Raycast(ray, out hit, rayDistance))
        {
          
            if (hit.collider.CompareTag("Cups"))
            {
                interactionText.text = "�������  E, �����  �����  ���������";

                if (Input.GetKeyDown(KeyCode.E))
                {
                    StartCoroutine(PlayAnimationAndTakeCup());
                }
            }
            
            else if (hit.collider.CompareTag("Lids"))
            {
                interactionText.text = "�������  E,  �����  �����  ������";

                if (Input.GetKeyDown(KeyCode.E))
                {
                    StartCoroutine(PlayAnimationAndTakeLid());
                }
            }

            else if (hit.collider.CompareTag("CoffeMachine"))
            {
                interactionText.text = "������� E, ����� �������� ����";
                if (Input.GetKeyDown(KeyCode.E))
                {
                    
                }
            }
        }

        
        Debug.DrawRay(transform.position, transform.forward * rayDistance, Color.red);
    }

    public void TakeCup()
    {
        CupInHand.SetActive(true);
        Cup.SetActive(false);
    }

    public void TakeLid()
    {
        LidInHand.SetActive(true);
        Lid.SetActive(false);
    }

    System.Collections.IEnumerator PlayAnimationAndTakeCup()
    {
        Collider cupCollider = Cup.GetComponent<Collider>();
        Collider lidCollider = Lid.GetComponent<Collider>();

        lidCollider.enabled = false;
        cupCollider.enabled = false;


        // ��������� ��������
        CupAnim.Play();

        // ���� ���������� �������� (�� ������������)
        yield return new WaitForSeconds(animationDuration);

        // �������� ����� ����� ���������� ��������
        TakeCup();
    }

    System.Collections.IEnumerator PlayAnimationAndTakeLid()
    {
        Collider cupCollider = Cup.GetComponent<Collider>();
        Collider lidCollider = Lid.GetComponent<Collider>();

        lidCollider.enabled = false;
        cupCollider.enabled = false;

        // ��������� ��������
        LidAnim.Play();

        // ���� ���������� �������� (�� ������������)
        yield return new WaitForSeconds(animationDuration);

        // �������� ����� ����� ���������� ��������
        TakeLid();
    }
}
