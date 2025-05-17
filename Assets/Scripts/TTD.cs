using UnityEngine;

public class TTD : MonoBehaviour
{
    [SerializeField] float tempsDeVie = 3;
    float tempsInitial;

    private void OnEnable()
    {
        tempsInitial = Time.time;
    }

    // Simple syst�me d�sactivant un objet apr�s un certain temps. Remplace TTL dans un ObjectPool
    void Update()
    {
        if (Time.time > tempsInitial + tempsDeVie)
        {
            gameObject.SetActive(false);
        }
    }
}
