using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public float speed = 5f;
    public float mouseSensitivity = 2f;
    public float jumpForce = 5f;
    public CharacterController controller;
    public Animator animator;

    private Vector3 velocity;
    private float gravity = -9.81f;
    private bool isGrounded;

    public Transform cameraTransform;
    private float xRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // Bloquea cursor
    }

    void Update()
    {
        // --- Movimiento ---
        float x = Input.GetAxisRaw("Horizontal"); // izquierda/derecha
        float z = Input.GetAxisRaw("Vertical");   // adelante/atrás

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.deltaTime);

        // --- Gravedad y salto ---
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
            animator.SetBool("IsJumping", true);
        }
        else if (isGrounded)
        {
            animator.SetBool("IsJumping", false);
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // --- Animaciones de caminar ---
        animator.SetBool("IsWalking", z != 0);       // Adelante/atrás
        animator.SetBool("IsWalkingRight", x > 0);   // Derecha
        animator.SetBool("IsWalkingLeft", x < 0);    // Izquierda

        // --- Rotación de cámara ---
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }
}


