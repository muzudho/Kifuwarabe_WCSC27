namespace kifuwarabe_wcsc27.interfaces
{
    public interface FenProtocol
    {
        string Fen { get; }

        /// <summary>
        /// 平手初期局面
        /// </summary>
        string Startpos { get; }

        /// <summary>
        /// 対局者１の駒、または打の表記。
        /// </summary>
        string MotigomaT1 { get; }
        string MotigomaT2 { get; }

        /// <summary>
        /// 盤上の駒
        /// </summary>
        string BanjoT1 { get; }
        string BanjoT2 { get; }

        string Suji { get; }
        string Dan { get; }

        /// <summary>
        /// 改造FEN の盤上句。
        /// fen krz/1h1/1H1/ZRK - 1
        /// みたいなやつと、
        /// startpos
        /// みたいなやつがある。
        /// </summary>
        string Position { get; }
        /// <summary>
        /// 改造FEN の持ち駒句。
        /// </summary>
        string MotigomaPos { get; }
        string MotigomaNasi { get; }

        /// <summary>
        /// 手番。数字は改造FEN。b,wはSFEN。
        /// </summary>
        string TebanPos { get; }

        /// <summary>
        /// 投了
        /// </summary>
        string Toryo { get; }
    }
}
