using UnityEngine;
using System.Collections;

public class CoffeeCupThrow : MonoBehaviour
{
    public float throwForce = 10f;
    public Transform handTransform;  // Сюда вернём чашку
    private Rigidbody rb;
    private bool hasBeenThrown = false;
    private bool hasHitClient = false;
    public static bool CupIsActive = true;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.detectCollisions = false;
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
        transform.parent = null;
        rb.isKinematic = false;
        rb.detectCollisions = true;
        rb.AddForce(transform.forward * throwForce, ForceMode.Impulse);
        hasBeenThrown = true;

        StartCoroutine(ReturnIfNoHit());
    }

    IEnumerator ReturnIfNoHit()
    {
        yield return new WaitForSeconds(2f);

        if (!hasHitClient)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;
            rb.detectCollisions = false;

            transform.position = handTransform.position;
            transform.rotation = handTransform.rotation;
            transform.parent = handTransform;

            hasBeenThrown = false;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (hasBeenThrown && collision.gameObject.CompareTag("Client"))
        {
            hasHitClient = true;
            gameObject.SetActive(false);
            CupIsActive = false;
        }
    }
}
