using UnityEngine;
using static BGMPack.Pack;
[CreateAssetMenu(fileName = "Pack", menuName = "ScriptableObjects/BGMPack")]


public class BGMPack : ScriptableObject
{
    [SerializeField]
    AudioClip DefaltSoundClip;

    [SerializeField]
    Pack[] packs;

    public AudioClip GetDefaltSoundClip() => DefaltSoundClip;

    public AudioClip TakeBGM(MusicType type,int index,bool clearSound = true)
    {
        foreach(var pack in packs)
        {
            if(pack.SameType(type))
            {
                if(pack.IndexCheck(index))
                {
                    return pack.GetClipData(index).GetClip(clearSound);
                }
            }
        }
        Debug.LogError("ヌル音源");
        return null;
    }

    [System.Serializable]
    public class Pack
    {
        [SerializeField]
        MusicType Mtype;

        [SerializeField]
        Clip[] clips;

        public bool SameType(MusicType type) => Mtype == type;
        public bool IndexCheck(int i) => clips != null &&  i >= 0 && i <clips.Length;
        public Clip GetClipData(int i) => clips[i];

        [System.Serializable]
        public class Clip
        {
            [SerializeField] AudioClip clipDefalt;
            [SerializeField] AudioClip clipNoize;

            /// <summary>
            /// 通常クリップまたはノイズクリップを設定
            /// </summary>
            public AudioClip GetClip(bool isDefaltClip)
            {
                return isDefaltClip ? clipDefalt : clipNoize;
            }
        }
    }
}
