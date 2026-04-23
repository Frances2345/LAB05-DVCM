using UnityEngine;
using Sirenix.OdinInspector;
using Unity.Cinemachine;
using System.Collections.Generic;
using TMPro;

public class CinematicController : MonoBehaviour
{
    public CinemachineCamera DollyTarget;
    public CinemachineCamera IntroDolly;

    public List<CinemachineCamera> CinematicCameras;
    public int currentCinematicCamera;

    public CinemachineCamera PlayerCam;

    public MonoBehaviour PlayerController;

    public TextMeshProUGUI timerText;
    public float timer;

    public float timePerCamera;
    public int Index = -1;
    private bool SequenceActive = true;

    void Start()
    {
        if(PlayerController != null)
        {
            PlayerController.enabled = false;
        }
        InvokeRepeating(nameof(SwitchCamera), timePerCamera, timePerCamera);

        ResetPriorities();
        if(IntroDolly  != null)
        {
            IntroDolly.Priority = 20;
        }
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if(timerText != null)
        {
            timerText.text = "Tiempo: " + (int)timer + " segundos";
        }
    }

    public void ActiveDolly()
    {
        DollyTarget.Priority = 20;
        foreach (var cam in CinematicCameras) cam.Priority = 0;
        PlayerCam.Priority = 0;
    }

    [Button]
    public void SwitchCamera()
    {
        if(!SequenceActive)
        {
            return;
        }
        ResetPriorities();
        Index++;

        if (Index == 0)
        {
            DollyTarget.Priority = 20;
        }
        else if(Index - 1 < CinematicCameras.Count)
        {
            CinematicCameras[Index - 1].Priority = 20;
        }
        else
        {
            CinematicEnd();
        }

    }

    private void ResetPriorities()
    {
        if(IntroDolly != null)
        {
            IntroDolly.Priority = 0;
        }

        if (DollyTarget != null)
        {
            DollyTarget.Priority = 0;
        }

        PlayerCam.Priority = 0;

        foreach (var cam in CinematicCameras)
        {
            cam.Priority = 0;
        }
    }

    private void CinematicEnd()
    {
        CancelInvoke(nameof(SwitchCamera));
        SequenceActive = false;
        ResetPriorities();
        PlayerCam.Priority = 15;

        if (PlayerController != null)
        {
            PlayerController.enabled = true;
        }
    }

   /*private void OnTriggerEnter(Collider other)
    {
        if(!SequenceActive && other.CompareTag("Player"))
        {
            CinematicCameras[0].Priority = 25;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (!SequenceActive && other.CompareTag("Player"))
        {
            CinematicCameras[0].Priority = 0;
            PlayerCam.Priority = 15;
        }
    }*/
}
