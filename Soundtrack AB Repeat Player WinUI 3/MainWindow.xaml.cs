using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Automation.Peers;
using Microsoft.UI.Xaml.Automation.Provider;
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

		private static string PLAY_BUTTON_LABEL_PLAY = "��";
		private static string PLAY_BUTTON_LABEL_STOP = "��";

		public MainWindow()
		{
			this.InitializeComponent();

			//mediaPlayerElement.Source = MediaSource.CreateFromUri(new Uri("file://C:/xxx/14-�s�����j�ގ҂���.wma"));

			mediaPlayerElement.MediaPlayer.IsLoopingEnabled = true;

			/*
			mediaPlayerElement.MediaPlayer.PlaybackSession.PlaybackStateChanged += xxx;
			void xxx(MediaPlaybackSession sender, object e)
			{
				if (this.current_truck_index != null)
				{
					// �����ŁuSystem.Runtime.InteropServices.COMException�v���N����B
					ListViewItem list_row_element = (ListViewItem)truck_list_view.ContainerFromIndex((int)this.current_truck_index);
					// Button truck_play_button = (Button)this.GetElementByName(list_row_element, "truck_play_button");
					// truck_play_button.RenderTransform
				}
					string aaa = "";
				if (sender.PlaybackState == MediaPlaybackState.Playing)
				{
					
				}
			}
			*/

			DispatcherTimer dispatcherTimer = new DispatcherTimer();
			dispatcherTimer.Tick += dispatcherTimer_Tick;
			dispatcherTimer.Interval = TimeSpan.FromMilliseconds(1);
			dispatcherTimer.Start();

			void dispatcherTimer_Tick(object sender, object e)
			{
				TimeSpan position = mediaPlayerElement.MediaPlayer.PlaybackSession.Position;
				position_label_element.Text = $"�Đ��ʒu�F{position.TotalSeconds}";

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

					// �g���b�N���X�g�̍Đ��{�^���̏�Ԃ�؂�ւ���B
					this.SetTruckPlayButtonLabel();
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

			truck_data = new Truck();
			truck_data.filepath = "C:/xxx/07-�^���^���R�� (�㔼 Ver_).wma";
			truck_data.name = "07-�^���^���R�� (�㔼 Ver_).wma";
			truck_data.position_a = "161.21";
			truck_data.position_b = "305.87";
			truck_data.ab_repeat_num = "";
			truck_data.is_start_position_a = false;
			truck_list.Add(truck_data);
			

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

		// �g���b�N���X�g��A�n�_�̑����{�^���N���b�N���̏���
		private void increment_position_a_button_Click(object sender, RoutedEventArgs e)
		{
			int? truck_index = this.GetTruckListIndex((Button)sender);
			if (truck_index != null)
			{
				this.current_truck_index = truck_index;

				ListViewItem list_row_element = (ListViewItem)truck_list_view.ContainerFromIndex((int)this.current_truck_index);

				NumberBox position_a_textbox = (NumberBox)this.GetElementByName(list_row_element, "position_a");
				double position_value = Convert.ToDouble(position_a_textbox.Text) + 0.01;
				position_a_textbox.Text = position_value.ToString("0.00");

				// B���O�Đ��{�^�����N���b�N���čĐ�������B
				Button truck_position_check_button = (Button)this.GetElementByName(list_row_element, "truck_position_check_button");
				ButtonAutomationPeer peer = new ButtonAutomationPeer(truck_position_check_button);
				IInvokeProvider invokeProv = peer.GetPattern(PatternInterface.Invoke) as IInvokeProvider;
				invokeProv.Invoke();
			}
		}

		// �g���b�N���X�g��A�n�_�̌����{�^���N���b�N���̏���
		private void decrement_position_a_button_Click(object sender, RoutedEventArgs e)
		{
			int? truck_index = this.GetTruckListIndex((Button)sender);
			if (truck_index != null)
			{
				this.current_truck_index = truck_index;

				ListViewItem list_row_element = (ListViewItem)truck_list_view.ContainerFromIndex((int)this.current_truck_index);

				NumberBox position_a_textbox = (NumberBox)this.GetElementByName(list_row_element, "position_a");
				double position_value = Convert.ToDouble(position_a_textbox.Text) - 0.01;
				if (position_value <= 0)
				{
					position_value = 0;
				}
				position_a_textbox.Text = position_value.ToString("0.00");

				// B���O�Đ��{�^�����N���b�N���čĐ�������B
				Button truck_position_check_button = (Button)this.GetElementByName(list_row_element, "truck_position_check_button");
				ButtonAutomationPeer peer = new ButtonAutomationPeer(truck_position_check_button);
				IInvokeProvider invokeProv = peer.GetPattern(PatternInterface.Invoke) as IInvokeProvider;
				invokeProv.Invoke();
			}
		}

		// �g���b�N���X�g��B�n�_�̑����{�^���N���b�N���̏���
		private void increment_position_b_button_Click(object sender, RoutedEventArgs e)
		{
			int? truck_index = this.GetTruckListIndex((Button)sender);
			if (truck_index != null)
			{
				this.current_truck_index = truck_index;

				ListViewItem list_row_element = (ListViewItem)truck_list_view.ContainerFromIndex((int)this.current_truck_index);

				NumberBox position_b_textbox = (NumberBox)this.GetElementByName(list_row_element, "position_b");
				double position_value = Convert.ToDouble(position_b_textbox.Text) + 0.01;
				position_b_textbox.Text = position_value.ToString("0.00");

				// B���O�Đ��{�^�����N���b�N���čĐ�������B
				Button truck_position_check_button = (Button)this.GetElementByName(list_row_element, "truck_position_check_button");
				ButtonAutomationPeer peer = new ButtonAutomationPeer(truck_position_check_button);
				IInvokeProvider invokeProv = peer.GetPattern(PatternInterface.Invoke) as IInvokeProvider;
				invokeProv.Invoke();
			}
		}

		// �g���b�N���X�g��B�n�_�̌����{�^���N���b�N���̏���
		private void decrement_position_b_button_Click(object sender, RoutedEventArgs e)
		{
			int? truck_index = this.GetTruckListIndex((Button)sender);
			if (truck_index != null)
			{
				this.current_truck_index = truck_index;

				ListViewItem list_row_element = (ListViewItem)truck_list_view.ContainerFromIndex((int)this.current_truck_index);

				NumberBox position_b_textbox = (NumberBox)this.GetElementByName(list_row_element, "position_b");
				double position_value = Convert.ToDouble(position_b_textbox.Text) - 0.01;
				if (position_value <= 0)
				{
					position_value = 0;
				}
				position_b_textbox.Text = position_value.ToString("0.00");

				// B���O�Đ��{�^�����N���b�N���čĐ�������B
				Button truck_position_check_button = (Button)this.GetElementByName(list_row_element, "truck_position_check_button");
				ButtonAutomationPeer peer = new ButtonAutomationPeer(truck_position_check_button);
				IInvokeProvider invokeProv = peer.GetPattern(PatternInterface.Invoke) as IInvokeProvider;
				invokeProv.Invoke();
			}
		}

		// �g���b�N���X�g�̍Đ��{�^���N���b�N���̏���
		private void truck_play_button_Click(object sender, RoutedEventArgs e)
		{
			int? truck_index = this.GetTruckListIndex((Button)sender);
			if (truck_index != null)
			{
				if (this.current_truck_index == truck_index &&
					mediaPlayerElement.MediaPlayer.PlaybackSession.PlaybackState == MediaPlaybackState.Playing)
				{
					mediaPlayerElement.MediaPlayer.Pause();
				}
				else
				{
					mediaPlayerElement.MediaPlayer.Pause();
					this.SetTruckPlayButtonLabel();
					
					this.current_truck_index = truck_index;
					this.PlayTruck(null);
				}
			}
		}

		// �g���b�N���X�g��B���O�Đ��{�^���N���b�N���̏���
		private void truck_position_check_button_Click(object sender, RoutedEventArgs e)
		{
			int? truck_index = this.GetTruckListIndex((Button)sender);
			if (truck_index != null)
			{
				mediaPlayerElement.MediaPlayer.Pause();
				this.SetTruckPlayButtonLabel();

				this.current_truck_index = truck_index;
				Truck current_truck_data = truck_list[(int)this.current_truck_index];

				// 1�b�}�C�i�X�����2�b�O���炢����Đ������B
				this.PlayTruck(Convert.ToDouble(current_truck_data.position_b) - 1);
			}
		}

		// �g���b�N�̍Đ�����
		private void PlayTruck(double? position)
		{
			if (this.current_truck_index != null)
			{
				Truck current_truck_data = truck_list[(int)this.current_truck_index];

				// truck_list�̃f�[�^�ɓ��͒l�𔽉f������B
				ListViewItem list_row_element = (ListViewItem)truck_list_view.ContainerFromIndex((int)this.current_truck_index);

				NumberBox position_a_textbox = (NumberBox)this.GetElementByName(list_row_element, "position_a");
				current_truck_data.position_a = position_a_textbox.Text;

				NumberBox position_b_textbox = (NumberBox)this.GetElementByName(list_row_element, "position_b");
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

				// �Đ���񗓂̏���ݒ肷��B

				// A�n�_
				string position_a_text = current_truck_data.position_a.ToString();
				if (position_a_text == "")
				{
					position_a_text = "�Ȃ�";
				}
				position_a_label_element.Text = $"A�n�_�F{position_a_text}";

				// B�n�_
				string position_b_text = current_truck_data.position_b.ToString();
				if (position_b_text == "")
				{
					position_b_text = "�Ȃ�";
				}
				position_b_label_element.Text = $"B�n�_�F{position_b_text}";

			}
		}

		// �g���b�N���X�g�̍Đ��{�^���̏�Ԃ�ݒ肷��B
		private void SetTruckPlayButtonLabel()
		{
			if (this.current_truck_index != null)
			{
				ListViewItem list_row_element = (ListViewItem)truck_list_view.ContainerFromIndex((int)this.current_truck_index);
				Button truck_play_button = (Button)this.GetElementByName(list_row_element, "truck_play_button");
				if (mediaPlayerElement.MediaPlayer.PlaybackSession.PlaybackState == MediaPlaybackState.Playing &&
					truck_play_button.Content.ToString() == PLAY_BUTTON_LABEL_PLAY)
				{
					truck_play_button.Content = PLAY_BUTTON_LABEL_STOP;
				}
				else if (mediaPlayerElement.MediaPlayer.PlaybackSession.PlaybackState != MediaPlaybackState.Playing &&
					truck_play_button.Content.ToString() == PLAY_BUTTON_LABEL_STOP)
				{
					truck_play_button.Content = PLAY_BUTTON_LABEL_PLAY;
				}
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
