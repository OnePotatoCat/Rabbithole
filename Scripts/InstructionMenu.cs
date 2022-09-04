using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionMenu : MonoBehaviour
{
    private void Start()
    {
        Time.timeScale = 0f;
        gameObject.SetActive(true);
    }


    public void Resume()
    {
        Time.timeScale = 1f;
        gameObject.SetActive(false);
    }

}
