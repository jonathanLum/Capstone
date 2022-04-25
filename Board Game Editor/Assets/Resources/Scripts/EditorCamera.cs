using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorCamera : MonoBehaviour
 {
     public float panSpeed = 5f;
     public Vector2 panLimit;

     public float scrollSpeed = 20f;
     public float minY = 5f;
     public float maxX = 12f;
 
     void Update()
     {
         Vector3 pos = transform.position;

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
         pos.y -= scroll * scrollSpeed * 17f * Time.deltaTime;
         
         pos.y = Mathf.Clamp(pos.y, minY, maxX);
         pos.x = Mathf.Clamp(pos.x, -panLimit.x, panLimit.x);
         pos.z = Mathf.Clamp(pos.z, -panLimit.y, panLimit.y);

         transform.position = pos;
     }
 }
