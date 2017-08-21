﻿using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoreLinq;
using ServiceStack.Redis.Generic;
using RDS.Data;

/// <summary>
/// Summary description for GalleryDb
/// </summary>
namespace Rds.Web.Helpers
{
    public class RDSDatabase : IDataAccess
    {
        public static IRedisClientsManager redisManager;
        public static int DbId { set; get; } = 1;
        public static string RedisConStr { set; get; } = "vFfVFMQI5xC/Q4Ib4Y08mcrup6hNixMV8zYu7lqte4g=@redis-murahaje.redis.cache.windows.net:6379";
        public RDSDatabase()
        {

            if (redisManager == null)
            {
                redisManager = new PooledRedisClientManager(DbId, RedisConStr);

            }

        }
        /*
        public static object GetPropValue(object src, string propName)
        {
            return src.GetType().GetProperty(propName).GetValue(src, null);
        }
        */
        public bool InsertBulkData<T>(IEnumerable<T> data)
        {


            using (var redis = redisManager.GetClient())
            {
                var redisStream = redis.As<T>();
                redisStream.StoreAll(data);
                return true;
            }
        }

        public bool InsertData<T>(T data)
        {
            try
            {
                if (data == null) return false;

                using (var redis = redisManager.GetClient())
                {
                    var redisStream = redis.As<T>();
                    redisStream.Store(data);
                    return true;
                }
            }
            catch
            {
                //print ke log
                //throw;
                return false;
            }
        }
        public bool DeleteData<T>(long id)
        {
            try
            {

                using (var redis = redisManager.GetClient())
                {
                    var redisStream = redis.As<T>();
                    redisStream.DeleteById(id);
                    return true;
                }
            }
            catch
            {
                //print ke log
                //throw;
                return false;
            }
        }
        public bool DeleteAllData<T>()
        {
            try
            {

                using (var redis = redisManager.GetClient())
                {
                    IRedisTypedClient<T> redis1 = redis as IRedisTypedClient<T>;

                    var datas = redis1.GetAll();
                    foreach (var item in datas)
                    {
                        redis1.Delete(item);
                    }

                    return true;
                }
            }
            catch
            {
                //print ke log
                //throw;
                return false;
            }
        }
        public bool DeleteDataBulk<T>(IEnumerable<T> Ids)
        {
            try
            {

                using (var redis = redisManager.GetClient())
                {
                    var redisStream = redis.As<T>();
                    redisStream.DeleteByIds(Ids);
                    return true;
                }
            }
            catch
            {
                //print ke log
                //throw;
                return false;
            }
        }

        public T GetDataById<T>(long Id)
        {

            using (var redis = redisManager.GetClient())
            {
                var redisStream = redis.As<T>();

                return redisStream.GetById(Id);

            }
        }
        public List<T> GetDataByIds<T>(params long[] Ids)
        {

            using (var redis = redisManager.GetClient())
            {
                var redisStream = redis.As<T>();
                var data = from c in redisStream.GetByIds(Ids)
                           select c;
                return data.ToList();
            }
        }

        public List<T> GetDataByStartId<T>(int Limit, long StartId)
        {

            using (var redis = redisManager.GetClient())
            {
                var redisStream = redis.As<T>();
                if (typeof(T) == typeof(UpdateQue))
                {
                    var data = from c in redisStream.GetAll()
                               where (c is UpdateQue) && (c as UpdateQue).Id >= StartId
                               orderby (c as UpdateQue).Id
                               select c;
                    return data.Take(Limit).ToList();
                }
                else if (typeof(T) == typeof(DeviceIdentity))
                {
                    var data = from c in redisStream.GetAll()
                               where (c is DeviceIdentity) && (c as DeviceIdentity).Id >= StartId
                               orderby (c as DeviceIdentity).Id
                               select c;
                    return data.Take(Limit).ToList();
                }
                return null;
                /*
                else
                {
                    var data = from c in redisStream.GetAll()
                               where Convert.ToInt32(GetPropValue(c, "Id")) >= StartId
                               select c;
                    return data.Take(Limit).ToList();
                }*/
            }
        }
        public List<T> GetAllData<T>(int Limit)
        {

            using (var redis = redisManager.GetClient())
            {
                var redisStream = redis.As<T>();
                var data = from c in redisStream.GetAll()
                           select c;

                return data == null ? null : data.TakeLast(Limit).ToList();
            }
        }
        public List<T> GetAllData<T>()
        {

            using (var redis = redisManager.GetClient())
            {

                var redisStream = redis.As<T>();
                var data = from c in redisStream.GetAll()
                           select c;
                return data.ToList();
            }
        }

        public long GetSequence<T>()
        {

            using (var redis = redisManager.GetClient())
            {

                var redisStream = redis.As<T>();
                long Id = redisStream.GetNextSequence();
                return Id;

            }
        }

    }

    public static class StringExtensions
    {
        public static bool Contains(this string source, string toCheck, StringComparison comp)
        {
            if (string.IsNullOrEmpty(source) || string.IsNullOrEmpty(toCheck)) return false;
            return source.IndexOf(toCheck, comp) >= 0;
        }
    }

}