﻿using Funky.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Funky.Fakes
{
    public class LoggingFunk : IFunk
    {
        private readonly ILogger<LoggingFunk> logger;

        public LoggingFunk(ILogger<LoggingFunk> logger)
        {
            if (logger is null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            this.logger = logger;
        }

        public Task DisableAsync() => Task.CompletedTask;

        public Task EnableAsync() => Task.CompletedTask;
    }
}
