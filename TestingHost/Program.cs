﻿using MinimalFramework;
using MinimalHost;
using TestingHost;

var options = new MinimalHostOptions
{
    ConsoleLogging = true,
    FileLogging = true,
    SeqLoggerOptions = new SeqLoggerOptions()
    {
        UseSeq = true,
        SeqServerUrl = "http://localhost:5341"
    }
};

var builder = new MinimalHostingBuilder(options)
    .Build(conf => {
    }, 
    messageHandlerAssembly: typeof(TestMessageHandler).Assembly);

builder.Run();

