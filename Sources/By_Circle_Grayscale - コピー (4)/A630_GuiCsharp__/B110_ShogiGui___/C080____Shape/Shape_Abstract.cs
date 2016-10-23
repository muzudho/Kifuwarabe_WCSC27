﻿
using System.Drawing;

namespace Grayscale.A630_GuiCsharp__.B110_ShogiGui___.C080____Shape
{

    /// <summary>
    /// ************************************************************************************************************************
    /// 描かれる図画です。共通の内容です。
    /// ************************************************************************************************************************
    /// </summary>
    public abstract class Shape_Abstract
    {

        #region プロパティー

        public string WidgetName { get { return this.widgetName; } }
        private string widgetName;

        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// 表示／非表示
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        public bool Visible
        {
            get;
            set;
        }

        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// 位置とサイズ
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        public Rectangle Bounds
        {
            get
            {
                return this.bounds;
            }
        }
        public void SetBounds(Rectangle rect)
        {
            this.bounds = rect;
        }
        private Rectangle bounds;

        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// 背景色
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        public Color BackColor
        {
            get;
            set;
        }

        #endregion


        /// <summary>
        /// ************************************************************************************************************************
        /// コンストラクターです。
        /// ************************************************************************************************************************
        /// </summary>
        public Shape_Abstract(string widgetName, int x, int y, int width, int height)
        {
            this.widgetName = widgetName;
            this.Visible = true;
            this.bounds = new Rectangle(x,y,width,height);
        }


        /// <summary>
        /// ************************************************************************************************************************
        /// マウスカーソルに重なっているか否か。
        /// ************************************************************************************************************************
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public bool HitByMouse(int x, int y)
        {
            bool hit = false;

            if (this.Visible && this.Bounds.Contains(x, y))
            {
                hit = true;
            }

            return hit;
        }

    }
}
