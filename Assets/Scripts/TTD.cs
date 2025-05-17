using UnityEngine;

public class TTD : MonoBehaviour
{
    [SerializeField] float tempsDeVie = 3;
    float tempsInitial;

    private void OnEnable()
    {
        tempsInitial = Time.time;
    }

    // Simple système désactivant un objet après un certain temps. Remplace TTL dans un ObjectPool
    void Update()
    {
        if (Time.time > tempsInitial + tempsDeVie)
        {
            gameObject.SetActive(false);
        }
    }
}
