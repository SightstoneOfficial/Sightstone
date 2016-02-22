using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Sightstone.Helper
{
    /// <summary>
    /// Helps with playing audio files
    /// </summary>
    public class Audio
    {
        #region Class
        public Audio(string fileLocation, bool loaded = false)
        {
            FileLocation = fileLocation;
            FileName = Path.GetFileName(fileLocation);
            if (!loaded)
            {
                LoadAudio();
            }
        }
        public Audio(string fileLocation, string fileName, bool loaded = false)
        {
            FileLocation = fileLocation;
            FileName = fileName;
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
        /// <summary>
        /// The location of the file
        /// </summary>
        public string FileLocation { get; }

        /// <summary>
        /// The name of the file (alias)
        /// </summary>
        public string FileName { get; }
        #endregion fileInfo

        #region DelegatesAndEvents
        //OnFileFinishedPlaying?.Invoke(filename, filelocation);
        public event OnFileFinished OnFileFinishedPlaying;
        public delegate void OnFileFinished(string fileName, string fileLocation);

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
        

        private void LoadAudio()
        {
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
            mciSendString($"open {FileLocation} type waveaudio alias {file[0]}", null, 0, IntPtr.Zero);
        }

        private void GetAudioData()
        {
            
        }
        #endregion mci
    }
}
