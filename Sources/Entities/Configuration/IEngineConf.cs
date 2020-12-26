namespace Grayscale.Kifuwarakei.Entities.Configuration
{
    public interface IEngineConf
    {
        string LogDirectory { get; }
        string DataDirectory { get; }
        string CommandDirectory { get; }

        string GetEngine(string key);
        string GetResourceFullPath(string key);
        string GetResourceBasename(string key);
        string GetLogBasename(string key);
    }
}
