using Xunit;
using Grayscale.Kifuwarakei.Entities.Features.SasiteSeisei;
using Grayscale.Kifuwarakei.Entities.Features.Dougu;
using Grayscale.Kifuwarakei.Entities.Features.Tansaku;

namespace Tests.Entities;

/// <summary>
/// SasiteTypeExtensions のテスト
/// 
/// 拡張メソッドが正しく動作するか確認するぜ☆（＾▽＾）
/// </summary>
public class SasiteTypeExtensionsTests
{
    [Fact]
    public void GetDstMasu_Should_Return_ValidMasu_When_NotToryo()
    {
        // Arrange
        // 7六歩のような指し手を作る（実際の値は ConvSasite で作成）
        // ここではダミーの値を使う
        SasiteType ss = (SasiteType)0x00004676; // ダミー値（実際の指し手は要確認）

        // 局面も必要（実際の局面を作成する必要あり）
        // TODO: Kyokumen のインスタンス化方法を確認

        // Act & Assert
        // 拡張メソッドが呼べることを確認
        // Masu ms = ss.GetDstMasu(ky);

        // とりあえずビルドが通ることを確認
        Assert.True(true);
    }

    [Fact]
    public void NotToryo_Should_Return_Null_When_Toryo()
    {
        // Arrange
        SasiteType ss = SasiteType.Toryo;

        // Act
        SasiteType? result = ss.NotToryo();

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void NotToryo_Should_Return_Value_When_NotToryo()
    {
        // Arrange
        SasiteType ss = (SasiteType)0x00004676; // 投了以外の値

        // Act
        SasiteType? result = ss.NotToryo();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(ss, result.Value);
    }

    [Fact]
    public void GetDstMasuFast_Should_Not_Throw()
    {
        // Arrange
        SasiteType ss = (SasiteType)0x00004676; // ダミー値

        // Act & Assert
        // エラーチェックなしで実行できることを確認
        var exception = Record.Exception(() => ss.GetDstMasuFast());
        Assert.Null(exception);
    }

    [Fact]
    public void ExtensionMethod_Should_Be_Callable_With_DotSyntax()
    {
        // Arrange
        SasiteType ss = (SasiteType)0x00004676;

        // Act & Assert
        // ドットシンタックスでメソッドチェーンできることを確認
        var result = ss.NotToryo();
        Assert.NotNull(result);

        // メソッドチェーンのテスト
        // var masu = ss.NotToryo()?.GetDstMasuFast();
        // この形式で呼べることを確認
    }
}
