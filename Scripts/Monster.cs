using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Monster : MonoBehaviour
{
    [SerializeField] private Collider2D col;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private SpriteRenderer sr;

    [SerializeField] public RoomGenerator rg;

    [SerializeField] public Player player;
    [SerializeField] public Transform playerPos;
    [SerializeField] AudioSource audioSource;

    public int level = 0;
    public float speed = 2.2f;
    public float painThreTime = 3f;

    public float stopMultiplier = 0;
    bool hunting = false;
    bool isHurting = false;

    int[] roomPos = new int[2];

    private List<Vector2> randPos = new List<Vector2>
    {
        new Vector2(5f, 3.9f),
        new Vector2(-5f, 3.9f),
        new Vector2(5f, -3.9f),
        new Vector2(-5f, -3.9f),
        new Vector2(-0f, -0f),
    };


    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        playerPos = player.gameObject.GetComponent<Transform>();
        rg = GameObject.FindGameObjectWithTag("RoomGen").GetComponent<RoomGenerator>();
        audioSource.Play();
    }

    public void Update()
    {
        if (Mathf.Abs(Vector2.Distance(playerPos.position, gameObject.transform.position)) <6.5f || isHurting)
        {
            hunting = true;
        }

        if (painThreTime <= 0f)
        {
            level++;
            stopMultiplier = 1f;
            painThreTime = 3f;
            FadeAndRelocate();
        }

        if (hunting)
        {
            float hurtMulti;
            Vector2 direction = new Vector2(playerPos.position.x - transform.position.x, playerPos.position.y - transform.position.y);
            transform.up = direction;
            direction.Normalize();

            if (isHurting)
            {
                painThreTime -= Time.deltaTime;
                hurtMulti = 0.35f;
            }
            else
            {
                hurtMulti = 1f;
            }

            rb.velocity = new Vector2(direction.x * hurtMulti * speed * (1f - stopMultiplier), direction.y * hurtMulti * speed* (1f - stopMultiplier));
        }      
    }

    private void FadeAndRelocate()
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(gameObject.transform.DOShakePosition(2f, new Vector3(0.5f, 0.5f, 0f)));
        seq.Join(sr.DOFade(0f, 2f));
        Relocate();
        stopMultiplier = -0.2f* level;
    }
    private void Relocate()
    {
        bool same;
        do
        {
            same = false;
            roomPos[0] = Random.Range(0, rg.mapSize);
            roomPos[1] = Random.Range(0, rg.mapSize);
            if (roomPos[0] == rg.startingRoomPos[0] && roomPos[1] == rg.startingRoomPos[1])
            {
                same = true;
            }
            if (roomPos[0] == rg.secretRoomPos[0] && roomPos[1] == rg.secretRoomPos[1])
            {
                same = true;
            }
            if (roomPos[0] == rg.exitRoomPos[0] && roomPos[1] == rg.exitRoomPos[1])
            {
                same = true;
            }

        } while (same);

        Vector2 pos = randPos[Random.Range(0, randPos.Count)];
        gameObject.transform.position = new Vector2(roomPos[0] * 20.4f + pos.x, roomPos[0] * 16.6f + pos.y);

        sr.DOFade(1f, 2f);
        isHurting = false;
        hunting = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Lantern Beam"))
        {
            isHurting = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Lantern Beam"))
        {
            isHurting = false;
        }
    }

}
