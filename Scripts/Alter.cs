using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Alter : MonoBehaviour
{
    [SerializeField] public Light2D light;
    [SerializeField] public GameObject medallion;
    float ligthflicker = 0.1f;
    [SerializeField] public GameObject alterMenu;

    [SerializeField] public Player player;
    [SerializeField] private GameObject anotherKid;
    [SerializeField] private AnotherKid kidScript;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            player.TouchAlter();
        }
    }

    // Update is called once per frame
    void Update()
    {
        light.intensity = ligthflicker * Random.Range(3f, 6f);
    }

    public void InsertMedallion()
    {
        medallion.SetActive(true);
        anotherKid.SetActive(true);
    }
}
