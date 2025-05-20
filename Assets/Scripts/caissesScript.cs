using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;

public class caissesScript : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] CartItems cart;
    [SerializeField] GameObject[] itemsToWin;

    List<string> checkList;

    private void Start()
    {
        // initialisation de la liste des noms d'objets à ramasser
        ResetCheckList();
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
                    // reset de la liste des noms d'objets à ramasser et sortie de la fonction
                    ResetCheckList();
                    return;
                }
            }

            if (checkList.Count == 0)
            {
                SceneManager.LoadScene("GameWon");
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                // il manque des objets
                // reset de la liste des noms d'objets à ramasser
                ResetCheckList();
            }
        }
    }
}
