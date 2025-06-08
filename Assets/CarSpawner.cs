using UnityEngine;

public class CarSpawner : MonoBehaviour
{
    public GameObject[] carPrefabs;
    public Transform spawnPoint;
    public float spawnInterval = 3f;
    public float carSpeed = 5f;

    void Start()
    {
        InvokeRepeating(nameof(SpawnCar), 0f, spawnInterval);
    }

    void SpawnCar()
    {
        int index = Random.Range(0, carPrefabs.Length);
        GameObject car = Instantiate(carPrefabs[index], spawnPoint.position, Quaternion.Euler(0, 90, 0)); // поворот по Y
        CarMover mover = car.AddComponent<CarMover>();
        mover.speed = carSpeed;
    }
}
