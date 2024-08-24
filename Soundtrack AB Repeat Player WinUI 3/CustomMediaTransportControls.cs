using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soundtrack_AB_Repeat_Player_WinUI_3
{
	class CustomMediaTransportControls : MediaTransportControls
	{
		public CustomMediaTransportControls()
		{
			// this.DefaultStyleKey = typeof(CustomMediaTransportControls);
		}

		protected override void OnApplyTemplate()
		{
			// 「Cast to Devidce」ボタンを消す。
			AppBarButton cast_button = (AppBarButton)GetTemplateChild("CastButton");
			CommandBar command_bar = (CommandBar)GetTemplateChild("MediaControlsCommandBar");
			command_bar.PrimaryCommands.Remove(cast_button);
			

			// スライダーをカスタマイズする。
			Slider sliderControl = (Slider)GetTemplateChild("ProgressSlider");

			// 動きを細かくする。
			sliderControl.StepFrequency = 0.01;
			
			// 高さを増やして真ん中に表示させる。
			sliderControl.Height = 40;


			base.OnApplyTemplate();
		}
	}
}
