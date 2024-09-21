using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [System.Serializable]
    public struct Audio
    {
        public AudioType type;
        public AudioClip clip;
        [Range(0f, 2f)] public float Volume;
    }

    public List<Audio> m_Audios = new List<Audio>();
    public AudioSource m_Source;

    public void PlayAudio(AudioType type)
    {
        if (!m_Audios.Exists(a => a.type == type))
        {
            Debug.LogError("Audio Type not found");
            return;
        }

        Audio audio = m_Audios.Find(a => a.type == type);
        m_Source.PlayOneShot(audio.clip, audio.Volume);
    }
    public void PlayPurchaseAudio()
    {
        Audio audio = m_Audios.Find(a => a.type == AudioType.Purchase);
        m_Source.PlayOneShot(audio.clip, audio.Volume);
    }

}

public enum AudioType
{
    CollectCoins,
    Purchase,
    Click,
    Error,
    Notification,
    Trash
}
