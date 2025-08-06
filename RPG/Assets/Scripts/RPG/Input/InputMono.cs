using System.Collections.Generic;
using RPG.Character;
using UnityEngine;

namespace RPG.Input
{
    public class InputMono : MonoBehaviour
    {
        private List<InputCmdBase> inputBuffers = new List<InputCmdBase>();
        public CharacterMono characterMono;

        public void Init(CharacterMono characterMono)
        {
            this.characterMono = characterMono;
            inputBuffers.Clear();
        }

        public void Update()
        {
            UpdateInputCommands();
            ExecuteInputCommand(GetInputCommand());
        }

        private void UpdateInputCommands()
        {
            inputBuffers.Clear();
            var horizontal = UnityEngine.Input.GetAxis("Horizontal");
            var vertical = UnityEngine.Input.GetAxis("Vertical");
            if (horizontal != 0 || vertical != 0)
            {
                var direction = new Vector3(horizontal, 0, vertical).normalized;
                var moveCmd = new InputCmdMove();
                moveCmd.SetDirection(direction);
                inputBuffers.Add(moveCmd);
            }
            if (UnityEngine.Input.GetKeyDown(KeyCode.Space))
            {
                var jumpCmd = new InputCmdJump();
                inputBuffers.Add(jumpCmd);
            }

            if (inputBuffers.Count == 0)
            {
                var noneCmd = new InputCmdNone();
                inputBuffers.Add(noneCmd);
            }
        }

        private InputCmdBase GetInputCommand()
        {
            if (inputBuffers.Count == 0) return null;
            return inputBuffers[0];
        }

        private void ExecuteInputCommand(InputCmdBase inputCmd)
        {
            if (inputCmd == null) return;

            inputCmd.Execute(this);
        }
    }
}