<head>

<link rel="stylesheet" type="text/css" href="https://mervinpais.github.io/Easy14_Programing_language/style.css">

</head>

<body class="dark_body">

<head1> Basic Functions </head1>

<br>

<head3> Console.print() </head3>

<br>

<para>Console.print() is used to print stuff to the screen</para>

<br>

<div class="code">
<code class="language-csharp">
<mark style="color: #0092fa;">Console</mark>.<mark style="color: #bc57ff;">print</mark>(<mark style="color: #2b6426;">"Hello World"</mark>);
</code>
</div>

<br>

<para> we can remove the unnessary "Console" part of our code by add a using statement at the top </para>

<br>

<div class="code">
<code class="language-csharp">
using Console;
print("Hello world");
</code>
</div>

<br>

<para> You can also print arithmetic operations (only one at a time though :( </para>

<br>

<div class="code">
<code class="language-csharp">
print(1+2);
//Works
print(1+4-2);
//Wont work :(
</code>
</div>

<br>

<para> You can do +, -, *, /, % (Modulo), and == in a print statement </para>

<br>

<head3> Console.input() </head3>

<br>

<para> Console.input() is used to ask the user for input </para>

<br>

<div class="code">
<code class="language-csharp">
Console.input();
</code>
</div>

<br>

<div class="code">
<code class="language-csharp">
using Console;
input();
</code>
</div>

<br>

<para> Below is what input(); can do currently </prar?>

<br>

<div class="code">
<code class="language-csharp">
input();
//Works
input("My Input");
//Wont work :(
</code>
</div>

<br>

</body>
