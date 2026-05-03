# 💻 開発者向けガイド

きふわらけい（Kifuwarakei）の開発に参加する方向けのガイドだぜ（＾▽＾）！

## 🎯 開発方針

### プロジェクトの現状

> このバージョンは開発終了で、次は設計から手を入れ直すぜ☆（＾～＾）

現在のバージョン (WCSC27版) は：
- ✅ 大会参加版をベースにしている
- ✅ 動作はするが、既知のバグがある
- ⚠️ 大規模な機能追加は推奨されない
- ✅ バグ修正やドキュメント改善は歓迎

### 次期バージョンの目標

- Unity 対応（クロスプラットフォーム化）
- クリーンアーキテクチャの徹底
- 既知のバグの修正
- 機械学習・定跡機能の復活

## 🛠️ 開発環境のセットアップ

### 必要なもの

- **Visual Studio 2026** (または 2022以降)
  - ワークロード: `.NET デスクトップ開発`
- **.NET 10.0 SDK**
- **Git**
- **(推奨) ReSharper** または **Rider** (リファクタリングツール)

### リポジトリのクローン

```bash
git clone https://github.com/muzudho/Kifuwarabe_WCSC27.git
cd Kifuwarabe_WCSC27
```

### ブランチ戦略

現在のブランチ:
- `dotnet-version-upgrade-net10.0` - .NET 10.0 へのアップグレード作業中

**推奨ブランチ運用**:
```bash
# 新機能開発
git checkout -b feature/my-new-feature

# バグ修正
git checkout -b fix/issue-123

# ドキュメント改善
git checkout -b docs/improve-readme
```

### 初回ビルド

```powershell
# Logsフォルダーを作成
mkdir Logs

# NuGetパッケージを復元
dotnet restore

# ビルド
dotnet build -c Debug
```

## 📁 コードの構造

### レイヤー間の依存関係

```plaintext
Engine → UseCases → Entities
```

**ルール**:
- 下位レイヤーは上位レイヤーを参照してはいけない
- 各レイヤーは明確な責務を持つ

### 名前空間の規則

```csharp
// Engine
namespace Grayscale.kifuwarakei.Engine;

// Entities
namespace Grayscale.Kifuwarakei.Entities;

// UseCases
namespace Grayscale.Kifuwarakei.UseCases;
```

### ファイル配置の規則

#### Entities プロジェクト

```plaintext
Sources/Entities/Features/
├── abstracts/      # 抽象クラス
├── interfaces/     # インターフェース
├── implements/     # 実装クラス
└── facade/         # ファサードパターン
```

#### UseCases プロジェクト

```plaintext
Sources/UseCases/
├── Playing.cs          # メインロジック
└── PlayingSupport.cs   # サポート機能
```

## 🧪 テストの書き方

### テストプロジェクト

`Tests/Test.csproj` (xUnit)

### テストの実行

```powershell
# すべてのテストを実行
dotnet test

# 特定のテストを実行
dotnet test --filter "FullyQualifiedName~MyTest"
```

### テストの書き方

```csharp
using Xunit;
using Grayscale.Kifuwarakei.Entities;

namespace Tests;

public class KomaTests
{
	[Fact]
	public void Koma_Should_Move_Correctly()
	{
		// Arrange
		var koma = new Koma();

		// Act
		var result = koma.Move(7, 6, 7, 5);

		// Assert
		Assert.True(result);
	}
}
```

詳しくは [09-Testing.md](./09-Testing.md) を参照。

## 🎨 コーディング規約

### C# コーディングスタイル

#### 命名規則

| 種類 | 規則 | 例 |
|------|------|-----|
| クラス | PascalCase | `KyokumenHash` |
| メソッド | PascalCase | `GenerateMoves()` |
| プロパティ | PascalCase | `BoardSize` |
| フィールド (private) | camelCase | `boardWidth` |
| 定数 | UPPER_SNAKE_CASE | `MAX_DEPTH` |
| インターフェース | IPascalCase | `ITaikyokusya` |

#### インデント

- **スペース4つ** (タブは使わない)

#### 波括弧

```csharp
// ✅ Good
if (condition)
{
	DoSomething();
}

// ❌ Bad
if (condition) {
	DoSomething();
}
```

#### コメント

**日本語コメント OK です！**

```csharp
// ☆（＾▽＾）のような顔文字もOK
// このプロジェクトの特徴です

/// <summary>
/// 駒を動かすメソッドだぜ☆（＾～＾）
/// </summary>
public void Move()
{
	// 処理
}
```

### コメントのトーン

このプロジェクトは **フレンドリーなトーン** でコメントを書くのが特徴です：

- `だぜ☆（＾▽＾）`
- `～なんで`
- `してくれだぜ`

このスタイルを維持してください！

## 🐛 既知のバグと対応方針

### バグリスト

| バグ | 優先度 | 対応方針 |
|------|--------|----------|
| 1段目でしか成らない | 高 | 次期バージョンで修正 |
| うさぎの1段目成らず | 中 | 次期バージョンで修正 |
| 世界一周バグ | 高 | 座標計算の見直しが必要 |
| 機械学習が動かない | 低 | 全面的に設計し直す |
| 定跡が3x4盤専用 | 中 | 汎用的な実装に変更 |

### バグ修正の進め方

1. **Issue を作成** (GitHub)
2. **ブランチを作成** (`fix/issue-123`)
3. **テストを書く** (バグを再現するテスト)
4. **修正を実装**
5. **テストが通ることを確認**
6. **Pull Request を作成**

## 📝 Pull Request の出し方

### PR の作成手順

1. **フォークする** (自分のアカウントにコピー)
2. **ブランチを作成**:
   ```bash
   git checkout -b feature/my-feature
   ```
3. **変更をコミット**:
   ```bash
   git add .
   git commit -m "Add: 新機能を追加"
   ```
4. **プッシュ**:
   ```bash
   git push origin feature/my-feature
   ```
5. **GitHub で Pull Request を作成**

### PR のタイトル規則

```
[種類] 簡潔な説明

例:
[Add] 定跡読み込み機能を追加
[Fix] 1段目成りバグを修正
[Docs] READMEを更新
[Refactor] Playing.cs をリファクタリング
```

### PR の説明テンプレート

```markdown
## 変更内容
- 何を変更したか

## 動機・背景
- なぜこの変更が必要か

## テスト方法
- どうやってテストしたか

## スクリーンショット (任意)
- (画像があれば)

## チェックリスト
- [ ] ビルドが通る
- [ ] テストが通る
- [ ] ドキュメントを更新した
```

## 🔍 デバッグ方法

### Visual Studio でのデバッグ

1. **ブレークポイント** を設定
2. **F5** キー (デバッグ開始)
3. **F10** (ステップオーバー) / **F11** (ステップイン)

### ログ出力

`Logs/` フォルダーにログが出力されます。

ログを追加する場合:
```csharp
// TODO: ログ実装の確認
Console.WriteLine($"# Debug: value=[{value}]");
```

### どうぶつしょうぎモードでのデバッグ

コンソールで手動でコマンドを入力してテスト:
```
ky
do 7g7f
ky
sasite
```

## 📚 参考リソース

### 内部ドキュメント

- [02-Architecture.md](./02-Architecture.md) - 設計思想
- [03-ProjectStructure.md](./03-ProjectStructure.md) - プロジェクト構成

### 外部リンク

- [Qiita: きふわらべ解説記事](http://qiita.com/muzudho1/items/f520859a997f16f2948f)
- [USI プロトコル仕様](http://shogidokoro.starfree.jp/usi.html)

## 🤝 コミュニティ

### コントリビューター募集中！

以下の貢献を歓迎します：

- 🐛 バグ修正
- 📝 ドキュメント改善
- ✨ 機能追加 (小規模なもの)
- 🧪 テストの追加
- 🌐 翻訳 (英語ドキュメント)

### 質問・相談

- **GitHub Issues**: バグ報告・機能提案
- **GitHub Discussions**: 雑談・質問
- **Qiita**: 解説記事のコメント欄

## 📚 次に読むドキュメント

- [09-Testing.md](./09-Testing.md) - テストの書き方
- [10-Troubleshooting.md](./10-Troubleshooting.md) - トラブルシューティング

---

**きふわらけいの開発に参加してくれてサンキュー！（＾ｑ＾）**
