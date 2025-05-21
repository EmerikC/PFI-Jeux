using UnityEngine;
using System.Collections;

public class SFXPlayer : MonoBehaviour
{
    AudioSource audioSource;

    private void Start()
    {
        // Récupération de la référence à l'AudioSource
        audioSource = GetComponent<AudioSource>();
    }

    // Fonction pour jouer le son lors de l'activation de l'objet
    private void OnEnable()
    {
        // On joue le son
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        // Vérification si l'AudioSource est présent et s'il doit jouer au démarrage
        if (audioSource != null && audioSource.playOnAwake && audioSource.clip != null)
        {
            StartCoroutine(DeactivateWhenDone());
        }
    }

    // Fonction pour jouer le son lors de l'activation de l'objet
    private IEnumerator DeactivateWhenDone()
    {
        // On attend que le son soit terminé puis on désactive l'objet
        yield return new WaitWhile(() => audioSource.isPlaying);

        gameObject.SetActive(false);
    }
}
