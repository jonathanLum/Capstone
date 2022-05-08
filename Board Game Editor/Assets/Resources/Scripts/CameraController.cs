using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    GameObject target;
    public static CameraController instance;
    public Transform mainCameraRig;
    public Transform playerCameraRig;
    public Transform followTransform;
    public Transform cameraTransform;
    public GameObject diceCamera;
    public float cameraSpeed;
    public float cameraMovementTime;
    public float cameraRotationAmount;

    public Vector3 cameraPosition;
    public Vector3 cameraZoom;
    public Vector3 cameraZoomAmount;
    public Quaternion cameraRotation;

    public enum CAMERA { MAIN, PLAYER };
    public CAMERA currentCamera;
    public Transform playerTarget;

    public bool gamePaused;
    void Awake()
    {
        instance = this;
        gamePaused = false;
    }
    // Start is called before the first frame update
    void Start()
    {
        // instance = this;

        cameraTransform.gameObject.GetComponent<Camera>().enabled = true;
        followTransform.gameObject.GetComponent<Camera>().enabled = false;
        diceCamera.GetComponent<Camera>().enabled = false;


        cameraPosition = mainCameraRig.position;
        cameraRotation = mainCameraRig.rotation;
        cameraZoom = mainCameraRig.localPosition;// cameraTransform.localPosition;  // stay relative to the camera rig
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseGame();
        }

        if (currentCamera == CAMERA.MAIN)
        {
            ControlKeyboardInput();
        }

        if (playerTarget != null)
        {
            playerCameraRig.position = Vector3.Lerp(playerCameraRig.position, playerTarget.position, cameraMovementTime * Time.deltaTime);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            SwitchCameras();
        }


        // transform.LookAt(target.transform);

        // float xDir = Input.GetAxis("Horizontal");
        // float zDir = Input.GetAxis("Vertical");

        // Vector3 moveDir = new Vector3(xDir, 0.0f, zDir);

        // transform.position += moveDir * 0.01f;
    }

    private void TogglePauseGame()
    {
        gamePaused = !gamePaused;

        if (gamePaused)
        {
            Time.timeScale = 0f;
        }

        else
        {
            Time.timeScale = 1f;
        }
    }

    private void FixedUpdate()
    {

    }
    public void SwitchCameras()
    {
        if (currentCamera == CAMERA.MAIN)
        {
            currentCamera = CAMERA.PLAYER;
            cameraTransform.gameObject.GetComponent<Camera>().enabled = false;
            followTransform.gameObject.GetComponent<Camera>().enabled = true;
            diceCamera.GetComponent<Camera>().enabled = true;
        }

        else
        {
            currentCamera = CAMERA.MAIN;
            cameraTransform.gameObject.GetComponent<Camera>().enabled = true;
            followTransform.gameObject.GetComponent<Camera>().enabled = false;
            diceCamera.GetComponent<Camera>().enabled = false;
        }
    }
    public void SetTarget(GameObject obj)
    {
        target = obj;
    }
    void ControlKeyboardInput()
    {

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            cameraPosition += mainCameraRig.forward * cameraSpeed;
        }

        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            cameraPosition += mainCameraRig.forward * -cameraSpeed;
        }

        else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            cameraPosition += mainCameraRig.right * -cameraSpeed;
        }

        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            cameraPosition += mainCameraRig.right * cameraSpeed;
        }

        else if (Input.GetKey(KeyCode.Q))
        {
            cameraRotation *= Quaternion.Euler(Vector3.up * cameraRotationAmount);
        }

        else if (Input.GetKey(KeyCode.E))
        {
            cameraRotation *= Quaternion.Euler(Vector3.up * -cameraRotationAmount);
        }

        else if (Input.GetKey(KeyCode.R))
        {
            cameraZoom += cameraZoomAmount;
        }

        else if (Input.GetKey(KeyCode.F))
        {
            cameraZoom += -cameraZoomAmount;
        }

        mainCameraRig.position = Vector3.Lerp(mainCameraRig.position, cameraPosition, Time.deltaTime * cameraMovementTime);
        mainCameraRig.rotation = Quaternion.Lerp(mainCameraRig.rotation, cameraRotation, Time.deltaTime * cameraMovementTime);
        mainCameraRig.localPosition = Vector3.Lerp(mainCameraRig.localPosition, cameraZoom, Time.deltaTime * cameraMovementTime);
    }
}
