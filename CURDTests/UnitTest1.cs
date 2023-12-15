using Xunit;

namespace CURDTests

{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            //Arrange(Упорядкувати)
            MyMath act = new MyMath();
            int a = 1, b = 20;
            int expected = 21;

            
            //Act(дія)
            int actual = act.Add(a, b);
            
            
            //Assert(підтвердження)
            Assert.Equal(expected, actual);
            
        }
    }
}