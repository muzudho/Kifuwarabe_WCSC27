using Grayscale.A060_Application.B210_Tushin_____.C500____Util;
using System;
using System.IO;
using System.Text;
using System.Xml;

namespace Grayscale.A060_Application.B310_Settei_____.C510____Xml
{

    /// <summary>
    /// ************************************************************************************************************************
    /// 設定XMLファイル
    /// ************************************************************************************************************************
    /// </summary>
    public class SetteiXmlFile
    {

        #region プロパティ類

        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// 設定XMLファイル名です。
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        public string FileName
        {
            get
            {
                return this.fileName;
            }
        }
        private string fileName;

        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// 設定ファイルのバージョンです。
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        public string SetteiFileVer
        {
            get
            {
                return this.setteiFileVer;
            }
        }
        private string setteiFileVer;

        public ShogiEngineImpl Player1 { get; set; }
        public ShogiEngineImpl Player2 { get; set; }

        #endregion



        /// <summary>
        /// コンストラクターです。
        /// </summary>
        public SetteiXmlFile(string fileName)
        {
            this.fileName = fileName;
            this.setteiFileVer = "0.00.0";
            this.Player1 = new ShogiEngineImpl("P1未使用", "misiyou_shogiEngine.exe");
            this.Player2 = new ShogiEngineImpl("P2未使用", "misiyou_shogiEngine.exe");
        }

        public void DebugWrite()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("設定ファイル          : " + this.FileName);
            sb.AppendLine("設定ファイルVer       : " + this.SetteiFileVer);

            sb.AppendLine("----");
            sb.AppendLine("(1P)将棋エンジン         : " + this.Player1.Name);
            sb.AppendLine("    将棋エンジン・ファイル: " + this.Player1.Filepath);
            sb.AppendLine("----");
            sb.AppendLine("(2P)将棋エンジン         : " + this.Player2.Name);
            sb.AppendLine("    将棋エンジン・ファイル: " + this.Player2.Filepath);
            sb.AppendLine("----");

            Util_Message.Whisper(sb.ToString());
        }

        public bool Exists()
        {
            return File.Exists(this.FileName);
        }

        public bool Read()
        {
            bool successfule = true;

            XmlDocument xDoc = new XmlDocument();

            try
            {
                xDoc.Load(this.fileName);

                XmlElement xKifunarabe = xDoc.DocumentElement;
                this.setteiFileVer = xKifunarabe.GetAttribute("setteiFileVer");

                XmlNodeList xPlayer1Nodelist = xKifunarabe.GetElementsByTagName("player1");
                foreach (XmlNode xPlayer1Node in xPlayer1Nodelist)
                {
                    XmlElement xPlayer1 = (XmlElement)xPlayer1Node;
                    this.Player1 = new ShogiEngineImpl(
                        xPlayer1.GetAttribute("name"),
                        xPlayer1.GetAttribute("file")
                        );
                    break;
                }

                XmlNodeList xPlayer2Nodelist = xKifunarabe.GetElementsByTagName("player2");
                foreach (XmlNode xPlayer2Node in xPlayer2Nodelist)
                {
                    XmlElement xPlayer2 = (XmlElement)xPlayer2Node;
                    this.Player2 = new ShogiEngineImpl(
                        xPlayer2.GetAttribute("name"),
                        xPlayer2.GetAttribute("file")
                        );
                    break;
                }
            }
            catch (Exception ex)
            {
                // エラー
                successfule = false;
                Util_Message.Whisper(ex.GetType().Name+"　"+ex.Message);
            }

            return successfule;
        }


        public bool Write()
        {
            bool successfule = true;

            XmlDocument xDoc = new XmlDocument();

            // UTF-8 エンコーディングで書くものとします。
            XmlProcessingInstruction xPi = xDoc.CreateProcessingInstruction("xml", "version=\"1.0\" encoding=\"UTF-8\"");
            xDoc.AppendChild(xPi);


            try
            {
                // ルート要素 <kifunarabe> を作成
                XmlElement xKifunarabe = xDoc.CreateElement("kifunarabe");
                xDoc.AppendChild(xKifunarabe);

                // setteiFileVer="1.00.0"
                xKifunarabe.SetAttribute("setteiFileVer", this.SetteiFileVer);

                // コメント
                xKifunarabe.AppendChild(xDoc.CreateComment("v(^-^)vｲｪｰｲ☆ 『将棋ＧＵＩ きふならべ』の設定ファイルなんだぜ☆！ 今は一番上に書いてある ＜shogiEngine＞ を見に行くぜ☆"));

                // <player1>
                XmlElement xPlayer1 = xDoc.CreateElement("player1");
                xPlayer1.SetAttribute("name", this.Player1.Name);
                xPlayer1.SetAttribute("file", this.Player1.Filepath);
                xKifunarabe.AppendChild(xPlayer1);

                // <player2>
                XmlElement xPlayer2 = xDoc.CreateElement("player2");
                xPlayer2.SetAttribute("name", this.Player2.Name);
                xPlayer2.SetAttribute("file", this.Player2.Filepath);
                xKifunarabe.AppendChild(xPlayer2);

                // .xmlファイルを保存
                xDoc.Save(this.FileName);
            }
            catch (Exception ex)
            {
                // エラー
                successfule = false;
                Util_Message.Show(ex.GetType().Name + "　" + ex.Message);
            }

            return successfule;
        }




    }
}
