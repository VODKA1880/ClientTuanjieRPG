using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace RPG.Animation
{
    public class AnimationUnitTest : MonoBehaviour
    {
        PlayableGraph graph;
        public AnimationClip clip;
        private AnimationUnit animUnit;
        void Start()
        {
            graph = PlayableGraph.Create();
            graph.SetTimeUpdateMode(DirectorUpdateMode.GameTime);


            animUnit = new AnimationUnit(graph, clip);
            var output = AnimationPlayableOutput.Create(graph, "Animation", GetComponent<Animator>());
            output.SetSourcePlayable(animUnit.GetAdapterPlayable());
            // animUnit.Enable();
            graph.Play();
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (animUnit.IsEnable)
                {
                    animUnit.Disable();
                }
                else
                {
                    animUnit.Enable();
                }
            }
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