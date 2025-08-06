namespace RPG.Input
{
    public abstract class InputCmdBase
    {
        public int Id { get; protected set; }
        public abstract void Execute(InputMono inputMono);
    }
}