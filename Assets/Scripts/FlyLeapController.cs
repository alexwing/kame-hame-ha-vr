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


    [Header("Hand")]
    [SerializeField] private Transform _leftHand;
    [SerializeField] private Transform _rightHand;

    [SerializeField] private Transform _RotationGestoureAnchor;


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

        transform.Translate(p);
        
        if (HandStateLeft) GestureRotation();


    }
    private void GestureRotation()
    {
             float distance;
             distance = Vector3.Distance(_leftHand.position, _RotationGestoureAnchor.position);
      //  Vector3 euler = transform.rotation.eulerAngles;
        float rotateInfluence = 0;


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


        /*
        bool curHatLeft = OVRInput.Get(OVRInput.Button.PrimaryShoulder);

        if (curHatLeft && !prevHatLeft)
            euler.y -= RotationRatchet;

        prevHatLeft = curHatLeft;

        bool curHatRight = OVRInput.Get(OVRInput.Button.SecondaryShoulder);

        if (curHatRight && !prevHatRight)
            euler.y += RotationRatchet;

        prevHatRight = curHatRight;
        */
      //  euler.y += rotateInfluence;
      //  transform.rotation = Quaternion.Euler(euler);
       // GetComponent<Rigidbody>().AddForce(euler);
        GetComponent<Rigidbody>().AddTorque(new Vector3(0, rotateInfluence,0));
        //_DistanceRotationText.text = "Rotation: " + String.Format("{0:0.00}", distance);
    }

    private Vector3 GetBaseInput()
    {
        // Returns the basic values, if it's 0 than it's not active.

        Vector3 p_Velocity = new Vector3();


        if (Input.GetKey(KeyCode.W)) p_Velocity += Vector3.forward;
        if (Input.GetKey(KeyCode.S)) p_Velocity += Vector3.back;
        if (Input.GetKey(KeyCode.A)) p_Velocity += Vector3.left;
        if (Input.GetKey(KeyCode.D)) p_Velocity += Vector3.right;
        if (HandStateRight)
        {
            //   transform.position += Camera.main.transform.forward * Time.deltaTime * 20;
            //  transform.position += Vector3.Lerp(transform.position, Camera.main.transform.forward, Time.time);
            /// transform.position += _rightHand.transform.forward.normalized * Time.deltaTime * 20;
            //   p_Velocity += Vector3.forward;
            mainSpeed = Mathf.Min(mainSpeed + forceMagnitude * Time.deltaTime, maxSpeed);

            Vector3 movement = Camera.main.transform.forward * mainSpeed * Time.deltaTime * 100;
            // GetComponent<Rigidbody>().MovePosition(transform.position + movement);
            GetComponent<Rigidbody>().AddForce(movement);


            //transform.position += _rightHand.GetComponent<Rigidbody>().transform.forward * Time.deltaTime * 20;

        }

        return p_Velocity;
    }
}