using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnNpcWalkersScript : MonoBehaviour
{
    public float timeSinceLastSpawn = 0f;
    public float timeBetweenSpawns = 3f;
    public GameObject WalkerNpcPrefab;
    public Transform WalkerNpcParent;
    public Transform spawnXN;
    public Transform spawnXP;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		if (timeSinceLastSpawn > timeBetweenSpawns)
		{
            timeSinceLastSpawn = 0f;
            GameObject npc = Instantiate(WalkerNpcPrefab, WalkerNpcParent);
            int r = Random.Range(0, 2);
            timeBetweenSpawns = Random.Range(2f, 4f);
            if (r == 0)
            {
                npc.transform.SetPositionAndRotation(spawnXP.position, npc.transform.rotation);
            }
			else
			{
                npc.transform.Rotate(0, 180, 0);
                npc.GetComponent<BasicWalkScript>().speed *= -1;
                npc.transform.SetPositionAndRotation(spawnXN.position, npc.transform.rotation);
            }
		}
        timeSinceLastSpawn += Time.deltaTime;
    }

}
