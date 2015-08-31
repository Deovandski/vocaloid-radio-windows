using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using MyToolkit.UI;

namespace WPAppStudio.Controls.Maps
{
    public class PushPinTemplateSelector : DataTemplateSelector
    {
        public DataTemplate LabelDescription { get; set; }
        public DataTemplate ImageDescription { get; set; }
        public DataTemplate ImageLabel { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var pushpin = item as MapPushPin;

            if (pushpin != null)
            {
                switch (pushpin.Template)
                {
                    case "LabelDescription":
                        return LabelDescription;
                    case "ImageDescription":
                        return ImageDescription;
                    case "ImageLabel":
                        return ImageLabel;
                }
            }

            return LabelDescription;
        }
    }
}
