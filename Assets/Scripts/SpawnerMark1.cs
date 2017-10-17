using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerMark1 : MonoBehaviour {
    public Transform spawnPosition;                 //Get Spawn Pos+Rot
    public GameObject robotMk1;                     //Get NPC
    public float spawnRate = 2.0f;
    public float rotationsPerMinute = 5.0f;
    public float spawnPosOffset = 1;
    private float counter;

    // Settings
    public float speed = 1.0f;
    public float newPositionTime = 5.0f;
    public float xRange = 20;
    public float zRange = 20;

    private Vector3 target;

    // Use this for initialization
    void Start () {
        counter = spawnRate;
    }
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(0, 6.0f * rotationsPerMinute * Time.deltaTime, 0);

        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, target, step);
        if (Vector3.Distance(transform.position, target) < 0.1f)
        {
            target = new Vector3(Random.Range(-xRange, xRange), 3, Random.Range(-zRange, zRange));
        }

        if (counter >= spawnRate) {
            for (var i = 0; i < 10; i++)
            {
                GameObject npc = Instantiate(robotMk1, spawnPosition.position + 
                                       Random.insideUnitSphere * spawnPosOffset, spawnPosition.rotation);
            }
            counter = 0;
        }
        counter += 1.0f * Time.deltaTime;
    }
}
