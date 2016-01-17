using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WindowsInput;

namespace KinectCursor.Sample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly KinectSensor sensor;
        private readonly KinectSystemCursor kinectCursor;
        private readonly InputSimulator inputSimulator;

        public MainWindow()
        {
            InitializeComponent();

            this.sensor = KinectSensor.GetDefault();
            this.sensor.Open();
            this.inputSimulator = new InputSimulator();
            this.kinectCursor = new KinectSystemCursor(
                sensor,
                new SimpleActiveBodySelector(),
                new CummulativeMovingAverageFilter(),
                new DoubleExponentialSmoothingFilter(0.4f, 0.5f),
                0.2f);

            this.kinectCursor.CursorMoved += KinectCursor_CursorMoved;
            this.kinectCursor.LeftMouseDown += KinectCursor_LeftMouseDown;
            this.kinectCursor.LeftMouseUp += KinectCursor_LeftMouseUp;
        }

        private void KinectCursor_CursorMoved(object sender, CursorPosition e)
        {
            this.inputSimulator.Mouse.MoveMouseTo(e.X, e.Y);
        }

        private void KinectCursor_LeftMouseDown(object sender, CursorPosition e)
        {
            this.inputSimulator.Mouse.LeftButtonDown();
        }

        private void KinectCursor_LeftMouseUp(object sender, CursorPosition e)
        {
            this.inputSimulator.Mouse.LeftButtonUp();
        }
    }
}
