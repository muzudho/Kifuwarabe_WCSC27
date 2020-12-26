namespace Grayscale.Kifuwarakei.Entities.Features
{
    public abstract class Util_TimeManager
    {
        public static bool CanShowJoho()
        {
            //return true;
            return
                0 <= Option_Application.Optionlist.JohoJikan // 正の数で
                &&
                Option_Application.TimeManager.LastJohoTime + Option_Application.Optionlist.JohoJikan <=
                Option_Application.TimeManager.Stopwatch_Tansaku.ElapsedMilliseconds
                ;
        }

        public static void DoneShowJoho()
        {
            Option_Application.TimeManager.LastJohoTime = Option_Application.TimeManager.Stopwatch_Tansaku.ElapsedMilliseconds;
        }
    }
}
