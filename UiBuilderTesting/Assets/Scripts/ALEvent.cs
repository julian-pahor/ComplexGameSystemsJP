using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ALEvent : MonoBehaviour
{
    public AudioLoopBundle targetBundle;

    [SerializeField]
    private CallType playCall;
    [SerializeField] 
    private CallType stopCall;
    [SerializeField]
    private CallType pauseCall;
    [SerializeField]
    private CallType resumeCall;

    private void Start()
    {
        if(playCall == CallType.OnStart)
        {
            targetBundle.FireAudio(this.transform);
        }
        if(stopCall == CallType.OnStart)
        {
            targetBundle.StopAudio();
        }
        if(pauseCall == CallType.OnStart)
        {
            targetBundle.PauseAudio();
        }
        if(resumeCall == CallType.OnStart)
        {
            targetBundle.ResumeAudio();
        }
    }

    private void Awake()
    {
        if (playCall == CallType.OnAwake)
        {
            targetBundle.FireAudio(this.transform);
        }
        if (stopCall == CallType.OnAwake)
        {
            targetBundle.StopAudio();
        }
        if (pauseCall == CallType.OnAwake)
        {
            targetBundle.PauseAudio();
        }
        if (resumeCall == CallType.OnAwake)
        {
            targetBundle.ResumeAudio();
        }
    }

    private void OnEnable()
    {
        if (playCall == CallType.OnEnable)
        {
            targetBundle.FireAudio(this.transform);
        }
        if (stopCall == CallType.OnEnable)
        {
            targetBundle.StopAudio();
        }
        if (pauseCall == CallType.OnEnable)
        {
            targetBundle.PauseAudio();
        }
        if (resumeCall == CallType.OnEnable)
        {
            targetBundle.ResumeAudio();
        }
    }

    private void OnDisable()
    {
        if (playCall == CallType.OnDisable)
        {
            targetBundle.FireAudio(this.transform);
        }
        if (stopCall == CallType.OnDisable)
        {
            targetBundle.StopAudio();
        }
        if (pauseCall == CallType.OnDisable)
        {
            targetBundle.PauseAudio();
        }
        if (resumeCall == CallType.OnDisable)
        {
            targetBundle.ResumeAudio();
        }
    }

    private void OnDestroy()
    {
        if (playCall == CallType.OnDestroy)
        {
            targetBundle.FireAudio(this.transform);
        }
        if (stopCall == CallType.OnDestroy)
        {
            targetBundle.StopAudio();
        }
        if (pauseCall == CallType.OnDestroy)
        {
            targetBundle.PauseAudio();
        }
        if (resumeCall == CallType.OnDestroy)
        {
            targetBundle.ResumeAudio();
        }
    }

    public void FireLoop()
    {
        targetBundle.FireAudio(this.transform);
    }

    public void StopLoop()
    {
        targetBundle.StopAudio();
    }

    public void PauseLoop()
    {
        targetBundle.PauseAudio();
    }
    public void ResumeLoop()
    {
        targetBundle.ResumeAudio();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (playCall == CallType.OnColliderEnter)
        {
            targetBundle.FireAudio(this.transform);
        }
        if (stopCall == CallType.OnColliderEnter)
        {
            targetBundle.StopAudio();
        }
        if (pauseCall == CallType.OnColliderEnter)
        {
            targetBundle.PauseAudio();
        }
        if (resumeCall == CallType.OnColliderEnter)
        {
            targetBundle.ResumeAudio();
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (playCall == CallType.OnColliderExit)
        {
            targetBundle.FireAudio(this.transform);
        }
        if (stopCall == CallType.OnColliderExit)
        {
            targetBundle.StopAudio();
        }
        if (pauseCall == CallType.OnColliderExit)
        {
            targetBundle.PauseAudio();
        }
        if (resumeCall == CallType.OnColliderExit)
        {
            targetBundle.ResumeAudio();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (playCall == CallType.OnTriggerEnter)
        {
            targetBundle.FireAudio(this.transform);
        }
        if (stopCall == CallType.OnTriggerEnter)
        {
            targetBundle.StopAudio();
        }
        if (pauseCall == CallType.OnTriggerEnter)
        {
            targetBundle.PauseAudio();
        }
        if (resumeCall == CallType.OnTriggerEnter)
        {
            targetBundle.ResumeAudio();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (playCall == CallType.OnTriggerExit)
        {
            targetBundle.FireAudio(this.transform);
        }
        if (stopCall == CallType.OnTriggerExit)
        {
            targetBundle.StopAudio();
        }
        if (pauseCall == CallType.OnTriggerExit)
        {
            targetBundle.PauseAudio();
        }
        if (resumeCall == CallType.OnTriggerExit)
        {
            targetBundle.ResumeAudio();
        }
    }
}
