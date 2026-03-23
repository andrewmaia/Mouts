using Microsoft.ApplicationInsights;
using Mouts.Application.Interfaces;


public sealed class AppInsightsObservability : IObservability
{
    private readonly TelemetryClient _telemetry;

    public AppInsightsObservability(TelemetryClient telemetry)
    {
        _telemetry = telemetry;
    }

    public void TrackEvent(string name, IDictionary<string, string>? properties = null)
    {
        _telemetry.TrackEvent(name, properties);
    }

    public void TrackMetric(string name, double value, IDictionary<string, string>? properties = null)
    {
        _telemetry.TrackMetric(name, value, properties);
    }
}