using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerMoving : MonoBehaviour
{
    [SerializeField] private CharacterController player;
    [SerializeField] private float speed;
    [SerializeField] private Vector3 move;
    [SerializeField] private Vector3 velocity;
    [SerializeField] private bool isGrounded;
    [SerializeField] private Transform pos;
    [SerializeField] private LayerMask ground;
    [SerializeField] private float rateCollide = 0.1f;
    [SerializeField] private float gravity = -40.0f;
    [SerializeField] private float jumpHeight = 2.0f;
    [SerializeField] private InputAction jump;
    [SerializeField] private InputAction movement;
    [SerializeField] private Animator animation;
    [SerializeField] private Animator anim;


    private void Awake()
    {
        this.player = GetComponent<CharacterController>();
        this.pos = GameObject.Find("GroundCheck").transform;
        //this.animation = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        this.jump = new InputAction("jump", binding: "<keyboard>/space");
        this.jump.AddBinding("<Gamepad>/a");
        this.movement = new InputAction("playerMovement", binding: "<Gamepad>/leftstick");
        this.movement.AddCompositeBinding("Dpad")
        .With("Up", "<keyboard>/w")
        .With("Down", "<keyboard>/s")
        .With("Left", "<keyboard>/a")
        .With("Right", "<keyboard>/d");

        this.jump.Enable();
        this.movement.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        this.ResultGround();
    }
    void FixedUpdate()
    {
        this.UpdateMove();
    }
    private void UpdateMove()
    {
        //float x = Input.GetAxis("Horizontal");
        //float z = Input.GetAxis("Vertical");
        float x = movement.ReadValue<Vector2>().x;
        float z = movement.ReadValue<Vector2>().y;
        move = transform.right * x + transform.forward * z;
        // if (Mathf.Approximately(this.movement.ReadValue<Vector2>().x, 1) ||
        //     Mathf.Approximately(this.movement.ReadValue<Vector2>().y, 1)
        // || Mathf.Approximately(this.movement.ReadValue<Vector2>().x, -1)
        // || Mathf.Approximately(this.movement.ReadValue<Vector2>().y, -1)
        //  )
        // {
        //     this.animation.SetFloat("move", 0.2f);
        // }
        // else
        // {
        //     this.animation.SetFloat("move", 0);
        // }

        this.animation.SetFloat("move", Mathf.Abs(x) + Mathf.Abs(z));
        this.anim.SetFloat("run", Mathf.Abs(x) + Mathf.Abs(z));
        //move = transform.right * x + transform.forward * z;
        this.player.Move(move * Time.deltaTime * this.speed);
    }
    private void ResultGround()
    {
        this.isGrounded = Physics.CheckSphere(this.pos.position, rateCollide, ground);
        if (isGrounded && velocity.y < 0)
        {
            this.velocity.y = -1.0f;
        }
        if (isGrounded)
        {
            if (Mathf.Approximately(this.jump.ReadValue<float>(), 1))
            {
                this.Jumping();
            }
        }
        else
        {
            velocity.y += gravity * Time.deltaTime;
        }
        this.player.Move(velocity * Time.deltaTime);
    }
    private void Jumping()
    {
        velocity.y = Mathf.Sqrt(jumpHeight * 2 * -gravity);
        //velocity.y = jumpHeight;
        //velocity.y=Mathf.Sqrt(2*2*40);
    }
}
