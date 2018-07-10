using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.UI;


public class ScreenStream: MonoBehaviour
{
	public GameObject uiBox;
	public GameObject warningBox;
	public Text warningText;
	public RawImage screenImage;

	Texture2D screen = null;
	bool synced = false;

	byte[] image;
	int width;
	int height;


	void Update()
	{
		if (synced && (screen != null))
		{
			screenImage.gameObject.SetActive(true);
			screenImage.texture = screen;
		}
		else
		{
			screenImage.gameObject.SetActive(false);
			if (SystemInfo.supportsGyroscope)
				Input.gyro.enabled = false;
		}

		string warningString = "";

#if UNITY_EDITOR
		warningString += "Warning: This project should not be run in the editor, please deploy to a device to use.";
#endif
		if (Utils.IsDebuggerPresent())
		{
			if (warningString.Length > 0)
				warningString += "\n";
			warningString += "Warning: debugger detected. Unity Remote will not function properly with debugger attached.";
		}

		if (warningString.Length > 0)
		{
			warningText.text = warningString;
			warningBox.SetActive(true);
		}
		else
			warningBox.SetActive(false);
	}

	void LateUpdate()
	{
		Profiler.BeginSample("ScreenStream.LateUpdate");

		if (screen == null || screen.width != width || screen.height != height)
			screen = new Texture2D(width, height, TextureFormat.RGB24, false);

		Profiler.BeginSample("LoadImage");
		if ((image != null) && (image.Length > 0) && screen.LoadImage(image))
			synced = true;
		image = null;
		Profiler.EndSample();

		Profiler.EndSample();
	}


	public void OnDisconnect()
	{
		synced = false;
		image = null;
	}


	public void UpdateScreen(byte[] data, int width, int height)
	{
		// Loading texture takes a lot of time, so we postpone it and do it in
		// LateUpdate(), in case we receive several images during single frame.
		this.image = data;
		this.width = width;
		this.height = height;
	}
}
