using UnityEngine;
using System.Collections;

public class SFXPlayer : MonoBehaviour
{
    AudioSource audioSource;

    private void Start()
    {
        // R�cup�ration de la r�f�rence � l'AudioSource
        audioSource = GetComponent<AudioSource>();
    }

    // Fonction pour jouer le son lors de l'activation de l'objet
    private void OnEnable()
    {
        // On joue le son
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        // V�rification si l'AudioSource est pr�sent et s'il doit jouer au d�marrage
        if (audioSource != null && audioSource.playOnAwake && audioSource.clip != null)
        {
            StartCoroutine(DeactivateWhenDone());
        }
    }

    // Fonction pour jouer le son lors de l'activation de l'objet
    private IEnumerator DeactivateWhenDone()
    {
        // On attend que le son soit termin� puis on d�sactive l'objet
        yield return new WaitWhile(() => audioSource.isPlaying);

        gameObject.SetActive(false);
    }
}
