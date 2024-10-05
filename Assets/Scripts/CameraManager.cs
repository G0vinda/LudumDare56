using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;
    public CinemachineVirtualCamera cam;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
    }
    public void SwitchCameraFollowTarget(Transform target)
    {
        cam.Follow = target;
    }


    public void ShakeCamera(float duration,float magnitude)
    {
        StartCoroutine(Shake(duration, magnitude));
    }


    public IEnumerator Shake(float duration,float magnitude)
    {
        float passedTime = 0;

        while (duration > passedTime)
        {
            cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = magnitude;
            passedTime+= Time.deltaTime;
            yield return new WaitForEndOfFrame();

        }

        cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 0;

    }
}
