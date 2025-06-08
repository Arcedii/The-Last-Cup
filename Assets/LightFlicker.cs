using UnityEngine;

[RequireComponent(typeof(Light))]
public class LightFlicker : MonoBehaviour
{
    public float minIntensity = 10f;
    public float maxIntensity = 30f;
    public float flickerSpeed = 5f;

    private Light flickerLight;
    private float timer;

    void Start()
    {
        flickerLight = GetComponent<Light>();
        timer = flickerSpeed;
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            float newIntensity = Random.Range(minIntensity, maxIntensity);
            flickerLight.intensity = newIntensity;

            timer = flickerSpeed + Random.Range(0f, flickerSpeed); // немного рандома в частоте
        }
    }
}
