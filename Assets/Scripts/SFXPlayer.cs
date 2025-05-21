using UnityEngine;
using System.Collections;

public class SFXPlayer : MonoBehaviour
{
    AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        if (audioSource != null && audioSource.playOnAwake && audioSource.clip != null)
        {
            StartCoroutine(DeactivateWhenDone());
        }
    }

    private IEnumerator DeactivateWhenDone()
    {
        // Wait until the audio has finished playing
        yield return new WaitWhile(() => audioSource.isPlaying);

        gameObject.SetActive(false);
    }
}
