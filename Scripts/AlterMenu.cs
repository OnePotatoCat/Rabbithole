using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using DG.Tweening;

public class AlterMenu : MonoBehaviour
{
    [SerializeField] CanvasGroup alterMenu;
    [SerializeField] RectTransform medal;
    [SerializeField] CanvasGroup fadeCanvas;
    [SerializeField] Camera mainCamera;
    [SerializeField] Light2D globalLight;
    [SerializeField] Alter alter;
    [SerializeField] public SavedKid savedKid;

    public void PlaceMdealOnAlter()
    {

        float dist = medal.gameObject.transform.parent.transform.position.y - medal.transform.position.y;

        Sequence seq = DOTween.Sequence();
        seq.Append(medal.DOMoveY(medal.transform.position.y +dist, 1.5f));
        seq.Append(alterMenu.DOFade(0f, 1f));
        seq.Join(fadeCanvas.DOFade(1f, 3f));
        seq.Join(mainCamera.DOShakePosition(3f, new Vector3(2f, 2f, 0f), 10, 90, true));
        seq.AppendCallback(UpdateGlobalLight);
        seq.Append(fadeCanvas.DOFade(0f, 3f));
        

        alter = GameObject.FindWithTag("Alter").GetComponent<Alter>();
        alter.InsertMedallion();

        var objs = GameObject.FindGameObjectsWithTag("Monster");
        foreach (var obj in objs)
        {
            obj.SetActive(false);
        }
        savedKid.savedKid = true;
        Time.timeScale = 1f;
    }

    public void UpdateGlobalLight()
    {
        globalLight.intensity = 1f;
    }
}
