using System.Collections.Generic;
using UnityEngine;

public class GM_GestionDestinations : MonoBehaviour
{
    [SerializeField] List<GameObject> destinations = new List<GameObject>();

    // Singleton
    public static GM_GestionDestinations gestionDestinationsInstance;

    private void Awake()
    {
        // Création du singleton
        if (gestionDestinationsInstance == null)
            gestionDestinationsInstance = this;
    }

    // Fonction pour avoir une destination
    public GameObject GetDestination()
    {
        return destinations[Random.Range(0, destinations.Count)];
    }
}
