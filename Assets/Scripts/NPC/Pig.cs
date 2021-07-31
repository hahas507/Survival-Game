using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pig : WeakAnimal
{
    protected override void ActionReset()
    {
        base.ActionReset();
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
}