namespace RollbarDotNet.Abstractions
{
    using System;

    public class SystemEnvironment : IEnvironment
    {
        public string MachineName => Environment.MachineName;
    }
}