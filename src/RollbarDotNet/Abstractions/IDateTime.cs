namespace RollbarDotNet.Abstractions
{
    using System;

    public interface IDateTime
    {
        DateTime UtcNow { get; }
    }
}
