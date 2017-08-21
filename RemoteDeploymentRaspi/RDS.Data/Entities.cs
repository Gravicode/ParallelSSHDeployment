using System;
using System.Collections.Generic;

namespace RDS.Data
{
    public interface IDataAccess
    {
        bool InsertBulkData<T>(IEnumerable<T> data);
        bool InsertData<T>(T data);
        bool DeleteAllData<T>();
        bool DeleteData<T>(long id);
        bool DeleteDataBulk<T>(IEnumerable<T> Ids);
        List<T> GetAllData<T>();
        T GetDataById<T>(long Id);
        List<T> GetDataByIds<T>(params long[] Ids);
        List<T> GetAllData<T>(int Limit);
        List<T> GetDataByStartId<T>(int Limit, long StartId);
        long GetSequence<T>();
    }


    #region entities

    public class DeviceIdentity
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }
        public string IpAddress { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FirmwareVersion { get; set; }
        public string Location { get; set; }
        public double Long { get; set; }
        public double Lat { get; set; }

    }

    public class UpdateHistory
    {
        public long Id { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateBy { get; set; }
        public string FirmwareVersion { get; set; }
        public long DeviceId { get; set; }
        public string DeviceName { get; set; }

    }

    public class FirmwareInfo
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string ReleaseBy { get; set; }
        public string Remark { get; set; }
        public string Url { get; set; }
    }
    public class UpdateQue
    {
        public long Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        public int Attempt { get; set; } = 0;
        public long DeviceId { get; set; }
        public string DeviceName { set; get; }
        public long FirmwareId { get; set; }
        public string FirmwareVersion { set; get; }

        public string FirmwareUrl { set; get; }

        public char Status { get; set; }

        public string UpdatedBy { get; set; }
    }

    public class BackupInfo
    {
        public long Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string Url { get; set; }
        public long DeviceId { get; set; }
        public string DeviceName { get; set; }
    }
    #endregion
}
