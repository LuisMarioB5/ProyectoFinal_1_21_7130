using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Configuration")]
    public float moveSpeed = 6f;
    public float gravity = 9.8f;

    private CharacterController controller;
    private Camera mainCamera;
    private Animator animator;

    private Vector3 velocity;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        mainCamera = Camera.main;
    }

    void Update()
    {
        MovePlayer();
        RotateTowardsMouse();
    }

    void MovePlayer()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 move = transform.right * horizontal + transform.forward * vertical;
        Vector3 moveGlobal = new Vector3(horizontal, 0, vertical).normalized;

        controller.Move(moveGlobal * moveSpeed * Time.deltaTime);

        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        velocity.y -= gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // if (moveGlobal.magnitude >= 0.1f)
        // {
        //     animator.SetBool("isRunning", true);
        // }
        // else
        // {
        //     animator.SetBool("isRunning", false);
        // }
    }

    void RotateTowardsMouse()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        float rayLength;

        if (groundPlane.Raycast(ray, out rayLength))
        {
            Vector3 pointToLook = ray.GetPoint(rayLength);
            Vector3 lookPosition = new Vector3(pointToLook.x, transform.position.y, pointToLook.z);
            transform.LookAt(lookPosition);
        }
    }
}
