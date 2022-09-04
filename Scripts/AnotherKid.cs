using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnotherKid : MonoBehaviour
{
    [SerializeField] Canvas DialogCanvas;
    [SerializeField] Dialog dialog;

    string dialogText;

    // Start is called before the first frame update
    void Start()
    {
        dialogText = "Thank you for breaking the curse, turning me back to human!";
        dialog.SpawnDialogBox(dialogText);
    }
}
