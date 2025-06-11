Multithreaded Timers WPF Application This WPF application demonstrates the use of multiple timers running on separate threads, with a real-time activity log and timer displays updated on the UI. 
## Features
* **Multiple Timers:** Three timers run concurrently on separate threads. * **Accurate Timekeeping:** Each timer displays elapsed time in `hh:mm:ss` format.
* * **Real-time Activity Log:** An activity log displays timer events and timestamps.
  *  * **Thread-Safe UI Updates:** UI updates are performed safely using `Dispatcher.Invoke` and locking mechanisms.
  *    * **Start/Stop Control:** Buttons to start and stop the timers. 

## How to Use 

1. **Build the Application:** Open the project in Visual Studio and build the solution. 
2.**Run the Application:** Execute the compiled application.
  3. **Start Timers:** Click the "Start Timers" button to start the timers. The elapsed time for each timer will be displayed, and activity log entries will be generated.
4. **Stop Timers:** Click the "Stop Timers" button to stop the timers.
  
6.  ## Key Components
7.  * **MainWindow.xaml:** Defines the UI layout, including the start/stop buttons, timer displays (ListBoxes), and activity log.
    * * **MainWindow.xaml.cs:** Contains the application logic, including:
      *  * Timer creation and management using `System.Threading.Timer`.
      * * Calculation and formatting of elapsed time.
        * * Thread-safe UI updates using `Dispatcher.Invoke` and `lock`. * Activity logging.
8.  ## Threading and Synchronization
9.  * The application uses multiple threads: the main UI thread and separate thread pool threads for each timer.
    *  * `Dispatcher.Invoke` is used to marshal UI updates from the timer threads to the UI thread.
       *  * A `lock` statement with the `_logLock` object is used to protect shared resources (`ActivityLog` and `TimerDisplay`) from race conditions.
10.  ## Notes * This application demonstrates basic multithreading concepts in WPF. * The timer interval is set to 1 second for demonstration purposes. * The activity log is limited to 100 entries to prevent excessive memory usage.
