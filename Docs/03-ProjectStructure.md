# 📂 プロジェクト構成

きふわらけい（Kifuwarakei）のプロジェクト構造を詳しく説明するぜ（＾▽＾）！

## 🗂️ フォルダー構成

```plaintext
📁 Kifuwarabe_WCSC27		# きふわらけい（のソリューションフォルダー）
+-- 📁 .github              # GitHub Actions など
+-- 📁 Bin                  # 実行ファイルの出力先フォルダー
|   +-- 📁 Debug           # デバッグビルドの出力先
|   +-- 📁 Release         # リリースビルドの出力先
|       +-- 📁 net10.0     # .NET 10.0 ランタイム用
|           +-- Grayscale.kifuwarakei.Engine.exe  # 実行ファイル
|           +-- appsettings.json                   # 設定ファイル
+-- 📁 Docs                 # 説明書を置くフォルダー
|   +-- README.md
|   +-- 01-GettingStarted.md
|   +-- 02-Architecture.md
|   +-- (その他のドキュメント)
+-- 📁 Logs                 # ログの出力先フォルダー
|   +-- (ログファイルが出力される)
+-- 📁 Settings             # 設定ファイルを置くフォルダー
|   +-- Engine.toml        # エンジン設定ファイル (TOML形式)
+-- 📁 Sources              # ソースコードを置くフォルダー
|   +-- 📁 Engine          # 将棋の思考エンジンのエントリーポイントとなる C# プロジェクト
|   |   +-- Program.cs
|   |   +-- Configuration/EngineConf.cs
|   |   +-- Engine.csproj
|   |   +-- appsettings.json
|   +-- 📁 Entities        # 将棋の駒や盤面などのエンティティを置くフォルダー
|   |   +-- Features/
|   |   |   +-- abstracts/    # 抽象クラス
|   |   |   +-- interfaces/   # インターフェース
|   |   |   +-- implements/   # 実装クラス
|   |   |   +-- facade/       # ファサード
|   |   +-- Entities.csproj
|   +-- 📁 UseCases         # 将棋のルールやアルゴリズムなどのユースケースを置くフォルダー
|       +-- Playing.cs
|       +-- PlayingSupport.cs
|       +-- UseCases.csproj
+-- 📁 Tests                # テストコードを置くフォルダー
|   +-- Test.csproj
|   +-- UnitTest1.cs
+-- 📁 packages             # NuGet パッケージのキャッシュ
+-- README.md               # プロジェクトのルート README
```

## 🎯 プロジェクト詳細

### 📦 Sources/Engine

**種類**: 実行可能プロジェクト (Console Application)  
**ターゲットフレームワーク**: .NET 10.0  
**出力**: `Grayscale.kifuwarakei.Engine.exe`

#### 主要ファイル

| ファイル | 説明 |
|---------|------|
| `Program.cs` | エントリーポイント。USIコマンド受付、どうぶつしょうぎモード |
| `Configuration/EngineConf.cs` | 設定ファイル読み込み |
| `appsettings.json` | 設定ファイル（Profile フォルダーのパスを指定） |
| `Engine.csproj` | プロジェクトファイル |

#### 依存関係

- `Sources/Entities` プロジェクトを参照
- `Sources/UseCases` プロジェクトを参照

#### NuGetパッケージ

- `Microsoft.Extensions.Configuration` (9.0.1) - 設定管理
- `Microsoft.Extensions.Configuration.Json` (9.0.1) - JSON設定
- `Nett` (0.15.0) - TOML ファイルの読み込み
- `Microsoft.CodeAnalysis.NetAnalyzers` (10.0.100) - コード分析

### 📦 Sources/Entities

**種類**: クラスライブラリ (Library)  
**ターゲットフレームワーク**: .NET 10.0  
**出力**: `Grayscale.Kifuwarakei.Entities.dll`

#### 主要な機能

##### 📁 Features/abstracts/
- `Med_Koma.cs` - 駒の定義
- `Util_KikaiGakusyu.cs` - 機械学習ユーティリティ (停止中)
- `Util_Sigmoid.cs` - シグモイド関数
- `Util_TantaiTest.cs` - 単体テストユーティリティ
- `Util_Test.cs` - テストユーティリティ

##### 📁 Features/interfaces/
- `Taikyokusya.cs` - 対局者インターフェース
- `FenProtocol.cs` - FEN記法のプロトコル

##### 📁 Features/implements/
- `JosekiImpl.cs` - 定跡の実装
- `KyokumenHashImpl.cs` - 局面ハッシュの実装

##### 📁 Features/facade/
- `Face_YomisujiJoho.cs` - 読み筋情報のファサード

#### NuGetパッケージ

- `Nett` (0.15.0) - TOML ファイルの読み込み

### 📦 Sources/UseCases

**種類**: クラスライブラリ (Library)  
**ターゲットフレームワーク**: .NET 10.0  
**出力**: `Grayscale.Kifuwarakei.UseCases.dll`

#### 主要ファイル

| ファイル | 説明 |
|---------|------|
| `Playing.cs` | 対局の進行制御、コマンド処理 |
| `PlayingSupport.cs` | 対局サポート機能 |

#### 依存関係

- `Sources/Entities` プロジェクトを参照

#### NuGetパッケージ

- `Nett` (0.15.0) - TOML ファイルの読み込み

### 📦 Tests

**種類**: テストプロジェクト (xUnit)  
**ターゲットフレームワーク**: .NET 10.0  

#### 主要ファイル

| ファイル | 説明 |
|---------|------|
| `UnitTest1.cs` | サンプルテストケース |

#### 依存関係

- `Sources/Entities` プロジェクトを参照
- `Sources/UseCases` プロジェクトを参照

#### NuGetパッケージ

- `xunit` (2.9.3) - テストフレームワーク
- `xunit.runner.visualstudio` (3.1.4) - Visual Studio テストランナー
- `Microsoft.NET.Test.Sdk` (17.14.1) - .NET テスト SDK
- `coverlet.collector` (6.0.4) - コードカバレッジ収集

## 📄 設定ファイル

### appsettings.json

場所: `Sources/Engine/appsettings.json`

```json
{
  "Profile": "../../../Settings"
}
```

ビルド後、`Bin/Release/net10.0/appsettings.json` にコピーされます。

**役割**: Settings フォルダーのパスを指定

### Engine.toml

場所: `Settings/Engine.toml` (予定)

**役割**: エンジンの詳細設定（現在は未配置の可能性あり）

## 🔗 プロジェクト間の依存関係

```plaintext
┌─────────────┐
│   Engine    │ ← エントリーポイント
└─────────────┘
	   ↓ 参照
┌─────────────┐
│  UseCases   │ ← ゲームロジック
└─────────────┘
	   ↓ 参照
┌─────────────┐
│  Entities   │ ← ドメインモデル
└─────────────┘

┌─────────────┐
│    Tests    │ ← テスト (Entities, UseCases を参照)
└─────────────┘
```

## 🛠️ ビルド設定

### デバッグビルド (Debug)

- 出力先: `Bin/Debug/net10.0/`
- 定数定義: `DEBUG`, `TRACE`
- 最適化: なし

### リリースビルド (Release)

- 出力先: `Bin/Release/net10.0/`
- 定数定義: `TRACE`
- 最適化: あり

### その他のビルド構成 (Unity用)

- `Unity`
- `Unity DEBUG`
- `Unity Kaihatu`

これらは将来的に Unity で動かすための構成です（現在は未使用）。

## 📚 次に読むドキュメント

- [04-HowToBuild.md](./04-HowToBuild.md) - ビルド手順の詳細
- [05-Configuration.md](./05-Configuration.md) - 設定ファイルのカスタマイズ
- [08-Development.md](./08-Development.md) - 開発に参加する場合

---

**プロジェクト構成を理解したら、ビルドしてみよう！（＾～＾）**
