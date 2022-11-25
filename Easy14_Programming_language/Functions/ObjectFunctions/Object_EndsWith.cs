namespace Easy14_Programming_Language
{
    public static class Object_EndsWith
    {
        public static object seperateCodeStatement(string line, bool returnObjectName = true)
        {
            object[] para = line.Split(",");
            if (returnObjectName == false)
                return para[1];
            else
                return para[0];
        }

        public static bool Interperate(object obj)
        {
            object objToBeFound = seperateCodeStatement(obj.ToString(), false);
            obj = seperateCodeStatement(obj.ToString());
            if (obj.ToString().EndsWith(objToBeFound.ToString()))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
