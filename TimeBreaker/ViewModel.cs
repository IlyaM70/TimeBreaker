using System;
using System.ComponentModel;
using System.IO;
using System.Media;
using System.Windows.Input;
using System.Windows.Threading;


namespace TimeBreaker
{
    internal class ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        void PropertyChanging(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private DispatcherTimer _timer;

        // private static string _pathToSound = "sound\\small-bell-ring-01a.wav";
        private static UnmanagedMemoryStream _preBreakSignal = Properties.Resources.small_bell_ring_01a;
        private static UnmanagedMemoryStream _breakSignal = Properties.Resources.mixkit_home_standard_ding_dong_109;
        private static UnmanagedMemoryStream _breakEndSignal = Properties.Resources.TD6K_219_Bell_73_SP;
        private static UnmanagedMemoryStream _breakMusic= Properties.Resources.Elevator_music;
        private static UnmanagedMemoryStream _testSignal= Properties.Resources.bell_ringing_03a;
        //private SoundPlayer _player;
        public ViewModel()
        {
            //timer
            _timer = new DispatcherTimer();
            _timer.Tick += new EventHandler(timer_Tick);
            _timer.Interval = new TimeSpan(0, 0, 1);
            //sound player
            //_player = new SoundPlayer();

        }


        private static string _workTime = "00:00:32";
        private static string _breakTime = "00:00:20";
        private static string _preBreakSignalTime = "00:00:30";
        private static bool _isBreak = false;






        private string _time = _workTime;
        public string Time
        {
            get { return _time; }
            set
            {
                _time = value;
                PropertyChanging("Time");

            }
        }

        private string _status = "Status";
        public string Status
        {
            get { return _status; }
            set
            {
                _status = value;
                PropertyChanging("Status");
            }
        }
        //TO DO
        // 1 Pause Unpause - DONE
        // 2 Reset - DONE
        // 3 Work time changing into break time - DONE
        // 4 Take break now - DONE
        // and skip break - DONE
        // 5 Status - DONE
        // 6 Merge Buttons - DONE
        //7 Sounds
        //     . 30 seconds before break - DONE
        //  . break signal - DONE
        //     . break end signal - DONE
        //     .relaxing music during break - DONE


        private string _startPauseContent = "Start";
        public string StartPauseContent
        {
            get { return _startPauseContent; }
            set
            {
                _startPauseContent = value;
                PropertyChanging("StartPauseContent");
            }
        }
        public ICommand StartPause
        {
            get
            {
                return new ButtonsCommand(
              () =>
              {
                  if (!_timer.IsEnabled)
                  {
                      _timer.Start();
                      Status = "Working...";
                      StartPauseContent = "Pause";
                  }
                  else
                  {
                      _timer.Stop();
                      Status = "TIMER ON PAUSE";
                      StartPauseContent = "Start";
                  }

              });
            }
        }

        public ICommand Reset
        {
            get
            {
                return new ButtonsCommand(
              () =>
              {
                  _timer.Stop();
                  Time = _workTime;
                  Status = "TIMER RESETED";
                  StartPauseContent = "Start";
                  if (_isBreak) _isBreak = false;

              });
            }
        }

        private string _breakNowSkipContent = "Take break now";
        public string BreakNowSkipContent
        {
            get { return _breakNowSkipContent; }
            set
            {
                _breakNowSkipContent = value;
                PropertyChanging("BreakNowSkipContent");
            }
        }
        public ICommand BreakNowSkip
        {
            get
            {
                return new ButtonsCommand(
              () =>
              {
                  if (_timer.IsEnabled && _isBreak == false)
                  {
                      _isBreak = true;
                      Time = _breakTime;
                      Status = "!!!!! BREAK TIME !!!!!";
                      BreakNowSkipContent = "Skip break";
                  }
                  else
                  {
                      _isBreak = false;
                      Time = _workTime;
                      Status = "Working...";
                      BreakNowSkipContent = "Take break now";
                  }

              });
            }
        }




        private void timer_Tick(object sender, EventArgs e)
        {
            if (Time == "00:00:00" && !_isBreak == true)
            {
                _isBreak = true;
                Time = _breakTime;
                Status = "!!!!! BREAK TIME !!!!!";

            }
            else if (Time == "00:00:00" && !_isBreak == false)
            {
                _isBreak = false;
                Time = _workTime;
                Status = "Working...";
            }

            ///// Pre Break Signal

            if (Time == _preBreakSignalTime && _isBreak == false)
            {
                //_player.Stop();
                //_player.Stream = _preBreakSignal;
                //_player.Load();
                //_player.Play();
                _preBreakSignal.Position = 0;
                using (SoundPlayer sound = new SoundPlayer(_preBreakSignal)) {
                    sound.Play();
                }



            }

            ///// Break Signal

            if ((Time == "00:00:00" && _isBreak == false) || (Time == _breakTime && _isBreak == true))
            {
                //_player.Stop();
                //_player.Stream = _breakSignal;
                //_player.Load();
                //_player.Play();
                _breakSignal.Position = 0;
                using (SoundPlayer sound = new SoundPlayer(_breakSignal))
                {
                    sound.Play();
                }

            }

            ///// Break End Signal

            if ((_isBreak == true & Time == "00:00:01"))
            {

                _breakEndSignal.Position = 0;
                using (SoundPlayer sound = new SoundPlayer(_breakEndSignal))
                {
                    sound.Play();
                }

            }

            ///// Break Music
           
            if ((_isBreak == true & Time == _breakTime))
            {
                System.Threading.Thread.Sleep(2000);
                _breakMusic.Position = 0;
                using (SoundPlayer sound = new SoundPlayer(_breakMusic))
                {
                    sound.PlayLooping();
                }

            }



            int intTime = StringToSeconds(Time);
            intTime--;
            Time = PrintTimeSpan(intTime);

        }

        




//public static string PrintTimeSpan(int secs)
//{
//    TimeSpan t = TimeSpan.FromSeconds(secs);
//    string answer;
//    if (t.TotalMinutes < 1.0)
//    {
//        answer = String.Format("{0}", t.Seconds);
//    }
//    else if (t.TotalHours < 1.0)
//    {
//        answer = String.Format("{0}:{1:D2}", t.Minutes, t.Seconds);
//    }
//    else // more than 1 hour
//    {
//        answer = String.Format("{0}:{1:D2}:{2:D2}", (int)t.TotalHours, t.Minutes, t.Seconds);
//    }

//    return answer;
//}
public static string PrintTimeSpan(int secs)
        {
            TimeSpan t = TimeSpan.FromSeconds(secs);
            string answer;

            answer = String.Format("{0:D2}:{1:D2}:{2:D2}", (int)t.TotalHours, t.Minutes, t.Seconds);


            return answer;
        }
        public static int StringToSeconds(string str)
        {
            TimeSpan ts = TimeSpan.Parse(str);
            int seconds = (int)ts.TotalSeconds;
            return seconds;
        }


    }
}
