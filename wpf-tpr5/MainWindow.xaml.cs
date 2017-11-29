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
        geometry_controller GlobalGeometryController = null;
        public MainWindow()
        {
            InitializeComponent();
            //canvasMain.MouseLeftButtonUp += Ellipse_MouseLeftButonUp;
            GlobalGeometryController = new geometry_controller(50 , 50 , 30 , 30 , circleNameTextBox.Text , ref canvasMain);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            GlobalGeometryController.addNewFigure();
        }

        private void Ellipse_MouseLeftButonUp(object sender, MouseButtonEventArgs e)
        {
            debugTextBox.AppendText("\nX:"+e.GetPosition(this).X + " Y:" + e.GetPosition(this).Y);
        }

        private void button_Click_1(object sender, RoutedEventArgs e)
        {

        }
    }
}