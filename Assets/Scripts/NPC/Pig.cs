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

    [SerializeField]
    private float runSpeed;

    private float applySpeed;

    private Vector3 direction;

    private bool isWalking;
    private bool isAction;
    private bool isRunning;
    private bool isDead;

    [SerializeField]
    private float walkTime; //걷는시간

    [SerializeField]
    private float waitTime; //대기시간

    [SerializeField]
    private float runTime;//뛰기시간

    private float currentTime;

    [SerializeField]
    private Animator anim;

    [SerializeField]
    private Rigidbody rig;

    [SerializeField]
    private BoxCollider boxCol;

    private AudioSource theAudio;

    [SerializeField]
    private AudioClip[] sound_pig_normal;

    [SerializeField]
    private AudioClip sound_pig_hurt;

    [SerializeField]
    private AudioClip sound_pig_dead;

    // Start is called before the first frame update
    private void Start()
    {
        theAudio = GetComponent<AudioSource>();
        currentTime = waitTime;
        isAction = true;
    }

    // Update is called once per frame
    private void Update()
    {
        if (!isDead)
        {
            Move();
            Rotation();
            ElapseTime();
        }
    }

    private void Rotation()
    {
        if (isWalking || isRunning)
        {
            Vector3 _rotation = Vector3.Lerp(transform.eulerAngles, new Vector3(0f, direction.y, 0f), 0.01f);
            rig.MoveRotation(Quaternion.Euler(_rotation));
        }
    }

    private void Move()
    {
        if (isWalking || isRunning)
        {
            rig.MovePosition(transform.position + transform.forward * applySpeed * Time.deltaTime);
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
        isRunning = false;

        applySpeed = walkSpeed;

        anim.SetBool("Walking", isWalking);
        anim.SetBool("Running", isRunning);

        direction.Set(0f, UnityEngine.Random.Range(0f, 360f), 0f);
        RandomAction();
    }

    private void RandomAction()
    {
        RandomSound();
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
        applySpeed = walkSpeed;
        Debug.Log("walk");
    }

    private void Run(Vector3 targetPos)
    {
        direction = Quaternion.LookRotation(transform.position - targetPos).eulerAngles; //오
        currentTime = runTime;
        isWalking = false;
        isRunning = true;
        applySpeed = runSpeed;
        anim.SetBool("Running", isRunning);
    }

    public void Damage(int _dmg, Vector3 targetPos)
    {
        if (!isDead)
        {
            hp -= _dmg;
            if (hp < 0)
            {
                Dead();
                return;
            }

            PlaySE(sound_pig_hurt);
            anim.SetTrigger("Hurt");
            Run(targetPos);
        }
    }

    private void Dead()
    {
        PlaySE(sound_pig_dead);
        isWalking = false;
        isRunning = false;
        anim.SetTrigger("Dead");
    }

    private void RandomSound()
    {
        int _random = UnityEngine.Random.Range(0, 3); //일상 사운드 3개
        PlaySE(sound_pig_normal[_random]);
    }

    private void PlaySE(AudioClip _clip)
    {
        theAudio.clip = _clip;
        theAudio.Play();
    }
}