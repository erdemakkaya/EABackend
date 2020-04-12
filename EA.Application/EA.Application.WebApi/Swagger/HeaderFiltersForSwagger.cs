﻿using System;
using System.Collections.Generic;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace EA.Application.WebApi.Swagger
{
    public class HeaderFiltersForSwagger : IOperationFilter
    {

        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
                operation.Parameters = new List<OpenApiParameter>();

            operation.Parameters.Add(new OpenApiParameter()
            {
                Name = "Authorization",
                In = ParameterLocation.Header,
                Required = false // set to false if this is optional,
            });
        }
    }
}
