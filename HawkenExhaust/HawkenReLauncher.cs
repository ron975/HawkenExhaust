﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;

namespace HawkenExhaust
{
    class HawkenReLauncher : SimpleGameReLauncher
    {
        public HawkenReLauncher(string gameProcessName, string gameProcessPath, string launcherProcessName, string launcherProcessPath)
            : base(gameProcessName, gameProcessPath, launcherProcessName, launcherProcessPath){
                this.OnLauncherClose += new LauncherCloseEventHandler(HawkenReLauncher_OnLauncherClose); 
        }

        /// <summary>
        /// Keep HawkenExhaust alive if there's an update that requires UAC elevation
        /// </summary>
        private void HawkenReLauncher_OnLauncherClose(object sender)
        {
                Task waitUAC = Task.Factory.StartNew(() => GetWindowHandle("User Account Control"));
                waitUAC.Wait();
                    
                if (SimpleGameReLauncher.IsProcessRunning(this.launcherProcessName))
                {
                    this.LauncherQuitListener(this.launcherProcessName);
                    return;
                }
                else
                {
                    Environment.Exit(0);
                }
        }

        /// <summary>
        /// Get whether a window with a certain name is present
        /// </summary>
        /// <param name="windowName">Name of the window shown in the title bar</param>
        /// <returns>Whether the window handle is present</returns>
        public static bool GetWindowHandle(string windowName)
        {
            foreach (Process process in Process.GetProcesses())
            {
                if (process.MainWindowTitle.Contains(windowName))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
