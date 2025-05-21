using UnityEngine;
using System.Collections;

public class AIDialog : MonoBehaviour
{
    public bool isTalking = false;
    [SerializeField] GameObject[] AI;
    OutlineScript outlineScript;


    void Start()
    {
        // Récupération du outlineScript
        outlineScript = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<OutlineScript>();
    }

    void Update()
    {
        // Si le joueur appuie sur E et qu'il n'est pas en train de parler
        if (Input.GetKeyDown(KeyCode.E) && !isTalking)
        {
            // Raycast pour détecter les objets AI
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.TransformDirection(Vector3.forward), out hit))
            {
                // Vérification si l'objet touché est un AI
                foreach (GameObject ai in AI)
                {
                    if (hit.transform.gameObject == ai)
                    {
                        // Si l'AI est touché, on joue le son
                        isTalking = true;

                        // Désactivation de l'outline
                        outlineScript.OutliningEnabled = false;

                        // Désactivation du mouvement du joueur et de l'agent AI
                        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>().canMove = false;
                        ai.GetComponent<AIScript>().agent.enabled = false;

                        // Rotation de l'AI vers la caméra
                        Vector3 direction = Camera.main.transform.position - ai.transform.position;
                        direction.y = 0;
                        if (direction.sqrMagnitude > 0.001f)
                        {
                            ai.transform.rotation = Quaternion.LookRotation(direction);
                        }

                        // Positionnement de la caméra sur la tête de l'AI
                        Transform headTransform = ai.transform.Find("head");
                        if (headTransform != null)
                        {
                            Camera.main.transform.LookAt(headTransform.position);
                        }

                        // Lancement de la coroutine pour jouer l'audio
                        StartCoroutine(PlayAudio(ai));

                        break;
                    }
                }
            }
        }
    }

    // Coroutine pour jouer l'audio
    private IEnumerator PlayAudio(GameObject ai)
    {
        // Récupération de l'AudioSource de l'AI
        AudioSource audioSource = ai.GetComponent<AudioSource>();

        // On attend que l'AI ait fini de parler
        if (audioSource != null)
        {
            audioSource.Play();
            yield return new WaitWhile(() => audioSource.isPlaying);
        }

        // Reset des variables
        isTalking = false;
        outlineScript.OutliningEnabled = true;
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>().canMove = true;
        ai.GetComponent<AIScript>().agent.enabled = true;
    }
}
