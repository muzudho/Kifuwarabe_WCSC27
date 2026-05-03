# 🐛 トラブルシューティング

きふわらべい（Kifuwarakei）でよくある問題と解決方法を説明するぜ（＾▽＾）！

## 🚨 ビルドエラー

### エラー: "TargetFramework 'net10.0' が見つかりません"

**症状**:
```
error MSB3644: 指定されたフレームワーク 'net10.0' が見つかりません
```

**原因**: .NET 10.0 SDK がインストールされていない

**解決方法**:

1. .NET 10.0 SDK をインストール
   - [.NET 公式サイト](https://dotnet.microsoft.com/)からダウンロード

2. インストール後、確認:
   ```powershell
   dotnet --list-sdks
   ```

3. Visual Studio を再起動

---

### エラー: "プロジェクト参照が解決できません"

**症状**:
```
error CS0246: 型または名前空間の名前 'Entities' が見つかりませんでした
```

**原因**: プロジェクト参照のパスが間違っている

**解決方法**:

1. Visual Studio で:
   - **ソリューションエクスプローラー** を開く
   - **参照** を右クリック → **参照の追加**
   - 不足しているプロジェクトを追加

2. コマンドラインで:
   ```powershell
   dotnet restore
   dotnet build
   ```

---

### エラー: "NuGet パッケージが復元できません"

**症状**:
```
error NU1101: パッケージ 'Nett' が見つかりません
```

**原因**: NuGet パッケージソースに接続できない

**解決方法**:

1. NuGet パッケージを手動で復元:
   ```powershell
   dotnet restore
   ```

2. NuGet キャッシュをクリア:
   ```powershell
   dotnet nuget locals all --clear
   dotnet restore
   ```

3. Visual Studio で:
   - **ツール → NuGet パッケージマネージャー → パッケージマネージャーの設定**
   - パッケージソースを確認

---

## 🏃 実行時エラー

### エラー: "Logs フォルダーが見つかりません"

**症状**:
```
DirectoryNotFoundException: Could not find a part of the path 'D:\...\Logs'
```

**原因**: `Logs` フォルダーが存在しない

**解決方法**:

プロジェクトルートに `Logs` フォルダーを作成:

```powershell
cd D:\github.com\muzudho\Kifuwarabe_WCSC27
mkdir Logs
```

---

### エラー: "appsettings.json が見つかりません"

**症状**:
```
FileNotFoundException: Could not find file 'appsettings.json'
```

**原因**: appsettings.json が出力フォルダーにコピーされていない

**解決方法**:

1. `Engine.csproj` を確認:
   ```xml
   <ItemGroup>
	 <None Update="appsettings.json">
	   <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
	 </None>
   </ItemGroup>
   ```

2. リビルド:
   ```powershell
   dotnet clean
   dotnet build
   ```

---

### エラー: "Settings/Engine.toml が見つかりません"

**症状**:
```
FileNotFoundException: Could not find file 'Settings/Engine.toml'
```

**原因**: Engine.toml ファイルが存在しない、またはパスが間違っている

**解決方法**:

1. `Settings` フォルダーを作成:
   ```powershell
   mkdir Settings
   ```

2. `Engine.toml` を作成:
   ```powershell
   New-Item -Path Settings\Engine.toml -ItemType File
   ```

3. appsettings.json のパスを確認:
   ```json
   {
	 "Profile": "../../../Settings"
   }
   ```

---

## 🎮 USI モードのエラー

### 問題: 将棋所でエンジンが起動しない

**症状**: 将棋所で「エンジンが応答しません」と表示される

**原因**:
- 実行ファイルのパスが間違っている
- 依存DLLが不足している

**解決方法**:

1. エンジンパスを確認:
   ```
   Bin\Release\net10.0\Grayscale.kifuwarakei.Engine.exe
   ```

2. コマンドラインで直接起動してテスト:
   ```powershell
   cd Bin\Release\net10.0
   .\Grayscale.kifuwarakei.Engine.exe
   ```

3. `usi` コマンドを入力して応答を確認:
   ```
   usi
   ```

   **期待される応答**:
   ```
   id name Kifuwarabe
   id author muzudho
   usiok
   ```

4. 依存DLLを確認:
   - `Grayscale.Kifuwarakei.Entities.dll`
   - `Grayscale.Kifuwarakei.UseCases.dll`
   - `Nett.dll`

---

### 問題: USIコマンドに応答しない

**症状**: `usi` コマンドを入力しても何も表示されない

**原因**: どうぶつしょうぎモードになっている可能性

**解決方法**:

- Enterキーを押さずに `usi` コマンドを入力
- USIモードに切り替わるまで待つ

---

## 🕹️ どうぶつしょうぎモードのエラー

### 問題: コマンドが反応しない

**症状**: コマンドを入力しても何も起こらない

**原因**: コマンドのスペルミス、または未実装

**解決方法**:

1. `man` コマンドでヘルプを表示:
   ```
   man
   ```

2. コマンドのスペルを確認:
   - `ky` (kyokumen)
   - `do` (指し手)
   - `undo` (戻す)

---

### 問題: 駒が動かない (バグ)

**症状**: `do` コマンドで駒を動かせない

**既知のバグ**:
- ❌ **1段目でしか成らない**
- ❌ **うさぎの1段目成らず**
- ❌ **世界一周バグ** (盤の端を超えて反対側に出現)

**解決方法**:

これらは既知のバグです。次期バージョンで修正予定です。

**回避方法**:
- 問題のある指し手を避ける
- `undo` コマンドで戻す

---

## 🧪 テストのエラー

### エラー: "テストが見つかりません"

**症状**: Visual Studio のテストエクスプローラーにテストが表示されない

**原因**: テストプロジェクトがビルドされていない

**解決方法**:

1. ソリューションをリビルド:
   ```powershell
   dotnet build
   ```

2. Visual Studio で:
   - **テスト → テストエクスプローラー**
   - ツールバーの **更新** ボタンをクリック

---

### エラー: "テストが失敗する"

**症状**: テストが予期せず失敗する

**原因**: テストの前提条件が満たされていない

**解決方法**:

1. テストを1つずつ実行して、どれが失敗しているか確認

2. テストコードを確認:
   ```csharp
   [Fact]
   public void MyTest()
   {
	   // Arrange, Act, Assert を確認
   }
   ```

3. エラーメッセージを読む:
   ```
   Expected: 5
   Actual:   3
   ```

---

## 🔧 設定ファイルのエラー

### エラー: "JSON の解析エラー"

**症状**:
```
JsonException: Invalid JSON
```

**原因**: appsettings.json の記述ミス

**解決方法**:

1. JSON の構文を確認:
   ```json
   {
	 "Profile": "../../../Settings"
   }
   ```

2. 最後のカンマを削除:
   ```json
   // ❌ Bad
   {
	 "Profile": "../../../Settings",
   }

   // ✅ Good
   {
	 "Profile": "../../../Settings"
   }
   ```

3. オンライン JSON バリデーターで検証:
   - [JSONLint](https://jsonlint.com/)

---

### エラー: "TOML の解析エラー"

**症状**:
```
TomlException: Invalid TOML
```

**原因**: Engine.toml の記述ミス

**解決方法**:

1. TOML の構文を確認:
   ```toml
   [Engine]
   name = "Kifuwarabe"
   ```

2. 文字列はダブルクォートで囲む:
   ```toml
   # ❌ Bad
   name = Kifuwarabe

   # ✅ Good
   name = "Kifuwarabe"
   ```

---

## 🖥️ Visual Studio のエラー

### 問題: IntelliSense が動かない

**症状**: コード補完が効かない

**解決方法**:

1. ソリューションをクリーンビルド:
   ```powershell
   dotnet clean
   dotnet build
   ```

2. Visual Studio を再起動

3. キャッシュを削除:
   - `.vs` フォルダーを削除
   - `obj` フォルダーを削除
   - `bin` フォルダーを削除

---

### 問題: デバッグが開始できない

**症状**: F5 を押してもデバッグが開始しない

**解決方法**:

1. スタートアッププロジェクトを設定:
   - ソリューションエクスプローラーで `Engine` プロジェクトを右クリック
   - **スタートアッププロジェクトに設定**

2. ビルド構成を確認:
   - ツールバーで **Debug** または **Release** を確認

---

## 📚 その他の問題

### 問題: ログが出力されない

**症状**: `Logs/` フォルダーにログファイルが作成されない

**原因**: ログ機能が実装されていない、または設定ミス

**解決方法**:

1. `Logs` フォルダーが存在するか確認

2. Engine.toml のログ設定を確認

3. ソースコードを確認:
   - ログ出力の実装があるか
   - ログレベルが適切か

---

### 問題: パフォーマンスが遅い

**症状**: 思考が遅い、動作が重い

**原因**:
- Debugビルドで実行している
- 探索深さが深すぎる

**解決方法**:

1. Releaseビルドで実行:
   ```powershell
   dotnet build -c Release
   cd Bin\Release\net10.0
   .\Grayscale.kifuwarakei.Engine.exe
   ```

2. 探索深さを調整 (Engine.toml):
   ```toml
   [Search]
   max_depth = 5  # 小さくする
   ```

---

## 🆘 それでも解決しない場合

### GitHub Issues で質問

1. [GitHub Issues](https://github.com/muzudho/Kifuwarabe_WCSC27/issues) を開く
2. **New Issue** をクリック
3. 以下の情報を含めて投稿:
   - エラーメッセージ（全文）
   - 実行環境 (OS, .NET バージョン, Visual Studio バージョン)
   - 再現手順
   - スクリーンショット（可能であれば）

### Qiita の記事を確認

- [Qiita: きふわらべ解説記事](http://qiita.com/muzudho1/items/f520859a997f16f2948f)

---

## 📚 関連ドキュメント

- [01-GettingStarted.md](./01-GettingStarted.md) - 初期セットアップ
- [04-HowToBuild.md](./04-HowToBuild.md) - ビルド手順
- [05-Configuration.md](./05-Configuration.md) - 設定ファイル
- [08-Development.md](./08-Development.md) - 開発ガイド

---

**問題が解決したら、次のステップへ進もう！（＾▽＾）**
