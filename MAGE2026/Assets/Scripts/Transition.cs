using UnityEngine;
using System.Collections;

public class Transition : MonoBehaviour
{
    public static Transition Instance;
    public Animator animator;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        Instance = this;
    }

    public IEnumerator PlayTransition()
    {
        animator.SetTrigger("Start");

        yield return new WaitForSeconds(1f);
    }

    public IEnumerator EndTransition()
    {
        animator.SetTrigger("End");

        yield return new WaitForSeconds(1f);
    }
}