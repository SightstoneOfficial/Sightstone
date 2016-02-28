using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace Sightstone.Helper
{
    /// <summary>
    /// Helps with playing audio files
    /// </summary>
    public class Audio : IDisposable
    {
        #region Class
        public Audio(string fileLocation, bool loaded = false)
        {
            FileLocation = fileLocation;
            FileName = Path.GetFileName(fileLocation);
            GetAudioData();
            if (!loaded)
            {
                LoadAudio();
            }
        }
        public Audio(string fileLocation, string fileName, bool loaded = false)
        {
            FileLocation = fileLocation;
            FileName = fileName;
            GetAudioData();
            if (!loaded)
            {
                LoadAudio();
            }
        }

        public Audio(string fileLocation, string fileName, AudioSettings settings, bool loaded = false)
        {
            FileLocation = fileLocation;
            FileName = fileName;
            AudioSettings = settings;
            GetAudioData();
            if (!loaded)
            {
                LoadAudio();
            }
        }

        public static Audio LoadPreviouslyLoadedSound(string fileLocation, string fileName)
        {
            return new Audio(fileLocation, fileName, true);
        }
        #endregion

        #region fileInfo
        public static List<string> LoadedFiles { get; } = new List<string>();

        /// <summary>
        /// The location of the file
        /// </summary>
        public string FileLocation { get; }

        /// <summary>
        /// The name of the file (alias)
        /// </summary>
        public string FileName { get; }

        public int FileLength { get; private set; }

        public AudioSettings AudioSettings { get; } = new AudioSettings();

        private System.Timers.Timer _timer;
        #endregion fileInfo

        #region DelegatesAndEvents
        //OnFileFinishedPlaying?.Invoke(filename, filelocation);
        public event OnFileFinished OnFileFinishedPlaying;
        public delegate void OnFileFinished(string fileName, string fileLocation);

        public event OnFileLoop OnFileLoopPlaying;
        public delegate void OnFileLoop(string fileName, string fileLocation);

        public event OnFileStart OnFileStartPlaying;
        public delegate void OnFileStart(string fileName, string fileLocation);
        #endregion DelegatesAndEvents

        #region mci
        /// <summary>
        /// The mciSendString function sends a command string to an MCI device.
        /// </summary>
        /// <param name="command">Pointer to a null-terminated string that specifies an MCI command string.</param>
        /// <param name="returnValue">Pointer to a buffer that receives return information. If no return information is needed, this parameter can be NULL.</param>
        /// <param name="returnLength">Size, in characters, of the return buffer specified by the lpszReturnString parameter.</param>
        /// <param name="winHandle">Handle to a callback window if the "notify" flag was specified in the command string.</param>
        /// <returns></returns>
        [DllImport("winmm.dll")]
        private static extern long mciSendString(string command, StringBuilder returnValue, int returnLength, IntPtr winHandle);
        
        // ReSharper disable once InconsistentNaming
        private static void mciSendString(string command)
        {
            mciSendString(command, null, 0, IntPtr.Zero);
        }

        private void LoadAudio()
        {
            LoadedFiles.Add(FileLocation);
            var file = FileName.Split('.');
            switch (file[1])
            {
                case "mp3":
                    file[1] = "mpegvideo";
                    break;
                case "wav":
                    file[1] = "waveaudio";
                    break;

            }
            mciSendString($"open {FileLocation} type waveaudio alias {FileName}");
        }

        private void GetAudioData()
        {
            var lengthBuf = new StringBuilder(32);
            mciSendString("status wave length", lengthBuf, lengthBuf.Capacity, IntPtr.Zero);
            int length;
            int.TryParse(lengthBuf.ToString(), out length);
            FileLength = length;
            _timer = new System.Timers.Timer(FileLength);
        }

        public void Play()
        {
            OnFileStartPlaying?.Invoke(FileName, FileLocation);
            mciSendString($"play {FileName}", null, 0, IntPtr.Zero);
            _timer.AutoReset = true;
            _timer.Elapsed += (sender, args) =>
            {
                if (!AudioSettings.LoopAudio || AudioSettings.LoopAmount == -1)
                {
                    _timer.Stop();
                    OnFileFinishedPlaying?.Invoke(FileName, FileLocation);
                }
                else
                {
                    if (AudioSettings.LoopAmount <= 0)
                    {
                        OnFileFinishedPlaying?.Invoke(FileName, FileLocation);
                    }
                    else
                    {
                        OnFileLoopPlaying?.Invoke(FileName, FileLocation);
                        AudioSettings.LoopAmount--;
                        Play();
                    }
                }
            };
        }

        public void Pause()
        {
            mciSendString($"pause {FileName}");
            _timer.Stop();
        }

        public void Stop()
        {
            
        }

        public void Dispose()
        {
            mciSendString($"close {FileLocation}", null, 0, IntPtr.Zero);
            _timer.Dispose();
        }
        #endregion mci
    }

    public class AudioSettings
    {
        public bool LoopAudio { get; set; } = false;

        public int LoopAmount { get; set; } = -1;

        public int StartTime { get; set; } = 0;

        public int EndTime { get; set; } = 0;
    }
}
