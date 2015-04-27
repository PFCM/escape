
using System;
using System.IO;
using UnityEngine;

namespace Escape.Util
{

	// Provides logging capabilities, 
	public class Logging : MonoBehaviour
	{
		public string logFileName = null;
		public bool echoToConsole = true; // double logging to console? 
		public bool logToFile = true; // actually write to the file?
		public bool absoluteTimeStamps = false; // should the time stamps be from the time this script woke up or just the actual time

		private StreamWriter outputStream = null;
		private DateTime start; // the time that the logger woke up, used subsequently for timestamps
		private bool startTimeSet = false;

		private static Logging singleton; // static instance of ourself, for wizard reasons
		static Logging instance {
			get { return singleton; }
		}

		// wake up, open the file, make the class etc
		void Awake() {
			if (singleton != null) {
				Debug.LogError("You've awoken the logger script more than once, this is probably a mistake.");
			}
			singleton = this;

			if (!startTimeSet) {
				startTimeSet = true;
				start = DateTime.Now;
			}

			// open log file
			if (logToFile) {
				if (logFileName == null || logFileName == "") {
					logFileName = string.Format ("Logs/{0:yyyy-MM-dd-H-mm-ss-fff}.txt", DateTime.Now); // default name
				}

				outputStream = new StreamWriter( logFileName ); // if the file exists it will overwrite!!
			}
		}

		// tidy up when we are done
		void OnDestroy() {
			if (outputStream != null) {
				outputStream.Close ();
				outputStream = null;
			}
		}

		// actually write a timestamped message
		// TODO: discuss the actual format of these
		private void write(string message) {
			// first add the timestamp
			if (absoluteTimeStamps) {
				message = string.Format ("[{0:H:mm:ss:fff}] {1}", DateTime.Now, message);
			} else { // relative timestamps
				TimeSpan ts = DateTime.Now - start;
				message = string.Format ("[{0}:{1}:{2}:{3}] {4}", ts.Hours,
				                         						  ts.Minutes,
				                         						  ts.Seconds,
				                         						  ts.Milliseconds,
				                         message);
			}
			if (echoToConsole) {
				Debug.Log(message);
			}
			if (logToFile) {
				outputStream.WriteLine(message);
				outputStream.Flush ();
			}
		}

		// the actual method used to do the logging
		public static void Log (string message) {
			if (Logging.instance != null) {
				Logging.instance.write (message);
			} else {
				Debug.Log("(Logging not initialised) " + message);
			}
		}
	}
}

