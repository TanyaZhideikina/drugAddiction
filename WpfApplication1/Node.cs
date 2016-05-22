using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace WpfApplication1
{
    /// <summary>
    /// An object representing a node. a node is a part of the binary tree which contains data
    /// </summary>
    public class Node
    {
        /// <summary>
        /// Gets or sets the value of current node, the value is unique
        /// </summary>
        public int Value { get; set; }

        /// <summary>
        /// Gets or sets the right node connected to this node, if any
        /// </summary>
        public Node Right { get; set; }

        /// <summary>
        /// Gets or sets the left node connected to this node, if any
        /// </summary>
        public Node Left { get; set; }

        private Brush _brushColor;
        /// <summary>
        /// Colors of the node
        /// </summary>
        public Brush BrushColor
        {
            get
            {                
                return _brushColor ?? Brushes.Black;
            }
            set { _brushColor = value; }
        }

        /// <summary>
        /// Constructor to create new instance of node
        /// </summary>
        /// <param name="value">
        /// The initial value of the node.
        /// </param>
        public Node(int value)
            : this(value, null, null)
        {
        }

        /// <summary>
        /// Constructor to create new instance of node
        /// </summary>
        /// <param name="value">
        /// The initial value of the node.
        /// </param>
        /// <param name="left">
        /// The node which will be connected to this node as the left node. null is also acceptable
        /// </param>
        /// <param name="right">
        /// The node which will be connected to this node as the right node. null is also acceptable
        /// </param>
        public Node(int value, Node left, Node right)
        {
            Right = left;
            Left = right;
            Value = value;
        }

        /// <summary>
        /// The backgroung image of each nodes, the size of this bitmap affects the quality of 
        /// output image
        /// </summary>
        private static Bitmap _nodeBg = new Bitmap(30, 25);

        /// <summary>
        /// The free space between nodes on the drawed image,
        /// </summary>
        private static Size _freeSpace = new Size(_nodeBg.Width / 8, (int)(_nodeBg.Height * 1.3f));

        /// <summary>
        /// A value which is used, (on drawing the Value of the nodes), in order to make sure the
        /// drawed image would be the same for any size of _nodeBg.
        /// </summary>
        private static readonly float Coef = _nodeBg.Width / 40f;

        /// <summary>
        /// The constructor of static members
        /// </summary>
        static Node()
        {
            var g = Graphics.FromImage(_nodeBg);                                    // get a Graphics from _nodeBg bitmap,
            g.SmoothingMode = SmoothingMode.HighQuality;                            // set the smoothing mode
            var rcl = new Rectangle(1, 1, _nodeBg.Width - 2, _nodeBg.Height - 2);   // get a rectangle of drawer
            g.FillRectangle(Brushes.White, rcl);
            //g.FillEllipse(new LinearGradientBrush(new Point(0, 0), new Point(_me.Width, _me.Height), Color.Goldenrod, Color.Black), rcl);
            g.DrawEllipse(new Pen(Color.Black, 1.2f), rcl);                          // draw ellipse, you could also comment this line, and uncomment the above line as another option for background image
        }

        /// <summary>
        /// Add a node to the left or right side of current node
        /// </summary>
        /// <param name="node">The node to be added</param>
        /// <param name="addLeft">A flag to determine whether to add to left or right side</param>
        public void Add(Node node, bool addLeft)
        {
            if (addLeft) // Add to left
            {
                Left = node;
            }
            else // Add to right
            {
                Right = node;
            }            
        }

        /// <summary>
        /// The location of the node.
        /// </summary>
        private int _lastImageLocationOfStarterNode;
        /// <summary>
        /// Font to be used
        /// </summary>
        private static Font font = new Font("Tahoma", 14f * Coef);

        /// <summary>
        /// Paints the node and its children
        /// </summary>
        /// <param name="center">The location of the node on the top of the drawn image.</param>
        /// <returns>The image representing the current node and its children</returns>
        public Image Draw(out int center)
        {
            center = _lastImageLocationOfStarterNode;

            var lCenter = 0;
            var rCenter = 0;

            Image lNodeImg = null, rNodeImg = null;
            if (Left != null)       // draw left node's image
                lNodeImg = Left.Draw(out lCenter);
            if (Right != null)      // draw right node's image
                rNodeImg = Right.Draw(out rCenter);

            // draw current node and it's childs (left node image and right node image)
            var lSize = new Size();
            var rSize = new Size();
            var under = (lNodeImg != null) || (rNodeImg != null);// if true the current node has childs
            if (lNodeImg != null)
                lSize = lNodeImg.Size;
            if (rNodeImg != null)
                rSize = rNodeImg.Size;

            var maxHeight = lSize.Height;
            if (maxHeight < rSize.Height)
                maxHeight = rSize.Height;

            if (lSize.Width <= 0)
                lSize.Width = (_nodeBg.Width - _freeSpace.Width) / 2;
            if (rSize.Width <= 0)
                rSize.Width = (_nodeBg.Width - _freeSpace.Width) / 2;

            var resSize = new Size
                              {
                                  Width = lSize.Width + rSize.Width + _freeSpace.Width,
                                  Height = _nodeBg.Size.Height + (under ? maxHeight + _freeSpace.Height : 0)
                              };

            var result = new Bitmap(resSize.Width, resSize.Height);
            var g = Graphics.FromImage(result);
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.FillRectangle(Brushes.White, new Rectangle(new Point(0, 0), resSize));
            g.DrawImage(_nodeBg, lSize.Width - _nodeBg.Width / 2 + _freeSpace.Width / 2, 0);
            var str = Value.ToString();
            g.DrawString(str, font, BrushColor, lSize.Width - _nodeBg.Width / 2 + _freeSpace.Width / 2 + (2 + (str.Length == 1 ? 10 : str.Length == 2 ? 5 : 0)) * Coef, _nodeBg.Height / 2f - 12 * Coef);

            center = lSize.Width + _freeSpace.Width / 2;
            var pen = new Pen(Brushes.Black, 1.2f * Coef)
                          {
                              EndCap = LineCap.ArrowAnchor,
                              StartCap = LineCap.Round
                          };

            float x1 = center;
            float y1 = _nodeBg.Height;
            float y2 = _nodeBg.Height + _freeSpace.Height;
            float x2 = lCenter;
            var h = Math.Abs(y2 - y1);
            var w = Math.Abs(x2 - x1);
            if (lNodeImg != null)
            {
                g.DrawImage(lNodeImg, 0, _nodeBg.Size.Height + _freeSpace.Height);
                var points1 = new List<PointF>
                                  {
                                      new PointF(x1, y1),
                                      new PointF(x1 - w/6, y1 + h/3.5f),
                                      new PointF(x2 + w/6, y2 - h/3.5f),
                                      new PointF(x2, y2),
                                  };
                g.DrawCurve(pen, points1.ToArray(), 0.5f);
            }
            if (rNodeImg != null)
            {
                g.DrawImage(rNodeImg, lSize.Width + _freeSpace.Width, _nodeBg.Size.Height + _freeSpace.Height);
                x2 = rCenter + lSize.Width + _freeSpace.Width;
                w = Math.Abs(x2 - x1);
                var points = new List<PointF>
                                 {
                                     new PointF(x1, y1),
                                     new PointF(x1 + w/6, y1 + h/3.5f),
                                     new PointF(x2 - w/6, y2 - h/3.5f),
                                     new PointF(x2, y2)
                                 };
                g.DrawCurve(pen, points.ToArray(), 0.5f);
            }

            _lastImageLocationOfStarterNode = center;
            return result;
        }
        
        /// <summary>
        /// The count of nodes under this node + 1
        /// </summary>
        public int Count
        {
            get
            {
                return 1 + (Left != null ? Left.Count : 0) + (Right != null ? Right.Count : 0);
            }
        }
    }
}