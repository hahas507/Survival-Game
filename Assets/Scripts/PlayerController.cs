using System;
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

    [SerializeField]
    private float swimSpeed;

    [SerializeField]
    private float swimFast;

    [SerializeField]
    private float swimUpSpeed;

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

    private Vector3 lastPos;

    [SerializeField]
    private Camera theCamera;

    private Rigidbody myRigid;
    private GunController theGunController;
    private MeleeWeapon theHand;
    private CrossHair theCrossHair;
    private StatusController theStatusController;

    private void Start()
    {
        capsuleCollider = GetComponent<CapsuleCollider>();
        myRigid = GetComponent<Rigidbody>();
        theGunController = FindObjectOfType<GunController>();
        theHand = FindObjectOfType<MeleeWeapon>();
        theCrossHair = FindObjectOfType<CrossHair>();

        applySpeed = walkSpeed;
        originPosY = theCamera.transform.localPosition.y;
        applyCrouchPosY = originPosY;
        theStatusController = FindObjectOfType<StatusController>();
    }

    private void Update()
    {
        if (GameManager.canPlayerMove)
        {
            WaterCheck();
            IsGround();
            TryJump();
            if (!GameManager.isWater)
            {
                TryRun();
            }
            TryCrouch();
            Move();
            MoveCheck();
            CameraRotation();
            CharacterRotation();
        }
    }

    private void WaterCheck()
    {
        if (GameManager.isWater)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                applySpeed = swimFast;
            }
            applySpeed = swimSpeed;
        }
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
        theCrossHair.CrouchAnimation(isCrouch);

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
        theCrossHair.JumpingAnimation(!isGround);
    }

    private void TryJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGround && theStatusController.GetCurrentSP() > 0 && !GameManager.isWater)
        {
            Jump();
        }
        else if (Input.GetKey(KeyCode.Space) && GameManager.isWater)
        {
            SwimUp();
        }
    }

    private void SwimUp()
    {
        myRigid.velocity = transform.up * swimUpSpeed;
    }

    private void Jump()
    {
        //앉은상태에서 점프시 앉은상태 해제
        if (isCrouch)
        {
            Crouch();
        }
        theStatusController.DecreaseStamina(100);
        myRigid.velocity = transform.up * jumpForce;
    }

    private void TryRun()
    {
        if (Input.GetKey(KeyCode.LeftShift) && theStatusController.GetCurrentSP() > 0)
        {
            Debug.Log("Running");
            Running();
            //Debug.Log("Key Down");
        }
        if (Input.GetKeyUp(KeyCode.LeftShift) || theStatusController.GetCurrentSP() <= 0)
        {
            Debug.Log("stopped running");
            RunningCancel();
            //Debug.Log("keyUP");
        }
    }

    private void RunningCancel()
    {
        isRun = false;
        theCrossHair.RunningAnimation(isRun);
        //theHand.anim.SetBool("Run", isRun);
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
        //theHand.anim.SetBool("Run", isRun);
        theCrossHair.RunningAnimation(isRun);
        theStatusController.DecreaseStamina(10);
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

        theCrossHair.WalkingAnimation(isWalk);
    }

    private void MoveCheck()
    {
        //if (!isRun && !isCrouch)
        //{
        //    if (Vector3.Distance(lastPos, transform.position) >= 0.01f)
        //    {
        //        isWalk = true;
        //        //Debug.Log(Vector3.Distance(lastPos, transform.position));
        //    }
        //    else
        //    { isWalk = false; }

        //    theCrossHair.WalkingAnimation(isWalk);
        //    lastPos = transform.position;
        //}
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