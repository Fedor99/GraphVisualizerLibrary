using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CalculationSteps;

namespace GraphVisualizer
{
    public partial class Graph : Form
    {
        public Graph(
            int sizeX, 
            int sizeY, 
            int circleSize, 
            GraphPointArray[] graphPointArray, 
            Color bgColor, 
            string name, 
            Size? multiplier = null, 
            bool drawLine = true)
        {
            this.sizeX = sizeX;
            this.sizeY = sizeY;
            this.circleSize = circleSize;
            this.graphPointArray = graphPointArray;
            this.bgColor = bgColor;
            this.name = name;
            this.multiplier = multiplier;
            this.drawLine = drawLine;

            StartForm();
        }

        public void StartForm()
        {
            InitializeComponent();

            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;

            int screenScale = Screen.PrimaryScreen.Bounds.Height;
            if (Screen.PrimaryScreen.Bounds.Width < screenScale)
                screenScale = Screen.PrimaryScreen.Bounds.Width;

            double standardValue = 1080;
            double sizeNormalization = screenScale / standardValue;

            int normalizedSizeX = (int)(sizeX * sizeNormalization);
            int normalizedSizeY = (int)(sizeY * sizeNormalization);

            this.Size = new Size(
                normalizedSizeX,
                normalizedSizeY);

            imageSizeX = normalizedSizeX;
            imageSizeY = normalizedSizeY;

            this.circleSize = (int)(circleSize * sizeNormalization);

            this.graphPointArray = graphPointArray;

            this.bgColor = bgColor;

            this.name = name;

            this.drawLine = drawLine;

            if (multiplier != null)
                this.multiplier = multiplier;

            imageControl = new PictureBox();
            imageControl.Size = new Size(imageSizeX, imageSizeY);
            Controls.Add(imageControl);

            this.Load += Graph_Load;
            imageControl.Paint += OnPaint;
            imageControl.MouseDown += imageControl_MouseDown;

            ShowInTaskbar = true;
            //ShowDialog();
        }

        private int sizeX;
        private int sizeY;

        private int imageSizeX;
        private int imageSizeY;
        private int circleSize;

        public Bitmap renderedImage;
        PictureBox imageControl;

        GraphPointArray[] graphPointArray;

        Color bgColor;

        string name;

        Size? multiplier = new Size(1, 1);

        bool drawLine;

        public void SetGraphPointArray(GraphPointArray[] graphPointArray)
        {
            this.graphPointArray = graphPointArray;
        }

        private void Graph_Load(object sender, EventArgs e)
        {
            //renderedImage = PaintGraph();
        }

        public Bitmap PaintGraph()
        {
            int _imageSizeX = imageSizeX;
            int _imageSizeY = imageSizeY;
            double multiplierX = 1;
            double multiplierY = 1;
            if (multiplier != null)
            {
                multiplierX = multiplier.GetValueOrDefault().Width;
                multiplierY = multiplier.GetValueOrDefault().Height;
                _imageSizeX = (int)(imageSizeX * multiplierX);
                _imageSizeY = (int)(imageSizeY * multiplierY);
            }

            Bitmap bm = new Bitmap(imageSizeX, imageSizeY);
            using (Graphics gfx = Graphics.FromImage(bm))
            using (SolidBrush brush = new SolidBrush(bgColor))
            {
                gfx.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                gfx.FillRectangle(brush, 0, 0, imageSizeX, imageSizeY);

                // Find max and min values  in a graph
                double defaultValueX = graphPointArray[0].GetPoints()[0].GetX();
                double defaultValueY = graphPointArray[0].GetPoints()[0].GetY();

                double minX = defaultValueX;
                double minY = defaultValueY;

                double initialMax = defaultValueX;
                foreach (GraphPointArray gpa in graphPointArray)
                {
                    foreach (Position p in gpa.GetPoints())
                    {
                        if (p.GetX() < minX)
                            minX = p.GetX();
                        if (p.GetY() < minY)
                            minY = p.GetY();

                        if (p.GetX() > initialMax)
                            initialMax = p.GetX();
                        if (p.GetY() > initialMax)
                            initialMax = p.GetY();
                    }
                }

                Console.WriteLine("minX = " + minX);
                Console.WriteLine("minY = " + minY);

                // If minX or minY value is less than zero: increase everything by min*(-1)
                if (minX < 0)
                {
                    foreach (GraphPointArray gpa in graphPointArray)
                    {
                        for (int pointIndex = 0; pointIndex < gpa.GetPoints().Count; pointIndex++)
                        {
                            Position p = gpa.GetPoints()[pointIndex];
                            Position newP = new Position(p.GetX() + (-1 * minX), p.GetY());
                            gpa.GetPoints()[pointIndex] = newP;
                        }
                    }
                }
                if (minY < 0)
                {
                    foreach (GraphPointArray gpa in graphPointArray)
                    {
                        for (int pointIndex = 0; pointIndex < gpa.GetPoints().Count; pointIndex++)
                        {
                            Position p = gpa.GetPoints()[pointIndex];
                            Position newP = new Position(p.GetX(), p.GetY() + (-1 * minY));
                            gpa.GetPoints()[pointIndex] = newP;
                        }
                    }
                }


                double max = graphPointArray[0].GetPoints()[0].GetX();
                double min = graphPointArray[0].GetPoints()[0].GetX();
                foreach (GraphPointArray gpa in graphPointArray)
                {
                    foreach (Position p in gpa.GetPoints())
                    {
                        if (p.GetX() > max)
                            max = p.GetX();
                        if (p.GetY() > max)
                            max = p.GetY();

                        if (p.GetX() < min)
                            min = p.GetX();
                        if (p.GetY() < min)
                            min = p.GetY();
                    }
                }
                Console.WriteLine("max = " + max);
                Console.WriteLine("min = " + min);


                Position previousPoint = null;
                foreach (GraphPointArray gpa in graphPointArray)
                {
                    Color color = gpa.GetColor();
                    Pen pen = new Pen(color);
                    pen.Width = circleSize;
                    foreach (Position p in gpa.GetPoints())
                    {
                        double _minX = 0;
                        double _minY = 0;
                        if (minX > 0)
                            _minX = minX;
                        if (minX < 0)
                            _minX = -minX;

                        if (minY > 0)
                            _minY = minY;
                        if (minY < 0)
                            _minY = -minY;
                        double _x = ((p.GetX() - _minX) / (max - _minX));
                        double _y = ((p.GetY()) / (max));
                        int x = (int)(_x * _imageSizeX);
                        int y = (int)(imageSizeY - (_y * _imageSizeY));

                        Console.WriteLine("p.GetX() = " + p.GetX());
                        Console.WriteLine("p.GetY() = " + p.GetY());
                        Console.WriteLine("max = " + max);
                        Console.WriteLine("_x = " + _x);
                        Console.WriteLine("_y = " + _y);
                        Console.WriteLine("x = " + x);
                        Console.WriteLine("y = " + y);
                        Console.WriteLine();
                        gfx.DrawEllipse(pen,
                            new Rectangle(
                                x - circleSize / 2,
                                y - circleSize / 2,
                                circleSize,
                                circleSize)
                            );

                        
                        if (previousPoint != null)
                        {
                            if (drawLine)
                            {
                                Pen pen1 = new Pen(color);
                                pen1.Width = 2;
                                gfx.DrawLine(pen1,
                                    new Point(x, y),
                                    new Point((int)previousPoint.GetX(), (int)previousPoint.GetY())
                                );
                            }
                        }
                        

                        previousPoint = new Position(x, y);
                    }

                    previousPoint = null;
                }

                Font arialFont = new Font("Arial", 10);
                gfx.DrawString("(y = " + (initialMax / multiplierY) + ")", arialFont, Brushes.White, new PointF(10f, 10f));
                gfx.DrawString("(x = " + minX + ", y = " + minY + ")", arialFont, Brushes.White, new PointF(10f, imageSizeY - 20f));
                gfx.DrawString("(x = " + (initialMax / multiplierX) + ")", arialFont, Brushes.White, new PointF(imageSizeY - 80f, imageSizeY - 20f));


                Font hugeArialFont = new Font("Arial", 14);
                StringFormat nameFormat = new StringFormat();
                nameFormat.LineAlignment = StringAlignment.Center;
                nameFormat.Alignment = StringAlignment.Center;
                gfx.DrawString(name, hugeArialFont, Brushes.White, new PointF(imageSizeX / 2, imageSizeY / 2), nameFormat);


                Pen penBorders = new Pen(Color.White);
                gfx.DrawLine(penBorders,
                    new Point(0, 0),
                    new Point(0, imageSizeY - 1)
                );
                gfx.DrawLine(penBorders,
                    new Point(0, 0),
                    new Point(imageSizeX - 1, 0)
                );
                gfx.DrawLine(penBorders,
                    new Point(0, imageSizeY - 1),
                    new Point(imageSizeX - 1, imageSizeY - 1)
                );
                gfx.DrawLine(penBorders,
                    new Point(imageSizeX - 1, 0),
                    new Point(imageSizeX - 1, imageSizeY - 1)
                );

                renderedImage = bm;
            }

            return bm;
        }


        private void OnPaint(object sender, PaintEventArgs e)
        {
            //imageControl.Image = (Image)renderedImage; 
        }


        #region UI stuff
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HTCAPTION = 0x2;

        [DllImport("User32.dll")]
        public static extern bool ReleaseCapture();

        [DllImport("User32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        private void imageControl_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0);
            }
        }
        #endregion
    }
}