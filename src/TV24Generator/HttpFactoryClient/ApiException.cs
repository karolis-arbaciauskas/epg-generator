﻿using System;

namespace EpgGenerator.HttpFactoryClient
{
    public class ApiException : Exception
    {
        public int StatusCode { get; set; }
        public string Content { get; set; }
    }
}