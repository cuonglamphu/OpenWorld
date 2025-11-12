using UnityEngine;

public class FireflyController : MonoBehaviour
{
    public ParticleSystem fireflies;
    public Light sunLight; // ánh sáng mặt trời hoặc directional light

    void Update()
    {
        if (sunLight.intensity < 0.3f)
        {
            if (!fireflies.isPlaying)
                fireflies.Play();  // Ban đêm
        }
        else
        {
            if (fireflies.isPlaying)
                fireflies.Stop();  // Ban ngày
        }
    }
}
