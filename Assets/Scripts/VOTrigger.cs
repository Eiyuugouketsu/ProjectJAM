using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class VOTrigger : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;

    bool played = false;
    public bool playOnStart;

    void Start()
    {
        if (audioSource != null && playOnStart && !played)
        {
            StartCoroutine(PlayAudio(audioSource,true));
        }

    }
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !played)
        {
            StartCoroutine(PlayAudio(audioSource,true));
            GetComponent<Collider>().enabled = false;
        }
    }
    public void PlayAudioEvent()
    {
        if (audioSource != null && !played)
        StartCoroutine(PlayAudio(audioSource,true));
    }
    public static List<AudioSource> audioQueue = new List<AudioSource>();
    IEnumerator PlayAudio(AudioSource m_audioSource, bool add)
    {
        if(add){
        if(audioQueue.Contains(m_audioSource)) yield break;
        audioQueue.Add(m_audioSource);
        Debug.Log("adding to queue audio " + m_audioSource.clip.name + "| queue count: " + audioQueue.Count);
        if(audioQueue.Count > 1) yield break;
        }
        m_audioSource.Play();
        Debug.Log("playing audio " + m_audioSource.clip.name);
        played = true;
        yield return new WaitForSeconds(m_audioSource.clip.length);
        audioQueue.Remove(m_audioSource);
        if(audioQueue.Count > 0)
            StartCoroutine(PlayAudio(audioQueue[0], false));
    }
    
}
