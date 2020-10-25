using System;
using System.Security.Claims;
using JedzenioPlanner.Api.Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;

namespace JedzenioPlanner.Api.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetUserId()
        {
            return _httpContextAccessor.HttpContext.User.Identity.Name;
        }
    }
}