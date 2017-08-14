using Renci.SshNet;
using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace RDS.Service
{
    class Program
    {
        public const string ConStr = "vFfVFMQI5xC/Q4Ib4Y08mcrup6hNixMV8zYu7lqte4g=@redis-murahaje.redis.cache.windows.net:6379";
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            GenerateSampleData();

            while (true)
            {
                var devices = GetDevices();
                Parallel.ForEach(devices, (dev) =>
                {
                    DoSSH(dev);
                    Console.WriteLine("Processing {0} on thread {1}", dev.Name, Thread.CurrentThread.ManagedThreadId);
                    //close lambda expression and method invocation
                });
                Thread.Sleep(3000);

            }
        }
        static IEnumerable<DeviceIdentity> GetDevices()
        {
            using (var redisManager = new PooledRedisClientManager(1, ConStr))
            using (var redis = redisManager.GetClient())
            {
                var redisTodos = redis.As<DeviceIdentity>();
                return redisTodos.GetAll();
            }
            
        }
        static void DoSSH(DeviceIdentity Dev)
        {
            // Setup Credentials and Server Information
            ConnectionInfo ConnNfo = new ConnectionInfo(Dev.IpAddress, 22, Dev.UserName,
                new AuthenticationMethod[]{
                // Pasword based Authentication
                new PasswordAuthenticationMethod(Dev.UserName,Dev.Password)                
                }
            );
            /*
            // Execute a (SHELL) Command - prepare upload directory
            using (var sshclient = new SshClient(ConnNfo))
            {
                sshclient.Connect();
                using (var cmd = sshclient.CreateCommand("mkdir -p /tmp/uploadtest && chmod +rw /tmp/uploadtest"))
                {
                    cmd.Execute();
                    Console.WriteLine("Command>" + cmd.CommandText);
                    Console.WriteLine("Return Value = {0}", cmd.ExitStatus);
                }
                sshclient.Disconnect();
            }
            
            // Upload A File
            using (var sftp = new SftpClient(ConnNfo))
            {
                string uploadfn = "Renci.SshNet.dll";

                sftp.Connect();
                sftp.ChangeDirectory("/tmp/uploadtest");
                using (var uplfileStream = System.IO.File.OpenRead(uploadfn))
                {
                    sftp.UploadFile(uplfileStream, uploadfn, true);
                }
                sftp.Disconnect();
            }
            */
            // Execute (SHELL) Commands
            Random rnd = new Random();
            using (var sshclient = new SshClient(ConnNfo))
            {
                sshclient.Connect();

                // quick way to use ist, but not best practice - SshCommand is not Disposed, ExitStatus not checked...
                Console.WriteLine(sshclient.CreateCommand($"cd Downloads/ && wget -O foo-{rnd.Next(1,100)}.html google.com").Execute());
                
                sshclient.Disconnect();
            }
        }

       static void GenerateSampleData()
        {
            using (var redisManager = new PooledRedisClientManager(1,ConStr))
            using (var redis = redisManager.GetClient())
            {
                var redisTodos = redis.As<DeviceIdentity>();
                if (redisTodos.GetAll().Count <= 0)
                {
                    var node = new DeviceIdentity() { Id = redisTodos.GetNextSequence(), Name = "Raspi-1", Desc = "this is first device", IpAddress = "192.168.100.43", UserName="root", Password="raspberry" };
                    redisTodos.Store(node);
                }
            }
        }
    }




    #region entities
  
    public class DeviceIdentity
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string  Desc { get; set; }
        public string IpAddress { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

    }
    #endregion
}