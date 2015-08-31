using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace WPAppStudio.Controls
{
    static class VisualTreeHelperExtensions
    {
        public static T FindDescendant<T>(this DependencyObject parent) where T : DependencyObject
        {
            var descendant = default(T);

            if (parent == null) return null;

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);

                if (child is T)
                {
                    descendant = child as T;
                    break;
                }

                descendant = child.FindDescendant<T>();

                if(descendant != null) break;
            }

            return descendant;
        }

        public static List<T> FindDescendants<T>(this DependencyObject parent) where T : DependencyObject
        {
            var descendants = new List<T>();

            if (parent == null) return null;

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);

                if (child is T)
                {
                    descendants.Add(child as T);
                }

                descendants.AddRange(child.FindDescendants<T>());
            }

            return descendants;
        }
    }
}
