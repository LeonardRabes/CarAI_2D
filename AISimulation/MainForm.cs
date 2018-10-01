﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Simulation;

namespace Simulation
{
    public partial class MainForm : Form
    {
        public Display SimulationDisplay;
        Engine engine;

        SettingsForm settings;

        public MainForm()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;

            settings = new SettingsForm(this);
        }

        public void ConfirmSettings()
        {
            if(SimulationDisplay != null)
            {
                SimulationDisplay.Dispose();
            }

            SimulationDisplay = new Display(settings.OverPath, settings.SimPath, settings.SpawnLocation, settings.TargetLocation);
            engine = SimulationDisplay.SimulationEngine;
            engine.SpawnRotation = settings.CarRotation;
            engine.CarWidth = settings.CarWidth;
            engine.CarLength = settings.CarLength;

            SimulationDisplay.Location = new Point(0, 0);
            SimulationDisplay.Size = new Size(this.Width, this.Height);
            SimulationDisplay.BackColor = Color.FromArgb(255, 38, 127, 0);
            this.Controls.Add(SimulationDisplay);

            this.control_panel.Enabled = true;

            SimulationDisplay.StartSimulation();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            settings.Show();
        }

        private void MainForm_SizeChanged(object sender, EventArgs e)
        {
            if(SimulationDisplay != null)
            {
                SimulationDisplay.Width = this.Width;
                SimulationDisplay.Height = this.Height;
            }
        }

        private void render_checkBox_CheckedChanged(object sender, EventArgs e)
        {
            SimulationDisplay.Render = render_checkBox.Checked;
        }

        private void grid_checkBox_CheckedChanged(object sender, EventArgs e)
        {
            SimulationDisplay.Grid = grid_checkBox.Checked;
        }

        private void save_button_Click(object sender, EventArgs e)
        {
            if(SimulationDisplay.SimulationEngine.CarRanking != null)
            {
                MemoryStream stream = SimulationDisplay.SimulationEngine.CarRanking[SimulationDisplay.SimulationEngine.CarRanking.Length - 1].Brain.BuildStructureStream();
                FileStream fstream = new FileStream(@"saves\save.carStream", FileMode.Create);
                stream.WriteTo(fstream);
                fstream.Close();
                stream.Close();

                MessageBox.Show("Car Saved", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void load_button_Click(object sender, EventArgs e)
        {
            FileStream fstream = new FileStream(@"saves\save.carStream", FileMode.Open);
            SimulationDisplay.SimulationEngine.LoadCarStructure(fstream);
            fstream.Close();
        }

        private void datails_checkBox_CheckedChanged(object sender, EventArgs e)
        {
            SimulationDisplay.Details = datails_checkBox.Checked;
        }

        private void halfspeed_button_Click(object sender, EventArgs e)
        {
            SimulationDisplay.SimulationEngine.LoopSpeed = 2.0F;
        }

        private void defaultspeed_button_Click(object sender, EventArgs e)
        {
            SimulationDisplay.SimulationEngine.LoopSpeed = 1.0F;
        }

        private void doublespeed_button_Click(object sender, EventArgs e)
        {
            SimulationDisplay.SimulationEngine.LoopSpeed = 0.5F;
        }

        private void unlimitedspeed_button_Click(object sender, EventArgs e)
        {
            SimulationDisplay.SimulationEngine.LoopSpeed = 0.0F;
        }

        private void settings_button_Click(object sender, EventArgs e)
        {
            this.control_panel.Enabled = false;
            settings.Show();
        }
    }
}
