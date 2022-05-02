using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CameraEdgeCollide : MonoBehaviour
{
    Mesh[] boundaryMeshes;
    MeshCollider[] boundaryColliders = new MeshCollider[4];
    MeshRenderer boundaryRender;

    void Start()
    {
        Vector3[] corners = { Camera.main.ScreenToWorldPoint(new Vector3(0,0,0)),
                            Camera.main.ScreenToWorldPoint(new Vector3(Screen.width,0,0)),
                            Camera.main.ScreenToWorldPoint(new Vector3(0,Screen.height,0)),
                            Camera.main.ScreenToWorldPoint(new Vector3(Screen.width,Screen.height,0))};

        /*Debug.Log("top left: " + corners[0]);
        Debug.Log("top right: " + corners[1]);
        Debug.Log("bot left: " + corners[2]);
        Debug.Log("bot right: " + corners[3]);*/

        boundaryMeshes = generateSelectionMeshes(corners);
        int i = 0;
        foreach(Mesh mesh in boundaryMeshes){
            boundaryColliders[i] = gameObject.AddComponent<MeshCollider>();
            boundaryColliders[i].sharedMesh = mesh;
            boundaryColliders[i].convex = false;
            boundaryColliders[i].isTrigger = false;
        }
        boundaryRender = gameObject.AddComponent<MeshRenderer>();
        var filter = gameObject.AddComponent<MeshFilter>();
        filter.mesh = boundaryMeshes[1];
    }

    Mesh[] generateSelectionMeshes(Vector3[] corners)
    {
        Vector3[] verts = new Vector3[8];
        int[] tris = { 0,1,2, 2,1,3 }; //map the tris of our plane

        for(int i = 0; i < 4; i++)
        {
            verts[i] = corners[i];
        }

        for(int j = 4; j < 8; j++)
        {
            verts[j] = corners[j - 4] + Vector3.down * 12f;
        }
        Vector3[][] finalVerts =
        {
            new Vector3[] {verts[0],verts[1],verts[4],verts[5]},
            new Vector3[] {verts[1],verts[3],verts[5],verts[7]},
            new Vector3[] {verts[3],verts[2],verts[7],verts[6]},
            new Vector3[] {verts[2],verts[0],verts[6],verts[4]}
        };

        Mesh[] selectionMeshes = {new Mesh(), new Mesh(), new Mesh(), new Mesh()};
        for(int i =0; i <4; i++){
            selectionMeshes[i].vertices = finalVerts[i];
            selectionMeshes[i].triangles = new int[]{ 0,1,2, 2,1,3 };
            selectionMeshes[i].RecalculateNormals();

            Vector3[] normals = selectionMeshes[i].normals;

            for(int n =0; n <4; n++){
                normals[n] *= -1f;
            }
            selectionMeshes[i].normals = normals;
            
            Debug.Log(selectionMeshes[i].normals[0]);
        }
        
        return selectionMeshes;
    }
}
