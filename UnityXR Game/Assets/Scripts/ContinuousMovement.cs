using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class ContinuousMovement : MonoBehaviour
{
    public XRNode inputSource;
    public float walkSpeed = 3;
    public float airControlSpeed = 1;
    public float gravity = -9.81f;

    private XRRig rig;
    private float fallingSpeed;
    private Rigidbody rb;

    private Vector2 inputAxis;

    public LayerMask groundLayer;

    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<XRRig>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        InputDevice device = InputDevices.GetDeviceAtXRNode(inputSource);
        device.TryGetFeatureValue(CommonUsages.primary2DAxis, out inputAxis);
    }

    private void FixedUpdate()
    {
        Quaternion headYaw = Quaternion.Euler(0, rig.cameraGameObject.transform.eulerAngles.y, 0);
        Vector3 direction = headYaw * new Vector3(inputAxis.x, 0, inputAxis.y);

        bool isGrounded = GroundCheck();    //Bool to store groundcheck value

        if (direction != Vector3.zero)
        {
            if(isGrounded) rb.MovePosition(transform.position + direction * Time.fixedDeltaTime * walkSpeed);   
            else rb.MovePosition(transform.position + direction * Time.fixedDeltaTime * airControlSpeed);       //Reduces influcence of joystick when in the air
        }

        if (isGrounded) fallingSpeed = 0;
        else fallingSpeed += gravity * Time.fixedDeltaTime;
        //rb.MovePosition(transform.position + Vector3.up * fallingSpeed * Time.fixedDeltaTime);
    }

    public bool GroundCheck()
    {
        Vector3 rayStart = transform.TransformPoint(rb.centerOfMass);
        float rayLength = rb.centerOfMass.y + 0.01f;
        bool hasHit = Physics.SphereCast(rayStart, GetComponent<CapsuleCollider>().radius, Vector3.down, out RaycastHit hitInfo, rayLength, groundLayer);
        return hasHit;
    }
}
