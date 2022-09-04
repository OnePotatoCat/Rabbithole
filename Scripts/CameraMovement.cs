using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraMovement : MonoBehaviour
{
    public GameObject player;
    [SerializeField] AudioSource asudioSource;
    private float roomSizeX = 20.4f;
    private float roomSizeY = 16.6f;

    private float xPos;
    private float yPos;
    private float oldXPos;
    private float oldYPos;
    private float timer;

    void Start()
    {
        //transform.position = player.transform.position;
        transform.DOMove(new Vector3(player.transform.position.x, player.transform.position.y, -10f), 0.25f, true);
        asudioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if ((transform.position.x - roomSizeX / 2) > player.transform.position.x)
        {
            xPos -= roomSizeX;
        }
        else if ((transform.position.x + roomSizeX / 2) < player.transform.position.x)
        {
            xPos += roomSizeX;
        }

        if ((transform.position.y - roomSizeY / 2) > player.transform.position.y)
        {
            yPos -= roomSizeY;
        }
        else if ((transform.position.y + roomSizeY / 2) < player.transform.position.y)
        {
            yPos += roomSizeY;
        }

        //if (oldXPos != xPos || oldYPos != yPos)
        //{
        //    if (timer > 10f)
        //    {
        //        transform.DOMove(new Vector3(xPos, yPos, -10f), 0.25f, true);
        //    }
        //    else
        //    {
                
        //    }
        //}
        transform.position = new Vector3(xPos, yPos, -10f);
        oldXPos = xPos;
        oldYPos = yPos;
    }
}
