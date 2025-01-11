using System.Net;
using System.Net.Http.Headers;
using FluentAssertions;
using Microsoft.AspNetCore.TestHost;

namespace Api.Template.Integration.Tests.Tests.Customers
{
    [Collection(nameof(TestFixtureCollection))]
    public class GetCustomersTests(BaseTestFixture baseFixture)
    {
        private readonly TestServer _testServer = baseFixture.ApiTestServer;

        [Fact]
        public async Task GetCustomers_Without_Bearer_Token_Should_Return_401_UnAuthorized_Response()
        {
            // Arrange
            var url = "customers";
            var client = _testServer.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Get, url);

            // Act
            var response = await client.SendAsync(request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task GetCustomers_With_NonExisting_Customers_Empty_Results_Response()
        {
            // Arrange
            var url = "customers";
            var client = _testServer.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", baseFixture.AccessToken);

            // Act
            var response = await client.SendAsync(request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task GetCustomers_With_Existing_FileName_Should_Return_Document_In_The_Response()
        {
            // Arrange
            var url = "customers";
            var client = _testServer.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", baseFixture.AccessToken);

            // Act
            var response = await client.SendAsync(request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
