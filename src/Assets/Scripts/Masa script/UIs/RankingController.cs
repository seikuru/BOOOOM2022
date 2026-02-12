using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankingController : MonoBehaviour
{
    [System.Serializable]
    class ObjectActives
    {
        public int index;
        public GameObject[] enable;
    }

    [SerializeField]
    ObjectActives[] activeObjectList;

    [SerializeField]
    SelectButton selectButton;

    [SerializeField]
    GameObject BackImageObject;

    [SerializeField]
    int[] BackImageIndex;

    int beforeIndex;

    // Update is called once per frame
    void Update()
    {
        int currentIndex = selectButton.GetCurrentIndex();

        if(beforeIndex == currentIndex)
        {
            beforeIndex = currentIndex;
            return;
        }
        else
        {
            bool enableCheck = false;
            foreach (var index in BackImageIndex)
            {
                if(index == currentIndex)
                {
                    enableCheck = true; break;
                }
            }

            BackImageObject.SetActive(enableCheck);

            foreach (var list in activeObjectList)
            {
                if(list.index == beforeIndex)
                    foreach (var objects in list.enable)
                        objects.SetActive(false);

                if (list.index == currentIndex)
                    foreach (var objects in list.enable)
                        objects.SetActive(true);

            }

            beforeIndex = currentIndex;
        }
    }
}
