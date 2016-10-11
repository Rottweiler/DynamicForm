﻿///
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

namespace CodeOverload.Windows
{
  [AttributeUsage(AttributeTargets.Property)]
  public class DisplaySettingsAttribute : Attribute
  {
    public string Label { get; set; }
    public bool ReadOnly { get; set; }
    public bool Visible { get; set; }
    public int Width { get; set; }

    public DisplaySettingsAttribute()
    {
      Visible = true;
    }
  }
}
