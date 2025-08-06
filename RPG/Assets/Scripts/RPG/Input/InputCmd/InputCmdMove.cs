namespace RPG.Input
{
    using UnityEngine;

    public class InputCmdMove : InputCmdBase
    {
        public Vector3 Direction { get; private set; }

        public InputCmdMove()
        {
            Id = (int)InputCmdId.Move;
        }

        public void SetDirection(Vector3 direction)
        {
            Direction = direction;
        }

        public override void Execute(InputMono inputMono)
        {

        }
    }
}