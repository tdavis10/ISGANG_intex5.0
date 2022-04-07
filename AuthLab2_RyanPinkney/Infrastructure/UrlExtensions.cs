using System;
using Microsoft.AspNetCore.Http;

namespace AuthLab2_RyanPinkney.Infrastructure
{
    public static class UrlExtensions
    {
        // allows for pagination
        public static string PathAndQuery(this HttpRequest request) =>
             request.QueryString.HasValue ? $"{request.Path}{request.QueryString}" : request.Path.ToString();

    }
}
