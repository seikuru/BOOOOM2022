using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
class PhaseObjectClass
{
    public GameObject[] Flags;
    public GameObject[] NextSpawnObjects;
    public GameObject[] BreakObjects;
    public Transform MaterialCenter;
}

public class TutrialStageManager : MonoBehaviour
{
    [SerializeField] PhaseObjectClass[] PhaseObjects;
    [SerializeField] Material StageMaterial;
    [SerializeField] String MaterialPivName = "_PivotPosition";

    private int currentPhase = 0;

    void Start()
    {
        foreach(PhaseObjectClass obj in PhaseObjects)
        {
            if(obj.NextSpawnObjects != null)
            {
                foreach (GameObject spawnObj in obj.NextSpawnObjects)
                {
                    if(spawnObj == null) continue;
                    spawnObj.SetActive(false);
                }
            }
            
            if(obj.BreakObjects != null)
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
            Debug.Log("Phase " + (currentPhase + 1) + " Clear!");
            // Break Objects
            foreach (GameObject breakObj in PhaseObjects[currentPhase].BreakObjects)
            {
                if(breakObj == null) continue;
                breakObj.SetActive(false);
            }

            // Spawn Objects
            foreach (GameObject spawnObj in PhaseObjects[currentPhase].NextSpawnObjects)
            {
                if(spawnObj == null) continue;
                spawnObj.SetActive(true);
            }

            // Next Phase
            currentPhase++;
            if(currentPhase >= PhaseObjects.Length)
            {
                enabled = false; // Copilot が提案した早期終了　よくわからん
                return;          // 多分下のインデックスの範囲外指定を咎めるものと思われる
            }

            // Change Material Pivot
            if (PhaseObjects[currentPhase].MaterialCenter != null)
            {
                StageMaterial.SetVector(MaterialPivName, PhaseObjects[currentPhase].MaterialCenter.position);
            }
        }
    }

    private bool CheckPhaseClear()
    {
        if (PhaseObjects[currentPhase].Flags == null || PhaseObjects[currentPhase].Flags.Length <= 0) return false;

        foreach (GameObject flag in PhaseObjects[currentPhase].Flags)
        {
            if(flag.activeSelf)
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
