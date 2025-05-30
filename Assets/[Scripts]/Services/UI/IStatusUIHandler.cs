

public interface IStatusUIHandler : IGameService
{
    public void UpdateStatusUIText(IGameManager sender, UIStatusEventArgs e);
}
