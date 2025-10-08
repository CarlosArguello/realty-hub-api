namespace Application.Seed;

public interface IDataSeeder
{
    Task RunAsync(CancellationToken ct = default);
}