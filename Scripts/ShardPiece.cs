using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShardPiece : MonoBehaviour
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
            player.RefuelLantern();
            Destroy(this.gameObject);
        }
    }
}
