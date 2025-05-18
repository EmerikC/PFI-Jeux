using System;
using UnityEngine;

public class PickUpScript : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] GameObject pickUpPosition;
    [SerializeField] float throwForce = 500f;
    [SerializeField] float pickUpRange = 5f;

    private GameObject heldObj;
    private Rigidbody heldObjRb;
    private bool canDrop = true;
    private int LayerNumber;
    PlayerScript playerScript;

    void Start()
    {
        //R�cup�ration du layer d'objet ramass�
        LayerNumber = LayerMask.NameToLayer("holdLayer");
        //R�cup�ration du script PlayerScript
        playerScript = player.GetComponent<PlayerScript>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (heldObj == null)
            {
                //Lance une raycast pour d�tecter les objets ramassables
                RaycastHit hit;
                if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, pickUpRange))
                {
                    //Si l'objet touch� a le tag "CanPickUp", on le ramasse
                    if (hit.transform.gameObject.tag == "CanPickUp")
                    {
                        PickUpObject(hit.transform.gameObject);
                    }
                }
            }
            else
            {
                //Si l'objet est ramass�, on le l�che
                if (canDrop == true)
                {
                    StopClipping();
                    DropObject();
                }
            }
        }

        //Si l'objet est ramass�, on le d�place
        if (heldObj != null)
        {
            MoveObject();

            //Si le joueur appuie sur le bouton de la souris gauche, on lance l'objet
            if (Input.GetKeyDown(KeyCode.Mouse0) && canDrop == true)
            {
                StopClipping();
                ThrowObject();
            }

        }
    }

    //Fonction pour ramasser l'objet
    void PickUpObject(GameObject pickUpObj)
    {
        //v�rifie si l'objet a un Rigidbody
        if (pickUpObj.GetComponent<Rigidbody>())
        {
            //initialise l'objet ramass�
            heldObj = pickUpObj;
            heldObjRb = pickUpObj.GetComponent<Rigidbody>();

            //isKinematic = true pour que l'objet puisse passer � travers les autres objets
            heldObjRb.isKinematic = true;

            //change la position de l'objet ramass� pour qu'il soit � la position de ramassage
            heldObjRb.transform.parent = pickUpPosition.transform;

            //change le layer de l'objet ramass� pour �viter le clipping
            foreach (Transform child in heldObj.transform)
            {
                child.gameObject.layer = LayerNumber;
            }

            //change le bool�en sprint du joueur
            playerScript.canSprint = false;

            //D�sactive les collisions entre l'objet ramass� et le joueur
            Physics.IgnoreCollision(heldObj.GetComponent<Collider>(), player.GetComponent<Collider>(), true);
        }
    }
    void DropObject()
    {
        //R�active les collisions entre l'objet ramass� et le joueur
        Physics.IgnoreCollision(heldObj.GetComponent<Collider>(), player.GetComponent<Collider>(), false);

        //r�initialise l'objet ramass� et le script
        heldObj.layer = 0;
        foreach (Transform child in heldObj.transform)
        {
            child.gameObject.layer = 0;
        }
        heldObjRb.isKinematic = false;
        heldObj.transform.parent = null;
        heldObj = null;
        playerScript.canSprint = true;
    }

    //Fonction pour d�placer l'objet ramass�
    void MoveObject()
    {
        //garde l'objet ramass� � la position de ramassage
        heldObj.transform.position = pickUpPosition.transform.position;
    }

    //Fonction pour lancer l'objet ramass�
    void ThrowObject()
    {
        //M�me principe que pour DropObject, mais on ajoute une force � l'objet ramass�
        Physics.IgnoreCollision(heldObj.GetComponent<Collider>(), player.GetComponent<Collider>(), false);
        foreach (Transform child in heldObj.transform)
        {
            child.gameObject.layer = 0;
        }
        heldObjRb.isKinematic = false;
        heldObj.transform.parent = null;

        //On ajoute une force � l'objet ramass�
        heldObjRb.AddForce(transform.forward * throwForce);
        heldObj = null;
        playerScript.canSprint = true;
    }

    //Fonction pour �viter le clipping entre l'objet ramass� et le monde lors du jetage
    void StopClipping()
    {
        //calcule la distance entre l'objet ramass� et le joueur
        var clipRange = Vector3.Distance(heldObj.transform.position, transform.position);

        //V�rifie si il y a d'autres objets entre le joueur et l'objet ramass�
        //RayCastAll est utilis� puisque l'objet ramass� bloque le raycast
        RaycastHit[] hits;
        hits = Physics.RaycastAll(transform.position, transform.TransformDirection(Vector3.forward), clipRange);

        //Si il y a plus d'un objet entre le joueur et l'objet ramass�, �a signifie qu'il y a un objet entre le joueur et l'objet ramass�
        if (hits.Length > 1)
        {
            //Change la position de l'objet ramass� pour qu'il soit � la position du joueur
            heldObj.transform.position = transform.position + new Vector3(0f, -0.5f, 0f); //offset pour �viter que l'objet soit jet� par dessus du joueur
        }
    }
}
