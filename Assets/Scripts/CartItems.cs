using NUnit.Framework;
using UnityEngine;

public class CartItems : MonoBehaviour
{
    public GameObject[] itemsInCart = new GameObject[10];

    private void OnTriggerEnter(Collider other)
    {
        for (int i = 0; i < itemsInCart.Length; i++)
        {
            if (itemsInCart[i] == null)
            {
                itemsInCart[i] = other.gameObject;
                other.gameObject.transform.SetParent(GameObject.FindGameObjectWithTag("itemsInCart").transform);
                break;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        for (int i = 0; i < itemsInCart.Length; i++)
        {
            if (itemsInCart[i] == other.gameObject)
            {
                other.gameObject.transform.SetParent(null);
                itemsInCart[i] = null;
                break;
            }
        }
    }

}
