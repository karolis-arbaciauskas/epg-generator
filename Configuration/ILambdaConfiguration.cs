﻿using Microsoft.Extensions.Configuration;

namespace awscsharp.Configuration
{
    public interface ILambdaConfiguration
    {
        IConfiguration Configuration { get; }
    }
}