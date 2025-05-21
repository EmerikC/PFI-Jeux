using UnityEngine;
using System.Collections;

public class AIDialog : MonoBehaviour
{
    public bool isTalking = false;
    [SerializeField] GameObject[] AI;
    OutlineScript outlineScript;


    void Start()
    {
        outlineScript = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<OutlineScript>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && !isTalking)
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.TransformDirection(Vector3.forward), out hit))
            {
                foreach (GameObject ai in AI)
                {
                    if (hit.transform.gameObject == ai)
                    {
                        isTalking = true;
                        outlineScript.OutliningEnabled = false;
                        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>().canMove = false;
                        ai.GetComponent<AIScript>().agent.enabled = false;

                        Vector3 direction = Camera.main.transform.position - ai.transform.position;
                        direction.y = 0;
                        if (direction.sqrMagnitude > 0.001f)
                        {
                            ai.transform.rotation = Quaternion.LookRotation(direction);
                        }

                        Transform headTransform = ai.transform.Find("head");
                        if (headTransform != null)
                        {
                            Camera.main.transform.LookAt(headTransform.position);
                        }

                        StartCoroutine(PlayAudio(ai));

                        break;
                    }
                }
            }
        }
    }

    private IEnumerator PlayAudio(GameObject ai)
    {
        AudioSource audioSource = ai.GetComponent<AudioSource>();
        if (audioSource != null)
        {
            audioSource.Play();
            yield return new WaitWhile(() => audioSource.isPlaying);
        }
        isTalking = false;
        outlineScript.OutliningEnabled = true;
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>().canMove = true;
        ai.GetComponent<AIScript>().agent.enabled = true;
    }
}
