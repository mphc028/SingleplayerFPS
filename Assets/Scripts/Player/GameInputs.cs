using UnityEngine;
using UnityEngine.InputSystem;

public class GameInputs : MonoBehaviour
{
    // Actions
    private InputAction xAxis;
    private InputAction yAxis;
    private InputAction camRotX;
    private InputAction camRotY;
    [HideInInspector] public InputAction jump;
    [HideInInspector] public InputAction crouch;
    [HideInInspector] public InputAction run;
    [HideInInspector] public InputAction interact;

    // Rotation
    [HideInInspector] public float xRotation;
    [HideInInspector] public float yRotation;

    // Input Values
    [HideInInspector] public float xAxisValue;
    [HideInInspector] public float yAxisValue;
    [HideInInspector] public float camRotXValue;
    [HideInInspector] public float camRotYValue;

    [Header("Input Configs")]
    public float camRotationSpeed;

    void Start()
    {
        // Get Inputs
        xAxis = GetComponent<PlayerInput>().actions.FindAction("xAxis");
        yAxis = GetComponent<PlayerInput>().actions.FindAction("yAxis");
        camRotX = GetComponent<PlayerInput>().actions.FindAction("CamRotX");
        camRotY = GetComponent<PlayerInput>().actions.FindAction("CamRotY");
        jump = GetComponent<PlayerInput>().actions.FindAction("Jump");
        crouch = GetComponent<PlayerInput>().actions.FindAction("Crouch");
        run = GetComponent<PlayerInput>().actions.FindAction("Run");
        interact = GetComponent<PlayerInput>().actions.FindAction("Interact");
    }

    void Update()
    {
        // Read Input Values
        xAxisValue = xAxis.ReadValue<float>();
        yAxisValue = yAxis.ReadValue<float>();
        camRotXValue = camRotX.ReadValue<float>();
        camRotYValue = camRotY.ReadValue<float>();
        
        xRotation += camRotXValue * camRotationSpeed * Time.deltaTime;
        yRotation += camRotYValue * camRotationSpeed * Time.deltaTime;
        yRotation = Mathf.Clamp(yRotation, -90, 90);
    }
}
