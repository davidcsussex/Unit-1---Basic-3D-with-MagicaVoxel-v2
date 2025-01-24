using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    // Start is called before the first frame update

    Rigidbody rb;
    float simpleTimer;
    float speed = 3;
    int dir;
    void Start()
    {

        rb = GetComponent<Rigidbody>();
        StartCoroutine("MoveGhost");
        dir = 0;
    }

    // Update is called once per frame
    void Update()
    {
        MoveGhost();

        if (dir == 0)
        {
            rb.linearVelocity = new Vector3(-speed, 0, 0);
        }
        if (dir == 1)
        {
            rb.linearVelocity = new Vector3(speed, 0, 0);
        }
        if (dir == 2)
        {
            rb.linearVelocity = new Vector3(0, 0, speed);
        }
        if (dir == 3)
        {
            rb.linearVelocity = new Vector3(0, 0, -speed);
        }
        print("dir=" + dir);


    }

    IEnumerator MoveGhost()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);

            dir = Random.Range(0, 4);




            yield return new WaitForSeconds(Random.Range(1, 5));
            rb.linearVelocity = Vector3.zero;
        }


    }
}
