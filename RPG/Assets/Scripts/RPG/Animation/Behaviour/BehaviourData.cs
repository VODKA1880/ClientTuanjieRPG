using RPG.Animation.BaseBehaviour;

namespace RPG.Animation.Behaviour
{
    [System.Serializable]
    public struct IdleBehaviourData
    {
        public AnimationClipData IdleAnim;
        public AnimationClipData[] IdleRandomAnims;
        public float RandomTime;
    }
}