using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField] public Transform myTransform;
    public int hi = 0;



    // Start is called before the first frame update
    void Start()
    {

        myTransform.DOMove(new Vector3(1f, 1f, 1f), 10f);
        Debug.Log("Hello");
    }

    // Update is called once per frame  
    void Update()
    {
        
    }
}
