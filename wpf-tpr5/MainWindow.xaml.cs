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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using geometry_shapes;

namespace wpf_tpr5
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        geometry_circle currentFigure;

        List<geometry_circle> mEllipses = new List<geometry_circle>();
        List<Line> myLines = new List<Line>();
        // The current drawing ellipse
        public MainWindow()
        {
            InitializeComponent();
            canvasMain.MouseLeftButtonUp += Ellipse_MouseLeftButonUp;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            geometry_circle C = new geometry_circle(50, 50, 30, 30, circleNameTextBox.Text, ref canvasMain);

            mEllipses.Add(C);
            currentFigure = C;

            debugTextBox.AppendText("\nAdded element!");
        }
        /*private void DrawLineBetween(Ellipse el1, Ellipse el2, ref Canvas canvas)
        {
            Line myLine = new Line();
            myLine.Stroke = System.Windows.Media.Brushes.LightSteelBlue;
            myLine.X1 = Canvas.GetLeft(el1) + 25;
            myLine.X2 = Canvas.GetLeft(el2) + 25; ;
            myLine.Y1 = Canvas.GetTop(el1) + 25;
            myLine.Y2 = Canvas.GetTop(el2) + 25;
            myLine.HorizontalAlignment = HorizontalAlignment.Left;
            myLine.VerticalAlignment = VerticalAlignment.Center;
            myLine.StrokeThickness = 2;
           

            canvas.Children.Add(myLine);
        }*/
        private void Ellipse_MouseLeftButonUp(object sender, MouseButtonEventArgs e)
        {
            debugTextBox.AppendText("\nX:"+e.GetPosition(this).X + " Y:" + e.GetPosition(this).Y);
        }

        private void button_Click_1(object sender, RoutedEventArgs e)
        {
            debugTextBox.AppendText(currentFigure.getLines());
            /*if(mEllipses.Count > 0) debugTextBox.AppendText("\nC.handlerWork : " + mEllipses.Last().handlerWork);
            if(currentFigure != null)debugTextBox.AppendText(currentFigure.ellipseCoords());*/
        }
    }
}