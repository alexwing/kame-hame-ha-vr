using Leap;
using System;
using UnityEngine;

public class FlyLeapController : MonoBehaviour
{
    /*
    WASD : basic movement
    SHIFT : Makes camera accelerate
    SPACE : Moves camera on X and Z axis only.  So camera doesn't gain any height
	*/


    [Header("HandS")]
    [SerializeField] private Transform _leftHand;
    [SerializeField] private Transform _rightHand;

    [SerializeField] private Transform _RotationGestoureAnchor;
    [SerializeField] private Transform _MoveGestoureAnchor;


    [Header("Score")]
    [SerializeField] private TMPro.TextMeshPro _DistanceRotationText;


    [Header("player")]
    public float rotationSpeed = 5f;       // Rotation speed.
    private float shiftAdd = 25f;           // Multiplied by how long shift is held.  Basically running.
    private float maxShift = 100f;			// Maximum speed when holdin gshift.
    private float totalRun = 1f;
    public static bool HandStateRight = false;
    public static bool HandStateLeft = false;

    public float mainSpeed = 70f;
    public float maxSpeed = 10f;
    public float forceMagnitude = 2f;


    public Controller controller;
    public void HandRightClosed()
    {
        HandStateRight = true;

    }
    public void HandRightOpened()
    {
        HandStateRight = false;
    }

    public void HandLeftClosed()
    {
        HandStateLeft = true;

    }
    public void HandLeftOpened()
    {
        HandStateLeft = false;
    }

    private void Update()
    {
        if (HandStateRight) PlayerMovement();
        if (HandStateLeft) GestureRotation();

    }
    private void GestureRotation()
    {
        float distance;
        distance = Vector3.Distance(_leftHand.position, _RotationGestoureAnchor.position);
        float rotateInfluence = 0f;

        if (!Utils.IsFrontAtObject(_RotationGestoureAnchor, _leftHand))
        {
            _DistanceRotationText.text = "RotLeft: " + String.Format("{0:0.00}", distance);
            rotateInfluence = -distance * Time.deltaTime * rotationSpeed;
        }
        else
        {
            _DistanceRotationText.text = "RotRight: " + String.Format("{0:0.00}", distance);
            rotateInfluence = +distance * Time.deltaTime * rotationSpeed;
        }

        GetComponent<Rigidbody>().AddTorque(new Vector3(0, rotateInfluence, 0));
    }

    private void PlayerMovement()
    {

        float distance;
        distance = Vector3.Distance(_rightHand.position, _MoveGestoureAnchor.position);
        float moveInfluence = 0f;


        if (!Utils.IsFrontAtObject(_MoveGestoureAnchor, _rightHand))
        {
            _DistanceRotationText.text = "moveFront: " + String.Format("{0:0.00}", distance);
            moveInfluence = distance * Time.deltaTime * rotationSpeed;
        }
        else
        {
            _DistanceRotationText.text = "MoveBack: " + String.Format("{0:0.00}", distance);
            moveInfluence = -distance * Time.deltaTime * rotationSpeed;
        }

        // float CurrentSpeed = Mathf.Min(mainSpeed * forceMagnitude * Time.deltaTime, maxSpeed);
        float CurrentSpeed = Mathf.Min(mainSpeed * moveInfluence * (forceMagnitude + Time.deltaTime), maxSpeed);

        Vector3 movement = Camera.main.transform.forward * CurrentSpeed;
        // GetComponent<Rigidbody>().MovePosition(transform.position + movement);
        GetComponent<Rigidbody>().AddForce(movement);



    }
}