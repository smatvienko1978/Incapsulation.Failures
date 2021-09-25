using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Incapsulation.Failures
{
   

    //public class Common
    //{
    //    public static int IsFailureSerious(int failureType)
    //    {
    //        if (failureType % 2 == 0) return 1;
    //        return 0;
    //    }


    //    public static int Earlier(object[] v, int day, int month, int year)
    //    {
    //        int vYear = (int)v[2];
    //        int vMonth = (int)v[1];
    //        int vDay = (int)v[0];
    //        if (vYear < year) return 1;
    //        if (vYear > year) return 0;
    //        if (vMonth < month) return 1;
    //        if (vMonth > month) return 0;
    //        if (vDay < day) return 1;
    //        return 0;
    //    }
    //}

    public class ReportMaker
    {
        public enum FailureType
        {
            UnexpectedShutDown = 0,
            ShortNonResponding = 1,
            HardwareFailures = 2,
            ConnectionProblems = 3
        }

        public class Device
        {
            public int DeviceId { get; set; }
            public string Name { get; set; }
            public Device(int id, string name)
            {
                DeviceId = id;
                Name = name;
            }
        }

        public class Failure
        {
            private FailureType FailureType { get; set; }
            public int DeviceId { get; set; }
            public DateTime Date { get; set; }

            public Failure(FailureType failureType, int deviceId, DateTime date)
            {
                FailureType = failureType;
                DeviceId = deviceId;
                Date = date;
            }

            public bool IsFailureSerious()
            {
                if ((int)FailureType % 2 == 0) return true;
                return false;
            }

            public bool Earlier(DateTime date)
            {
                if (Date.Year < date.Year) return true;
                if (Date.Year > date.Year) return false;
                if (Date.Month < date.Month) return true;
                if (Date.Month > date.Month) return false;
                if (Date.Day < date.Day) return true;
                return false;
            }
        }
        /// <summary>
        /// </summary>
        /// <param name="day"></param>
        /// <param name="failureTypes">
        /// 0 for unexpected shutdown, 
        /// 1 for short non-responding, 
        /// 2 for hardware failures, 
        /// 3 for connection problems
        /// </param>
        /// <param name="deviceId"></param>
        /// <param name="times"></param>
        /// <param name="devices"></param>
        /// <returns></returns>
        public static List<string> FindDevicesFailedBeforeDateObsolete(
            int day,
            int month,
            int year,
            int[] failureTypes, 
            int[] deviceId, 
            object[][] times,
            List<Dictionary<string, object>> devices)
        {
            List<Failure> failures = new List<Failure>();
            for (int i = 0; i < failureTypes.Length; i++)
            {
                failures.Add(new Failure(
                    (FailureType)failureTypes[i],
                    deviceId[i],
                    new DateTime((int)times[i][2], (int)times[i][1], (int)times[i][0])
                ));
            }

            List<Device> devs = new List<Device>();
            foreach (var device in devices)
            {
                devs.Add(new Device(
                    (int)device["DeviceId"],
                    device["Name"] as string
                ));
            }

            return FindDevicesFailedBeforeDate(new DateTime(year, month, day), failures, devs);
        }
        public static List<string> FindDevicesFailedBeforeDate(DateTime date, List<Failure> failures, List<Device> devices)
        {
            var problematicDevices = new HashSet<int>();
            foreach (var failure in failures)
            {
                if (failure.IsFailureSerious() && failure.Earlier( date))
                {
                    problematicDevices.Add(failure.DeviceId);
                }
            }

            var result = new List<string>();
            foreach (var device in devices)
            {
                if (problematicDevices.Contains(device.DeviceId))
                {
                    result.Add(device.Name);
                }
            }
            return result;
        }
    }
}
