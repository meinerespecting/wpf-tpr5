using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Drawing;

namespace geometry_shapes
{
    /*
     * Контроллер для всех Геометрий.
     * Обработчики висят на нем.
     * 
     * 
     */
    public class geometry_controller : UIElement
    {
        int count = 0;
        List<geometry_circle> totalGeometries = new List<geometry_circle>();

        int _x;
        int _y;
        int _width;
        int _height;
        string _labelText;
        Canvas _canvas;

        /// <summary>
        /// arguments
        /// </summary>
        /// <param name="x">координата x</param>
        /// <param name="y">координата y</param>
        /// <param name="width">ширина</param>
        /// <param name="height">высота</param>
        /// <param name="labelText">подпись вершины</param>
        /// <param name="canvas">ссылка на Canvas</param>

        //saving it's params to add new figures simply
        public geometry_controller(int x, int y, int width, int height, string labelText, ref Canvas canvas)
        {
            _x = x;
            _y = y;
            _width = width;
            _height = height;
            _labelText = labelText;
            _canvas = canvas;

            count++;
            totalGeometries.Add(new geometry_circle(x , y , width , height , labelText , count , ref canvas));
        }
        public void addNewFigure(string newLabelText)
        {
            count++;
            totalGeometries.Add(new geometry_circle(_x , _y , _width , _height , newLabelText , count , ref _canvas));
        }

        private void MouseLeftButtonUp(object sender , MouseButtonEventArgs e)
        {
            
        }
    }
    public class geometry_circle : UIElement
    {
        public Ellipse el = null;
        private Ellipse selectedEllipseForLine = null;

        private double _x = 10, _y = 10;
        private double _width = 20, _height = 20;
        private int count;//for searching in List by this ref

        public Label textLabel = null;

        public string name = "default name";
        private string debugStr = "";

        private bool isPressed = false;

        private Point begin, curr;
        private Line innerLine1 = null;
        private Line outerLine1 = null;
        private Line outerLine2 = null;

        private Canvas currentCanvas = null;//need it for placing ellipses and lines inside the class
        private new void MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            debugStr += "\nMouseLeftButtonDown";

            el = (Ellipse)sender;
            begin = e.GetPosition(null);//HOW IS THIOS WORKS HUH?
            curr = begin;//need it for labels
            isPressed = true;
            el.CaptureMouse();
        }

        private new void MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            debugStr += "\nMouseRightButtonDown";
            Ellipse currEl = (Ellipse)sender;
            textLabel.Content = "clicled for drawing[" + curr.X + "." + curr.Y + "]";
            var x1 = Canvas.GetLeft(currEl);
            var y1 = Canvas.GetTop(currEl);
            var x2 = Canvas.GetLeft(selectedEllipseForLine);
            var y2 = Canvas.GetTop(selectedEllipseForLine);
            /*myLine.X1 =  x1;
              myLine.X2 =  y1;
              myLine.Y1 =  x2;
              myLine.Y2 =  y2;*/

            //double x1, double y1, double x2, double y2
            DrawLineBetweenCoordinates(x1, y1, x2, y2);
        }
        private new void MouseMove(object sender, MouseEventArgs e)
        {
            if (isPressed)
            {
                el = (Ellipse)sender;
                curr = e.GetPosition(null);
                el.SetValue(Canvas.LeftProperty, curr.X - 25);
                el.SetValue(Canvas.TopProperty, curr.Y - 25);
                refreshLabelCoords();//тягаем поганца за собой
            }

        }

        private new void MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            el = (Ellipse)sender;
            if (isPressed)
            {
                isPressed = false;
                el.ReleaseMouseCapture();
                //return;
            }
            selectedEllipseForLine = el;//aibt working for some reason, so there will be runtimes!

        }

        public geometry_circle(int x, int y, int height, int width, string labelText, int count, ref Canvas canvas)// 1, 1, this.Handle
        {
            el = new Ellipse();
            currentCanvas = canvas;//saves reference to our  canvas for drawing lines

            setFigureSize(height, width);
            setFigureCoords(x, y);

            name = labelText;
            this.count = count;//for searching in List<geometries>
            setEllipseSize();
            el.VerticalAlignment = VerticalAlignment.Top;
            el.Fill = Brushes.Green;//заполнили зеленым
            el.Stroke = Brushes.Red;//обвели красным
            el.StrokeThickness = 3;
            el.SetValue(Canvas.TopProperty, (double)_y);
            el.SetValue(Canvas.LeftProperty, (double)_x);
            //el.MouseLeftButtonDown += MouseLeftButtonDown;
            el.MouseUp += MouseLeftButtonUp;
            el.MouseLeftButtonDown += MouseLeftButtonDown;
            el.MouseRightButtonDown += MouseRightButtonDown;
            el.MouseMove += MouseMove;
            currentCanvas.Children.Add(el);

            if (labelText.Length > 0)//если дали строку лэйблу или она не пустая
            {
                textLabel = new Label();
                textLabel.Content = labelText;
                canvas.Children.Add(textLabel);
                setPointsOnSpawn(x, y);
            }

        }
        private void refreshLabelCoords()
        {
            if (textLabel != null)
            {
                textLabel.SetValue(Canvas.LeftProperty, curr.X - 45);
                textLabel.SetValue(Canvas.TopProperty, curr.Y - 50);
            }
        }

        /*
         * Присваивает заданные координаты полю данного класса
         * Используется в перетягивании обьектов
         */
        private void setCurrPoints(double x, double y)
        {
            curr.X = x;
            curr.Y = y;
        }

        /* Устанавливает координаты фигуры на полотне.
         * Не может быть отрицательным числом.
         */
        private void setFigureCoords(double x, double y)
        {
            if (x > 0) _x = x;
            if (y > 0) _y = y;
        }

        /* Устанавливает высоту и ширину фигуры.
         * Не может быть отрицательным числом.
         */
        private void setFigureSize(double height, double width)
        {
            if (height > 0) _height = height;
            if (width > 0) _width = width;
        }

        /* Устанавливает высоту и ширину создаваемого эллипса.
         * Требуется вызов setFigureSixe(x, y)
         */
        private void setEllipseSize()
        {
            if (el != null) {
                if (_height > 0) el.Height = _height;
                if (_width > 0) el.Width = _width;
            }
        }

        //костыль, в будущем заменить или поместить в какой-нибудь еще метод
        private void setPointsOnSpawn(double x, double y)
        {
            if (curr.X == 0) curr.X = x;
            if (curr.Y == 0) curr.Y = y ;
            textLabel.SetValue(Canvas.LeftProperty, curr.X - 18);
            textLabel.SetValue(Canvas.TopProperty, curr.Y - 25);
        }
        private void DrawLineBetweenCoordinates(double x1, double y1, double x2, double y2)
        { 
            debugStr += "\nEntered DrawLineCoords";
            Line myLine = new Line();

            myLine.Stroke = Brushes.Black;
            myLine.X1 =  x1;
            myLine.X2 =  y1;
            myLine.Y1 =  x2;
            myLine.Y2 =  y2;
            myLine.StrokeThickness = 2;
            currentCanvas.Children.Add(myLine);

            myLine.UpdateLayout();
            debugStr += "\nChildren.Add!";
        }
        public string ellipseCoords()
        {
            string coords = "defaultc coords!";

            coords = "\nWidth:" + _width.ToString() + " Height:" + _height.ToString() + 
                     "\nBeginY:" + begin.Y.ToString() + " BeginX:" + begin.X.ToString() + 
                     "\nCurrY:" + curr.Y.ToString() + " CurrX:" + curr.X.ToString();
            return coords;
        }
        public string getLines()
        {
            return debugStr;
        }
    }
}

