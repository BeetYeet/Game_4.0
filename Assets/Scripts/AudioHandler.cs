using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools;

public class AudioHandler : MonoBehaviour
{
    public static AudioHandler instance;

    private AudioSource template;

    [SerializeField]
    private List<AudioClip> rawClips = new List<AudioClip>();

    public static Dictionary<string, AudioClip> clips { get; private set; }

    [SerializeField]
    private int baseVoiceCount = 5;

    [SerializeField]
    private int voicesInUse = 0;

    [SerializeField, Range(0f, 1f)]
    private float defaultSpatialBlend = 1f;

    private static List<AudioSource> voices = new List<AudioSource>();
    private static List<AudioSource> freeVoices = new List<AudioSource>();

    [SerializeField]
    private AudioContext buttonHoverContext;

    [SerializeField]
    private AudioContext buttonClickContext;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        template = GetComponent<AudioSource>();
        InitializeVoices();
        InitializeClips();
    }

    public static void Click()
    {
        PlaySound("Menu Click", instance.buttonClickContext);
    }

    public static void Click2()
    {
        PlaySound("Menu Click 2", instance.buttonHoverContext);
    }

    private void Update()
    {
        voicesInUse = voices.Count - freeVoices.Count;
    }

    private void InitializeClips()
    {
        clips = new Dictionary<string, AudioClip>();
        rawClips.ForEach(x => clips.Add(x.name, x));
    }

    private void InitializeVoices()
    {
        voices.Add(template);
        freeVoices.Add(template);
        for (int i = 1; i < baseVoiceCount; i++)
        {
            NewVoice(i);
        }
        voices.ForEach(x => x.transform.position = Vector3.zero);
    }

    private void NewVoice(int index)
    {
        GameObject voiceGameObject = new GameObject($"Voice {index}");
        voiceGameObject.transform.parent = transform;
        AudioSource audioSource = template.CopyComponent(voiceGameObject);
        voices.Add(audioSource);
        freeVoices.Add(audioSource);
    }

    public static AudioCallback PlaySound(string name)
    {
        return PlaySound(name, AudioContext.empty, Vector3.zero);
    }

    public static AudioCallback PlaySound(string name, AudioContext context)
    {
        return PlaySound(name, context, Vector3.zero);
    }

    public static AudioCallback PlaySound(string name, Vector3 position)
    {
        AudioContext context = AudioContext.empty;
        context.spatialBlend = instance.defaultSpatialBlend;
        return PlaySound(name, context, position);
    }

    public static AudioCallback PlaySound(string name, AudioContext context, Vector3 position)
    {
        if (freeVoices.Count < 1)
        {
            // we need another voice
            instance.NewVoice(voices.Count);
        }
        // get name
        AudioClip clip = clips[name];
        // get source
        AudioSource source = freeVoices[0];
        // setup and play
        return PlaySoundUsing(source, clip, context, position);
    }

    private static AudioCallback PlaySoundUsing(AudioSource source, AudioClip clip, AudioContext context, Vector3 position)
    {
        source.clip = clip;
        source.transform.position = position;
        source.Apply(context);
        source.time = 0f;
        source.Play();
        freeVoices.Remove(source);

        AudioCallback callback = new AudioCallback(source, clip, context);

        // return to free voices when done
        instance.StartCoroutine(WaitForComplete(source, callback));

        return callback;
    }

    private static IEnumerator WaitForComplete(AudioSource source, AudioCallback callback)
    {
        while (source.isPlaying)
        {
            yield return null;
        }
        callback.Stop();
    }

    private static void StopSound(AudioSource source)
    {
        source.time = 0f;
        freeVoices.Add(source);
    }

    [System.Serializable]
    public struct AudioContext
    {
        public float volume;
        public float minPitch, maxPitch;
        public float spatialBlend;
        public int priority;

        public static readonly AudioContext empty = new AudioContext(.5f, 1f, 0f, 128);

        public AudioContext(float volume, float pitch, float spatialBlend, int priority)
        {
            this.volume = Mathf.Clamp01(volume);
            this.minPitch = pitch;
            this.maxPitch = pitch;
            this.spatialBlend = Mathf.Clamp01(spatialBlend);
            this.priority = priority;
        }

        public AudioContext(float volume, float minPitch, float maxPitch, float spatialBlend, int priority)
        {
            this.volume = Mathf.Clamp01(volume);
            this.minPitch = minPitch;
            this.maxPitch = maxPitch;
            this.spatialBlend = Mathf.Clamp01(spatialBlend);
            this.priority = priority;
        }
    }

    public struct AudioCallback
    {
        public readonly AudioSource source;
        public readonly AudioClip clip;
        public readonly AudioContext startingContext;
        public string Name { get { return clip.name; } }
        public bool isPlaying;

        public event System.Action OnEnd;

        public AudioCallback(AudioSource source, AudioClip clip, AudioContext context)
        {
            this.source = source;
            this.clip = clip;
            this.isPlaying = true;
            this.startingContext = context;
            OnEnd = null;
            OnEnd += NotPlaying;
        }

        private void NotPlaying()
        {
            isPlaying = false;
        }

        public void Stop()
        {
            if (!isPlaying) return;
            StopSound(source);
            OnEnd?.Invoke();
        }
    }
}

public static class AudioExtras
{
    public static T CopyComponent<T>(this T original, GameObject destination) where T : Component
    {
        System.Type type = original.GetType();
        var dst = destination.GetComponent(type) as T;
        if (!dst) dst = destination.AddComponent(type) as T;
        var fields = type.GetFields();
        foreach (var field in fields)
        {
            if (field.IsStatic) continue;
            field.SetValue(dst, field.GetValue(original));
        }
        var props = type.GetProperties();
        foreach (var prop in props)
        {
            if (!prop.CanWrite || !prop.CanWrite || prop.Name == "name") continue;
            if (!prop.IsDefined(typeof(System.ObsoleteAttribute), true))
                prop.SetValue(dst, prop.GetValue(original, null), null);
        }
        return dst as T;
    }

    public static void Apply(this AudioSource source, AudioHandler.AudioContext context)
    {
        source.volume = context.volume;

        if (context.minPitch == context.maxPitch)
            source.pitch = context.minPitch;
        else
        {
            source.pitch = NormalRandom(context.minPitch, context.maxPitch, 0.1f, Mathf.Infinity);
        }

        source.spatialBlend = context.spatialBlend;
        source.priority = context.priority;
    }

    public static float NormalRandom(float min, float max, float absMin, float absMax)
    {
        float u, v, S;

        do
        {
            u = 2.0f * Random.value - 1.0f;
            v = 2.0f * Random.value - 1.0f;
            S = u * u + v * v;
        }
        while (S >= 1.0f);

        // Standard Normal Distribution
        float std = u * Mathf.Sqrt(-2.0f * Mathf.Log(S) / S);

        float mean = (min + max) / 2f;
        float sigma = (max - mean) / 3f;
        return Mathf.Clamp(std * sigma + mean, absMin, absMax);
    }
}