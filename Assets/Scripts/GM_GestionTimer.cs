using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class GM_GestionTimer : MonoBehaviour
{
<<<<<<< Updated upstream
    [SerializeField] float timer = 60f * 3f; //3 minutes
    [SerializeField] GameObject timerUI; // fait référence à l'UI du timer

    void Update()
    {
        // on reduit le timer 
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            //quand le timer est a zero, game over

            timer = 0; //evite les valeurs negatives
            SceneManager.LoadScene("GameLost");
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
=======
    [SerializeField] float timer = 60f * 3f;
    [SerializeField] GameObject timerUI;
    [SerializeField] PlayerScript player; // Reference to the player script

    AudioSource audioSource;
    bool hasEnded = false;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (hasEnded)
            return;

        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            timer = 0;
            hasEnded = true;
            StartCoroutine(EndGameSequence());
>>>>>>> Stashed changes
        }
        updateTimerUI(); //MAJ de l'UI
    }

    private void updateTimerUI()
    {
<<<<<<< Updated upstream
        //afichage des minutes et secondes
=======
>>>>>>> Stashed changes
        int minutes = (int)(timer / 60);
        int seconds = (int)(timer % 60);

        string formattedTime = string.Format("{0}:{1:00}", minutes, seconds);
        timerUI.GetComponent<TextMeshProUGUI>().text = formattedTime;
    }

    private IEnumerator EndGameSequence()
    {
        // Block player controls
        if (player != null)
            player.canMove = false;

        // Play audio if available
        if (audioSource != null && audioSource.clip != null)
        {
            transform.position = player.transform.position;
            audioSource.Play();
            yield return new WaitWhile(() => audioSource.isPlaying);
        }

        // Unlock and show cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Load the GameLost scene
        SceneManager.LoadScene("GameLost");
    }
}
