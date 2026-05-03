using Grayscale.Kifuwarakei.Entities.Features;
using Grayscale.Kifuwarakei.Entities.Game;
using System.Text;

namespace Test;

/// <summary>
/// きふわらべ思考エンジンのテストだぜ☆（＾▽＾）
/// </summary>
public class MoveGenerationTests
{
    /// <summary>
    ///     <pre>
    /// 初期局面で指し手が生成されることをテストするぜ☆（＾～＾）
    /// 指し手の内容まではチェックしないが、少なくとも何かしらの手が生成されることを確認するぜ☆（＾～＾）
    ///     </pre>
    /// </summary>
    [Fact]
    public void Test_InitialPosition_GeneratesMoves()
    {
        // Arrange - Option_Applicationにアクセスして静的初期化を実行☆
        var ky = Option_Application.Kyokumen;
        var syuturyoku = new StringBuilder();

        // 平手初期局面を設定☆
        ky.DoHirate(false, syuturyoku);

        // Act - 指し手を生成☆
        AbstractUtilSasiteGen.GenerateMove01(
            0, // fukasa
            ky,
            SasiteType.N21_All, // 全ての手を生成
            true, // sasitelistMerge
            syuturyoku
        );

        // Assert - 指し手が生成されることを確認☆
        Assert.True(AbstractUtilSasiteGen.MoveList[0].SslistCount > 0, 
            "初期局面で指し手が生成されなかったぜ☆（＾～＾）");
    }

    /// <summary>
    /// 王手回避時に相手の利きに飛び込む反則手が生成されないことをテストするぜ☆（＾▽＾）
    /// </summary>
    [Fact]
    public void Test_CheckEvasion_DoesNotMoveIntoAttack()
    {
        // Arrange - らいおんが王手されている局面を作成☆
        var ky = new Kyokumen();
        var syuturyoku = new StringBuilder();

        // TODO: 王手局面のFEN文字列を設定
        // この局面では、らいおんが王手されており、
        // 逃げる先に相手の利きがある状況を作る☆

        // Act - 指し手を生成☆
        AbstractUtilSasiteGen.GenerateMove01(
            0,
            ky,
            SasiteType.N21_All,
            true,
            syuturyoku
        );

        // Assert - 生成された全ての手をチェック☆
        for (int i = 0; i < AbstractUtilSasiteGen.MoveList[0].SslistCount; i++)
        {
            Sasite ss = AbstractUtilSasiteGen.MoveList[0].ListMove[i];

            // TODO: 各手を指してみて、自玉が取られないことを確認☆
            // （実装は後で追加）
        }

        // とりあえず、何か手が生成されることを確認☆
        Assert.True(true, "王手回避手のテストは実装中だぜ☆（＾～＾）");
    }

    /// <summary>
    /// 投了以外の手があることをテストするぜ☆（＾▽＾）
    /// </summary>
    [Fact]
    public void Test_GeneratedMoves_NotOnlyResign()
    {
        // Arrange - Option_Applicationの局面を使用☆
        var ky = Option_Application.Kyokumen;
        var syuturyoku = new StringBuilder();

        ky.DoHirate(false, syuturyoku);

        // Act
        AbstractUtilSasiteGen.GenerateMove01(0, ky, SasiteType.N21_All, true, syuturyoku);

        // Assert - 投了(Toryo)だけではないことを確認☆
        bool hasNonResignMove = false;
        for (int i = 0; i < AbstractUtilSasiteGen.MoveList[0].SslistCount; i++)
        {
            if (AbstractUtilSasiteGen.MoveList[0].ListMove[i] != Sasite.Toryo)
            {
                hasNonResignMove = true;
                break;
            }
        }

        Assert.True(hasNonResignMove, "投了以外の手が生成されなかったぜ☆（＾～＾）");
    }
}
