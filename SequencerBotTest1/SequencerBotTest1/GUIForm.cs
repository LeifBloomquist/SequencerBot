using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SequencerBotTest1
{
    public partial class GUIForm : Form
    {
        private System.Timers.Timer myTimer = new System.Timers.Timer();
       
        TwitterHandler twitter = TwitterHandler.Instance;
        MIDIHandler midi = MIDIHandler.Instance;

        public GUIForm()
        {
            InitializeComponent();            
            midi.InitializeMIDI();
            twitter.LoginandStart(OutputLabel);
            OutputLabel.Text = "Sequencer Bot ready.";
        }

        private void GUIForm_FormClosed(object sender, FormClosedEventArgs e)
        {
          Environment.Exit(0);
        }             
    }
}
