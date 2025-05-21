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
        //Quand un item rentre dans le panier (box collider trigger), on le met dans le tableau itemsInCart
        for (int i = 0; i < itemsInCart.Length; i++)
        {
            if (itemsInCart[i] == null)
            {
                itemsInCart[i] = other.gameObject;
                other.gameObject.transform.SetParent(GameObject.FindGameObjectWithTag("itemsInCart").transform); //on met ensuite l'objet comme enfant du panier (pour eviter les bugs de collisions)
                gestionListe.OnEnterList(other.gameObject); //on met l'UI a jour
                break;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //Quand un item sort du panier, on le retire du tableau itemsInCart
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
