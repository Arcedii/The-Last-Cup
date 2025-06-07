using UnityEngine;

public class ObjectTransfer : MonoBehaviour
{
    [System.Serializable]
    public class TransferPair
    {
        public string triggerTag;       // Тег триггера
        public GameObject objectInHand; // Объект в руках
        public GameObject objectAtMachine; // Объект у машины
    }

    [Header("Transfer Settings")]
    public TransferPair[] transferPairs; // Массив пар для передачи

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