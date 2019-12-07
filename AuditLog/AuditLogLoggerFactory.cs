using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace AuditLog
{
    [ExcludeFromCodeCoverage]
    public static class AuditLogLoggerFactory
    {
        public static ILoggerFactory LoggerFactory { get; set; } = new NullLoggerFactory();

        public static ILogger<T> CreateInstance<T>() => LoggerFactory.CreateLogger<T>();
    }
}