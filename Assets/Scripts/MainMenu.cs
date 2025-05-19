using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    //Fonction qui permet de lancer le jeu
    public void StartGame()
    {
        SceneManager.LoadScene("supermarket");
    }

    //Fonction qui permet de quitter le jeu
    public void ExitGame()
    {
        Application.Quit();
    }
    //Fonction qui permet de retourner au menu principal
    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}