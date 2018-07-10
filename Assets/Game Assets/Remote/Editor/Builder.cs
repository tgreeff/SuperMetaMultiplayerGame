using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.IO;
using System.Linq;
using UnityEditor.Build.Reporting;

public static class Builder
{
	static string[] scenes = { "Assets/Remote.unity" };

	private static void CheckError(BuildReport report)
	{
		if (report.summary.result != BuildResult.Succeeded) {
			throw new Exception("Build failed: " + string.Join("\n", report.steps.SelectMany(s => s.messages).Select(m => string.Format("[{0}] {1}", m.type, m.content)).ToArray()));
		}
	}

	private static void LogExceptionIfPossible(Exception xc)
	{
		try
		{
			using (StreamWriter log = File.AppendText("build/Error.log"))
			{
				log.WriteLine(xc.Message);
				log.WriteLine(xc.StackTrace);
			}
		}
		catch (Exception e)
		{
			Console.WriteLine(e.Message);
			Console.WriteLine(e.StackTrace);
		}
	}

	[MenuItem("Build/Build iOS")]
	public static void BuildiOS()
	{
		try
		{
			var report = BuildPipeline.BuildPlayer(scenes, "build/UnityRemote-iOS", BuildTarget.iOS, BuildOptions.None);

			CheckError(report);
		}
		catch (Exception xc)
		{
			LogExceptionIfPossible(xc);
			throw xc;
		}
	}

	[MenuItem("Build/Build tvOS")]
	public static void BuildtvOS()
	{
		try
		{
			var report = BuildPipeline.BuildPlayer(scenes, "build/UnityRemote-tvOS", BuildTarget.tvOS, BuildOptions.None);

			CheckError(report);
		}
		catch (Exception xc)
		{
			LogExceptionIfPossible(xc);
			throw xc;
		}
	}

	[MenuItem("Build/Build Android")]
	public static void BuildAndroid()
	{
		try
		{
			string sdk = Environment.GetEnvironmentVariable("ANDROID_SDK_ROOT");
			EditorPrefs.SetString("AndroidSdkRoot", sdk);
			PlayerSettings.Android.keystoreName = "mock.keystore";
			PlayerSettings.Android.keyaliasName = "fake";
			PlayerSettings.Android.keystorePass = "password";
			PlayerSettings.Android.keyaliasPass = "password";
			var report = BuildPipeline.BuildPlayer(scenes, "build/UnityRemote-Android.apk", BuildTarget.Android, BuildOptions.None);

			CheckError(report);
		}
		catch (Exception xc)
		{
			LogExceptionIfPossible(xc);
			throw xc;
		}
	}
}
