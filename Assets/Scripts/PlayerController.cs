﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float runSpeed;

    [SerializeField]
    private float walkSpeed;

    [SerializeField]
    private float crouchSpeed;

    private float applySpeed;

    [SerializeField]
    private float jumpForce;

    private bool isRun = false;
    private bool isWalk = false;
    private bool isGround = true;
    private bool isCrouch = false;

    //앉았을때 얼마나 앉을지
    [SerializeField]
    private float crouchPosY;

    private float originPosY;
    private float applyCrouchPosY;

    private CapsuleCollider capsuleCollider;

    [SerializeField]
    private float lookSensitivity;

    [SerializeField]
    private float cameraRotationLimit;

    private float currentCameraRotationX = 0f;

    [SerializeField]
    private Camera theCamera;

    private Rigidbody myRigid;
    private GunController theGunController;
    private Hand theHand;

    private void Start()
    {
        capsuleCollider = GetComponent<CapsuleCollider>();
        myRigid = GetComponent<Rigidbody>();
        theGunController = FindObjectOfType<GunController>();
        theHand = FindObjectOfType<Hand>();

        applySpeed = walkSpeed;
        originPosY = theCamera.transform.localPosition.y;
        applyCrouchPosY = originPosY;
    }

    private void Update()
    {
        IsGround();
        TryJump();
        TryRun();
        TryCrouch();
        Move();
        CameraRotation();
        CharacterRotation();
    }

    //앉기 시도
    private void TryCrouch()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            Crouch();
        }
    }

    //앉기
    private void Crouch()
    {
        isCrouch = !isCrouch;

        if (isCrouch)
        {
            applySpeed = crouchSpeed;
            applyCrouchPosY = crouchPosY;
        }
        else
        {
            applySpeed = walkSpeed;
            applyCrouchPosY = originPosY;
        }

        StartCoroutine(CrouchCoroutine());
    }

    //부드러운 앉기 동작
    private IEnumerator CrouchCoroutine()
    {
        float posY = theCamera.transform.localPosition.y;

        int count = 0;

        while (posY != applyCrouchPosY)
        {
            posY = Mathf.Lerp(posY, applyCrouchPosY, 0.3f);
            theCamera.transform.localPosition = new Vector3(0, posY, 0);

            if (count > 15)
                break;

            yield return null;
        }
        theCamera.transform.localPosition = new Vector3(0, crouchPosY, 0);
    }

    private void IsGround()
    {
        isGround = Physics.Raycast(transform.position, Vector3.down, capsuleCollider.bounds.extents.y + 0.1f);
    }

    private void TryJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            Jump();
        }
    }

    private void Jump()
    {
        //앉은상태에서 점프시 앉은상태 해제
        if (isCrouch)
        {
            Crouch();
        }
        myRigid.velocity = transform.up * jumpForce;
    }

    private void TryRun()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            Debug.Log("Running");
            Running();
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            Debug.Log("stopped running");
            RunningCancel();
        }
    }

    private void RunningCancel()
    {
        isRun = false;
        theHand.anim.SetBool("Run", isRun);
        applySpeed = walkSpeed;
    }

    private void Running()
    {
        if (isCrouch)
        {
            Crouch();
        }

        theGunController.CancelFineSight();

        isRun = true;
        theHand.anim.SetBool("Run", isRun);
        applySpeed = runSpeed;
    }

    private void Move()
    {
        isWalk = true;
        float moveDirX = Input.GetAxis("Horizontal");
        float moveDirZ = Input.GetAxis("Vertical");

        Vector3 moveHorizontal = transform.right * moveDirX;
        Vector3 moveVertical = transform.forward * moveDirZ;

        Vector3 velocity = (moveHorizontal + moveVertical).normalized * applySpeed;

        myRigid.MovePosition(transform.position + velocity * Time.deltaTime);

        if (moveDirX == 0 && moveDirZ == 0)
        {
            isWalk = false;
        }

        theHand.anim.SetBool("Walk", isWalk);
    }

    private void CameraRotation()
    {
        float xRotation = Input.GetAxisRaw("Mouse Y");
        float cameraRotationX = xRotation * lookSensitivity;
        currentCameraRotationX -= cameraRotationX;
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);

        theCamera.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
    }

    private void CharacterRotation()
    {
        float yRotation = Input.GetAxisRaw("Mouse X");
        Vector3 characterRotationY = new Vector3(0f, yRotation, 0f) * lookSensitivity;
        myRigid.MoveRotation(myRigid.rotation * Quaternion.Euler(characterRotationY));
    }
}