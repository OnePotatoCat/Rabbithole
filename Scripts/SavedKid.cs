using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavedKid : MonoBehaviour
{
    public static SavedKid instance;
    public bool savedKid = false;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this.gameObject); 
    }
}
