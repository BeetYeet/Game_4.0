using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoRise : MonoBehaviour
{
    public float riseSpeed = 1f;

    [Range(0f, 1f)]
    public float risePart = .5f;

    public float lifeTime = 2f;
    private float timeLived = 0f;

    [SerializeField]
    private CanvasGroup group = null;

    [SerializeField]
    private TMPro.TextMeshProUGUI text = null;

    public void SetText(string text)
    {
        this.text.text = text;
    }

    private void Update()
    {
        timeLived += Time.deltaTime;

        if (timeLived > Mathf.InverseLerp(0, lifeTime, risePart * lifeTime))
        {
            float val = Mathf.InverseLerp(risePart * lifeTime, lifeTime, timeLived);
            group.alpha = 1f - val;
        }

        transform.position += Vector3.up * riseSpeed * Time.deltaTime;

        if (timeLived > lifeTime)
        {
            Destroy(gameObject);
        }
    }
}