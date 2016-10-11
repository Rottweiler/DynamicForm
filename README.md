# DynamicForm
Dynamic forms for your c# or visual basic .net application

### Usage
In this example we create a simple dynamic form based off of the properties of the class 'Find'
```c#
// using CodeOverload.Windows
public Example()
{
  DynamicForm df = new DynamicForm("Find", new Find());
  result = df.ShowDialog();
  if (result == DialogResult.OK)
  {
    Find data = (Find)df.DataItem;
    // The data object is automatically bound to the form
  }
}

public class Find
{
  public int TaskNumber { get; set; }
}
```

### Credits
https://codeoverload.wordpress.com/2010/12/24/dynamic-forms-in-c/

### License
This source is covered by the GNU General Public License v2 and later