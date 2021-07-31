using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Animal : MonoBehaviour
{
    [SerializeField]
    protected string animalName; //동물이름

    [SerializeField]
    protected int hp; //동물 체력

    [SerializeField]
    protected float walkSpeed; //동물 걷기속도

    [SerializeField]
    protected float runSpeed;

    protected Vector3 destination;

    protected bool isWalking;
    protected bool isAction;
    protected bool isRunning;
    protected bool isDead;

    [SerializeField]
    protected float walkTime; //걷는시간

    [SerializeField]
    protected float waitTime; //대기시간

    [SerializeField]
    protected float runTime;//뛰기시간

    protected float currentTime;

    [SerializeField]
    protected Animator anim;

    [SerializeField]
    protected Rigidbody rig;

    [SerializeField]
    protected BoxCollider boxCol;

    protected AudioSource theAudio;
    protected NavMeshAgent nav;

    [SerializeField]
    protected AudioClip[] sound_normal;

    [SerializeField]
    protected AudioClip sound_hurt;

    [SerializeField]
    protected AudioClip sound_dead;

    private void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        theAudio = GetComponent<AudioSource>();
        currentTime = waitTime;
        isAction = true;
    }

    private void Update()
    {
        if (!isDead)
        {
            Move();
            ElapseTime();
        }
    }

    protected void Move()
    {
        if (isWalking || isRunning)
        {
            //rig.MovePosition(transform.position + transform.forward * applySpeed * Time.deltaTime);
            nav.SetDestination(transform.position + destination * 5f);
        }
    }

    protected void ElapseTime()
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

    protected virtual void ActionReset()
    {
        isWalking = false;
        isAction = true;
        isRunning = false;

        nav.speed = walkSpeed;

        nav.ResetPath();

        anim.SetBool("Walking", isWalking);
        anim.SetBool("Running", isRunning);

        destination.Set(Random.Range(-0.2f, 0.2f), 0f, Random.Range(0.5f, 1f));
    }

    protected void TryWalk()
    {
        isWalking = true;
        anim.SetBool("Walking", isWalking);
        currentTime = walkTime;
        nav.speed = walkSpeed;
        Debug.Log("walk");
    }

    public virtual void Damage(int _dmg, Vector3 targetPos)
    {
        if (!isDead)
        {
            hp -= _dmg;
            if (hp <= 0)
            {
                Dead();
                return;
            }

            PlaySE(sound_hurt);
            anim.SetTrigger("Hurt");
        }
    }

    protected void Dead()
    {
        isDead = true;
        PlaySE(sound_dead);
        isWalking = false;
        isRunning = false;
        anim.SetTrigger("Dead");
    }

    protected void RandomSound()
    {
        int _random = UnityEngine.Random.Range(0, 3); //일상 사운드 3개
        PlaySE(sound_normal[_random]);
    }

    protected void PlaySE(AudioClip _clip)
    {
        theAudio.clip = _clip;
        theAudio.Play();
    }
}