using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Snake_Game
{
    public partial class Form1 : Form
    {
        private List<Rectangle> snake;
        private Rectangle food;
        private int direction;
        private int score;
        private Random random;

        public Form1()
        {
            InitializeComponent();
            InitializeGame();
        }

        //Создание змеи и замуск игры
        private void InitializeGame()
        {
            snake = new List<Rectangle>();
            snake.Add(new Rectangle(50, 50, 10, 10)); 
            direction = 0; 
            score = 0;
            random = new Random();
            GenerateFood();
            timer1.Start();
        }

        //Генерация еды в рамках игрового поля
        private void GenerateFood()
        {
            int x = random.Next(0, this.ClientSize.Width / 10) * 10;
            int y = random.Next(0, this.ClientSize.Height / 10) * 10;
            food = new Rectangle(x, y, 10, 10);
        }

        //Изменение направления движения змеи путем нажатия стрелок
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            switch (e.KeyCode)
            {
                case Keys.Right:
                    if (direction != 2) direction = 0;
                    break;
                case Keys.Down:
                    if (direction != 3) direction = 1;
                    break;
                case Keys.Left:
                    if (direction != 0) direction = 2;
                    break;
                case Keys.Up:
                    if (direction != 1) direction = 3;
                    break;
            }
        }

        //"Игровой движок"
        private void timer1_Tick(object sender, EventArgs e)
        {
            MoveSnake();
            CheckCollision();
            Invalidate();
        }

        //"Оживление" змеи
        private void MoveSnake()
        {
            //Перемещеие
            Rectangle head = snake[0];
            switch (direction)
            {
                case 0: head.X += 10; break;
                case 1: head.Y += 10; break;
                case 2: head.X -= 10; break;
                case 3: head.Y -= 10; break;
            }

            //Рост змеи путем поедания еды
            snake.Insert(0, head);
            if (head.IntersectsWith(food))
            {
                score += 10;
                GenerateFood();
            }
            else
            {
                snake.RemoveAt(snake.Count - 1);
            }
        }

        //"Смерть" змейке
        private void CheckCollision()
        {
            Rectangle head = snake[0];

            //выход за игровое поле
            if (head.X < 0 || head.X >= this.ClientSize.Width || head.Y < 0 || head.Y >= this.ClientSize.Height)
            {
                GameOver();
            }

            //Поедание своего хвоста
            for (int i = 1; i < snake.Count; i++)
            {
                if (head.IntersectsWith(snake[i]))
                {
                    GameOver();
                }
            }
        }

        //Вывод сообщения "Окончание игры с числом съеденной еды"
        private void GameOver()
        {
            timer1.Stop();
            MessageBox.Show($"Game Over! Your score: {score}", "Game Over");
            InitializeGame();
        }

        //Графический движок
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            foreach (Rectangle segment in snake)
            {
                g.FillRectangle(Brushes.Green, segment);
            }
            g.FillRectangle(Brushes.Red, food);
            g.DrawString($"Score: {score}", this.Font, Brushes.Black, new PointF(10, 10));
        }
    }
}
