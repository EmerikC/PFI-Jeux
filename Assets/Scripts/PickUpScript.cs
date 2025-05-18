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
        //Récupération du layer d'objet ramassé
        LayerNumber = LayerMask.NameToLayer("holdLayer");
        //Récupération du script PlayerScript
        playerScript = player.GetComponent<PlayerScript>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (heldObj == null)
            {
                //Lance une raycast pour détecter les objets ramassables
                RaycastHit hit;
                if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, pickUpRange))
                {
                    //Si l'objet touché a le tag "CanPickUp", on le ramasse
                    if (hit.transform.gameObject.tag == "CanPickUp")
                    {
                        PickUpObject(hit.transform.gameObject);
                    }
                }
            }
            else
            {
                //Si l'objet est ramassé, on le lâche
                if (canDrop == true)
                {
                    StopClipping();
                    DropObject();
                }
            }
        }

        //Si l'objet est ramassé, on le déplace
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
        //vérifie si l'objet a un Rigidbody
        if (pickUpObj.GetComponent<Rigidbody>())
        {
            //initialise l'objet ramassé
            heldObj = pickUpObj;
            heldObjRb = pickUpObj.GetComponent<Rigidbody>();

            //isKinematic = true pour que l'objet puisse passer à travers les autres objets
            heldObjRb.isKinematic = true;

            //change la position de l'objet ramassé pour qu'il soit à la position de ramassage
            heldObjRb.transform.parent = pickUpPosition.transform;

            //change le layer de l'objet ramassé pour éviter le clipping
            foreach (Transform child in heldObj.transform)
            {
                child.gameObject.layer = LayerNumber;
            }

            //change le booléen sprint du joueur
            playerScript.canSprint = false;

            //Désactive les collisions entre l'objet ramassé et le joueur
            Physics.IgnoreCollision(heldObj.GetComponent<Collider>(), player.GetComponent<Collider>(), true);
        }
    }
    void DropObject()
    {
        //Réactive les collisions entre l'objet ramassé et le joueur
        Physics.IgnoreCollision(heldObj.GetComponent<Collider>(), player.GetComponent<Collider>(), false);

        //réinitialise l'objet ramassé et le script
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

    //Fonction pour déplacer l'objet ramassé
    void MoveObject()
    {
        //garde l'objet ramassé à la position de ramassage
        heldObj.transform.position = pickUpPosition.transform.position;
    }

    //Fonction pour lancer l'objet ramassé
    void ThrowObject()
    {
        //Même principe que pour DropObject, mais on ajoute une force à l'objet ramassé
        Physics.IgnoreCollision(heldObj.GetComponent<Collider>(), player.GetComponent<Collider>(), false);
        foreach (Transform child in heldObj.transform)
        {
            child.gameObject.layer = 0;
        }
        heldObjRb.isKinematic = false;
        heldObj.transform.parent = null;

        //On ajoute une force à l'objet ramassé
        heldObjRb.AddForce(transform.forward * throwForce);
        heldObj = null;
        playerScript.canSprint = true;
    }

    //Fonction pour éviter le clipping entre l'objet ramassé et le monde lors du jetage
    void StopClipping()
    {
        //calcule la distance entre l'objet ramassé et le joueur
        var clipRange = Vector3.Distance(heldObj.transform.position, transform.position);

        //Vérifie si il y a d'autres objets entre le joueur et l'objet ramassé
        //RayCastAll est utilisé puisque l'objet ramassé bloque le raycast
        RaycastHit[] hits;
        hits = Physics.RaycastAll(transform.position, transform.TransformDirection(Vector3.forward), clipRange);

        //Si il y a plus d'un objet entre le joueur et l'objet ramassé, ça signifie qu'il y a un objet entre le joueur et l'objet ramassé
        if (hits.Length > 1)
        {
            //Change la position de l'objet ramassé pour qu'il soit à la position du joueur
            heldObj.transform.position = transform.position + new Vector3(0f, -0.5f, 0f); //offset pour éviter que l'objet soit jeté par dessus du joueur
        }
    }
}
