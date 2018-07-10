using System;
using System.IO;
using System.Runtime.InteropServices; 
using System.Text;


public static class Utils
{
	public static string ReadCustomString(this BinaryReader reader)
	{
		int length = (int)reader.ReadUInt32();
		byte[] bytes = reader.ReadBytes(length);
		return Encoding.UTF8.GetString(bytes);
	}

	public static void CopyToStream(Stream src, Stream dst, byte[] buffer, int numBytes)
	{
		while (numBytes > 0)
		{
			int req = Math.Min(buffer.Length, numBytes);
			int read = src.Read(buffer, 0, req);
			dst.Write(buffer, 0, read);
			numBytes -= read;
		}
	}

#if UNITY_IOS && !UNITY_EDITOR
	[DllImport("__Internal", CharSet=CharSet.Auto, ExactSpelling=true)]
	public static extern bool IsDebuggerPresent();
#else
	public static bool IsDebuggerPresent() { return false; }
#endif
}
