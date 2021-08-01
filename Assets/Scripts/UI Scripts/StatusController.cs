using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusController : MonoBehaviour
{
    //체력
    [SerializeField]
    private int hp;

    private int currentHP;

    //스테미나
    [SerializeField]
    private int sp;

    private int currentSP;

    [SerializeField]
    private int spIncreaseSpeed;

    [SerializeField]
    private int spRechargeTime;

    private int currentSprechargeTime;

    //스테미나 감소 여부
    private bool spUsed;

    [SerializeField]
    private int dp;

    private int currentDP;

    //배고픔
    [SerializeField]
    private int hunger;

    private int currentHunger;

    [SerializeField]
    private int hungerDecreaseTime;

    private int currentHungerDecreaseTime;

    //목마름
    [SerializeField]
    private int thirsty;

    private int currentThirsty;

    [SerializeField]
    private int thirstyDecreaseTime;

    private int currentThirstyDecreaseTime;

    //만족도
    [SerializeField]
    private int satisfy;

    private int currentSatisfy;

    [SerializeField]
    private Image[] image_Gauge;

    private const int HP = 0, DP = 1, SP = 2, HUNGER = 3, THIRSTY = 4, SATISFY = 5;

    private void Start()
    {
        currentHP = hp;
        currentDP = dp;
        currentThirsty = thirsty;
        currentHunger = hunger;
        currentSP = sp;
        currentSatisfy = satisfy;
    }

    private void Update()
    {
        Hunger();
        Thristy();
        GaugeUpdate();
        SpRechargeTime();
        SpRecover();
    }

    private void GaugeUpdate()
    {
        image_Gauge[HP].fillAmount = (float)currentHP / hp;
        image_Gauge[SP].fillAmount = (float)currentSP / sp;
        image_Gauge[DP].fillAmount = (float)currentDP / dp;
        image_Gauge[HUNGER].fillAmount = (float)currentHunger / hunger;
        image_Gauge[THIRSTY].fillAmount = (float)currentThirsty / thirsty;
        image_Gauge[SATISFY].fillAmount = (float)currentSatisfy / satisfy;
    }

    private void Hunger()
    {
        if (currentHunger > 0)
        {
            if (currentHungerDecreaseTime <= hungerDecreaseTime)
            {
                currentHungerDecreaseTime++;
            }
            else
            {
                currentHunger--;
                currentHungerDecreaseTime = 0;
            }
        }
        else
        {
            Debug.Log("hunger status reached ZERO");
        }
    }

    private void Thristy()
    {
        if (currentThirsty > 0)
        {
            if (currentThirstyDecreaseTime <= thirstyDecreaseTime)
            {
                currentThirstyDecreaseTime++;
            }
            else
            {
                currentThirsty--;
                currentThirstyDecreaseTime = 0;
            }
        }
        else
        {
            Debug.Log("Thirsty status reached ZERO");
        }
    }

    public void IncreaseThristy(int _count)
    {
        if (currentThirsty + _count < thirsty)
        {
            currentThirsty += _count;
        }
        currentThirsty -= thirsty;
    }

    public void DecreaseThristy(int _count)
    {
        if (currentThirsty - _count < 0)
        {
            currentThirsty = 0;
        }
        currentThirsty -= _count;
    }

    public void DecreaseHP(int _count)
    {
        if (currentHP > 0)
        {
            currentHP -= _count;
            return;
        }
        currentHP -= _count;

        if (currentHP <= 0)
        {
            Debug.Log("Player's HP has hit ZERO");
        }
    }

    public void IncreaseHP(int _count)
    {
        if (currentHP + _count < hp)
        {
            currentHP += _count;
        }
        else
        {
            currentHP = hp;
        }
    }

    public void IncreaseDP(int _count)
    {
        if (currentDP + _count < dp)
        {
            currentDP += _count;
        }
        else
        {
            currentDP = dp;
        }
    }

    public void IncreaseSP(int _count)
    {
        if (currentSP + _count < sp)
        {
            currentSP += _count;
        }
        else
        {
            currentSP = sp;
        }
    }

    public void IncreaseSATISFY(int _count)
    {
        if (currentSatisfy + _count < satisfy)
        {
            currentSatisfy += _count;
        }
        else
        {
            currentSatisfy = satisfy;
        }
    }

    public void DecreaseDP(int _count)
    {
        currentDP -= _count;
        if (currentDP <= 0)
        {
            Debug.Log("Player's DP has hit ZERO");
        }
    }

    public void IncreaseHunger(int _count)
    {
        if (currentHunger + _count < hunger)
        {
            currentHunger += _count;
        }
        else
        {
            currentHunger = hunger;
        }
    }

    public void DecreaseHunger(int _count)
    {
        if (currentHunger - _count < 0)
        {
            currentHunger = 0;
        }
        currentHunger -= _count;
    }

    public void DecreaseStamina(int _count)
    {
        spUsed = true;
        currentSprechargeTime = 0;

        if (currentSP - _count > 0)
        {
            currentSP -= _count;
        }
        else
        {
            currentSP = 0;
        }
    }

    public void SpRechargeTime()
    {
        if (spUsed)
        {
            if (currentSprechargeTime < spRechargeTime)
            {
                currentSprechargeTime++;
            }
            else
            {
                spUsed = false;
            }
        }
    }

    public void SpRecover()
    {
        if (!spUsed && currentSP < sp)
        {
            currentSP += spIncreaseSpeed;
        }
    }

    public int GetCurrentSP()
    {
        return currentSP;
    }
}