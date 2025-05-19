using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    Canvas canvas;
    void Start()
    {
        // Ici nous allons chercher la référence à notre bouton pour ajouter un OnClickListener
        Button btn = GetComponentInChildren<Button>();
        btn.onClick.AddListener(Resume);
        canvas = GetComponent<Canvas>();
    }

    public void Pause()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Time.timeScale = 0.0f;
        canvas.enabled = true;
    }

    void Resume()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1.0f;
        canvas.enabled = false;
    }
}
