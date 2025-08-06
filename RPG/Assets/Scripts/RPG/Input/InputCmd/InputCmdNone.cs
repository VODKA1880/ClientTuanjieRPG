namespace RPG.Input
{
    public class InputCmdNone : InputCmdBase
    {
        public InputCmdNone()
        {
            Id = (int)InputCmdId.None;
        }

        public override void Execute(InputMono inputMono)
        {

        }
    }
}