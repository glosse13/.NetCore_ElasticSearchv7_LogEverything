﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace ElasticSearch.Configuration
{
    public static class Extensions
    {
        public static int ToInt(this object o)
        {
            if (o == null)
            {
                return default(int);
            }
            return Convert.ToInt32(o);
        }
        public static int GetAuthUser(this HttpContext context)  // Get User from Jwt Token
        {
            return context.User.Claims.FirstOrDefault(x => x.Type == "userId")?.Value.ToInt()??0;
        }

        public static (string, string) GetControllerAndActionName(this ActionExecutingContext context) // Controller Name & Action Name
        {
            var controllerName = context.RouteData.Values["controller"].ToString(); 
            var actionName = context.RouteData.Values["action"].ToString();
            return (controllerName, actionName);
        }
        public static string GetDisplayName(this IProperty props) // Get 'Display Name' from Model Property
        {
            var prop = props.PropertyInfo;
            if (prop.CustomAttributes == null || prop.CustomAttributes.Count() == 0)
                return prop.Name;

            var displayNameAttribute = prop.CustomAttributes.Where(x => x.AttributeType == typeof(DisplayNameAttribute)).FirstOrDefault();

            if (displayNameAttribute == null || displayNameAttribute.ConstructorArguments == null || displayNameAttribute.ConstructorArguments.Count == 0)
                return prop.Name;

            return displayNameAttribute.ConstructorArguments[0].Value.ToString() ?? prop.Name;
        }

        public static bool CheckAttributeExist<T>(this Type type) => type.GetCustomAttributes(false).FirstOrDefault(x => x.GetType() == typeof(T)) != null;
        public static bool CheckAttributeExist<T>(this PropertyInfo type) => type.GetCustomAttributes(false).FirstOrDefault(x => x.GetType() == typeof(T)) != null;
    }
}
