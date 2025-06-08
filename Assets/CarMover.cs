using UnityEngine;

public class CarMover : MonoBehaviour
{
    public float speed = 5f;

    void Start()
    {
        Destroy(gameObject, 20f); // Удаление через 5 секунд
    }

    void Update()
    {
        transform.position += Vector3.right * speed * Time.deltaTime;
    }
}
