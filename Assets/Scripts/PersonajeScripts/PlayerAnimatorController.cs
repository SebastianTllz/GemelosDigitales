using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorController : MonoBehaviour
{
    [Header("Referencias")]
    public Animator animator;      // Animator del modelo humanoide
    public Rigidbody rb;           // Rigidbody del Player
    public bool grounded;          // Si el personaje está en el suelo

    [Header("Animaciones (Opcional, solo para referencia)")]
    public AnimationClip idleClip;
    public AnimationClip walkClip;
    public AnimationClip jumpClip;

    void Start()
    {
        // Busca el Animator si no se asignó manualmente
        if (!animator)
            animator = GetComponentInChildren<Animator>();

        // Busca Rigidbody si no se asignó
        if (!rb)
            rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        UpdateAnimatorParameters();
    }

    private void UpdateAnimatorParameters()
    {
        if (!animator || !rb) return;

        // Velocidad horizontal del personaje
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        float speed = flatVel.magnitude;

        // Actualiza parámetros del Animator
        animator.SetFloat("Speed", speed);    // Idle ↔ Walk/Run
        animator.SetBool("Grounded", grounded);
    }

    // Llamar desde tu script de movimiento cuando salta
    public void TriggerJump()
    {
        if (animator)
            animator.SetTrigger("Jump");
    }
}
