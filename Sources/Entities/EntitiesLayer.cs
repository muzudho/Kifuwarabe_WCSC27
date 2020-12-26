using System;
using Grayscale.Kifuwarakei.Entities.Configuration;

namespace Grayscale.Kifuwarakei.Entities
{
    public class EntitiesLayer
    {
        private static readonly Guid unique = Guid.NewGuid();
        public static Guid Unique { get { return unique; } }


        public static void Implement(IEngineConf engineConf)
        {
            //SpecifiedFiles.Init(engineConf);
            //Logger.Init(engineConf);
            // Util_KifuTreeLogWriter.Init(engineConf);
        }
    }
}
