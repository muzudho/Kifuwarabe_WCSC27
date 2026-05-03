# 🧪 テストの書き方

きふわらけい（Kifuwarakei）のテストについて説明するぜ（＾▽＾）！

## 🎯 テストの方針

### テストの重要性

- ✅ **バグを早期に発見**
- ✅ **リファクタリングを安全に**
- ✅ **仕様をドキュメント化**

### テストの範囲

現在のプロジェクトでは、以下を優先的にテストします：

1. **ビットボード演算** (Entities)
2. **駒の移動・成り** (Entities)
3. **合法手生成** (UseCases)
4. **局面ハッシュ** (Entities)

## 🛠️ テストフレームワーク

### xUnit

きふわらけいでは **xUnit** を使用しています。

**特徴**:
- .NET で最も人気のあるテストフレームワーク
- Visual Studio と統合
- `[Fact]` と `[Theory]` 属性でテストを定義

### NuGet パッケージ

```xml
<PackageReference Include="xunit" Version="2.9.3" />
<PackageReference Include="xunit.runner.visualstudio" Version="3.1.4" />
<PackageReference Include="Microsoft.NET.Test.Sdk" Version="17.14.1" />
<PackageReference Include="coverlet.collector" Version="6.0.4" />
```

## 📁 テストプロジェクトの構造

```plaintext
Tests/
├── Test.csproj
├── UnitTest1.cs              # サンプルテスト
├── Entities/                 # Entities のテスト
│   ├── KomaTests.cs
│   ├── BitboardTests.cs
│   └── KyokumenHashTests.cs
├── UseCases/                 # UseCases のテスト
│   ├── PlayingTests.cs
│   └── MoveGenerationTests.cs
└── TestHelpers/              # テスト用ヘルパー
	└── BoardBuilder.cs
```

## ✍️ テストの書き方

### 基本的なテスト

```csharp
using Xunit;
using Grayscale.Kifuwarakei.Entities;

namespace Tests.Entities;

public class KomaTests
{
	[Fact]
	public void Koma_Should_Be_Created()
	{
		// Arrange (準備)
		// Act (実行)
		var koma = new Koma();

		// Assert (検証)
		Assert.NotNull(koma);
	}
}
```

### Arrange-Act-Assert パターン

テストは **AAA パターン** で書きます：

```csharp
[Fact]
public void Koma_Should_Move_To_Valid_Position()
{
	// Arrange: テストの準備
	var board = new Board();
	var koma = board.GetKoma(7, 7);

	// Act: テスト対象のメソッドを実行
	var result = koma.MoveTo(7, 6);

	// Assert: 結果を検証
	Assert.True(result);
	Assert.Equal(7, koma.X);
	Assert.Equal(6, koma.Y);
}
```

### パラメータ化テスト

`[Theory]` を使って、複数のパターンをテストします：

```csharp
[Theory]
[InlineData(7, 7, 7, 6, true)]   // 正常な移動
[InlineData(7, 7, 7, 5, false)]  // 2マス移動（不正）
[InlineData(7, 7, 10, 7, false)] // 盤外（不正）
public void Koma_Move_Should_Validate_Position(
	int fromX, int fromY, int toX, int toY, bool expected)
{
	// Arrange
	var board = new Board();
	var koma = board.GetKoma(fromX, fromY);

	// Act
	var result = koma.MoveTo(toX, toY);

	// Assert
	Assert.Equal(expected, result);
}
```

## 🎯 アサーション (Assert)

### よく使うアサーション

| メソッド | 説明 | 例 |
|---------|------|-----|
| `Assert.True(condition)` | 条件が真 | `Assert.True(result)` |
| `Assert.False(condition)` | 条件が偽 | `Assert.False(isError)` |
| `Assert.Equal(expected, actual)` | 等しい | `Assert.Equal(5, count)` |
| `Assert.NotEqual(expected, actual)` | 等しくない | `Assert.NotEqual(0, id)` |
| `Assert.Null(obj)` | null である | `Assert.Null(result)` |
| `Assert.NotNull(obj)` | null でない | `Assert.NotNull(koma)` |
| `Assert.Contains(expected, collection)` | コレクションに含まれる | `Assert.Contains(move, moves)` |
| `Assert.Throws<TException>(() => ...)` | 例外が発生 | `Assert.Throws<InvalidOperationException>(() => koma.Move())` |

## 🧩 テストケースの例

### 例1: ビットボード演算のテスト

```csharp
using Xunit;

namespace Tests.Entities;

public class BitboardTests
{
	[Fact]
	public void Bitboard_Should_Set_Bit_At_Position()
	{
		// Arrange
		var bitboard = new Bitboard();

		// Act
		bitboard.SetBit(7, 7);

		// Assert
		Assert.True(bitboard.GetBit(7, 7));
	}

	[Fact]
	public void Bitboard_Should_Clear_Bit_At_Position()
	{
		// Arrange
		var bitboard = new Bitboard();
		bitboard.SetBit(7, 7);

		// Act
		bitboard.ClearBit(7, 7);

		// Assert
		Assert.False(bitboard.GetBit(7, 7));
	}
}
```

### 例2: 駒の移動テスト

```csharp
using Xunit;
using Grayscale.Kifuwarakei.Entities;

namespace Tests.Entities;

public class MoveTests
{
	[Theory]
	[InlineData("7g7f", true)]   // 歩の1マス前進
	[InlineData("7g7e", false)]  // 歩の2マス前進（不正）
	[InlineData("8h2b", true)]   // 角の斜め移動
	public void Move_Should_Validate_Correctly(string move, bool expected)
	{
		// Arrange
		var board = new Board();
		board.SetStartPosition();

		// Act
		var result = board.TryMove(move);

		// Assert
		Assert.Equal(expected, result);
	}
}
```

### 例3: 局面ハッシュのテスト

```csharp
using Xunit;
using Grayscale.Kifuwarakei.Entities;

namespace Tests.Entities;

public class KyokumenHashTests
{
	[Fact]
	public void KyokumenHash_Should_Be_Unique_For_Different_Positions()
	{
		// Arrange
		var board1 = new Board();
		board1.SetStartPosition();

		var board2 = new Board();
		board2.SetStartPosition();
		board2.TryMove("7g7f");

		// Act
		var hash1 = board1.GetHash();
		var hash2 = board2.GetHash();

		// Assert
		Assert.NotEqual(hash1, hash2);
	}

	[Fact]
	public void KyokumenHash_Should_Be_Same_For_Same_Position()
	{
		// Arrange
		var board1 = new Board();
		board1.SetStartPosition();

		var board2 = new Board();
		board2.SetStartPosition();

		// Act
		var hash1 = board1.GetHash();
		var hash2 = board2.GetHash();

		// Assert
		Assert.Equal(hash1, hash2);
	}
}
```

## 🏃 テストの実行

### Visual Studio で実行

1. **テストエクスプローラー** を開く:
   - メニュー → **テスト → テストエクスプローラー**
2. **すべてのテストを実行**: ツールバーの **▶** ボタン
3. **特定のテストを実行**: テストを右クリック → **テストの実行**

### コマンドラインで実行

```powershell
# すべてのテストを実行
dotnet test

# Verboseモード（詳細表示）
dotnet test --verbosity normal

# 特定のテストのみ実行
dotnet test --filter "FullyQualifiedName~BitboardTests"

# Releaseビルドでテスト
dotnet test -c Release
```

### テスト結果の確認

**成功した場合**:
```
Passed!  - Failed:     0, Passed:    10, Skipped:     0, Total:    10
```

**失敗した場合**:
```
Failed!  - Failed:     1, Passed:     9, Skipped:     0, Total:    10
```

## 📊 コードカバレッジ

### coverlet の使用

```powershell
# カバレッジ付きテスト実行
dotnet test /p:CollectCoverage=true

# HTML レポート生成 (ReportGenerator が必要)
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura
reportgenerator -reports:coverage.cobertura.xml -targetdir:coveragereport
```

### カバレッジ目標

- **Entities**: 80% 以上
- **UseCases**: 70% 以上
- **Engine**: 50% 以上 (UIなので低め)

## 🐛 バグを再現するテストを書く

バグを修正する前に、**バグを再現するテストを書く** のが重要です。

### 例: 1段目成りバグのテスト

```csharp
[Fact]
public void Bug_Nari_Should_Work_On_First_Rank()
{
	// Arrange
	var board = new Board();
	// (駒を1段目の直前に配置する処理)

	// Act
	var result = board.TryMove("7b7a+"); // 成る

	// Assert
	Assert.True(result, "1段目で成れるべき");
	// (駒が成っていることを確認)
}
```

このテストが失敗することを確認してから、修正を実装します。
修正後、テストが通ることを確認します。

## 📚 テストのベストプラクティス

### ✅ すべきこと

- **1テスト1検証**: 1つのテストで1つのことだけをテストする
- **テスト名は明確に**: `Should_` や `When_` を使って意図を明確にする
- **AAA パターン**: Arrange, Act, Assert を明確に分ける
- **独立性**: テストは他のテストに依存しない
- **高速**: テストは高速に実行できるようにする

### ❌ やってはいけないこと

- **複雑すぎるテスト**: テストが読みにくくなる
- **テストの順序に依存**: xUnit はテストの順序を保証しない
- **外部リソースに依存**: ファイル、ネットワーク、DBは避ける
- **テストのためのテスト**: 価値のないテストは書かない

## 📚 次に読むドキュメント

- [08-Development.md](./08-Development.md) - 開発全般のガイド
- [10-Troubleshooting.md](./10-Troubleshooting.md) - テストエラーの対処

---

**テストを書いて、安心してリファクタリングしよう！（＾▽＾）**
