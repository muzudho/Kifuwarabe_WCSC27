﻿namespace Grayscale.Kifuwarakei.Entities.Configuration
{
    using System.IO;

    /// <summary>
    /// Resource. ファイルについて。
    /// </summary>
    public class ResFile : IResFile
    {
        ResFile(string name)
        {
            this.Name = name;
        }

        /// <summary>
        /// ファイル名。
        /// 拡張子は .log 固定。ファイル削除の目印にします。
        /// </summary>
        public string Name { get; private set; }

        public static IResFile AsData(string logDirectory, string basename)
        {
            return new ResFile(Path.Combine(logDirectory, basename));
        }
        public static IResFile AsLog(string logDirectory, string basename)
        {
            return new ResFile(Path.Combine(logDirectory, $"[{EntitiesLayer.Unique}]{basename}"));
        }
    }
}
