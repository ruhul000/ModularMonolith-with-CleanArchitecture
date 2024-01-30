using System.Text.RegularExpressions;

namespace Helper.Extensions
{
    public static class CommonExtensions
    {
        #region Miscellaneous
        public static string RemoveSpecialCharacter(this string str)
        {
            str = Regex.Replace(str, @"[$&+,:;=?@#|'<>/\\.^*()%!-]", String.Empty);
            return str;
        }
        public static string RemoveWhiteSpace(this string s)
        {
            return s.Replace(" ", "");
        }
        public static bool IsNullOrEmpty(this string s)
        {
            if (String.IsNullOrEmpty(s))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static string IsNullOrEmpty(this string s, string s2)
        {
            if (String.IsNullOrEmpty(s))
            {
                return s2;
            }
            else
            {
                return s;
            }
        }
        public static T IsNothing<T>(this T t, T t2)
        {
            if (t == null)
            {
                t = t2;
            }
            return t;
        }
        public static string Take(this string s, int i)
        {
            if (i >= s.Length)
            {
                return s;
            }
            else
            {
                return s.Substring(0, i);
            }
        }
        #endregion

        #region To Default Value For Nullable Type
        public static string ToDefaultValue(this string s)
        {
            if (s != null)
            {
                return s;
            }
            else
            {
                return String.Empty;
            }
        }
        public static int ToDefaultValue(this int? i)
        {
            if (i == null)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(i);
            }
        }
        public static long ToDefaultValue(this long? l)
        {
            if (l == null)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt64(l);
            }
        }
        public static double ToDefaultValue(this double? d)
        {
            if (d == null)
            {
                return 0.0;
            }
            else
            {
                return Convert.ToDouble(d);
            }
        }
        public static bool ToDefaultValue(this bool? b)
        {
            if (b == null)
            {
                return false;
            }
            else
            {
                return Convert.ToBoolean(b);
            }
        }
        public static List<T> GetIterable<T>(this List<T> l)
        {
            if (l == null)
            {
                return new List<T>();
            }
            else
            {
                return l;
            }
        }
        public static void Vacant<T>(this List<T> l)
        {
            if (l.Count > 0)
            {
                l = new List<T>();
            }
        }
        #endregion

        #region Converter
        public static List<string> FromStringToList(this string str)
        {
            return new List<string>()
        {
            str
        };
        }
        public static List<string> FromCommaSeparatedStringToList(this string str)
        {
            if (!str.IsNullOrEmpty())
            {
                return str.Replace(" ", "").Split(',').ToList();
            }
            else
            {
                return new List<string>();
            }
        }
        public static List<int> FromCommaSeparatedIntegerToList(this string str)
        {
            if (!str.IsNullOrEmpty())
            {
                return str.Replace(" ", "").Split(',').Select(x => x.ToInt32()).ToList();
            }
            else
            {
                return new List<int>();
            }
        }
        public static string[] FromStringToArray(this string str)
        {
            return new string[]
            {
            str
            };
        }
        public static string ToCommaSeparatedString(this List<string> strList)
        {
            var str = String.Empty;
            if (strList.Count > 0)
            {
                foreach (var _str in strList)
                {
                    var index = strList.FindIndex(x => x == _str);
                    if (index < (strList.Count - 1))
                    {
                        str = str + _str + ",";
                    }
                    else if (index == (strList.Count - 1))
                    {
                        str = str + _str;
                    }
                }
            }
            return str;
        }
        public static string ToCommaSeparatedDBString(this List<string> strList)
        {
            var str = String.Empty;
            if (strList.Count > 0)
            {
                foreach (var _str in strList)
                {
                    //var index = strList.FindIndex(x => x == _str);
                    //if (index < (strList.Count - 1))
                    //{
                    //    str = str + "'" + _str + "'" + ",";
                    //}
                    //else if (index == (strList.Count - 1))
                    //{
                    //    str = str + "'" + _str + "'";
                    //}
                    str = str + "'" + _str + "'" + ",";
                }
                str = str.TrimEnd(',');
            }
            return str;
        }
        public static long ToInt64(this int i)
        {
            return Convert.ToInt64(i);
        }
        public static int ToInt32(this long l)
        {
            return Convert.ToInt32(l);
        }
        public static int ToInt32(this object o)
        {
            try
            {
                return Convert.ToInt32(o);
            }
            catch (Exception)
            {
                return 0;
            }
        }
        public static long ToInt64(this object o)
        {
            try
            {
                return Convert.ToInt64(o);
            }
            catch (Exception)
            {
                return 0;
            }
        }
        public static double ToDouble(this object o)
        {
            try
            {
                return Convert.ToDouble(o);
            }
            catch (Exception)
            {
                return 0.0;
            }
        }
        public static DateTime ToDateTime(this DateTime? dt)
        {
            try
            {
                return Convert.ToDateTime(dt);
            }
            catch (Exception)
            {
                return DateTime.Now;
            }
        }
        public static List<T> ToObjectList<T>(this T obj)
        {
            var list = new List<T>();
            list.Add(obj);
            return list;
        }
        public static object ToDBNullIfNothing<T>(this T value)
        {
            if (value == null)
            {
                return DBNull.Value;
            }
            else
            {
                return value;
            }
        }
        public static string ToLength(this string s, int l)
        {
            if (!String.IsNullOrEmpty(s))
            {
                if (s.Length > l)
                {
                    s = s.Substring(0, l).ToString();
                    return s;
                }
                else
                {
                    return s;
                }
            }
            else
            {
                return s;
            }
        }
        public static bool? ToBoolean(this object o)
        {
            try
            {
                return Convert.ToBoolean(o);
            }
            catch (Exception)
            {
                return null;
            }
        }
        public static byte[] ReadToEnd(this System.IO.Stream stream)
        {
            long originalPosition = 0;

            if (stream.CanSeek)
            {
                originalPosition = stream.Position;
                stream.Position = 0;
            }

            try
            {
                byte[] readBuffer = new byte[4096];

                int totalBytesRead = 0;
                int bytesRead;

                while ((bytesRead = stream.Read(readBuffer, totalBytesRead, readBuffer.Length - totalBytesRead)) > 0)
                {
                    totalBytesRead += bytesRead;

                    if (totalBytesRead == readBuffer.Length)
                    {
                        int nextByte = stream.ReadByte();
                        if (nextByte != -1)
                        {
                            byte[] temp = new byte[readBuffer.Length * 2];
                            Buffer.BlockCopy(readBuffer, 0, temp, 0, readBuffer.Length);
                            Buffer.SetByte(temp, totalBytesRead, (byte)nextByte);
                            readBuffer = temp;
                            totalBytesRead++;
                        }
                    }
                }

                byte[] buffer = readBuffer;
                if (readBuffer.Length != totalBytesRead)
                {
                    buffer = new byte[totalBytesRead];
                    Buffer.BlockCopy(readBuffer, 0, buffer, 0, totalBytesRead);
                }
                return buffer;
            }
            finally
            {
                if (stream.CanSeek)
                {
                    stream.Position = originalPosition;
                }
            }
        }
        public static async Task<byte[]> ReadToEndAsync(this System.IO.Stream stream)
        {
            long originalPosition = 0;

            if (stream.CanSeek)
            {
                originalPosition = stream.Position;
                stream.Position = 0;
            }

            try
            {
                byte[] readBuffer = new byte[4096];

                int totalBytesRead = 0;
                int bytesRead;

                while ((bytesRead = await stream.ReadAsync(readBuffer, totalBytesRead, readBuffer.Length - totalBytesRead)) > 0)
                {
                    totalBytesRead += bytesRead;

                    if (totalBytesRead == readBuffer.Length)
                    {
                        int nextByte = stream.ReadByte();
                        if (nextByte != -1)
                        {
                            byte[] temp = new byte[readBuffer.Length * 2];
                            Buffer.BlockCopy(readBuffer, 0, temp, 0, readBuffer.Length);
                            Buffer.SetByte(temp, totalBytesRead, (byte)nextByte);
                            readBuffer = temp;
                            totalBytesRead++;
                        }
                    }
                }

                byte[] buffer = readBuffer;
                if (readBuffer.Length != totalBytesRead)
                {
                    buffer = new byte[totalBytesRead];
                    Buffer.BlockCopy(readBuffer, 0, buffer, 0, totalBytesRead);
                }
                return buffer;
            }
            finally
            {
                if (stream.CanSeek)
                {
                    stream.Position = originalPosition;
                }
            }
        }
        public static DateTime ToDateTime(this int i)
        {
            /*
             * Why 'UtcNow' instead of 'Now'?
             * Cause this might fall in previous date as per server date server.
             */
            try
            {
                TimeSpan ts = TimeSpan.FromTicks(i);
                return DateTime.UtcNow + ts;
            }
            catch (Exception)
            {
                return DateTime.UtcNow;
            }
        }
        #endregion
    }
}

