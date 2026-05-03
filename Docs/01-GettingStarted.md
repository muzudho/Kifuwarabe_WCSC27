# 🚀 クイックスタートガイド

きふわらけい（Kifuwarakei）を5分で動かすための手順だぜ（＾▽＾）！

## 📋 必要な環境

- **Visual Studio 2026** (または Visual Studio 2022以降)
- **.NET 10.0 SDK**
- **Windows OS** (コマンドプロンプト/PowerShell)
- **(オプション) 将棋所** または **ShogiGUI** (USIエンジンとして使う場合)

## ⚡ 5分で動かす

### 1️⃣ リポジトリをクローン

```bash
git clone https://github.com/muzudho/Kifuwarabe_WCSC27.git
cd Kifuwarabe_WCSC27
```

### 2️⃣ 必要なフォルダーを作成

プロジェクトルートに `Logs` フォルダーを作成してください。

```powershell
mkdir Logs
```

### 3️⃣ Visual Studio でソリューションを開く

Visual Studio で開くファイル: **（ソリューションファイルを見つける必要あり）**

### 4️⃣ ビルド構成を設定

- ビルド構成を **Release** に変更
- プラットフォームを **AnyCPU** に設定

### 5️⃣ ビルド実行

メニューから **ビルド → ソリューションのビルド** を選択

成功すると `Bin/Release/` フォルダーに実行ファイルが出力されます。

### 6️⃣ 実行！

#### パターンA: どうぶつしょうぎモードで遊ぶ

```powershell
cd Bin\Release\net10.0
.\Grayscale.kifuwarakei.Engine.exe
```

黒い画面（コンソール）が開いたら **Enter キー** を押してください。
どうぶつしょうぎモードで遊べるぜ（＾▽＾）！

#### パターンB: USIエンジンとして将棋所に登録

1. 将棋所を起動
2. メニューから **対局 → エンジン管理**
3. **追加** ボタンをクリック
4. `Bin\Release\net10.0\Grayscale.kifuwarakei.Engine.exe` を選択
5. エンジン名を「きふわらけい」などに設定して **OK**

これで将棋所で対局できるようになります！

## 🎮 どうぶつしょうぎモードの基本コマンド

コンソールで以下のコマンドが使えるぜ（＾▽＾）：

| コマンド | 説明 |
|---------|------|
| `man` | コマンド一覧・ヘルプを表示 |
| `ky` | 盤面を表示 (kyokumen) |
| `do <move>` | 指し手を実行 (例: `do 7776FU`) |
| `undo` | 1手戻す |
| `set` | 各種設定 |
| `@` | 外部スクリプトファイルを実行 |
| `sasite` | 指し手情報を表示 |
| `kiki` | 利き情報を表示 |

詳しくは [07-Commands.md](./07-Commands.md) を見てくれだぜ！

## 🐛 うまく動かない場合

[10-Troubleshooting.md](./10-Troubleshooting.md) を確認してください。

## 📚 次に読むドキュメント

- [02-Architecture.md](./02-Architecture.md) - 設計思想を理解する
- [05-Configuration.md](./05-Configuration.md) - 設定ファイルをカスタマイズする
- [06-USI-Protocol.md](./06-USI-Protocol.md) - USIプロトコル対応の詳細

---

**さあ、きふわらけいで遊ぼう！（＾～＾）**
