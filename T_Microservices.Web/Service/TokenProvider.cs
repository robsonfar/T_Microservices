using T_Microservices.Web.Service.IService;
using T_Microservices.Web.Util;

namespace T_Microservices.Web.Service
{
    public class TokenProvider : ITokenProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TokenProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }


        public void SetToken(string token)
        {
            _httpContextAccessor.HttpContext?.Response.Cookies.Append(SD.TokenCookie, token);
        }

        public string? GetToken()
        {
            string? token = null;
            bool? hasToken = _httpContextAccessor.HttpContext?.Request.Cookies.TryGetValue(SD.TokenCookie, out token);

            return hasToken is true ? token : null;
        }

        public void ClearToken()
        {
            _httpContextAccessor.HttpContext?.Response.Cookies.Delete(SD.TokenCookie);
        }

    }
}
