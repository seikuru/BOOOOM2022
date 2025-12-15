using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class UIPause : MonoBehaviour
{
    [SerializeField]
    List<GameObject> ActivateObjectList;

    [SerializeField]
    PlayableDirector playableDirector;
    

    private void OnEnable()
    {
        playableDirector.Play();
    }

    private void OnDisable()
    {
        
    }


    public void Enable()
    {
        foreach (GameObject obj in ActivateObjectList)
        {
            obj.SetActive(true);
        }
    }

    public void Disable()
    {
        foreach (GameObject obj in ActivateObjectList)
        {
            obj.SetActive(false);
        }
    }

    

    // Update is called once per frame
    void Update()
    {
        
    }
}
