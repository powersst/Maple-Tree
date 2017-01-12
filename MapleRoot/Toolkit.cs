// Project: MapleRoot
// File: Toolkit.cs
// Updated By: Jared
// 

#region usings

using System.Threading;

#endregion

namespace MapleRoot
{
    public static class Toolkit
    {
        public static void Sleep(int ms)
        {
            Thread.Sleep(ms);
        }
    }
}