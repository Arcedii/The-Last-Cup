using UnityEngine;

public class CoffeeCupThrow : MonoBehaviour
{
    public float throwForce = 10f;
    private Rigidbody rb;
    private bool hasBeenThrown = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;         // Физика отключена
        rb.detectCollisions = false;   // Игнорирует столкновения до броска
    }

    void Update()
    {
        if (!hasBeenThrown && Input.GetKeyDown(KeyCode.E))
        {
            ThrowForward();
        }
    }

    void ThrowForward()
    {
        transform.parent = null;           // Отсоединяем от игрока (если был дочерним)
        rb.isKinematic = false;            // Включаем физику
        rb.detectCollisions = true;        // Включаем столкновения
        rb.AddForce(transform.forward * throwForce, ForceMode.Impulse);
        hasBeenThrown = true;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (hasBeenThrown && collision.gameObject.CompareTag("Client"))
        {
            gameObject.SetActive(false);
        }
    }
}
