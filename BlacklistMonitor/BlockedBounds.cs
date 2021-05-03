using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace BlacklistMonitor
{
    /// <summary>
    /// Represents the bounds the cursor is not supposed to cross.
    /// Due to how the blocking mechanism works we need subtract one pixel from the left and top of the blocked bounds 
    /// when comparing them to the cursor position.
    /// </summary>
    class BlockedBounds
    {
        #region Fields
        /// <summary>
        /// Left position of the rectangle.
        /// </summary>
        private int left;
        /// <summary>
        /// Top position of the rectangle.
        /// </summary>
        private int top;
        /// <summary>
        /// Right position of the rectangle.
        /// </summary>
        private int right;
        /// <summary>
        /// Bottom position of the rectangle.
        /// </summary>
        private int bottom;
        #endregion

        #region Constructors
        public BlockedBounds(int left, int top, int right, int bottom)
        {
            this.left = left;
            this.top = top;
            this.right = right;
            this.bottom = bottom;
        }

        public BlockedBounds(System.Windows.Rect rect)
        {
            left = (int)rect.Left;
            top = (int)rect.Top;
            right = (int)rect.Right;
            bottom = (int)rect.Bottom;
        }
        #endregion

        #region Properties
        public int Left
        {
            get
            {
                return left - 1;
            }
        }

        public int OgLeft { get => left; }

        public int Top
        {
            get
            {
                return top - 1;
            }
        }

        public int OgTop { get => top; }

        public int Right
        {
            get
            {
                return right;
            }
        }

        public int OgRight { get => right; }

        public int Bottom
        {
            get
            {
                return bottom;
            }
        }

        public int OgBottom { get => bottom; }
        #endregion

        #region Operators
        /// <summary>
        /// Operator to convert a BlockedBounds to Drawing.Rectangle.
        /// </summary>
        /// <param name="rect">BlockedBounds to convert.</param>
        public static implicit operator Rectangle(BlockedBounds rect)
        {
            return Rectangle.FromLTRB(rect.Left, rect.Top, rect.Right, rect.Bottom);
        }

        /// <summary>
        /// Operator to convert Drawing.Rectangle to a BlockedBounds.
        /// </summary>
        /// <param name="rect">Rectangle to convert.</param>
        public static implicit operator BlockedBounds(Rectangle rect)
        {
            return new BlockedBounds(rect.Left, rect.Top, rect.Right, rect.Bottom);
        }
        #endregion

        public bool Contains(Point point)
        {
            return (point.X < Right && point.X > Left && point.Y > Top && point.Y < Bottom);
        }
        
        public bool Equals(Rectangle rectangle)
        {
            return rectangle.Left == left && rectangle.Top == top && rectangle.Right == right && rectangle.Bottom == bottom;
        }
    }
}
