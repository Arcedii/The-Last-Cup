using UnityEngine;

public class ChildActivityChecker : MonoBehaviour
{
    [Header("Specific Objects Check")]
    public GameObject firstObject;   // Первый объект (например, cup)
    public GameObject secondObject;  // Второй объект (например, lid)
    public GameObject thirdObject;   // Третий объект, который включаем (например, Coffee)

    [Header("All Children Check")]
    public GameObject targetObject; // Объект, дочерние элементы которого проверяем (например, Paper_coffee_cup)

    public bool AreTwoObjectsActive { get; private set; } // Состояние активности двух объектов
    public bool AreAllChildrenActive { get; private set; } // Состояние активности всех дочерних объектов

    void Start()
    {
        if (targetObject == null)
        {
            targetObject = gameObject; // Если не указан, используем текущий объект
        }
        UpdateActivityState();
        

    }

    void Update()
    {
        UpdateActivityState();
    }

    void UpdateActivityState()
    {
        // Проверка двух конкретных объектов
        if (firstObject == null || secondObject == null || thirdObject == null)
        {
            AreTwoObjectsActive = false;
        }
        else
        {
            bool firstActive = firstObject.activeSelf;
            bool secondActive = secondObject.activeSelf;
            AreTwoObjectsActive = firstActive && secondActive;

            // Если оба активны, включаем третий объект
            if (AreTwoObjectsActive)
            {
                thirdObject.SetActive(true);
            }
        }

        // Проверка всех дочерних объектов
        if (targetObject == null)
        {
            AreAllChildrenActive = false;
        }
        else
        {
            bool allActive = true;
            int childCount = targetObject.transform.childCount;

            for (int i = 0; i < childCount; i++)
            {
                Transform child = targetObject.transform.GetChild(i);
                if (!child.gameObject.activeSelf)
                {
                    allActive = false;
                    break;
                }
            }

            AreAllChildrenActive = allActive;
        }
    }
}