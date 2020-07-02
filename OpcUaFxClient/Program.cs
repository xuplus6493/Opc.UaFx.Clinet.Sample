using System;
using Opc.UaFx.Client;

namespace OpcUaFxClient
{
    class Program
    {
        private static void HandleDataChanged(object sender,OpcDataChangeReceivedEventArgs e)
        {
            OpcMonitoredItem item = (OpcMonitoredItem)sender;
            Console.WriteLine( "Data Change from NodeId '{0}': {1}",item.NodeId,e.Item.Value);
        }


        static void Main(string[] args)
        {
            var client = new OpcClient("opc.tcp://127.0.0.1:49320");
            client.Security.UserIdentity = new OpcClientIdentity("Tim", "1qaz2wsx3edc4rfv");

            client.Connect();


            // 布林Tag 讀寫
            client.WriteNode("ns=2;s=Channel2.Device1.Tag1", false);
            Console.WriteLine("ReadNode: {0}", client.ReadNode("ns=2;s=Channel2.Device1.Tag1"));
            client.WriteNode("ns=2;s=Channel2.Device1.Tag1", true);
            Console.WriteLine("ReadNode: {0}", client.ReadNode("ns=2;s=Channel2.Device1.Tag1"));

            // 字串Tag 讀寫
            client.WriteNode("ns=2;s=Channel2.Device1.Tag2", "Test");
            Console.WriteLine("ReadNode: {0}", client.ReadNode("ns=2;s=Channel2.Device1.Tag2"));
            client.WriteNode("ns=2;s=Channel2.Device1.Tag2", "Test...");
            Console.WriteLine("ReadNode: {0}", client.ReadNode("ns=2;s=Channel2.Device1.Tag2"));

            // float Tag 讀寫
            client.WriteNode("ns=2;s=Channel2.Device1.Tag3", 8.7);
            Console.WriteLine("ReadNode: {0}", client.ReadNode("ns=2;s=Channel2.Device1.Tag3"));
            client.WriteNode("ns=2;s=Channel2.Device1.Tag3", 9.2);
            Console.WriteLine("ReadNode: {0}", client.ReadNode("ns=2;s=Channel2.Device1.Tag3"));

            // word Tag 讀寫
            client.WriteNode("ns=2;s=Channel2.Device1.Tag4", (ushort)88);
            Console.WriteLine("ReadNode: {0}", client.ReadNode("ns=2;s=Channel2.Device1.Tag4"));
            client.WriteNode("ns=2;s=Channel2.Device1.Tag4", (ushort)33);
            Console.WriteLine("ReadNode: {0}", client.ReadNode("ns=2;s=Channel2.Device1.Tag4"));


            // 訂閱Tag5
            OpcSubscription subscription = client.SubscribeDataChange("ns=2;s=Channel2.Device1.Tag5", HandleDataChanged);
            subscription.PublishingInterval = 1000;
            subscription.ApplyChanges();


            Console.ReadLine();

            client.Disconnect();
        }
    }
}
