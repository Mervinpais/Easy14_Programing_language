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
<a style="color: #0092fa;">Console</a>.<a style="color: #bc57ff;">print</a>(<a style="color: #409438;">"Hello World"</a>);
</code>
</div>

<br>

<para> we can remove the unnessary "Console" part of our code by add a using statement at the top </para>

<br>

<div class="code">
    <code class="language-csharp">
        <a style="color: #0662b2;">using</a><a style="color: #ffffff;"> Console;</a>
    </code>
</div>

<div class="code">
    <code class="language-csharp">
        <a style="color: #bc57ff;">print</a>(<a style="color: #409438;">"Hello world"</a>);
    </code>
</div>

<br>

<para> You can also print arithmetic operations (only one at a time though :( </para>

<br>

<div class="code">
    <code class="language-csharp">
        <a style="color: #bc57ff;">print</a>(1+2);
    </code>
</div>

<div class="code">
    <code class="language-csharp">
        <a style="color: #387632;">//Works</a>
    </code>
</div>

<div class="code">
    <code class="language-csharp">
        <a style="color: #bc57ff;">print</a>(1+4-2);
    </code>
</div>

<div class="code">
    <code class="language-csharp">
        <a style="color: #387632;">//Wont work :(</a>
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
        <a style="color: #0092fa;">Console</a>.<a style="color: #bc57ff;">input</a>();
    </code>
</div>

<br>

<div class="code">
    <code class="language-csharp">
        <a style="color: #0662b2;">using</a><a style="color: #ffffff;"> Console;</a>
    </code>
</div>

<div class="code">
    <code class="language-csharp">
        <a style="color: #bc57ff;">input</a>();
    </code>
</div>

<br>

<para> Below is what input(); can do currently </para>

<br>

<div class="code">
    <code class="language-csharp">
        <a style="color: #bc57ff;">input</a>();
    </code>
</div>
<div class="code">
    <code class="language-csharp">
        <a style="color: #387632;">//Works</a>
    </code>
</div>
<div class="code">
    <code class="language-csharp">
        <a style="color: #bc57ff;">input</a>("My Input");
    </code>
</div>
<div class="code">
    <code class="language-csharp">
        <a style="color: #387632;">//Wont work :(</a>
    </code>
</div>

<br>

</body>
