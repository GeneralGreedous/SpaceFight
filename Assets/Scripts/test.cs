using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    // Start is called before the first frame update

    public float moveLeftRight;
    // Update is called once per frame
    void Update()
    {
        moveLeftRight = Input.GetAxis("MoveLeftRightAxis");
    }
}
