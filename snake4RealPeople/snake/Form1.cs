using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace snake
{

    public partial class Form1 : Form, ISynchronizeInvoke
    {
        int _count = 0;
        int _height = 30;
        int _width = 20;
        int _squareSize = 20;
        Unidade[][] _unidades;
        Point _Cabeca = new Point(3, 0);
        Point _Cauda = new Point(0,0);
        Point _Food;
        Sentido _sentidoAtual = Sentido.Direita;
        SolidBrush _solidBrushFill = new SolidBrush(Color.FromArgb(255, 0, 0, 0));
        SolidBrush _solidBrushEmpty = new SolidBrush(Color.FromArgb(255, Color.White));
        bool _finished = false;
        bool _mudou = false;
        int _delay = 200;
        private bool _onPrint;
        Graphics _gGame;
        Graphics _gFundo;
        List<Unidade> _BodyFIFO = new List<Unidade>();
        private bool _paused = true;
        bool _accelerating = false;
        private object _lockChange = new object();
        private bool _firstTime = true;
        private bool _working = true;

        public Form1()
        {
            InitializeComponent();
            //this.Height = _height * _squareSize + _squareSize;
            //this.Width = _width * _squareSize + _squareSize;
            this.AutoSize = true;
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            this.pnGame.Width = _width * _squareSize + 2*_squareSize;
            this.pnGame.Height = _height * _squareSize + 2*_squareSize;
            this.pnF.Width = _width * _squareSize + 2 * _squareSize;
            this.pnF.Height = _height * _squareSize + 2 * _squareSize;

            Start();
        }
        private void Start()
        {
            _sentidoAtual = Sentido.Direita;
            PreencheUnidades();
            PrintAllBlank();
            IniciaSnake();
            NewFood();


            pnGame.Invalidate(false);
            //StartControlCycleAsync();
            StartGameCycleAsync();
        }
        private void StartControlCycleAsync()
        {
            Thread controlCycle = new Thread(ControlCycle);

            controlCycle.Start();
        }

        private void ControlCycle()
        {
            if(!_mudou)
            {

            }
            _mudou = true;
        }

        private void StartGameCycleAsync()
        {
            Thread gameCycle = new Thread(GameCycle);

            gameCycle.Start();
        }

        private void GameCycle()
        {
            _finished = false;
            while (!_finished)
            {
                if (_paused)
                {
                    EnterPauseLoop();
                }
                lock (_lockChange)
                {
                    if (_sentidoAtual == Sentido.Baixo)
                    {
                        _Cabeca = new Point(_Cabeca.X, _Cabeca.Y + 1);
                    }
                    else if (_sentidoAtual == Sentido.Cima)
                    {
                        _Cabeca = new Point(_Cabeca.X, _Cabeca.Y - 1);
                    }
                    else if (_sentidoAtual == Sentido.Direita)
                    {
                        _Cabeca = new Point(_Cabeca.X + 1, _Cabeca.Y);
                    }
                    else if (_sentidoAtual == Sentido.Esquerda)
                    {
                        _Cabeca = new Point(_Cabeca.X - 1, _Cabeca.Y);
                    }

                    AnalisaColisao();
                    this.pnGame.Invalidate(false);
                    while (_onPrint)
                    {
                        Thread.Sleep(10);
                    }
                    _mudou = false;
                    Thread.Sleep(_delay);
                }
            }

            DialogResult dialogResult = MessageBox.Show("Recomeçar?", "Fim", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                _paused = true;
                Start();
                return;
            }
            else if (dialogResult == DialogResult.No)
            {
                this.SynchronizedInvoke(() => this.Close());
            }
        }

        private void EnterPauseLoop()
        {
            while (_paused)
            {
                Thread.Sleep(1000);
            }
        }

        private void AnalisaColisao()
        {
            if(_Cabeca.X < 0 ||
               _Cabeca.X >= _width ||
               _Cabeca.Y < 0 ||
               _Cabeca.Y >= _height ||
               _unidades[_Cabeca.X][_Cabeca.Y].IsBody)
            {
                MessageBox.Show("You Lost!!");
                FinishAll();
                return;
            }

            _unidades[_Cabeca.X][_Cabeca.Y].IsBody = true;
            _unidades[_Cabeca.X][_Cabeca.Y].Order = _count;
            _BodyFIFO.Add(_unidades[_Cabeca.X][_Cabeca.Y]);

            if(_unidades[_Cabeca.X][_Cabeca.Y].IsFood)
            {
                _unidades[_Cabeca.X][_Cabeca.Y].IsFood = false;

                NewFood();
            }
            else
            {
                RemoveUltimo();
            }

            _count++;

            if(_count == int.MaxValue)
            {
                ResetaContagem();
            }
        }

        private void ResetaContagem()
        {
            _count = 0;

            var ordened = _BodyFIFO.OrderBy(x => x.Order);
            foreach(Unidade un in ordened)
            {
                un.Order = _count;
                _count++;
            }
        }

        private void RemoveUltimo()
        {
            Unidade aSerRemovido = _BodyFIFO.OrderBy(x => x.Order).First();
            aSerRemovido.IsBody = false;
            aSerRemovido.NoMoreBody = true;

            _BodyFIFO.RemoveAll(x => x.Order == aSerRemovido.Order);
        }

        private void PrintNewState()
        {
            List<Point> recs = new List<Point>();
            for(int i = 0; i < _width; i++)
            {
                for(int j = 0; j < _height; j ++)
                {
                    if(_unidades[i][j].IsBody || _unidades[i][j].IsFood)
                    {
                        recs.Add(new Point(i, j));
                    }
                    else if(_unidades[i][j].NoMoreBody)
                    {
                        PrintSquareEmpty(i, j);
                        _unidades[i][j].NoMoreBody = false;
                    }
                }
            }
            if(recs.Count > 0)
            {
                PrintSquareFill(recs);
            }

        }

        private void PrintAllBlank()
        {
            for (int i = 0; i < _width; i++)
            {
                for (int j = 0; j < _height; j++)
                {

                    _unidades[i][j].NoMoreBody = true;

                }
            }
        }
        private void PrintSquareFill(List<Point> points)
        {
            List<Rectangle> recs = new List<Rectangle>();
            foreach (Point p in points)
            {
                Rectangle rec = new Rectangle(p.X * _squareSize + 1, p.Y * _squareSize + 1, _squareSize - 1, _squareSize - 2);
                recs.Add(rec);
            }
            _gGame.FillRectangles(_solidBrushFill, recs.ToArray());
        }

        private void PrintSquareEmpty(int i, int j)
        {

            _gGame.FillRectangle(_solidBrushEmpty, i * _squareSize + 1, j * _squareSize + 1, _squareSize - 1, _squareSize -2);
        }

        private void NewFood()
        {
            Random gen = new Random();

            bool success = false;

            while (!success)
            {
                int rdmX = gen.Next(0, _width - 1);
                int rdmY = gen.Next(0, _height - 1);

                if(!_unidades[rdmX][rdmY].IsBody)
                {
                    _unidades[rdmX][rdmY].IsFood = true;
                    _unidades[rdmX][rdmY].NoMoreBody = false;
                    _Food = new Point(rdmX, rdmY);
                    success = true;
                }
            }
        }

        private void IniciaSnake()
        {
            _Cabeca = new Point(3, 0);
            _Cauda = new Point(0, 0);
            _BodyFIFO = new List<Unidade>();
            for (int i = _Cauda.X; i <= _Cabeca.X; i++)
            {
                _unidades[i][0].IsBody = true;
                _unidades[i][0].Order = _count;
                _unidades[i][0].NoMoreBody = false;
                _BodyFIFO.Add(_unidades[i][0]);
                _count++;
            }
        }

        private void PreencheUnidades()
        {
            _unidades = new Unidade[_width][];

            for(int i = 0; i < _width; i++)
            {
                _unidades[i] = new Unidade[_height];
                for(int j = 0; j < _height; j ++)
                {
                    _unidades[i][j] = new Unidade();

                    _unidades[i][j].X = i;
                    _unidades[i][j].Y = j;
                }
            }
        }

        private void PrintBoard()
        {
            System.Drawing.Pen myPen = new System.Drawing.Pen(System.Drawing.Color.Red);
            

            for(int i = 0; i < _width; i++)
            {
                for(int j = 0; j < _height; j++)
                {
                    _gFundo.DrawRectangle(myPen, i * _squareSize, j * _squareSize, _squareSize, _squareSize);
;                }
            }

            //pnGame.AutoSize = true;
            //pnGame.AutoSizeMode = AutoSizeMode.GrowAndShrink;
           
            myPen.Dispose();
        }

        private void FinishAll()
        {
            _gGame.Dispose();
            _finished = true;
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Enter)
            {
                _paused = !_paused;
            }
            else if((keyData & Keys.Shift) == Keys.Shift)
            {
                if(_accelerating)
                {
                    _delay = _delay * 4;
                }
                else
                {
                    _delay = _delay / 4;
                }
                _accelerating = !_accelerating;
            }
            lock (_lockChange)
            {
                if (!_mudou)
                {

                    //capture up arrow key
                    if (keyData == Keys.Up)
                    {
                        if (_sentidoAtual == Sentido.Baixo || _sentidoAtual == Sentido.Cima)
                        {
                            return true;
                        }
                        _sentidoAtual = Sentido.Cima;
                    }
                    //capture down arrow key
                    if (keyData == Keys.Down)
                    {
                        if (_sentidoAtual == Sentido.Baixo || _sentidoAtual == Sentido.Cima)
                        {
                            return true;
                        }
                        _sentidoAtual = Sentido.Baixo;
                    }
                    //capture left arrow key
                    if (keyData == Keys.Left)
                    {
                        if (_sentidoAtual == Sentido.Esquerda || _sentidoAtual == Sentido.Direita)
                        {
                            return true;
                        }
                        _sentidoAtual = Sentido.Esquerda;
                    }
                    //capture right arrow key
                    if (keyData == Keys.Right)
                    {
                        if (_sentidoAtual == Sentido.Esquerda || _sentidoAtual == Sentido.Direita)
                        {
                            return true;
                        }
                        _sentidoAtual = Sentido.Direita;
                    }
                    _mudou = true;
                }
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }




     
     
           

        private void PnGame_Paint(object sender, PaintEventArgs e)
        {
            _onPrint = true;
            _gGame = e.Graphics;


            //_gGame.FillRectangle(_solidBrushEmpty, pnGame.ClientRectangle);
            PrintNewState();

            _onPrint = false;
            
        }

       

        private void PnF_Paint(object sender, PaintEventArgs e)
        {
            if (_firstTime)
            {
                _gFundo = e.Graphics;
                PrintBoard();
                _firstTime = false;
            }
        }
        //protected override bool IsInputKey(Keys keyData)
        //{
        //    switch (keyData)
        //    {
        //        case Keys.Right:
        //        case Keys.Left:
        //        case Keys.Up:
        //        case Keys.Down:
        //            return true;
        //        case Keys.Shift | Keys.Right:
        //        case Keys.Shift | Keys.Left:
        //        case Keys.Shift | Keys.Up:
        //        case Keys.Shift | Keys.Down:
        //            return true;
        //    }
        //    return base.IsInputKey(keyData);
        //}
        //protected override void OnKeyDown(KeyEventArgs e)
        //{
        //    base.OnKeyDown(e);
        //    switch (e.KeyCode)
        //    {
        //        case Keys.Left:
        //        case Keys.Right:
        //        case Keys.Up:
        //        case Keys.Down:
        //            if (e.Shift)
        //            {

        //            }
        //            else
        //            {
        //            }
        //            break;
        //    }
        //}
    }
}
