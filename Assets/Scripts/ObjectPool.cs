using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    private List<GameObject> pool = new List<GameObject>();

    [SerializeField] GameObject[] ObjectsToPool;
    [SerializeField] float[] quantiteParObjet;

    public static ObjectPool ObjectPoolinstance;

    private void Awake()
    {
        // Nous assignons notre variable static qui pointera vers notre ObjectPool
        if (ObjectPoolinstance == null)
            ObjectPoolinstance = this;
    }

    void Start()
    {
        // Dans ce cas, nous avons un Object Pool pouvant contenir plusieurs types d'objets différents. 
        // Ici nous vérifions si les variables déclarer sont du bon nombre (exp je déclare 2 types d'objets mais je ne donne qu'une quantité)
        for (int i = 0; i < Mathf.Min(ObjectsToPool.Length, quantiteParObjet.Length); i++)
        {
            // Nous instancions nos objets en prenant soin de changer leurs noms pour allez les chercher plus tard et en les déactivant. 
            // Changer les noms n'est pas nécessaire dans un Object Pool contenant seulement un type d'objet
            for (int o = 0; o < quantiteParObjet[i]; o++)
            {
                GameObject obj = Instantiate(ObjectsToPool[i]);
                obj.name = ObjectsToPool[i].name;
                obj.SetActive(false);
                pool.Add(obj);
            }
        }
    }

    // Cette méthode nous permet d'aller chercher un objet dans notre Pool qui est présentement disponible (donc n'est pas actif)
    public GameObject GetPooledObject(GameObject typeObjet)
    {
        for (int i = 0; i < pool.Count; i++)
        {
            if (!pool[i].activeInHierarchy && pool[i].name == typeObjet.name)
            {
                return pool[i];
            }
        }
        return null; // Si aucun objet n'est disponible, retourne Null pour éviter une erreur, mais ce cas ne devrait jamais arriver si vous calculez bien votre Pool Size
    }
}
