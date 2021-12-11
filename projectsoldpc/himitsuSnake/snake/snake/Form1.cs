using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
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
        public Point _Cabeca = new Point(3, 0);
        public Point _Cauda = new Point(0,0);
        public int _snakeSize;
        public Point _Food;
        public Sentido _sentidoAtual = Sentido.Direita;
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
        private object _lockSetSentido = new object();
        private bool _firstTime = true;
        private string path = @"C:\Users\hidde\Desktop\bla\ble\";
        private string file;

        internal void SetSentido(Sentido sent)
        {
            //Directory.CreateDirectory(path);
            //using (StreamWriter outputFile = new StreamWriter(path + file))
            //{
            //    outputFile.WriteLine(sent.ToString());
            //}
            lock (_lockSetSentido)
            {
                if (!_mudou)
                {
                    if (sent == Sentido.Cima)
                    {
                        //segue reto
                    }
                    else if(sent == Sentido.Direita)
                    {
                        if(_sentidoAtual == Sentido.Baixo)
                        {
                            _sentidoAtual = Sentido.Esquerda;
                            _mudou = true;
                        }
                        else if(_sentidoAtual == Sentido.Esquerda)
                        {
                            _sentidoAtual = Sentido.Cima;
                            _mudou = true;
                        }
                        else if(_sentidoAtual == Sentido.Cima)
                        {
                            _sentidoAtual = Sentido.Direita;
                            _mudou = true;
                        }
                        else if(_sentidoAtual == Sentido.Direita)
                        {
                            _sentidoAtual = Sentido.Baixo;
                            _mudou = true;
                        }
                    }
                    else if (sent == Sentido.Esquerda)
                    {
                        if (_sentidoAtual == Sentido.Baixo)
                        {
                            _sentidoAtual = Sentido.Direita;
                            _mudou = true;
                        }
                        else if (_sentidoAtual == Sentido.Esquerda)
                        {
                            _sentidoAtual = Sentido.Baixo;
                            _mudou = true;
                        }
                        else if (_sentidoAtual == Sentido.Cima)
                        {
                            _sentidoAtual = Sentido.Esquerda;
                            _mudou = true;
                        }
                        else if (_sentidoAtual == Sentido.Direita)
                        {
                            _sentidoAtual = Sentido.Cima;
                            _mudou = true;
                        }
                    }
                    ////capture up arrow key
                    //if (_sentidoAtual != sent)
                    //{
                    //    if (sent == Sentido.Baixo && _sentidoAtual != Sentido.Cima)
                    //    {
                    //        _sentidoAtual = sent;
                    //        _mudou = true;
                    //    }
                    //    if (sent == Sentido.Cima && _sentidoAtual != Sentido.Baixo)
                    //    {
                    //        _sentidoAtual = sent;
                    //        _mudou = true;
                    //    }
                    //    if (sent == Sentido.Esquerda && _sentidoAtual != Sentido.Direita)
                    //    {
                    //        _sentidoAtual = sent;
                    //        _mudou = true;
                    //    }
                    //    if (sent == Sentido.Direita && _sentidoAtual != Sentido.Esquerda)
                    //    {
                    //        _sentidoAtual = sent;
                    //        _mudou = true;

                    //    }
                    //}
                }
                _waitigAI = false;
            }
        }

        private bool _show;
        private Thread _gameCycle;
        private int _pontuacao;
        private double _distancia = 0;
        private bool _waitigAI = true;
        private bool _firstFood = true;

        public Form1(bool show, bool startCycle)
        {
            _show = show;
            InitializeComponent();
            //this.Height = _height * _squareSize + _squareSize;
            //this.Width = _width * _squareSize + _squareSize;
            this.AutoSize = true;
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            this.pnGame.Width = _width * _squareSize + 2*_squareSize;
            this.pnGame.Height = _height * _squareSize + 2*_squareSize;
            this.pnF.Width = _width * _squareSize + 2 * _squareSize;
            this.pnF.Height = _height * _squareSize + 2 * _squareSize;
            _pontuacao = 0;
            DateTime agora = DateTime.Now;

            file = agora.ToString("yyyyMMddHHmmssfff") + ".txt";
            Start(startCycle);
        }
        public void Start(bool startCycle)
        {
            _sentidoAtual = Sentido.Direita;
            PreencheUnidades();
            PrintAllBlank();
            IniciaSnake();
            NewFood();
            AfastouDaComida();

            pnGame.Invalidate(false);
            //StartControlCycleAsync();
            if (startCycle)
            {
                StartGameCycleAsync();
            }
        }
        private void StartForHimitsu(bool show)
        {
            _sentidoAtual = Sentido.Direita;
            PreencheUnidades();
            PrintAllBlank();
            IniciaSnake();
            NewFood();

            _show = show;
            if(_show)
                pnGame.Invalidate(false);
            //StartControlCycleAsync();
            StartGameCycleAsync();
        }
        private void StartControlCycleAsync()
        {
            Thread controlCycle = new Thread(ControlCycle);

            controlCycle.Start();
        }
        public bool IsPaused
        {
            get { return _paused; }
        }
        internal double[] GetCurrentInputs()
        {
            while(_mudou && !_finished)
            {
                Thread.Sleep(10);
            }
            if(_finished)
            {
                return null;
            }
            List<double> inputs = new List<double>();
            double[] envoltoProibido = VerificaSeEnvoltaEhProibido();
            double[] direcaoComida = VerificaDirecaoComida();
            //double[] sentidoAtual = VerificaSentidoAtual();

            inputs.AddRange(envoltoProibido);
            inputs.AddRange(direcaoComida);
            //inputs.AddRange(sentidoAtual);

            return inputs.ToArray();
        }

        private double[] VerificaSentidoAtual()
        {
            List<double> sentido = new List<double>();

            double cima = 0;
            double baixo = 0;
            double direita = 0;
            double esquerda = 0;

            try
            {
                if(_sentidoAtual == Sentido.Baixo)
                {
                    baixo = 1;
                }
                if (_sentidoAtual == Sentido.Cima)
                {
                    cima = 1;
                }
                if (_sentidoAtual == Sentido.Direita)
                {
                    direita = 1;
                }
                if (_sentidoAtual == Sentido.Esquerda)
                {
                    esquerda = 1;
                }
            }
            catch(Exception ex)
            {

            }
            sentido.Add(esquerda);
            sentido.Add(direita);
            sentido.Add(baixo);
            sentido.Add(cima);

            return sentido.ToArray();
        }

        private double[] VerificaDirecaoComida()
        {
            List<double> food = new List<double>();

            double cima = 0;
            double baixo = 0;
            double direita = 0;
            double esquerda = 0;

            try
            {
                int vertical = _Cabeca.Y - _Food.Y;//y maior mais pra baixo
                int horizontal = _Cabeca.X - _Food.X;//x maior mais pra direita

                if((horizontal == 0 && ((vertical > 0 && _sentidoAtual == Sentido.Cima) || (vertical < 0 && _sentidoAtual == Sentido.Baixo)))
                    ||
                    (vertical == 0 && ((horizontal > 0 && _sentidoAtual == Sentido.Esquerda) || (horizontal < 0 && _sentidoAtual == Sentido.Direita))))
                {
                   
                   
                            cima = 1;
                       
                }
                else if(vertical == 0 || horizontal == 0)
                {
                    baixo = 1;
                }
                else
                if (_sentidoAtual == Sentido.Cima)
                {
                    
                    if(horizontal > 0)
                    {
                        esquerda = 1;
                    }
                    if(horizontal < 0)
                    {
                        direita = 1;
                    }

                }
                else if (_sentidoAtual == Sentido.Baixo)
                {

                    if (horizontal > 0)
                    {
                        direita = 1;
                    }
                    if (horizontal < 0)
                    {
                        esquerda = 1;
                    }

                }
                else if (_sentidoAtual == Sentido.Direita)
                {

                    if (vertical > 0)
                    {
                        esquerda = 1;
                    }
                    if (vertical < 0)
                    {
                        direita = 1;
                    }

                }
                else if (_sentidoAtual == Sentido.Esquerda)
                {

                    if (vertical > 0)
                    {
                        direita = 1;
                    }
                    if (vertical < 0)
                    {
                        esquerda = 1;
                    }

                }
            }

            catch (Exception ex)
            {

            }
            food.Add(esquerda);
            food.Add(direita);
            food.Add(baixo);
            food.Add(cima);

            return food.ToArray();
        }
        

        private double[] VerificaSeEnvoltaEhProibido()
        {
            List<double> envolto = new List<double>();
            double direita = 0;
            double esquerda = 0;
            double cima = 0;
            //double baixo = 0;
            try
            {
                //direita
                if (_Cabeca.X + 1 >= _width || _unidades[_Cabeca.X + 1][_Cabeca.Y].IsBody)
                {
                    if (_sentidoAtual == Sentido.Cima)
                    {
                        direita = 1;
                    }
                    else if(_sentidoAtual == Sentido.Direita)
                    {
                        cima = 1;
                    }
                    else if (_sentidoAtual == Sentido.Baixo)
                    {
                        esquerda = 1;
                    }
                    else if (_sentidoAtual == Sentido.Esquerda)
                    {
                        
                    }
                }
                //esquerda
                if (_Cabeca.X - 1 < 0 || _unidades[_Cabeca.X - 1][_Cabeca.Y].IsBody)
                {
                    if (_sentidoAtual == Sentido.Cima)
                    {
                        esquerda = 1;
                    }
                    else if (_sentidoAtual == Sentido.Direita)
                    {
                        
                    }
                    else if (_sentidoAtual == Sentido.Baixo)
                    {
                        direita = 1;
                    }
                    else if (_sentidoAtual == Sentido.Esquerda)
                    {
                        cima = 1;
                    }
                }
                //baixo
                if (_Cabeca.Y + 1 > _height || _unidades[_Cabeca.X][_Cabeca.Y + 1].IsBody)
                {
                    if (_sentidoAtual == Sentido.Cima)
                    {
                        
                    }
                    else if (_sentidoAtual == Sentido.Direita)
                    {
                        direita = 1;
                    }
                    else if (_sentidoAtual == Sentido.Baixo)
                    {
                        cima = 1;
                    }
                    else if (_sentidoAtual == Sentido.Esquerda)
                    {
                        esquerda = 1;
                    }
                }
                //cima
                if (_Cabeca.Y - 1 < 0 || _unidades[_Cabeca.X][_Cabeca.Y - 1].IsBody)
                {
                    if (_sentidoAtual == Sentido.Cima)
                    {
                        cima = 1;
                    }
                    else if (_sentidoAtual == Sentido.Direita)
                    {
                        esquerda = 1;
                    }
                    else if (_sentidoAtual == Sentido.Baixo)
                    {
                        
                    }
                    else if (_sentidoAtual == Sentido.Esquerda)
                    {
                        direita = 1;
                    }
                }
            }
            catch(Exception ex)
            {

            }
            envolto.Add(direita);
            envolto.Add(esquerda);
            //envolto.Add(baixo);
            envolto.Add(cima);

            return envolto.ToArray();
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
            //if(_gameCycle != null && _gameCycle.IsAlive)
            //{
            //    _gameCycle.Abort();
            //}
            _gameCycle = new Thread(GameCycle);

            _gameCycle.Name = "Game Cycle";
            _gameCycle.Start();
        }

        private void GameCycle()
        {
            try
            {
                _finished = false;
                while (!_finished)
                {
                    if (_show && _paused)
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
                        if (_show)
                        {
                            this.pnGame.Invalidate(false);
                            while (_onPrint)
                            {
                                Thread.Sleep(10);
                            }
                        }
                        _mudou = false;
                        if (_show)
                            Thread.Sleep(_delay);
                        else
                        {
                            WaitAIAction();
                        }
                    }
                }

                if (_show)
                {
                    DialogResult dialogResult = MessageBox.Show("Recomeçar?", "Fim", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        _paused = true;
                        Start(true);
                        return;
                    }
                    else if (dialogResult == DialogResult.No)
                    {
                        this.SynchronizedInvoke(() => this.Close());
                        if (_gameCycle != null && _gameCycle.IsAlive)
                        {
                            _gameCycle.Abort();
                        }
                    }
                }
            }
            catch(Exception ex)
            {

            }
        }

        private void WaitAIAction()
        {
            while(_waitigAI)
            {
                Thread.Sleep(10);
            }
            _waitigAI = true;
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
                FinishAll();
                if(_show)
                    MessageBox.Show("You Lost!!");
                return;
            }

            _unidades[_Cabeca.X][_Cabeca.Y].IsBody = true;
            _unidades[_Cabeca.X][_Cabeca.Y].Order = _count;
            _BodyFIFO.Add(_unidades[_Cabeca.X][_Cabeca.Y]);

            if(_unidades[_Cabeca.X][_Cabeca.Y].IsFood)
            {
                _unidades[_Cabeca.X][_Cabeca.Y].IsFood = false;
                _snakeSize++;
                _pontuacao = _pontuacao + 500;
                NewFood();
            }
            else
            {
                AjustaPontuacao();
                RemoveUltimo();
            }

            _count++;

            if(_count == int.MaxValue)
            {
                ResetaContagem();
            }
        }

        private void AjustaPontuacao()
        {
            if (AfastouDaComida())
            {
                _pontuacao = _pontuacao - 10;
                if (Pontuacao < -40)
                { FinishAll(); }
            }
            else
            {
                _pontuacao = _pontuacao + 1;
            }
        }

        private bool AfastouDaComida()
        {
            double distancia = Math.Sqrt(Math.Pow(_Food.X - _Cabeca.X, 2) + Math.Pow(_Food.Y - _Cabeca.Y, 2));

            if(distancia < _distancia)
            {
                _distancia = distancia;
                return false;
            }
            _distancia = distancia;

            return true;
        }
        public double Pontuacao
        {
            get { return _pontuacao; }
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
                if(_firstFood)
                {
                    rdmX = 5;
                    rdmY = 5;
                    _firstFood = false;
                }

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
            _snakeSize = _Cabeca.X - _Cauda.X;
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
            _finished = true;
            if (_show)
            {
                _gGame.Dispose();
            }
            else
            {
                //if (_gameCycle != null && _gameCycle.IsAlive)
                //{
                //    _gameCycle.Abort();
                //}
            }
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
