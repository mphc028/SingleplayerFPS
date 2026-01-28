using UnityEngine;

[CreateAssetMenu(fileName = "AudioSourceArray", menuName = "ScriptableObjects/AudioSourceArray", order = 1)]
public class AudioSourceArray : ScriptableObject
{
    [SerializeField]
    private AudioClip[] audioClips;

    public AudioClip[] AudioClips
    {
        get { return audioClips; }
    }

    public int Length
    {
        get { return audioClips.Length; }
    }

    public AudioClip GetAudioClip(int index)
    {
        if (index >= 0 && index < audioClips.Length)
        {
            return audioClips[index];
        }
        else
        {
            Debug.LogError("Index out of range for AudioSourceArray");
            return null;
        }
    }
}