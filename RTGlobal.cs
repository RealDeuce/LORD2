﻿using RandM.RMLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LORD2
{
    public static class RTGlobal
    {
        // Events
        public static EventHandler OnDRAWMAP = null;
        public static EventHandler OnMOVEBACK = null;
        public static EventHandler OnUPDATE = null;

        // Ref files
        public static Dictionary<string, RTRFile> RefFiles = new Dictionary<string, RTRFile>(StringComparer.OrdinalIgnoreCase);

        // World variables
        public static Dictionary<string, string> S = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        public static Dictionary<string, Int32> V = new Dictionary<string, Int32>(StringComparer.OrdinalIgnoreCase);

        // Player variables
        public static Dictionary<string, Int32> P = new Dictionary<string, Int32>(StringComparer.OrdinalIgnoreCase);
        public static Dictionary<string, byte> T = new Dictionary<string, byte>(StringComparer.OrdinalIgnoreCase);
        public static Dictionary<string, Int16> I = new Dictionary<string, Int16>(StringComparer.OrdinalIgnoreCase);

        // Item variables
        public static Dictionary<string, string> PLUS = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        // Other variables
        public static Dictionary<string, string> ReadOnlyVariables = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        public static Dictionary<string, string> LanguageVariables = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        static RTGlobal()
        {
            // Load all the ref files in the current directory
            string[] RefFileNames = Directory.GetFiles(ProcessUtils.StartupPath, "*.ref", SearchOption.TopDirectoryOnly);
            foreach (string RefFileName in RefFileNames)
            {
                LoadRefFile(RefFileName);
            }

            // Init global variables
            for (int i = 1; i <= 99; i++) I.Add("`I" + StringUtils.PadLeft(i.ToString(), '0', 2), 0);
            for (int i = 1; i <= 99; i++) P.Add("`P" + StringUtils.PadLeft(i.ToString(), '0', 2), 0);
            for (int i = 1; i <= 99; i++) PLUS.Add("`+" + StringUtils.PadLeft(i.ToString(), '0', 2), "");
            for (int i = 1; i <= 10; i++) S.Add("`S" + StringUtils.PadLeft(i.ToString(), '0', 2), "");
            for (int i = 1; i <= 99; i++) T.Add("`T" + StringUtils.PadLeft(i.ToString(), '0', 2), 0);
            for (int i = 1; i <= 40; i++) V.Add("`V" + StringUtils.PadLeft(i.ToString(), '0', 2), 0);

            // Read-only variables
            ReadOnlyVariables.Add("LOCAL", (Door.Local() ? "5" : "0"));
            ReadOnlyVariables.Add("NIL", "");
            ReadOnlyVariables.Add("RESPONCE", "0");
            ReadOnlyVariables.Add("RESPONSE", "0");

            // These are all TODOs (some need to be populated when something changes, some always need to be populated before using)
            // Variable symbols (ro) (Translated during @SHOW and @DO WRITE)
            // `C: Clears the screen and simulates 2 carriage returns. (handled in Door.Write)
            // `D: Sends a #8 (delete). (handled in Door.Write)
            ReadOnlyVariables.Add("`E", "ENEMY"); // Enemy name
            ReadOnlyVariables.Add("`G", (Door.DropInfo.Emulation == DoorEmulationType.ANSI ? "3" : "0")); // Current Graphics Level.
            // `K: Presents the more propmt and waits for ENTER to be pressed. (handled in Door.Write)
            // `L: About a half second wait. (handled in Door.Write)
            ReadOnlyVariables.Add("`N", "NAME"); // User's game name.
            // `R0 to 1R7: change background color. (handled in Door.Write)
            // `W: One tenth a second wait. (handled in Door.Write)
            // `X: Adds a space. (handled in Door.Write)
            // `1 to `%: change color. (handled in Door.Write)
            // `\: Simulates a carriage return. (handled in Door.Write)
            ReadOnlyVariables.Add("&realname", Door.DropInfo.Alias); // Real name as passed by the drop file
            ReadOnlyVariables.Add("&date", "DATE"); // The date and time like 12/12/97 format.
            ReadOnlyVariables.Add("&nicedate", "NICEDATE"); // Time AND date like 5:19 on 12/12.
            ReadOnlyVariables.Add("s&armour", "ARMOUR"); // equipped armour name.
            ReadOnlyVariables.Add("s&arm_num", "0"); // equipped armour's defensive value
            ReadOnlyVariables.Add("s&weapon", "WEAPON"); // equipped weapon name.
            ReadOnlyVariables.Add("s&wep_num", "0"); // equipped weapon's attack value.
            ReadOnlyVariables.Add("s&son", "SON"); // son/daughter, depending on current users sex
            ReadOnlyVariables.Add("s&boy", "BOY"); // boy/girl, depending on current users sex
            ReadOnlyVariables.Add("s&man", "MAN"); // man/lady, depending on current users sex
            ReadOnlyVariables.Add("s&sir", "SIR"); // sir/ma'am, depending on current users sex
            ReadOnlyVariables.Add("s&him", "HIM"); // him/her, depending on current users sex
            ReadOnlyVariables.Add("s&his", "HIS"); // his/her, depending on current users sex
            ReadOnlyVariables.Add("&money", "0"); // current users gold
            ReadOnlyVariables.Add("&bank", "0"); // current users gold in bank
            ReadOnlyVariables.Add("&lastx", "27"); // users x position before last move.
            ReadOnlyVariables.Add("&lasty", "7"); // users y position before last move - helpfull to determine which direction they came from before the hit the ref, etc.
            ReadOnlyVariables.Add("&map", "155"); // current map #
            ReadOnlyVariables.Add("&lmap", "155"); // last 'visible' map the player was on.
            ReadOnlyVariables.Add("&time", "1"); // current age of the game in days.
            ReadOnlyVariables.Add("&timeleft", "60"); // minutes the user has left in the door.
            ReadOnlyVariables.Add("&sex", "1"); // returns 0 if player is female, 1 if player is male
            ReadOnlyVariables.Add("&playernum", "0"); // the account # of the current player.
            ReadOnlyVariables.Add("&totalaccounts", "1"); // how many player accounts exist. Includes accounts marked deleted.

            // Language variables (rw) (Not translated during @SHOW or @DO WRITE)
            LanguageVariables.Add("BANK", "0"); // moola in bank
            LanguageVariables.Add("DEAD", "0"); // 1 is player is dead
            LanguageVariables.Add("ENEMY", "0"); // force `e (last monster faught) to equal a certain name
            LanguageVariables.Add("MAP", "155"); // players current block #
            LanguageVariables.Add("MONEY", "0"); // players moola
            LanguageVariables.Add("NARM", "0"); // current armour #
            LanguageVariables.Add("NWEP", "0"); // current weapon #
            LanguageVariables.Add("SEXMALE", "1"); // 1 if player is male
            LanguageVariables.Add("X", "27"); // players x cordinates
            LanguageVariables.Add("Y", "7"); // players y cordinates
        }

        private static void LoadRefFile(string fileName)
        {
            // A place to store all the sections found in this file
            RTRFile NewFile = new RTRFile(fileName);

            // Where to store the info for the section we're currently working on
            string NewSectionName = "_HEADER";
            RTRSection NewSection = new RTRSection();

            // Loop through the file
            string[] Lines = FileUtils.FileReadAllLines(fileName, RMEncoding.Ansi);
            foreach (string Line in Lines)
            {
                string LineTrimmed = Line.Trim().ToUpper();

                // Check for new section
                if (LineTrimmed.StartsWith("@#"))
                {
                    // Store last section we were working on in dictionary
                    if (NewFile.Sections.ContainsKey(NewSectionName))
                    {
                        // Section already exists, so we can't add it
                        // CASTLE4 has multiple DONE sections
                        // STONEB has multiple NOTHING sections
                        // Both appear harmless, but keep that in mind if either ever seems buggy
                    }
                    else
                    {
                        NewFile.Sections.Add(NewSectionName, NewSection);
                    }

                    // Get new section name (presumes only one word headers allowed, trims @# off start) and reset script block
                    NewSectionName = Line.Trim().Split(' ')[0].Substring(2);
                    NewSection = new RTRSection();
                }
                else if (LineTrimmed.StartsWith("@LABEL "))
                {
                    NewSection.Script.Add(Line);

                    string[] Tokens = LineTrimmed.Split(' ');
                    NewSection.Labels.Add(Tokens[1].ToUpper(), NewSection.Script.Count - 1);
                }
                else
                {
                    NewSection.Script.Add(Line);
                }
            }

            // Store last section we were working on in dictionary
            if (NewFile.Sections.ContainsKey(NewSectionName))
            {
                // Section already exists, so we can't add it
                // CASTLE4 has multiple DONE sections
                // STONEB has multiple NOTHING sections
                // Both appear harmless, but keep that in mind if either ever seems buggy
            }
            else
            {
                NewFile.Sections.Add(NewSectionName, NewSection);
            }

            RefFiles.Add(Path.GetFileNameWithoutExtension(fileName), NewFile);
        }
    }
}