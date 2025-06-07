using UnityEngine;

public class ChildActivityChecker : MonoBehaviour
{
    [Header("Specific Objects Check")]
    public GameObject firstObject;   // ������ ������ (��������, cup)
    public GameObject secondObject;  // ������ ������ (��������, lid)
    public GameObject thirdObject;   // ������ ������, ������� �������� (��������, Coffee)

    [Header("All Children Check")]
    public GameObject targetObject; // ������, �������� �������� �������� ��������� (��������, Paper_coffee_cup)

    public bool AreTwoObjectsActive { get; private set; } // ��������� ���������� ���� ��������
    public bool AreAllChildrenActive { get; private set; } // ��������� ���������� ���� �������� ��������

    void Start()
    {
        if (targetObject == null)
        {
            targetObject = gameObject; // ���� �� ������, ���������� ������� ������
        }
        UpdateActivityState();
        

    }

    void Update()
    {
        UpdateActivityState();
    }

    void UpdateActivityState()
    {
        // �������� ���� ���������� ��������
        if (firstObject == null || secondObject == null || thirdObject == null)
        {
            AreTwoObjectsActive = false;
        }
        else
        {
            bool firstActive = firstObject.activeSelf;
            bool secondActive = secondObject.activeSelf;
            AreTwoObjectsActive = firstActive && secondActive;

            // ���� ��� �������, �������� ������ ������
            if (AreTwoObjectsActive)
            {
                thirdObject.SetActive(true);
            }
        }

        // �������� ���� �������� ��������
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