using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GM_GestionTimer : MonoBehaviour
{
    [SerializeField] float timer = 60f * 3f;
    [SerializeField] GameObject timerUI;

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            timer = 0;
            // Call the function to end the game
            SceneManager.LoadScene("GameLost");
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        updateTimerUI();
    }

    private void updateTimerUI()
    {
        // Update the UI element with the remaining time
        // Assuming timerUI has a Text component to display the time
        int minutes = (int)(timer / 60);
        int seconds = (int)(timer % 60);

        string formattedTime = string.Format("{0}:{1:00}", minutes, seconds);
        timerUI.GetComponent<TextMeshProUGUI>().text = formattedTime;
    }
}
