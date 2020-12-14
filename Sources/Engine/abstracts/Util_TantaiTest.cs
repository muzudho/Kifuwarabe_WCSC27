using kifuwarabe_wcsc27.facade;
using kifuwarabe_wcsc27.implements;
using kifuwarabe_wcsc27.interfaces;
using kifuwarabe_wcsc27.machine;


namespace kifuwarabe_wcsc27.abstracts
{
    /// <summary>
    /// 単体テストだぜ☆（＾▽＾）
    /// </summary>
    public abstract class Util_TantaiTest
    {
        /// <summary>
        /// 千日手のテストだぜ☆（＾▽＾）
        /// </summary>
        public static void SennitiTe(bool isSfen, Kyokumen ky, Mojiretu syuturyoku)
        {
            Util_Machine.Flush(syuturyoku);// 溜まっているログがあれば、吐き出させておくぜ☆（＾～＾）

            CommandMode mode = CommandMode.NigenYoConsoleKaihatu;
            Mojiretu sippaiZenbu = new MojiretuImpl();
            bool seikou = true;//成功☆

            // 準備
            int motonoJosekiPer = Option_Application.Optionlist.JosekiPer;// 定跡通り指すと千日手になることがあるので、切っておくぜ☆（／＿＼）
            Option_Application.Optionlist.JosekiPer = 0;
            double motonoNikomaHyokaKeisu = Option_Application.Optionlist.NikomaHyokaKeisu;// ２駒関係の評価値が付いているときは、テストケースの想定している評価値と異なり、結果が変わることがあるぜ☆（＾～＾）
            Option_Application.Optionlist.NikomaHyokaKeisu = 0.0d;
            long motonoSikoJikan = Option_Application.Optionlist.SikoJikan;// PCが重かったりして、1秒の思考時間じゃ足りないこともある☆？？（＾▽＾）
            int motonoSikoJikanRandom = Option_Application.Optionlist.SikoJikanRandom;
            Option_Application.Optionlist.SikoJikan = 5000; // 5秒もあれば足りるだろ☆（＾～＾）
            Option_Application.Optionlist.SikoJikanRandom = 0;
            int motonoJohoJikan = Option_Application.Optionlist.JohoJikan;// 読み筋は全部出してしまおうぜ☆（＾▽＾）
            Option_Application.Optionlist.JohoJikan = 0;

            #region 千日手のテスト☆
            // 千日手を判定するテストだぜ☆　きりんを上下しようぜ☆ｗｗｗｗ（＾▽＾）
            {
                #region （０１） 対局者１が千日手を認識するかのテストだぜ☆
                //if(false)
                {
                    Mojiretu mojiretu1 = new MojiretuImpl();
                    mojiretu1.AppendLine("# （０１） 対局者１が千日手を認識するかのテストだぜ☆");
                    int fail = 0;
                    ky.DoHirate(isSfen, syuturyoku);
                    int oldSaidaiFukasa = Option_Application.Optionlist.SaidaiFukasa;
                    Option_Application.Optionlist.SaidaiFukasa = 1; // ログが出過ぎないように1手読みにするぜ☆
                    int count;
                    Util_Commands.Do(isSfen, "do c4c3", ky, mode, mojiretu1); count = ky.Konoteme.GetSennititeCount(); if (Const_Game.SENNITITE_COUNT == count) { fail = 1; goto gt_EndUnittestSennitite1a; }//1回目
                    Util_Commands.Do(isSfen, "do a1a2", ky, mode, mojiretu1); count = ky.Konoteme.GetSennititeCount(); if (Const_Game.SENNITITE_COUNT == count) { fail = 2; goto gt_EndUnittestSennitite1a; }
                    Util_Commands.Do(isSfen, "do c3c4", ky, mode, mojiretu1); count = ky.Konoteme.GetSennititeCount(); if (Const_Game.SENNITITE_COUNT == count) { fail = 3; goto gt_EndUnittestSennitite1a; }
                    Util_Commands.Do(isSfen, "do a2a1", ky, mode, mojiretu1); count = ky.Konoteme.GetSennititeCount(); if (Const_Game.SENNITITE_COUNT == count) { fail = 4; goto gt_EndUnittestSennitite1a; }

                    Util_Commands.Do(isSfen, "do c4c3", ky, mode, mojiretu1); count = ky.Konoteme.GetSennititeCount(); if (Const_Game.SENNITITE_COUNT == count) { fail = 5; goto gt_EndUnittestSennitite1a; }//2回目
                    Util_Commands.Do(isSfen, "do a1a2", ky, mode, mojiretu1); count = ky.Konoteme.GetSennititeCount(); if (Const_Game.SENNITITE_COUNT == count) { fail = 6; goto gt_EndUnittestSennitite1a; }
                    Util_Commands.Do(isSfen, "do c3c4", ky, mode, mojiretu1); count = ky.Konoteme.GetSennititeCount(); if (Const_Game.SENNITITE_COUNT == count) { fail = 7; goto gt_EndUnittestSennitite1a; }
                    Util_Commands.Do(isSfen, "do a2a1", ky, mode, mojiretu1); count = ky.Konoteme.GetSennititeCount(); if (Const_Game.SENNITITE_COUNT == count) { fail = 8; goto gt_EndUnittestSennitite1a; }

                    Util_Commands.Do(isSfen, "do c4c3", ky, mode, mojiretu1); count = ky.Konoteme.GetSennititeCount(); if (Const_Game.SENNITITE_COUNT != count) { fail = 9; goto gt_EndUnittestSennitite1a; }//3回目 千日手☆（＾▽＾）

                    gt_EndUnittestSennitite1a:
                    Option_Application.Optionlist.SaidaiFukasa = oldSaidaiFukasa;//設定を元に戻しておくぜ☆
                    if (0 != fail)
                    {
                        if (seikou)
                        {
                            sippaiZenbu.Append(mojiretu1.ToContents());
                        }
                        seikou = false;
                        sippaiZenbu.AppendLine("# 失敗（０１）：　対局者１に、千日手が見えなかったぜ☆（／＿＼） fail=[" + fail + "] count=[" + count + "]");
                        //goto gt_EndUnitTest;
                    }
                }
                #endregion
                #region （０２） 対局者２が千日手を認識するかのテストだぜ☆
                //if (false)
                {
                    Mojiretu mojiretu1 = new MojiretuImpl();
                    mojiretu1.AppendLine("# （０２） 対局者２が千日手を認識するかのテストだぜ☆");
                    int fail = 0;
                    ky.SetBanjo(isSfen,
                        "　ラゾ" +//キは持ち駒に☆
                        "　ヒ　" +
                        "　ひ　" +
                        "ぞらき",false, syuturyoku
                        );
                    ky.MotiKomas.Clear().Set(MotiKoma.k,1);//{ 0, 0, 0, 0, 1, 0 }
                    ky.Tekiyo(true, syuturyoku);
                    int oldSaidaiFukasa = Option_Application.Optionlist.SaidaiFukasa;
                    Option_Application.Optionlist.SaidaiFukasa = 1; // ログが出過ぎないように1手読みにするぜ☆
                    int count;
                    Util_Commands.Do(isSfen, "do c4c3", ky, mode, mojiretu1); count = ky.Konoteme.GetSennititeCount(); if (Const_Game.SENNITITE_COUNT == count) { fail = 1; goto gt_EndUnittestSennitite1b; }// 同一局面から外れた手

                    Util_Commands.Do(isSfen, "do K*a2", ky, mode, mojiretu1); count = ky.Konoteme.GetSennititeCount(); if (Const_Game.SENNITITE_COUNT == count) { fail = 2; goto gt_EndUnittestSennitite1b; }// 1回目（きりん打）
                    Util_Commands.Do(isSfen, "do c3c4", ky, mode, mojiretu1); count = ky.Konoteme.GetSennititeCount(); if (Const_Game.SENNITITE_COUNT == count) { fail = 3; goto gt_EndUnittestSennitite1b; }
                    Util_Commands.Do(isSfen, "do a2a1", ky, mode, mojiretu1); count = ky.Konoteme.GetSennititeCount(); if (Const_Game.SENNITITE_COUNT == count) { fail = 4; goto gt_EndUnittestSennitite1b; }
                    Util_Commands.Do(isSfen, "do c4c3", ky, mode, mojiretu1); count = ky.Konoteme.GetSennititeCount(); if (Const_Game.SENNITITE_COUNT == count) { fail = 5; goto gt_EndUnittestSennitite1b; }

                    Util_Commands.Do(isSfen, "do a1a2", ky, mode, mojiretu1); count = ky.Konoteme.GetSennititeCount(); if (Const_Game.SENNITITE_COUNT == count) { fail = 6; goto gt_EndUnittestSennitite1b; }// 2回目（きりん指し）
                    Util_Commands.Do(isSfen, "do c3c4", ky, mode, mojiretu1); count = ky.Konoteme.GetSennititeCount(); if (Const_Game.SENNITITE_COUNT == count) { fail = 7; goto gt_EndUnittestSennitite1b; }
                    Util_Commands.Do(isSfen, "do a2a1", ky, mode, mojiretu1); count = ky.Konoteme.GetSennititeCount(); if (Const_Game.SENNITITE_COUNT == count) { fail = 8; goto gt_EndUnittestSennitite1b; }
                    Util_Commands.Do(isSfen, "do c4c3", ky, mode, mojiretu1); count = ky.Konoteme.GetSennititeCount(); if (Const_Game.SENNITITE_COUNT == count) { fail = 9; goto gt_EndUnittestSennitite1b; }

                    Util_Commands.Do(isSfen, "do a1a2", ky, mode, mojiretu1); count = ky.Konoteme.GetSennititeCount(); if (Const_Game.SENNITITE_COUNT != count) { fail = 10; goto gt_EndUnittestSennitite1b; }//3回目 千日手☆（＾▽＾）

                    gt_EndUnittestSennitite1b:
                    Option_Application.Optionlist.SaidaiFukasa = oldSaidaiFukasa;//設定を元に戻しておくぜ☆
                    if (0 != fail)
                    {
                        if (seikou)
                        {
                            sippaiZenbu.Append(mojiretu1.ToContents());
                        }
                        seikou = false;
                        sippaiZenbu.AppendLine("# 失敗（０２）：　対局者２に、千日手が見えなかったぜ☆（／＿＼） fail=[" + fail + "] count=[" + count + "]");
                        //goto gt_EndUnitTest;
                    }
                }
                #endregion
                #region （０３）対局者１が、勝っているときは　千日手を回避するかのテストだぜ☆
                //if (false)
                {
                    Mojiretu mojiretu1 = new MojiretuImpl();
                    mojiretu1.AppendLine("# （０３）対局者１が、勝っているときは　千日手を回避するかのテストだぜ☆");
                    bool fail = false;
                    ky.SetBanjo(isSfen,
                        "キラ　" + // ゾは対局者１の持ち駒に☆
                        "　　　" + // ヒは対局者１の持ち駒に☆
                        "　ひ　" +
                        "ぞらき", false, syuturyoku
                        );
                    ky.MotiKomas.Clear().Set(MotiKoma.Z,1).Set(MotiKoma.H,1);// = new int[] { 1, 0, 1, 0, 0, 0 };
                    ky.Tekiyo(true, syuturyoku);
                    int oldSaidaiFukasa = Option_Application.Optionlist.SaidaiFukasa;
                    Option_Application.Optionlist.SaidaiFukasa = 1; // ログが出過ぎないように1手読みにするぜ☆
                    mojiretu1.AppendLine("# 1回目");
                    Util_Commands.Do(isSfen, "do c4c3", ky, mode, mojiretu1);
                    Util_Commands.Do(isSfen, "do a1a2", ky, mode, mojiretu1);
                    Util_Commands.Do(isSfen, "do c3c4", ky, mode, mojiretu1);
                    Util_Commands.Do(isSfen, "do a2a1", ky, mode, mojiretu1);

                    mojiretu1.AppendLine("# 2回目");
                    Util_Commands.Do(isSfen, "do c4c3", ky, mode, mojiretu1);
                    Util_Commands.Do(isSfen, "do a1a2", ky, mode, mojiretu1);
                    Util_Commands.Do(isSfen, "do c3c4", ky, mode, mojiretu1);
                    Util_Commands.Do(isSfen, "do a2a1", ky, mode, mojiretu1);

                    mojiretu1.AppendLine("# 次に千日手を回避するかだぜ☆（＾▽＾）");
                    Util_Commands.Go(isSfen, mode, ky, mojiretu1);
                    int count = ky.Konoteme.GetSennititeCount();
                    if (Const_Game.SENNITITE_COUNT == count) { fail = true; goto gt_EndUnittestSennitite2a; }

                    gt_EndUnittestSennitite2a:
                    Option_Application.Optionlist.SaidaiFukasa = oldSaidaiFukasa;//設定を元に戻しておくぜ☆
                    if (fail)
                    {
                        if (seikou)
                        {
                            sippaiZenbu.Append(mojiretu1.ToContents());
                        }
                        seikou = false;
                        sippaiZenbu.AppendLine("# 失敗（０３）：　対局者１が、勝っているときに　千日手を回避しなかったぜ☆（／＿＼）");
                        //goto gt_EndUnitTest;
                    }
                }
                #endregion
                #region （０４）対局者２が、勝っているときは　千日手を回避するかのテストだぜ☆
                //if (false)
                {
                    Mojiretu mojiretu1 = new MojiretuImpl();
                    mojiretu1.AppendLine("# （０４）対局者２が、勝っているときは　千日手を回避するかのテストだぜ☆");
                    bool fail = false;
                    ky.SetBanjo(isSfen,
                        "　ラゾ" +//キ　は対局者２の持ち駒に☆
                        "　ヒ　" +
                        "　　　" +//ひ　は対局者２の持ち駒に☆
                        "　らき"//ぞ　は対局者２の持ち駒に☆
                        , false, syuturyoku);
                    ky.MotiKomas.Clear().Set(MotiKoma.z,1).Set(MotiKoma.k,1).Set(MotiKoma.h,1);// = new int[] { 0, 0, 0, 1, 1, 1 };
                    ky.Tekiyo(true, syuturyoku);
                    int oldSaidaiFukasa = Option_Application.Optionlist.SaidaiFukasa;
                    Option_Application.Optionlist.SaidaiFukasa = 1; // ログが出過ぎないように1手読みにするぜ☆

                    mojiretu1.AppendLine("# 同一局面から外れた手");
                    Util_Commands.Do(isSfen, "do c4c3", ky, mode, mojiretu1);

                    mojiretu1.AppendLine("# 1回目（きりん打）");
                    Util_Commands.Do(isSfen, "do K*a2", ky, mode, mojiretu1);
                    Util_Commands.Do(isSfen, "do c3c4", ky, mode, mojiretu1);
                    Util_Commands.Do(isSfen, "do a2a1", ky, mode, mojiretu1);
                    Util_Commands.Do(isSfen, "do c4c3", ky, mode, mojiretu1);

                    mojiretu1.AppendLine("# 2回目（きりん指し）");
                    Util_Commands.Do(isSfen, "do a1a2", ky, mode, mojiretu1);
                    Util_Commands.Do(isSfen, "do c3c4", ky, mode, mojiretu1);
                    Util_Commands.Do(isSfen, "do a2a1", ky, mode, mojiretu1);
                    Util_Commands.Do(isSfen, "do c4c3", ky, mode, mojiretu1);

                    mojiretu1.AppendLine("# 次に千日手を回避するか、指させるぜ☆（＾▽＾）ｗｗ");
                    Util_Commands.Go(isSfen, mode, ky, mojiretu1);
                    int count = ky.Konoteme.GetSennititeCount();
                    if (Const_Game.SENNITITE_COUNT == count) { fail = true; goto gt_EndUnittestSennitite2b; }

                    gt_EndUnittestSennitite2b:
                    Option_Application.Optionlist.SaidaiFukasa = oldSaidaiFukasa;//設定を元に戻しておくぜ☆
                    if (fail)
                    {
                        if (seikou)
                        {
                            sippaiZenbu.Append(mojiretu1.ToContents());
                        }
                        seikou = false;
                        sippaiZenbu.AppendLine("# 失敗（０４）：　対局者２が、勝っているときに　千日手を回避しなかったぜ☆（／＿＼）");
                        //goto gt_EndUnitTest;
                    }
                }
                #endregion
                #region （０５）対局者１が、勝っているときは　千日手の権利を渡さないテストだぜ☆
                //if (false)
                {
                    Mojiretu mojiretu1 = new MojiretuImpl();
                    mojiretu1.AppendLine("# （０５）対局者１が、勝っているときは　千日手の権利を渡さないテストだぜ☆");
                    bool fail = false;
                    ky.SetBanjo(isSfen,
                        "キラ　" + // ゾは対局者１の持ち駒に☆
                        "　　　" + // ヒは対局者１の持ち駒に☆
                        "　ひ　" +
                        "ぞらき"
                        , false, syuturyoku);
                    ky.MotiKomas.Clear().Set(MotiKoma.Z, 1).Set(MotiKoma.H, 1);// = new int[] { 1, 0, 1, 0, 0, 0 };
                    ky.Tekiyo(true, syuturyoku);
                    int oldSaidaiFukasa = Option_Application.Optionlist.SaidaiFukasa;
                    Option_Application.Optionlist.SaidaiFukasa = 1; // ログが出過ぎないように1手読みにするぜ☆
                    Util_Commands.Do(isSfen, "do c4c3", ky, mode, mojiretu1); //1回目
                    Util_Commands.Do(isSfen, "do a1a2", ky, mode, mojiretu1);
                    Util_Commands.Do(isSfen, "do c3c4", ky, mode, mojiretu1);
                    Util_Commands.Do(isSfen, "do a2a1", ky, mode, mojiretu1);

                    Util_Commands.Do(isSfen, "do c4c3", ky, mode, mojiretu1); //2回目
                    Util_Commands.Do(isSfen, "do a1a2", ky, mode, mojiretu1);
                    Util_Commands.Do(isSfen, "do c3c4", ky, mode, mojiretu1);

                    // 次に千日手の権利を渡すのを回避するかだぜ☆（＾▽＾）
                    Util_Commands.Go(isSfen, mode, ky, mojiretu1);
                    int count = ky.Konoteme.GetSennititeCount();
                    if (Const_Game.SENNITITE_COUNT == count) { fail = true; goto gt_EndUnittestSennitite3a; }

                    gt_EndUnittestSennitite3a:
                    Option_Application.Optionlist.SaidaiFukasa = oldSaidaiFukasa;//設定を元に戻しておくぜ☆
                    if (fail)
                    {
                        if (seikou)
                        {
                            sippaiZenbu.Append(mojiretu1.ToContents());
                        }
                        seikou = false;
                        sippaiZenbu.AppendLine("# 失敗（０５）：　対局者１が、勝っているときに　千日手の権利を渡さないことをしなかったんだぜ☆（／＿＼）");
                        //goto gt_EndUnitTest;
                    }
                }
                #endregion
                #region （０６）対局者２が、勝っているときは　千日手の権利を渡さないテストだぜ☆
                //if (false)
                {
                    Mojiretu mojiretu1 = new MojiretuImpl();
                    mojiretu1.AppendLine("# （０６）対局者２が、勝っているときは　千日手の権利を渡さないテストだぜ☆");
                    bool fail = false;
                    ky.SetBanjo(isSfen,
                        "　ラゾ" + // キ　は対局者２の持ち駒に☆
                        "　ヒ　" +
                        "　　　" + // ひ　は対局者２の持ち駒に☆
                        "　らき" // ぞ　は対局者２の持ち駒に☆
                        , false, syuturyoku);
                    ky.MotiKomas.Clear().Set(MotiKoma.z, 1).Set(MotiKoma.k, 1).Set(MotiKoma.h, 1);// = new int[] { 0, 0, 0, 1, 1, 1 };
                    ky.Tekiyo(true, syuturyoku);
                    int oldSaidaiFukasa = Option_Application.Optionlist.SaidaiFukasa;
                    Option_Application.Optionlist.SaidaiFukasa = 1; // ログが出過ぎないように1手読みにするぜ☆
                    Util_Commands.Do(isSfen, "do c4c3", ky, mode, mojiretu1); // 同一局面から外れた手

                    Util_Commands.Do(isSfen, "do K*a2", ky, mode, mojiretu1); // 1回目（きりん打）
                    Util_Commands.Do(isSfen, "do c3c4", ky, mode, mojiretu1);
                    Util_Commands.Do(isSfen, "do a2a1", ky, mode, mojiretu1);
                    Util_Commands.Do(isSfen, "do c4c3", ky, mode, mojiretu1);

                    Util_Commands.Do(isSfen, "do a1a2", ky, mode, mojiretu1); // 2回目（きりん指し）
                    Util_Commands.Do(isSfen, "do c3c4", ky, mode, mojiretu1);
                    Util_Commands.Do(isSfen, "do a2a1", ky, mode, mojiretu1);

                    // 次に千日手の権利を渡すのを回避するかだぜ☆（＾▽＾）
                    Util_Commands.Go(isSfen, mode, ky, mojiretu1);
                    int count = ky.Konoteme.GetSennititeCount();
                    if (Const_Game.SENNITITE_COUNT == count) { fail = true; goto gt_EndUnittestSennitite3b; }

                    gt_EndUnittestSennitite3b:
                    Option_Application.Optionlist.SaidaiFukasa = oldSaidaiFukasa;//設定を元に戻しておくぜ☆
                    if (fail)
                    {
                        if (seikou)
                        {
                            sippaiZenbu.Append(mojiretu1.ToContents());
                        }
                        seikou = false;
                        sippaiZenbu.AppendLine("# 失敗（０６）：　対局者２が、勝っているときに　千日手の権利を渡さないことをしなかったんだぜ☆（／＿＼）");
                        //goto gt_EndUnitTest;
                    }
                }
                #endregion
                #region （０７）対局者１が、負けているときは　千日手を受け入れるテストだぜ☆
                //if (false)
                {
                    Mojiretu mojiretu1 = new MojiretuImpl();
                    mojiretu1.AppendLine("# （０７）対局者１が、負けているときは　千日手を受け入れるテストだぜ☆");
                    bool fail = false;
                    ky.SetBanjo(isSfen,
                        "キラゾ" +
                        "　ヒ　" +
                        "　　　" + // ひ　は対局者２の持ち駒に☆
                        "　らき" // ぞ　は対局者２の持ち駒に☆
                        , false, syuturyoku);
                    ky.MotiKomas.Clear().Set(MotiKoma.z, 1).Set(MotiKoma.h, 1);// = new int[] { 0, 0, 0, 1, 0, 1 };
                    ky.Tekiyo(true, syuturyoku);
                    int oldSaidaiFukasa = Option_Application.Optionlist.SaidaiFukasa;
                    Option_Application.Optionlist.SaidaiFukasa = 1; // ログが出過ぎないように1手読みにするぜ☆

                    Util_Commands.Do(isSfen, "do c4c3", ky, mode, mojiretu1); //1回目
                    Util_Commands.Do(isSfen, "do a1a2", ky, mode, mojiretu1);
                    Util_Commands.Do(isSfen, "do c3c4", ky, mode, mojiretu1);
                    Util_Commands.Do(isSfen, "do a2a1", ky, mode, mojiretu1);

                    Util_Commands.Do(isSfen, "do c4c3", ky, mode, mojiretu1); //2回目
                    Util_Commands.Do(isSfen, "do a1a2", ky, mode, mojiretu1);
                    Util_Commands.Do(isSfen, "do c3c4", ky, mode, mojiretu1);
                    Util_Commands.Do(isSfen, "do a2a1", ky, mode, mojiretu1);

                    // 次に千日手を受け入れるかだぜ☆（＾▽＾）
                    Util_Commands.Go(isSfen, mode, ky, mojiretu1);
                    Option_Application.Optionlist.SaidaiFukasa = oldSaidaiFukasa;//設定を元に戻しておくぜ☆

                    int count = ky.Konoteme.GetSennititeCount();
                    if (Const_Game.SENNITITE_COUNT != count) { fail = true; goto gt_EndUnittestSennitite4a; }

                    gt_EndUnittestSennitite4a:
                    if (fail)
                    {
                        if (seikou)
                        {
                            sippaiZenbu.Append(mojiretu1.ToContents());
                        }
                        seikou = false;
                        sippaiZenbu.AppendLine("# 失敗（０７）：　対局者１が、負けているときに　千日手を受け入れなかったんだぜ☆（／＿＼）");
                        //goto gt_EndUnitTest;
                    }
                }
                #endregion
                #region （０８）対局者２が、負けているときは　千日手を受け入れるテストだぜ☆
                //if (false)
                {
                    Mojiretu mojiretu1 = new MojiretuImpl();
                    mojiretu1.AppendLine("# （０８）対局者２が、負けているときは　千日手を受け入れるテストだぜ☆");
                    bool fail = false;
                    ky.SetBanjo(isSfen,
                        "　ラ　" + // キ　は対局者２の持ち駒に☆　ゾ　は対局者１の持ち駒に☆
                        "　　　" + // ヒ　は対局者１の持ち駒に☆
                        "　ひ　" +
                        "ぞらき"
                        , false, syuturyoku);
                    ky.MotiKomas.Clear().Set(MotiKoma.Z, 1).Set(MotiKoma.H, 1).Set(MotiKoma.k, 1);// = new int[] { 1, 0, 1, 0, 1, 0 };
                    ky.Tekiyo(true, syuturyoku);
                    int oldSaidaiFukasa = Option_Application.Optionlist.SaidaiFukasa;
                    Option_Application.Optionlist.SaidaiFukasa = 1; // ログが出過ぎないように1手読みにするぜ☆

                    mojiretu1.AppendLine("# 同一局面から外れた手");
                    Util_Commands.Do(isSfen, "do c4c3", ky, mode, mojiretu1);

                    mojiretu1.AppendLine("# 1回目（きりん打）");
                    Util_Commands.Do(isSfen, "do K*a2", ky, mode, mojiretu1);
                    Util_Commands.Do(isSfen, "do c3c4", ky, mode, mojiretu1);
                    Util_Commands.Do(isSfen, "do a2a1", ky, mode, mojiretu1);
                    Util_Commands.Do(isSfen, "do c4c3", ky, mode, mojiretu1);

                    mojiretu1.AppendLine("# 2回目（きりん指し）");
                    Util_Commands.Do(isSfen, "do a1a2", ky, mode, mojiretu1);
                    Util_Commands.Do(isSfen, "do c3c4", ky, mode, mojiretu1);
                    Util_Commands.Do(isSfen, "do a2a1", ky, mode, mojiretu1);
                    Util_Commands.Do(isSfen, "do c4c3", ky, mode, mojiretu1);

                    mojiretu1.AppendLine("# 次に　きりん　を上げて、千日手を受け入れるかだぜ☆（＾▽＾）");
                    Util_Commands.MoveCmd(isSfen, "move seisei", ky, mojiretu1);
                    Util_Commands.Go(isSfen, mode, ky, mojiretu1);
                    Option_Application.Optionlist.SaidaiFukasa = oldSaidaiFukasa;//設定を元に戻しておくぜ☆

                    int count = ky.Konoteme.GetSennititeCount();
                    if (Const_Game.SENNITITE_COUNT != count) { fail = true; goto gt_EndUnittestSennitite4b; }

                    gt_EndUnittestSennitite4b:
                    if (fail)
                    {
                        if (seikou)
                        {
                            sippaiZenbu.Append(mojiretu1.ToContents());
                        }
                        seikou = false;
                        sippaiZenbu.AppendLine("# 失敗（０８）：　対局者２が、負けているときに　千日手を受け入れなかったんだぜ☆（／＿＼）");
                        //goto gt_EndUnitTest;
                    }
                }
                #endregion
                #region （０９）対局者２が、負けているときは　千日手の権利を相手に渡すテストだぜ☆
                //if (false)
                {
                    Mojiretu mojiretu1 = new MojiretuImpl();
                    mojiretu1.AppendLine("# （０９）対局者２が、負けているときは　千日手の権利を相手に渡すテストだぜ☆");
                    bool fail = false;
                    ky.SetBanjo(isSfen,
                        "キラ　" + // ゾは対局者１の持ち駒に☆
                        "　　　" + // ヒは対局者１の持ち駒に☆
                        "　ひ　" +
                        "ぞらき"
                        , false, syuturyoku);
                    ky.MotiKomas.Clear().Set(MotiKoma.Z, 1).Set(MotiKoma.H, 1);// = new int[] { 1, 0, 1, 0, 0, 0 };
                    ky.Tekiyo(true, syuturyoku);
                    int oldSaidaiFukasa = Option_Application.Optionlist.SaidaiFukasa;
                    Option_Application.Optionlist.SaidaiFukasa = 2; // 相手に千日手の手番を回したいので、2手読み以上にする必要があるぜ☆（＾▽＾）

                    mojiretu1.AppendLine("# 1回目");
                    Util_Commands.Do(isSfen, "do c4c3", ky, mode, mojiretu1);
                    Util_Commands.Do(isSfen, "do a1a2", ky, mode, mojiretu1);
                    Util_Commands.Do(isSfen, "do c3c4", ky, mode, mojiretu1);
                    Util_Commands.Do(isSfen, "do a2a1", ky, mode, mojiretu1);

                    mojiretu1.AppendLine("# 2回目");
                    Util_Commands.Do(isSfen, "do c4c3", ky, mode, mojiretu1);
                    Util_Commands.Do(isSfen, "do a1a2", ky, mode, mojiretu1);
                    Util_Commands.Do(isSfen, "do c3c4", ky, mode, mojiretu1);

                    mojiretu1.AppendLine("# ↓次の手に注目だぜ☆　対局者２は　きりん　を引いて、千日手の権利を相手に渡すかだぜ☆（＾▽＾）");
                    Util_Commands.Go(isSfen, mode, ky, mojiretu1);// do a2a1 とやることを期待☆
                    Option_Application.Optionlist.SaidaiFukasa = oldSaidaiFukasa;//設定を元に戻しておくぜ☆

                    Kyokumen ky2 = new Kyokumen();
                    ky2.SetBanjo(isSfen,
                        "キラ　" +
                        "　　　" +
                        "　ひ　" +
                        "ぞらき",true, syuturyoku);
                    int[] motikomas1 = { 1, 0, 1, 0, 0, 0, };
                    if (!ky.Equals(ky2.Shogiban, motikomas1))
                    {
                        fail = true;
                    }

                    if (fail)
                    {
                        if (seikou)
                        {
                            sippaiZenbu.Append(mojiretu1.ToContents());
                        }
                        seikou = false;
                        sippaiZenbu.AppendLine("# 失敗（０９）：　対局者２が、負けているときに　千日手の権利を相手に渡さなかったんだぜ☆（／＿＼）");
                        //goto gt_EndUnitTest;
                    }
                }
                #endregion
                #region （１０）対局者１が、負けているときは　千日手の権利を相手に渡すテストだぜ☆
                //if (false)
                {
                    Mojiretu mojiretu1 = new MojiretuImpl();
                    mojiretu1.AppendLine("# （１０）対局者１が、負けているときは　千日手の権利を相手に渡すテストだぜ☆");
                    bool fail = false;
                    ky.SetBanjo(isSfen,
                        "　ラゾ" + // キ　は対局者２の持ち駒に☆
                        "　ヒ　" +
                        "　　　" + // ひ　は対局者２の持ち駒に☆
                        "　らき" // ぞ　は対局者２の持ち駒に☆
                        , false, syuturyoku);
                    ky.MotiKomas.Clear().Set(MotiKoma.z,1).Set(MotiKoma.k,1).Set(MotiKoma.h,1);// = new int[] { 0, 0, 0, 1, 1, 1 };
                    ky.Tekiyo(true, syuturyoku);
                    int oldSaidaiFukasa = Option_Application.Optionlist.SaidaiFukasa;
                    Option_Application.Optionlist.SaidaiFukasa = 2; // 相手に千日手の手番を回したいので、2手読み以上にする必要があるぜ☆（＾▽＾）

                    mojiretu1.AppendLine("# 同一局面から外れた手");
                    Util_Commands.Do(isSfen, "do c4c3", ky, mode, mojiretu1);

                    mojiretu1.AppendLine("# 1回目（きりん打）");
                    Util_Commands.Do(isSfen, "do K*a2", ky, mode, mojiretu1);
                    Util_Commands.Do(isSfen, "do c3c4", ky, mode, mojiretu1);
                    Util_Commands.Do(isSfen, "do a2a1", ky, mode, mojiretu1);
                    Util_Commands.Do(isSfen, "do c4c3", ky, mode, mojiretu1);

                    mojiretu1.AppendLine("# 2回目（きりん指し）");
                    Util_Commands.Do(isSfen, "do a1a2", ky, mode, mojiretu1);
                    Util_Commands.Do(isSfen, "do c3c4", ky, mode, mojiretu1);
                    Util_Commands.Do(isSfen, "do a2a1", ky, mode, mojiretu1);

                    mojiretu1.AppendLine("# ↓次の手に注目だぜ☆　対局者１は　きりん　を上げて、千日手の権利を相手に渡すかだぜ☆（＾▽＾）");
                    Util_Commands.Go(isSfen, mode, ky, mojiretu1);// do c4c3 とやることを期待☆
                    Option_Application.Optionlist.SaidaiFukasa = oldSaidaiFukasa;//設定を元に戻しておくぜ☆

                    Kyokumen ky2 = new Kyokumen();
                    ky2.SetBanjo(isSfen,
                        "キラゾ" +
                        "　ヒ　" +
                        "　　き" +
                        "　ら　", true, syuturyoku);
                    int[] motikomas1 = { 0, 0, 0, 1, 0, 1, };
                    if (!ky.Equals(ky2.Shogiban, motikomas1))
                    {
                        fail = true;
                    }

                    if (fail)
                    {
                        if (seikou)
                        {
                            sippaiZenbu.Append(mojiretu1.ToContents());
                        }
                        seikou = false;
                        sippaiZenbu.AppendLine("# 失敗（１０）：　対局者１が、負けているときに、千日手の権利を相手に渡さなかったんだぜ☆（／＿＼）");
                        //goto gt_EndUnitTest;
                    }
                }
                #endregion
            }
            #endregion
            //gt_EndUnitTest:
            //;

            // 元に戻すぜ☆（＾▽＾）
            Option_Application.Optionlist.JosekiPer = motonoJosekiPer;
            Option_Application.Optionlist.NikomaHyokaKeisu = motonoNikomaHyokaKeisu;
            Option_Application.Optionlist.SikoJikan = motonoSikoJikan;
            Option_Application.Optionlist.SikoJikanRandom = motonoSikoJikanRandom;
            Option_Application.Optionlist.JohoJikan = motonoJohoJikan;

            if (seikou)
            {
                syuturyoku.AppendLine("ユニットテストの結果は、オール・オッケーだぜ☆（＾▽＾）");
            }
            else
            {
                syuturyoku.Append(sippaiZenbu.ToContents()); // 失敗した過程のログ☆
            }
            Util_Machine.Flush(syuturyoku);
        }
    }
}
