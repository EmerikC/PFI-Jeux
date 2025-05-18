using TMPro;
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

    // Valeur Inputs
    Vector2 move = Vector2.zero;
    float sprint = 0;
    Vector2 look = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
        charController = GetComponent<CharacterController>();
        cameraFPS = GetComponentInChildren<Camera>();
        Cursor.lockState = CursorLockMode.Locked;
    }


    // Update is called once per frame
    void Update()
    {
        Deplacement();
        RotationCamera();
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
            charController.Move((direction * vitesse) * Time.deltaTime);
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

    // Inputs
    public void InputMove(InputAction.CallbackContext movement)
    {
        move = movement.ReadValue<Vector2>();
    }

    public void InputLook(InputAction.CallbackContext movement)
    {
        look = movement.ReadValue<Vector2>();
    }

    public void InputSprint(InputAction.CallbackContext movement)
    {
        sprint = movement.ReadValue<float>();
    }

    public void InputPause(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            //menuPause.Pause();
        }
    }
}
