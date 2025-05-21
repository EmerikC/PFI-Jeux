using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;
using System.Collections;

public class caissesScript : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] CartItems cart;
    [SerializeField] GameObject[] itemsToWin;

    List<string> checkList;

    [SerializeField] List<GameObject> itemsNotNeededSFX;
    [SerializeField] List<GameObject> missingItemsSFX;

    AudioSource audioSource;
    private void Start()
    {
        // initialisation de la liste des noms d'objets à ramasser
        ResetCheckList();

        // récupération de la référence à l'AudioSource
        audioSource = GetComponent<AudioSource>();
    }

    // fonction pour réinitialiser la liste des noms d'objets à ramasser
    private void ResetCheckList()
    {
        checkList = itemsToWin.Select(go => NormalizeName(go.name)).ToList();
    }

    // fonction utilitaire pour normaliser le nom d'un objet
    private string NormalizeName(string name)
    {
        if (string.IsNullOrEmpty(name)) return "";
        return name.Trim().ToLowerInvariant();
    }

    // fonction pour vérifier si le joueur a ramassé tous les objets
    private void OnTriggerEnter(Collider other)
    {
        // on vérifie si le joueur entre dans le trigger
        if (other.gameObject == player)
        {
            // reset de la liste des noms d'objets à ramasser
            ResetCheckList();

            // vérification de la liste d'objets ramassés
            foreach (GameObject item in cart.itemsInCart)
            {
                if (item == null) continue;

                // on normalise le nom de l'objet ramassé
                string normalizedName = NormalizeName(item.name);

                // vérification si l'objet ramassé est dans la liste des objets à ramasser
                bool found = false;
                for (int i = 0; i < checkList.Count; i++)
                {
                    if (checkList[i] == normalizedName)
                    {
                        checkList.RemoveAt(i);
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    // item non demandé dans la liste
                    // on joue un son d'erreur
                    GameObject sfx = ObjectPool.ObjectPoolinstance.GetPooledObject(itemsNotNeededSFX[Random.Range(0, itemsNotNeededSFX.Count)]);
                    if (sfx != null)
                    {
                        sfx.transform.position = GameObject.FindGameObjectWithTag("Player").transform.position;
                        sfx.SetActive(true);
                    }

                    // reset de la liste des noms d'objets à ramasser et sortie de la fonction
                    ResetCheckList();
                    return;
                }
            }

            if (checkList.Count == 0)
            {
                StartCoroutine(EndGameSequence());
            }
            else
            {
                // il manque des objets
                // on joue un son d'erreur
                GameObject sfx = ObjectPool.ObjectPoolinstance.GetPooledObject(missingItemsSFX[Random.Range(0, missingItemsSFX.Count)]);
                if (sfx != null)
                {
                    sfx.transform.position = GameObject.FindGameObjectWithTag("Player").transform.position;
                    sfx.SetActive(true);
                }

                // reset de la liste des noms d'objets à ramasser
                ResetCheckList();
            }
        }
    }

    private IEnumerator EndGameSequence()
    {
        // Block player controls
        if (player != null)
            player.GetComponent<PlayerScript>().canMove = false;

        // Play audio if available
        if (audioSource != null && audioSource.clip != null)
        {
            audioSource.Play();
            yield return new WaitWhile(() => audioSource.isPlaying);
        }

        // Unlock and show cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Load the GameLost scene
        SceneManager.LoadScene("GameWon");
    }
}
