using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tweetinvi;
using Tweetinvi.Core.Exceptions;
using Tweetinvi.Core.Interfaces;

namespace SequencerBotTest1
{
  class TwitterHandler
  {
    // Singleton
    private TwitterHandler() {}
    static TwitterHandler() { }
    private static TwitterHandler _instance = new TwitterHandler();
    public static TwitterHandler Instance { get { return _instance; } }

    Parser parser = (Parser)Parser.Instance;
    Control textcontrol = null;

    public void LoginandStart(Control textcontrol)
    {
      this.textcontrol = textcontrol;

      try
      {
        //Auth.SetUserCredentials("CONSUMER_KEY", "CONSUMER_SECRET", "ACCESS_TOKEN", "ACCESS_TOKEN_SECRET");
        Auth.SetUserCredentials("9Xr6ZtzXFJsX1VbnKj8MGNyVq", "ffvKYeoO8tebOEfLgsBYUsyDPyTQDDlfYW8Ou7ZVoMmVOGHJXG", "716827397945827328-98tFDNfFAFhqhG6DbT9eKjPOdTrHX6Q", "VMmPvX6zUy7NxiMUsGyRKYStHFPTPXztqL9o4cSKDc9DE");
        var authenticatedUser = User.GetAuthenticatedUser();

        if (authenticatedUser == null)
        {
          MessageBox.Show("Twitter: Could not authenticate user!");
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
      stream.JsonObjectReceived += stream_JsonObjectReceived;
      stream.StartStreamAsync();
    }

    private void stream_JsonObjectReceived(object sender, Tweetinvi.Events.JsonObjectEventArgs args)
    {
      DebugDisplay("JSON received (debug): " + args.Json);
    }

    private void stream_TweetReceived(object sender, Tweetinvi.Events.TweetReceivedEventArgs args)
    {
      String received_tweet = args.Tweet.ToString();
      DebugDisplay("Tweet received: " + received_tweet);
      parser.ProcessCommand(received_tweet);
    }

    private void stream_MessageReceived(object sender, Tweetinvi.Events.MessageEventArgs args)
    {
      String received_message = args.Message.ToString();
      DebugDisplay("Message received: " + received_message);
      parser.ProcessCommand(received_message);
    }

    private void DebugDisplay(String text)
    {
      textcontrol.Invoke(new Action(() =>
          textcontrol.Text = text
      ));
    }
  }
}



// ITweet thetweet = args.Tweet;
//      String message = "Tweet received from @" + thetweet.CreatedBy.ScreenName + " - ACKNOWLEDGED";
//      DoTweet(message);