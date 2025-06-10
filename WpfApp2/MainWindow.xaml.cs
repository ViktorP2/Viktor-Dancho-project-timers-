 using System;
 using System.Collections.ObjectModel;
 using System.ComponentModel;
 using System.Threading;
 using System.Windows;
 using System.Windows.Controls;
 using System.Windows.Data;
 using System.Windows.Threading;
 

 namespace WpfApp1
 {
  public partial class MainWindow : Window, INotifyPropertyChanged
  {
  private const int NumberOfTimers = 3;
  private readonly Timer[] _timers = new Timer[NumberOfTimers];
  private readonly TimeSpan[] _elapsedTime = new TimeSpan[NumberOfTimers];
  private readonly object _logLock = new object();
  private bool _timersRunning = false;
  private DateTime _startTime;
 

  public ObservableCollection<string> ActivityLog { get; } = new ObservableCollection<string>();
  public ObservableCollection<string> TimerDisplay { get; } = new ObservableCollection<string>();
 

  public event PropertyChangedEventHandler PropertyChanged;
 

  protected virtual void OnPropertyChanged(string propertyName)
  {
  PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
  }
 

  public MainWindow()
  {
  InitializeComponent();
  DataContext = this;
  BindingOperations.EnableCollectionSynchronization(ActivityLog, _logLock);
  BindingOperations.EnableCollectionSynchronization(TimerDisplay, _logLock);
 

  for (int i = 0; i < NumberOfTimers; i++)
  {
  TimerDisplay.Add($"Timer {i}: 00:00:00");
  _elapsedTime[i] = TimeSpan.Zero;
  }
  }
 

  private void StartTimers()
  {
  if (_timersRunning) return;
 

  _startTime = DateTime.UtcNow; // Capture the start time
 

  for (int i = 0; i < NumberOfTimers; i++)
  {
  int timerId = i;
  _elapsedTime[i] = TimeSpan.Zero; // Reset elapsed time
  _timers[i] = new Timer(TimerCallback, timerId, 0, 1000); // Tick every 1 second
  Log($"Timer {timerId} started.");
  }
  _timersRunning = true;
  UpdateButtonStates();
  }
 

  private void StopTimers()
  {
  if (!_timersRunning) return;
 

  for (int i = 0; i < NumberOfTimers; i++)
  {
  _timers[i]?.Dispose();
  _timers[i] = null;
  Log($"Timer {i} stopped.");
  }
  _timersRunning = false;
  UpdateButtonStates();
  }
 

  private void TimerCallback(object state)
  {
  int timerId = (int)state;
 

  // Calculate elapsed time
  _elapsedTime[timerId] = DateTime.UtcNow - _startTime;
 

  // Format the elapsed time
  string formattedTime = _elapsedTime[timerId].ToString(@"hh\:mm\:ss");
 

  string message = $"Timer {timerId}: Tick at {DateTime.Now:HH:mm:ss} - Elapsed: {formattedTime}";
  Log(message);
 

  // Update the timer display
  Application.Current.Dispatcher.Invoke(() =>
  {
  lock (_logLock)
  {
  TimerDisplay[timerId] = $"Timer {timerId}: {formattedTime}";
  }
  });
  }
 

  private void Log(string message)
  {
  Application.Current.Dispatcher.Invoke(() =>
  {
  lock (_logLock)
  {
  ActivityLog.Add(message);
  if (ActivityLog.Count > 100)
  {
  ActivityLog.RemoveAt(0);
  }
  }
  });
  }
 

  private void StartButton_Click(object sender, RoutedEventArgs e)
  {
  StartTimers();
  }
 

  private void StopButton_Click(object sender, RoutedEventArgs e)
  {
  StopTimers();
  }
 

  private void UpdateButtonStates()
  {
  StartButton.IsEnabled = !_timersRunning;
  StopButton.IsEnabled = _timersRunning;
  }
 

  protected override void OnClosed(EventArgs e)
  {
  base.OnClosed(e);
  StopTimers();
  }
  }
 }
 