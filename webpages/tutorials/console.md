<head>
   <link rel="stylesheet" type="text/css" href="https://mervinpais.github.io/Easy14_Programing_language/style.css">
</head>
<body class="dark_body">
   <head1> Console </head1>
   <br>
   <para>Console was the first ever (Hardcoded) library made for Easy14 by Mervinpais (Creator of Easy14)</para>
   <br>
   <div class="code">
      <code class="language-csharp">
      <a style="color: #0092fa;">Console</a>.<a style="color: #bc57ff;">print</a>(<a style="color: #409438;">"Hello World"</a>);
      </code>
   </div>
   <br>
   <para>NOTE; you can not have a local variable without a value, so you need to specify "" as null so there is no error (will be fixed in version 2.5)</para>
   <br>
   <br>
   <para>See below for what is/isn't allowed;</para>
   <br>
   <div class="code">
      <code class="language-csharp">
      <a style="color: #5016c5;">var</a> myVariable = <a style="color: #409438;">""</a>;
      </code>
   </div>
   <div class="code">
      <code class="language-csharp">
      <a style="color: #387632;">//Works</a>
      </code>
   </div>
   <div class="code">
      <code class="language-csharp">
      <a style="color: #5016c5;">var</a> myVariable = <a style="color: #bce4b9;">14</a>;
      </code>
   </div>
   <div class="code">
      <code class="language-csharp">
      <a style="color: #387632;">//Works</a>
      </code>
   </div>
   <div class="code">
      <code class="language-csharp">
      <a style="color: #5016c5;">var</a> myVariable;
      </code>
   </div>
   <div class="code">
      <code class="language-csharp">
      <a style="color: #387632;">//Wont work :(</a>
      </code>
   </div>
   <br>
   <para>Ok so now how do we use those variables?<br> Well here are a few examples</para>
   <div class="code">
      <code class="language-csharp">
         <a style="color: #5016c5;">var</a> myVariable = <a style="color: #409438;">"This was from a variable"</a>;
      </code>
   </div>
   <div class="code">
      <code class="language-csharp">
         <a style="color: #5016c5;">var</a> myVariable2 = <a style="color: #409438;">10</a>;
      </code>
   </div>
   <div class="code">
      <code class="language-csharp">
         <a style="color: #5016c5;">var</a> result = <a style="color: #409438;">myVariable2</a> + <a style="color: #409438;">4</a>;
      </code>
   </div>
   <div class="code">
      <code class="language-csharp">
      <a style="color: #0092fa;">Console</a>.<a style="color: #bc57ff;">print</a>(<a style="color: #8cabd9;">result</a>);
      </code>
   </div>
   <br>
   <para>More info comming soon</para>
   <br>
</body>