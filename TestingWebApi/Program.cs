using MinimalWebApi;

var options = new MinimalAppOptions
{
    StartUrl = "http://localhost:6000",
    UseSwagger = true
};

var builder = new MinimalWebAppBuilder(options);

builder?.Application?.MapGet("/", () => {
    return "Testing";
});

builder?.Build().Start();
