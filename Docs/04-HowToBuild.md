# 🛠️ ビルド・デプロイ手順

きふわらべい（Kifuwarakei）のビルド方法とデプロイ手順を説明するぜ（＾▽＾）！

## 📋 前提条件

- Visual Studio 2026 (または 2022以降)
- .NET 10.0 SDK
- Git (ソースコードをクローンする場合)

## 🔨 ビルド手順

### 1️⃣ Visual Studio でビルド

#### Step 1: ソリューションを開く

Visual Studio で開く（ソリューションファイルの場所を確認してください）

#### Step 2: ビルド構成を選択

ツールバーから以下を選択：

- **構成**: `Debug` または `Release`
- **プラットフォーム**: `AnyCPU`

#### Step 3: ビルド実行

メニューから以下のいずれかを選択：

- **ビルド → ソリューションのビルド** (Ctrl + Shift + B)
- **ビルド → ソリューションのリビルド** (クリーンビルド)

#### Step 4: 出力の確認

ビルドが成功すると、以下のフォルダーに実行ファイルが出力されます：

```plaintext
📁 Bin/
  +-- 📁 Debug/              # Debugビルドの場合
  |   +-- 📁 net10.0/
  |       +-- Grayscale.kifuwarakei.Engine.exe
  |       +-- Grayscale.Kifuwarakei.Entities.dll
  |       +-- Grayscale.Kifuwarakei.UseCases.dll
  |       +-- appsettings.json
  |       +-- (その他の依存DLL)
  +-- 📁 Release/            # Releaseビルドの場合
	  +-- 📁 net10.0/
		  +-- (同上)
```

### 2️⃣ コマンドラインでビルド

PowerShell または コマンドプロンプトを使う場合：

#### プロジェクトルートに移動

```powershell
cd D:\github.com\muzudho\Kifuwarabe_WCSC27
```

#### ビルド実行

```powershell
# Releaseビルド
dotnet build -c Release

# Debugビルド
dotnet build -c Debug
```

#### クリーンビルド

```powershell
dotnet clean
dotnet build -c Release
```

## 🚀 実行方法

### コンソールから直接実行

```powershell
cd Bin\Release\net10.0
.\Grayscale.kifuwarakei.Engine.exe
```

### Visual Studio から実行 (デバッグ)

1. `Sources/Engine/Engine.csproj` をスタートアッププロジェクトに設定
2. **F5** キー (デバッグ実行) または **Ctrl + F5** (デバッグなし実行)

## 📦 配布用パッケージの作成

### 必要なファイルを集める

配布用には以下のファイルが必要です：

```plaintext
📁 Kifuwarabe_Release/
  +-- Grayscale.kifuwarakei.Engine.exe
  +-- Grayscale.Kifuwarakei.Entities.dll
  +-- Grayscale.Kifuwarakei.UseCases.dll
  +-- appsettings.json
  +-- Nett.dll (及びその他の依存DLL)
  +-- 📁 Settings/
  |   +-- Engine.toml
  +-- 📁 Logs/  (空フォルダー)
```

### PowerShell スクリプトで自動化 (例)

```powershell
# 配布フォルダーを作成
$distDir = ".\Kifuwarabe_Release"
New-Item -ItemType Directory -Force -Path $distDir
New-Item -ItemType Directory -Force -Path "$distDir\Settings"
New-Item -ItemType Directory -Force -Path "$distDir\Logs"

# ファイルをコピー
$sourceDir = ".\Bin\Release\net10.0"
Copy-Item "$sourceDir\*.exe" -Destination $distDir
Copy-Item "$sourceDir\*.dll" -Destination $distDir
Copy-Item "$sourceDir\appsettings.json" -Destination $distDir
Copy-Item ".\Settings\*" -Destination "$distDir\Settings" -Recurse

Write-Host "配布パッケージが作成されました: $distDir"
```

## 🐛 ビルドエラーの対処法

### エラー: "TargetFramework 'net10.0' が見つかりません"

**原因**: .NET 10.0 SDK がインストールされていない

**解決方法**:
```powershell
# SDKバージョンを確認
dotnet --list-sdks

# .NET 10.0 SDK をインストール
# (公式サイトからインストーラーをダウンロード)
```

### エラー: "プロジェクト参照が解決できません"

**原因**: プロジェクト参照のパスが間違っている

**解決方法**:
1. ソリューションエクスプローラーで参照を確認
2. 必要に応じて参照を削除して再追加

### エラー: "Nett パッケージが見つかりません"

**原因**: NuGet パッケージが復元されていない

**解決方法**:
```powershell
dotnet restore
```

または Visual Studio で:
- **ソリューションを右クリック → NuGet パッケージの復元**

## 🧪 ビルド後のテスト

### 自動テストの実行

```powershell
# すべてのテストを実行
dotnet test

# Releaseビルドでテスト
dotnet test -c Release
```

Visual Studio で:
- **テスト → すべてのテストを実行** (Ctrl + R, A)

### 手動テスト

1. 実行ファイルを起動
2. `usi` コマンドを入力
3. 以下のような応答があれば成功：

```
id name Kifuwarabe
id author muzudho
usiok
```

## 🔄 継続的インテグレーション (CI)

GitHub Actions などで自動ビルド・テストを設定する場合：

### サンプル `.github/workflows/build.yml`

```yaml
name: Build

on: [push, pull_request]

jobs:
  build:
	runs-on: windows-latest

	steps:
	- uses: actions/checkout@v3

	- name: Setup .NET
	  uses: actions/setup-dotnet@v3
	  with:
		dotnet-version: '10.0.x'

	- name: Restore dependencies
	  run: dotnet restore

	- name: Build
	  run: dotnet build -c Release --no-restore

	- name: Test
	  run: dotnet test -c Release --no-build --verbosity normal
```

## 📚 次に読むドキュメント

- [05-Configuration.md](./05-Configuration.md) - 設定ファイルのカスタマイズ
- [09-Testing.md](./09-Testing.md) - テストの書き方
- [10-Troubleshooting.md](./10-Troubleshooting.md) - トラブルシューティング

---

**ビルドが成功したら、次は設定をカスタマイズしてみよう！（＾～＾）**
