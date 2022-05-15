using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunOrbit : MonoBehaviour
{
    public float distance = 20;
    public float orbitSpeed = 20f;
    public float tiltOffset = 20f;
    public float tiltMagnitude = 10f;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var orbitTime = Time.time * (orbitSpeed * 0.01f);
        var tiltTime = Time.time * (orbitSpeed * 0.01f) + (tiltOffset * 0.1f);
        float x = -Mathf.Cos(orbitTime) * distance;
        float z = Mathf.Sin(orbitTime) * distance;
        float y = Mathf.Sin(tiltTime) * tiltMagnitude;
        transform.position = new Vector3(x, y, z);
    }
}
