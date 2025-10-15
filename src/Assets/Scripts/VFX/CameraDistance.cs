using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.VFX;
using UnityEngine.VFX;

public class CameraDistance : MonoBehaviour
{
    [SerializeField] GameObject MainCamera;
    [SerializeField] GameObject TargetObject;
    [SerializeField] VisualEffect effect;


    void Start()
    {
        
    }

    void FixedUpdate()
    {
        var dist = Vector3.Distance(MainCamera.transform.position, TargetObject.transform.position);
        effect.SetFloat("Distance", dist);
    }
}
