# Why does a readme for this exist?

## ExceptionSender.cs

```csharp
//Exceptionsender is a class that sends exceptions to the user incase of an error

ExceptionSender exceptionSender = new ExceptionSender();
exceptionSender.SendException(exception);
```
<!--
source: Application Code\ExceptionSender.cs
-->

The program can only send exceptions to the user if it is a fatal error/exception in C# code.

Example;
```csharp
exceptionSender.SendException(0x000001);
```

This is a "nothing" exception that is never thrown under normal circumstances.

## Throw Error Message

```csharp
//ThrowErrorMessage is a method that throws an exception with a message 

```