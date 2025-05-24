using UnityEngine;

public class MicInput : MonoBehaviour
{
    public static float MicLoudness;
    private AudioClip _clip;
    private const int SampleWindow = 128;

    void Start()
    {
        if (Microphone.devices.Length > 0)
        {
            _clip = Microphone.Start(null, true, 1, 44100);
        }
        else
        {
            Debug.LogError("Mikrofon bulunamadı!");
        }
    }

    void Update()
    {
        if (Microphone.IsRecording(null))
        {
            MicLoudness = GetMaxVolume();
        }
    }

    float GetMaxVolume()
    {
        float maxLevel = 0f;
        float[] waveData = new float[SampleWindow];
        int micPosition = Microphone.GetPosition(null) - SampleWindow + 1;
        if (micPosition < 0) return 0;

        _clip.GetData(waveData, micPosition);
        for (int i = 0; i < SampleWindow; i++)
        {
            float wavePeak = Mathf.Abs(waveData[i]);
            if (wavePeak > maxLevel)
                maxLevel = wavePeak;
        }

        return Mathf.Clamp01(maxLevel * 10); 
    }
}