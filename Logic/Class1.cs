using Data;

namespace Logic
{
    public class MessageService
    {
        private readonly Message _message;

        public MessageService()
        {
            _message = new Message();
        }

        public string GetHelloMessage()
        {
            return _message.GetMessage();
        }
    }
}
