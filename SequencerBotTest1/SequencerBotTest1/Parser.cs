using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sanford.Multimedia.Midi;

namespace SequencerBotTest1
{
  class Parser
  {
    // Singleton
    private Parser() {}
    static Parser() { }
    private static Parser _instance = new Parser();
    public static Parser Instance { get { return _instance; } }

    MIDIHandler midi = MIDIHandler.Instance;
    private Boolean playing = false;

    public void ProcessCommand(string text)
    {
      text = text.ToLower();
      string[] words = text.Split(' ');

      for (int i = 0; i < words.Length; i++)
      {
        if (words[i] == "volume") DoVolume(words[i + 1]);
        if (words[i] == "bpm") DoBPM(words[i + 1]);
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
      midi.SendMIDI(ChannelCommand.Controller, 0, 1, vol);
    }

    private void DoBPM(string arg)
    {
      int bpm = ParamToCC(arg);
      midi.SendMIDI(ChannelCommand.Controller, 0, 2, bpm);
    }

    private void DoKick(string arg)
    {
      int kick = ParamToCC(arg);
      midi.SendMIDI(ChannelCommand.Controller, 0, 3, kick);
    }

    private void DoBass(string arg)
    {
      int bass = ParamToCC(arg);
      midi.SendMIDI(ChannelCommand.Controller, 0, 4, bass);
    }

    private void DoSynth(string arg)
    {
      int synth = ParamToCC(arg);
      midi.SendMIDI(ChannelCommand.Controller, 0, 5, synth);
    }

    private void DoDrums(string arg)
    {
      int drums = ParamToCC(arg);
      midi.SendMIDI(ChannelCommand.Controller, 0, 6, drums);
    }

    private void DoPlay()
    {
      if (!playing)
      {
        midi.SendMMCCommand(0x02);  // Play
      }

      playing = true;
    }

    private void DoStop()
    {
      midi.SendMMCCommand(0x01);  // Stop
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
  }
}
