using System.Text.Json;

namespace SmartMetering.Api.Services;

// Client HTTP vers le micro-service IA (conteneur "ai-service").
public class AiServiceClient
{
    private readonly HttpClient _http;

    public AiServiceClient(HttpClient http, IConfiguration config)
    {
        _http = http;
        _http.BaseAddress = new Uri(config["AiService:BaseUrl"] ?? "http://ai-service:8000");
    }

    public record AnomalyResult(bool IsAnomaly, double Score);
    public record PredictionResult(double PredictedNextValue);

    public async Task<AnomalyResult?> DetectAnomalyAsync(IEnumerable<double> recentValues, double currentValue)
    {
        var payload = new { history = recentValues, current = currentValue };
        var response = await _http.PostAsJsonAsync("/anomaly", payload);
        if (!response.IsSuccessStatusCode) return null;
        return await response.Content.ReadFromJsonAsync<AnomalyResult>();
    }

    public async Task<PredictionResult?> PredictNextAsync(IEnumerable<double> history)
    {
        var payload = new { history };
        var response = await _http.PostAsJsonAsync("/predict", payload);
        if (!response.IsSuccessStatusCode) return null;
        return await response.Content.ReadFromJsonAsync<PredictionResult>();
    }
}
