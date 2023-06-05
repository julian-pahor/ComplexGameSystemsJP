using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("AudioManager/AFEvent")]
public class AFEvent : MonoBehaviour
{
    public AudioFileBundle targetBundle;



    [SerializeField]
    private List<CallType> playCalls = new List<CallType>();


    private void Start()
    {
        if(playCalls.Contains(CallType.OnStart))
        {
            targetBundle.FireAudio(this.transform);
        }
    }

    private void Awake()
    {
        if (playCalls.Contains(CallType.OnAwake))
        {
            targetBundle.FireAudio(this.transform);
        }
    }

    private void OnDisable()
    {
        if (playCalls.Contains(CallType.OnDisable))
        {
            targetBundle.FireAudio(this.transform);
        }
    }

    private void OnDestroy()
    {
        if (playCalls.Contains(CallType.OnDestroy))
        {
            targetBundle.FireAudio(this.transform);
        }
    }

    private void OnEnable()
    {
        if (playCalls.Contains(CallType.OnEnable))
        {
            targetBundle.FireAudio(this.transform);
        }
    }

    public void FireAudio()
    {
        targetBundle.FireAudio(this.transform);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (playCalls.Contains(CallType.OnColliderEnter))
        {
            targetBundle.FireAudio(this.transform);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (playCalls.Contains(CallType.OnColliderExit))
        {
            targetBundle.FireAudio(this.transform);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (playCalls.Contains(CallType.OnTriggerEnter))
        {
            targetBundle.FireAudio(this.transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (playCalls.Contains(CallType.OnTriggerExit))
        {
            targetBundle.FireAudio(this.transform);
        }
    }

    ///Collision / Trigger

    //Play (General UnityEvent)
}
