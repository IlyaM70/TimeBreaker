using System;
using System.ComponentModel;
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
        private SoundPlayer _player;
        public ViewModel()
        {
            //timer
            _timer = new DispatcherTimer();
            _timer.Tick += new EventHandler(timer_Tick);
            _timer.Interval = new TimeSpan(0, 0, 1);
            //sound player
            _player = new SoundPlayer(_pathToSound);
            _player.Load();

        }


        private static string _workTime = "00:00:32";
        private static string _breakTime = "00:00:30";
        private static bool _isBreak = false;
        private static string _pathToSound = "C:\\Users\\iliya\\source\\repos\\C#\\TimeBreaker\\TimeBreaker\\sound\\small-bell-ring-01a.wav";


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
        //     . 30 seconds before break
        //  . break signal
        //     . break end signal
        //     .relaxing music during break


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

            if (Time == "00:30:00" && !_isBreak == true)
            {
                _player.Play();
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
