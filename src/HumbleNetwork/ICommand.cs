namespace HumbleNetwork
{
    /// <summary>
    /// TODO: inverter client, stream
    /// </summary>
    public interface ICommand
    {
        void Execute(IHumbleStream stream);
    }
}