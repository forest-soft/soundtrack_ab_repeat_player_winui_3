using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Core;
using Windows.Media.Playback;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Soundtrack_AB_Repeat_Player_WinUI_3
{
	/// <summary>
	/// An empty window that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class MainWindow : Window
	{
		public MainWindow()
		{
			this.InitializeComponent();

			mediaPlayerElement.Source = MediaSource.CreateFromUri(new Uri("file://C:/xxx/14-çsÇ≠éËÇëjÇﬁé“ÇΩÇø.wma"));

			mediaPlayerElement.MediaPlayer.IsLoopingEnabled = true;

			DispatcherTimer dispatcherTimer = new DispatcherTimer();
			dispatcherTimer.Tick += dispatcherTimer_Tick;
			dispatcherTimer.Interval = TimeSpan.FromMilliseconds(1);
			dispatcherTimer.Start();

			void dispatcherTimer_Tick(object sender, object e)
			{
				TimeSpan position = mediaPlayerElement.MediaPlayer.PlaybackSession.Position;
				position_label_element.Text = $"à íuÅF{position.TotalSeconds}";

				if (200.19 <= position.TotalSeconds)
				{
					mediaPlayerElement.MediaPlayer.PlaybackSession.Position = TimeSpan.FromSeconds(3.02);
					// mediaPlayerElement.MediaPlayer.Pause();
				}
			}
		}

		private void play_button_Click(object sender, RoutedEventArgs e)
		{
			mediaPlayerElement.MediaPlayer.PlaybackSession.Position = TimeSpan.FromSeconds(200.19 - 2);
			mediaPlayerElement.MediaPlayer.Play();
		}
	}
}
