using UnityEngine;

public class PlayerController : MonoBehaviour
{
    const float TALL_RAYCAST = 1.01f;

    [Header("References")]
    [SerializeField] private GameObject playerCamera;

    [Header("Movement Variables")]
    [SerializeField] private float walkSpeed;
    [SerializeField] private float couchedSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float jumpForce;

    [Header("Other Movement Variables")]
    [SerializeField] private float crouchHeight;
    [SerializeField] private float charcterHeight;
    [SerializeField] private float crouchedCameraHeight;
    [SerializeField] private float gravityScale;
    private float inGravityScale;
    private float maxSpeed;

    [Header("State Variables")]
    public bool jumping;
    public bool crouched;
    public bool onGrounded;
    public string groundType = "";

    private GameInputs gameInputs;
    private float xRotation;
    private float yRotation;

    private RaycastHit downRayhit;
    private float downAngle;

    private Vector3 movement;

    private Rigidbody rb;
    private CapsuleCollider capsuleCol;

    private Vector3 camOffset;

    Vector3 vel = Vector3.zero;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        gameInputs = GetComponent<GameInputs>();
        gameInputs.xRotation = playerCamera.transform.rotation.eulerAngles.x;
        gameInputs.yRotation = transform.rotation.eulerAngles.y;

        camOffset = transform.position - playerCamera.transform.position;

        rb = GetComponent<Rigidbody>();
        capsuleCol = GetComponent<CapsuleCollider>();
    }

    private void FixedUpdate()
    {
        rb.AddForce(Vector3.down * inGravityScale, ForceMode.Acceleration);

        if(rb.linearVelocity.magnitude > maxSpeed)
        {
            if (onGrounded)
                rb.AddForce(-rb.linearVelocity.normalized * 40, ForceMode.Force);
            else
                rb.AddForce(- new Vector3(rb.linearVelocity.normalized.x, 0, rb.linearVelocity.normalized.z) * 2, ForceMode.Force);
        }
        else
        {
            rb.AddForce(Vector3.ProjectOnPlane(movement, downRayhit.normal).normalized * 40, ForceMode.Force);
        }
    }

    void Update()
    {
        movement = transform.forward * gameInputs.yAxisValue + transform.right * gameInputs.xAxisValue;
        
        xRotation = gameInputs.xRotation;
        yRotation = gameInputs.yRotation;

        if (crouched)
            maxSpeed = couchedSpeed;
        else if (gameInputs.run.IsPressed())
            maxSpeed = runSpeed;
        else
            maxSpeed = walkSpeed;

        InteractVoid();
        CameraFunction();
        JumpVoid();
        DownRaycast();
        CrouchVoid();
    }

    private void JumpVoid()
    {
        if (gameInputs.jump.WasPressedThisFrame() && onGrounded)
        {
            rb.AddForce(new Vector3(0, 1, 0) * jumpForce, ForceMode.Impulse);
        }
    }

    private void InteractVoid()
    {

        if (gameInputs.interact.WasPressedThisFrame()) 
        {
            Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 3f))
            {
                InteractableObject interactScript = hit.collider.GetComponent<InteractableObject>();

                if (interactScript != null)
                {
                    interactScript.Interact();
                    Debug.Log("Interacted with: " + hit.collider.name);
                }
                else
                {
                    Debug.Log("Object has no InteractableObject script.");
                }
            }
            else
            {
                Debug.Log("Nothing in front to interact with.");
            }
        }
    }


    private void CrouchVoid()
    {
        crouched = (gameInputs.crouch.IsPressed());
        capsuleCol.height = charcterHeight - (crouched?crouchHeight:0);
        capsuleCol.center = new Vector3(0, - (crouched?(crouchHeight / 2):0), 0);

    }

    private void CameraFunction()
    {
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, xRotation, transform.rotation.eulerAngles.z);
        playerCamera.transform.rotation = Quaternion.Slerp(playerCamera.transform.rotation, Quaternion.Euler(yRotation, transform.rotation.eulerAngles.y, 0), 0.4f);
        playerCamera.transform.position = Vector3.SmoothDamp(playerCamera.transform.position, transform.position - camOffset + new Vector3(0,crouched?-crouchedCameraHeight:0,0), ref vel, 0.05f);
    }

    private void DownRaycast()
    {
        onGrounded = Physics.Raycast(transform.position, Vector3.down, out downRayhit, charcterHeight / 2 + 0.1f);
        if (onGrounded) groundType = downRayhit.collider.gameObject.tag;

        Debug.DrawRay(transform.position, Vector3.down * (charcterHeight/2 + 0.1f), Color.green);
        Debug.DrawRay(transform.position, Vector3.ProjectOnPlane(movement, downRayhit.normal).normalized, Color.green);

        downAngle = Vector3.Angle(Vector3.up, downRayhit.normal);

        if (onGrounded && gameInputs.xAxisValue == 0 && gameInputs.yAxisValue == 0)
        {
            rb.linearVelocity = Vector3.zero;
        }

        if (onGrounded && downAngle > 0)
        {
            inGravityScale = 0;
            rb.useGravity = false;
        }
        else
        {
            inGravityScale = gravityScale;
            rb.useGravity = true;
        }

    }

    private void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 100, 20), "speed: "+rb.linearVelocity.magnitude);
        GUI.Label(new Rect(10, 60, 100, 20), "ground: "+groundType);
    }
}
