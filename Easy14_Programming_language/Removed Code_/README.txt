(This is a ReadMe For Removed_Code)

The Old Code here is only for files if converted to seperate files, cause issues, then we cant revert them,
otherwise those who dont have a remove code .txt file dont need to bereverted because they probably wont get issues

Diagram; (Very Bad lol)

if (seperateFunc(funcName).status == "hasError(s)WhenCompiled")
{
    if (FindOldFile(funcName).Name == funcName) {
        revertSepFunc(oldCodeFile, seperateFuncFileName);
    }
    else
    {
        throw new Exception("NO_OLD_FILE_FOUND");
    }
}
else {}

=========================================
Anyhow thanks for reading