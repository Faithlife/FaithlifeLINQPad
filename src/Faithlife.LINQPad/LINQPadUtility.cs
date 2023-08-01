using System.Reflection;
using System.Runtime.InteropServices;
using LINQPad;
using Nito.Disposables;
using NuGet.RuntimeModel;

namespace Faithlife.LINQPad;

/// <summary>
/// Provides helper methods for working with LINQPad.
/// </summary>
public static class LINQPadUtility
{
	/// <summary>
	/// Enables loading native dependencies from NuGet packages referenced by the current query.
	/// Returns an <see cref="IDisposable"/> that should be disposed at the end of the query.
	/// If it is not disposed, then you can clear the process state by doing Query => Kill Process.
	/// </summary>
	public static IDisposable EnableNativeNuGetReferences()
	{
		var supportedRuntimes = GetSupportedRuntimes();
		var folders = Util.CurrentQuery.NuGetReferences
			.SelectMany(x => x.GetPackageFolders())
			.Distinct()
			.Where(x => Directory.Exists($@"{x}\runtimes"))
			.Select(x => supportedRuntimes
				.Select(y => $@"{x}\runtimes\{y}\native")
				.FirstOrDefault(Directory.Exists))
			.Where(x => x != null)
			.ToList();

		using var tempDisposable = new CollectionDisposable();
		foreach (var folder in folders)
			tempDisposable.Add(DllHelpers.AddDllDirectory(folder!));
		return new CollectionDisposable(tempDisposable.Abandon());
	}

	private static IReadOnlyList<string> GetSupportedRuntimes()
	{
		var assembly = Assembly.GetExecutingAssembly();
		using var resourceStream = assembly.GetManifestResourceStream($"{assembly.GetName().Name}.runtime.json")
			?? throw new InvalidOperationException("Embedded runtime.json not found");
		var runtimeGraph = JsonRuntimeFormat.ReadRuntimeGraph(resourceStream);

		return runtimeGraph.ExpandRuntime(RuntimeInformation.RuntimeIdentifier).ToList();
	}
}
