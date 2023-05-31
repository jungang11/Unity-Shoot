using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class PlayerMover : MonoBehaviour
{
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float jumpForce;

    private CharacterController controller;
    private Animator anim;

    private Vector3 moveDir;

    private float ySpeed = 0;
    private float moveSpeed;    // 현재 이동속도

    private bool isWalking;
    private bool isGrounded;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
    }
    
    private void Update()
    {
        Move();
        Jump();
    }

    private void FixedUpdate()
    {
        GroundCheck();
    }

    private void Move()
    {
        // 안움직임
        if(moveDir.magnitude == 0)
        {
            moveSpeed = Mathf.Lerp(moveSpeed, 0, 0.5f);
        }
        else if (isWalking)
        {
            moveSpeed = Mathf.Lerp(moveSpeed, walkSpeed, 0.5f);
        }
        else
        {
            moveSpeed = Mathf.Lerp(moveSpeed, runSpeed, 0.5f);
        }

        // 로컬 기준
        controller.Move(transform.forward * moveDir.z * moveSpeed * Time.deltaTime);
        controller.Move(transform.right * moveDir.x * moveSpeed * Time.deltaTime);

        anim.SetFloat("XSpeed", moveDir.x, 0.1f, Time.deltaTime);
        anim.SetFloat("YSpeed", moveDir.z, 0.1f, Time.deltaTime);
        anim.SetFloat("Speed", moveSpeed);
    }

    private void OnMove(InputValue value)
    {
        Vector2 input = value.Get<Vector2>();
        moveDir = new Vector3(input.x, 0, input.y);
    }

    private void Jump()
    {
        // 부드러운 점프
        ySpeed += Physics.gravity.y * Time.deltaTime;

        if (isGrounded && ySpeed < 0)
        {
            ySpeed = -1;
        }

        controller.Move(Vector3.up * ySpeed * Time.deltaTime);
    }

    private void OnJump(InputValue value)
    {
        if (isGrounded)
        {
            ySpeed = jumpForce;
        }
    }

    private void GroundCheck()
    {
        RaycastHit hit;
        // SphereCast -> 직선 레이저가 아니라 원 모양 레이저로 판단
        isGrounded = Physics.SphereCast(transform.position + Vector3.up * 1, 0.5f, Vector3.down, out hit, 0.6f);
    }

    private void OnWalk(InputValue value)
    {
        isWalking = value.isPressed;
    }
}
