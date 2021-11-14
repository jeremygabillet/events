using System;
using System.IO;

namespace EventTest
{
    /// <summary>
    /// Main program
    /// </summary>
    /// <remarks>
    /// This class is the <see cref="LogEvent"/> event publisher
    /// </remarks>
    class Program
    {
        public event EventHandler<SendLogEventArgs> OnSendLog;

        static void Main(string[] args)
        {
            Program program = new Program();
            StandardOutputLogger standardLogger = new StandardOutputLogger();
            FileOutputLogger fileLogger = new FileOutputLogger();
            // Subscribe the logger to OnSendLog event
            standardLogger.Subscribe(program);
            fileLogger.Subscribe(program);
            SendLogEventArgs eventArgs = new SendLogEventArgs("LogEvent published", DateTime.Now);
            // Dispatch OnSendLog subscribed loggers
            if (program.OnSendLog != null)
            {
                program.OnSendLog(program, eventArgs);
            }
        }
    }

    /// <summary>
    /// Logs messages to the standard output
    /// </summary>
    /// <remarks>
    /// This class is a subscriber for <see cref="LogEvent"/>
    /// </remarks>
    class StandardOutputLogger
    {
        /// <summary>
        /// Subscribe to the log publisher
        /// </summary>
        public void Subscribe(Program program)
        {
            program.OnSendLog += OnLogSent;
        }

        /// <summary>
        /// Delegate called when a log is sent by the publisher
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void OnLogSent(object sender, SendLogEventArgs args)
        {
            // When OnLogSent is called, it writes a log on the stream
            Write(args.Message, args.DateTime);
        }

        /// <summary>
        /// Write a log to the standard output
        /// </summary>
        /// <param name="message"></param>
        /// <param name="dateTime"></param>
        public void Write(String message, DateTime? dateTime = null)
        {
            if (dateTime == null)
            {
                dateTime = DateTime.Now;
            }
            String formattedMessage = String.Format($"{dateTime} - {message}\n");
            Console.WriteLine(formattedMessage);
        }
    }

    class FileOutputLogger
    {
        /// <summary>
        /// Subscribe to the log publisher
        /// </summary>
        public void Subscribe(Program program)
        {
            program.OnSendLog += OnLogSent;
        }

        /// <summary>
        /// Delegate called when a log is sent by the publisher
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void OnLogSent(object sender, SendLogEventArgs args)
        {
            // When OnLogSent is called, it writes a log on the stream
            Write(args.Message, args.DateTime);
        }

        /// <summary>
        /// Write a log to the standard output
        /// </summary>
        /// <param name="message"></param>
        /// <param name="dateTime"></param>
        public void Write(String message, DateTime? dateTime = null)
        {
            if (dateTime == null)
            {
                dateTime = DateTime.Now;
            }
            String formattedMessage = String.Format($"{dateTime} - {message}\n");
            File.AppendAllText("log.txt", formattedMessage);
        }
    }

    /// <summary>
    /// Arguments of a log
    /// </summary>
    /// <remarks>
    /// This class carries out a log message and the date when the message was sent
    /// </remarks>
    class SendLogEventArgs : EventArgs
    {
        public String Message;
        public DateTime DateTime;

        public SendLogEventArgs(String message, DateTime dateTime)
        {
            Message = message;
            DateTime = dateTime;
        }
    }
}