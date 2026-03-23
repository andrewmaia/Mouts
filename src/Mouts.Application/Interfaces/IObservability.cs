namespace Mouts.Application.Interfaces;

public interface IObservability
{
    void TrackEvent(string name, IDictionary<string, string>? properties = null);

    void TrackMetric(string name, double value, IDictionary<string, string>? properties = null);

}
