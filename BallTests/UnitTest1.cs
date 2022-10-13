using System.Collections;
namespace BallTests
{
    public class ContainerAndBallTests
    {
        [Theory]
        [ClassData(typeof(BallDataClass))]
        public void BallXandYNotEqualdCoordsEdges(Ball ball)
        {
            Container container = new Container(0, 0, 10, 10);
            container.Ball = ball;

            double expected = 0;
            double expected2 = 10;


            container.Move(ball, ball.Radius * 4);

            double actualX = ball.X;
            double actualY = ball.Y;

            Assert.NotEqual(expected, actualX);
            Assert.NotEqual(expected, actualY);
            Assert.NotEqual(expected2, actualX);
            Assert.NotEqual(expected2, actualY);
        }

        [Theory]
        [ClassData(typeof(ContainerAndBallDataClass))]
        public void ContainerSquareAndBallCoordsEdgesTest(Container container, Ball ball)
        {
            container.Ball = ball;

            double expected1 = container.x1;
            double expected2 = container.x2;

            container.Move(ball, ball.Radius * 4);

            double actualX = ball.X;
            double actualY = ball.Y;

            Assert.NotEqual(expected1, actualX);
            Assert.NotEqual(expected1, actualY);
            Assert.NotEqual(expected2, actualX);
            Assert.NotEqual(expected2, actualY);
        }

        public class BallDataClass : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[]
                {
                    new Ball(1, 1)
                };
                yield return new object[]
                {
                    new Ball(2, 2)
                };
                yield return new object[]
                {
                    new Ball(3, 3)
                };
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }

        public class ContainerAndBallDataClass : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[]
                {
                    new Container(0,0, 10, 10),
                    new Ball(1, 1, 1)
                };

                yield return new object[]
                {
                    new Container(1,1, 9, 9),
                    new Ball(2,2, 1)
                };

                yield return new object[]
                {
                    new Container(0, 0, 20, 20),
                    new Ball(18, 18)
                };
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
    }
}