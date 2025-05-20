using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GM_GestionListe : MonoBehaviour
{
    [SerializeField] GameObject[] prefabsChoices;
    public GameObject[] listeObjetsText;
    void Start()
    {
        AssignObjects();
    }

    private void AssignObjects()
    {
        //shuffle le tableau de prefabs
        ShuffleUtility.Shuffle(prefabsChoices);

        //chercher la longueur de la liste
        for (int i = 0; i < listeObjetsText.Length; i++)
        {
            Console.WriteLine(prefabsChoices[i].name);
            listeObjetsText[i].GetComponent<TextMeshProUGUI>().text = prefabsChoices[i].name;
        }
    }

    public void OnEnterList(GameObject item) //scribble
    {
        for (int i = 0; i < listeObjetsText.Length; i++)
        {
            if (listeObjetsText[i].GetComponent<TextMeshProUGUI>().text == item.name)
            {
                listeObjetsText[i].GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Strikethrough;
            }
        }
    }

    public void OnExitList(GameObject item) //remove scribble
    {
        for (int i = 0; i < listeObjetsText.Length; i++)
        {
            if (listeObjetsText[i].GetComponent<TextMeshProUGUI>().text == item.name)
            {
                listeObjetsText[i].GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Normal;
            }
        }
    }
}

public static class ShuffleUtility
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
