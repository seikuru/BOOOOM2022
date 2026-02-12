using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class MusicSymbolObject : MonoBehaviour
{
    /// 音楽シンボルオブジェクトの管理クラス
    /// 敵を全滅させるとシンボルが解放され、BGM変更・エフェクト再生・回転アニメーションが実行される
    /// 
    [SerializeField] MusicType type;

    [SerializeField] List<GameObject> Enemylist;

    [SerializeField] AudioSource openSymbolSource;

    [SerializeField] GameObject[] DisenableObjects;
    [SerializeField] VisualEffect[] openSymbolEffects;
    [SerializeField] ParticleSystem openSymbolParticle;
    [SerializeField] float RotateSpeed  = 15.0f;
    [SerializeField] float fuwaSpeed = 2.0f;
    [SerializeField] Transform KurufuwaTransform;

    /// <summary>
    /// 外部から敵リストを設定(Editor拡張で利用)
    /// </summary>
    public void SetList(List<GameObject> list) => Enemylist = new(list);

    // シンボルが解放されたかのフラグ
    private bool openSymbol;

    private void OnEnable()
    {
        // 初期化処理
        Start();
    }

    // Start is called before the first frame update
    private void Start()
    {
        // 敵リストの自動取得とエフェクトの初期状態設定

        openSymbol = false;

        // リストが既に設定されている場合は処理をスキップ
        if (Enemylist != null)
            return;

        Enemylist = new();

        // 直下の子のみ取得（孫は含まれない）
        for (int i = 0; i < this.transform.childCount; i++)
        {
            Transform child = this.transform.GetChild(i);
            Enemylist.Add(child.gameObject);
        }

        // エフェクトを初期状態(停止・非表示)に設定
        foreach (var Effect in openSymbolEffects)
        {
            Effect.Stop();
            Effect.gameObject.SetActive(false);
        }
    }


    private void Update()
    {
        // 敵の全滅チェックとシンボル解放処理

        // 既に解放済みなら処理しない
        if (openSymbol) 
            return;

        // 敵リストをチェック。1体でもアクティブならリターン
        foreach (var _object in Enemylist)
        {
            if (_object.activeSelf)
                return;
        }

        // 全滅確認後、シンボル解放処理を実行
        openSymbol = true;

        foreach (var _object in DisenableObjects)
        {
            if(_object != null)
                 _object.SetActive(false);
        }

        // BGMと動的スプライトの開放フラグを送信
        BGMControll.OpenTypeSetting(type);
        SpriteAutoTransform.OpenTypeSetting(type);

        // 解放サウンドを再生
        openSymbolSource.PlayOneShot(openSymbolSource.clip);

        // 解放エフェクトを再生
        foreach (var Effect in openSymbolEffects)
        {
            Effect.gameObject.SetActive(true);
            Effect.SendEvent("OnPlay");
        }
 
        openSymbolParticle.Stop();
        openSymbolParticle.Play();

        // 回転・浮遊アニメーション開始
        StartCoroutine(Kurufuwa());
    }

    /// <summary>
    /// シンボルの回転・浮遊アニメーション
    /// Y軸回転とサイン波による上下運動を無限ループで実行
    /// </summary>
    private IEnumerator Kurufuwa()
    {
        Vector3 Position = KurufuwaTransform.localPosition;
        float sin = Mathf.Sin(Time.time);

        while (true)
        {
            // Y軸回転
            KurufuwaTransform.Rotate(new Vector3(0, RotateSpeed, 0) * Time.deltaTime);
            // サイン波で上下運動
            sin = Mathf.Sin(Time.time);
            KurufuwaTransform.localPosition = new Vector3(Position.x, Position.y + (sin * fuwaSpeed), Position.z);

            yield return null;
        }
        //yield return null;
    }
}
