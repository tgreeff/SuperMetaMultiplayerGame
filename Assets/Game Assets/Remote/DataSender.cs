﻿using UnityEngine;
using System.Collections;
using System.IO;
using System.Linq;
using System.Text;
using SDebug = System.Diagnostics.Debug;


public enum RemoteMessage: byte
{
	Invalid = 0,

	Hello = 1,
	Options = 2,
	GyroSettings = 3,
	DeviceOrientation = 4,
	DeviceFeatures = 5,

	TouchInput = 10,
	AccelerometerInput = 11,
	TrackBallInput = 12,
	Key = 13,
	GyroInput = 14,
	MousePresence = 15,
	JoystickInput = 16,
	JoystickNames = 17,

	WebCamDeviceList = 20,
	WebCamStream = 21,

	LocationServiceData = 30,
	CompassData = 31,

	Reserved = 255,
}


public class PacketWriter
{
	BinaryWriter writer;
	MemoryStream packet;
	RemoteMessage message;
	byte[] buffer = new byte[128 * 1024];


	public void BeginMessage(RemoteMessage message)
	{
		SDebug.Assert(message == RemoteMessage.Invalid);

		this.message = message;
		packet.Position = 0;
		packet.SetLength(0);
	}

	public void EndMessage(Stream stream)
	{
		SDebug.Assert(message != RemoteMessage.Invalid);

		// Write message header
		stream.WriteByte((byte)message);
		uint len = (uint)packet.Length;
		stream.WriteByte((byte)(len & 0xFF));
		stream.WriteByte((byte)((len >> 8) & 0xFF));
		stream.WriteByte((byte)((len >> 16) & 0xFF));
		stream.WriteByte((byte)((len >> 24) & 0xFF));

		// Write the message
		packet.Position = 0;
		Utils.CopyToStream(packet, stream, buffer, (int)packet.Length);

		message = RemoteMessage.Invalid;
	}

	public void Write(bool value) { writer.Write(value); }
	public void Write(byte value) { writer.Write(value); }
	public void Write(int value) { writer.Write(value); }
	public void Write(uint value) { writer.Write(value); }
	public void Write(long value) { writer.Write(value); }
	public void Write(ulong value) { writer.Write(value); }
	public void Write(float value) { writer.Write(value); }
	public void Write(double value) { writer.Write(value); }
	public void Write(byte[] value) { writer.Write(value); }

	public void Write(string value) 
	{
		writer.Write((uint)value.Length);
		writer.Write(Encoding.UTF8.GetBytes(value)); 
	}

	public PacketWriter()
	{
		packet = new MemoryStream();
		writer = new BinaryWriter(packet);
		message = RemoteMessage.Invalid;
	}
}


public struct OldLocationData
{
	public bool isEnabledByUser;
	public LocationServiceStatus status;
	public LocationInfo lastData;
}


public struct OldCompassData
{
	public bool enabled;
	public float magneticHeading;
	public float trueHeading;
	public float headingAccuracy;
	public Vector3 rawVector;
	public double timestamp;
}


public class DataSender
{
	const int MAX_AXES = 28;
	const int MAX_JOYSTICKS = 9;

	PacketWriter writer;
	Stream stream;


	OldLocationData? oldLocationData;
	OldCompassData?  oldCompassData;
	string[] oldJoystickNames = {};
	uint[] oldJoystickButtons = new uint[MAX_JOYSTICKS];
	float[,] oldJoystickAxes = new float[MAX_JOYSTICKS, MAX_AXES];

	public void SendHello()
	{
		writer.BeginMessage(RemoteMessage.Hello);
		writer.Write("UnityRemote");
		writer.Write((uint)0);
		writer.EndMessage(stream);
	}


	public void SendOptions()
	{
		// Add Screen size information
		// TODO: only send when changed
		writer.BeginMessage(RemoteMessage.Options);
		writer.Write(Screen.width);
		writer.Write(Screen.height);
		writer.EndMessage(stream);
	}


	public void SendDeviceOrientation()
	{
		writer.BeginMessage(RemoteMessage.DeviceOrientation);
		writer.Write((int)Input.deviceOrientation);
		writer.EndMessage(stream);
	}


	public void SendAccelerometerInput()
	{
		writer.BeginMessage(RemoteMessage.AccelerometerInput);
		writer.Write(Input.acceleration.x);
		writer.Write(Input.acceleration.y);
		writer.Write(Input.acceleration.z);
		writer.Write(Time.deltaTime);
		writer.EndMessage(stream);
	}


	public void SendGyroscopeSettings()
	{
		Gyroscope gyro = Input.gyro;
		writer.BeginMessage(RemoteMessage.GyroSettings);
		writer.Write(gyro.enabled ? 1 : 0);
		writer.Write(gyro.updateInterval);
		writer.EndMessage(stream);
	}


	public void SendGyroscopeInput()
	{
		// TODO: check updateInterval here..
		Gyroscope gyro = Input.gyro;
		writer.BeginMessage(RemoteMessage.GyroInput);
		writer.Write(gyro.rotationRate.x);
		writer.Write(gyro.rotationRate.y);
		writer.Write(gyro.rotationRate.z);
		writer.Write(gyro.rotationRateUnbiased.x);
		writer.Write(gyro.rotationRateUnbiased.y);
		writer.Write(gyro.rotationRateUnbiased.z);
		writer.Write(gyro.gravity.x);
		writer.Write(gyro.gravity.y);
		writer.Write(gyro.gravity.z);
		writer.Write(gyro.userAcceleration.x);
		writer.Write(gyro.userAcceleration.y);
		writer.Write(gyro.userAcceleration.z);
		writer.Write(gyro.attitude.x);
		writer.Write(gyro.attitude.y);
		writer.Write(gyro.attitude.z);
		writer.Write(gyro.attitude.w);
		writer.EndMessage(stream);
	}


	public void SendTouchInput()
	{
		for (int i = 0; i < Input.touchCount; ++i)
		{
			Touch touch = Input.GetTouch(i);
			writer.BeginMessage(RemoteMessage.TouchInput);
			writer.Write(touch.position.x);
			writer.Write(touch.position.y);
			writer.Write((long)Time.frameCount);
			writer.Write(touch.fingerId);
			writer.Write((int)touch.phase);
			writer.Write((int)touch.tapCount);

			// Added in Unity 5.4
			writer.Write(touch.radius);
			writer.Write(touch.radiusVariance);
			writer.Write((int)touch.type);
			writer.Write(touch.pressure);
			writer.Write(touch.maximumPossiblePressure);
			writer.Write(touch.azimuthAngle);
			writer.Write(touch.altitudeAngle);

			writer.EndMessage(stream);
		}
	}


	public void SendDeviceFeatures()
	{
		writer.BeginMessage(RemoteMessage.DeviceFeatures);
		writer.Write(Input.touchPressureSupported);
		writer.Write(Input.stylusTouchSupported);
		writer.EndMessage(stream);
	}


	// joystick/axis number byte incorporates flags to differentiate between joysticks/axes and if joystick buttons are sent
	const byte JOYSTICK_FLAG = 0x80;
	const byte BUTTONS_FLAG = 0x40;
	const byte JOYSTICK_NUMBER_MASK = 0x0F;
	const byte AXIS_NUMBER_MASK = 0x1F;

	private void WriteJoystick(int joystick)
	{
		uint buttonBits = 0;
		string strJoyNum = (joystick != 0 ? joystick.ToString() : "");
		int firstCode = (int)System.Enum.Parse(typeof(KeyCode), "Joystick" + strJoyNum + "Button0");
		int lastCode = (int)System.Enum.Parse(typeof(KeyCode), "Joystick" + strJoyNum + "Button19");

		for (int intCode = firstCode; intCode <= lastCode; intCode++)
		{
			if (Input.GetKey((KeyCode)intCode))
				buttonBits |= (1u << (intCode - firstCode));
		}

		bool writeButtons = (buttonBits != oldJoystickButtons[joystick]);
		bool joystickSent = false;

		oldJoystickButtons[joystick] = buttonBits;

		if (writeButtons)
		{
			joystickSent = true;
			writer.Write((byte)(JOYSTICK_FLAG | BUTTONS_FLAG | (joystick & JOYSTICK_NUMBER_MASK)));
			writer.Write(buttonBits);
		}

		for (byte axis = 0; axis < MAX_AXES; axis++)
		{
			float value = Input.GetAxis("j" + joystick + "a" + axis);
			if (oldJoystickAxes[joystick, axis] != value)
			{
				oldJoystickAxes[joystick, axis] = value;
				if (!joystickSent)
				{
					joystickSent = true;
					writer.Write((byte)(JOYSTICK_FLAG | (joystick & JOYSTICK_NUMBER_MASK)));
				}
				writer.Write((byte)(axis & AXIS_NUMBER_MASK));
				writer.Write(value);
			}
		}
	}


	public void SendJoystickInput()
	{
		writer.BeginMessage(RemoteMessage.JoystickInput);

		for (int joystick = 0; joystick < MAX_JOYSTICKS; joystick++)
		{
			WriteJoystick(joystick);
		}

		writer.EndMessage(stream);
	}


	public void SendJoystickNames()
	{
		string[] joystickNames = Input.GetJoystickNames();

		if (joystickNames.Length == 0 && oldJoystickNames.Length == 0)
			return;

		if (!Enumerable.SequenceEqual(oldJoystickNames, joystickNames))
		{
			oldJoystickNames = joystickNames;

			writer.BeginMessage(RemoteMessage.JoystickNames);
			writer.Write((byte)joystickNames.Length);

			foreach (string joystickName in joystickNames)
			{
				writer.Write(joystickName);
			}

			writer.EndMessage(stream);
		}
	}


	public void SendWebCamDeviceList(RemoteWebCamDevice[] devices)
	{
		writer.BeginMessage(RemoteMessage.WebCamDeviceList);
		writer.Write((uint)devices.Length);
		foreach (var device in devices)
		{
			writer.Write(device.device.isFrontFacing);
			writer.Write(device.name);
		}
		writer.EndMessage(stream);
	}


	public void SendWebCamStream(string name, int width, int height, byte[] image, int angle, bool verticallyMirrored)
	{
		writer.BeginMessage(RemoteMessage.WebCamStream);
		writer.Write(name);
		writer.Write((uint)width);
		writer.Write((uint)height);
		writer.Write(angle);
		writer.Write(verticallyMirrored);
		writer.Write((uint)image.Length);
		writer.Write(image);
		writer.EndMessage(stream);
	}


	public void SendLocationServiceData()
	{
		var data = new OldLocationData
		{
			isEnabledByUser = Input.location.isEnabledByUser,
			status = Input.location.status,
			lastData = Input.location.status == LocationServiceStatus.Running ? Input.location.lastData : default(LocationInfo)
		};

		if (oldLocationData.HasValue && (oldLocationData.Value.Equals(data)))
			return;

		writer.BeginMessage(RemoteMessage.LocationServiceData);
		writer.Write(data.isEnabledByUser);
		writer.Write((int)data.status);
		writer.Write(data.lastData.timestamp);
		writer.Write(data.lastData.latitude);
		writer.Write(data.lastData.longitude);
		writer.Write(data.lastData.altitude);
		writer.Write(data.lastData.horizontalAccuracy);
		writer.Write(data.lastData.verticalAccuracy);
		writer.EndMessage(stream);

		oldLocationData = data;
	}


	public void SendCompassData()
	{
		var compass = Input.compass;
		var data = new OldCompassData
		{
			enabled = compass.enabled,
			magneticHeading = compass.magneticHeading,
			trueHeading = compass.trueHeading,
			headingAccuracy = 0.0f,
			rawVector = compass.rawVector,
			timestamp = compass.timestamp,
		};

		if (oldCompassData.HasValue && (oldCompassData.Value.Equals(data)))
			return;

		writer.BeginMessage(RemoteMessage.CompassData);
		writer.Write(data.enabled);
		writer.Write(data.magneticHeading);
		writer.Write(data.trueHeading);
		writer.Write(data.headingAccuracy);
		writer.Write(data.rawVector.x);
		writer.Write(data.rawVector.y);
		writer.Write(data.rawVector.z);
		writer.Write(data.timestamp);
		writer.EndMessage(stream);

		oldCompassData = data;
	}


	public DataSender(Stream stream)
	{
		this.stream = stream;
		writer = new PacketWriter();
	}
}