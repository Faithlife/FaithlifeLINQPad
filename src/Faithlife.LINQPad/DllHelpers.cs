using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;
using Windows.Win32;
using Windows.Win32.System.LibraryLoader;

namespace Faithlife.LINQPad;

internal static class DllHelpers
{
	public static unsafe IDisposable AddDllDirectory(string directory)
	{
		if (!PInvoke.SetDefaultDllDirectories(LOAD_LIBRARY_FLAGS.LOAD_LIBRARY_SEARCH_DEFAULT_DIRS))
			Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());

		var handle = new IntPtr(PInvoke.AddDllDirectory(directory));
		if (handle == IntPtr.Zero)
			Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());

		return new DllDirectory(handle);
	}

	private sealed class DllDirectory : SafeHandleZeroOrMinusOneIsInvalid
	{
		public DllDirectory(IntPtr handle)
			: base(ownsHandle: true)
		{
			SetHandle(handle);
		}

		protected override unsafe bool ReleaseHandle() => PInvoke.RemoveDllDirectory(DangerousGetHandle().ToPointer());
	}
}
