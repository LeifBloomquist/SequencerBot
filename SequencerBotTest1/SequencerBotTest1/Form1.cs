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
            //Auth.SetUserCredentials("CONSUMER_KEY", "CONSUMER_SECRET", "ACCESS_TOKEN", "ACCESS_TOKEN_SECRET");
            

            try
            {
                Auth.SetUserCredentials("PjrQTBBxU792e09klffO8r4NO", "01VNgL6YjVrPe5dbHDkOepdxC23hsa32pqlL2xIGLGPyMAZfZr", "716827397945827328-98tFDNfFAFhqhG6DbT9eKjPOdTrHX6Q", "VMmPvX6zUy7NxiMUsGyRKYStHFPTPXztqL9o4cSKDc9DE");

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
              this.OutputLabel.Text = "Tweet received: " + received_message
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
            }            
        }

        private void DoVolume(string v)
        {
            int vol = (int)long.Parse(v);
            SendMIDI(ChannelCommand.Controller, 0, 1, vol);
        }

        private void DoBPM(string arg)
        {
            int bpm = (int)long.Parse(arg);
            SendMIDI(ChannelCommand.Controller, 0, 2, bpm);
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
    }
}
