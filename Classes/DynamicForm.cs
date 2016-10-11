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
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;

namespace CodeOverload.Windows
{
    public partial class DynamicForm : Form
    {
        private List<Control> controls;

        public object DataItem { get; private set; }

        public DynamicForm(string title, object item)
        {
            InitializeComponent();
            InitializeForm(title);
            InitializeControls(item);
            this.DataItem = item;

            // We need to ensure that the required fields are clearly marked, so that
            // the user can see which controls they must complete in order for the OK
            // button to become enabled.
            ValidateControls();
        }

        private void InitializeForm(string title)
        {
            this.Text = title;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterScreen;
            errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
            controls = new List<Control>();
        }

        private void InitializeControls(object item)
        {
            PropertyInfo[] properties = item.GetType().GetProperties();

            // Create layout table
            this.tableLayoutPanel1.RowCount = properties.Length;

            // For each property
            int rowNumber = 0;
            foreach (var property in properties)
            {
                Control ctrl = ControlFactory.CreateControl(item, property);
                if (ctrl != null)
                {
                    // Get custom attributes
                    object[] attributes = property.GetCustomAttributes(true);
                    ctrl = ApplyAttributes(ctrl, attributes);

                    // Disable the control if property read only
                    if (!property.CanWrite)
                        ctrl.Enabled = false;

                    // Set the tab index
                    //ctrl.TabIndex = controls.Count + 1;

                    // Build label
                    if (ctrl.Visible)
                    {
                        ControlTag tag = (ControlTag)ctrl.Tag;
                        Label label = ControlFactory.CreateLabel(property.Name);
                        if (!string.IsNullOrEmpty(tag.CustomLabel))
                            label.Text = tag.CustomLabel;
                        tableLayoutPanel1.Controls.Add(label, 0, rowNumber);
                        tableLayoutPanel1.Controls.Add(ctrl, 1, rowNumber);
                        controls.Add(ctrl);
                    }
                }
                rowNumber++;
            }

            // Resize the form
            this.Width = tableLayoutPanel1.Width + 40;
            this.Height = tableLayoutPanel1.Height + 90;
        }

        /// <summary>
        /// Applies the settings from the custom attributes to the control.
        /// </summary>
        /// <param name="ctrl">A control bound to property</param>
        /// <param name="attributes">Custom attributes for the property</param>
        /// <returns></returns>
        private Control ApplyAttributes(Control ctrl, object[] attributes)
        {
            ControlTag tag = (ControlTag)ctrl.Tag;
            NumericSettingsAttribute attrRange = null;
            DisplaySettingsAttribute attrDisplay = null;
            RequiredFieldAttribute attrRequired = null;
            foreach (object attribute in attributes)
            {
                if (attribute is NumericSettingsAttribute)
                    attrRange = (NumericSettingsAttribute)attribute;
                else if (attribute is DisplaySettingsAttribute)
                    attrDisplay = (DisplaySettingsAttribute)attribute;
                else if (attribute is RequiredFieldAttribute)
                    attrRequired = (RequiredFieldAttribute)attribute;
            }

            // Attach LostFocus handler for input validation
            ctrl.LostFocus += ctrl_LostFocus;

            // Range Attribute
            if (attrRange != null)
            {
                // todo
            }

            // Display Attribute
            if (attrDisplay != null)
            {
                tag.CustomLabel = attrDisplay.Label;
                ctrl.Enabled = !attrDisplay.ReadOnly;
                ctrl.Visible = attrDisplay.Visible;
                if (attrDisplay.Width > 0)
                    ctrl.Width = attrDisplay.Width;
            }

            // Required Field Attribute
            if (attrRequired != null)
            {
                if (string.IsNullOrEmpty(attrRequired.Message))
                    tag.ErrorMessage = "Required";
                else
                    tag.ErrorMessage = attrRequired.Message;
            }
            return ctrl;
        }

        private void ctrl_LostFocus(object sender, EventArgs e)
        {
            // TODO: It would be better to validate just this control and update the
            // OK button accordingly, instead of validating every control on the
            // form.
            ValidateControls();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            SaveControlValues();
        }

        private bool ValidateControl(Control control)
        {
            bool isValid = true;

            // Validation currently limited to TextBoxes only
            TextBox txt = control as TextBox;
            if (txt != null)
            {
                // If the textbox is empty, show a warning
                ControlTag tag = (ControlTag)txt.Tag;
                if (tag.IsRequired && string.IsNullOrEmpty(txt.Text))
                {
                    errorProvider.SetError(txt, tag.ErrorMessage);
                    isValid = false;
                }
                else
                    errorProvider.SetError(txt, string.Empty);
            }
            return isValid;
        }

        /// <summary>
        /// Returns false if any controls are invalid.
        /// </summary>
        /// <returns></returns>
        private void ValidateControls()
        {
            bool isValid = true;
            foreach (Control control in controls)
            {
                if (!ValidateControl(control))
                    isValid = false;
            }
            btnOk.Enabled = isValid;
        }

        /// <summary>
        /// Saves the controls value back to the data object.
        /// </summary>
        private void SaveControlValues()
        {
            // For each TextBox, Dropdown etc...
            foreach (Control c in controls)
            {
                ControlTag tag = (ControlTag)c.Tag;
                PropertyInfo property = DataItem.GetType().GetProperty(tag.PropertyName);
                Type type = property.PropertyType;
                if (c is TextBox)
                {
                    TextBox textbox = (TextBox)c;
                    if (type == typeof(string))
                        property.SetValue(DataItem, textbox.Text, null);
                    else if (type == typeof(char))
                        property.SetValue(DataItem, Convert.ToChar(textbox.Text), null);
                }
                else if (c is NumericUpDown)
                {
                    NumericUpDown numeric = (NumericUpDown)c;
                    if (type == typeof(int))
                        property.SetValue(DataItem, Convert.ToInt32(numeric.Value), null);
                    else if (type == typeof(decimal))
                        property.SetValue(DataItem, Convert.ToDecimal(numeric.Value), null);
                }
                else if (c is CheckBox)
                {
                    CheckBox checkbox = c as CheckBox;
                    property.SetValue(DataItem, checkbox.Checked, null);
                }
                else if (c is ComboBox)
                {
                    ComboBox dropdown = c as ComboBox;
                    property.SetValue(DataItem, Enum.Parse(tag.PropertyType, Convert.ToString(dropdown.SelectedItem)), null);
                }
            }
        }
    }
}
