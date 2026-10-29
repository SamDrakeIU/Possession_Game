using UnityEngine;

using UnityEngine.SceneManagement;


public class PauseManager : MonoBehaviour

{

    public GameObject pauseMenuUI;

    private bool isPaused = false;


    void Update()

    {

        if (Input.GetKeyDown(KeyCode.M))

        {

            if (isPaused)

                Resume();

            else

                Pause();

        }

    }


    public void Resume()

    {

        pauseMenuUI.SetActive(false);

        Time.timeScale = 1f;

        isPaused = false;

    }


    public void Pause()

    {

        pauseMenuUI.SetActive(true);

        Time.timeScale = 0f;

        isPaused = true;

    }


    public void RestartGame()

    {

        Time.timeScale = 1f;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }


    public void ExitGame()

    {

        Time.timeScale = 1f;

        Application.Quit();

        Debug.Log("Exit requested (won’t quit in editor)");

    }

}