using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandMarkObject : MonoBehaviour
{
    [SerializeField] MusicType type;

    [SerializeField] List<GameObject> Enemylist;

    public void SetList(List<GameObject> list) => Enemylist = new(list);

    bool openLandMark;

    // Start is called before the first frame update
    void Start()
    {
        openLandMark = false;

        if (Enemylist != null)
            return;

        Enemylist = new();

        // ’¼‰º‚Ìq‚Ì‚İæ“¾i‘·‚ÍŠÜ‚Ü‚ê‚È‚¢j
        for (int i = 0; i < this.transform.childCount; i++)
        {
            Transform child = this.transform.GetChild(i);
            Enemylist.Add(child.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (openLandMark) 
            return;

        foreach(var _object in Enemylist)
        {
            if (_object.activeSelf)
                return;
        }

        openLandMark = true;
        BGMControll.OpenTypeSetting(type);
    }
}
