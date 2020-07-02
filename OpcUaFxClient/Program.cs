using System;
using Opc.UaFx.Client;
using Opc.UaFx;
using System.Collections.Generic;

namespace OpcUaFxClient
{
    class Program
    {
        private static void HandleDataChanged(object sender,OpcDataChangeReceivedEventArgs e)
        {
            OpcMonitoredItem item = (OpcMonitoredItem)sender;
            Console.WriteLine("DataChange: {0} = {1}", item.NodeId,e.Item.Value);
        }

        static void Main(string[] args)
        {
            var client = new OpcClient("opc.tcp://127.0.0.1:49320");
            client.Security.UserIdentity = new OpcClientIdentity("Tim", "1qaz2wsx3edc4rfv");

            client.Connect();


            // 一次寫取多個Tag
            OpcWriteNode[] wCommands = new OpcWriteNode[] {
                new OpcWriteNode("ns=2;s=Channel2.Device1.Tag1", false),    // 寫 boolean
                new OpcWriteNode("ns=2;s=Channel2.Device1.Tag2", "Test"),   // 寫 sting
                new OpcWriteNode("ns=2;s=Channel2.Device1.Tag3", 8.7),      // 寫 float
                new OpcWriteNode("ns=2;s=Channel2.Device1.Tag3", (ushort)88)// 寫 word
            };
            OpcStatusCollection results = client.WriteNodes(wCommands);

            // 一次讀取多個Tag
            OpcReadNode[] rCommands = new OpcReadNode[] {
                new OpcReadNode("ns=2;s=Channel2.Device1.Tag1"),
                new OpcReadNode("ns=2;s=Channel2.Device1.Tag2"),
                new OpcReadNode("ns=2;s=Channel2.Device1.Tag3"),
                new OpcReadNode("ns=2;s=Channel2.Device1.Tag4")
            };
            IEnumerable<OpcValue> job = client.ReadNodes(rCommands);
            int i = 0;
            foreach(OpcValue value in job)
            {
                Console.WriteLine("ReadNode: {0},\t = {1}", rCommands[i].NodeId, value);
                i++;
            }


            // 訂閱Tag5
            OpcSubscription subscription = client.SubscribeDataChange("ns=2;s=Channel2.Device1.Tag5", HandleDataChanged);
            subscription.PublishingInterval = 1000;
            subscription.ApplyChanges();


            Console.ReadLine();

            client.Disconnect();
        }
    }
}
