<head>
<link rel="stylesheet" type="text/css" href="https://mervinpais.github.io/Easy14_Programing_language/style.css">
</head>

<body class="dark_body">
<head1> Basic Functions </head1>
<br>
<head2> Console.print() </head2>
<br>
<para>Console.print() is used to print stuff to the screen</para>

<code class="language-csharp">
Console.print("Hello World");
</code>

<para> we can remove the unnessary "Console" part of our code by add a using statement at the top </para>

<code class="language-csharp">
using Console;
print("Hello world");
</code>

<para> You can also print arithmetic operations (only one at a time though :( </para>

<code class="language-csharp">
print(1+2);
//Works
print(1+4-2);
//Wont work :(
</code>

<para> You can do +, -, *, /, % (Modulo), and == in a print statement </para>

___

<head2> Console.input() </head2>

<para> Console.input() is used to ask the user for input </para>

<code class="language-csharp">
Console.input();
</code>

<code class="language-csharp">
using Console;
input();
</code>

Below is what input(); can do currently

<code class="language-csharp">
input();
//Works
input("My Input");
//Wont work :(
</code>

</body>
