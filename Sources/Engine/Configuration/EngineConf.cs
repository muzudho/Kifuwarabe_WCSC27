﻿namespace Grayscale.Kifuwarakei.Engine.Configuration
{
    using System.Configuration;
    using System.IO;
    using Grayscale.Kifuwarakei.Entities.Configuration;
    using Nett;

    public class EngineConf : IEngineConf
    {
        public string GetEngine(string key)
        {
            return this.EngineToml.Get<TomlTable>("Engine").Get<string>(key);
        }

        public string GetResourceFullPath(string key)
        {
            return Path.Combine(this.ProfilePath, this.EngineToml.Get<TomlTable>("Resources").Get<string>(key));
        }
        public string GetResourceBasename(string key)
        {
            return this.EngineToml.Get<TomlTable>("Resources").Get<string>(key);
        }

        /// <summary>
        /// フルパスにしない方が使いやすい？
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetLogBasename(string key)
        {
            return this.EngineToml.Get<TomlTable>("Logs").Get<string>(key);
        }

        public string LogDirectory
        {
            get
            {
                if (this.logDirectory_ == null)
                {
                    this.logDirectory_ = Path.Combine(this.ProfilePath, this.EngineToml.Get<TomlTable>("Resources").Get<string>("LogDirectory"));
                }
                return this.logDirectory_;
            }
        }
        string logDirectory_;

        public string DataDirectory
        {
            get
            {
                if (this.dataDirectory_ == null)
                {
                    this.dataDirectory_ = Path.Combine(this.ProfilePath, this.EngineToml.Get<TomlTable>("Resources").Get<string>("DataDirectory"));
                }
                return this.dataDirectory_;
            }
        }
        string dataDirectory_;

        public string CommandDirectory
        {
            get
            {
                if (this.commandDirectory_ == null)
                {
                    this.commandDirectory_ = Path.Combine(this.ProfilePath, this.EngineToml.Get<TomlTable>("Resources").Get<string>("CommandDirectory"));
                }
                return this.commandDirectory_;
            }
        }
        string commandDirectory_;

        TomlTable EngineToml
        {
            get
            {
                if (this.engineToml_ == null)
                {
                    this.engineToml_ = Toml.ReadFile(Path.Combine(this.ProfilePath, "Engine.toml"));
                }
                return this.engineToml_;
            }
        }
        TomlTable engineToml_;

        string ProfilePath
        {
            get
            {
                if (this.profilePath_ == null)
                {
                    this.profilePath_ = ConfigurationManager.AppSettings["Profile"];
                }
                return this.profilePath_;
            }
        }
        string profilePath_;
    }
}
