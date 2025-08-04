using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Animation.BaseBehaviour
{
    [System.Serializable]
    public struct AnimationClipData
    {
        public AnimationClip clip;
        public float enterTime;
    } // Clip,Mixer,Random

    [System.Serializable]
    public struct Blend2DPlayableDataClip
    {
        public AnimationClipData[] clips;
        public float x;
        public float y;
    } // Blend2D

    [System.Serializable]
    public struct Blend2DPlayableData
    {
        public Playable playable;
        public float x;
        public float y;

        public Blend2DPlayableData(Playable playable, float x, float y) : this()
        {
            this.playable = playable;
            this.x = x;
            this.y = y;
        }
    } // Blend2D
}