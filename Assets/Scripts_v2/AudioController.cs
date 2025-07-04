using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;

    public bool IsPlaying => audioSource.isPlaying;

    /// <summary>
    /// ����@�q�y��
    /// </summary>
    public void PlayAudio(AudioClip clip)
    {
        if (clip == null)
        {
            Debug.LogWarning("AudioController: �ǤJ�� AudioClip �O null");
            return;
        }

        audioSource.Stop();
        audioSource.clip = clip;
        audioSource.Play();
    }

    /// <summary>
    /// ����ثe���y������
    /// </summary>
    public void Stop()
    {
        if (audioSource.isPlaying)
            audioSource.Stop();
    }
}
