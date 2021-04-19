using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TargetSnap : MonoBehaviour
{
    public Transform snapTo;
    private Rigidbody body;
    public float snapTime = 2;

    private float dropTimer;
    public bool interactable = true;

    private void Start()
    {
        body = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {

        if (interactable)
        {
            //  body.isKinematic = false;
            dropTimer = -1;
        }
        else
        {
            dropTimer += Time.deltaTime / (snapTime / 2);

            //  body.isKinematic = dropTimer > 1;

            if (dropTimer > 1)
            {
                body.velocity = new Vector3(0f, 0f, 0f);
                body.angularVelocity = new Vector3(0f, 0f, 0f);
                //transform.parent = snapTo;
                transform.position = snapTo.position;
                transform.rotation = snapTo.rotation;

                interactable = true;
            }
            else
            {
                float t = Mathf.Pow(35, dropTimer);

                body.velocity = Vector3.Lerp(body.velocity, Vector3.zero, Time.fixedDeltaTime * 4);
                if (body.useGravity)
                    body.AddForce(-Physics.gravity);

                transform.position = Vector3.Lerp(transform.position, snapTo.position, Time.fixedDeltaTime * t * 3);
                transform.rotation = Quaternion.Slerp(transform.rotation, snapTo.rotation, Time.fixedDeltaTime * t * 2);

            }
        }
    }

}