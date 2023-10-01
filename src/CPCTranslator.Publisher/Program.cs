// See https://aka.ms/new-console-template for more information

using CPCTranslator.Publisher.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder();
builder.Services.AddHostedService<PublishService>();

var host = builder.Build();

host.Run();