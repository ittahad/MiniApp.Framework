namespace MinimalFramework
{
    public sealed class MinimalWebAppOptions : MinimalHostOptions
    {
        public string? StartUrl { get; set; }
        public bool? UseSwagger { get; set; }
        public bool? UseAuthentication { get; set; } = false;
    }
}
