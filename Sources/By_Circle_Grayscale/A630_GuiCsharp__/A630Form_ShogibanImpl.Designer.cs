﻿namespace Grayscale.A630_GuiCsharp__
{
    partial class A630Form_ShogibanImpl
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.uc_Form_Shogi = new Grayscale.A630_GuiCsharp__.A630Uc_ShogibanImpl();
            this.SuspendLayout();
            // 
            // ui_PnlMain1
            // 
            this.uc_Form_Shogi.Location = new System.Drawing.Point(-1, 0);
            this.uc_Form_Shogi.Name = "ui_PnlMain1";
            this.uc_Form_Shogi.Size = new System.Drawing.Size(1039, 761);
            this.uc_Form_Shogi.TabIndex = 0;
            // 
            // Ui_Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1050, 773);
            this.Controls.Add(this.uc_Form_Shogi);
            this.Name = "Ui_Form1";
            this.Text = "Grayscale.KifuNarabe";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Ui_Form1_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Ui_Form1_FormClosed);
            this.Load += new System.EventHandler(this.Ui_Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private A630Uc_ShogibanImpl uc_Form_Shogi;





    }
}

