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

			//mediaPlayerElement.Source = MediaSource.CreateFromUri(new Uri("file://C:/xxx/14-�s�����j�ގ҂���.wma"));

			mediaPlayerElement.MediaPlayer.IsLoopingEnabled = true;

			DispatcherTimer dispatcherTimer = new DispatcherTimer();
			dispatcherTimer.Tick += dispatcherTimer_Tick;
			dispatcherTimer.Interval = TimeSpan.FromMilliseconds(1);
			dispatcherTimer.Start();

			void dispatcherTimer_Tick(object sender, object e)
			{
				TimeSpan position = mediaPlayerElement.MediaPlayer.PlaybackSession.Position;
				position_label_element.Text = $"�ʒu�F{position.TotalSeconds}";

				if (this.current_truck_index != null)
				{
					Truck current_truck_data = truck_list[(int)this.current_truck_index];

					if (current_truck_data.position_a != "" &&
						current_truck_data.position_b != "" &&
						Convert.ToDouble(current_truck_data.position_b) <= position.TotalSeconds)
					{
						mediaPlayerElement.MediaPlayer.PlaybackSession.Position = TimeSpan.FromSeconds(Convert.ToDouble(current_truck_data.position_a));
						// mediaPlayerElement.MediaPlayer.Pause();
					}
				}
			}


			Truck truck_data = new Truck();
			truck_data.filepath = "C:/xxx/14-�s�����j�ގ҂���.wma";
			truck_data.name = "14-�s�����j�ގ҂���.wma";
			truck_data.position_a = "3.02";
			truck_data.position_b = "200.19";
			truck_data.ab_repeat_num = "";
			truck_data.is_start_position_a = false;
			truck_list.Add(truck_data);

			truck_data = new Truck();
			truck_data.filepath = "C:/xxx/72-�Ł[�����ǂւ��� (9st_�A�N�V����).wma";
			truck_data.name = "72-�Ł[�����ǂւ��� (9st_�A�N�V����).wma";
			truck_data.position_a = "52.25";
			truck_data.position_b = "103.02";
			truck_data.ab_repeat_num = "";
			truck_data.is_start_position_a = false;
			truck_list.Add(truck_data);

		}

		private void play_button_Click(object sender, RoutedEventArgs e)
		{
			mediaPlayerElement.MediaPlayer.PlaybackSession.Position = TimeSpan.FromSeconds(200.19 - 2);
			mediaPlayerElement.MediaPlayer.Play();
		}

		// �g���b�N���X�g�́~�{�^���N���b�N���̏���
		private void truck_delete_button_Click(object sender, RoutedEventArgs e)
		{
			int? click_index = this.GetTruckListIndex((Button)sender);
			if (click_index != null)
			{
				truck_list.RemoveAt((int)click_index);
			}
		}

		// �g���b�N���X�g�̍Đ��{�^���N���b�N���̏���
		private void truck_play_button_Click(object sender, RoutedEventArgs e)
		{
			int? truck_index = this.GetTruckListIndex((Button)sender);
			if (truck_index != null)
			{
				this.current_truck_index = truck_index;
				this.PlayTruck(null);
			}
		}

		// �g���b�N���X�g��B���O�Đ��{�^���N���b�N���̏���
		private void truck_position_check_button_Click(object sender, RoutedEventArgs e)
		{
			int? truck_index = this.GetTruckListIndex((Button)sender);
			if (truck_index != null)
			{
				this.current_truck_index = truck_index;
				Truck current_truck_data = truck_list[(int)this.current_truck_index];

				// 1�b�}�C�i�X�����2�b�O���炢����Đ������B
				this.PlayTruck(Convert.ToDouble(current_truck_data.position_b) - 1);
			}
		}

		private void PlayTruck(double? position)
		{
			// Todo �t�H�[�J�X���O�����ɍĐ��{�^����������A�n�_�AB�n�_�̒l�����Ȃ��B

			if (this.current_truck_index != null)
			{
				// Todo A�n�_�AB�n�_�AA-B�ԍĐ��񐔂̓��̓`�F�b�N(�t�H�[�J�X�O�����Ƃ��̕����ǂ����H)


				Truck current_truck_data = truck_list[(int)this.current_truck_index];

				// truck_list�̃f�[�^�ɓ��͒l�𔽉f������B
				ListViewItem list_row_element = (ListViewItem)truck_list_view.ContainerFromIndex((int)this.current_truck_index);

				TextBox position_a_textbox = (TextBox)this.GetElementByName(list_row_element, "position_a");
				current_truck_data.position_a = position_a_textbox.Text;

				TextBox position_b_textbox = (TextBox)this.GetElementByName(list_row_element, "position_b");
				current_truck_data.position_b = position_b_textbox.Text;


				mediaPlayerElement.MediaPlayer.Pause();

				mediaPlayerElement.Source = MediaSource.CreateFromUri(new Uri("file://" + current_truck_data.filepath));
				if (position != null)
				{
					mediaPlayerElement.MediaPlayer.PlaybackSession.Position = TimeSpan.FromSeconds((double)position);
				}
				else
				{
					// Todo A����Đ���ON�̏ꍇ�̏���
					mediaPlayerElement.MediaPlayer.PlaybackSession.Position = TimeSpan.FromSeconds(0);
				}

				mediaPlayerElement.MediaPlayer.Play();
			}
		}


		// �g���b�N���X�g�̃N���b�N���ꂽ�s��Ԃ��B
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

		private DependencyObject GetElementByName(DependencyObject element, string name)
		{

			if (element.GetType().IsSubclassOf(typeof(FrameworkElement)))
			{
				object find_element = ((FrameworkElement)element).FindName(name);
				if (find_element != null)
				{
					return (DependencyObject)find_element;
				}
			}

			int children_count = VisualTreeHelper.GetChildrenCount(element);
			for (int i = 0; i < children_count; i++)
			{
				DependencyObject child_element = VisualTreeHelper.GetChild(element, i);
				if (child_element.GetType().IsSubclassOf(typeof(FrameworkElement)))
				{
					object find_element = this.GetElementByName(child_element, name);
					if (find_element != null)
					{
						return (DependencyObject)find_element;
					}
				}
			}
			return null;
		}


	}
}
