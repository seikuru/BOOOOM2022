using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.VFX;

[Serializable]
class PhaseObjectClass
{
    public GameObject[] Flags;
    public GameObject[] NextSpawnObjects;
    public GameObject[] BreakObjects;
    public Transform MaterialCenter;
    public bool PlayerDontMove = false;
    public bool ChangeCamera = false;
    public bool FadeOutBGM = false;
}

public class TutrialStageManager : MonoBehaviour
{
    [SerializeField] PhaseObjectClass[] PhaseObjects;
    [SerializeField] Material StageMaterial;
    [SerializeField] Transform PlayerPosition;
    [SerializeField] String MaterialPivName = "_PivotPosition";
    [SerializeField] string MaterialCenterName = "_CenterPosition";
    [SerializeField] Rigidbody PlayerRigidbody;
    [SerializeField] List<VisualEffect> PhaseChangeEffect;
    [SerializeField] Animator PlayerMotionAnimator;
    [SerializeField] CinemachineVirtualCamera subCamera;
    [SerializeField] AudioSource TutorialAudio;
    //[SerializeField] ThroughBomb throughBomb;

    private int currentPhase = 0;

    void Start()
    {
        ThroughBomb.TutrialCheck = true;

        foreach (PhaseObjectClass obj in PhaseObjects)
        {
            if (obj.NextSpawnObjects != null)
            {
                foreach (GameObject spawnObj in obj.NextSpawnObjects)
                {
                    if (spawnObj == null) continue;
                    spawnObj.SetActive(false);
                }
            }

            if (obj.BreakObjects != null)
            {
                //foreach (GameObject breakObj in obj.BreakObjects)
                //{
                //    // breakObj.SetActive(true);
                //}
            }
        }

        if(PhaseObjects[currentPhase].MaterialCenter != null)
            StageMaterial.SetVector(MaterialPivName, PhaseObjects[currentPhase].MaterialCenter.position);

        if(PhaseObjects[currentPhase].PlayerDontMove)
        {
            BindPlayer();
            Debug.Log("Bind Player at Start");
        }
        else
        {
            ReleasePlayer();
        }

        if (PhaseObjects[currentPhase].ChangeCamera)
        {
            subCamera.Priority = 11;
        }
        else
        {
            subCamera.Priority = 1;
        }

        
    }

    // Update is called once per frame
    void Update()
    {
        // Check Phase Clear
        if (CheckPhaseClear())
        {
            // Debug.Log("Phase " + (currentPhase + 1) + " Clear!");
            // Break Objects
            foreach (GameObject breakObj in PhaseObjects[currentPhase].BreakObjects)
            {
                if (breakObj == null) continue;
                breakObj.SetActive(false);
            }

            // Spawn Objects
            foreach (GameObject spawnObj in PhaseObjects[currentPhase].NextSpawnObjects)
            {
                if (spawnObj == null) continue;
                spawnObj.SetActive(true);
            }

            // Next Phase
            currentPhase++;
            if (currentPhase >= PhaseObjects.Length)
            {
                ThroughBomb.TutrialCheck = false;
                //throughBomb.enabled = false;
                enabled = false; // Disable this script

                ReleasePlayer();

                return;          // End of all phases
            }

            // Player Bind/Release
            if (PhaseObjects[currentPhase].PlayerDontMove)
            {
                BindPlayer();
            }
            else
            {
                ReleasePlayer();
            }

            // Change Camera Priority
            if (PhaseObjects[currentPhase].ChangeCamera)
            {
                subCamera.Priority = 11;
            }
            else
            {
                subCamera.Priority = 1;
            }

            if (PhaseObjects[currentPhase].FadeOutBGM)
            {
                if (TutorialAudio != null)
                {
                    StartCoroutine(FadeOut_TutorialBGM());
                }
            }

            // Change Material Pivot
            if (PhaseObjects[currentPhase].MaterialCenter != null)
            {
                StageMaterial.SetVector(MaterialPivName, PhaseObjects[currentPhase].MaterialCenter.position);
            }

            // Play Effect
            if (PhaseChangeEffect != null)
            {
                foreach(VisualEffect vfx in PhaseChangeEffect)
                {
                    vfx.SendEvent("OnPlay");
                }
            }
        }

        // Update Material Center Position
        StageMaterial.SetVector(MaterialCenterName, PlayerPosition.position);
    }

    private bool CheckPhaseClear()
    {
        if (PhaseObjects[currentPhase].Flags == null || PhaseObjects[currentPhase].Flags.Length <= 0) return false;

        foreach (GameObject flag in PhaseObjects[currentPhase].Flags)
        {
            if (flag.activeSelf)
            {
                return false;
            }
        }
        return true;
    }

    private void BindPlayer()
    {
        PlayerRigidbody.constraints = RigidbodyConstraints.FreezeAll;
        PlayerMotionAnimator.SetBool("Binding", true);
        PlayerMotionAnimator.SetBool("Ground", true);
    }

    private void ReleasePlayer()
    {
        PlayerRigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        PlayerMotionAnimator.SetBool("Binding", false);
        PlayerMotionAnimator.SetBool("Ground", false);
    }

    [SerializeField] UnityEvent DisableEvent;
    private void OnDisable()
    {
        ThroughBomb.TutrialCheck = false;
        DisableEvent.Invoke();
    }

    IEnumerator FadeOut_TutorialBGM()
    {
        WaitForSeconds wait = new WaitForSeconds(0.75f);
        while (TutorialAudio.volume > 0.0f)
        {
            TutorialAudio.volume -= 0.1f;
            yield return wait;
        }
    }
}

