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

        public ShogiEngineImpl ShogiEngine { get; set; }

        #endregion



        /// <summary>
        /// コンストラクターです。
        /// </summary>
        public SetteiXmlFile(string fileName)
        {
            this.fileName = fileName;
            this.setteiFileVer = "0.00.0";
            this.ShogiEngine = new ShogiEngineImpl("The将棋エンジン", "shogiEngine.exe");
        }

        public void DebugWrite()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("設定ファイル          : " + this.FileName);
            sb.AppendLine("設定ファイルVer       : " + this.SetteiFileVer);
            sb.AppendLine("将棋エンジン          : " + this.ShogiEngine.Name);
            sb.AppendLine("将棋エンジン・ファイル: " + this.ShogiEngine.Filepath);
            sb.AppendLine();
            sb.AppendLine();
            sb.AppendLine();
            sb.AppendLine();

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

                XmlNodeList xShogiEngineNodeList = xKifunarabe.GetElementsByTagName("shogiEngine");
                foreach (XmlNode xShogiEngineNode in xShogiEngineNodeList)
                {
                    XmlElement xShogiEngine = (XmlElement)xShogiEngineNode;

                    this.ShogiEngine.Name = xShogiEngine.GetAttribute("name");
                    this.ShogiEngine.Filepath = xShogiEngine.GetAttribute("file");
                    break;
                }
            }
            catch(Exception ex)
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

                // <shogiEngine>
                XmlElement xShogiEngine = xDoc.CreateElement("shogiEngine");

                // name="The将棋エンジン"
                xShogiEngine.SetAttribute("name", this.ShogiEngine.Name);

                // file="shogiEngine.exe"
                xShogiEngine.SetAttribute("file", this.ShogiEngine.Filepath);

                xKifunarabe.AppendChild(xShogiEngine);

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
