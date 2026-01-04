using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
class FlagObjectClass
{
    public GameObject[] BreakObjects;
    public GameObject[] SpawnObjects;
}

public class TutorialScript : MonoBehaviour
{

    [SerializeField] FlagObjectClass[] FlagObjectClasses;
    [SerializeField] int MaxTutorialFlags = 3;

    int TutorialFlags = 0;

    // Start is called before the first frame update
    void Start()
    {
        foreach (FlagObjectClass obj in FlagObjectClasses)
        {
            foreach(GameObject obj2 in obj.SpawnObjects)
            {
                obj2.SetActive(false);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        FlagProgress();
    }



    void FlagProgress()
    {
        for (int i = 0; i < FlagObjectClasses[TutorialFlags].BreakObjects.Length; i++)
        {
            //Destroy(FlagObjectClasses[TutorialFlags].BreakObjects[i]);
            FlagObjectClasses[TutorialFlags].BreakObjects[i].SetActive(false);
        }
        for (int i = 0; i < FlagObjectClasses[TutorialFlags].SpawnObjects.Length; i++)
        {
            FlagObjectClasses[TutorialFlags].SpawnObjects[i].SetActive(true);
        }
    }
}
