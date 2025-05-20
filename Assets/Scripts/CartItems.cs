using NUnit.Framework;
using UnityEngine;

public class CartItems : MonoBehaviour
{
    public GameObject[] itemsInCart = new GameObject[10];
    GM_GestionListe gestionListe;

    private void Start()
    {
        gestionListe = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GM_GestionListe>();
    }

    private void OnTriggerEnter(Collider other)
    {
        for (int i = 0; i < itemsInCart.Length; i++)
        {
            if (itemsInCart[i] == null)
            {
                itemsInCart[i] = other.gameObject;
                other.gameObject.transform.SetParent(GameObject.FindGameObjectWithTag("itemsInCart").transform);
                gestionListe.OnEnterList(other.gameObject);
                break;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        int nbItemsInCart = 0;
        for (int i = 0; i < itemsInCart.Length; i++)
        {
            if(itemsInCart[i] != null)
            {
                if(itemsInCart[i].name == other.gameObject.name)
                {
                    nbItemsInCart++;
                }

                if (itemsInCart[i] == other.gameObject)
                {
                    other.gameObject.transform.SetParent(null);
                    itemsInCart[i] = null;
                }
            }
        }

        if(nbItemsInCart == 1)
        {
            gestionListe.OnExitList(other.gameObject);
        }
    }

}
