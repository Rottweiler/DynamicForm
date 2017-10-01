///
/// Copyright © 2011 James Allen
///
/// This program is free software; you can redistribute it and/or modify
/// it under the terms of the GNU General Public License as published by
/// the Free Software Foundation; either version 2 of the License, or
/// (at your option) any later version.
///
/// This program is distributed in the hope that it will be useful,
/// but WITHOUT ANY WARRANTY; without even the implied warranty of
/// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
/// GNU General Public License for more details.
///
/// You should have received a copy of the GNU General Public License
/// along with this program; if not, write to the Free Software
/// Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA  02110-1301 USA
///
using CodeOverload.Windows.UI.Components;
using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace CodeOverload.Windows
{
    internal class ControlFactory
    {
        internal static Control CreateControl(object item, PropertyInfo property)
        {
            Control ctrl = null;
            Type type = property.PropertyType;

            // The control depends on the property type
            if (type == typeof(string))
            {
                ctrl = new TextBox();
                TextBox textbox = ctrl as TextBox;
                textbox.Text = (string)property.GetValue(item, null);
                textbox.Margin = new Padding(3, 3, 16, 0);
            }
            else if (type == typeof(char))
            {
                ctrl = new TextBox();
                TextBox textbox = ctrl as TextBox;
                textbox.MaxLength = 1;
                textbox.Width = 20;
                textbox.Text = Convert.ToString(property.GetValue(item, null));
                textbox.Margin = new Padding(3, 3, 16, 0);
            }
            else if (type == typeof(int))
            {
                ctrl = new NumericUpDown();
                NumericUpDown numeric = ctrl as NumericUpDown;
                numeric.Value = Convert.ToDecimal(property.GetValue(item, null));
            }
            else if (type == typeof(decimal))
            {
                ctrl = new NumericUpDown();
                NumericUpDown numeric = ctrl as NumericUpDown;
                numeric.DecimalPlaces = 2;
                numeric.Value = Convert.ToDecimal(property.GetValue(item, null));
            }
            else if (type == typeof(bool))
            {
                ctrl = new CheckBox();
                CheckBox checkbox = ctrl as CheckBox;
                checkbox.Checked = Convert.ToBoolean(property.GetValue(item, null));
            }
            else if (type.BaseType == typeof(Enum))
            {
                ctrl = new ComboBox();
                ComboBox dropdown = ctrl as ComboBox;
                dropdown.DropDownStyle = ComboBoxStyle.DropDownList;
                string[] names = Enum.GetNames(type);
                string value = Convert.ToString(property.GetValue(item, null));
                foreach (var name in names)
                {
                    dropdown.Items.Add(name);
                    if (name == value)
                        dropdown.SelectedIndex = dropdown.Items.Count - 1;
                }
            }
            else if (type == typeof(DateTime))
            {
                ctrl = new DateTimePicker();
                DateTimePicker date = ctrl as DateTimePicker;
                DateTime dateValue = Convert.ToDateTime(property.GetValue(item, null));
                if (dateValue < date.MinDate)
                    dateValue = date.MinDate;
                if (dateValue > date.MaxDate)
                    dateValue = date.MaxDate;
                date.Value = dateValue;
            }else if(type == typeof(FileInfo))
            {
                ctrl = new FileBrowserButton();
                FileBrowserButton button = ctrl as FileBrowserButton;
                button.FilePath.Text = (string) property.GetValue(item, null);
            }


            if (ctrl != null)
            {
                ControlTag tag = new ControlTag();
                tag.PropertyName = property.Name;
                tag.PropertyType = property.PropertyType;
                ctrl.Tag = tag;
            }
            return ctrl;
        }

        /// <summary>
        /// Creates a new instance of the Label control using the specified text value.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        internal static Label CreateLabel(string text)
        {
            Label label = new Label();
            label.Text = GetLabel(text) + ":";
            label.AutoSize = true;
            label.Margin = new Padding(3, 6, 6, 0);
            return label;
        }

        /// <summary>
        /// Returns a friendly label from the supplied name. For example, the
        /// string "firstName" would be returned as "First Name".
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private static string GetLabel(string text)
        {
            bool isFirst = true;
            StringBuilder sb = new StringBuilder();
            foreach (char c in text.ToCharArray())
            {
                if (isFirst)
                {
                    sb.Append(Char.ToUpper(c));
                    isFirst = false;
                }
                else
                {
                    if (Char.IsUpper(c))
                        sb.Append(' ');
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }
    }
}
