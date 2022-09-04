using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;


public class Walking : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] GameObject hole;

    public void StartGame()
    {
        StartCoroutine(WaitThenHole());
        animator.SetBool("start", true);
        StartCoroutine(WaitThenLoad());
    }

    IEnumerator WaitThenHole()
    {
        yield return new WaitForSeconds(0.7f);
        hole.SetActive(true);
    }

    IEnumerator WaitThenLoad()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(1);
    }

    public void Awake()
    {
        gameObject.transform.DOMove(new Vector3(4.55f, -3.6f, 1f), 150f);
    }
}
