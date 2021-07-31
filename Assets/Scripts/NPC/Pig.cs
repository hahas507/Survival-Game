using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pig : MonoBehaviour
{
    [SerializeField]
    private string animalName; //동물이름

    [SerializeField]
    private int hp; //동물 체력

    [SerializeField]
    private float walkSpeed; //동물 걷기속도

    private Vector3 direction;

    private bool isWalking;
    private bool isAction;

    [SerializeField]
    private float walkTime; //걷는시간

    [SerializeField]
    private float waitTime; //대기시간

    private float currentTime;

    [SerializeField]
    private Animator anim;

    [SerializeField]
    private Rigidbody rig;

    [SerializeField]
    private BoxCollider boxCol;

    // Start is called before the first frame update
    private void Start()
    {
        currentTime = waitTime;
        isAction = true;
    }

    // Update is called once per frame
    private void Update()
    {
        Move();
        Rotation();
        ElapseTime();
    }

    private void Rotation()
    {
        if (isWalking)
        {
            Vector3 _rotation = Vector3.Lerp(transform.eulerAngles, direction, 0.01f);
            rig.MoveRotation(Quaternion.Euler(_rotation));
        }
    }

    private void Move()
    {
        if (isWalking)
        {
            rig.MovePosition(transform.position + transform.forward * walkSpeed * Time.deltaTime);
        }
    }

    private void ElapseTime()
    {
        if (isAction)
        {
            currentTime -= Time.deltaTime;
            if (currentTime <= 0)
            {
                ActionReset();
            }
        }
    }

    private void ActionReset()
    {
        isWalking = false;
        isAction = true;
        anim.SetBool("Walking", isWalking);
        direction.Set(0f, UnityEngine.Random.Range(0f, 360f), 0f);
        RandomAction();
    }

    private void RandomAction()
    {
        int _random = UnityEngine.Random.Range(0, 4);

        if (_random == 0)
        {
            Wait();
        }
        else if (_random == 1)
        {
            Eat();
        }
        else if (_random == 2)
        {
            Peek();
        }
        else if (_random == 3)
        {
            TryWalk();
        }
    }

    private void Wait()
    {
        currentTime = waitTime;
        Debug.Log("wait");
    }

    private void Eat()
    {
        currentTime = waitTime;
        anim.SetTrigger("Eat");
        Debug.Log("eat");
    }

    private void Peek()
    {
        currentTime = waitTime;
        anim.SetTrigger("Peek");
        Debug.Log("peek");
    }

    private void TryWalk()
    {
        isWalking = true;
        anim.SetBool("Walking", isWalking);
        currentTime = walkTime;
        Debug.Log("walk");
    }
}