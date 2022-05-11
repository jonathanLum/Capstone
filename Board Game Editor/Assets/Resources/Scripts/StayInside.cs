using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StayInside : MonoBehaviour
{

    public CameraEdgeWalls cameraBoundaries;
    // Update is called once per frame
    void Update()
    {
        if(transform.position.x < cameraBoundaries.corners[0].x || transform.position.x > cameraBoundaries.corners[1].x || transform.position.z < cameraBoundaries.corners[0].z || transform.position.z > cameraBoundaries.corners[1].z)
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, cameraBoundaries.corners[0].x, cameraBoundaries.corners[1].x), transform.position.y, Mathf.Clamp(transform.position.z, cameraBoundaries.corners[0].z, cameraBoundaries.corners[1].z));
    }
}
