using Xunit;

namespace CURDTests

{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            //Arrange(������������)
            MyMath act = new MyMath();
            int a = 1, b = 20;
            int expected = 21;

            
            //Act(��)
            int actual = act.Add(a, b);
            
            
            //Assert(������������)
            Assert.Equal(expected, actual);
            
        }
    }
}