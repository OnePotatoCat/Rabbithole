using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class UpdateGameOverCanvas : MonoBehaviour
{
    [SerializeField] GameObject singleScout;
    [SerializeField] GameObject twoScouts;
    [SerializeField] TextMeshProUGUI text;
    private void Awake()
    {
        if (SavedKid.instance.savedKid)
        {
            twoScouts.SetActive(true);
            text.text = "You escaped with a new friend!";
        }
        else
        {
            singleScout.SetActive(true);
            text.text = "You escaped alone. Perhaps you can saved someone in the rabbit hole?";
        }
    }


    public void LoadMainMenu()
    {
        Destroy(SavedKid.instance.gameObject);
        SceneManager.LoadScene(0);
    }
}
