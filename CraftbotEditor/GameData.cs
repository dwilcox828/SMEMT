﻿using Gameloop.Vdf;
using Gameloop.Vdf.Linq;
using Microsoft.Win32;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CraftbotEditor
{
    //TODO: make this a generic class for different stations
    class GameData
    {
        // path strings
        public static String SteamPath; // Probably doesn't fit well, but it's going here anyways.
        public static String GamePath;
        public static String DataPath;
        public static String SurvivalPath;
        public static String RecipePath;

        // Loaded data
        public static Dictionary<String,String> item_names;
        public static List<Recipe> CraftbotDocument;

        // Background images
        public static BitmapImage MainBackground;
        public static BitmapImage SecondaryBackground;

        // GUI images
        public static BitmapImage[] ProductBackground = new BitmapImage[1];

        public static void Load()
        {
            // Gets Steam path from the registry
            try
            {
                SteamPath = Registry.CurrentUser.OpenSubKey("Software").OpenSubKey("Valve").OpenSubKey("Steam").GetValue("SteamPath").ToString();
            }
            catch
            {
                MessageBox.Show("Do you have a legitimate copy of the game?", "Steam not found!", MessageBoxButton.OK, MessageBoxImage.Error);
                Application.Current.Shutdown();
            }

            // Checks for \steamapps\appmanifest_387990.acf (387990 is SM app id)
            if (File.Exists(SteamPath + "\\steamapps\\appmanifest_387990.acf"))
                GamePath = SteamPath + "\\steamapps\\common\\Scrap Mechanic";
            else
            {
                //TODO: multi steam folder support
                /*
                // Reads \steamapps\libraryfolders.vdf
                VProperty libraryfolders = VdfConvert.Deserialize(File.ReadAllText(SteamPath + "\\steamapps\\libraryfolders.vdf"));
                // loops over available installation locations
                for (int i = 1; i<libraryfolders.Max,i++)
                {
                    if(libraryfolders[i.ToString()])
                }
                */

                // I don't currently have a secondary drive to see if this code would even work, so it's left out for the time being. -Grant
                MessageBox.Show("Either you don't have Scrap Mechanic installed, or you have it installed on another drive.\n" +
                    "This is not a bug, please do not report this.", "Game not found!", MessageBoxButton.OK, MessageBoxImage.Error);
                Application.Current.Shutdown();
            }

            // General path strings
            DataPath = GamePath + "\\Data";
            SurvivalPath = GamePath + "\\Survival";
            RecipePath = SurvivalPath + "\\CraftingRecipes";

            // Loading necessary data
            item_names = JsonConvert.DeserializeObject<Dictionary<String, String>>(File.ReadAllText(RecipePath + "\\item_names.json"));
            CraftbotDocument = JsonConvert.DeserializeObject<List<Recipe>>(File.ReadAllText(RecipePath + "\\craftbot.json"));

            // Background images
            MainBackground = GoodImage.FromFile(DataPath + "\\Gui\\Resolutions\\3840x2160\\BackgroundImages\\gui_background_craftbot.png");
            SecondaryBackground = GoodImage.FromFile(DataPath + "\\Gui\\Resolutions\\3840x2160\\BackgroundImages\\gui_background_craftbot.png");

            // GUI element images

        }
    }
}