using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Nero
{
    public static class Sound
    {
        static SFML.Audio.Music musicDevice;                                        // Dispositivo de musica
        static List<SFML.Audio.Sound> soundDevice = new List<SFML.Audio.Sound>();   // Dispositivo de som
        static string cacheMusic = "";                                              // Cache de musica

        // Volumes
        static byte vol_Music = 100;    // Volume da musica
        static byte vol_Sound = 100;    // Volume dos soms

        /// <summary>
        /// Volume da musica
        /// </summary>
        public static byte Volume_Music
        {
            get => vol_Music;
            set
            {
                if (value >= 0 && value <= 100)
                {
                    vol_Music = value;
                    if (musicDevice != null)
                        musicDevice.Volume = value;
                }
            }
        }

        /// <summary>
        /// Volume dos soms
        /// </summary>
        public static byte Volume_Sound
        {
            get => vol_Sound;
            set
            {
                if (value >= 0 && value <= 100)
                {
                    vol_Sound = value;
                    if (soundDevice.Count > 0)
                        foreach (var i in soundDevice)
                            i.Volume = value;
                }
            }
        }

        /// <summary>
        /// Toca uma musica
        /// </summary>
        /// <param name="fileName"></param>
        public static void PlayMusic(string fileName, bool replay = true)
        {
            var filePath = Game.Path + fileName;
            if (!File.Exists(filePath))
                throw new Exception($"Nero::Sound::PlayMusic()\n");

            if (cacheMusic == fileName) return;

            StopMusic();

            musicDevice = new SFML.Audio.Music(filePath);
            musicDevice.Loop = replay;
            musicDevice.Volume = Volume_Music;
            musicDevice.Play();
            cacheMusic = fileName;
        }

        /// <summary>
        /// Para a musica atual
        /// </summary>
        public static void StopMusic()
        {
            if (musicDevice != null)
            {
                musicDevice.Stop();
                musicDevice.Dispose();
            }
        }

        /// <summary>
        /// Toca um som
        /// </summary>
        /// <param name="buffer"></param>
        public static void PlaySound(SFML.Audio.SoundBuffer buffer)
        {
            var s = new SFML.Audio.Sound(buffer);
            s.Volume = Volume_Sound;
            s.Play();
            soundDevice.Add(s);
        }

        /// <summary>
        /// Para todos os soms
        /// </summary>
        public static void StopSounds()
        {
            if (soundDevice.Count > 0)
                foreach (var i in soundDevice)
                    i.Stop();
        }

        /// <summary>
        /// Processa os soms
        /// </summary>
        internal static void ProcessSounds()
        {
            if (soundDevice.Count > 0)
            {
                var soundends = soundDevice.Where(i => i != null && i.Status == SFML.Audio.SoundStatus.Stopped).ToList();
                if (soundends.Count > 0)
                    foreach (var i in soundends)
                    {
                        i.Dispose();
                        soundDevice.Remove(i);
                    }
            }
        }


    }
}
