using RDS.Data;
using Renci.SshNet;
using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using MoreLinq;

namespace RDS.Service
{
    class Program
    {
        public const string ConStr = "127.0.0.1:6379"; //"vFfVFMQI5xC/Q4Ib4Y08mcrup6hNixMV8zYu7lqte4g=@redis-murahaje.redis.cache.windows.net:6379";
        static void Main(string[] args)
        {
            Console.WriteLine("Start deployment service!");
            GenerateSampleData();

            while (true)
            {
                var Ques = GetQues();
                if (Ques != null && Ques.Count() > 0)
                {
                    var devices = GetDevices();
                    //update flag to start
                    UpdateQueStatus(Ques, 'S');
                    Parallel.ForEach(Ques, (que) =>
                    {
                        var dev = devices[que.DeviceId];
                        try
                        {
                            DoSSH(dev, que);
                            Console.WriteLine("Processing update on device {0} on thread {1}", dev.Name, Thread.CurrentThread.ManagedThreadId);
                            que.Status = 'E';
                        }
                        catch (Exception ex)
                        {
                            que.Status = 'F';
                            Console.WriteLine("Failure update on device {0} on thread {1}", dev.Name, Thread.CurrentThread.ManagedThreadId);
                        }
                        finally
                        {
                            que.Attempt += 1;
                        }
                    //close lambda expression and method invocation
                    });
                    UpdateQueStatus(Ques);
                    UpdateHistory(Ques);
                }
                Thread.Sleep(3000);

            }
        }

        static void UpdateQueStatus(IEnumerable<UpdateQue> data,char State='X')
        {
            
            using (var redisManager = new PooledRedisClientManager(1, ConStr))
            using (var redis = redisManager.GetClient())
            {
                var redisTodos = redis.As<UpdateQue>();
                foreach (var item in data)
                {
                    if(State!='X')
                        item.Status = State;
                    redisTodos.Store(item);
                }
            }
        }

        static void UpdateHistory(IEnumerable<UpdateQue> Ques)
        {
            using (var redisManager = new PooledRedisClientManager(1, ConStr))
            using (var redis = redisManager.GetClient())
            {
                var redisTodos = redis.As<UpdateHistory>();
                var datas = from c in Ques
                            where c.Status == 'E'
                            select c;
                foreach (var item in datas)
                {
                    redisTodos.Store(new Data.UpdateHistory() { Id = redisTodos.GetNextSequence(), DeviceId = item.DeviceId, DeviceName = item.DeviceName, FirmwareVersion = item.FirmwareVersion, UpdateBy = "system", UpdateDate = DateTime.Now });
                }
            }
        }
        static IEnumerable<UpdateQue> GetQues()
        {
            using (var redisManager = new PooledRedisClientManager(1, ConStr))
            using (var redis = redisManager.GetClient())
            {
                var redisTodos = redis.As<UpdateQue>();
                var datas = from c in redisTodos.GetAll()
                            where c.Status == 'P' || (c.Status=='F' && c.Attempt<10)
                            select c;
                return datas.ToList();
            }

        }
        static Dictionary<long,DeviceIdentity> GetDevices()
        {
            var data = new Dictionary<long, DeviceIdentity>();
            using (var redisManager = new PooledRedisClientManager(1, ConStr))
            using (var redis = redisManager.GetClient())
            {
                var redisTodos = redis.As<DeviceIdentity>();
                var datas = redisTodos.GetAll();
                foreach(var item in datas)
                {
                    data.Add(item.Id, item);
                }
            }
            return data;
            
        }
        static void DoSSH(DeviceIdentity Dev, UpdateQue Que)
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
                Console.WriteLine(sshclient.CreateCommand($"cd Downloads/ && wget -O foo-{rnd.Next(1,100)}.html {Que.FirmwareUrl}").Execute());
                Console.WriteLine($"SSH executed on {Dev.Name}");
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
                    var node = new DeviceIdentity() { Id = redisTodos.GetNextSequence(), Name = "Raspi-1", Desc = "this is first device", IpAddress = "192.168.100.43", UserName="root", Password="raspberry", FirmwareVersion="1.0", Location="Jakarta" };
                    redisTodos.Store(node);
                    var node2 = new DeviceIdentity() { Id = redisTodos.GetNextSequence(), Name = "Raspi-2", Desc = "this is second device", IpAddress = "192.168.100.44", UserName = "root", Password = "raspberry", FirmwareVersion="1.0", Location="Bogor" };
                    redisTodos.Store(node2);

                }
            }
        }
    }




}