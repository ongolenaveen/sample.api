using Api.Template.Domain.ReadModels;

namespace Api.Template.Services.Interfaces
{
    public interface ICustomersService
    {
        Task<List<Customer>> GetCustomers(RequestParam requestParam);
    }
}
