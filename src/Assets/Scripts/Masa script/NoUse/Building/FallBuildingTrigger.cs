using UnityEngine;

public class FallBuildingTrigger : MonoBehaviour
{
　　/// 落ちた建物の残骸を判定するクラス

    [SerializeField] string BuildingTag = "Building";//タグの名前

    [SerializeField] FallBuildingCounter fallBuildingCounter; //建物をカウントするクラス

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == BuildingTag)
        {
            // オブジェクトを非表示に
            other.gameObject.SetActive(false);

        　　// カウンターにオブジェクトを登録する
            fallBuildingCounter?.AddFallObject(other.gameObject);
        }
    }
}
