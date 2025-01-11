using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Api.Template.Integration.Tests.Config;
using Microsoft.IdentityModel.Tokens;

namespace Api.Template.Integration.Tests.Builders
{
    public class JwtTokenBuilder
    {
        private TestsAuthConfig? _authConfig;
        private SigningCredentials? _signingCredentials;
        private IEnumerable<Claim>? _claims;
        private DateTime? _notBeforeDateTime;
        private DateTime? _expiryDateTime;

        public JwtTokenBuilder WithAuthConfig(TestsAuthConfig authConfig)
        {
            _authConfig = authConfig;
            return this;
        }
        public JwtTokenBuilder WithSigningCredentials(SigningCredentials signingCredentials)
        {
            _signingCredentials = signingCredentials;
            return this;
        }

        public JwtTokenBuilder WithNotBeforeDateTime(DateTime notBeforeDateTime)
        {
            _notBeforeDateTime = notBeforeDateTime;
            return this;
        }

        public JwtTokenBuilder WithExpiryDateTime(DateTime expiryDateTime)
        {
            _expiryDateTime = expiryDateTime;
            return this;
        }

        public JwtTokenBuilder WithClaims(IEnumerable<Claim> claims)
        {
            _claims = claims;
            return this;
        }

        public string BuildToken()
        {
            var jwt = new JwtSecurityToken(
                audience: _authConfig?.Audience,
                issuer: _authConfig?.Issuer,
                claims: _claims,
                notBefore: _notBeforeDateTime,
                expires: _expiryDateTime,
                signingCredentials: _signingCredentials
            );
            var token = new JwtSecurityTokenHandler().WriteToken(jwt);
            return token;
        }
    }
}
