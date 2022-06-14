<head>
   <link rel="stylesheet" type="text/css" href="https://mervinpais.github.io/Easy14_Programing_language/style.css">
</head>
<body class="dark_body">
   <head1> Variables </head1>
   <br>
   <para>Easy14 is Dynamically Typed (AKA you dont need to specifiy the type of data like string, bool, int)</para>
   <br>
   <div class="code">
      <code class="language-csharp">
      <a style="color: #5016c5;">var</a>myVariable = "";
      </code>
   </div>
   <br>
   <para>NOTE; you can not have a local variable without a value<, so you need to specify "" as null so there is no error (will be fixed in version 2.5)</para>
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
</body>