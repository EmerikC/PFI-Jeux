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
        gm = GM_GestionDestinations.gestionDestinationsInstance;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

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
        if (animator != null)
        {
            animator.SetFloat("Speed", agent.velocity.magnitude);
        }

        if (rb.linearVelocity.y <= 0 && Physics.Raycast(transform.position, Vector3.down, 1f))
            agent.enabled = true;

        if (agent.isOnNavMesh && !isWaiting)
        {
            agent.destination = destination.transform.position;

            // Check if agent has reached destination
            if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance && !agent.hasPath)
            {
                StartCoroutine(WaitAndGetNewDestination());
            }
        }
    }

    IEnumerator WaitAndGetNewDestination()
    {
        isWaiting = true;
        yield return new WaitForSeconds(2);

        // Get a new destination from GM_GestionDestinations
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
