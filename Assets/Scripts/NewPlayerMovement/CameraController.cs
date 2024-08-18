using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private Vector2 camSensitivity = new Vector2(0.5f, 0.5f);
    [SerializeField]
    private Transform orientation;
    [SerializeField]
    private float cameraYLimit;
    private Vector2 cameraRotation;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * camSensitivity.x;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * camSensitivity.y;

        cameraRotation.y += mouseX;
        cameraRotation.x -= mouseY;
        cameraRotation.x = Mathf.Clamp(cameraRotation.x, -cameraYLimit, cameraYLimit);

        transform.rotation = Quaternion.Euler(cameraRotation.x, cameraRotation.y, 0);
        orientation.rotation = Quaternion.Euler(0, cameraRotation.y, 0);
    }
}
