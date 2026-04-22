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
        InvokeRepeating(nameof(SwitchCamera), 0f, 15f);
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if(timerText != null)
        {
            timerText.text = "Tiempo: " + (int)timer + "s";
        }
    }

    [Button]
    public void SwitchCamera()
    {
        if(!SequenceActive)
        {
            return;
        }

        Dolly.Priority = 0;

        foreach(var cam in CinematicCameras) cam.Priority = 0;

        if(Index == -1)
        {
            Dolly.Priority = 20;
        }
        else if (Index < CinematicCameras.Count)
        {
            CinematicCameras[Index].Priority = 20;
        }
        else
        {
            CinematicEnd();
            return;
        }
       Index++;
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
