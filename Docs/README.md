# 📚 きふわらべい ドキュメント

説明書を置くフォルダーだぜ（＾▽＾）！  
Copilot にも生成を頼んでいるぜ（＾▽＾）！  

## 📖 目次

### 🚀 はじめに

1. **[クイックスタートガイド](./01-GettingStarted.md)**
   - 環境構築
   - 5分で動かす手順
   - どうぶつしょうぎモードの起動
   - 将棋所への登録方法

### 🏗️ アーキテクチャ・設計

2. **[アーキテクチャ](./02-Architecture.md)**
   - 設計コンセプト
   - クリーンアーキテクチャ
   - 128bit ビットボード
   - レイヤー構成

3. **[プロジェクト構成](./03-ProjectStructure.md)**
   - フォルダー構成
   - プロジェクト詳細（Engine, Entities, UseCases, Tests）
   - 依存関係
   - ビルド設定

### 🛠️ ビルド・設定

4. **[ビルド・デプロイ手順](./04-HowToBuild.md)**
   - Visual Studio でのビルド
   - コマンドラインビルド
   - 配布パッケージの作成
   - ビルドエラーの対処法

5. **[設定ファイル](./05-Configuration.md)**
   - appsettings.json
   - Engine.toml
   - ログ設定
   - 設定のカスタマイズ例

### 🎮 使い方

6. **[USIプロトコル対応](./06-USI-Protocol.md)**
   - USIプロトコルとは
   - 将棋所への登録方法
   - USIコマンド一覧
   - SFEN形式

7. **[コマンド一覧](./07-Commands.md)**
   - どうぶつしょうぎモードのコマンド
   - 基本コマンド（man, ky, do, undo）
   - 使用例
   - 既知のバグ

### 💻 開発者向け

8. **[開発者向けガイド](./08-Development.md)**
   - 開発環境のセットアップ
   - コーディング規約
   - Pull Request の出し方
   - デバッグ方法

9. **[テストの書き方](./09-Testing.md)**
   - xUnit の使い方
   - テストの書き方（AAA パターン）
   - アサーション
   - テストの実行方法

### 🐛 トラブルシューティング

10. **[トラブルシューティング](./10-Troubleshooting.md)**
    - ビルドエラー
    - 実行時エラー
    - USIモードのエラー
    - テストのエラー

---

## 🎯 クイックリンク

### 初めての方

- [5分で動かす](./01-GettingStarted.md#-5分で動かす)
- [どうぶつしょうぎモードで遊ぶ](./07-Commands.md)
- [将棋所に登録する](./06-USI-Protocol.md#-将棋所への登録方法)

### 開発者の方

- [開発環境のセットアップ](./08-Development.md#-開発環境のセットアップ)
- [ビルド方法](./04-HowToBuild.md)
- [テストの書き方](./09-Testing.md)

### 困ったときは

- [トラブルシューティング](./10-Troubleshooting.md)
- [既知のバグ](./07-Commands.md#-既知の問題)
- [GitHub Issues](https://github.com/muzudho/Kifuwarabe_WCSC27/issues)

---

## フォルダー構成

```plaintext
📁 Kifuwarabe_WCSC27		# きふわらけい（のソリューションフォルダー）
+-- 📁 Bin                 # 実行ファイルの出力先フォルダー
+-- 📁 Docs                # 説明書を置くフォルダー
|       +-- README.md
+-- 📁 Logs                # ログの出力先フォルダー
+-- 📁 Settings			# 設定ファイルを置くフォルダー
+-- 📁 Sources				# ソースコードを置くフォルダー
|   +-- 📁 Engine          # 将棋の思考エンジンのエントリーポイントとなる C# プロジェクト
|   +-- 📁 Entities        # 将棋の駒や盤面などのエンティティを置くフォルダー
|   +-- 📁 UseCases			# 将棋のルールやアルゴリズムなどのユースケースを置くフォルダー
+-- 📁 Tests 				# テストコードを置くフォルダー
```


# 📁 Engine プロジェクト


# 📁 Entities プロジェクト

```plaintext
📁 Kifuwarabe_WCSC27
+-- 📁 Sources
    +-- 📁 Entities      # プロジェクト
        +-- 📁 Configuration
        +-- 📁 Features
            +-- 📁 abstracts
            +-- 📁 facade
            +-- 📁 implements
            +-- 📁 interfaces
            +-- 📁 machine
            +-- 📁 Presenter
            +-- 📄 DebugOptions.cs
        +-- 📁 Game
        +-- 📁 Language
        +-- 📁 Logging
        +-- 📁 Take1Base
        +-- 📄 EntitiesLayer.cs
        +-- 📄 IPlaying.cs
```


# 📁 UseCases プロジェクト


# 📁 Tests プロジェクト

