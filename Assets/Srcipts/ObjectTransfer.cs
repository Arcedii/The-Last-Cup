using UnityEngine;

public class ObjectTransfer : MonoBehaviour
{
    [System.Serializable]
    public class TransferPair
    {
        public string triggerTag;       // ��� ��������
        public GameObject objectInHand; // ������ � �����
        public GameObject objectAtMachine; // ������ � ������
    }

    [Header("Transfer Settings")]
    public TransferPair[] transferPairs; // ������ ��� ��� ��������

    void OnTriggerEnter(Collider other)
    {
        foreach (TransferPair pair in transferPairs)
        {
            if (pair.objectInHand == null || pair.objectAtMachine == null)
            {             
                continue;
            }

            if (other.CompareTag(pair.triggerTag) && pair.objectInHand.activeSelf)
            {             
                pair.objectInHand.SetActive(false);
                pair.objectAtMachine.SetActive(true);
            }
        }
    }
}