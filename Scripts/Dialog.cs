using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class Dialog : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private TextMeshProUGUI dialogText;
    public float fadeOut = 3.5f;
    public float fadeIn = 3.5f;
    private Tween fadeTween;


    public void Start()
    {
        canvasGroup.DOFade(0f, 0.01f);
    }


    public void SpawnDialogBox(string text)
    {
        dialogText.text = text;
        ShowDialog();
    }


    private void ShowDialog()
    {
        Sequence dialogSeq = DOTween.Sequence();
        dialogSeq.Append(canvasGroup.DOFade(1f, 0.1f))
                .AppendInterval(12f)
                .Append(canvasGroup.DOFade(0f, 1f));
    }
}
