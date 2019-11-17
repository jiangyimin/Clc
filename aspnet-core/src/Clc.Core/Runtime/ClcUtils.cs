using System;
using System.Collections.Generic;

namespace Clc.Runtime
{
    public class LambdaEqualityComparer<T> : IEqualityComparer<T>
    {
        public LambdaEqualityComparer(Func<T, T, bool> equalsFunction)
        {
            _equalsFunction = equalsFunction;
        }

        public bool Equals(T x, T y)
        {
            return _equalsFunction(x, y);
        }

        public int GetHashCode(T obj)
        {
            return obj.GetHashCode();
        }

        private readonly Func<T, T, bool> _equalsFunction;
    }

    public class ClcUtils
    {
        private const double EARTH_RADIUS = 6378.137 * 1000;//地球半径,单位为米  
        private static double rad(double d)
        {  
            return d* Math.PI / 180.0;  
        }  
        /// <summary>  
        /// 返回两点之间的距离，单位为米  
        /// </summary>  
        /// <param name="lat1"></param>  
        /// <param name="lng1"></param>  
        /// <param name="lat2"></param>  
        /// <param name="lng2"></param>  
        /// <returns></returns>  
        public static double GetDistance(double lat1, double lng1, double lat2, double lng2)
        {  
            double radLat1 = rad(lat1);  
            double radLat2 = rad(lat2);  
            double a = radLat1 - radLat2;  
            double b = rad(lng1) - rad(lng2);  
 
            double s = 2 * Math.Asin(Math.Sqrt(Math.Pow(Math.Sin(a / 2), 2) +
            Math.Cos(radLat1) * Math.Cos(radLat2) * Math.Pow(Math.Sin(b / 2), 2)));  
            s = s* EARTH_RADIUS;  
            s = Math.Round(s* 10000) / 10000;  
            return s;  
        } 

        public static DateTime GetDateTime(string time, bool isTomorrow = false)
        {
            DateTime today = DateTime.Now;
            DateTime tomorrow = today.AddDays(1);
            if (!isTomorrow)
                return new DateTime(today.Year, today.Month, today.Day, int.Parse(time.Substring(0, 2)), int.Parse(time.Substring(3, 2)), 0);
            else 
                return new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, int.Parse(time.Substring(0, 2)), int.Parse(time.Substring(3, 2)), 0);
        }

        public static bool NowInTimeZone(string start, string end)
        {
            if (DateTime.Now >= ClcUtils.GetDateTime(start) && DateTime.Now <= ClcUtils.GetDateTime(end))
                return true;
            else
                return false;
        }
        public static bool NowInTimeZone(DateTime start, DateTime end)
        {
            if (DateTime.Now >= start && DateTime.Now <= end) return true;
            return false;
        }

        public static bool NowInTimeZone(string startTime, int lead, int deadline)
        {
            DateTime start = GetDateTime(startTime);
            if (DateTime.Now >= start.Subtract(TimeSpan.FromMinutes(lead)) && 
                    DateTime.Now <= start.Add(TimeSpan.FromMinutes(deadline))) return true;
            return false;
        }

        public static string GetRandomNumber(int length)
        {
            Random rnd = new Random();
            string ret = string.Empty;
            for (int i = 0; i < length; i++)
                ret += rnd.Next(9);
            return ret;
        }
    }
}
