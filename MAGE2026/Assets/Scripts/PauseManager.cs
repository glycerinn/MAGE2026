using UnityEngine;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
        public static bool GameisPaused = false;
        public GameObject pauseMenuUI;

        void Start()
        {
            pauseMenuUI.SetActive(false);
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (GameisPaused)
                {
                    Resume();
                }
                else
                {
                    Pause();
                }
            }
        }

        public void Resume()
        {
            pauseMenuUI.SetActive(false);
            Time.timeScale = 1f;
            GameisPaused = false;
        }

        public void Pause()
        {
            pauseMenuUI.SetActive(true);
            Time.timeScale = 0f;
            GameisPaused = true;
        }

        public void LoadMenu()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene("MainMenu");
        }
}