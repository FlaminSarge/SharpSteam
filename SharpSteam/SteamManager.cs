﻿using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using VDFParser;
using VDFParser.Models;

namespace SharpSteam
{
    public class SteamManager
    {
        public string GetSteamFolder()
        {
            RegistryKey key = Registry.LocalMachine.OpenSubKey("Software\\Valve\\Steam");
            if (key == null)
                if ((key = Registry.LocalMachine.OpenSubKey("Software\\Wow6432Node\\Valve\\Steam")) == null)
                    return null;
            return key.GetValue("InstallPath").ToString();
        }

        /// <summary>
        /// Returns all the users on userdata
        /// </summary>
        /// <param name="steamInstallPath">Steam's current installed path</param>
        /// <returns>ListString of users path</returns>
        public List<String> GetUsers(string steamInstallPath)
        {
            return Directory.GetDirectories(steamInstallPath + "\\userdata").ToList<String>();
        }

        public VDFEntry[] ReadShortcuts(string userPath)
        {
            string shortcutFile = userPath + "\\config\\shortcuts.vdf";
            VDFEntry[] shortcuts = null;

            using (StreamReader reader = new StreamReader(shortcutFile))
            {
                shortcuts = VDFParser.VDFParser.Parse(reader.ReadToEnd());
            }

            return shortcuts;
        }

        public void WriteShortcuts(VDFEntry[] vdf, string vdfPath)
        {
            byte[] result = VDFSerializer.Serialize(vdf);

            try
            {
                File.WriteAllBytes(vdfPath, result);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}