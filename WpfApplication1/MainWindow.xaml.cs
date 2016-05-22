using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

namespace WpfApplication1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private List<int> EclipseIDArray = new List<int>();
        private void Insert_Click(object sender, RoutedEventArgs e)
        {
            CreateCircle();
        }

        private List<int> Numbers = new List<int>();
        private int index = -1;
        private List<Ellipse> ellipses = new List<Ellipse>();
        private void CreateCircle()
        {
            if (InsertTextBox.Text != "")
            {
                index++;
                Numbers.Add(Convert.ToInt32(InsertTextBox.Text));
                Ellipse myEllipse = new Ellipse();
                myEllipse.StrokeThickness = 1;
                myEllipse.Stroke = System.Windows.Media.Brushes.Black;
                if(CircleHeight.Text == "")
                {
                    myEllipse.Height = 50;
                }
                else
                {
                    myEllipse.Height = Convert.ToInt32(CircleHeight.Text);
                }
                if(CircleWidth.Text == "")
                {
                    myEllipse.Width = 50;
                }
                else
                {
                    myEllipse.Width = Convert.ToInt32(CircleWidth.Text);
                }
                myEllipse.Name = "Ellipse" + Numbers[index].ToString();
                myEllipse.Fill = new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#FFF4F4F5"));
                Canvas.SetLeft(myEllipse, 40);
                Canvas.SetTop(myEllipse, 50);
                TextBlock myTextBlock = new TextBlock();
                myTextBlock.Inlines.Add("1");
                //myEllipse.Fill = myTextBlock;
                //myEllipse.TextInput = myTextBlock;
                MainGrid.Children.Add(myEllipse);
                MainGrid.Children.Add(myTextBlock);
                if(index != 0)
                {
                    CircleAddWay(myEllipse);
                }
                ellipses.Add(myEllipse);
            }
        }

        private void CircleAddWay(Ellipse myEllipse)
        {
            var sb = FindResource("ellipseSB") as Storyboard;
            sb.Stop();
            //PathGeometry pathGeometry = new PathGeometry();
            DoubleAnimation da = new DoubleAnimation();
            da.From = 0;
            da.To = -100;
            da.Duration = new Duration(TimeSpan.FromSeconds(3));
            TranslateTransform rt = new TranslateTransform();
            myEllipse.RenderTransform = rt;
            rt.BeginAnimation(TranslateTransform.XProperty, da);
            da.From = 0;
            da.To = 100;
            rt.BeginAnimation(TranslateTransform.YProperty, da);
        }
        private BinaryTree _tree = new BinaryTree();
        private int _baseX = 2;
        private int _baseY = 1;
        private int _maxDeepLevel = 7;
        private Random _rnd = new Random();
        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            //Default settings
            //txtBaseX.Text = _baseX.ToString();
            //txtBaseY.Text = _baseY.ToString();
            //txtTreeLevel.Text = _maxDeepLevel.ToString();

            //Create a random binary tree
            CreateTree();
        }

        private void CreateTree()
        {
            //Init the tree
            _tree = new BinaryTree();
            //lblMsg.Text = @"Random binary tree created";

            //Add a new node, to the right side of the root node
            Node currentNode = new Node(0);
            _tree.Add(currentNode, false);

            //Populate its children nodes
            PopulateLeftRightNode(currentNode, 0);

            //Draw the tree
            PaintTree();

            //Update label
            //lblCount.Text = _tree.Count.ToString();
        }

        private Node PopulateLeftRightNode(Node node, int level)
        {
            if (level < _maxDeepLevel)
            {
                level += 1;

                node.Left = PopulateLeftRightNode(new Node(node.Value + _baseX), level);

                //Randomly create the right child node
                if (_rnd.Next(1, 999) % 2 > 0)
                {
                    node.Right = PopulateLeftRightNode(new Node(node.Value + _baseY), level);
                }
            }

            return node;
        }

        private void PaintTree()
        {
            if (_tree == null) return;

            // ImageSource ...

            BitmapImage bi = new BitmapImage();

            bi.BeginInit();
            MemoryStream ms = new MemoryStream();
            _tree.Draw().Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);

            // Rewind the stream...

            ms.Seek(0, SeekOrigin.Begin);
            // Tell the WPF image to use this stream...
            bi.StreamSource = ms;
            bi.EndInit();
            image.Source = bi;
            //MainGrid.Children.Add(bi);
        }

    }
}
