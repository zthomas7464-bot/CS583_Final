using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    public Light lightSource;
    public float minIntensity = 0.5f;
    public float maxIntensity = 2f;

    void Update()
    {
        lightSource.intensity = Random.Range(minIntensity, maxIntensity);
    }
}
