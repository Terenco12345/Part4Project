using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MainCameraControls : MonoBehaviour
{
    public float speed = 1f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            float xValue = transform.eulerAngles.x - Input.GetAxis("Mouse Y") * speed;
            float rotation = Mathf.Clamp(xValue > 180 ? xValue - 360 : xValue, 20, 90);
           transform.eulerAngles = new Vector3(rotation, 0, 0);
        }
    }
}
