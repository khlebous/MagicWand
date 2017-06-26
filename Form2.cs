using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MagicWand
{
    public partial class Form2 : Form
    {
        private static int moveTimerInterval = 100;
        private static int degrees = 360;
        private static int minEdges = 3;
        private static int circleNm = 8;
        private static int minSpeed = 5;
        private static int maxSpeed = 10;
        private static double angleStep = 0.1;
        private static int screenWidth = Screen.PrimaryScreen.Bounds.Width;
        private static int screenHeight = Screen.PrimaryScreen.Bounds.Height;

        private static int a = 50; // help to find next vertex of poligon
        private static int b = 50;
        private static int r = 40;

        private Random _rnd;
        private Form1 _parent;
        private int _formSize = 100;
        private SolidBrush _backgroundColor;
        private int _liveTime = 5000;
        private double _angleWhereMove;
        private int _polygonEdges; // 8-circle
        private int _speed;

        private Timer _liveTimer;
        private Timer _moveTimer;

        private int _xMove;
        private int _yMove;
        private double _opacityStep = 0.07;
        private double _angleNow = 0;

        public Form2(Form1 parent)
        {
            _rnd = new Random(System.DateTime.Now.Millisecond);
            _parent = parent;
            InitializeComponent();

            this.ClientSize = new System.Drawing.Size(_formSize, _formSize);
            this.SetStyle(
               //System.Windows.Forms.ControlStyle  s.UserPaint |
               System.Windows.Forms.ControlStyles.AllPaintingInWmPaint |
               System.Windows.Forms.ControlStyles.OptimizedDoubleBuffer,
               true);
            this.TransparencyKey = Color.White;
            var Coordinates = this.PointToClient(System.Windows.Forms.Cursor.Position);
            this.Left = Coordinates.X - _formSize / 2;
            this.Top = Coordinates.Y - _formSize / 2;
            _backgroundColor = new SolidBrush(System.Drawing.Color.FromArgb(_rnd.Next(256), _rnd.Next(256), _rnd.Next(256)));

            _liveTimer = new Timer();
            _liveTimer.Interval = _liveTime;
            _liveTimer.Tick += new EventHandler(LiveTimerTick);

            _moveTimer = new Timer();
            _moveTimer.Interval = moveTimerInterval;
            _moveTimer.Tick += new EventHandler(MoveTimerTick);

            _angleWhereMove = _rnd.Next(degrees + 1) / 180.0 * Math.PI; // from degrees to radians
            _polygonEdges = _rnd.Next(minEdges, circleNm + 1);
            _speed = _rnd.Next(minSpeed, maxSpeed + 1);
            _xMove = (int)(Math.Cos(_angleWhereMove) * _speed);
            _yMove = (int)(Math.Sin(_angleWhereMove) * _speed);

            _liveTimer.Start();
            _moveTimer.Start();
        }

        public void DrawPoligon(PaintEventArgs e)
        {
            if (circleNm == _polygonEdges)
            {
                Rectangle rectangle = new Rectangle(20, 20, 80, 80);
                e.Graphics.FillEllipse(_backgroundColor, rectangle);
            }
            else
            {
                double step = 2 * Math.PI / _polygonEdges;
                Point[] points = new Point[this._polygonEdges];
                for (int i = 0; i < _polygonEdges; i++)
                {
                    double angle = i * step;
                    int x = a + (int)(r * Math.Cos(angle + _angleNow));
                    int y = b + (int)(r * Math.Sin(angle + _angleNow));
                    points[i] = new Point(x, y);
                }
                e.Graphics.FillPolygon(_backgroundColor, points);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            DrawPoligon(e);
            //base.OnPaint(e);
        }

        void LiveTimerTick(Object myObject, EventArgs myEventArgs)
        {
            _liveTimer.Stop();
            if (_parent.Forms.Count < 2)
                _parent.Close();
            this.Close();
            _parent.Forms.Remove(this);
        }

        void MoveTimerTick(Object myObject, EventArgs myEventArgs)
        {
            _angleNow += angleStep;
            this.Invalidate();
            if (this.Left <= 0 || this.Right > screenWidth)
                _xMove = -_xMove;
            if (this.Top <= 0 || this.Bottom > screenHeight)
                _yMove = -_yMove;
            this.Left += _xMove;
            this.Top += _yMove;
            if (this.Opacity == 0 || this.Opacity == 1)
                _opacityStep = -_opacityStep;
            this.Opacity += _opacityStep;
        }
    }
}
