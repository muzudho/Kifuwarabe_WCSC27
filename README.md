# きふわらべ WCSC27

2020年11月の 電竜戦から きふわらけい(Kifuwarakei)にリネームして開発再開だぜ☆（＾～＾）  

|                         | ファイル                                              |
| ----------------------- | ----------------------------------------------------- |
| ソース                  | `Kifuwarabe_WCSC27/Kifuwarabe_WCSC27/Kifuwarakei.sln` |
| 将棋エンジン ソース     | Kifuwarakei project                                   |
| GUI                     | なし                                                  |
| 将棋エンジン ランタイム | `Kifuwarabe_WCSC27/Game/kifuwarabe_wcsc27.exe`        |
| 設定ファイル1           | `Kifuwarabe_WCSC27/Game/kifuwarabe_wcsc27.exe.config` |
| 設定ファイル2           | `Kifuwarabe_WCSC27/Profile/Engine.toml`               |

* `Kifuwarabe_WCSC25` のトップ・ディレクトリーに `Logs` ディレクトリーを作成してください。
* `Kifuwaragyoku.sln` を `Release` モードで ビルドしてください。
* 設定ファイル1 の `Grayscale.A500_ShogiEngine.exe.config` の中にある `Profile` のパスを、 設定ファイル2 の親ディレクトリー `Profile` に合わせてください。  

## Manual

MIT License
https://opensource.org/licenses/mit-license.php

Game フォルダーに kifuwarabe_wcsc27.exe を入れてくれだぜ☆（＾～＾）Visual Studio 2017 とかでコンパイルしろだぜ☆（＾～＾）
大会参加のものを そのまま放り込むぜ☆（＾～＾）

このバージョンは開発終了で、次は設計から手を入れ直すぜ☆（＾～＾）
Unityで動くようにすれば Macユーザーでも遊べるのではないか、という改造だぜ☆(＾◇＾)


- [USIモード] USIエンジンなんで将棋所に登録できるぜ☆（＾～＾）
- [どうぶつしょうぎモード](Windows コマンドプロンプト用) .exe をダブルクリックして 黒い画面で [Enter]キーを押せば どうぶつしょうぎ で遊べるぜ☆（＾～＾）
- ビットボードは128bitなんで、3x4 盤～9x9盤 の間で遊んでくれだぜ☆（＾～＾）
- １段目でしか成らないバグや、うさぎの１段目成らず、盤の反対側へ世界一周したりするバグなど  様々な設計忘れがあるぜ☆（＾～＾）

## しんでる機能

- 機械学習、定跡、成績 の３つは 3x4盤 にしか対応してないので、機能をころしてしまったぜ☆（＾～＾） 3x4盤でも動かなくなった☆（＾～＾）



## どうぶつしょうぎモード の説明

- 「man」コマンドで説明が読めるぜ☆（＾～＾）

よく使うコマンド

- 「@」コマンドで外部スクリプト・ファイル実行。Game/Command フォルダー下のテキストファイルを読めだぜ☆
- 「set」コマンドで各種設定。
- 「do」コマンドで指す。
- 「undo」コマンドで戻す。
- 「ky」で盤面表示。kyokumen。
- 「sasite」「kiki」で情報表示。


なんか勘で使ってくれだぜ☆（＾～＾）


ソースコードの解説は Qiita に書いていくが、開発終わってるんで　ソースコードの改良は行わないぜ☆（＾～＾）
http://qiita.com/muzudho1/items/f520859a997f16f2948f

