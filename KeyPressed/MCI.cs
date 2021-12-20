using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace KeyPressed
{
    public class MciAudio
    {
        public MciAudio()
        {
        }

        public static int ik = 1;

        public string k = "2";

        //internal  string SoundRecordingDeviceId = "soundRecordingDevice"; // any valid identifier
        public  string SoundRecordingDeviceId = "soundRecordingDevice"; // any valid identifier
        public  string PlayMediaFile = "";

         string OpenCommandFormat = "open new type waveaudio alias {0}";
         string RecordCommandFormat = "record {0}";
         string PauseCommandFormat = "pause {0}";
         string StopCommandFormat = "stop {0}";         
         string CloseCommandFormat = "close {0}";                  
        
         string SaveCommandFormatFormat = @"save {0} ""{{0}}""";        

        [DllImport("winmm.dll")]
        private  static extern long mciSendString(
            string strCommand, StringBuilder strReturn, int iReturnLength, IntPtr oCallback);

        public void Initialize()
        {
            ik++;
            k = ik.ToString();
        }

        internal  void Open()
        {
            string OpenRecorderCommand = string.Format(OpenCommandFormat, SoundRecordingDeviceId);

            mciSendString(OpenRecorderCommand, null, 0, IntPtr.Zero);
        } //Open

        internal  void Record()
        {
            string RecordCommand = string.Format(RecordCommandFormat, SoundRecordingDeviceId);
            mciSendString(RecordCommand, null, 0, IntPtr.Zero);
        } //Record

        internal  void Pause()
        {
            string PauseCommand = string.Format(PauseCommandFormat, SoundRecordingDeviceId);

            mciSendString(PauseCommand, null, 0, IntPtr.Zero);
        } //Pause

        internal  void Play()
        {
            string PlayCommandFormat = "play MediaFile"+k+" notify";
            string PlayCommand = string.Format(PlayCommandFormat, PlayMediaFile);

            mciSendString(PlayCommand, null, 0, IntPtr.Zero);
        } //Play

        internal  void OpenPlay()
        {
            string OpenPlayCommandFormat = "open \"{0}\" type mpegvideo alias MediaFile"+k;

            string OpenPlayCommand = string.Format(OpenPlayCommandFormat, PlayMediaFile);

            mciSendString(OpenPlayCommand, null, 0, IntPtr.Zero);
        } //OpenPlay

        internal  void Stop()
        {
            string StopCommand = string.Format(StopCommandFormat, SoundRecordingDeviceId);

            mciSendString(StopCommand, null, 0, IntPtr.Zero);
        } //Stop

        internal  void StopPlay()
        {
            string StopPlayCommandFormat = "stop MediaFile" + k;

            string StopPlayCommand = string.Format(StopPlayCommandFormat, PlayMediaFile);

            mciSendString(StopPlayCommand, null, 0, IntPtr.Zero);
        } //StopPlay

        internal  void Close()
        {
            string CloseRecorderCommand = string.Format(CloseCommandFormat, SoundRecordingDeviceId);

            mciSendString(CloseRecorderCommand, null, 0, IntPtr.Zero);
        } //Close

        internal  void ClosePlay()
        {
            string ClosePlayCommandFormat = "close MediaFile" + k;

            string ClosePlayCommand = string.Format(ClosePlayCommandFormat, PlayMediaFile);

            mciSendString(ClosePlayCommand, null, 0, IntPtr.Zero);
        } //ClosePlay

        internal  void SaveRecording(string fileName)
        {
            string SaveCommandFormat = string.Format(SaveCommandFormatFormat, SoundRecordingDeviceId);

            mciSendString(string.Format(SaveCommandFormat, fileName), null, 0, IntPtr.Zero);
        } //SaveRecording

        internal  void SetVolume(string volume)
        {
            mciSendString("setaudio something volume to " + volume, null, 0, IntPtr.Zero);
        }

        internal  bool IsPlaying()
        {
            string cmd = "status MediaFile"+k+" mode";

            StringBuilder returnData = new StringBuilder();

            mciSendString(cmd, returnData, 128, IntPtr.Zero);

            if (returnData.Length == 7 &&
                      returnData.ToString().Substring(0, 7) == "playing")
                return true;
            else
                return false;
        }
    }
}
