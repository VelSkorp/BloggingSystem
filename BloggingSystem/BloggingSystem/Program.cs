using BloggingSystem;
using Microsoft.AspNetCore;

var builder = WebHost.CreateDefaultBuilder(args);

builder.UseStartup<Startup>();

var app = builder.Build();

app.Run();