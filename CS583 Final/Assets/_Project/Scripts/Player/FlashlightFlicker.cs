using UnityEngine;

public class FlashlightFlicker : MonoBehaviour
{
    public Light lightSource;
    public float minIntensity = 2f;
    public float maxIntensity = 4f;
    public float speed = 10f;

    void Update()
    {
        float noise = Mathf.PerlinNoise(Time.time * speed, 0f);
        lightSource.intensity = Mathf.Lerp(minIntensity, maxIntensity, noise);
    }
}
