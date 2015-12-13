using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Timers;

namespace BilDestek
{
    class Task
    {
        public int id = -1;
        public String taskname;
        public int delay;
        public String param1;
        public String param2;
        public String message;
        public Computer comp;
        public String result;
        public bool keep;
        
        public Task(Computer comp, int id,String taskname, int delay, String param1, String param2, String message,bool keep)
        {
            this.comp = comp;
            this.taskname = taskname;
            this.delay = (delay <0 ) ? 0 :delay;
            this.param1 = param1;
            this.param2 = param2;
            this.message = message;
            this.keep = keep;
            this.result = "--";
        }
        public bool isReal() {
            if (this.id == -1)
                return false;
            return true;
        }
        public void run() {
            if (this.id == -1) {
                return;
            }
            Timer t = new Timer(delay*1000);
            t.Elapsed += t_Elapsed;
            t.Start();
             
            if(this.message.Length>1)
            {
                Timer t2 = new Timer(500);
                t2.Elapsed += t2_Elapsed;
                t2.Start();
                if(this.keep && this.param1 == "endProcess")
                    keepDoing();
            }
        }

        void t2_Elapsed(object sender, ElapsedEventArgs e)
        {
            ((Timer)sender).Stop();
            System.Windows.Forms.MessageBox.Show(this.message, "Özyurt Tekstil - Bilgi İşlem");
        }
        void t_Elapsed(object sender, ElapsedEventArgs e)
        {
            action();
            ((Timer)sender).Stop();
        }
        private void action() {
            switch (this.taskname)
            {
                case "endProcess": endProcess(this.param1); break;
                case "reboot": reboot(this.param1); break;
                case "checkOnline": break;
                case "shutdown": shutdown(this.param1); break;
                case "command": ExecuteCommandSync(this.param1); break;
                default:
                    break;
            }
        }

        private void endProcess(String param1) {
            foreach (var process in Process.GetProcessesByName(param1))
            {
                process.Kill();
            }
        }
        
        private void showMessage() { 
            
        }
        public void ExecuteCommandSync(string command)
        {
            try
            {
                string cmd = "/c " + command;
                System.Diagnostics.Process proc = new System.Diagnostics.Process();
                proc.StartInfo.FileName = "cmd.exe";
                proc.StartInfo.Arguments = cmd;
                proc.StartInfo.UseShellExecute = false;
                proc.StartInfo.RedirectStandardOutput = true;
                proc.Start();
            }
            catch (Exception objException)
            {
                //throw objException;
            }
        }

        private void reboot(String param1)
        {
            if (String.IsNullOrEmpty(param1) || String.IsNullOrWhiteSpace(param1))
                param1 = "30";
            System.Diagnostics.Process.Start("shutdown.exe", "-r -t " + param1);
        }
        private void shutdown(String param1)
        {
            if (String.IsNullOrEmpty(param1) || String.IsNullOrWhiteSpace(param1))
                param1 = "30";
            System.Diagnostics.Process.Start("shutdown.exe", "-s -t "+param1);
        }

        int i = 0;
        int timeToKeep = 3; //in minutes
        int keepInterval = 3000; //in miliseconds
        private void keepDoing() {
            if ((timeToKeep * 60000) - (i * keepInterval) > 0)
            {
                i++;
                Timer t3 = new Timer(keepInterval);
                t3.Start();
                t3.Elapsed += t3_Elapsed;
            }
        }

        void t3_Elapsed(object sender, ElapsedEventArgs e)
        {
            this.action();
            ((Timer)sender).Stop();
            keepDoing();
        }
    }
}
