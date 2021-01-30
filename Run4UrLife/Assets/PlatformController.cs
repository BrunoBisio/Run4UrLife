﻿using UnityEngine;
using System.Collections.Generic;

public enum Seed
{
    X,
    Y
}

public class PlatformController : MonoBehaviour
{

    public  GameObject prefab;
    private Queue<GameObject> queue = new Queue<GameObject>();
    public  float maxDistanceX = 3;
    public float maxDistanceY = 3;
    [Header("Seed")]
    public float seedX = 1.25f;
    public float seedY = 1.25f;
    public bool random = false;
    [Header("PlatformSizes")]
    public float xSize = 1;
    public float ySize = 1;
    [Header("Resize")]
    public float xReSize = 1f;
    public float yReSize = 1f;

    private float lastNoise = 0;
    private float diffBetweenNoises = 0;
    // Start is called before the first frame update
    void Start()
    {
        if(random) {
            generateSeed(Seed.X);
            generateSeed(Seed.Y);
        }
        
        //createElements(0);


    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void DestroyPlatforms(int topXElements)
    {
        Debug.Log("Llame al destroy");
        if(queue.Count > 10)
        {
            for (int x = 0; x < topXElements; x++)
            {
                GameObject platform = queue.Dequeue();
                Debug.Log(platform);
                Destroy(platform, 0.5f);
            }
        }
       
        
    }
    public float createElements(float baseX)
    {
        float xD = baseX;
        for (int x = 0; x < 10; x++)
        {
            changeNoise(Seed.Y);
            float yDistance = (float)getPerlinNoise(xD, seedY, maxDistanceY);
            if(lastNoise != 0)
            {
                diffBetweenNoises += lastNoise - yDistance;
            }
            lastNoise = yDistance;
            float xDistance = (float)getPerlinNoise(xD, seedX, maxDistanceX);

            GameObject platform = Instantiate(prefab, new Vector2((xD) + xDistance, yDistance), Quaternion.identity);
            Renderer re = platform.GetComponent<SpriteRenderer>();

            platform.transform.localScale = new Vector2(xReSize, yReSize);
            xD += re.bounds.size.x + xDistance;
            queue.Enqueue(platform);
        }
        return xD;
    }

    public void generateSeed(Seed seed)
    {
        switch(seed)
        {
            case Seed.X:
                seedX = Random.Range(0f, 1000f);
                break;
            case Seed.Y:
                seedY = Random.Range(0f, 1000f);
                break;
        }
        
    }

    public void changeNoise(Seed seed)
    {
        if(Mathf.Abs(diffBetweenNoises) < 2)
        {
            generateSeed(seed);
        }
    }

    public static double getPerlinNoise(float x, float seed, float maxSize)
    {
        return (Mathf.PerlinNoise(x, seed) -0.5) * maxSize + (maxSize/2);
    }
}