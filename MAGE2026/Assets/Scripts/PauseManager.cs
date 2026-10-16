using UnityEngine;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
using System.Collections;

public class PauseManager : MonoBehaviour
{
        public static bool GameisPaused = false;
        public GameObject pauseMenuUI;
        private bool isTransitioning;

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
            if (isTransitioning)
            return;

            isTransitioning = true;
            Time.timeScale = 1f;
            StartCoroutine(MainLevel());
        }

        IEnumerator MainLevel()
        {
            yield return StartCoroutine(Transition.Instance.PlayTransition());

            SceneManager.LoadScene("MainMenu");

            yield return StartCoroutine(Transition.Instance.EndTransition());
        }
}