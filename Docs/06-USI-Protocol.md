# 🎮 USIプロトコル対応

きふわらべい（Kifuwarakei）の USI (Universal Shogi Interface) プロトコル対応について説明するぜ（＾▽＾）！

## 📖 USIプロトコルとは？

**USI (Universal Shogi Interface)** は、将棋エンジンと GUI（将棋所、ShogiGUI など）が通信するための標準プロトコルです。

### 特徴

- **テキストベース**: コマンドと応答を文字列でやり取り
- **標準入出力**: stdin/stdout で通信
- **チェスの UCI プロトコル** をベースに日本将棋用に拡張

### 公式仕様

- [USI プロトコル仕様 (日本語)](http://shogidokoro.starfree.jp/usi.html)

## 🔌 将棋所への登録方法

### Step 1: 将棋所のダウンロード

将棋所を公式サイトからダウンロード・インストールしてください。

- [将棋所公式サイト](http://shogidokoro.starfree.jp/)

### Step 2: エンジンの登録

1. 将棋所を起動
2. メニューから **対局 → エンジン管理** を選択
3. **追加** ボタンをクリック
4. 以下のファイルを選択:
   ```
   Kifuwarabe_WCSC27/Bin/Release/net10.0/Grayscale.kifuwarakei.Engine.exe
   ```
5. エンジン名を設定 (例: `きふわらべい`)
6. **OK** をクリック

### Step 3: 対局開始

1. メニューから **対局 → 対局開始** を選択
2. 先手または後手に「きふわらべい」を選択
3. **対局開始** ボタンをクリック

## 📡 USIコマンド一覧

きふわらべいが対応している USI コマンドを紹介します。

### 基本コマンド

#### `usi`

**説明**: エンジンの初期化・情報取得

**応答例**:
```
id name Kifuwarabe
id author muzudho
usiok
```

---

#### `isready`

**説明**: エンジンが準備完了か確認

**応答**:
```
readyok
```

---

#### `usinewgame`

**説明**: 新しい対局の開始を通知

**応答**: なし（内部で局面をリセット）

---

#### `position [startpos | sfen <sfenstring>] [moves <move1> <move2> ...]`

**説明**: 局面を設定

**例1**: 平手初期局面
```
position startpos
```

**例2**: 初期局面から指し手を進める
```
position startpos moves 7g7f 3c3d
```

**例3**: SFEN形式で局面を指定
```
position sfen lnsgkgsnl/1r5b1/ppppppppp/9/9/9/PPPPPPPPP/1B5R1/LNSGKGSNL b - 1
```

---

#### `go [btime <ms>] [wtime <ms>] [byoyomi <ms>] [binc <ms>] [winc <ms>]`

**説明**: 思考開始

**パラメータ**:
- `btime`: 先手の残り時間 (ミリ秒)
- `wtime`: 後手の残り時間 (ミリ秒)
- `byoyomi`: 秒読み時間 (ミリ秒)
- `binc`: 先手の加算時間 (ミリ秒)
- `winc`: 後手の加算時間 (ミリ秒)

**例**:
```
go btime 600000 wtime 600000 byoyomi 10000
```

**応答**:
```
bestmove 7g7f
```

---

#### `quit`

**説明**: エンジンを終了

**応答**: なし（プログラム終了）

---

### 拡張コマンド (オプション)

#### `stop`

**説明**: 思考を中断

**応答**: 現時点での最善手を返す
```
bestmove 7g7f
```

---

#### `setoption name <name> value <value>`

**説明**: オプション設定

**例**:
```
setoption name USI_Ponder value true
setoption name USI_Hash value 256
```

**きふわらべいの対応状況**: 
一部のオプションは未対応の可能性があります。

---

#### `gameover [win | lose | draw]`

**説明**: 対局終了を通知

**例**:
```
gameover win
```

---

## 🧪 USIモードのテスト

### コマンドラインでテスト

1. 実行ファイルを起動:
   ```powershell
   cd Bin\Release\net10.0
   .\Grayscale.kifuwarakei.Engine.exe
   ```

2. USIコマンドを手動で入力:

   ```
   usi
   ```

   **期待される応答**:
   ```
   id name Kifuwarabe
   id author muzudho
   usiok
   ```

3. 局面を設定:
   ```
   position startpos
   isready
   ```

4. 思考開始:
   ```
   go btime 10000 wtime 10000
   ```

   **期待される応答**:
   ```
   bestmove 7g7f
   ```

5. 終了:
   ```
   quit
   ```

### デバッグログの確認

USIモードでの通信ログは `Logs/` フォルダーに出力される場合があります。

## 📋 SFEN形式

**SFEN (Shogi Forsyth-Edwards Notation)** は局面を文字列で表現する記法です。

### 形式

```
<盤面> <手番> <持ち駒> <手数>
```

### 例

**平手初期局面**:
```
lnsgkgsnl/1r5b1/ppppppppp/9/9/9/PPPPPPPPP/1B5R1/LNSGKGSNL b - 1
```

**解説**:
- `lnsgkgsnl/...`: 盤面（1段目から9段目）
- `b`: 手番 (b=先手, w=後手)
- `-`: 持ち駒なし
- `1`: 手数

## 🛠️ 対応状況

### ✅ 対応済み

- `usi`
- `isready`
- `usinewgame`
- `position startpos`
- `position startpos moves ...`
- `go`
- `quit`

### ⚠️ 部分対応

- `position sfen ...` (対応しているが動作未確認)
- `setoption` (一部のオプションのみ)

### ❌ 未対応

- `go ponder` (先読み機能)
- `ponderhit`
- 一部の拡張オプション

## 🐛 既知の問題

### 1段目でしか成らないバグ

駒が1段目に到達した場合のみ成る処理が動作します。
2段目や3段目では成りが正しく処理されない可能性があります。

### うさぎの1段目成らず

どうぶつしょうぎモードで、うさぎ（歩）が1段目で成らない問題があります。

### 世界一周バグ

駒が盤の端を超えて反対側に出現することがあります（座標計算のバグ）。

## 📚 参考リソース

- [USI プロトコル仕様](http://shogidokoro.starfree.jp/usi.html)
- [将棋所公式サイト](http://shogidokoro.starfree.jp/)
- [ShogiGUI](http://shogigui.siganus.com/)

## 📚 次に読むドキュメント

- [07-Commands.md](./07-Commands.md) - どうぶつしょうぎモードのコマンド
- [10-Troubleshooting.md](./10-Troubleshooting.md) - USI関連のエラー対処

---

**USIモードで将棋所と対局してみよう！（＾～＾）**
