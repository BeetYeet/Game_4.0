using System.Collections.Generic;
using UnityEngine;

public class LightAnimation : MonoBehaviour
{
    [Range(1, 10)]
    public int layers = 3;

    public new Light light;

    public AnimationCurve intensityOverTime;

    [SerializeField]
    private List<float> time = new List<float>();

    public float intensityMultiplier = 80f;
    public float timeCutoff = 5;
    public bool followTimeScale = true;

    [Range(0f, .4f)]
    public float debugTime = 0f;

    private void Start()
    {
        light.enabled = false;
    }

    private void OnValidate()
    {
        light.enabled = true;
        RecalculateIntensity(new List<float>() { debugTime });
    }

    public void Trigger()
    {
        time.Add(0f);
    }

    private void LateUpdate()
    {
        UpdateTime();
        if (time != null && time.Count > 0)
        {
            light.enabled = true;
        }
        else
        {
            light.enabled = false;
        }
        RecalculateIntensity(time);
    }

    private void UpdateTime()
    {
        if (time == null)
            time = new List<float>();
        List<float> newTime = new List<float>();
        for (int i = 0; i < time.Count; i++)
        {
            time[i] += followTimeScale ? Time.deltaTime : Time.unscaledDeltaTime;
            if (time[i] > timeCutoff)
            {
                continue;
            }
            if (i < time.Count - layers)
            {
                Debug.LogWarning("Lightpool saturated");
                continue;
            }
            else
                newTime.Add(time[i]);
        }
        time = newTime;
    }

    private void RecalculateIntensity(List<float> times)
    {
        float total = 0f;
        for (int i = 0; i < times.Count; i++)
        {
            total += intensityOverTime.Evaluate(times[i]);
        }
        light.intensity = total * intensityMultiplier;
    }
}