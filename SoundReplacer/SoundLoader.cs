﻿using SoundReplacer.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace SoundReplacer
{
    internal static class SoundLoader
    {
        public static List<string> GlobalSoundList = new List<string>();

        private static AudioClip? _cachedEmpty;

        public static void GetSoundLists()
        {
            GlobalSoundList.Add("None");
            GlobalSoundList.Add("Default");

            var folderPath = Environment.CurrentDirectory + "\\UserData\\SoundReplacer";
            if (!Directory.Exists(folderPath)) {
                Directory.CreateDirectory(folderPath);
            }

            var files = Directory.GetFiles(folderPath);
            foreach (var file in files) {
                var fileInfo = new FileInfo(file);
                if (fileInfo.Extension == ".ogg" ||
                    fileInfo.Extension == ".mp3" ||
                    fileInfo.Extension == ".wav") {
                    GlobalSoundList.Add(fileInfo.Name);
                }
            }
        }

        private static string GetFullPath(string name)
        {
            var path = Environment.CurrentDirectory + "\\UserData\\SoundReplacer\\" + name;
            var fileInfo = new FileInfo(path);
            return fileInfo.FullName;
        }

        private static UnityWebRequest GetRequest(string fullPath)
        {
            var fileUrl = "file:///" + fullPath;
            var fileInfo = new FileInfo(fullPath);
            var extension = fileInfo.Extension;
            switch (extension) {
                case ".ogg":
                    return UnityWebRequestMultimedia.GetAudioClip(fileUrl, AudioType.OGGVORBIS);
                case ".mp3":
                    return UnityWebRequestMultimedia.GetAudioClip(fileUrl, AudioType.MPEG);
                case ".wav":
                    return UnityWebRequestMultimedia.GetAudioClip(fileUrl, AudioType.WAV);
                default:
                    return UnityWebRequestMultimedia.GetAudioClip(fileUrl, AudioType.UNKNOWN);
            }
        }

        private static void ReplaceMissing(string name)
        {
            const string text = "Default";
            if (PluginConfig.Instance.GoodHitSound == name) {
                PluginConfig.Instance.GoodHitSound = text;
            }

            if (PluginConfig.Instance.BadHitSound == name) {
                PluginConfig.Instance.BadHitSound = text;
            }

            if (PluginConfig.Instance.ClickSound == name) {
                PluginConfig.Instance.ClickSound = text;
            }

            if (PluginConfig.Instance.FailSound == name) {
                PluginConfig.Instance.FailSound = text;
            }

            if (PluginConfig.Instance.SuccessSound == name) {
                PluginConfig.Instance.SuccessSound = text;
            }

            if (PluginConfig.Instance.MenuMusic == name) {
                PluginConfig.Instance.MenuMusic = text;
            }
        }

        public static AudioClip LoadAudioClip(string name)
        {
            var fullPath = GetFullPath(name);
            var request = GetRequest(fullPath);

            AudioClip? loadedAudio = null;
            var task = request.SendWebRequest();

            // while I would normally kill people for this
            // we are loading a local file, so it should be
            // basically instant success or error
            while (!task.isDone) { }

            if (request.isNetworkError || request.isHttpError) {
                Plugin.Log.Error($"Failed to load file {name} with error {request.error}");
                ReplaceMissing(name);
                return GetEmptyClip();
            }

            loadedAudio = DownloadHandlerAudioClip.GetContent(request);
            return loadedAudio;
        }

        public static AudioClip GetEmptyClip()
        {
            if (_cachedEmpty != null) {
                return _cachedEmpty;
            }

            _cachedEmpty = AudioClip.Create("Empty", 10, 1, 44100 * 2, false);
            return _cachedEmpty;
        }
    }
}
