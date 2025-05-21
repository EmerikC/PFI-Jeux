using NUnit.Framework;
using UnityEngine;

public class PickUpCart : MonoBehaviour
{
    public bool isHolding = false;
    GameObject cart;
    GameObject handle;
    OutlineScript outlineScript;
    [SerializeField] GameObject listeUI;


    void Start()
    {
        cart = GameObject.FindGameObjectWithTag("playerCart");
        handle = GameObject.Find("grab_handle");
        outlineScript = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<OutlineScript>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!isHolding)
            {
                //Si le hit est sur le panier, on le prend
                RaycastHit hit;
                if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.TransformDirection(Vector3.forward), out hit))
                {
                    if (hit.transform.gameObject == handle)
                    {
                        TogglePickUp();
                    }
                }
            }
            else
            {
                //si le joueur a le panier, on le repose
                TogglePickUp();
            }
        }

        if (isHolding)
        {
            //On deplace le panier avec le joueur
            MoveCart();
        }
    }

    //Fonctions pour gerer le mouvement du panier
    public void MoveCart()
    {
        //positionne le panier devant le joueur
        Vector3 newPos = Camera.main.transform.position + Camera.main.transform.forward * 0.5f;
        newPos.y = cart.transform.position.y;
        cart.transform.position = newPos;

        //rotation selon la camera
        float camY = Camera.main.transform.eulerAngles.y;
        Quaternion cameraYRotation = Quaternion.Euler(0f, camY, 0f);
        cart.transform.rotation = cameraYRotation;
        cart.transform.Rotate(0f, 90f, 0f); //reajuste la position du panier (autrement, le panier s'affiche a l'horizontal)
    }
    public void TogglePickUp()
    {
        //On change l'etat de isHolding
        isHolding = !isHolding;
        outlineScript.OutliningEnabled = !outlineScript.OutliningEnabled;
        listeUI.SetActive(isHolding); //on affiche ou desaffiche l'UI de la liste
    }
}
