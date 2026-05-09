using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiscSetting : MonoBehaviour
{
    [SerializeField] Image DiscImage;

    [SerializeField] ResultBGMControll resultBGMControll;
    
    void Start()
    {
        if(resultBGMControll == null)
        {
            var pack = resultBGMControll.GetUsePack();

            if (pack == null)
                return;
            DiscImage.sprite = pack.BGM.GetDisc();
        }
    }
}
