using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(Rigidbody))]
public class MoveController : MonoBehaviour
{


    [Header("=== Ship Movment Setting ===")]
    [SerializeField]
    private float yawTorgue = 500f;
    [SerializeField]
    private float pitchTorgue = 1000f;
    [SerializeField]
    private float rollTorgue = 1000f;

    [SerializeField]
    private float frontThrust = 100f;
    [SerializeField]
    private float maxFrontSpeed = 100f;
    [SerializeField]
    private float backThrust = 30f;
    [SerializeField]
    private float maxBackwardSpeed = 100f;
    [SerializeField]
    private float upDownThrust = 50f;
    [SerializeField]
    private float maxUpDownSpeed = 50f;
    [SerializeField]
    private float strafeThrust = 50f;
    [SerializeField]
    private float maxStrafeSpeed = 50f;

    [SerializeField]
    private float thrustIncrement = 0.01f;

    [SerializeField, Range(0.001f, 0.999f)]
    private float frontThrustReduction = 0.99f;

    [SerializeField, Range(0.001f, 0.999f)]
    private float backThrustReduction = 0.5f;

    [SerializeField, Range(0.001f, 0.999f)]
    private float upDownThrustReduction = 0.1f;

    [SerializeField, Range(0.001f, 0.999f)]
    private float leftRightThrustReduction = 0.1f;

    float glide = 0f;

    private Rigidbody rb;
    PlayerInputActions _playerInputActions;


    // testy

    public Vector3 mov;
    public Vector3 rota;
    public float forwardo;
    public float frontmove;
    public Vector3 movforce;

    //input values

    private float thrust1D = 0;
    private float upDown1D = 0;
    private float strafe1D = 0;

    private float roll1D;
    private Vector2 pitchYaw;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        _playerInputActions = new PlayerInputActions();
        _playerInputActions.ShipController.Enable();

    }



    private void Start()
    {

    }

    private void FixedUpdate()
    {
        GetMovmentInput();
        GetRotationInput();

        Vector3 rotationVector = RotationVector();
        rb.AddRelativeTorque(rotationVector * Time.deltaTime);



        Vector3 movmentVector = MovmentVector();
        rb.AddRelativeForce(movmentVector * Time.deltaTime, ForceMode.Force);
        movforce = movmentVector;
    }



    private Vector3 RotationVector()
    {
        float pitch = pitchYaw.x * pitchTorgue;
        float yaw = pitchYaw.y * yawTorgue;
        float roll = roll1D * rollTorgue * (-1);

        return new Vector3(pitch, yaw, roll);
    }

    private Vector3 MovmentVector()
    {
        Vector3 velocity = rb.velocity;
        float stafeDelta = maxStrafeSpeed - velocity.magnitude;
        float stafe = strafe1D * strafeThrust;

        float upDown = upDown1D * upDownThrust;


        float thrust = 0;

        float forwardSpeedDelta = thrust1D * maxFrontSpeed - velocity.z;
        if (Mathf.Abs(0 - forwardSpeedDelta) < 0.1f && thrust1D != 0f)
        {
            forwardSpeedDelta = 0f;
            Debug.Log("1");
        }
        else if (Mathf.Abs(0 - forwardSpeedDelta) < 0.1f && thrust1D == 0f && velocity.z != 0)
        {
            forwardSpeedDelta = 0f;
            float forceZ = rb.mass * velocity.z / 1f;
            rb.AddRelativeForce(new Vector3(0.0f, 0.0f, -forceZ), ForceMode.Impulse);
            thrust = 0;
            Debug.Log("2");
        }

        if (forwardSpeedDelta > 0)
        {
            thrust = frontThrust;
            frontmove = 1;
        }
        else if (forwardSpeedDelta < 0)
        {
            frontmove = -1;
            thrust = backThrust * (-1);

        }
        else
        {
            frontmove = 0;
        }
        forwardo = thrust;

        return new Vector3(stafe, upDown, thrust);
    }


    private void GetMovmentInput()
    {

        var movment = _playerInputActions.ShipController.Movment.ReadValue<Vector3>();
        mov = movment;
        thrust1D = HandleThrust(movment.z);
        upDown1D = movment.y;
        strafe1D = movment.x;

    }

    private float HandleThrust(float z)
    {
        float thrust = Mathf.Clamp((thrust1D + (z * thrustIncrement)), -1f, 1f);

        UIController.Instance.SetThrustSlider(thrust);
        return thrust;
    }

    public void ThrustStop(InputAction.CallbackContext context) //odwo³anie z unity player controller
    {
        if (context.performed)
        {
            thrust1D = 0;
        }

    }

    private void GetRotationInput()
    {
        var rotation = _playerInputActions.ShipController.Rotation.ReadValue<Vector3>();
        rota = rotation;
        roll1D = rotation.z;
        pitchYaw = new Vector2(rotation.y, rotation.x);


    }


}



