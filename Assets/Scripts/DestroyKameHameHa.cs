
using System.Collections;
using UnityEngine;

public class DestroyKameHameHa : MonoBehaviour
{

    [SerializeField] private int FrameRate = 1;
    [SerializeField] public float Distance = 20;
    void Start()
    {
        InvokeRepeating("CheckDistance", 0, 0.5f / FrameRate);
    }

    private void CheckDistance()
    {
        float cameraDistance = Vector3.Distance(Camera.main.transform.position, transform.position);
        if (cameraDistance > Distance)
        {

            Destroy(gameObject);


        }
    }

    void OnDisable()
    {
        CancelInvoke("CheckDistance");
    }
    private void OnDestroy()
    {
        CancelInvoke("CheckDistance");
    }
}




