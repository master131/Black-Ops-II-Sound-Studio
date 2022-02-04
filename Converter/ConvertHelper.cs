using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using BlackOps2SoundStudio.Encoders;

namespace BlackOps2SoundStudio.Converter
{
    class ConversionProgressChangedEventArgs : EventArgs
    {
        public string SourceFormat { get; set; }
        public string Source { get; set; }
        public string TargetFormat { get; set; }
        public int Progress { get; set; }
    }

    class ConversionCompletedEventArgs : EventArgs
    {
        public Stream Output { get; set; }
        public object State { get; set; }
    }

    public class ConvertOptions
    {
        public int AudioChannels { get; set; }
        public int SampleRate { get; set; }
        
        public ConvertOptions()
        {
            AudioChannels = SampleRate = -1;
        }

        public override string ToString()
        {
            string arguments = string.Empty;
            if (AudioChannels != -1)
                arguments += "-ac " + AudioChannels + " ";
            if (SampleRate != -1)
                arguments += "-ar " + SampleRate;
            return arguments.TrimEnd();
        }
    }

    class ConvertHelper
    {
        private const string FFmpegPath = @"ffmpeg\bin\ffmpeg.exe";
        private const string ToWavPath = @"towav\towav.exe";
        private AsyncOperation _async;

        public event EventHandler<ConversionProgressChangedEventArgs> ConversionProgressChanged;
        public event EventHandler<ConversionCompletedEventArgs> ConversionCompleted;

        public static List<string> TemporaryFiles = new List<string>();

        public static void Cleanup()
        {
            var remainingFiles = new List<string>();

            foreach (var file in TemporaryFiles)
            {
                try
                {
                    File.Delete(file);
                }
                catch (Exception)
                {
                    remainingFiles.Add(file);
                }
            }

            TemporaryFiles.Clear();
            TemporaryFiles.AddRange(remainingFiles);
        }

        public static bool CanConvert()
        {
            return File.Exists(FFmpegPath);
        }

        public TimeSpan GetDuration(Stream input)
        {
            string path;
            var fileStream = input as FileStream;

            if (fileStream != null)
            {
                path = fileStream.Name;
            }
            else
            {
                path = Path.GetTempPath();
                using (fileStream = File.OpenWrite(path))
                    input.CopyTo(fileStream);
                TemporaryFiles.Add(path);
            }

            // Execute an invalid command to get the duration.
            TimeSpan duration = TimeSpan.Zero;
            ExecuteCommand(FFmpegPath, "-i \"" + path + "\"", (sender, args) =>
            {
                if (string.IsNullOrEmpty(args.Data))
                    return;

                var match = Regex.Match(args.Data, @"Duration: (?<duration>(\d{2}[:.]){3}\d{2})");
                if (match.Success) duration = TimeSpan.Parse(match.Groups["duration"].Value);
            });
            return duration;
        }

        public static Stream ConvertXMAToWAV(Stream input)
        {
            if (!File.Exists(ToWavPath))
                return null;

            // Create a temp file stream.
            var name = Guid.NewGuid().ToString();
            var temp = Path.Combine(Path.GetTempPath(), name + ".xma");
            using (var fs = File.Open(temp, FileMode.Create, FileAccess.ReadWrite, FileShare.Read))
            {
                // Write the data to the file and mark as temporary.
                TemporaryFiles.Add(temp);
                input.CopyTo(fs);
                fs.Flush();
            }

            // Begin decoding.
            var psi = new ProcessStartInfo(Path.GetFullPath(ToWavPath), "\"" + temp + "\"") {
                WorkingDirectory = Path.GetTempPath(),
                CreateNoWindow = true,
                UseShellExecute = false
            };
            var p = Process.Start(psi);
            p.WaitForExit();

            // Find the output file.
            var outputPath = Path.Combine(Path.GetTempPath(), name + ".wav");
            if (File.Exists(outputPath))
            {
                TemporaryFiles.Add(outputPath);
                return File.Open(outputPath, FileMode.Open, FileAccess.ReadWrite, FileShare.Read);
            }

            return null;
        }

        public Stream ConvertToFLAC(string inputFile, ConvertOptions options)
        {
            // Convering to FLAC is a 2-way step. First we convert to WAV.
            var output = Convert(inputFile, ".wav", "-f wav -sample_fmt s16 " + options, null);
            if (output.Length == 0) return output;
            FormatRepair.RepairWAVStream(output);

            // Offset the memory to temp folder.
            var temp = Path.GetTempFileName();
            var fs = File.Open(temp, FileMode.Open, FileAccess.ReadWrite, FileShare.Read);
            TemporaryFiles.Add(temp);

            // Next we encode using libFLAC's native encoder because FFmpeg unfortunately
            // does not support --no-padding and --no-seektable at the moment. *sigh*
            using (var reader = new WavReader(output))
            using (var writer = new FlacWriter(fs, reader.BitDepth, reader.Channels, reader.SampleRate))
            {
                // Buffer for 1 second's worth of audio data
                var buffer = new byte[1 * reader.Bitrate / 8];
                int read;
                do
                {
                    OnConversionProgressChanged(new ConversionProgressChangedEventArgs {
                        SourceFormat = ".wav",
                        TargetFormat = ".flac",
                        Progress = (int) (reader.InputStream.Position * 100 / reader.InputStream.Length)
                    });
                    read = reader.InputStream.Read(buffer, 0, buffer.Length);
                    writer.Write(buffer, 0, read);
                } while (read > 0);
            }

            output.Close();
            return fs;
        }

        public void ConvertToFLACAsync(string inputFile, ConvertOptions options, object userState = null)
        {
            if (_async != null)
                throw new InvalidOperationException("Conversion is already in progress.");

            _async = AsyncOperationManager.CreateOperation(userState);
            ThreadPool.QueueUserWorkItem(x => OnConversionCompleted(new ConversionCompletedEventArgs { Output = ConvertToFLAC(inputFile, options), State = _async.UserSuppliedState }));
        }

        public void ConvertToHeaderlessWavAsync(string inputFile, ConvertOptions options, object userState = null)
        {
            if (_async != null)
                throw new InvalidOperationException("Conversion is already in progress.");

            _async = AsyncOperationManager.CreateOperation(userState);
            ThreadPool.QueueUserWorkItem(x => OnConversionCompleted(new ConversionCompletedEventArgs { Output = ConvertToHeaderlessWav(inputFile, options), State = _async.UserSuppliedState }));
        }

        public void ConvertToMP3Async(string inputFile, ConvertOptions options, object userState = null)
        {
            if (_async != null)
                throw new InvalidOperationException("Conversion is already in progress.");

            _async = AsyncOperationManager.CreateOperation(userState);
            ThreadPool.QueueUserWorkItem(x => OnConversionCompleted(new ConversionCompletedEventArgs { Output = ConvertToMP3(inputFile, options), State = _async.UserSuppliedState }));
        }

        public Stream ConvertToHeaderlessWav(string inputFile, ConvertOptions options)
        {
            var temp = Path.GetTempFileName();
            TemporaryFiles.Add(temp);
            Convert(inputFile, ".wav", "-f s16le " + options + " -y", temp);
            return File.OpenRead(temp);
        }

        public Stream ConvertToMP3(string inputFile, ConvertOptions options)
        {
            var temp = Path.GetTempFileName();
            TemporaryFiles.Add(temp);
            Convert(inputFile, ".mp3", "-f mp3 " + options + " -y", temp);
            return File.OpenRead(temp);
        }

        private void OnConversionProgressChanged(ConversionProgressChangedEventArgs args)
        {
            if (_async != null)
                _async.Post(x => ConversionProgressChanged(this, args), null);
            else
            {
                ConversionProgressChanged(this, args);
            }
        }

        private void OnConversionCompleted(ConversionCompletedEventArgs args)
        {
            if (_async != null)
            {
                _async.Post(x => ConversionCompleted(this, args), null);
                _async.OperationCompleted();
                _async = null;
            }
        }

        private static Stream ExecuteCommand(string fileName, string arguments, DataReceivedEventHandler handler)
        {
            // Create the process object and setup path and options.
            var p = new Process();
            p.StartInfo.FileName = fileName;
            p.StartInfo.Arguments = arguments;

            // Enable redirection of stdout.
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.CreateNoWindow = true;
            if (handler != null) p.ErrorDataReceived += handler;

            // Start the process.
            p.Start();
            p.BeginErrorReadLine();

            // Read stdout.
            var str = p.StandardOutput.BaseStream;
            var ms = new MemoryStream();
            while (!p.HasExited)
            {
                var buffer = new byte[1024];
                var count = str.Read(buffer, 0, 1024);
                ms.Write(buffer, 0, count);
            }

            // Wait for completion and return stream.
            p.WaitForExit();

            // Check return code.
            if (p.ExitCode != 0)
                ms.SetLength(0);

            ms.Position = 0;
            return ms;
        }

        private void Convert(string inputFile, string displayFormat, string options)
        {
            var result = Convert(inputFile, displayFormat, options, null);
            result.Close();
        }

        private Stream Convert(string inputFile, string displayFormat, string options, string outputFile)
        {
            TimeSpan totalDuration = TimeSpan.Zero;
            var sourceFormat = Path.GetExtension(inputFile);

            return ExecuteCommand(FFmpegPath, "-i \"" + inputFile + "\" " + options + " " + (string.IsNullOrEmpty(outputFile) ? "-" : outputFile),
            (sender, args) =>
            {
                if (string.IsNullOrEmpty(args.Data))
                    return;

                Debug.WriteLine(args.Data);

                if (totalDuration != TimeSpan.Zero)
                {
                    if (!args.Data.StartsWith("size=") &&
                        !args.Data.StartsWith("frame="))
                        return;

                    var match = Regex.Match(args.Data, @"time=(?<time>(\d{2}[:.]){3}\d{2})");
                    if (!match.Success) return;
                    var current = TimeSpan.Parse(match.Groups["time"].Value);

                    OnConversionProgressChanged(new ConversionProgressChangedEventArgs
                    {
                        SourceFormat = sourceFormat,
                        TargetFormat = displayFormat,
                        Source = inputFile,
                        Progress = (int)(current.TotalMilliseconds * 100 / totalDuration.TotalMilliseconds)
                    });
                }
                else
                {
                    var match = Regex.Match(args.Data, @"Duration: (?<duration>(\d{2}[:.]){3}\d{2})");
                    if (match.Success)
                        totalDuration = TimeSpan.Parse(match.Groups["duration"].Value);
                }
            });
        }
    }
}
