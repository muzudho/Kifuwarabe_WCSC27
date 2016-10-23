using Grayscale.A090_UsiFramewor.B500_usiFramewor.C___150_EngineOption;

namespace Grayscale.A090_UsiFramewor.B500_usiFramewor.C150____EngineOption
{
    public class Eo_ButtonImpl : Eo_StringImpl, Eo_Button
    {
        public Eo_ButtonImpl()
        {
            this.m_value_ = "";
            this.m_default_ = "";
        }

        /// <summary>
        /// 最初に入ってきた値を、既定値とします。
        /// </summary>
        /// <param name="value"></param>
        public Eo_ButtonImpl(string value)
        {
            this.m_value_ = value;
            this.m_default_ = value;
        }

        public Eo_ButtonImpl(string value, string defaultValue)
        {
            this.m_value_ = value;
            this.m_default_ = defaultValue;
        }

        public override string ToString()
        {
            return "value " + this.m_value_;
        }
    }
}
