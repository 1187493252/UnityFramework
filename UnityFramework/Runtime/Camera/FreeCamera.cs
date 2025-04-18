﻿using UnityEngine;


public class FreeCamera : MonoBehaviour
{
    public float camSens = 0.1f;
    public float camRoate = 0.5f;


    public float camSpeed = 1.0f;
    public float fov = 60.0f;

    private Vector3 camPos;
    private Vector3 camVel;
    private Vector3 lastMouse;
    private Vector2 angVel;
    private float camPhi;
    private float camTheta;


    protected void Start()
    {
        var mainCamera = Camera.main;

        if (mainCamera == null)
        {
            return;
        }

        camPos = mainCamera.transform.position;
        camVel = new Vector3(0f, 0f, 0f);
        lastMouse = Input.mousePosition;
        camPhi = mainCamera.transform.rotation.eulerAngles.x;
        camTheta = mainCamera.transform.rotation.eulerAngles.y;
        angVel = new Vector2(0, 0);
    }


    protected void Update()
    {
        UpdateCamera();
    }

    private Vector3 GetMousePosition()
    {
        var mainCamera = Camera.main;

        if (mainCamera == null)
        {
            return new Vector3(0, 10000, 0);
        }

        var ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        return Physics.Raycast(ray, out var hit) ? hit.point : new Vector3(0, 10000, 0);
    }

    private static Vector3 GetBaseInput()
    {
        var pVelocity = new Vector3();

        if (Input.GetKey(KeyCode.W))
        {
            pVelocity += new Vector3(0, 0, 1);
        }

        if (Input.GetKey(KeyCode.S))
        {
            pVelocity += new Vector3(0, 0, -1);
        }

        if (Input.GetKey(KeyCode.A))
        {
            pVelocity += new Vector3(-1, 0, 0);
        }

        if (Input.GetKey(KeyCode.D))
        {
            pVelocity += new Vector3(1, 0, 0);
        }

        if (Input.GetKey(KeyCode.E))
        {
            pVelocity += new Vector3(0, 1, 0);
        }

        if (Input.GetKey(KeyCode.Q))
        {
            pVelocity += new Vector3(0, -1, 0);
        }

        return pVelocity;
    }

    private void UpdateCamera()
    {
        lastMouse = Input.mousePosition - lastMouse;
        lastMouse = new Vector3(-lastMouse.y * camSens, lastMouse.x * camSens, 0.0f);
        if (Input.GetMouseButton(0))
            angVel += new Vector2(lastMouse.x, lastMouse.y);

        angVel *= camRoate;

        camPhi += angVel.x;
        camTheta += angVel.y;
        lastMouse = Input.mousePosition;

        camVel += Quaternion.Euler(camPhi, camTheta, 0) * GetBaseInput() * camSpeed;



        camPos += camVel * Time.smoothDeltaTime;
        camVel *= 0.94f;

        var mainCamera = Camera.main;

        // update projection matrix
        if (mainCamera != null)
        {
            mainCamera.transform.position = camPos;
            mainCamera.transform.rotation = Quaternion.Euler(camPhi, camTheta, 0);
            mainCamera.fieldOfView = fov;
        }
    }
}
