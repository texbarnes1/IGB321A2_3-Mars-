using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour
{
    public float rotationSpeed;
    public float bobSpeed;
    public float bobHeight;

    private float verticalTarget;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);

        if (Mathf.Abs(transform.localPosition.y) >= Mathf.Abs(bobHeight))
        {
            bobHeight *= -1;
        }
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, Vector3.up * bobHeight, bobSpeed * Time.deltaTime);


    }
}
