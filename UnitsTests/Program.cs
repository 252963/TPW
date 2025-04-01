using Logic;
using Xunit;

namespace UnitsTests
{
    public class MessageServiceTests
    {
        [Fact]
        public void GetHelloMessage_ReturnsCorrectMessage()
        {
            var service = new MessageService();

            string result = service.GetHelloMessage();

            Assert.Equal("Hello, World", result);
        }
    }
}

