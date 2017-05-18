namespace kifuwarabe_wcsc27.facade
{
    /// <summary>
    /// スマホで外部ファイルを読込む方法が分からないので、埋め込むならこれを使うんだぜ☆（＾▽＾）
    /// </summary>
    public abstract class Face_Joseki
    {
        #region 組み込み定跡ファイル
        /// <summary>
        /// 外部ファイルを読めない場合、ここに直書きしておくぜ☆（＾▽＾）
        /// </summary>
        /// <returns></returns>
        public static string[] GetKumikomiJoseki()
        {
            // TODO: 最新の joseki.txt の内容を貼りつけること☆
            return @"".Split('\n');
            /*
                        return @"
            fen krz/1h1/1H1/ZRK - 1
            B3B2 none 2 9 102
            fen krz/1H1/3/ZRK H 2
            B1B2 none 0 6 0
            B1A2 none 0 9 0
            C1B2 none 0 9 0
            fen k1z/1r1/3/ZRK Hh 1
            A4B3 none 2 12 101
            fen k1z/1r1/1Z1/1RK Hh 2
            A1B1 none 0 5 0
            B2B1 none 2 8 0
            H*C2 none 0 8 0
            A1A2 none 2 8 100
            fen 1kz/1r1/1Z1/1RK Hh 1
            H*C2 none 6 5 0
            fen 1kz/1rH/1Z1/1RK h 2
            B1A1 none 0 6 0
            B2A1 none 0 6 0
            H*A3 none 0 13 101
            fen k1z/1rH/1Z1/1RK h 1
            C2C1 none 6 12 100
            fen k1H/1r1/1Z1/1RK Zh 2
            A1B1 none 0 0 0
            fen 1kH/1r1/1Z1/1RK Zh 1
            B3A4 none 4 12 100
            Z*A3 none 4 8 100
            fen 1kH/1r1/3/ZRK Zh 2
            B1A1 none 0 0 0
            fen k1H/1r1/3/ZRK Zh 1
            A4B3 none 4 12 100
            fen rkz/2H/1Z1/1RK h 1
            C2C1 none 6 5 0
            fen rkH/3/1Z1/1RK Zh 2
            A1B2 none 0 0 0
            fen k1z/rH1/3/ZRK H 1
            C4C3 none 4 8 102
            fen k1z/rH1/2K/ZR1 H 2
            C1B2 none 0 7 102
            fen k2/rz1/2K/ZR1 Hh 1
            C3C2 none 2 8 0
            fen k2/rzK/3/ZR1 Hh 2
            B2A3 none 0 6 102
            fen k2/r1K/z2/ZR1 Hh 1
            B4C3 none 2 9 0
            fen k2/r1K/z1R/Z2 Hh 2
            A2B1 none 0 7 102
            fen kr1/2K/z1R/Z2 Hh 1
            H*B2 none 2 8 0
            fen kr1/1HK/z1R/Z2 h 2
            A3B2 none 2 9 0
            fen kr1/1zK/2R/Z2 2h 1
            C2B2 none 87 8 0
            fen kr1/1K1/2R/Z2 Z2h 2
            B1C1 none 0 0 0
            fen k1r/1K1/2R/Z2 Z2h 1
            A4B3 none 87 8 0
            fen k1r/1K1/1ZR/3 Z2h 2
            H*A2 none 0 0 0
            fen k1r/hK1/1ZR/3 Zh 1
            B2C2 none 87 8 0
            fen k1r/h1K/1ZR/3 Zh 2
            C1B1 none 0 0 0
            fen kr1/h1K/1ZR/3 Zh 1
            B3A2 none 87 14 102
            fen kr1/Z1K/2R/3 ZHh 2
            A1A2 none 0 7 0
            B1A2 none 0 6 102
            fen 1r1/k1K/2R/3 ZHzh 1
            H*B2 none 2 7 0
            fen 1r1/kHK/2R/3 Zzh 2
            B1A1 none 2 8 0
            fen r2/kHK/2R/3 Zzh 1
            C2C1 none 2 7 0
            fen r1K/kH1/2R/3 Zzh 2
            A2B2 none 2 8 0
            fen r1K/1k1/2R/3 Zz2h 1
            Z*A3 none 0 7 0
            fen r1K/1k1/Z1R/3 z2h 2
            A1A2 none 2 8 0
            fen 2K/rk1/Z1R/3 z2h 1
            A3B2 none 2 8 0
            fen 2K/rZ1/2R/3 Kz2h 2
            Z*B4 none 0 7 0
            fen 2K/rZ1/2R/1z1 K2h 1
            C3C2 none 87 8 0
            fen 2K/rZR/3/1z1 K2h 2
            H*A1 none 0 0 0
            fen h1K/rZR/3/1z1 Kh 1
            C1B1 none 87 8 0
            fen hK1/rZR/3/1z1 Kh 2
            B4A3 none 0 0 0
            fen hK1/rZR/z2/3 Kh 1
            B1A1 none 87 14 0
            fen K2/rZR/z2/3 KHh 2
            toryo none 0 0 0
            fen kr1/1z1/3/ZRK Hh 1
            A4B3 none 0 8 0
            C4C3 none 0 8 0
            fen kr1/1z1/1Z1/1RK Hh 2
            A1A2 none 2 8 0
            fen 1r1/kz1/1Z1/1RK Hh 1
            B3A2 none 4 8 0
            C4C3 none 0 1 0
            fen 1r1/Zz1/3/1RK KHh 2
            B1A2 none 0 8 0
            fen 3/rz1/3/1RK KHzh 1
            K*B3 none 2 7 0
            fen 3/rz1/1K1/1RK Hzh 2
            A2A1 none 0 8 0
            fen r2/1z1/1K1/1RK Hzh 1
            B3B2 none 4 7 0
            fen r2/1K1/3/1RK ZHzh 2
            A1B2 none 0 7 0
            fen 3/1r1/3/1RK ZHzkh 1
            C4C3 none 0 7 0
            fen 3/1r1/2K/1R1 ZHzkh 2
            B2B1 none 87 6 0
            fen 1r1/3/2K/1R1 ZHzkh 1
            Z*C2 none 2 7 0
            fen 1r1/2Z/2K/1R1 Hzkh 2
            B1A1 none 0 8 0
            fen r2/2Z/2K/1R1 Hzkh 1
            C2B3 none 0 7 0
            fen r2/3/1ZK/1R1 Hzkh 2
            H*A3 none 2 7 0
            fen r2/3/hZK/1R1 Hzk 1
            B3C2 none 2 7 0
            fen r2/2Z/h1K/1R1 Hzk 2
            A1A2 none 2 8 0
            fen 3/r1Z/h1K/1R1 Hzk 1
            C2B3 none 0 8 0
            fen 3/r2/hZK/1R1 Hzk 2
            A2A1 none 2 8 0
            fen krz/3/1Z1/1RK Hh 1
            B4A3 none 0 1 0
            fen krz/3/RZ1/2K Hh 2
            C1B2 none 0 9 0
            fen kr1/1z1/RZ1/2K Hh 1
            A3B4 none 0 1 0
            fen 1r1/kz1/1ZK/1R1 Hh 2
            B1A1 none 4 7 0
            fen r2/kz1/1ZK/1R1 Hh 1
            B4C4 none 0 1 0
            fen r2/kz1/1ZK/2R Hh 2
            A1B1 none 4 7 0
            fen 1r1/kz1/1ZK/2R Hh 1
            C3C2 none 0 1 0
            fen 1r1/kzK/1Z1/2R Hh 2
            A2A1 none 2 8 0
            fen kr1/1zK/1Z1/2R Hh 1
            H*A2 none 0 1 0
            fen kr1/HzK/1Z1/2R h 2
            H*C3 none 2 8 0
            fen kr1/HzK/1Zh/2R - 1
            C4B4 none 0 1 0
            fen kr1/HzK/1Zh/1R1 - 2
            A1A2 none 2 8 0
            fen 1r1/kzK/1Zh/1R1 h 1
            C2B2 none 0 0 0
            fen 1r1/kK1/1Zh/1R1 Zh 2
            A2B2 none 4 7 0
            fen 1r1/1k1/1Zh/1R1 Zkh 1
            B4C3 none 0 1 0
            fen 1r1/1k1/1ZR/3 ZHkh 2
            H*C2 none 4 7 0
            fen 1r1/1kh/1ZR/3 ZHk 1
            C3B4 none 0 1 0
            fen 1r1/1kh/1Z1/1R1 ZHk 2
            K*C3 none 4 7 0
            fen 1r1/1kh/1Zk/1R1 ZH 1
            Z*A2 none 0 1 0
            fen 1r1/Zkh/1Zk/1R1 H 2
            B1A1 none 4 8 0
            fen r2/Zkh/1Zk/1R1 H 1
            B4A3 none 0 1 0
            fen r2/Zkh/RZk/3 H 2
            B2B1 none 4 8 0
            fen rk1/Z1h/RZk/3 H 1
            A2B1 none 0 0 0
            fen rZ1/2h/RZk/3 KH 2
            C3B3 none 0 8 0
            fen rZ1/2h/Rk1/3 KHz 1
            A3B3 none 0 0 0
            fen rZ1/2h/1R1/3 2KHz 2
            C2C3 none 0 7 0
            fen rZ1/3/1Rh/3 2KHz 1
            H*A2 none 0 0 0
            fen rZ1/H2/1Rh/3 2Kz 2
            A1B1 none 0 8 0
            fen 1r1/H2/1Rh/3 2K2z 1
            K*B2 none 0 1 0
            fen 1r1/HK1/1Rh/3 K2z 2
            B1C1 none 0 9 0
            fen 2r/HK1/1Rh/3 K2z 1
            K*C2 none 0 1 0
            fen 2r/HKK/1Rh/3 2z 2
            toryo none 0 0 0
            fen k1z/1rh/1Z1/1RK H 1
            H*C3 none 2 9 0
            fen k1z/1rh/1ZH/1RK - 2
            C2C3 none 2 9 0
            fen k1z/1r1/1Zh/1RK h 1
            C4C3 none 2 9 0
            fen k1z/1r1/1ZK/1R1 Hh 2
            A1A2 none 2 10 100
            C3C2 none 4 9 100
            B3C2 none 4 8 101
            fen 2z/kr1/1ZK/1R1 Hh 1
            B3A2 none 4 8 0
            B2A1 none 0 8 100
            fen 2z/Zr1/2K/1R1 KHh 2
            B2A2 none 0 7 0
            fen 2z/r2/2K/1R1 KHzh 1
            C3C2 none 0 7 0
            C3B3 none 0 6 0
            fen 2z/r1K/3/1R1 KHzh 2
            C1B2 none 2 7 0
            fen 3/rzK/3/1R1 KHzh 1
            K*B3 none 4 7 0
            C2B2 none 4 7 0
            fen 3/rzK/1K1/1R1 Hzh 2
            Z*C1 none 0 7 0
            B2A3 none 0 7 0
            fen 2z/rzK/1K1/1R1 Hh 1
            C2B2 none 4 7 0
            fen 2z/rK1/1K1/1R1 ZHh 2
            C1B2 none 0 7 0
            fen 3/rz1/1K1/1R1 ZHkh 1
            H*A3 none 4 7 0
            fen 3/rz1/HK1/1R1 Zkh 2
            A2A1 none 2 8 0
            fen r2/1z1/HK1/1R1 Zkh 1
            B3B2 none 4 7 0
            fen r2/1K1/H2/1R1 2Zkh 2
            A1B2 none 0 7 0
            fen 3/1r1/H2/1R1 2Z2kh 1
            Z*C1 none 0 7 0
            fen 2Z/1r1/H2/1R1 Z2kh 2
            B2C1 none 4 8 0
            fen 2r/3/H2/1R1 Zz2kh 1
            A3A2 none 0 0 0
            fen 2r/H2/3/1R1 Zz2kh 2
            C1B1 none 4 6 0
            fen 1r1/H2/3/1R1 Zz2kh 1
            B4C3 none 0 0 0
            fen 1r1/H2/2R/3 Zz2kh 2
            B1C1 none 6 6 0
            fen 2r/H2/2R/3 Zz2kh 1
            Z*B2 none 87 7 0
            fen 2r/HZ1/2R/3 z2kh 2
            C1B1 none 4 8 0
            fen 1r1/HZ1/2R/3 z2kh 1
            A2A1+ none 87 7 0
            fen Nr1/1Z1/2R/3 z2kh 2
            toryo none 0 0 0
            fen kr1/1z1/2K/ZR1 Hh 2
            A1A2 none 4 8 0
            fen 1r1/kz1/2K/ZR1 Hh 1
            C3C2 none 0 7 0
            fen 1r1/kzK/3/ZR1 Hh 2
            B1C2 none 4 8 0
            fen 3/kzr/3/ZR1 Hkh 1
            A4B3 none 0 8 0
            fen 3/kzr/1Z1/1R1 Hkh 2
            C2B1 none 4 8 0
            fen 1r1/kz1/1Z1/1R1 Hkh 1
            B3A4 none 0 7 0
            fen 1r1/kz1/3/ZR1 Hkh 2
            B1A1 none 6 7 0
            fen r2/kz1/3/ZR1 Hkh 1
            A4B3 none 0 0 0
            fen r2/kz1/1Z1/1R1 Hkh 2
            B2A3 none 4 7 0
            fen r2/k2/zZ1/1R1 Hkh 1
            B4C3 none 0 0 0
            fen r2/k2/zZR/3 Hkh 2
            A2B2 none 6 8 0
            fen r2/1k1/zZR/3 Hkh 1
            H*A2 none 0 0 0
            fen r2/Hk1/zZR/3 kh 2
            A1B1 none 10 8 0
            fen 1r1/Hk1/zZR/3 kh 1
            A2A1+ none 0 0 0
            fen Nr1/1k1/zZR/3 kh 2
            B1A1 none 87 9 0
            fen r2/1k1/zZR/3 k2h 1
            B3C2 none 0 0 0
            fen r2/1kZ/z1R/3 k2h 2
            A1A2 none 87 8 0
            fen 3/rkZ/z1R/3 k2h 1
            C2B1 none 0 0 0
            fen 1Z1/rk1/z1R/3 k2h 2
            A2A1 none 87 9 0
            fen rZ1/1k1/z1R/3 k2h 1
            B1C2 none 87 9 0
            fen 3/r1K/zK1/1R1 Hzh 1
            B3A3 none 4 8 0
            fen 3/r1K/K2/1R1 ZHzh 2
            A2B1 none 0 0 0
            fen 1r1/2K/K2/1R1 ZHzh 1
            C2C3 none 4 6 0
            fen 1r1/3/K1K/1R1 ZHzh 2
            Z*B2 none 0 7 0
            fen 1r1/1z1/K1K/1R1 ZHh 1
            B4B3 none 4 7 0
            C3B3 none 4 6 0
            fen 1r1/1z1/KRK/3 ZHh 2
            H*A1 none 0 7 0
            fen hr1/1z1/KRK/3 ZH 1
            C3C2 none 4 8 0
            fen hr1/1zK/KR1/3 ZH 2
            B2A3 none 0 8 0
            fen hr1/2K/zR1/3 ZHk 1
            Z*C3 none 4 7 0
            fen hr1/2K/zRZ/3 Hk 2
            A1A2 none 0 8 0
            fen 1r1/h1K/zRZ/3 Hk 1
            C2C1 none 2 8 0
            fen 1rK/h2/zRZ/3 Hk 2
            B1C1 none 4 9 0
            fen 2r/h2/zRZ/3 H2k 1
            B3A2 none 2 8 0
            fen 2r/R2/z1Z/3 2H2k 2
            K*B1 none 2 9 0
            fen 1kr/R2/z1Z/3 2Hk 1
            A2A3 none 2 8 0
            fen 1kr/3/R1Z/3 Z2Hk 2
            K*C4 none 0 7 0
            fen 1kr/3/R1Z/2k Z2H 1
            A3B3 none 2 8 0
            fen 1kr/3/1RZ/2k Z2H 2
            C4C3 none 2 8 0
            fen 1kr/3/1Rk/3 Z2Hz 1
            B3C3 none 2 7 0
            fen 1kr/3/2R/3 ZK2Hz 2
            B1A1 none 0 0 0
            fen k1r/3/2R/3 ZK2Hz 1
            C3B3 none 6 6 0
            fen k1r/3/1R1/3 ZK2Hz 2
            Z*A2 none 0 0 0
            fen k1r/z2/1R1/3 ZK2H 1
            B3C3 none 10 8 0
            fen k1r/z2/2R/3 ZK2H 2
            A2B3 none 0 0 0
            fen k1r/3/1zR/3 ZK2H 1
            C3B3 none 6 6 0
            fen k1r/3/1R1/3 2ZK2H 2
            C1B1 none 0 0 0
            fen kr1/3/1R1/3 2ZK2H 1
            Z*B2 none 87 7 0
            fen kr1/1Z1/1R1/3 ZK2H 2
            A1A2 none 0 0 0
            fen 1r1/kZ1/1R1/3 ZK2H 1
            Z*C2 none 87 7 0
            fen 1r1/kZZ/1R1/3 K2H 2
            toryo none 0 0 0
            fen krz/3/1ZK/1R1 Hh 1
            H*A2 none 4 7 0
            fen krz/H2/1ZK/1R1 h 2
            A1A2 none 2 8 0
            fen 1rz/k2/1ZK/1R1 2h 1
            C3C2 none 2 8 0
            fen 1rz/k1K/1Z1/1R1 2h 2
            C1B2 none 2 8 0
            fen 1r1/kzK/1Z1/1R1 2h 1
            C2C1 none 2 8 0
            fen 1rK/kz1/1Z1/1R1 2h 2
            B1A1 none 6 8 0
            fen r1K/kz1/1Z1/1R1 2h 1
            C1B1 none 2 8 0
            fen rK1/kz1/1Z1/1R1 2h 2
            A1B1 none 6 8 0
            fen 1r1/kz1/1Z1/1R1 k2h 1
            B3A2 none 0 0 0
            fen 1r1/Zz1/3/1R1 Kk2h 2
            B1A2 none 6 8 0
            fen 3/rz1/3/1R1 Kzk2h 1
            K*B3 none 0 0 0
            fen 3/rz1/1K1/1R1 zk2h 2
            A2A1 none 6 7 0
            fen r2/1z1/1K1/1R1 zk2h 1
            B4C4 none 0 0 0
            fen r2/1z1/1K1/2R zk2h 2
            B2C1 none 6 7 0
            fen r1z/3/1K1/2R zk2h 1
            B3A3 none 0 0 0
            fen r1z/3/K2/2R zk2h 2
            A1B1 none 6 7 0
            fen 1rz/3/K2/2R zk2h 1
            A3B3 none 87 12 0
            fen 1rz/3/1K1/2R zk2h 2
            B1A1 none 6 8 0
            fen k1z/1rZ/3/1RK 2H 2
            B2C2 none 2 9 0
            fen k1z/2r/3/1RK 2Hz 1
            C4C3 none 0 8 0
            fen k1z/2r/2K/1R1 2Hz 2
            C2B1 none 2 8 0
            fen krz/3/2K/1R1 2Hz 1
            C3B3 none 2 8 0
            fen krz/3/1K1/1R1 2Hz 2
            A1A2 none 2 8 0
            fen 1rz/k2/1K1/1R1 2Hz 1
            B4A4 none 0 7 0
            fen 1rz/k2/1K1/R2 2Hz 2
            B1A1 none 2 8 0
            fen r1z/k2/1K1/R2 2Hz 1
            H*B1 none 0 7 0
            fen rHz/k2/1K1/R2 Hz 2
            A1B1 none 4 8 0
            fen 1rz/k2/1K1/R2 Hzh 1
            B3A3 none 0 0 0
            fen 1rz/k2/K2/R2 Hzh 2
            A2A3 none 8 7 0
            fen 1rz/3/k2/R2 Hzkh 1
            A4A3 none 0 0 0
            fen 1rz/3/R2/3 KHzkh 2
            C1B2 none 4 6 0
            fen 1r1/1z1/R2/3 KHzkh 1
            A3A4 none 0 0 0
            fen 1r1/1z1/3/R2 KHzkh 2
            K*A3 none 4 6 0
            fen 1r1/1z1/k2/R2 KHzh 1
            A4B4 none 0 0 0
            fen 1r1/1z1/k2/1R1 KHzh 2
            Z*C3 none 6 6 0
            fen 1r1/1z1/k1z/1R1 KHh 1
            B4C4 none 0 0 0
            fen 1r1/1z1/k1z/2R KHh 2
            H*B3 none 6 7 0
            fen 1r1/1z1/khz/2R KH 1
            K*C1 none 0 0 0
            fen 1rK/1z1/khz/2R H 2
            B1C1 none 87 9 0
            fen 2r/1z1/khz/2R Hk 1
            H*A1 none 0 0 0
            fen H1r/1z1/khz/2R k 2
            C1B1 none 87 10 0
            fen Hr1/1z1/khz/2R k 1
            toryo none 0 0 0
            fen 3/rK1/3/1R1 ZKHzh 2
            A2B2 none 0 7 0
            fen 3/1r1/3/1R1 ZKHzkh 1
            Z*C1 none 0 6 0
            fen 2Z/1r1/3/1R1 KHzkh 2
            B2C1 none 4 7 0
            B2A1 none 4 7 100
            fen 2r/3/3/1R1 KH2zkh 1
            H*C2 none 0 0 0
            fen 2r/2H/3/1R1 K2zkh 2
            C1B1 none 6 8 0
            fen 1r1/2H/3/1R1 K2zkh 1
            C2C1+ none 0 0 0
            fen 1rN/3/3/1R1 K2zkh 2
            B1C1 none 6 7 0
            fen 2r/3/3/1R1 K2zk2h 1
            B4A3 none 0 0 0
            fen 2r/3/R2/3 K2zk2h 2
            C1B1 none 6 6 0
            fen 1r1/3/R2/3 K2zk2h 1
            A3B3 none 0 0 0
            fen 1r1/3/1R1/3 K2zk2h 2
            B1A1 none 6 6 0
            fen r2/3/1R1/3 K2zk2h 1
            B3C2 none 0 0 0
            fen r2/2R/3/3 K2zk2h 2
            K*B1 none 6 8 0
            fen rk1/2R/3/3 K2z2h 1
            C2B3 none 0 0 0
            fen rk1/3/1R1/3 K2z2h 2
            B1B2 none 6 6 0
            fen r2/1k1/1R1/3 K2z2h 1
            B3C3 none 0 0 0
            fen r2/1k1/2R/3 K2z2h 2
            A1B1 none 8 7 0
            fen 1r1/1k1/2R/3 K2z2h 1
            C3B4 none 0 0 0
            fen 1r1/1k1/3/1R1 K2z2h 2
            B1A1 none 8 6 0
            fen r2/1k1/3/1R1 K2z2h 1
            B4C3 none 87 11 0
            fen 2r/3/3/2R KH2zkh 2
            C1B1 none 4 6 0
            fen 1r1/3/3/2R KH2zkh 1
            C4B3 none 0 0 0
            fen 1r1/3/1R1/3 KH2zkh 2
            B1A1 none 4 6 0
            fen r2/3/1R1/3 KH2zkh 1
            K*B2 none 0 6 0
            fen r2/1K1/1R1/3 H2zkh 2
            Z*A4 none 4 7 0
            fen r2/1K1/1R1/z2 Hzkh 1
            B3C2 none 87 7 0
            fen r2/1KR/3/z2 Hzkh 2
            K*B1 none 4 9 0
            fen rk1/1KR/3/z2 Hzh 1
            B2B1 none 87 8 0
            fen rK1/2R/3/z2 KHzh 2
            A1A2 none 0 14 0
            fen 1K1/r1R/3/z2 KHzh 1
            C2C1 none 87 14 0
            fen 1KR/r2/3/z2 KHzh 2
            A2A3 none 87 8 0
            fen rz1/1k1/2R/3 Kz2h 1
            K*A3 none 0 0 0
            fen rz1/1k1/K1R/3 z2h 2
            B1C2 none 8 8 0
            fen r2/1kz/K1R/3 z2h 1
            C3B4 none 0 0 0
            fen r2/1kz/K2/1R1 z2h 2
            A1B1 none 6 8 0
            fen 1r1/1kz/K2/1R1 z2h 1
            B4C3 none 0 0 0
            fen 1r1/1kz/K1R/3 z2h 2
            Z*C1 none 8 8 0
            fen 1rz/1kz/K1R/3 2h 1
            C3C4 none 0 0 0
            fen 1rz/1kz/K2/2R 2h 2
            H*B3 none 8 8 0
            fen 1rz/1kz/Kh1/2R h 1
            C4C3 none 0 0 0
            fen 1rz/1kz/KhR/3 h 2
            H*A1 none 8 8 0
            fen hrz/1kz/KhR/3 - 1
            C3C4 none 0 0 0
            fen hrz/1kz/Kh1/2R - 2
            A1A2 none 8 10 0
            fen 1rz/hkz/Kh1/2R - 1
            A3A4 none 0 0 0
            fen 1rz/hkz/1h1/K1R - 2
            B1A1 none 8 10 0
            fen r1z/hkz/1h1/K1R - 1
            C4C3 none 0 0 0
            fen r1z/hkz/1hR/K2 - 2
            A1B1 none 8 10 0
            A2A3 none 8 10 0
            fen 1rz/hkz/1hR/K2 - 1
            C3C4 none 0 0 0
            fen r1z/1kz/hhR/K2 - 1
            A4A3 none 0 0 0
            fen r1z/1kz/KhR/3 H 2
            A1B1 none 6 9 0
            fen 1rz/1kz/KhR/3 H 1
            C3C4 none 0 0 0
            fen 1rz/1kz/Kh1/2R H 2
            B1A1 none 6 9 0
            fen r1z/1kz/Kh1/2R H 1
            H*C3 none 0 9 0
            fen r1z/1kz/KhH/2R - 2
            B2A2 none 6 9 0
            fen r1z/k1z/KhH/2R - 1
            C3C2 none 2 9 0
            fen r1z/k1H/Kh1/2R Z 2
            A2A3 none 4 8 0
            fen r1z/2H/kh1/2R Zk 1
            C2C1+ none 2 8 0
            fen r1N/3/kh1/2R 2Zk 2
            A1B2 none 2 8 0
            fen 2N/1r1/kh1/2R 2Zk 1
            Z*C3 none 2 8 0
            fen 2N/1r1/khZ/2R Zk 2
            B2A1 none 2 8 0
            fen r1N/3/khZ/2R Zk 1
            C3B2 none 6 8 0
            fen r1N/1Z1/kh1/2R Zk 2
            A1B2 none 6 9 0
            fen 2N/1r1/kh1/2R Zzk 1
            C1B1 none 0 0 0
            fen 1N1/1r1/kh1/2R Zzk 2
            B2B1 none 8 8 0
            fen 1r1/3/kh1/2R Zzkh 1
            C4C3 none 0 0 0
            fen 1r1/3/khR/3 Zzkh 2
            Z*B2 none 87 7 0
            fen 1r1/1z1/khR/3 Zkh 1
            C3C4 none 0 0 0
            fen 1r1/1z1/kh1/2R Zkh 2
            A3A4 none 87 8 0
            fen 1r1/1z1/1h1/k1R Zkh 1
            C4B3 none 0 0 0
            fen 1r1/1z1/1R1/k2 ZHkh 2
            K*A3 none 87 7 0
            fen 1r1/1z1/kR1/k2 ZHh 1
            B3C4 none 0 0 0
            fen 1r1/1z1/k2/k1R ZHh 2
            H*C2 none 87 8 0
            fen 1r1/1zh/k2/k1R ZH 1
            Z*A2 none 0 0 0
            fen 1r1/Zzh/k2/k1R H 2
            B1A1 none 87 10 0
            fen r2/Zzh/k2/k1R H 1
            A2B1 none 0 0 0
            fen rZ1/1zh/k2/k1R H 2
            C2C3 none 87 14 0
            fen rZ1/1z1/k1h/k1R H 1
            toryo none 0 0 0
            fen 2z/r2/1K1/1R1 KHzh 2
            C1B2 none 0 7 0
            fen 3/rz1/1K1/1R1 KHzh 1
            B3B2 none 4 7 0
            fen 1r1/1z1/KK1/1R1 ZHh 2
            B2A3 none 0 7 0
            fen 1r1/3/zK1/1R1 ZHkh 1
            B3A3 none 4 6 0
            fen 1r1/3/K2/1R1 2ZHkh 2
            B1C1 none 0 0 0
            fen 2r/3/K2/1R1 2ZHkh 1
            A3B3 none 4 6 0
            fen 2r/3/1K1/1R1 2ZHkh 2
            C1B1 none 0 0 0
            fen 1r1/3/1K1/1R1 2ZHkh 1
            Z*B2 none 4 6 0
            fen 1r1/1Z1/1K1/1R1 ZHkh 2
            B1C2 none 0 0 0
            fen 3/1Zr/1K1/1R1 ZHkh 1
            H*C3 none 4 6 0
            fen 3/1Zr/1KH/1R1 Zkh 2
            C2B1 none 0 0 0
            fen 1r1/1Z1/1KH/1R1 Zkh 1
            B2A3 none 4 7 0
            fen 1r1/3/ZKH/1R1 Zkh 2
            K*C4 none 0 7 0
            fen 1r1/3/ZKH/1Rk Zh 1
            B4C4 none 8 8 0
            fen 1r1/3/ZKH/2R ZKh 2
            H*C2 none 0 0 0
            fen 1r1/2h/ZKH/2R ZK 1
            B3B2 none 87 8 0
            fen 1r1/1Kh/Z1H/2R ZK 2
            B1A1 none 0 0 0
            fen r2/1Kh/Z1H/2R ZK 1
            B2C2 none 87 8 0
            fen r2/2K/Z1H/2R ZKH 2
            A1A2 none 0 0 0
            fen 3/r1K/Z1H/2R ZKH 1
            A3B2 none 87 8 0
            fen 3/rZK/2H/2R ZKH 2
            A2B1 none 0 0 0
            fen 1r1/1ZK/2H/2R ZKH 1
            B2A3 none 87 8 0
            C4B3 none 87 8 0
            fen 1r1/2K/Z1H/2R ZKH 2
            B1A2 none 0 0 0
            fen 1r1/1ZK/1RH/3 ZKH 2
            toryo none 0 0 0
            fen 2z/kr1/1Z1/1RK Hh 1
            B3A2 none 4 9 100
            fen 2z/Zr1/3/1RK KHh 2
            B2A2 none 0 7 100
            fen 2z/r2/3/1RK KHzh 1
            K*A3 none 2 7 100
            fen 2z/r2/K2/1RK Hzh 2
            A2A1 none 0 12 100
            fen r1z/3/K2/1RK Hzh 1
            A3B3 none 2 7 100
            fen r1z/3/1K1/1RK Hzh 2
            A1B1 none 2 7 100
            fen 1rz/3/1K1/1RK Hzh 1
            B3A3 none 2 7 100
            fen 1rz/3/K2/1RK Hzh 2
            B1A1 none 0 8 100
            B1B2 none 0 7 100
            fen 1rz/3/2K/1RK Hzh 2
            H*B2 none 2 7 100
            fen 1rz/1h1/2K/1RK Hz 1
            B4A3 none 2 8 100
            fen 1rz/1h1/R1K/2K Hz 2
            B1A1 none 0 9 100
            fen r1z/1h1/R1K/2K Hz 1
            H*C2 none 4 8 100
            fen r1z/1hH/R1K/2K z 2
            B2B3 none 0 9 100
            fen r1z/2H/RhK/2K z 1
            C3B3 none 8 8 100
            fen r1z/2H/RK1/2K Hz 2
            A1B1 none 0 0 0
            fen 1rz/2H/RK1/2K Hz 1
            H*A2 none 10 8 100
            fen 1rz/H1H/RK1/2K z 2
            Z*B4 none 0 10 100
            fen 1rz/H1H/RK1/1zK - 1
            C4B4 none 87 9 100
            fen 1rz/H1H/RK1/1K1 Z 2
            B1C2 none 0 0 0
            fen 2z/H1r/RK1/1K1 Zh 1
            Z*B1 none 87 8 100
            fen 1Zz/H1r/RK1/1K1 h 2
            C2B1 none 0 14 100
            fen 1rz/H2/RK1/1K1 zh 1
            B3B2 none 87 14 100
            fen 1rz/HK1/R2/1K1 zh 2
            C1B2 none 87 8 100
            fen 1r1/Hz1/R2/1K1 zkh 1
            A3B3 none 0 0 0
            fen 1r1/Hz1/1R1/1K1 zkh 2
            Z*C2 none 87 7 100
            fen 1r1/Hzz/1R1/1K1 kh 1
            B3A4 none 0 0 0
            fen 1r1/Hzz/3/RK1 kh 2
            K*A3 none 87 8 100
            fen 1r1/Hzz/k2/RK1 h 1
            toryo none 0 0 0
            fen 2z/1r1/K2/1RK Hzh 1
            B4A4 none 2 7 100
            fen 2z/1r1/K2/R1K Hzh 2
            B2C2 none 0 8 100
            fen 2z/2r/K2/R1K Hzh 1
            A3B3 none 2 8 100
            fen 2z/2r/1K1/R1K Hzh 2
            C2B1 none 0 8 100
            fen 1rz/3/1K1/R1K Hzh 1
            B3A3 none 2 7 100
            fen 1rz/3/K2/R1K Hzh 2
            B1A1 none 0 7 100
            fen r1z/3/K2/R1K Hzh 1
            H*A2 none 2 7 100
            fen r1z/H2/K2/R1K zh 2
            A1B1 none 0 9 100
            fen 1rz/H2/K2/R1K zh 1
            C4C3 none 2 8 100
            fen 1rz/H2/K1K/R2 zh 2
            B1B2 none 2 11 100
            fen 2z/Hr1/K1K/R2 zh 1
            C3B3 none 2 8 100
            fen 2z/Hr1/KK1/R2 zh 2
            B2B1 none 0 9 100
            fen 1rz/H2/KK1/R2 zh 1
            B3C3 none 2 8 100
            fen 1rz/H2/K2/RK1 zh 2
            C1B2 none 2 8 100
            fen 1r1/Hz1/K2/RK1 zh 1
            A2A1 none 2 8 100
            fen Hr1/1z1/K2/RK1 zh 2
            B2A1 none 4 8 100
            fen zr1/3/K2/RK1 z2h 1
            A3B3 none 0 0 0
            fen zr1/3/1K1/RK1 z2h 2
            Z*C2 none 4 8 100
            fen zr1/2z/1K1/RK1 2h 1
            B4C4 none 0 0 0
            fen zr1/2z/1K1/R1K 2h 2
            A1B2 none 6 8 100
            fen 1r1/1zz/1K1/R1K 2h 1
            B3B2 none 2 8 100
            fen 1r1/1Kz/3/R1K Z2h 2
            B1B2 none 2 8 100
            fen 3/1rz/3/R1K Zk2h 1
            Z*A1 none 0 0 0
            fen Z2/1rz/3/R1K k2h 2
            B2A1 none 6 8 100
            fen r2/2z/3/R1K zk2h 1
            C4C3 none 0 0 0
            fen r2/2z/2K/R2 zk2h 2
            A1B2 none 6 7 100
            fen 3/1rz/2K/R2 zk2h 1
            C3C2 none 0 0 0
            fen 3/1rK/3/R2 Zzk2h 2
            B2C2 none 6 7 100
            fen 3/2r/3/R2 Zz2k2h 1
            A4A3 none 0 0 0
            fen 3/2r/R2/3 Zz2k2h 2
            C2B1 none 10 6 100
            fen 1r1/3/R2/3 Zz2k2h 1
            A3B3 none 0 0 0
            fen 1r1/3/1R1/3 Zz2k2h 2
            B1A1 none 6 6 100
            fen r2/3/1R1/3 Zz2k2h 1
            Z*C1 none 0 0 0
            fen r1Z/3/1R1/3 z2k2h 2
            A1B1 none 6 10 100
            fen 1rZ/3/1R1/3 z2k2h 1
            C1B2 none 0 0 0
            fen 1r1/1Z1/1R1/3 z2k2h 2
            Z*A1 none 8 6 100
            fen zr1/1Z1/1R1/3 2k2h 1
            B2A1 none 0 0 0
            fen Zr1/3/1R1/3 Z2k2h 2
            B1A1 none 6 6 100
            fen 1rz/1Z1/1R1/3 2k2h 1
            B2C1 none 0 0 0
            fen 1rZ/3/1R1/3 Z2k2h 2
            B1C1 none 6 6 100
            fen 2r/3/1R1/3 Zz2k2h 1
            Z*A1 none 0 0 0
            fen Z1r/3/1R1/3 z2k2h 2
            Z*C2 none 6 7 100
            fen Z1r/2z/1R1/3 2k2h 1
            B3A2 none 0 0 0
            fen Z1r/R1z/3/3 2k2h 2
            C2B1 none 6 8 100
            fen Zzr/R2/3/3 2k2h 1
            A2B3 none 87 11 100
            fen Zzr/3/1R1/3 2k2h 2
            B1C2 none 6 8 100
            fen k1H/1rh/1Z1/1RK Z 1
            B3A4 none 6 9 100
            B4A4 none 6 9 100
            fen k1H/1rh/3/ZRK Z 2
            A1A2 none 0 9 100
            fen 2H/krh/3/ZRK Z 1
            A4B3 none 6 10 100
            fen 2H/krh/1Z1/1RK Z 2
            A2A1 none 0 9 100
            fen 1kH/1rh/3/ZRK Z 1
            C4C3 none 4 9 100
            fen 1kH/1rh/2K/ZR1 Z 2
            B1C1 none 2 9 100
            fen 2k/1rh/2K/ZR1 Zh 1
            C3C2 none 4 8 100
            fen 2k/1rK/3/ZR1 ZHh 2
            C1C2 none 0 8 100
            fen 3/1rk/3/ZR1 ZHkh 1
            A4B3 none 0 7 100
            fen 3/1rk/1Z1/1R1 ZHkh 2
            B2A1 none 0 7 100
            fen r2/2k/1Z1/1R1 ZHkh 1
            B3C2 none 4 7 100
            fen r2/2Z/3/1R1 ZKHkh 2
            A1B2 none 0 7 100
            fen 3/1rZ/3/1R1 ZKHkh 1
            C2B1 none 4 7 100
            fen 1Z1/1r1/3/1R1 ZKHkh 2
            K*B3 none 87 7 100
            fen 1Z1/1r1/1k1/1R1 ZKHh 1
            B4A4 none 4 8 100
            fen 1Z1/1r1/1k1/R2 ZKHh 2
            H*A3 none 87 7 100
            fen 1Z1/1r1/hk1/R2 ZKH 1
            toryo none 0 0 0
            fen 1kH/1r1/ZZ1/1RK h 2
            B2A1 none 0 10 100
            fen rkH/3/ZZ1/1RK h 1
            B4C3 none 4 10 100
            fen rkH/3/ZZR/2K h 2
            B1C1 none 0 0 0
            fen r1k/3/ZZR/2K 2h 1
            A3B2 none 6 9 100
            fen r1k/1Z1/1ZR/2K 2h 2
            A1B1 none 0 0 0
            fen 1rk/1Z1/1ZR/2K 2h 1
            B2C1 none 6 13 100
            fen 1rZ/3/1ZR/2K K2h 2
            B1C1 none 0 0 0
            fen 2r/3/1ZR/2K Kz2h 1
            K*C2 none 2 11 100
            fen 2r/2K/1ZR/2K z2h 2
            C1B1 none 0 0 0
            fen 1r1/2K/1ZR/2K z2h 1
            C2B2 none 87 8 100
            fen 1r1/1K1/1ZR/2K z2h 2
            B1C1 none 0 0 0
            fen 2r/1K1/1ZR/2K z2h 1
            B3A2 none 87 8 100
            fen 2r/ZK1/2R/2K z2h 2
            Z*B4 none 0 0 0
            fen 2r/ZK1/2R/1zK 2h 1
            C3B3 none 87 10 100
            fen 2r/ZK1/1R1/1zK 2h 2
            B4A3 none 0 0 0
            fen 2r/ZK1/zR1/2K 2h 1
            B2B1 none 87 14 100
            fen 1Kr/Z2/zR1/2K 2h 2
            toryo none 0 0 0
            fen 2k/1r1/3/ZRK Z2h 1
            A4B3 none 2 8 100
            fen 2k/1r1/1Z1/1RK Z2h 2
            H*C2 none 0 8 100
            fen 2k/1rh/1Z1/1RK Zh 1
            B3C2 none 4 9 100
            fen 2k/1rZ/3/1RK ZHh 2
            C1C2 none 0 8 100
            fen 3/1rk/3/1RK ZHzh 1
            B4A4 none 0 8 100
            fen 3/1rk/3/R1K ZHzh 2
            B2A1 none 4 7 100
            fen r2/2k/3/R1K ZHzh 1
            A4A3 none 0 11 100
            fen r2/2k/R2/2K ZHzh 2
            A1B1 none 0 7 100
            fen 1r1/2k/R2/2K ZHzh 1
            A3B3 none 0 10 100
            fen 1r1/2k/1R1/2K ZHzh 2
            B1C1 none 0 7 100
            fen 2r/2k/1R1/2K ZHzh 1
            B3A2 none 87 8 100
            fen 2r/R1k/3/2K ZHzh 2
            Z*B2 none 0 10 100
            fen 2r/Rzk/3/2K ZHh 1
            Z*A1 none 87 14 100
            fen Z1r/Rzk/3/2K Hh 2
            C2C3 none 4 11 100
            fen Z1r/Rz1/2k/2K Hh 1
            A1B2 none 87 10 100
            fen 2r/RZ1/2k/2K ZHh 2
            C1C2 none 0 0 0
            fen 3/RZr/2k/2K ZHh 1
            A2A1 none 87 14 100
            fen R2/1Zr/2k/2K ZHh 2
            C2B3 none 87 8 100
            fen 1kz/1r1/1ZK/1R1 Hh 2
            B1A1 none 0 13 100
            fen r1z/k2/1ZK/1R1 Hh 1
            B3A2 none 4 8 100
            fen r1z/Z2/2K/1R1 KHh 2
            A1A2 none 0 8 100
            fen r1Z/3/3/1R1 KHzkh 1
            B4B3 none 0 7 100
            fen r1Z/3/1R1/3 KHzkh 2
            Z*A2 none 0 7 100
            fen r1Z/z2/1R1/3 KHkh 1
            B3C2 none 0 8 100
            fen r1Z/z1R/3/3 KHkh 2
            A2B1 none 0 9 100
            fen rzZ/2R/3/3 KHkh 1
            C2B3 none 0 9 100
            fen rzZ/3/1R1/3 KHkh 2
            B1A2 none 0 8 100
            fen k1z/1rK/1Z1/1R1 Hh 2
            B2B1 none 0 9 100
            fen krz/2K/1Z1/1R1 Hh 1
            B4C3 none 4 11 101
            fen krz/2K/1ZR/3 Hh 2
            A1A2 none 0 12 100
            fen 1rz/k1K/1ZR/3 Hh 1
            C2C1 none 4 9 100
            fen 1rK/k2/1ZR/3 ZHh 2
            B1C1 none 0 8 100
            fen 2r/k2/1ZR/3 ZHkh 1
            B3A2 none 4 10 101
            fen 2r/Z2/2R/3 ZKHkh 2
            H*C2 none 0 7 100
            fen 2r/Z1h/2R/3 ZKHk 1
            C3B3 none 4 15 101
            fen 2r/Z1h/1R1/3 ZKHk 2
            K*B2 none 0 9 100
            fen 2r/Zkh/1R1/3 ZKH 1
            B3A3 none 4 9 101
            fen 2r/Zkh/R2/3 ZKH 2
            C2C3 none 0 12 100
            fen 2r/Zk1/R1h/3 ZKH 1
            Z*B3 none 4 11 101
            A3B4 none 4 8 101
            A2B3 none 4 8 101
            fen 2r/Zk1/RZh/3 KH 2
            B2B3 none 0 9 100
            fen 2r/Z2/Rkh/3 KHz 1
            A3B3 none 87 8 100
            fen 2r/Z2/1Rh/3 2KHz 2
            Z*B1 none 0 0 0
            Z*C2 none 0 0 0
            fen 1zr/Z2/1Rh/3 2KH 1
            H*B2 none 87 8 100
            fen 1zr/ZH1/1Rh/3 2K 2
            B1C2 none 0 9 100
            fen 2r/ZHz/1Rh/3 2K 1
            B3C3 none 87 10 100
            fen 2r/ZHz/2R/3 2KH 2
            C2B3 none 0 0 0
            fen 2r/ZH1/1zR/3 2KH 1
            A2B1 none 87 8 100
            fen 1Zr/1H1/1zR/3 2KH 2
            B3C4 none 0 0 0
            fen 1Zr/1H1/2R/2z 2KH 1
            B1A2 none 87 10 100
            fen 2r/ZH1/2R/2z 2KH 2
            C4B3 none 87 14 100
            fen 2r/1HZ/2R/2z 2KH 2
            C4B3 none 87 11 100
            fen 2r/1HZ/1zR/3 2KH 1
            B2B1+ none 87 8 100
            fen 1Nr/2Z/1zR/3 2KH 2
            toryo none 0 0 0
            fen 2H/1rh/kZ1/1RK Z 1
            Z*A1 none 6 9 100
            fen Z1H/1rh/kZ1/1RK - 2
            B2A1 none 0 9 100
            fen r1H/2h/kZ1/1RK z 1
            B4A3 none 4 9 100
            fen r1H/2h/RZ1/2K Kz 2
            Z*B2 none 0 0 0
            fen r1H/1zh/RZ1/2K K 1
            A3A4 none 4 10 100
            fen r1H/1zh/1Z1/R1K K 2
            A1B1 none 0 9 100
            fen 1rH/1zh/1Z1/R1K K 1
            K*A2 none 6 9 100
            fen 1rH/Kzh/1Z1/R1K - 2
            B1C1 none 0 9 100
            fen 2r/Kzh/1Z1/R1K h 1
            A4B4 none 6 9 100
            fen 2r/Kzh/1Z1/1RK h 2
            C2C3 none 0 9 100
            fen 2r/Kz1/1Zh/1RK h 1
            A2B2 none 6 8 100
            fen 2r/1K1/1Zh/1RK Zh 2
            C3C4+ none 0 8 100
            fen 2r/1K1/1Z1/1Rn Zkh 1
            B3C4 none 4 8 100
            fen 2r/1K1/3/1RZ ZHkh 2
            C1B2 none 0 7 100
            fen 3/1r1/3/1RZ ZH2kh 1
            C4B3 none 0 7 100
            fen 3/1r1/1Z1/1R1 ZH2kh 2
            B2A1 none 0 7 100
            fen r2/3/1Z1/1R1 ZH2kh 1
            B3C2 none 2 7 100
            A1B1 none 0 7 100
            fen r2/2Z/3/1R1 ZH2kh 2
            A1A2 none 0 7 100
            fen 3/r1Z/3/1R1 ZH2kh 1
            C2B3 none 0 7 100
            fen 3/r2/1Z1/1R1 ZH2kh 2
            A2A1 none 0 7 100
            A2B1 none 0 8 100
            fen 1r1/3/1Z1/1R1 ZH2kh 1
            B3A4 none 0 10 100
            fen 1r1/3/3/ZR1 ZH2kh 2
            B1A1 none 0 7 100
            fen r2/3/3/ZR1 ZH2kh 1
            A4B3 none 0 7 100
            B4A3 none 0 7 100
            fen r2/3/R2/Z2 ZH2kh 2
            K*A2 none 0 7 100
            fen r2/k2/R2/Z2 ZHkh 1
            A3B3 none 0 8 100
            fen r2/k2/1R1/Z2 ZHkh 2
            A1B1 none 4 7 100
            fen 1r1/k2/1R1/Z2 ZHkh 1
            B3C3 none 0 7 100
            fen 1r1/k2/2R/Z2 ZHkh 2
            B1A1 none 0 7 100
            fen r2/k2/2R/Z2 ZHkh 1
            C3C2 none 0 7 100
            fen r2/k1R/3/Z2 ZHkh 2
            K*B1 none 0 9 100
            fen rk1/k1R/3/Z2 ZHh 1
            Z*C1 none 87 8 100
            fen rkZ/k1R/3/Z2 Hh 2
            A2A3 none 4 11 100
            fen rkZ/2R/k2/Z2 Hh 1
            C1B2 none 87 10 100
            fen rk1/1ZR/k2/Z2 Hh 2
            B1B2 none 4 14 100
            fen r2/1kR/k2/Z2 Hzh 1
            C2C1 none 87 14 100
            fen r1R/1k1/k2/Z2 Hzh 2
            Z*B1 none 87 8 100
            fen k1r/3/1Z1/1RK Z2h 1
            B3C2 none 2 8 100
            fen k1r/2Z/3/1RK Z2h 2
            C1C2 none 2 8 100
            fen k2/2r/3/1RK Zz2h 1
            B4A3 none 0 0 0
            fen k2/2r/R2/2K Zz2h 2
            Z*B2 none 87 8 100
            fen k2/1zr/R2/2K Z2h 1
            A3B4 none 0 0 0
            fen k2/1zr/3/1RK Z2h 2
            H*B3 none 87 8 100
            fen k2/1zr/1h1/1RK Zh 1
            B4A4 none 0 0 0
            fen k2/1zr/1h1/R1K Zh 2
            H*A3 none 87 8 100
            fen k2/1zr/hh1/R1K Z 1
            toryo none 0 0 0
            fen k1H/1rh/1Z1/R1K Z 2
            A1B1 none 0 9 100
            fen 1kH/1rh/1Z1/R1K Z 1
            B3C2 none 6 9 100
            fen 1kH/1rZ/3/R1K ZH 2
            B1C1 none 0 8 100
            fen 2k/1rZ/3/R1K ZHh 1
            C2B1 none 4 8 100
            fen 1Zk/1r1/3/R1K ZHh 2
            C1B1 none 0 8 100
            fen 1k1/1r1/3/R1K ZHzh 1
            A4B4 none 0 7 100
            fen 1k1/1r1/3/1RK ZHzh 2
            B1A1 none 2 7 100
            fen k2/1r1/3/1RK ZHzh 1
            B4A4 none 0 7 100
            C4C3 none 0 7 100
            fen k2/1r1/3/R1K ZHzh 2
            A1B1 none 4 7 100
            fen k2/1r1/2K/1R1 ZHzh 2
            A1A2 none 0 7 100
            fen 3/kr1/2K/1R1 ZHzh 1
            C3C2 none 0 7 100
            fen 3/krK/3/1R1 ZHzh 2
            B2A1 none 4 8 100
            fen r2/k1K/3/1R1 ZHzh 1
            C2C1 none 0 8 100
            fen r1K/k2/3/1R1 ZHzh 2
            A1B2 none 0 7 100
            fen 2K/kr1/3/1R1 ZHzh 1
            C1C2 none 0 7 100
            fen r1K/1k1/3/1R1 ZHzh 1
            B4A3 none 0 7 100
            fen r1K/1k1/R2/3 ZHzh 2
            Z*C2 none 0 8 100
            fen r1K/1kz/R2/3 ZHh 1
            C1C2 none 4 8 100
            fen r2/1kK/R2/3 2ZHh 2
            B2C2 none 0 8 100
            fen r2/2k/R2/3 2ZHkh 1
            A3B3 none 2 7 100
            fen r2/2k/1R1/3 2ZHkh 2
            A1B1 none 0 7 100
            fen 1r1/2k/1R1/3 2ZHkh 1
            B3A3 none 0 7 100
            B1C1 none 0 7 100
            fen 1r1/2k/R2/3 2ZHkh 2
            B1A1 none 0 7 100
            A3B3 none 0 7 100
            fen 1r1/2k/3/R2 2ZHkh 2
            B1C1 none 0 7 100
            fen 2r/2k/3/R2 2ZHkh 1
            A4A3 none 0 7 100
            fen 2r/2k/R2/3 2ZHkh 2
            C1B1 none 0 7 100
            fen 2r/2k/1R1/3 2ZHkh 1
            B3A2 none 0 7 100
            fen 2r/R1k/3/3 2ZHkh 2
            K*B1 none 0 9 100
            fen 1kr/R1k/3/3 2ZHh 1
            Z*A1 none 4 8 100
            fen Zkr/R1k/3/3 ZHh 2
            C2C3 none 4 9 100
            fen Zkr/R2/2k/3 ZHh 1
            H*B2 none 87 8 100
            fen Zkr/RH1/2k/3 Zh 2
            C1C2 none 4 9 100
            fen Zk1/RHr/2k/3 Zh 1
            B2B1 none 87 9 100
            fen ZH1/R1r/2k/3 ZKh 2
            C2C1 none 0 0 0
            fen ZHr/R2/2k/3 ZKh 1
            A1B2 none 87 9 100
            fen 1Hr/RZ1/2k/3 ZKh 2
            C1C2 none 0 0 0
            fen 1H1/RZr/2k/3 ZKh 1
            A2A1 none 87 14 100
            fen RH1/1Zr/2k/3 ZKh 2
            C2B3 none 87 8 100
            fen k1z/1rZ/2K/1R1 Hh 2
            A1B1 none 0 9 100
            A1A2 none 2 9 101
            fen 1kz/1rZ/2K/1R1 Hh 1
            C2B1 none 4 8 100
            fen 1Zz/1r1/2K/1R1 KHh 2
            B2B1 none 0 8 100
            fen 1rz/3/2K/1R1 KHzh 1
            C3B3 none 2 7 100
            fen 1rz/3/1K1/1R1 KHzh 2
            B1A1 none 0 9 100
            fen r1z/3/1K1/1R1 KHzh 1
            B3A3 none 0 7 100
            fen r1z/3/K2/1R1 KHzh 2
            A1B1 none 0 7 100
            fen 1rz/3/K2/1R1 KHzh 1
            A3B3 none 0 7 100
            fen 1kz/1rH/hZ1/1RK - 1
            C2C1 none 6 12 101
            fen 1kH/1r1/hZ1/1RK Z 2
            B1A1 none 0 9 101
            fen k1H/1r1/hZ1/1RK Z 1
            C4C3 none 4 16 101
            fen k1H/1r1/hZK/1R1 Z 2
            A1B1 none 0 0 0
            fen 1kH/1r1/hZK/1R1 Z 1
            C3C2 none 87 9 101
            fen 1kH/1rK/hZ1/1R1 Z 2
            B2A1 none 0 0 0
            fen rkH/2K/hZ1/1R1 Z 1
            B4A3 none 87 11 101
            fen rkH/2K/RZ1/3 ZH 2
            B1C1 none 0 0 0
            fen r1k/2K/RZ1/3 ZHh 1
            C2C1 none 87 8 101
            fen r1K/3/RZ1/3 ZKHh 2
            H*B2 none 0 0 0
            fen r1K/1h1/RZ1/3 ZKH 1
            C1C2 none 87 8 101
            fen r2/1hK/RZ1/3 ZKH 2
            A1B1 none 0 0 0
            fen 1r1/1hK/RZ1/3 ZKH 1
            C2C1 none 87 8 101
            fen 1rK/1h1/RZ1/3 ZKH 2
            B1C1 none 0 0 0
            fen 2r/1h1/RZ1/3 ZKHk 1
            A3A2 none 87 10 101
            fen 2r/Rh1/1Z1/3 ZKHk 2
            K*B1 none 0 14 101
            fen 1kr/Rh1/1Z1/3 ZKH 1
            B3C2 none 87 14 101
            Z*A1 none 87 8 101
            fen 1kr/RhZ/3/3 ZKH 2
            C1C2 none 0 17 101
            fen 1k1/Rhr/3/3 ZKHz 1
            Z*B3 none 87 9 101
            fen 1k1/Rhr/1Z1/3 KHz 2
            B2B3 none 6 11 101
            fen 1k1/R1r/1h1/3 KH2z 1
            K*B2 none 87 8 101
            fen 1k1/RKr/1h1/3 H2z 2
            C2C1 none 8 13 101
            fen 1kr/RK1/1h1/3 H2z 1
            B2B1 none 87 15 101
            fen 1Kr/R2/1h1/3 KH2z 2
            C1C2 none 0 21 101
            fen 1K1/R1r/1h1/3 KH2z 1
            A2A1 none 87 21 101
            fen 2r/Z1z/1Rh/3 2KH 1
            B3A3 none 87 9 101
            fen 2r/Z1z/R1h/3 2KH 2
            C2B1 none 0 0 0
            fen 1zr/Z2/R1h/3 2KH 1
            K*B2 none 87 8 101
            fen 1zr/ZK1/R1h/3 KH 2
            C3C4+ none 0 13 101
            fen 1zr/ZK1/R2/2n KH 1
            A2B1 none 87 10 101
            fen 1Zr/1K1/R2/2n ZKH 2
            C4C3 none 0 0 0
            fen 1Zr/1K1/R1n/3 ZKH 1
            B1A2 none 87 16 101
            fen 2r/ZK1/R1n/3 ZKH 2
            C3C4 none 87 11 101
            fen 2r/ZK1/R2/2n ZKH 1
            A2B1 none 87 9 101
            A2B3 none 87 9 101
            fen Zkr/Rh1/1Z1/3 KH 2
            B2B3 none 2 9 101
            fen Zkr/R2/1h1/3 KHz 1
            A1B2 none 87 8 101
            fen 1kr/RZ1/1h1/3 KHz 2
            C1C2 none 4 10 101
            fen 1k1/RZr/1h1/3 KHz 1
            K*C1 none 87 9 101
            fen 1kK/RZr/1h1/3 Hz 2
            B1C1 none 4 21 101
            fen 2k/RZr/1h1/3 Hzk 1
            A2A1 none 87 21 101
            fen 2r/Zk1/2h/1R1 ZKH 2
            B2A2 none 0 8 101
            fen 2r/k2/2h/1R1 ZKHz 1
            B4B3 none 2 7 101
            fen 2r/k2/1Rh/3 ZKHz 2
            A2B2 none 2 7 101
            fen 2r/1k1/1Rh/3 ZKHz 1
            B3A3 none 2 8 101
            fen 2r/1k1/R1h/3 ZKHz 2
            C1B1 none 2 7 101
            fen 1r1/1k1/R1h/3 ZKHz 1
            A3A4 none 0 7 101
            fen 1r1/1k1/2h/R2 ZKHz 2
            B1A1 none 2 7 101
            fen r2/1k1/2h/R2 ZKHz 1
            A4B4 none 0 7 101
            fen r2/1k1/2h/1R1 ZKHz 2
            B2B1 none 2 7 101
            fen rk1/3/2h/1R1 ZKHz 1
            B4C3 none 2 8 101
            fen rk1/3/2R/3 ZK2Hz 2
            A1A2 none 0 0 0
            fen 1k1/r2/2R/3 ZK2Hz 1
            Z*B3 none 87 7 101
            fen 1k1/r2/1ZR/3 K2Hz 2
            A2A1 none 0 0 0
            fen rk1/3/1ZR/3 K2Hz 1
            K*A2 none 87 8 101
            fen rk1/K2/1ZR/3 2Hz 2
            toryo none 0 0 0
            fen 2r/1k1/RZh/3 ZKH 2
            C1B1 none 0 8 101
            fen 1r1/1k1/RZh/3 ZKH 1
            A3A4 none 4 8 101
            fen 1r1/1k1/1Zh/R2 ZKH 2
            B1A1 none 0 7 101
            fen r2/1k1/1Zh/R2 ZKH 1
            K*C1 none 4 8 101
            fen r1K/1k1/1Zh/R2 ZH 2
            B2B3 none 0 9 101
            fen r1K/3/1kh/R2 ZHz 1
            A4B3 none 4 8 101
            fen r1K/3/1Rh/3 ZKHz 2
            Z*A2 none 0 0 0
            fen r1K/z2/1Rh/3 ZKH 1
            B3C2 none 87 8 101
            fen r1K/z1R/2h/3 ZKH 2
            A2B3 none 0 0 0
            fen r1K/2R/1zh/3 ZKH 1
            C2B3 none 87 10 101
            fen r1K/3/1Rh/3 2ZKH 2
            C3C4 none 0 0 0
            fen r1K/3/1R1/2h 2ZKH 1
            C1B1 none 87 9 101
            fen rK1/3/1R1/2h 2ZKH 2
            A1B1 none 0 0 0
            fen 1r1/3/1R1/2h 2ZKHk 1
            Z*A2 none 87 7 101
            fen 1r1/Z2/1R1/2h ZKHk 2
            B1A1 none 0 0 0
            fen r2/Z2/1R1/2h ZKHk 1
            B3C2 none 87 8 101
            fen r2/Z1R/3/2h ZKHk 2
            K*B1 none 0 9 101
            fen rk1/Z1R/3/2h ZKH 1
            A2B1 none 87 8 101
            fen rZ1/2R/3/2h Z2KH 2
            toryo none 0 0 0
            fen 2z/krZ/2K/1R1 Hh 1
            C2B3 none 0 9 101
            fen 2z/kr1/1ZK/1R1 Hh 2
            B2B1 none 0 8 101
            fen 1rz/k2/1ZK/1R1 Hh 1
            H*C2 none 4 8 101
            fen 1rz/k1H/1ZK/1R1 h 2
            C1B2 none 0 8 101
            fen 1r1/kzH/1ZK/1R1 h 1
            C2C1 none 4 9 101
            fen 1rH/kz1/1ZK/1R1 h 2
            B1A1 none 4 8 101
            fen r1H/kz1/1ZK/1R1 h 1
            B3A2 none 4 9 101
            fen r1H/Zz1/2K/1R1 Kh 2
            A1A2 none 2 8 101
            fen 2H/rz1/2K/1R1 Kzh 1
            C3B3 none 0 8 101
            fen 2H/rz1/1K1/1R1 Kzh 2
            A2A1 none 2 8 101
            fen r1H/1z1/1K1/1R1 Kzh 1
            B3B2 none 4 8 101
            fen r1H/1K1/3/1R1 ZKzh 2
            A1B2 none 0 8 101
            fen 2H/1r1/3/1R1 ZKzkh 1
            Z*C2 none 0 7 101
            fen 2H/1rZ/3/1R1 Kzkh 2
            Z*A3 none 87 8 101
            fen 2H/1rZ/z2/1R1 Kkh 1
            B4A4 none 0 9 101
            fen 2H/1rZ/z2/R2 Kkh 2
            K*B4 none 87 8 101
            fen 2H/1rZ/z2/Rk1 Kh 1
            toryo none 0 0 0
            fen 2r/3/RZh/3 2KHz 2
            C3C4+ none 0 0 0
            fen 2r/3/RZ1/2n 2KHz 1
            A3A2 none 87 8 101
            fen 2r/R2/1Z1/2n 2KHz 2
            Z*B2 none 0 0 0
            fen 2r/Rz1/1Z1/2n 2KH 1
            B3A4 none 87 9 101
            fen 2r/Rz1/3/Z1n 2KH 2
            C4B4 none 0 0 0
            fen 2r/Rz1/3/Zn1 2KH 1
            A4B3 none 87 10 101
            fen 2r/Rz1/1Z1/1n1 2KH 2
            B4B3 none 2 9 101
            fen 2r/Rz1/1n1/3 2KHz 1
            K*B1 none 87 8 101
            fen 1Kr/Rz1/1n1/3 KHz 2
            C1C2 none 2 11 101
            fen 1K1/Rzr/1n1/3 KHz 1
            B1C1 none 87 10 101
            fen 2K/Rzr/1n1/3 KHz 2
            C2C1 none 6 10 101
            fen 2r/Rz1/1n1/3 KHzk 1
            K*B1 none 87 8 101
            fen 1Kr/Rz1/1n1/3 Hzk 2
            C1C2 none 6 14 101
            fen 1K1/Rzr/1n1/3 Hzk 1
            B1B2 none 87 11 101
            fen 3/RKr/1n1/3 ZHzk 2
            B3B2 none 6 21 101
            fen 3/Rnr/3/3 ZHz2k 1
            A2A1 none 87 21 101
            fen rK1/1h1/RZ1/3 ZKH 2
            A1B1 none 0 0 0
            fen 1r1/1h1/RZ1/3 ZKHk 1
            Z*A2 none 87 8 101
            fen 1r1/Zh1/RZ1/3 KHk 2
            B1A1 none 0 0 0
            fen r2/Zh1/RZ1/3 KHk 1
            K*B1 none 87 8 101
            fen rK1/Zh1/RZ1/3 Hk 2
            toryo none 0 0 0
            fen 2r/1K1/RZ1/2n ZKH 2
            C4C3 none 87 10 101
            fen 2r/1K1/RZn/3 ZKH 1
            A3A2 none 87 8 101
            fen 2r/RK1/1Zn/3 ZKH 2
            C3B3 none 0 0 0
            fen 2r/RK1/1n1/3 ZKHz 1
            A2A1 none 87 21 101
            fen k2/r1K/2R/3 ZHzh 1
            C2C1 none 0 5 102
            fen k1K/r2/2R/3 ZHzh 2
            A1B1 none 0 5 102
            fen 1kK/r2/2R/3 ZHzh 1
            Z*B3 none 4 5 102
            fen 1kK/r2/1ZR/3 Hzh 2
            A2A3 none 0 7 102
            fen 1kK/3/rZR/3 Hzh 1
            H*A4 none 87 6 102
            fen 1kK/3/rZR/H2 zh 2
            toryo none -87 14 102
                            ".Split('\n');
            */
        }
        #endregion

    }
}
