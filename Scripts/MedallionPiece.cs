using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedallionPiece : MonoBehaviour
{
    [SerializeField] public Player player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            player.GainAMedallion();
            Destroy(this.gameObject);
        }
    }
}
