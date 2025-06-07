using UnityEngine;
using UnityEngine.UI;
using static Unity.Burst.Intrinsics.X86.Avx;

public class PlayerTake : MonoBehaviour
{
    [Header("Settings")]
    public float rayDistance = 2.0f;
    public float PreparingCoffeTime = 2.0f;
    public float elapsedTime = 0f;
    public float startFill = 0f; // Начальный уровень
    public float endFill = 1f; // Конечный уровень (почти полный стакан)

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
    public ChildActivityChecker activityChecker;



    public float animationDuration = 1.0f;

    void Start()
    { 
        Coffee.transform.localScale = Vector3.zero;
    }

    void Update()
    {
       
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        
        interactionText.text = "";

        if (Physics.Raycast(ray, out hit, rayDistance))
        {
          
            if (hit.collider.CompareTag("Cups"))
            {
                interactionText.text = "Нажмите  E, чтобы  взять  стаканчик";

                if (Input.GetKeyDown(KeyCode.E))
                {
                    StartCoroutine(PlayAnimationAndTakeCup());
                }
            }
            
            else if (hit.collider.CompareTag("Lids"))
            {
                interactionText.text = "Нажмите  E,  чтобы  взять  крышку";

                if (Input.GetKeyDown(KeyCode.E))
                {
                    StartCoroutine(PlayAnimationAndTakeLid());
                }
            }

            else if (hit.collider.CompareTag("CoffeMachine"))
            {
                interactionText.text = "Нажмите E, чтобы заварить кофе";
                if (Input.GetKeyDown(KeyCode.E))
                {
                    StartCoroutine(BrewCoffee());
                }
            }

            else if (activityChecker.AreAllChildrenActive)
            {
                if (hit.collider.CompareTag("CoffeConstruct"))
                {
                    interactionText.text = "Нажмите E, чтобы забрать кофе";
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
        PreparedCoffe.SetActive(false);
        PreparedCoffeInHand.SetActive(true);
    }

    System.Collections.IEnumerator PlayAnimationAndTakeCup()
    {
        Collider cupCollider = Cup.GetComponent<Collider>();
        Collider lidCollider = Lid.GetComponent<Collider>();

        
        cupCollider.enabled = false;


        // Запускаем анимацию
        CupAnim.Play();

        // Ждем завершения анимации (по длительности)
        yield return new WaitForSeconds(animationDuration);

        // Вызываем метод после завершения анимации
        TakeCup();
    }

    System.Collections.IEnumerator PlayAnimationAndTakeLid()
    {
        Collider cupCollider = Cup.GetComponent<Collider>();
        Collider lidCollider = Lid.GetComponent<Collider>();

        lidCollider.enabled = false;
        

        // Запускаем анимацию
        LidAnim.Play();

        // Ждем завершения анимации (по длительности)
        yield return new WaitForSeconds(animationDuration);

        // Вызываем метод после завершения анимации
        TakeLid();
    }

    System.Collections.IEnumerator BrewCoffee()
    {
        elapsedTime = 0f; // Сбрасываем время
        coffeePour.Play(); // Включаем эффект струи

        Vector3 startScale = Vector3.zero; // Начальный масштаб
        Vector3 endScale = new Vector3(90f, 80f, 90f); // Конечный масштаб

        while (elapsedTime < PreparingCoffeTime)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / PreparingCoffeTime;
            Coffee.transform.localScale = Vector3.Lerp(startScale, endScale, t); // Плавное масштабирование
            Debug.Log("Scale: " + Coffee.transform.localScale); // Отладка
            yield return null;
        }

        coffeePour.Stop(); // Выключаем эффект
    }
}
