using Sanford.Multimedia.Midi;
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
using Tweetinvi;
using Tweetinvi.Core.Exceptions;
using Tweetinvi.Core.Interfaces;

namespace SequencerBotTest1
{
    public partial class Form1 : Form
    {
        private OutputDevice outDevice = null;
        private SynchronizationContext context;

        private Boolean connected = false;

        private Boolean playing = false;

        private System.Timers.Timer myTimer = new System.Timers.Timer();

        public Form1()
        {
            InitializeComponent();
            InitializeMIDI();
            LoginandStart();
            OutputLabel.Text = "Sequencer Bot ready.";
        }

        private void InitializeMIDI()
        {
            connected = false;

            int devices = OutputDevice.DeviceCount;

 	        if (devices == 0)
            {
                MessageBox.Show("No MIDI output devices available.", "Warning!",  MessageBoxButtons.OK, MessageBoxIcon.Warning);                
            }
            else
            {
                try
                {
                    context = SynchronizationContext.Current;                    
                   
                    for (int i=0; i< devices; i++)
                    {
                        MidiOutCaps caps = OutputDevice.GetDeviceCapabilities(i);                        
                        if (caps.name == "Generator")
                        {
                            ConnectMIDI(i);
                            return;
                        }
                    }

                    MessageBox.Show("Generator device not found!", "MIDI Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    Close();
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message, "MIDI Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    Close();
                }
            }
        }

        private void ConnectMIDI(int selected)
        {
            if (outDevice != null)
            {
                outDevice.Close();
            }

            outDevice = new OutputDevice(selected);            
            outDevice.Reset();            
            connected = true;
        }

        private void LoginButton_Click(object sender, EventArgs e)
        {
            //LoginandStart();
        }

        private void LoginandStart()
        {
            try
            {
                //Auth.SetUserCredentials("CONSUMER_KEY", "CONSUMER_SECRET", "ACCESS_TOKEN", "ACCESS_TOKEN_SECRET");
                
                var authenticatedUser = User.GetAuthenticatedUser();

                if (authenticatedUser == null)
                {
                    MessageBox.Show("Could not authenticate user!");
                    Environment.Exit(-1);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception authenticating user! " + ex.Message);
                Environment.Exit(-2);
            }

            var stream = Stream.CreateUserStream();
            stream.TweetCreatedByAnyoneButMe += stream_TweetReceived;
            stream.MessageReceived += stream_MessageReceived;
            stream.StartStreamAsync();    
        }


        private void stream_TweetReceived(object sender, Tweetinvi.Events.TweetReceivedEventArgs args)
        {
          String received_tweet = args.Tweet.ToString();

          Invoke(new Action(() =>
              this.OutputLabel.Text = "Tweet received: " + received_tweet
          ));


          ProcessCommand(received_tweet);

          // ITweet thetweet = args.Tweet;
          //      String message = "Tweet received from @" + thetweet.CreatedBy.ScreenName + " - ACKNOWLEDGED";
          //      DoTweet(message);
        }


        private void stream_MessageReceived(object sender, Tweetinvi.Events.MessageEventArgs args)
        {
          String received_message = args.Message.ToString();


          Invoke(new Action(() =>
              this.OutputLabel.Text = "Message received: " + received_message
          ));


          ProcessCommand(received_message);

          // ITweet thetweet = args.Tweet;
          //      String message = "Tweet received from @" + thetweet.CreatedBy.ScreenName + " - ACKNOWLEDGED";
          //      DoTweet(message);
        }

        private void ProcessCommand(string text)
        {
            text = text.ToLower();
            string[] words = text.Split(' ');

            for (int i=0; i < words.Length; i++)
            { 
                if (words[i] == "volume") DoVolume(words[i+1]); 
                if (words[i] == "bpm") DoBPM(words[i+1]); 
                if (words[i] == "kick") DoKick(words[i + 1]);
                if (words[i] == "bass") DoBass(words[i + 1]);
                if (words[i] == "synth") DoSynth(words[i + 1]);
                if (words[i] == "drums") DoDrums(words[i + 1]);
                if (words[i] == "stop") DoStop();
                if (words[i] == "play") DoPlay();
            }            
        }

        private void DoVolume(string v)
        {
           int vol = ParamToCC(v);
           SendMIDI(ChannelCommand.Controller, 0, 1, vol);
        }

        private void DoBPM(string arg)
        {
          int bpm = ParamToCC(arg);
          SendMIDI(ChannelCommand.Controller, 0, 2, bpm);
        }

        private void DoKick(string arg)
        {
          int kick = ParamToCC(arg);
          SendMIDI(ChannelCommand.Controller, 0, 3, kick);
        }

        private void DoBass(string arg)
        {
          int bass = ParamToCC(arg);
          SendMIDI(ChannelCommand.Controller, 0, 4, bass);
        }

        private void DoSynth(string arg)
        {
          int synth = ParamToCC(arg);
          SendMIDI(ChannelCommand.Controller, 0, 5, synth);
        }

        private void DoDrums(string arg)
        {
          int drums = ParamToCC(arg);
          SendMIDI(ChannelCommand.Controller, 0, 6, drums);
        }

        private void DoPlay()
        {
          if (!playing)
          {
            SendMMCCommand(0x02);  // Play
          }

          playing = true;
        }

        private void DoStop()
        {
           SendMMCCommand(0x01);  // Stop
           playing = false;
        }

        private int ParamToCC(string arg)
        {
          int val = (int)long.Parse(arg);
          float f = (float)val / 100f;
          int cc = (int)(f * 127f);

          if (cc < 0) cc = 0;
          if (cc > 127) cc = 127;

          return cc;
        }
   
        private void DoTweet(String s)
        {
            var atweet = Tweet.PublishTweet(s);
        }

        private void SendMIDI(ChannelCommand command, int channel, int data1, int data2)
        {
            if (!connected) return;

            ChannelMessage cm = new ChannelMessage(command, channel, data1, data2);
            outDevice.Send(cm);
        }

        private void SendMMCCommand(byte command)
        {
          byte[] MMC = new byte[6];

          MMC[0] = 0xF0;
          MMC[1] = 0x7F;
          MMC[2] = 0x7F;
          MMC[3] = 0x06;
          MMC[4] = command;
          MMC[5] = 0xF7;

          SysExMessage sysex = new SysExMessage(MMC);
          outDevice.Send(sysex);
        }
    }
}
