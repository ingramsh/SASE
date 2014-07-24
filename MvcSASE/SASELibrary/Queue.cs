using System;
namespace SASELibrary
{
    interface Queue
    {
        bool CreateQueue(string name);
        string DequeueMessage(string name);
        bool EnqueueMessage(string name, string message);
        int GetMessageCount(string name);
        System.Collections.Generic.List<string> GetQueueNames();
        System.Collections.Generic.List<string> PeekMessage(string name);
    }
}
