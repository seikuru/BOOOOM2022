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
    [SerializeField] VisualEffect[] openSymbolEffects;
    [SerializeField] float RotateSpeed  = 15.0f;
    [SerializeField] float fuwaSpeed = 2.0f;
    [SerializeField] Transform KurufuwaTransform;


    public void SetList(List<GameObject> list) => Enemylist = new(list);

    bool openSymbol;

    private void OnEnable()
    {
        Start();
    }

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

        foreach (var Effect in openSymbolEffects)
        {
            Effect.Stop();
            Effect.gameObject.SetActive(false);
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
        SpriteAutoTransform.OpenTypeSetting(type);

        openSymbolSource.PlayOneShot(openSymbolSource.clip);

        foreach (var Effect in openSymbolEffects)
        {
            Effect.gameObject.SetActive(true);
            Effect.SendEvent("OnPlay");
        }
        
        StartCoroutine(Kurufuwa());
    }

    IEnumerator Kurufuwa()
    {
        Vector3 Position = KurufuwaTransform.localPosition;
        float sin = Mathf.Sin(Time.time);

        while (true)
        {
            KurufuwaTransform.Rotate(new Vector3(0, RotateSpeed, 0) * Time.deltaTime);
            sin = Mathf.Sin(Time.time);
            KurufuwaTransform.localPosition = new Vector3(Position.x, Position.y + (sin * fuwaSpeed), Position.z);

            yield return null;
        }

        yield return null;
    }
}
