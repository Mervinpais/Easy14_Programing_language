#COPYRIGHT (C) Mervinpaismakeswindows14
import time
import requests
import zipfile
import shutil
import filecmp
import os

wait = time.sleep

print("COPYRIGHT (C) Mervinpaismakeswindows14")
print("Current Working directory: " + os.getcwd())
print("")
wait(1)
print("(NOTE: You will need Wifi/Internet to do a repair as it needs to get files from github)\n")
bkSlash = '\\'
a = input("Would you like to do a repair? do note it will reset any configurations you may have set (Y/N)\n>")
if a.lower() == "y":
    print("\nReady to repair...")
    wait(1)
    URL = "https://github.com/Mervinpais/Easy14_Programing_language/archive/refs/heads/Latest_version.zip"
    response = requests.get(URL)
    open("RepairFiles_ZIP.zip", "wb").write(response.content)
    with zipfile.ZipFile("RepairFiles_ZIP.zip", 'r') as zip_ref:
        zip_ref.extractall("RepairFiles_Folder")
    print("Please now replace each folder with the RepairFiles_Folder folder, Have a good time!\n(Press Enter To Close)")
    input("\n>")
else:
    print("\nClosing...")
    wait(1)
    exit()
"""
#Application/.net files (apps related to the program files/dll's and not the .exe ex; apphost, APPNAME.dll)
AppDLLs = bkSlash + "obj" + bkSlash + "Debug" + bkSlash + "net5.0"
    if filecmp.cmp(os.getcwd() + bkSlash + "RepairFiles_Folder" + bkSlash + "Easy14_Programing_language-Latest_version" + bkSlash + "Program.cs", os.getcwd() + "\\..\\Program.cs") == false:
        shutil.copyfile(os.getcwd() + bkSlash + "RepairFiles_Folder" + bkSlash + "Easy14_Programing_language-Latest_version" + bkSlash + "Program.cs", os.getcwd() + "\\..\\Program.cs")
    input("next?")
    if filecmp.cmp(os.getcwd() + "\\RepairFiles_Folder\\Easy14_Programing_language-Latest_version\\Easy14 Coding Language.csproj", "..\\Easy14 Coding Language.csproj") == false:
        shutil.copyfile(os.getcwd() + "\\RepairFiles_Folder\\Easy14_Programing_language-Latest_version\\Easy14 Coding Language.csproj", "..\\Easy14 Coding Language.csproj")
    if filecmp.cmp(os.getcwd() + "\\RepairFiles_Folder\\Easy14_Programing_language-Latest_version\\Easy14 Coding Language.csproj.user", "..\\Easy14 Coding Language.csproj.user") == false:
        shutil.copyfile(os.getcwd() + "\\RepairFiles_Folder\\Easy14_Programing_language-Latest_version\\Easy14 Coding Language.csproj.user", "..\\Easy14 Coding Language.csproj.user")
    if filecmp.cmp(os.getcwd() + "\\RepairFiles_Folder\\Easy14_Programing_language-Latest_version\\LICENSE", "\\..\\LICENSE") == false:
        shutil.copyfile(os.getcwd() + "\\RepairFiles_Folder\\Easy14_Programing_language-Latest_version\\LICENSE", "\\..\\LICENSE")
    if filecmp.cmp(os.getcwd() + "\\RepairFiles_Folder\\Easy14_Programing_language-Latest_version\\README.md", "\\..\\README.md") == false:
        shutil.copyfile(os.getcwd() + "\\RepairFiles_Folder\\Easy14_Programing_language-Latest_version\\README.md", "\\..\\README.md")
"""