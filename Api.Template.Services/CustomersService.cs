using Api.Template.Domain.Interfaces;
using Api.Template.Domain.ReadModels;
using Api.Template.Services.Interfaces;

namespace Api.Template.Services
{
    public class CustomersService(ICustomersDataProvider dataProvider) : ICustomersService
    {
        public async Task<List<Customer>> GetCustomers(RequestParam requestParam)
        {
            var customers = await dataProvider.GetCustomers(requestParam);
            return customers;
        }
    }
}
