using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanternBeam : MonoBehaviour
{
    [SerializeField] public PolygonCollider2D col;
    [SerializeField] public Player player;

    //private void OnTriggerEnter2D(Collider2D other)
    //{
    //    if (other.CompareTag("Monster"))
    //    {
    //        if (player.lightIntensity == player.maxLightIntensity)
    //        {
    //            other.GetComponent<Monster>().Hurt();
    //        }
    //    }
    //}

    //private void OnTriggerStay2D(Collider2D other)
    //{
    //    if (other.CompareTag("Monster"))
    //    {
    //        if (player.lightIntensity == 2.8f)
    //        {
    //            other.GetComponent<Monster>().Hurt();
    //        }
    //    }
    //}
}
