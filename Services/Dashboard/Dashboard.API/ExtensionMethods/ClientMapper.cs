using Dashboard.Domain.Models;

namespace Dashboard.API.ExtensionMethods;

public static class ClientMapper
{
    public static ClientCreationDTO ToDTO(this IFormCollection formData, byte[] imageData)
    {
        return new ClientCreationDTO(formData["name"], formData["account_manager"], formData["description"], imageData);
    }

    public static ModifyClientDTO ToModifyDTO(this IFormCollection formData, byte[] imageData)
    {
        return new ModifyClientDTO(formData["name"], formData["account_manager"], formData["description"], imageData);
    }
}
