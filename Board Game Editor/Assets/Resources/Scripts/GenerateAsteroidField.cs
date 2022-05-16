using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateAsteroidField : MonoBehaviour
{
    public Transform asteroidPrefab;
    public int fieldRadius = 100;
    int asteroidCount = 100;

    // Start is called before the first frame update
    void Start()
    {
        asteroidCount = Random.Range(20, 200);
        var loc = gameObject.transform.position;
        for (int loop=0; loop < asteroidCount; loop++){
            Transform temp = Instantiate(asteroidPrefab, Random.insideUnitSphere * fieldRadius + loc, Random.rotation, gameObject.transform);
            temp.localScale = temp.localScale * Random.Range(.5f, 3);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
