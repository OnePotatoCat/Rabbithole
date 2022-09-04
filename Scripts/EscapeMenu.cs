using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class EscapeMenu : MonoBehaviour
{
    [SerializeField] CanvasGroup escapeMenu;
    [SerializeField] CanvasGroup fadeCanvas;
    [SerializeField] public Player player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            player.EscapeAttempt();
        }
    }
    public void Escape()
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(escapeMenu.DOFade(0f, 1f));
        seq.Join(fadeCanvas.DOFade(1f, 3f));
        escapeMenu.gameObject.SetActive(false);
        Time.timeScale = 1f;
        SceneManager.LoadScene(2);
    }

    public void EscapeLater()
    {
        Time.timeScale = 1f;
        escapeMenu.gameObject.SetActive(false);
    }
}
