using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using static Unity.Burst.Intrinsics.X86.Avx;

public class PlayerTake : MonoBehaviour
{
    [Header("Settings")]
    public float rayDistance = 2.0f;
    public float PreparingCoffeTime = 2.0f;
    public float elapsedTime = 0f;
    public float startFill = 0f; // ��������� �������
    public float endFill = 1f; // �������� ������� (����� ������ ������)

    [Header("UI Elements")]
    public Text interactionText;

    public GameObject LidInHand;
    public GameObject Lid;
    public GameObject CupInHand;
    public GameObject Cup;
    public GameObject Coffee;

    public GameObject PreparedCoffe;
    public GameObject PreparedCoffeInHand;

    public Animation CupAnim;
    public Animation LidAnim;

    public ParticleSystem coffeePour;
    public ParticleSystem MoneyBoom;
    public ChildActivityChecker activityChecker;

    bool CoffeIsprepared = false;
    private bool moneyEffectPlayed = false;



    public float animationDuration = 1.0f;

    void Start()
    { 
        Coffee.transform.localScale = Vector3.zero;
    }

    void Update()
    {

        if (CoffeeCupThrow.CupIsActive == false)
        {
            Debug.Log("������ ���������");
            StartCoroutine(PlayMoneyEffectOnce());

        }

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
                    StartCoroutine(BrewCoffee());
                }
            }

            else if (CoffeIsprepared == true & hit.collider.CompareTag("Client"))
            {

                interactionText.text = "������� E, ����� ���� ���� ������� ����";
               
            }

            else if (activityChecker.AreAllChildrenActive)
            {
                if (hit.collider.CompareTag("CoffeConstruct"))
                {
                    interactionText.text = "������� E, ����� ������� ����";
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        TakeCoffee();
                    }
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

    public void TakeCoffee()
    {
        CoffeIsprepared = true;
        PreparedCoffe.SetActive(false);
        PreparedCoffeInHand.SetActive(true);
    }

    

    System.Collections.IEnumerator PlayAnimationAndTakeCup()
    {
        Collider cupCollider = Cup.GetComponent<Collider>();
        Collider lidCollider = Lid.GetComponent<Collider>();

        
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
        

        // ��������� ��������
        LidAnim.Play();

        // ���� ���������� �������� (�� ������������)
        yield return new WaitForSeconds(animationDuration);

        // �������� ����� ����� ���������� ��������
        TakeLid();
    }

    System.Collections.IEnumerator BrewCoffee()
    {
        elapsedTime = 0f; // ���������� �����
        coffeePour.Play(); // �������� ������ �����

        Vector3 startScale = Vector3.zero; // ��������� �������
        Vector3 endScale = new Vector3(90f, 80f, 90f); // �������� �������

        while (elapsedTime < PreparingCoffeTime)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / PreparingCoffeTime;
            Coffee.transform.localScale = Vector3.Lerp(startScale, endScale, t); // ������� ���������������
            Debug.Log("Scale: " + Coffee.transform.localScale); // �������
            yield return null;
        }

        coffeePour.Stop(); // ��������� ������
    }

    private IEnumerator PlayMoneyEffectOnce()
    {
        if (moneyEffectPlayed) yield break;

        moneyEffectPlayed = true;
        MoneyBoom.Play();

        // ���� ���������� ������� (����� ����� ��������)
        yield return new WaitForSeconds(MoneyBoom.main.duration);

        // ������ ��������. ���� �����, ������ ��� ���-�� ��� ������� (����. ������ �������)
    }
}
