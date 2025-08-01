using UnityEngine;
using UnityEngine.Playables;

namespace RPG.MyAnimation
{

    public class AnimationMono : MonoBehaviour
    {
        PlayableGraph graph;
        private BlendTree2DBehaviour blendTree2D;
        public Blend2DClipData[] clips;
        public Vector2 BlendPosition;
        void Start()
        {
            graph = AnimationTool.GetAnimGraph();

            var animPlayable = ScriptPlayable<BlendTree2DBehaviour>.Create(graph);
            blendTree2D = animPlayable.Init(graph, clips);
            blendTree2D.Enable();

            graph.SetSourcePlayable(animPlayable, GetComponent<Animator>());
            graph.Play();
        }

        void Update()
        {
            BlendPosition.x = Input.GetAxis("Horizontal");
            BlendPosition.y = Input.GetAxis("Vertical");
            BlendPosition = BlendPosition.normalized;
            blendTree2D.SetBlend(BlendPosition.x, BlendPosition.y);
        }

        void OnEnable()
        {

        }

        void OnDisable()
        {
            graph.Destroy();
        }
    }
}