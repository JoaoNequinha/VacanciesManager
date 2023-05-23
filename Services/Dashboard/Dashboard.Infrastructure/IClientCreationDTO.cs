namespace Dashboard.API.DTO
{
    public interface IClientCreationDTO
    {
        string account_manager { get; }
        string client_logo { get; }
        string description { get; }
        string name { get; }
    }
}