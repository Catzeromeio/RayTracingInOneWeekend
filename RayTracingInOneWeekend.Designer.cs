using System;
using System.Threading;
using System.Windows.Forms;

namespace RayTracingInOneWeekend
{
    partial class RayTracingInOneWeekend
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>

        Thread thread;
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(Screen.Width, Screen.Height);
            this.Text = "Ray Tracing In One Weekend";

            GameMain.GameInit(this);
            thread = new Thread(new ThreadStart(GameMain.GameMainProc));
            thread.Start();
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);

            if (thread == null)
                return;
            thread.Abort();


            Screen.Width = ClientSize.Width;
            Screen.Height = ClientSize.Height;
            GameMain.GameInit(this);

            thread = new Thread(new ThreadStart(GameMain.GameMainProc));
            thread.Start();
        }
        #endregion
    }
}

