using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CameraScript : MonoBehaviour
{

    public Transform target;
    public Vector3 targetLookOffset;
    public Vector3 targetFollowOffset;

    public float smoothTime = 0.25f;
    Vector3 currentVelocity;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        transform.LookAt(target.position + targetLookOffset);


        transform.position = Vector3.SmoothDamp( transform.position, target.position + targetFollowOffset, ref currentVelocity, smoothTime );

    }
}
