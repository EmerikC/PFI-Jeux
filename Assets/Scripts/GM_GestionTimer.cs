using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class GM_GestionTimer : MonoBehaviour
{
    [SerializeField] float timer = 60f * 3f; //3 minutes
    [SerializeField] GameObject timerUI; // fait référence à l'UI du timer
    [SerializeField] PlayerScript player; // Référence au script du joueur

    AudioSource audioSource;
    bool hasEnded = false;

    private void Start()
    {
        // Récupération de la référence à l'AudioSource
        audioSource = GetComponent<AudioSource>();
    }
    void Update()
    {
        if (hasEnded)
            return;

        // on reduit le timer 
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            //quand le timer est a zero, game over
            timer = 0;
            hasEnded = true;
            StartCoroutine(EndGameSequence());
        }
        updateTimerUI(); //MAJ de l'UI
    }

    private void updateTimerUI()
    {
        //afichage des minutes et secondes
        int minutes = (int)(timer / 60);
        int seconds = (int)(timer % 60);

        string formattedTime = string.Format("{0}:{1:00}", minutes, seconds);
        timerUI.GetComponent<TextMeshProUGUI>().text = formattedTime;
    }

    // Coroutine pour gérer la fin du jeu
    private IEnumerator EndGameSequence()
    {
        // On bloque le mouvement du joueur
        if (player != null)
            player.canMove = false;

        // On joue le son de fin de jeu
        if (audioSource != null && audioSource.clip != null)
        {
            transform.position = player.transform.position;
            audioSource.Play();
            yield return new WaitWhile(() => audioSource.isPlaying);
        }

        // On affiche le curseur
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Changement de la scène
        SceneManager.LoadScene("GameLost");
    }
}
