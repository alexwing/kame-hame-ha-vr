﻿using Leap;
using UnityEngine;

public class FlyLeapController : MonoBehaviour
{
    /*
    WASD : basic movement
    SHIFT : Makes camera accelerate
    SPACE : Moves camera on X and Z axis only.  So camera doesn't gain any height
	*/


    [Header("Hand")]
    [SerializeField] private Transform _leftHand;
    [SerializeField] private Transform _rightHand;

    public float mainSpeed = 2f;            // Regular speed.
    public float rotationSpeed = 20f;       // Rotation speed.
    private float shiftAdd = 25f;           // Multiplied by how long shift is held.  Basically running.
    private float maxShift = 100f;			// Maximum speed when holdin gshift.
    private float totalRun = 1f;
    private bool lockMovement = false;
    public static bool HandState = false;

    public Controller controller;



    public void  HandClosed()
    {
        HandState = true;

    }

    public void HandOpened()
    {
        HandState = false;

    }

    private void Update()
    {



        Cursor.visible = lockMovement;
		Cursor.lockState = lockMovement ? CursorLockMode.None : CursorLockMode.Locked;

		if (!lockMovement)
        {
           

            Vector3 p = GetBaseInput();

            if (Input.GetKey(KeyCode.LeftShift))
            {
                totalRun += Time.deltaTime;
                p = p * totalRun * shiftAdd;
                p.x = Mathf.Clamp(p.x, -maxShift, maxShift);
                p.y = Mathf.Clamp(p.y, -maxShift, maxShift);
                p.z = Mathf.Clamp(p.z, -maxShift, maxShift);
            }
            else
            {
                totalRun = Mathf.Clamp(totalRun * 0.5f, 1f, 1000f);
                p = p * mainSpeed;
            }

            p = p * Time.deltaTime;
            Vector3 newPosition = transform.position;

            if (Input.GetKey(KeyCode.Space))
            { 
				// If player wants to move on X and Z axis only

                transform.Translate(p);
                newPosition.x = transform.position.x;
                newPosition.z = transform.position.z;
                transform.position = newPosition;
            }
            else transform.Translate(p);
		}

        if (Input.GetKeyDown(KeyCode.Escape))
			lockMovement = !lockMovement;


      
    }

    private Vector3 GetBaseInput()
    { 
		// Returns the basic values, if it's 0 than it's not active.

        Vector3 p_Velocity = new Vector3();

        if (!lockMovement)
        {
			if (Input.GetKey(KeyCode.W)) p_Velocity += Vector3.forward;
			if (Input.GetKey(KeyCode.S)) p_Velocity += Vector3.back;
			if (Input.GetKey(KeyCode.A)) p_Velocity += Vector3.left;
			if (Input.GetKey(KeyCode.D)) p_Velocity += Vector3.right;
            if (HandState) p_Velocity += Vector3.forward;
        }

      

        return p_Velocity;
    }
}