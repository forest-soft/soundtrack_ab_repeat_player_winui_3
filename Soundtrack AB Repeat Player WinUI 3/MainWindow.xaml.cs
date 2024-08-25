using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Core;
using Windows.Media.Playback;
using static System.Runtime.InteropServices.JavaScript.JSType;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Soundtrack_AB_Repeat_Player_WinUI_3
{

	public class Truck
	{
		public string filepath { get; set; }
		public string name { get; set; }
		public string position_a { get; set; }
		public string position_b { get; set; }
		public string ab_repeat_num { get; set; }
		public bool is_start_position_a { get; set; }
	}

	/// <summary>
	/// An empty window that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class MainWindow : Window
	{

		private ObservableCollection<Truck> truck_list = new ObservableCollection<Truck>();
		private int? current_truck_index = null;

		public MainWindow()
		{
			this.InitializeComponent();

			//mediaPlayerElement.Source = MediaSource.CreateFromUri(new Uri("file://C:/xxx/14-行く手を阻む者たち.wma"));

			mediaPlayerElement.MediaPlayer.IsLoopingEnabled = true;

			DispatcherTimer dispatcherTimer = new DispatcherTimer();
			dispatcherTimer.Tick += dispatcherTimer_Tick;
			dispatcherTimer.Interval = TimeSpan.FromMilliseconds(1);
			dispatcherTimer.Start();

			void dispatcherTimer_Tick(object sender, object e)
			{
				TimeSpan position = mediaPlayerElement.MediaPlayer.PlaybackSession.Position;
				position_label_element.Text = $"位置：{position.TotalSeconds}";

				if (this.current_truck_index != null)
				{
					Truck current_truck_data = truck_list[(int)this.current_truck_index];

					
					
					if (Convert.ToDouble(current_truck_data.position_b) <= position.TotalSeconds)
					{
						mediaPlayerElement.MediaPlayer.PlaybackSession.Position = TimeSpan.FromSeconds(Convert.ToDouble(current_truck_data.position_a));
						// mediaPlayerElement.MediaPlayer.Pause();
					}
				}
			}


			Truck truck_data = new Truck();
			truck_data.filepath = "C:/xxx/14-行く手を阻む者たち.wma";
			truck_data.name = "14-行く手を阻む者たち.wma";
			truck_data.position_a = "3.02";
			truck_data.position_b = "200.19";
			truck_data.ab_repeat_num = "";
			truck_data.is_start_position_a = false;
			truck_list.Add(truck_data);

			truck_data = new Truck();
			truck_data.filepath = "C:/xxx/14-行く手を阻む者たち.wma";
			truck_data.name = "14-行く手を阻む者たち.wma";
			truck_data.position_a = "3.02";
			truck_data.position_b = "200.19";
			truck_data.ab_repeat_num = "";
			truck_data.is_start_position_a = false;
			truck_list.Add(truck_data);

		}

		private void play_button_Click(object sender, RoutedEventArgs e)
		{
			mediaPlayerElement.MediaPlayer.PlaybackSession.Position = TimeSpan.FromSeconds(200.19 - 2);
			mediaPlayerElement.MediaPlayer.Play();
		}

		// トラックリストの×ボタンクリック時の処理
		private void truck_delete_button_Click(object sender, RoutedEventArgs e)
		{
			int? click_index = this.GetTruckListIndex((Button)sender);
			if (click_index != null)
			{
				truck_list.RemoveAt((int)click_index);
			}
		}

		// トラックリストの再生ボタンクリック時の処理
		private void truck_play_button_Click(object sender, RoutedEventArgs e)
		{
			int? truck_index = this.GetTruckListIndex((Button)sender);
			if (truck_index != null)
			{
				this.current_truck_index = truck_index;
				this.PlayTruck(null);
			}
		}

		// トラックリストのB2秒前再生ボタンクリック時の処理
		private void truck_position_check_button_Click(object sender, RoutedEventArgs e)
		{
			int? truck_index = this.GetTruckListIndex((Button)sender);
			if (truck_index != null)
			{
				this.current_truck_index = truck_index;
				Truck current_truck_data = truck_list[(int)this.current_truck_index];
				this.PlayTruck(Convert.ToDouble(current_truck_data.position_b) - 2);
			}
		}

		private void PlayTruck(double? position)
		{
			if (this.current_truck_index != null)
			{
				// Todo A地点、B地点、A-B間再生回数の入力チェック(フォーカス外したときの方が良いか？)


				Truck current_truck_data = truck_list[(int)this.current_truck_index];

				// Todo truck_listのデータに入力値を反映する処理
				// current_truck_data.position_a = "";

				mediaPlayerElement.MediaPlayer.Pause();

				mediaPlayerElement.Source = MediaSource.CreateFromUri(new Uri("file://" + current_truck_data.filepath));
				if (position != null)
				{
					mediaPlayerElement.MediaPlayer.PlaybackSession.Position = TimeSpan.FromSeconds((double)position);
				}
				else
				{
					// Todo Aから再生がONの場合の処理
					mediaPlayerElement.MediaPlayer.PlaybackSession.Position = TimeSpan.FromSeconds(0);
				}

				mediaPlayerElement.MediaPlayer.Play();
			}
		}


		// トラックリストのクリックされた行を返す。
		private int? GetTruckListIndex(DependencyObject current_element)
		{
			DependencyObject parent_element = VisualTreeHelper.GetParent(current_element);
			if (parent_element == null)
			{
				return null;
			}
			if (typeof(ListViewItem) != parent_element.GetType())
			{
				return this.GetTruckListIndex(parent_element);
			}
			return truck_list_view.IndexFromContainer(parent_element);
		}


	}
}
