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
    //[SerializeField] ThroughBomb throughBomb;

    private int currentPhase = 0;

    void Start()
    {
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

        StageMaterial.SetVector(MaterialPivName, PhaseObjects[currentPhase].MaterialCenter.position);
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

            if (PhaseObjects[currentPhase].PlayerDontMove)
            {
                PlayerRigidbody.constraints = RigidbodyConstraints.FreezeAll;
                PlayerMotionAnimator.SetBool("Binding", true);
                subCamera.Priority = 11;
            }
            else
            {
                PlayerRigidbody.constraints = RigidbodyConstraints.FreezeRotation;
                PlayerMotionAnimator.SetBool("Binding", false);
                subCamera.Priority= 1;
            }

            // Next Phase
            currentPhase++;
            if (currentPhase >= PhaseObjects.Length)
            {
                //throughBomb.enabled = false;
                enabled = false; // Copilot ����Ă��������I���@�悭�킩���
                return;          // �������̃C���f�b�N�X�͈̔͊O�w����߂���̂Ǝv����
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


    [SerializeField] UnityEvent DisableEvent;
    private void OnDisable()
    {
        DisableEvent.Invoke();
    }
}

