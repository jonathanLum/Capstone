using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public GameObject target;
    Vector3 endPos;
    public float speed = 2f;


    private void Start() {
        float heightOffset = 0.531f;
        endPos = new Vector3(target.transform.position.x, target.transform.position.y+heightOffset, target.transform.position.z);
        gameObject.transform.LookAt(endPos);
    }

    void Update()
    {
        var step =  speed * Time.deltaTime; // calculate distance to move
        transform.position = Vector3.MoveTowards(transform.position, endPos, step);

        // Check if the position of the cube and sphere are approximately equal.
        if (Vector3.Distance(transform.position, endPos) < 0.001f)
        {
            target.GetComponent<Animator>().SetTrigger("Hit");
            Destroy(gameObject);
        }
    }
}
