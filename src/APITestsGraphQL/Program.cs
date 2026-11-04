using HotChocolate.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services
    .AddGraphQLServer()
    .AddQueryType<Query>();

var app = builder.Build();

app.MapOpenApi();

var enableNitro = builder.Configuration.GetValue<bool>("EnableNitro");
if (enableNitro)
    app.Logger.LogInformation("Foi habilitado o uso da interface Nitro.");
else
    app.Logger.LogWarning("O uso da interface Nitro não foi habilitado.");
app.UseHttpsRedirection();
app.MapGraphQL("/graphql/api/cloudnative").WithOptions(
    options => options.Tool.Enable = enableNitro);;

app.Run();

public sealed class Query
{
    public IReadOnlyList<Technology> GetTechnologies() => TechnologyCatalog.All;

    public Technology? GetTechnology(string name) =>
        TechnologyCatalog.All.FirstOrDefault(
            technology => string.Equals(technology.Name, name, StringComparison.OrdinalIgnoreCase));
}

public sealed record Technology(string Name, string Description);

public static class TechnologyCatalog
{
    public static IReadOnlyList<Technology> All { get; } =
    [
        new("Kubernetes", "Orquestra contêineres e gerencia aplicações distribuídas."),
        new("Docker", "Empacota aplicações e suas dependências em contêineres."),
        new("Helm", "Gerencia e instala aplicações Kubernetes usando charts."),
        new("Istio", "Adiciona recursos de comunicação e segurança entre serviços."),
        new("Prometheus", "Coleta e consulta métricas de aplicações e infraestrutura."),
        new("OpenTelemetry", "Padroniza a coleta de métricas, logs e rastros distribuídos."),
        new("Argo CD", "Sincroniza aplicações Kubernetes com configurações versionadas no Git."),
        new("Terraform", "Provisiona infraestrutura usando arquivos declarativos."),
        new("Knative", "Executa workloads serverless sobre Kubernetes."),
        new("containerd", "Gerencia o ciclo de vida de contêineres em ambientes de execução.")
    ];
}
