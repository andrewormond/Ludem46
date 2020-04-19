using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudSpawner : MonoBehaviour
{
    public float minRate = 10;
    public float maxRate = 40;

    public float xMin = -500;
    public float xMax = 500;
    public float zMin = -500;
    public float zMax = 500;
    public float yMin = 45;
    public float yMax = 100;

    float zSpeedMax = 10;
    float xSpeedMin = 10;
    float xSpeedMax = 25;

    public GameObject cloudPrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private static int count = 0;
    public void SpawnCloud(Vector3 position)
    {
        if (cloudPrefab != null)
        {
            GameObject obj = Instantiate(cloudPrefab);
            obj.name = "Cloud " + (count++);
            obj.transform.SetParent(transform);
            Cloud cloud = obj.GetComponent<Cloud>();
            cloud.Direction = new Vector3(Random.Range(xSpeedMin, xSpeedMax), 0, Random.Range(-zSpeedMax, zSpeedMax));
            cloud.transform.position = position;
        }
    }

    public void SpawnCloud()
    {
        SpawnCloud(new Vector3(xMin, Random.Range(yMin, yMax), Random.Range(zMin, zMax)));
    }

    private void Awake()
    {
        for(int i = 0; i < maxRate * 0.75f; i++)
        {
            SpawnCloud(new Vector3(Random.Range(xMin, xMax), Random.Range(yMin, yMax), Random.Range(zMin, zMax)));
        }
    }

    private float holdoff = 0;
    // Update is called once per frame
    void Update()
    {
        holdoff -= Time.deltaTime;
        if(holdoff <= 0)
        {
            SpawnCloud();

            holdoff = Random.Range(60f/minRate, 60f/maxRate);
        }
    }
}
