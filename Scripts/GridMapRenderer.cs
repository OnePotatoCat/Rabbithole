using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMapRenderer : MonoBehaviour
{
    [SerializeField] private List<GameObject> randObjs;
    [SerializeField] private GameObject lantern;
    [SerializeField] private GameObject alter;
    [SerializeField] private GameObject escape;


    [SerializeField] private List<GameObject> laternRockFracments;
    [SerializeField] private List<GameObject> medallionPiece;
    [SerializeField] private GameObject rabbitMonster;

    private List<GameObject> fullRandObjs;

    private List<Vector2> randObjPos = new List<Vector2> 
    { 
        new Vector2(5f, 3.9f),
        new Vector2(-5f, 3.9f),
        new Vector2(5f, -3.9f),
        new Vector2(-5f, -3.9f),

        new Vector2(-4f, 0f),
        new Vector2(4f, 0f),
        new Vector2(0f, -1.6f),
        new Vector2(0f, 1.6f),

        new Vector2(1.75f ,2.45f),
        new Vector2(-1.75f,2.45f),
        new Vector2(1.75f, -2.45f),
        new Vector2(-1.75f, -2.45f),

        new Vector2(-3.5f, 3.6f),
        new Vector2(2.45f, -2f),
        new Vector2(-2.55f, -3.7f),
        new Vector2(2.55f, 3.7f),

        new Vector2(0f, 0.1f),
        new Vector2(-0.5f, -0.25f),
    };

    private List<Vector2> randLanternPos = new List<Vector2>
    {
        new Vector2(4.6f, -0.5f),
        new Vector2(-2.5f, -3.45f),
        new Vector2(-5.5f, -1.8f),
        new Vector2(-3.6f, 2.8f),
        new Vector2(5.0f, 3.5f)
    };

    private List<Vector2> randMedallionPos = new List<Vector2>
    {
        new Vector2(-6.5f, 1.5f),
        new Vector2(-6.5f, 4.5f),
        new Vector2(7.0f, 3.5f),
        new Vector2(6.5f, -5.2f),
        new Vector2(-6.5f, 4.5f),
        new Vector2(7.0f, -1.8f),
        new Vector2(-5.0f, -5.0f)
    };

    private void Start()
    {
        fullRandObjs = new List<GameObject>(randObjs.Count + laternRockFracments.Count);
        fullRandObjs.AddRange(randObjs);
        fullRandObjs.AddRange(laternRockFracments);
    }

    public bool GenerateRandomObjects(int numberOfObj)
    {
        fullRandObjs = new List<GameObject>(randObjs.Count + laternRockFracments.Count);
        fullRandObjs.AddRange(randObjs);
        fullRandObjs.AddRange(laternRockFracments);

        bool spawnLanternFragment = false;
        for (int i = 0; i < numberOfObj; i++)
        {
            Vector2 objPos = randObjPos[Random.Range(0, randObjPos.Count)];
            randObjPos.Remove(objPos);

            int objIndex = Random.Range(0, fullRandObjs.Count);
            if ((objIndex+1)> fullRandObjs.Count)
            {
                spawnLanternFragment = true;
            }

            GameObject spawnObj = fullRandObjs[objIndex];

            GameObject gameObject = Instantiate(spawnObj, new Vector2(this.transform.position.x + objPos.x, this.transform.position.y + objPos.y), Quaternion.identity);
        }

        return spawnLanternFragment;
    }

    public void GenerateStartRoomsObejct()
    {
        Vector2 objPos = randLanternPos[Random.Range(0, randLanternPos.Count)];
        GameObject gameObject = Instantiate(lantern, new Vector2(this.transform.position.x + objPos.x, this.transform.position.y + objPos.y), Quaternion.identity);
    }

    public void GenerateSecretRoomObject()
    {
        GameObject gameObject = Instantiate(alter, new Vector2(this.transform.position.x, this.transform.position.y), Quaternion.identity);
    }

    public void GenerateEscapeRoom()
    {
        GameObject gameObject = Instantiate(escape, new Vector2(this.transform.position.x, this.transform.position.y+ 0.15f), Quaternion.identity);
    }

    public void GenerateMedallionPiece(int i)
    {
        Vector2 objPos = randMedallionPos[Random.Range(0, randMedallionPos.Count)];
        GameObject gameObject = Instantiate(medallionPiece[i], new Vector2(this.transform.position.x + objPos.x, this.transform.position.y + objPos.y), Quaternion.identity);
    }

    public void SpawnMonster()
    {
        Vector2 objPos = randObjPos[Random.Range(0, randObjPos.Count)];
        randObjPos.Remove(objPos);
        GameObject gameObject = Instantiate(rabbitMonster, new Vector2(this.transform.position.x + objPos.x, this.transform.position.y + objPos.y), Quaternion.identity);
    }

}
