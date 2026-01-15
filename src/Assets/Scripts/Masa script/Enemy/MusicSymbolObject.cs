using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicSymbolObject : MonoBehaviour
{
    [SerializeField] MusicType type;

    [SerializeField] List<GameObject> Enemylist;

    [SerializeField] AudioSource openSymbolSource;

    [SerializeField] GameObject[] DisenableObjects;
    public void SetList(List<GameObject> list) => Enemylist = new(list);

    bool openSymbol;

    // Start is called before the first frame update
    void Start()
    {
        openSymbol = false;

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
        if (openSymbol) 
            return;

        foreach(var _object in Enemylist)
        {
            if (_object.activeSelf)
                return;
        }

        openSymbol = true;

        foreach (var _object in DisenableObjects)
        {
            if(_object != null)
                 _object.SetActive(false);
        }

        BGMControll.OpenTypeSetting(type);
        openSymbolSource.PlayOneShot(openSymbolSource.clip);
    }
}
