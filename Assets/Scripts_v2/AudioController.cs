using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;

    public bool IsPlaying => audioSource.isPlaying;

    /// <summary>
    /// 播放一段語音
    /// </summary>
    public void PlayAudio(AudioClip clip)
    {
        if (clip == null)
        {
            Debug.LogWarning("AudioController: 傳入的 AudioClip 是 null");
            return;
        }

        audioSource.Stop();
        audioSource.clip = clip;
        audioSource.Play();
    }

    /// <summary>
    /// 停止目前的語音播放
    /// </summary>
    public void Stop()
    {
        if (audioSource.isPlaying)
            audioSource.Stop();
    }
}
