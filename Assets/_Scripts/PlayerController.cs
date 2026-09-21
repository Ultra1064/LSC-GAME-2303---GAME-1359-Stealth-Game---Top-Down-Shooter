using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    TS_Inputs _inputs; //Apparently the InputActions has to generate its own C# script and class to be able to be read. Or else it errors as there's no reference.
    CharacterController cc;

    [Header("Player Controls")]
    public float moveSpeed = 4f;
    [SerializeField] private Vector2 moveInput;
    [SerializeField] private Vector2 aimInput;

    [Header("Gamepad Setup")]
    PlayerInput pInput;
    [SerializeField] float controllerDeadZone = 0.1f;
    [SerializeField] float controllerRotateSmooth = 1000;

    public bool isGamepad;

    public bool isDead;

    private void Awake()
    {
        _inputs = new TS_Inputs();
        cc = GetComponent<CharacterController>();
    }

    private void OnEnable()
    {
        _inputs.Enable();
    }

    private void OnDisable()
    {
        _inputs.Disable();
    }

    private void Update()
    {
        HandleInputs();
        if (!isDead)
        {
            HandleMovement();
            HandleRotation();
        }
        
    }

    void HandleInputs()
    {
        moveInput = _inputs.Player.Movement.ReadValue<Vector2>();
        aimInput = _inputs.Player.Aiming.ReadValue<Vector2>();
    }

    void HandleMovement()
    {
        Vector3 move;
        move = new Vector3(moveInput.x, 0, moveInput.y).normalized;
        cc.Move(move * moveSpeed * Time.deltaTime);
    }

    void HandleRotation() //Need to look up this math/logic. I do not understand (for the second half) //Need to look up logic for first half as well, Quaternions
    {
        if (isGamepad)
        {
            if (Mathf.Abs(aimInput.x) > controllerDeadZone || Mathf.Abs(aimInput.y) > controllerDeadZone)
            {
                Vector3 playerDirection = (Vector3.right * aimInput.x) + (Vector3.forward * aimInput.y);
                if (playerDirection.sqrMagnitude > 0) //This just checks that the number is positive?
                {
                    Quaternion newRotation = Quaternion.LookRotation(playerDirection, Vector3.up);
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, newRotation, controllerRotateSmooth * Time.deltaTime);
                }
            }
        }
        else
        {
            //This uses a Raycast from the cursor onto the surface(floor/ground) to create a vector to the player.
            Ray ray = Camera.main.ScreenPointToRay(aimInput);
            Plane groundPlane = new Plane(Vector3.up, Vector3.zero);

            float rayDistance; //From our mouse position to the groundPlane

            if (groundPlane.Raycast(ray, out rayDistance))
            {
                Vector3 point = ray.GetPoint(rayDistance);
                LookAt(point);
            }
        }
        
    }

    void LookAt(Vector3 lookPoint)
    {
        Vector3 heightCorrectedPoint = new Vector3(lookPoint.x,
            transform.position.y, lookPoint.z);
        transform.LookAt(heightCorrectedPoint); //This LookAt is a function within the Transform class, it is NOT the same as this function here
    }

    public void OnDeviceChange(PlayerInput ctrl)
    {
        isGamepad = ctrl.currentControlScheme.Equals("Gamepad") ? true : false;
    }
}
