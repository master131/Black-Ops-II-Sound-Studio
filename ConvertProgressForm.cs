using System;
using System.IO;
using System.Windows.Forms;
using Black_Ops_II_Sound_Studio_Extended;
using BlackOps2SoundStudio.Converter;
using BlackOps2SoundStudio.Format;

namespace BlackOps2SoundStudio
{
    public partial class ConvertProgressForm : Form
    {
        private AudioFormat _targetFormat;
        private Stream _outputStream;
        private string _inputPath;
        private ConvertHelper _convertHelper;
        private ConvertOptions _convertOptions;

        private Stream OutputStream
        {
            get { return _outputStream; }
        }

        public ConvertProgressForm()
        {
            InitializeComponent();

            _convertHelper = new ConvertHelper();

            _convertHelper.ConversionProgressChanged += (sender, args) =>
            {
                var targetFormat = args.TargetFormat;
                targetFormat = targetFormat.Substring(1).ToUpperInvariant();
                if (targetFormat == "FLAC")
                    conversionLabel.Text = "Encoding WAV to FLAC... ";
                else
                    conversionLabel.Text = "Converting " + Path.GetFileName(args.Source) + " to " + targetFormat + "... ";

                int progress = args.Progress > 100 ? 100 : args.Progress;
                conversionLabel.Text += progress + "%";
                conversionProgressBar.Value = progress;
            };

            _convertHelper.ConversionCompleted += (sender, args) =>
            {
                GC.Collect();
                _outputStream = args.Output;
                Close();
            };
        }

        public ConvertProgressForm(IProgress<int> conversionProgressReporter)
        {
            InitializeComponent();

            _convertHelper = new ConvertHelper();

            _convertHelper.ConversionProgressChanged += (sender, args) =>
            {
                var targetFormat = args.TargetFormat;
                targetFormat = targetFormat.Substring(1).ToUpperInvariant();
                /*if (targetFormat == "FLAC")
                    conversionLabel.Text = "Encoding WAV to FLAC... ";
                else
                    conversionLabel.Text = "Converting " + Path.GetFileName(args.Source) + " to " + targetFormat + "... ";

                conversionLabel.Text += args.Progress + "%";
                conversionProgressBar.Value = args.Progress;*/
                conversionProgressReporter.Report(args.Progress);
            };

            _convertHelper.ConversionCompleted += (sender, args) =>
            {
                GC.Collect();
                _outputStream = args.Output;
                Close();
            };
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams myCp = base.CreateParams;
                myCp.ClassStyle = myCp.ClassStyle | 0x200; // CP_NOCLOSE_BUTTON
                return myCp;
            }
        }

        private void SetOptions(string inputFile, AudioFormat format, ConvertOptions options)
        {
            _targetFormat = format;
            _inputPath = inputFile;
            _convertOptions = options;
        }

        internal static Stream Convert(string inputFile, AudioFormat format, ConvertOptions options)
        {
            if (options == null) options = new ConvertOptions();
            var form = new ConvertProgressForm();
            form.SetOptions(inputFile, format, options);
            form.ShowDialog();
            return form.OutputStream;
        }

        internal static Stream ConvertExternalProgressBar(string inputFile, AudioFormat format, ConvertOptions options, ReplaceAllForm replaceAllManager)
        {
            if (options == null) options = new ConvertOptions();
            var form = new ConvertProgressForm(replaceAllManager.conversionProgress);
            //form.conversionProgressBar = bar;
            //form.conversionLabel = conversionLabel;
            form.SetOptions(inputFile, format, options);
            return form.ConvertProgressForm_Sync();
        }

        public static Stream ConvertToFLAC(string inputFile, ConvertOptions options = null)
        {
            return Convert(inputFile, AudioFormat.FLAC, options);
        }

        public static Stream ConvertToMP3(string inputFile, ConvertOptions options = null)
        {
            return Convert(inputFile, AudioFormat.MP3, options);
        }

        public static Stream ConvertToHeaderlessWav(string inputFile, ConvertOptions options = null)
        {
            return Convert(inputFile, AudioFormat.PCMS16, options);
        }

        private void ConvertProgressForm_Load(object sender, EventArgs e)
        {
            if (_targetFormat == AudioFormat.FLAC)
                _convertHelper.ConvertToFLACAsync(_inputPath, _convertOptions);
            else if (_targetFormat == AudioFormat.PCMS16)
                _convertHelper.ConvertToHeaderlessWavAsync(_inputPath, _convertOptions);
            else if (_targetFormat == AudioFormat.MP3)
                _convertHelper.ConvertToMP3Async(_inputPath, _convertOptions);
        }

        private Stream ConvertProgressForm_Sync()
        {
            if (_targetFormat == AudioFormat.FLAC)
                return _convertHelper.ConvertToFLAC(_inputPath, _convertOptions);
            else if (_targetFormat == AudioFormat.PCMS16)
                return _convertHelper.ConvertToHeaderlessWav(_inputPath, _convertOptions);
            else if (_targetFormat == AudioFormat.MP3)
                return _convertHelper.ConvertToMP3(_inputPath, _convertOptions);
            else
                return null;
        }
    }
}
