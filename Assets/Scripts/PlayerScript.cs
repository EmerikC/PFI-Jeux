using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] float vitesse = 50;
    [SerializeField] float vitesseCamera = 1;
    [SerializeField] float gravite;
    CharacterController charController;
    Camera cameraFPS;
    Vector3 rotationCamera = new Vector3(0, 0, 0);
    public bool canSprint = true; // Si le joueur peut sprinter ou non (ex: si il est en train de ramasser un objet, il ne peut pas sprinter)

    // Valeur Inputs
    Vector2 move = Vector2.zero;
    float sprint = 0;
    Vector2 look = Vector2.zero;

    //UI Pause
    PauseMenu menuPause;
    bool doOnce = true;

    //Sons de pas
    AudioSource audioSource;
    [SerializeField] float movementThreshold = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        charController = GetComponent<CharacterController>();
        cameraFPS = GetComponentInChildren<Camera>();
        Cursor.lockState = CursorLockMode.Locked;
        menuPause = GetComponentInChildren<PauseMenu>();
        audioSource = GetComponent<AudioSource>();
    }


    // Update is called once per frame
    void Update()
    {
        Deplacement();
        RotationCamera();
        GestionPause();
    }

    void Deplacement()
    {
        // Calculer le Deplacement normal
        Vector3 direction = cameraFPS.transform.forward * move.y + cameraFPS.transform.right * move.x;  // N'est pas normalized, donc si nous allons en diagonal, les vitesses s'additionne (comme DOOM, GoldenEye, etc)
        if (direction.magnitude > 0)
        {
            direction = new Vector3(direction.x, 0, direction.z);
            direction = direction.normalized;
            // Sprint
            direction = (sprint * 0.5f + 1) * direction; // Si le Sprint est appuy?, nous allons ? 150% de la vitesse 
        }

        // Déplace le joueur selon tout les forces et Inputs
        if (Time.timeScale != 0)
            charController.Move(direction * vitesse * Time.deltaTime);
            charController.Move(gravite * Time.deltaTime * Vector3.down); // Applique la gravité au joueur
    }

    void RotationCamera()
    {
        // Ici nous ne voulons pas Normalized. Si le joueur veux tourner sa camera en diagonal, il fait sense que les vitesse s'additionne
        rotationCamera += new Vector3(-look.y * Time.deltaTime * vitesseCamera,
            look.x * Time.deltaTime * vitesseCamera, 0);

        // Nous limitons le pitch de la camera pour que le joueur ne puisse pas se retrouver avec la tête vers le bas
        rotationCamera.x = Mathf.Clamp(rotationCamera.x, -70, 70);

        // Nous changons la rotation de la camera pour notre valeur
        cameraFPS.transform.rotation = Quaternion.Euler(rotationCamera.x, rotationCamera.y, 0);

    }

    void GestionPause()
    {
        if (Input.GetAxis("Pause") > 0 && doOnce || Input.GetButtonDown("Pause"))
        {
            doOnce = false;
            menuPause.Pause();
        }
        else if (Input.GetAxis("Pause") <= 0)
            doOnce = true;
    }

    // Inputs
    public void InputMove(InputAction.CallbackContext movement)
    {
        //if game is on pause return
        if (Time.timeScale == 0)
        {
            audioSource.Stop();
            return;
        }

        move = movement.ReadValue<Vector2>();

        // Si le joueur bouge, on joue le son de pas
        if (move.magnitude > movementThreshold && !audioSource.isPlaying)
        {
            audioSource.Play();
        }
        else if (move.magnitude <= movementThreshold)
        {
            audioSource.Stop();
        }
    }

    public void InputLook(InputAction.CallbackContext movement)
    {
        look = movement.ReadValue<Vector2>();
    }

    public void InputSprint(InputAction.CallbackContext movement)
    {
        if (canSprint)
        {
            sprint = movement.ReadValue<float>();
        }
        else
        {
            sprint = 0;
        }
    }
}
