Ball ball = new Ball(10, 10, 1);
Container container = new Container(0, 0, 40, 40);
container.Ball = ball;

while (true)
{
    Console.WriteLine(container + "\n");
    container.Move(ball, ball.Radius * 4);
    Thread.Sleep(50);
}

public class Ball
{
    public Ball(int x, int y, int radius = 1)
    {
        X = x;
        Y = y;
        Radius = radius;
    }

    public int X { get; set; }
    public int Y { get; set; }
    public int Radius { get; set; }
}

public class Container
{
    public Container(int x1, int y1, int x2, int y2)
    {
        this.x1 = x1;
        this.y1 = y1;
        this.x2 = x2;
        this.y2 = y2;
    }

    public int x1;
    public int y1;
    public int x2;
    public int y2;

    public Ball Ball { get; set; } = new Ball(1, 1, 1);

    private void MoveRight(int step)
    {
        this.Ball.X += step;
    }

    private void MoveUp(int step)
    {
        this.Ball.Y += step;
    }

    private void MoveDown(int step)
    {
        this.Ball.Y -= step;
    }

    private void MoveLeft(int step)
    {
        this.Ball.X -= step;
    }

    private bool ColidesWithY(Ball ball, int step)
    {
        this.Ball.Y += step;
        if (this.Ball.Y > y2 - 1)
        {
            this.Ball.Y -= step;
            return true;
        }
        else
        {
            this.Ball.Y -= step;
        }

        this.Ball.Y -= step;
        if (this.Ball.Y < y1 + 1)
        {
            this.Ball.Y += step;
            return true;
        }
        else
        {
            this.Ball.Y += step;
            return false;
        }
    }
    private bool ColidesWithX(Ball ball, int step)
    {
        this.Ball.X += step;
        if (this.Ball.X > x2 - 1)
        {
            this.Ball.X -= step;
            return true;
        }
        else
        {
            this.Ball.X -= step;
        }
        this.Ball.X -= step;
        if (this.Ball.X < x1 + 1)
        {
            this.Ball.X += step;
            return true;
        }
        else
        {
            this.Ball.X += step;
            return false;
        }
    }

    private bool ColidesWithUpX(Ball ball, int step)
    {
        this.Ball.Y += step;
        if (this.Ball.Y > y2 - 1)
        {
            this.Ball.Y -= step;
            return true;
        }
        else
        {
            this.Ball.Y -= step;
            return false;
        }
    }

    private bool ColidesWithDownX(Ball ball, int step)
    {
        this.Ball.Y -= step;
        if (this.Ball.Y < y1 + 1)
        {
            this.Ball.Y += step;
            return true;
        }
        else
        {
            this.Ball.Y += step;
            return false;
        }
    }

    private bool ColidesWithRightY(Ball ball, int step)
    {
        this.Ball.X += step;
        if (this.Ball.X > x2 - 1)
        {
            this.Ball.X -= step;
            return true;
        }
        else
        {
            this.Ball.X -= step;
            return false;
        }
    }

    private bool ColidesWithLeftY(Ball ball, int step)
    {
        this.Ball.X -= step;
        if (this.Ball.X < x1 + 1)
        {
            this.Ball.X += step;
            return true;
        }
        else
        {
            this.Ball.X += step;
            return false;
        }
    }

    public void Move(Ball ball, int step)
    {
        if ((this.ColidesWithX(ball, step) == false) && (this.ColidesWithY(ball, step) == false))
        {
            int result = new Random().Next(1, 5);
            switch (result)
            {
                case 1:
                    this.MoveUp(step);
                    break;
                case 2:
                    this.MoveRight(step);
                    break;
                case 3:
                    this.MoveDown(step);
                    break;
                case 4:
                    this.MoveLeft(step);
                    break;
            }

        }
        else if (this.ColidesWithX(ball, step) && this.ColidesWithY(ball, step) == false)
        {
            if (this.ColidesWithUpX(ball, step))
            {
                this.MoveDown(step);
            }
            else // ColidesWithDownX
            {
                this.MoveUp(step);
            }
        }
        else if (this.ColidesWithY(ball, step) && this.ColidesWithX(ball, step) == false)
        {
            if (this.ColidesWithRightY(ball, step))
            {
                this.MoveLeft(step);
            }
            else // ColidewsWithLeftY
            {
                this.MoveRight(step);
            }
        }
        else // Якшо колайдиться з Upx and RightY or Upx and LeftY or LeftY and DownX or DownX and RightY
        {
            if (this.ColidesWithUpX(ball, step) && this.ColidesWithRightY(ball, step))
            {
                this.MoveLeft(step);
                this.MoveDown(step);
            }
            else if (this.ColidesWithUpX(ball, step) && this.ColidesWithLeftY(ball, step))
            {
                this.MoveRight(step);
                this.MoveDown(step);
            }
            else if (this.ColidesWithDownX(ball, step) && this.ColidesWithLeftY(ball, step))
            {
                this.MoveUp(step);
                this.MoveRight(step);
            }
            else // DownX and RightY
            {
                this.MoveUp(step);
                this.MoveLeft(step);
            }
        }
    }
    public override string ToString()
    {
        return $"Ball: X - {this.Ball.X}, Y - {this.Ball.Y}\nContainer: X1 - {this.x1}, Y1 - {this.y1} | X2 - {this.x2}, Y2 - {this.y2}";
    }
}