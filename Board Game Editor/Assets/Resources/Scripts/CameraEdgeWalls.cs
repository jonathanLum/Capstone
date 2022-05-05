using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraEdgeWalls : MonoBehaviour
{
    public Camera cam;
    List<GameObject> walls = new List<GameObject>();
    void Start()
    {
        Vector3[] corners = { cam.ScreenToWorldPoint(new Vector3(0,0,0)),
                            cam.ScreenToWorldPoint(new Vector3(Screen.width,Screen.height,0))};
        
        //Debug.Log("top left: " + corners[0]);
        //Debug.Log("top right: " + corners[1]);

        GameObject plane1 = GameObject.CreatePrimitive(PrimitiveType.Plane);
        walls.Add(plane1);
        plane1.transform.position = new Vector3(gameObject.transform.position.x, 0, corners[0].z);
        plane1.transform.rotation = Quaternion.Euler(90,0,0);

        GameObject plane2 = GameObject.CreatePrimitive(PrimitiveType.Plane);
        walls.Add(plane2);
        plane2.transform.position = new Vector3(gameObject.transform.position.x, 0, corners[1].z);
        plane2.transform.rotation = Quaternion.Euler(-90,0,0);

        GameObject plane3 = GameObject.CreatePrimitive(PrimitiveType.Plane);
        walls.Add(plane3);
        plane3.transform.position = new Vector3(corners[0].x, 0, gameObject.transform.position.z);
        plane3.transform.rotation = Quaternion.Euler(0,0,-90);

        GameObject plane4 = GameObject.CreatePrimitive(PrimitiveType.Plane);
        walls.Add(plane4);
        plane4.transform.position = new Vector3(corners[1].x, 0, gameObject.transform.position.z);
        plane4.transform.rotation = Quaternion.Euler(0,0,90);

        foreach(GameObject plane in walls){
            plane.transform.localScale = new Vector3(100,100,100);
            plane.GetComponent<MeshRenderer>().enabled = false;
        }
    }

}
