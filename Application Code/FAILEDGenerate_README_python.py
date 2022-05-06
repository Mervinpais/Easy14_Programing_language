#I didnt want to wait time typing in rules so this is the rules generator so i just need to add a new statement and it will make a read with the
#rules made :)

#Also incase you delete the orignal Read me, you can use this python app to regenerate it :)
def make_Statement(title, contents):
    contents_of_file = [str(title)]
    for line in contents:
        contents_of_file.append(line)
        print(contents_of_file)
    return contents_of_file

def makeIntoFileLook(contents):
    re = "==={}===\n\n{}\n{}\n{}".format(contents[0],contents[1],contents[2],contents[3])
    print(re)

Tcont = { "0x0000A# Meanings", "0x0000B# Meanings", 
          "0x0000C# Meanings", "0x0000D# Meanings", 
          "0x0000E# Meanings", "0x0000F# Meanings"}

cont = { "0x0000A0 - Excuse Error for stuff ununderstandable stuff",
         "0x0000B0 - Excuse Error for stuff ununderstandable stuff",
         "0x0000C0 - Excuse Error for stuff ununderstandable stuff",
         "0x0000D0 - Excuse Error for stuff ununderstandable stuff",
         "0x0000E0 - Excuse Error for stuff ununderstandable stuff",
         "0x0000F0 - Excuse Error for stuff ununderstandable stuff"}

cont2 = {"0x0000A1 - Application Failed to do a task",
         "0x0000B1 - ",
         "0x0000C1 - Missing Code",
         "0x0000D1 - ",
         "0x0000E1 - ",
         "0x0000F1 - "}
a = make_Statement("Test", cont)
dsffwf = {None}
dsffwf.append(Tcont)
dsffwf.append(cont)
dsffwf.append(cont2)
makeIntoFileLook()
