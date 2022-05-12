using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorCamera : MonoBehaviour
 {
     public float panSpeed = 5f;
     public Vector2 panLimit;

     public float scrollSpeed = 20f;
     public float maxZoom = 3f;
     public float minZoom = 8f;
 
     void Update()
     {
         Vector3 pos = transform.position;
         var zoom = GetComponent<Camera>().orthographicSize;

         if (Input.GetKey("w"))
         {
             pos.z += panSpeed * Time.deltaTime;
         }
         if (Input.GetKey("s"))
         {
             pos.z -= panSpeed * Time.deltaTime;
         }
         if (Input.GetKey("d"))
         {
             pos.x += panSpeed * Time.deltaTime;
         }
         if (Input.GetKey("a"))
         {
             pos.x -= panSpeed * Time.deltaTime;
         }
         
         float scroll = Input.GetAxis("Mouse ScrollWheel");
         zoom -= scroll * scrollSpeed * 17f * Time.deltaTime;
         
         GetComponent<Camera>().orthographicSize = Mathf.Clamp(zoom, maxZoom, minZoom);
         pos.x = Mathf.Clamp(pos.x, -panLimit.x, panLimit.x);
         pos.z = Mathf.Clamp(pos.z, -panLimit.y, panLimit.y);

         transform.position = pos;
     }
 }
