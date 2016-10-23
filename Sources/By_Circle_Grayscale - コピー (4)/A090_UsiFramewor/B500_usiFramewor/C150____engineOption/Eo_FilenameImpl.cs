using Grayscale.A090_UsiFramewor.B500_usiFramewor.C___150_EngineOption;

namespace Grayscale.A090_UsiFramewor.B500_usiFramewor.C150____EngineOption
{
    public class Eo_FilenameImpl : Eo_StringImpl, Eo_Filename
    {
        public Eo_FilenameImpl()
        {
            this.m_value_ = "";
            this.m_default_ = "";
        }

        /// <summary>
        /// 最初に入ってきた値を、既定値とします。
        /// </summary>
        /// <param name="value"></param>
        public Eo_FilenameImpl(string value)
        {
            this.m_value_ = value;
            this.m_default_ = value;
        }

        public Eo_FilenameImpl(string value, string defaultValue)
        {
            this.m_value_ = value;
            this.m_default_ = defaultValue;
        }

    }
}
