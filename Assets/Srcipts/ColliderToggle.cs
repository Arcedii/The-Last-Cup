using UnityEngine;

public class ColliderToggle : MonoBehaviour
{
    [Header("Settings")]
    public GameObject object1; // ������ GameObject, ��� ���������� �����������
    public GameObject object2; // ������ GameObject � �����������

    void Update()
    {
        if (object2 != null)
        {
            Collider collider = object2.GetComponent<Collider>();
            if (collider != null)
            {
                if (object1 != null)
                {
                    collider.enabled = object1.activeSelf; // ��������/��������� ��������� � ����������� �� ���������� object1
                    Debug.Log("Object1 active: " + object1.activeSelf + ", Collider on Object2 enabled: " + collider.enabled);
                }
                else
                {
                    Debug.LogError("Object1 is not assigned!");
                }
            }
            else
            {
                Debug.LogError("No Collider found on Object2!");
            }
        }
        else
        {
            Debug.LogError("Object2 is not assigned!");
        }
    }
}