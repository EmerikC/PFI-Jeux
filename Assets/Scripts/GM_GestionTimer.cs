using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GM_GestionTimer : MonoBehaviour
{
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
}
