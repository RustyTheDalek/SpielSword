/// <summary>
/// Timer class that helps with the timing of objects
/// </summary>
using UnityEngine;

public class Timer : MonoBehaviour
{
    //helps identify timeer
    public string ID;
    //Timer counter and maximum time
    public float timer, MAXTime;

    //Whether the timer starts immediately
    public bool startOnAwake;


    public bool countsDown, //If true counts down else count up
                active = false, //Whether the timer is running
                complete = false; //Is the timer finished

    //Constructor for the timer
    public void Setup(string _ID, float _MaxTime, bool _CountsDown)
    {
        ID = _ID;
        MAXTime = _MaxTime;
        countsDown = _CountsDown;
    }

    //Activates timer
    public void StartTimer()
    {
        if (countsDown)
            timer = MAXTime;
        else
            timer = 0;

        active = true;
        complete = false;
    }

    public void Start()
    {
        if(startOnAwake)
        {
            StartTimer();
        }
    }

    public void Reset()
    {
        if(countsDown)
        {
            timer = MAXTime;
        }
        else
        {
            timer = 0;
        }

        complete = false;
    }

    public void Update()
    {
        if(active)
        {
            if (countsDown)
            {
                if (timer <= 0)
                {
                    active = false;
                    complete = true;
                    timer = MAXTime;
                }
                else
                {
                    timer -= Time.deltaTime;
                }
            }
            else
            {
                if (timer >= MAXTime)
                {
                    active = false;
                    complete = true;
                    timer = 0;
                }
                else
                {
                    timer += Time.deltaTime;
                }
            }

        }
    }
}
