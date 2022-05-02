using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour
{

    public Camera cam;
    private Vector3 mOffset;
    [SerializeField] private Vector3 mouseDir;
    private float heightOffset = 2f;
    private float mZCoord;
    public float power = 15;
    public bool rolled = false;
    private bool rolling = false;
    private Vector3 resetPos;

    private void Start() {
        resetPos = transform.position;
    }
    
    private void Update() {
        mouseDir = new Vector3(Input.GetAxis("Mouse X"), 0, Input.GetAxis("Mouse Y"));
        
        if(rolling){
            if(GetComponent<Rigidbody>().IsSleeping()){
                StartCoroutine(Delay());
            }
        }
    }
    
    private void OnMouseDown() {
        mZCoord = cam.WorldToScreenPoint(gameObject.transform.position).z;
        mOffset = gameObject.transform.position - GetMouseWorldPos();
        mOffset.y += heightOffset;
    }

    private void OnMouseDrag() {
        if(!rolling && !rolled)
            transform.position = GetMouseWorldPos() + mOffset;
        //Debug.Log("dragging");
    }

    private void OnMouseUp() {
        if(!rolling && !rolled){
            Vector3 rot = new Vector3(Random.rotation.x, Random.rotation.y, Random.rotation.z);
            GetComponent<Rigidbody>().AddTorque(rot * 30f, ForceMode.Impulse);
            var spot = mOffset;
            spot.y -= heightOffset;
            GetComponent<Rigidbody>().AddForceAtPosition(new Vector3(Input.GetAxis("Mouse X"), 1, Input.GetAxis("Mouse Y")) * power, spot);
            GetComponent<Rigidbody>().velocity = new Vector3(Input.GetAxis("Mouse X"), 0, Input.GetAxis("Mouse Y")) * power;
            rolling = true;
        }
    }

    private Vector3 GetMouseWorldPos(){
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = mZCoord;
        //Debug.Log(cam.ScreenToWorldPoint(mousePoint));
        return cam.ScreenToWorldPoint(mousePoint);
    }

    IEnumerator Delay(){
        yield return new WaitForSeconds(1);
        rolling = false;
        rolled = true;
    }

    public void Reset(){
        rolling = false;
        rolled = false;
        transform.rotation = Quaternion.identity;
        transform.position = resetPos;
    }
}
