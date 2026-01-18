using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;

public class MusicSymbolObject : MonoBehaviour
{
    [SerializeField] MusicType type;

    [SerializeField] List<GameObject> Enemylist;

    [SerializeField] AudioSource openSymbolSource;

    [SerializeField] GameObject[] DisenableObjects;
    [SerializeField] VisualEffect openSymbolEffect;
    [SerializeField] float RotateSpeed  = 15.0f;
    [SerializeField] float fuwaSpeed = 2.0f;


    public void SetList(List<GameObject> list) => Enemylist = new(list);

    bool openSymbol;

    // Start is called before the first frame update
    void Start()
    {
        openSymbol = false;

        if (Enemylist != null)
            return;

        Enemylist = new();

        // íºâ∫ÇÃéqÇÃÇ›éÊìæÅië∑ÇÕä‹Ç‹ÇÍÇ»Ç¢Åj
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
        openSymbolEffect.SendEvent("OnPlay");

        StartCoroutine(Kurufuwa());
    }

    IEnumerator Kurufuwa()
    {
        Vector3 Position = this.transform.position;
        float sin = Mathf.Sin(Time.time);

        while (true)
        {
            this.transform.Rotate(new Vector3(0, RotateSpeed, 0) * Time.deltaTime);
            sin = Mathf.Sin(Time.time);
            this.transform.position = new Vector3(Position.x, Position.y + (sin * fuwaSpeed), Position.z);

            yield return null;
        }

        yield return null;
    }
}
