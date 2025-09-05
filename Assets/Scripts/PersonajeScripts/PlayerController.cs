using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float PlayerSpeed = 5f;
    public float gravity = 9.8f;

    private CharacterController player;
    private Vector3 movePlayer;
    private Vector3 camForward;
    private Vector3 camRight;
    private float fallVelocity;

    public Camera mainCamera;

    void Awake()
    {
        player = GetComponent<CharacterController>();

        // Posicionar la cápsula sobre el piso
        // Calculamos la posición inicial usando el center que Unity asigna automáticamente
        Vector3 pos = transform.position;
        pos.y = player.center.y; // Base de la cápsula en Y=0
        transform.position = pos;
    }

    void Update()
    {
        // Input
        float horizontalMove = Input.GetAxis("Horizontal");
        float verticalMove = Input.GetAxis("Vertical");
        Vector3 playerInput = new Vector3(horizontalMove, 0, verticalMove);
        playerInput = Vector3.ClampMagnitude(playerInput, 1);

        // Movimiento relativo a la cámara
        camForward = mainCamera.transform.forward;
        camRight = mainCamera.transform.right;
        camForward.y = 0;
        camRight.y = 0;
        camForward.Normalize();
        camRight.Normalize();

        movePlayer = playerInput.x * camRight + playerInput.z * camForward;
        movePlayer *= PlayerSpeed;

        if (movePlayer != Vector3.zero)
            transform.forward = movePlayer.normalized;

        // Gravedad
        if (player.isGrounded)
        {
            fallVelocity = -1f; // Mantener contacto con el suelo
        }
        else
        {
            fallVelocity -= gravity * Time.deltaTime;
        }

        movePlayer.y = fallVelocity;

        // Mover jugador
        player.Move(movePlayer * Time.deltaTime);
    }
}
