using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    [Header("Thời gian")]
    [Tooltip("Số phút thực tế = 1 giờ trong game")]
    public float realMinutesPerGameHour = 1f; // 1 phút = 1 giờ
    [Range(0, 24)]
    public float currentHour = 12f;           // bắt đầu lúc 12h trưa
    [Tooltip("Hệ số nhân tốc độ thời gian")]
    public float timeMultiplier = 1f;         // 1 = bình thường, 2 = nhanh gấp 2 lần

    [Header("Ánh sáng")]
    public Light sunLight;
    public float maxIntensity = 1f;
    public float minIntensity = 0.05f;

    private float secondsPerGameHour;

    void Start()
    {
        if (sunLight == null)
        {
            sunLight = RenderSettings.sun;
        }
        secondsPerGameHour = realMinutesPerGameHour * 60f;
    }

    void Update()
    {
        // Cập nhật thời gian với tốc độ nhân
        currentHour += (Time.deltaTime / secondsPerGameHour) * timeMultiplier;
        if (currentHour >= 24f) currentHour -= 24f;

        UpdateLighting();
    }

    void UpdateLighting()
    {
        // Góc mặt trời: 0 độ = trưa, 180 = tối
        float sunAngle = (currentHour / 24f) * 360f - 90f;
        sunLight.transform.rotation = Quaternion.Euler(sunAngle, 170f, 0f);

        // Intensity ánh sáng
        float intensity;
        if (currentHour <= 12f)
        {
            intensity = Mathf.Lerp(minIntensity, maxIntensity, currentHour / 12f);
        }
        else
        {
            intensity = Mathf.Lerp(maxIntensity, minIntensity, (currentHour - 12f) / 12f);
        }

        sunLight.intensity = intensity;
    }
}
