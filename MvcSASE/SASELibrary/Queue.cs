using System;
namespace SASELibrary
{
    public abstract class Queue
    {
        public abstract bool CreateQueue(string name);
        public abstract string DequeueMessage(string name);
        public abstract bool EnqueueMessage(string name, string message);
        public abstract int GetMessageCount(string name);
        public abstract System.Collections.Generic.List<string> GetQueueNames();
        public abstract System.Collections.Generic.List<string> PeekMessage(string name);
    }
}
