using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerTake : MonoBehaviour
{
    [Header("Settings")]
    public float rayDistance = 2.0f;
    public float preparingCoffeeTime = 2.0f;
    public float animationDuration = 1.0f;

    [Header("UI Elements")]
    public Text interactionText;

    public GameObject LidInHand;
    public GameObject Lid;
    public GameObject CupInHand;
    public GameObject Cup;
    public GameObject Coffee;

    public GameObject PreparedCoffee;
    public GameObject PreparedCoffeeInHand;

    public Animation CupAnim;
    public Animation LidAnim;

    public ParticleSystem coffeePour;
    public ParticleSystem MoneyBoom;
    public ChildActivityChecker activityChecker;

    private bool hasCup = false; // Состояние: стакан взят
    private bool coffeeBrewed = false; // Состояние: кофе налит
    private bool hasLid = false; // Состояние: крышка взята
    private bool coffeeReady = false; // Состояние: кофе готов к выдаче
    private bool moneyEffectPlayed = false;

    private float elapsedTime = 0f;

    [Header("Client Reaction")]
    public SkinnedMeshRenderer clientFaceRenderer; // Renderer клиента с BlendShapes
    public int smileBlendShapeIndex = 0; // Индекс BlendShape "Smile"
    public float smileAnimationDuration = 1.5f;
    public AudioSource paymentSound; // Звук оплаты
    public GameObject directorFinalScene; // Объект финальной сцены
    public Director directorScript; // Ссылка на скрипт Director

    public AudioSource bellSound; // Звук колокольчика



    void Start()
    {
        Coffee.transform.localScale = Vector3.zero;
        LidInHand.SetActive(false);
        CupInHand.SetActive(false);
        PreparedCoffeeInHand.SetActive(false);
    }

    void Update()
    {
        if (CoffeeCupThrow.CupIsActive == false)
        {
            Debug.Log("Клиент радостный");
            StartCoroutine(PlayMoneyEffectOnce());
        }

        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        interactionText.text = "";

        if (Physics.Raycast(ray, out hit, rayDistance))
        {
            if (hit.collider.CompareTag("Cups") && !hasCup)
            {
                interactionText.text = "Нажмите E, чтобы взять стаканчик";
                if (Input.GetKeyDown(KeyCode.E))
                {
                    StartCoroutine(PlayAnimationAndTakeCup());
                }
            }
            else if (hit.collider.CompareTag("CoffeMachine") && hasCup && !coffeeBrewed)
            {
                interactionText.text = "Нажмите E, чтобы заварить кофе";
                if (Input.GetKeyDown(KeyCode.E))
                {
                    StartCoroutine(BrewCoffee());
                }
            }
            else if (hit.collider.CompareTag("Lids") && coffeeBrewed && !hasLid)
            {
                interactionText.text = "Нажмите E, чтобы взять крышку";
                if (Input.GetKeyDown(KeyCode.E))
                {
                    StartCoroutine(PlayAnimationAndTakeLid());
                }
            }
            else if (activityChecker.AreAllChildrenActive && hasLid && !coffeeReady)
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
            else if (coffeeReady && hit.collider.CompareTag("Client"))
            {
                interactionText.text = "Нажмите E, чтобы дать кофе клиенту";
                if (Input.GetKeyDown(KeyCode.E))
                {
                    GiveCoffeeToClient();
                }
            }
        }

        Debug.DrawRay(transform.position, transform.forward * rayDistance, Color.red);
    }

    public void TakeCup()
    {
        CupInHand.SetActive(true);
        Cup.SetActive(false);
        hasCup = true;
        Debug.Log("Стакан взят");
    }

    public void TakeLid()
    {
        LidInHand.SetActive(true);
        Lid.SetActive(false);
        hasLid = true;
        Debug.Log("Крышка взята");
    }

    public void TakeCoffee()
    {
        PreparedCoffee.SetActive(false);
        PreparedCoffeeInHand.SetActive(true);
        coffeeReady = true;
        Debug.Log("Кофе забран");
    }

    public void GiveCoffeeToClient()
    {
        PreparedCoffeeInHand.SetActive(false);
        CoffeeCupThrow.CupIsActive = false;
        coffeeReady = false;
        hasCup = false;
        hasLid = false;
        coffeeBrewed = false;

        StartCoroutine(ReactClientAfterCoffee());
    }

    private IEnumerator ReactClientAfterCoffee()
    {
        // 1. Анимация улыбки клиента через BlendShape
        float elapsed = 0f;
        while (elapsed < smileAnimationDuration)
        {
            elapsed += Time.deltaTime;
            float weight = Mathf.Lerp(0f, 100f, elapsed / smileAnimationDuration);
            clientFaceRenderer.SetBlendShapeWeight(smileBlendShapeIndex, weight);
            yield return null;
        }

        // 2. Звук оплаты
        if (paymentSound != null && paymentSound.clip != null)
        {
            paymentSound.Play();
            yield return new WaitForSeconds(paymentSound.clip.length);
        }

        // 4. Fade In
        yield return StartCoroutine(directorScript.FadeInDarkScreen());

        // 3. Звук колокольчика
        if (bellSound != null && bellSound.clip != null)
        {
            bellSound.Play();
            yield return new WaitForSeconds(bellSound.clip.length);
        }

       
        // 5. Активация финального объекта
        if (directorFinalScene != null)
            directorFinalScene.SetActive(true);

        // 6. Fade Out
        yield return StartCoroutine(directorScript.FadeOutDarkScreen());
    }



    System.Collections.IEnumerator PlayAnimationAndTakeCup()
    {
        Collider cupCollider = Cup.GetComponent<Collider>();
        cupCollider.enabled = false;

        CupAnim.Play();
        yield return new WaitForSeconds(animationDuration);
        TakeCup();
    }

    System.Collections.IEnumerator PlayAnimationAndTakeLid()
    {
        Collider lidCollider = Lid.GetComponent<Collider>();
        lidCollider.enabled = false;

        LidAnim.Play();
        yield return new WaitForSeconds(animationDuration);
        TakeLid();
    }

    System.Collections.IEnumerator BrewCoffee()
    {
        elapsedTime = 0f;
        coffeePour.Play();

        Vector3 startScale = Vector3.zero;
        Vector3 endScale = new Vector3(90f, 80f, 90f);

        while (elapsedTime < preparingCoffeeTime)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / preparingCoffeeTime;
            Coffee.transform.localScale = Vector3.Lerp(startScale, endScale, t);
            Debug.Log("Scale: " + Coffee.transform.localScale);
            yield return null;
        }

        coffeePour.Stop();
        coffeeBrewed = true;
        Debug.Log("Кофе заварен");
    }

    private IEnumerator PlayMoneyEffectOnce()
    {
        if (moneyEffectPlayed) yield break;

        moneyEffectPlayed = true;
        MoneyBoom.Play();
        yield return new WaitForSeconds(MoneyBoom.main.duration);
    }
}