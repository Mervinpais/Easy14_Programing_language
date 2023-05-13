using System;
using System.Diagnostics;
using System.IO;

namespace Easy14_Programming_Language
{
    public static class FFC //Functions-For-CSharp
    {
        public static void help()
        {
            Debug.Write("");
        }
        public static class IO
        {
            static private string FullName_;
            static public string FullName
            {
                get { return FullName_; }
                set { FullName_ = value; }
            }
            public static object RecursiveParent(object path, int repeat)
            {
                object resultPath = path;
                for (int i = 0; i < repeat; i++)
                {
                    FullName = Directory.GetParent(Convert.ToString(resultPath)).FullName;
                    resultPath = Directory.GetParent(Convert.ToString(resultPath));
                }

                return resultPath;
            }
        }
    }
}
