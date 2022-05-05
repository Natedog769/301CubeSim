using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//this is the basic controls for the camera to move around the world space
public class CameraControls : MonoBehaviour
{

    public Rigidbody myRb;
    public float moveSpeed=20f;
    public float rotSpeed = 50f;
    float zoom = -10;
    Vector3 moveVector;
    Vector3 rotVector;
    float idleTimer;
    float maxWait = 2f;

    public Camera cam;
    public Transform target;
    private Vector3 previousPos;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {


        WASDMovement();

    }

    private void FixedUpdate()
    {
        //transform.position = transform.Translate();
    }
    

    void WASDMovement()
    {


        float horiz = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        float vert = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;

        float rot = Input.GetAxis("AltHoriz") * rotSpeed * Time.deltaTime;

        float zoom = Input.mouseScrollDelta.y;

        moveVector = new Vector3(horiz, vert, zoom);

        rotVector = new Vector3(0, rot);

        transform.Translate(moveVector);
        transform.Rotate(rotVector);

        if (horiz == 0 && vert == 0 && rot == 0)
        {
            idleTimer += Time.deltaTime;
            if (idleTimer > maxWait)
            {
            }
        }
    }

    void MouseLooks()
    {

        if (Input.GetMouseButtonDown(0))
        {
            previousPos = cam.ScreenToViewportPoint(Input.mousePosition);
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 dir = previousPos - cam.ScreenToViewportPoint(Input.mousePosition);

            cam.transform.position = new Vector3();

            cam.transform.Rotate(new Vector3(1, 0, 0), dir.y * 180);
            cam.transform.Rotate(new Vector3(0, 1, 0), -dir.x * 180, Space.World);

            cam.transform.Translate(new Vector3(0, 0, zoom));

            previousPos = cam.ScreenToViewportPoint(Input.mousePosition);

        }



        if (Input.mouseScrollDelta.y != 0)
        {

            zoom += Input.mouseScrollDelta.y * moveSpeed;
            if (zoom > -10 && zoom < 40)
            {
                cam.transform.position = (new Vector3(cam.transform.position.x, cam.transform.position.y, zoom));

            }
        }
    }


}
