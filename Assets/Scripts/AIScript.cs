using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class AIScript : MonoBehaviour
{
    GameObject destination;
    Animator animator;
    Rigidbody rb;
    public NavMeshAgent agent;
    bool isWaiting = false;
    GM_GestionDestinations gm;

    void Start()
    {
        // On récupère le GM_GestionDestinations et les composants nécessaires
        gm = GM_GestionDestinations.gestionDestinationsInstance;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

        // On trouve la première destination
        if (gm != null)
        {
            destination = gm.GetDestination();
            if (destination != null)
            {
                agent.destination = destination.transform.position;
            }
        }
    }

    void Update()
    {
        // Vitesse pour l'animation
        if (animator != null)
        {
            animator.SetFloat("Speed", agent.velocity.magnitude);
        }

        // Si l'agent est sur le NavMesh et qu'il ne tombe pas, on le réactive
        if (rb.linearVelocity.y <= 0 && Physics.Raycast(transform.position, Vector3.down, 1f))
            agent.enabled = true;

        // Si l'agent est sur le NavMesh et qu'il n'est pas en attente, on met à jour la destination
        if (agent.isOnNavMesh && !isWaiting)
        {
            agent.destination = destination.transform.position;

            // Lorsque l'agent atteint sa destination, on attend 2 secondes avant de choisir une nouvelle destination
            if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance && !agent.hasPath)
            {
                StartCoroutine(WaitAndGetNewDestination());
            }
        }
    }

    // Coroutine pour attendre avant de choisir une nouvelle destination
    IEnumerator WaitAndGetNewDestination()
    {
        // On attend 2 secondes avant de choisir une nouvelle destination
        isWaiting = true;
        yield return new WaitForSeconds(2);

        // On choisit une nouvelle destination
        GM_GestionDestinations gm = GM_GestionDestinations.gestionDestinationsInstance;
        if (gm != null)
        {
            GameObject newDest = gm.GetDestination();
            if (newDest != null)
            {
                destination = newDest;
                agent.destination = destination.transform.position;
            }
        }
        isWaiting = false;
    }
}
