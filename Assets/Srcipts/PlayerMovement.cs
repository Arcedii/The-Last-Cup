using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5.0f; // Скорость движения
    public float friction = 5.0f;  // Сила трения для остановки

    [Header("Mouse Look Settings")]
    public float mouseSensitivity = 100.0f; // Чувствительность мыши
    public Transform playerBody; // Тело персонажа (для поворота)
    public Transform cameraTransform; // Камера (для поворота вверх/вниз)
    private float xRotation = 0f; // Угол поворота камеры по вертикали

    private Rigidbody rb;

    void Start()
    {
        // Получаем компонент Rigidbody
        rb = GetComponent<Rigidbody>();

        // Блокируем и скрываем курсор для плавного управления мышью
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Отключаем вращение Rigidbody по умолчанию (управляем вручную)
        rb.freezeRotation = true;
    }

    void FixedUpdate()
    {
        // Движение
        MovePlayer();
    }

    void Update()
    {
        // Управление камерой мышью
        LookWithMouse();
    }

    void MovePlayer()
    {
        // Получаем ввод с клавиатуры (WASD)
        float horizontal = Input.GetAxisRaw("Horizontal"); // A/D
        float vertical = Input.GetAxisRaw("Vertical");     // W/S

        // Вычисляем направление движения относительно ориентации персонажа
        Vector3 moveDirection = new Vector3(horizontal, 0f, vertical).normalized;
        moveDirection = transform.TransformDirection(moveDirection);

        // Применяем силу движения
        Vector3 moveForce = moveDirection * moveSpeed;
        rb.linearVelocity = new Vector3(moveForce.x, rb.linearVelocity.y, moveForce.z);

        // Добавляем трение для плавной остановки
        if (moveDirection.magnitude == 0)
        {
            Vector3 frictionForce = -rb.linearVelocity * friction * Time.fixedDeltaTime;
            rb.AddForce(frictionForce, ForceMode.Impulse);
        }
    }

    void LookWithMouse()
    {
        // Получаем ввод с мыши
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Поворот камеры по вертикали (ограничение угла)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Ограничиваем угол от -90 до 90 градусов
        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Поворот тела персонажа по горизонтали
        playerBody.Rotate(Vector3.up * mouseX);
    }
}
