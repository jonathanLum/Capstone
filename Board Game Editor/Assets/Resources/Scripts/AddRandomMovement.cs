using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddRandomMovement : MonoBehaviour
{
    public float minSpinSpeed = 80f;
    public float maxSpinSpeed = 1000f;
    public float minThrust = 50f;
    public float maxThrust = 600f;
    private float spinSpeed;

    // Start is called before the first frame update
    void Start()
    {
        spinSpeed = Random.Range(minSpinSpeed, maxSpinSpeed);
        float thrust = Random.Range(minThrust, maxThrust);

        Rigidbody rg = GetComponent<Rigidbody>();
        rg.AddForce(transform.forward * thrust * 2, ForceMode.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up, spinSpeed * Time.deltaTime);
    }
}
