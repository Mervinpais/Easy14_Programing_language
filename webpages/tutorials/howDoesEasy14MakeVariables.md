# How does Easy14 make variables?

## Easy14 save variables in a weird way (because it's the eaisest :_)

We use System.IO in C# to make a file in the user's Desktop folder

*But we can't just make a file in the desktop, that would be bad if we made alot of variables and the desktop gets filled up with variable files!*

In that case, we make a folder called EASY14_Variables_TEMP in the Desktop directory

Then we make variables as .txt files (very good, i know)

Then just store the variable in that file and then access it anytime

After Easy14 is done excuting the current program, Easy14 discards all variables by deleting the folder.

This is much easier than dealing with system memory

### Although you do still need storage space to store these variables.

FAQ;

Will there better a better variable storage system
:   Now is not the time...

