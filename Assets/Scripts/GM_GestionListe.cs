using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GM_GestionListe : MonoBehaviour
{
    [SerializeField] GameObject[] prefabsChoices; //choix que l'on met dans la liste d'epicerie
    public GameObject[] listeObjetsText; //UI de la liste d'epicerie (text)
    void Start()
    {
        AssignObjects();
    }

    private void AssignObjects()
    {
        //shuffle le tableau de prefabs, pour une liste melangee
        ShuffleUtility.Shuffle(prefabsChoices);

        //mettre les noms des items dans la liste d'epicerie
        for (int i = 0; i < listeObjetsText.Length; i++)
        {
            listeObjetsText[i].GetComponent<TextMeshProUGUI>().text = prefabsChoices[i].name;
        }
    }

    public void OnEnterList(GameObject item) 
    {
        //Quand un objet rentre dans le panier (grace au box collider trigger), on "strike" la ligne de texte
        for (int i = 0; i < listeObjetsText.Length; i++)
        {
            if (listeObjetsText[i].GetComponent<TextMeshProUGUI>().text == item.name)
            {
                listeObjetsText[i].GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Strikethrough;
            }
        }
    }

    public void OnExitList(GameObject item)
    {
        //Quand un objet sort du panier, on "un-strike" la ligne de texte
        for (int i = 0; i < listeObjetsText.Length; i++)
        {
            if (listeObjetsText[i].GetComponent<TextMeshProUGUI>().text == item.name)
            {
                listeObjetsText[i].GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Normal;
            }
        }
    }
}

public static class ShuffleUtility //shuffle un tableau
{
    public static void Shuffle<T>(this T[] array)
    {
        for (int i = 0; i < array.Length; i++)
        {
            int randomIndex = UnityEngine.Random.Range(i, array.Length);
            T temp = array[i];
            array[i] = array[randomIndex];
            array[randomIndex] = temp;
        }
    }
}
