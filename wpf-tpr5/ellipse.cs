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
    public class geometry_circle : UIElement
    {
        public Ellipse el = null;//экземпляр этого класса, который отрисовывается
        private Ellipse selectedEllipseForLine = null;

        private double _x = 10, _y = 10;
        private double _width = 20, _height = 20;

        public Label textLabel = null;

        public string name = "default name";
        public string handlerWork = "1";
        private string debugStr = "";

        //Обработка перетягивания
        bool isPressed = false;


        //private Point innerLine, outerLinePoint1, outerLinePoint2;
        private Point begin, curr;
        public Line innerLine1 = null;//прорисовка линий для отображения жадного алгоритма
        public Line outerLine1 = null;
        public Line outerLine2 = null;

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
            Ellipse currEl = (Ellipse)sender;//clicking on some else figure will make it marked
            /*if (selectedEllipseForLine == null) selectedEllipseForLine = buf;*/
            textLabel.Content = "clicled for drawing[" + curr.X + "." + curr.Y + "]";
            var x1 = Canvas.GetTop(currEl);
            var y1 = Canvas.GetTop(selectedEllipseForLine);
            var x2 = Canvas.GetLeft(currEl);
            var y2 = Canvas.GetLeft(selectedEllipseForLine);
            //DrawLineBetweenEllipses(ref currEl , ref selectedEllipseForLine /*, ref currentCanvas*/);
            DrawLineBetweenCoordinates(x1, y1, x2, y2);
        }
        private new void MouseMove(object sender, MouseEventArgs e)
        {
            if (isPressed)
            {
                el = (Ellipse)sender;
                curr = e.GetPosition(null);
                el.SetValue(System.Windows.Controls.Canvas.LeftProperty, curr.X - 25);
                el.SetValue(System.Windows.Controls.Canvas.TopProperty, curr.Y - 25);
                refreshLabelCoords();//тягаем поганца за собой
            }

        }

        private new void MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
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

        public geometry_circle(int x, int y, int height, int width, string labelText, ref Canvas canvas)// 1, 1, this.Handle
        {
            el = new Ellipse();
            currentCanvas = canvas;//saves reference to our  canvas for drawing lines

            setFigureSize(height, width);
            setFigureCoords(x, y);

            name = labelText;
            setEllipseSize();
            el.VerticalAlignment = VerticalAlignment.Top;
            el.Fill = Brushes.Green;//заполнили зеленым
            el.Stroke = Brushes.Red;//обвели красным
            el.StrokeThickness = 3;
            el.SetValue(System.Windows.Controls.Canvas.TopProperty, (double)_y);
            el.SetValue(System.Windows.Controls.Canvas.LeftProperty, (double)_x);
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
                textLabel.SetValue(System.Windows.Controls.Canvas.LeftProperty, curr.X - 45);
                textLabel.SetValue(System.Windows.Controls.Canvas.TopProperty, curr.Y - 50);
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
            textLabel.SetValue(System.Windows.Controls.Canvas.LeftProperty, curr.X - 18);
            textLabel.SetValue(System.Windows.Controls.Canvas.TopProperty, curr.Y - 25);
        }
        private void DrawLineBetweenCoordinates(double x1, double y1, double x2, double y2)
        { 
            debugStr += "\nEntered DrawLineCoords";
            Line myLine = new Line();

            myLine.Stroke = System.Windows.Media.Brushes.Black;
            //if change this to numbers - it works
            myLine.X1 =  x1;
            myLine.X2 =  x2;
            myLine.Y1 =  y1;
            myLine.Y2 =  y2;
            myLine.HorizontalAlignment = HorizontalAlignment.Left;
            myLine.VerticalAlignment = VerticalAlignment.Center;
            myLine.StrokeThickness = 2;
            currentCanvas.Children.Add(myLine);

            myLine.UpdateLayout();
            debugStr += "\nChildren.Add!";
        }
        private void DrawLineBetweenEllipses(ref Ellipse el1 ,ref Ellipse el2)/* , ref Canvas canvas)*/
        {
//            if (el1 == el2) return;//throw exception blah-blah
            debugStr += "\nEntered DrawLine";
            Line myLine = new Line();
            /*if (outerLine1 == null)
            {//saves reference to connected Line
                outerLine1 = myLine;
                debugStr += "\nOuterLine1";
            }
            else
            {
                outerLine2 = myLine;
                debugStr += "\nOuterLine2";
            }*/
            myLine.Stroke = System.Windows.Media.Brushes.Black;
            //if change this to numbers - it works
            myLine.X1 = (int)(Canvas.GetLeft(el1) + 25.0);
            myLine.X2 = (int)(Canvas.GetLeft(el2) + 25.0); 
            myLine.Y1 = (int)(Canvas.GetTop(el1) + 25.0);
            myLine.Y2 = (int)(Canvas.GetTop(el2) + 25.0);
            myLine.HorizontalAlignment = HorizontalAlignment.Left;
            myLine.VerticalAlignment = VerticalAlignment.Center;
            myLine.StrokeThickness = 2;

            myLine.UpdateLayout();
            debugStr += "\nChildren.Add!";
        }
        public string ellipseCoords()
        {
            string coords = "defaultc coords!";

            //coords = "";
            coords = "\nWidth:" + _width.ToString() + " Height:" + _height.ToString() + 
                     "\nBeginY:" + begin.Y.ToString() + " BeginX:" + begin.X.ToString() + 
                     "\nCurrY:" + curr.Y.ToString() + " CurrX:" + curr.X.ToString();
            return coords;
        }
        public string getLines()
        {
            //string answer = "";
            //answer += outerLine1.ToString();
            //answer += outerLine2.ToString();
            return debugStr;
        }
    }
}

