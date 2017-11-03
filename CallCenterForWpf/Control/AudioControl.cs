﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;

namespace CallCenterForWpf
{
    public class AudioControl
    {
        public WaveIn mWavIn;
        public WaveFileWriter mWavWriter;

        /// <summary>
        /// 开始录音
        /// </summary>
        /// <param name="filePath"></param>
        public void StartRecord(string filePath)
        {
            mWavIn = new WaveIn();
            mWavIn.DataAvailable += MWavIn_DataAvailable;
            mWavIn.RecordingStopped += MWavIn_RecordingStopped;
            mWavWriter = new WaveFileWriter(filePath, mWavIn.WaveFormat);
            mWavIn.StartRecording();
        }

        /// <summary>
        /// 停止录音
        /// </summary>
        public void StopRecord()
        {
            mWavIn?.StopRecording();
            mWavIn?.Dispose();
            mWavIn = null;
            mWavWriter?.Close();
            mWavWriter = null;
        }

        private void MWavIn_RecordingStopped(object sender, StoppedEventArgs e)
        {
            mWavIn?.Dispose();
            mWavIn = null;
            mWavWriter?.Close();
            mWavWriter = null;
        }

        private void MWavIn_DataAvailable(object sender, WaveInEventArgs e)
        {
            mWavWriter.Write(e.Buffer, 0, e.BytesRecorded);
            int secondsRecorded = (int)mWavWriter.Length / mWavWriter.WaveFormat.AverageBytesPerSecond;
        }

        private System.Media.SoundPlayer sp = null;
        public void PlayWav(string filePath)
        {
            sp = new System.Media.SoundPlayer();
            sp.SoundLocation = filePath;  
            sp.Load();  
            sp.Play(); 
         }

        public void StopPlay()
        {
            sp?.Stop();
            sp?.Dispose();
            sp = null;
        }
    }

}
