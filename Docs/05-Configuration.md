# ⚙️ 設定ファイル

きふわらべい（Kifuwarakei）の設定ファイルについて説明するぜ（＾▽＾）！

## 📄 設定ファイル一覧

きふわらべいには2つの設定ファイルがあります：

| ファイル | 場所 | 形式 | 用途 |
|---------|------|------|------|
| **appsettings.json** | `Sources/Engine/appsettings.json` | JSON | Settings フォルダーのパスを指定 |
| **Engine.toml** | `Settings/Engine.toml` | TOML | エンジンの詳細設定 |

## 📝 appsettings.json

### ファイルの場所

- **開発時**: `Sources/Engine/appsettings.json`
- **ビルド後**: `Bin/Release/net10.0/appsettings.json`

### 内容

```json
{
  "Profile": "../../../Settings"
}
```

### 設定項目

#### `Profile`

**説明**: Settings フォルダーへの相対パス

**デフォルト値**: `"../../../Settings"`

**変更例**:

```json
{
  "Profile": "C:/MySettings"  // 絶対パスも指定可能
}
```

**注意事項**:
- パスの区切りは `/` (スラッシュ) または `\\` (バックスラッシュ2つ)
- 相対パスは実行ファイルからの相対位置

### パスの構造

実行ファイルから Settings フォルダーへのパス：

```plaintext
Bin/Release/net10.0/Grayscale.kifuwarakei.Engine.exe
					↓ ../
Bin/Release/
			↓ ../
Bin/
	↓ ../
(プロジェクトルート)
	↓ Settings/
Settings/Engine.toml
```

## 📝 Engine.toml

### ファイルの場所

`Settings/Engine.toml`

### TOML形式について

TOML (Tom's Obvious, Minimal Language) は設定ファイル用の記述言語です。

**特徴**:
- INIファイルに似た構文
- 人間が読みやすい
- JSONより記述が簡潔

### 設定例 (サンプル)

```toml
[Engine]
name = "Kifuwarabe"
author = "muzudho"
version = "1.0.0"

[Board]
# 盤面サイズ (3x4 ～ 9x9)
width = 9
height = 9

[Search]
# 探索深さ
max_depth = 10

# 思考時間 (ミリ秒)
think_time_ms = 5000

[Log]
# ログレベル (Debug, Info, Warning, Error)
level = "Info"

# ログ出力先
output_dir = "../Logs"

[Joseki]
# 定跡を使用するか
enabled = false

# 定跡ファイルのパス
file_path = "./joseki.dat"
```

### 主要な設定項目 (予想)

#### [Engine]

| 項目 | 型 | 説明 |
|------|-----|------|
| `name` | string | エンジン名 |
| `author` | string | 作者名 |
| `version` | string | バージョン |

#### [Board]

| 項目 | 型 | 説明 |
|------|-----|------|
| `width` | int | 盤面の幅 (3～9) |
| `height` | int | 盤面の高さ (4～9) |

#### [Search]

| 項目 | 型 | 説明 |
|------|-----|------|
| `max_depth` | int | 探索の最大深さ |
| `think_time_ms` | int | 思考時間 (ミリ秒) |

#### [Log]

| 項目 | 型 | 説明 |
|------|-----|------|
| `level` | string | ログレベル |
| `output_dir` | string | ログ出力先 |

#### [Joseki]

| 項目 | 型 | 説明 |
|------|-----|------|
| `enabled` | bool | 定跡を使用するか |
| `file_path` | string | 定跡ファイルのパス |

**注意**: 
- 現在、定跡機能は 3x4 盤専用で、他のサイズでは動作しません
- Engine.toml の実際の設定項目はソースコードを確認してください

## 📁 Logs フォルダー

### 場所

`Logs/` (プロジェクトルート直下)

### 作成方法

**初回実行前に手動で作成する必要があります**:

```powershell
mkdir Logs
```

### ログファイル

実行時に以下のようなログファイルが出力されます (予想):

```plaintext
📁 Logs/
  +-- engine_2025-01-15.log
  +-- error_2025-01-15.log
  +-- debug_2025-01-15.log
```

## 🔧 設定の読み込み

### コード内での読み込み

`Sources/Engine/Configuration/EngineConf.cs` で設定を読み込んでいます。

#### appsettings.json の読み込み

```csharp
using Microsoft.Extensions.Configuration;

var configuration = new ConfigurationBuilder()
	.SetBasePath(Directory.GetCurrentDirectory())
	.AddJsonFile("appsettings.json", optional: false)
	.Build();

string profilePath = configuration["Profile"];
```

#### Engine.toml の読み込み

```csharp
using Nett;

var config = Toml.ReadFile<EngineConfig>(
	Path.Combine(profilePath, "Engine.toml")
);
```

## 🛠️ 設定のカスタマイズ例

### 例1: ログ出力先を変更

**appsettings.json**:
```json
{
  "Profile": "D:/MyKifuwarabeSettings"
}
```

**D:/MyKifuwarabeSettings/Engine.toml**:
```toml
[Log]
level = "Debug"
output_dir = "D:/KifuwarabeLogs"
```

### 例2: 盤面サイズを変更 (5x5将棋)

**Engine.toml**:
```toml
[Board]
width = 5
height = 5
```

### 例3: 思考時間を短く

**Engine.toml**:
```toml
[Search]
think_time_ms = 1000  # 1秒
```

## 🚨 注意事項

### ⚠️ パスの指定

- Windows の場合、バックスラッシュはエスケープが必要: `"C:\\MyPath"` または `/` を使用
- 相対パスは実行ファイルからの相対位置

### ⚠️ Logs フォルダーは必須

Logs フォルダーが存在しないとエラーになる可能性があります。
実行前に必ず作成してください。

### ⚠️ Engine.toml の配置

appsettings.json で指定したパスに Engine.toml が存在しない場合、エラーになります。

## 📚 次に読むドキュメント

- [06-USI-Protocol.md](./06-USI-Protocol.md) - USIプロトコル対応
- [07-Commands.md](./07-Commands.md) - コマンド一覧
- [10-Troubleshooting.md](./10-Troubleshooting.md) - 設定エラーの解決

---

**設定をカスタマイズして、自分好みのエンジンにしよう！（＾～＾）**
