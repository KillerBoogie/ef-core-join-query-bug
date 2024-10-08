﻿using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace VC.WebApi.Infrastructure.Middleware.Attributes
{
    public class CommaSeparatedQueryStringConvention : IActionModelConvention
    {
        public void Apply(ActionModel action)
        {
            SeparatedQueryStringAttribute attribute = null!;
            foreach (var parameter in action.Parameters)
            {
                if (parameter.Attributes.OfType<CommaSeparatedAttribute>().Any())
                {
                    if (attribute == null)
                    {
                        attribute = new SeparatedQueryStringAttribute(",");
                        parameter.Action.Filters.Add(attribute);
                    }

                    attribute.AddKey(parameter.ParameterName);
                }
            }
        }
    }
}