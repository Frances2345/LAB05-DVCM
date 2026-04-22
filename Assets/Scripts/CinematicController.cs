using UnityEngine;
using Sirenix.OdinInspector;
using Unity.Cinemachine;
using System.Collections.Generic;
using TMPro;

public class CinematicController : MonoBehaviour
{
    public List<CinemachineCamera> CinematicCameras;
    public int currentCinematicCamera;

    public CinemachineCamera PlayerCam;
    public CinemachineCamera Dolly;

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

        Index = -1;
        ActiveDolly();
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if(timerText != null)
        {
            timerText.text = "Tiempo: " + (int)timer + "s";
        }
    }

    public void ActiveDolly()
    {
        Dolly.Priority = 20;
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

        Dolly.Priority = 0;
        PlayerCam.Priority = 0;
        foreach (var cam in CinematicCameras) cam.Priority = 0;

        Index++;

        if(Index >= 0 && Index < CinematicCameras.Count)
        {
            CinematicCameras[Index].Priority = 20;
        }
        else
        {
            CinematicEnd();
        }
    }

    private void CinematicEnd()
    {
        CancelInvoke(nameof(SwitchCamera));
        SequenceActive = false;

        Dolly.Priority = 0;
        foreach (var cam in CinematicCameras)
        {
            cam.Priority = 0;
        }

        PlayerCam.Priority = 15;

        if (PlayerController != null)
        {
            PlayerController.enabled = true;
        }
    }

    private void OnTriggerEnter(Collider other)
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
    }
}
