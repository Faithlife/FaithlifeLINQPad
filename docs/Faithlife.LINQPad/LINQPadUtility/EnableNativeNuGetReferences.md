# LINQPadUtility.EnableNativeNuGetReferences method

Enables loading native dependencies from NuGet packages referenced by the current query. Returns an IDisposable that should be disposed at the end of the query. If it is not disposed, then you can clear the process state by doing Query =&gt; Kill Process.

```csharp
public static IDisposable EnableNativeNuGetReferences()
```

## See Also

* class [LINQPadUtility](../LINQPadUtility.md)
* namespace [Faithlife.LINQPad](../../Faithlife.LINQPad.md)

<!-- DO NOT EDIT: generated by xmldocmd for Faithlife.LINQPad.dll -->
