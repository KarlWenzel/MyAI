using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CnnData.Console
{
  public interface IConsoleMenuItem
  {
    int Choice { get; set; }
    string Description { get; set; }
    void OnChoose();
  }

  public class ConsoleMenuActionItem : IConsoleMenuItem
  {
    public IConsoleMenuDataContext Context { get; set; }
    public int Choice { get; set; }
    public string Description { get; set; }
    public Action<IConsoleMenuDataContext> MenuAction { get; set; }

    public void OnChoose()
    {
      this.MenuAction(this.Context);
    }
  }

  public class ConsoleMenuSubmenuItem : IConsoleMenuItem
  {
    public int Choice { get; set; }
    public string Description { get; set; }
    public ConsoleMenu ConsoleMenu { get; set; }

    public void OnChoose()
    {
      this.ConsoleMenu.Show();
    }
  }

  public interface IConsoleMenuDataContext
  {
    string GetMenuHeader();
    string GetPrompt();
  }

  public class BaseConsoleMenuDataContext : IConsoleMenuDataContext
  {
    public BaseConsoleMenuDataContext(string menuName, char menuDelimiterChar)
    {
      this.MenuName = menuName;
      this.MenuDelimiterChar = menuDelimiterChar;
    }

    public string MenuName { get; set; }
    public char MenuDelimiterChar { get; set; }

    public virtual string GetMenuHeader()
    {
      var label = string.Format("{0} {1} {0}", this.MenuDelimiterChar, this.MenuName);
      return string.Format("\n{0}\n{1}\n{0}\n", new string(this.MenuDelimiterChar, label.Length), label);
    }

    public virtual string GetPrompt() { return ":"; }
  }

  public class ConsoleMenu
  {
    public ConsoleMenu(IConsoleMenuDataContext context = null)
    {
      this.Context = (context == null) ? new BaseConsoleMenuDataContext("Main Menu", '#') : context;
      this.MenuItems = new List<IConsoleMenuItem>();
    }

    public IConsoleMenuDataContext Context { get; set; }
    private List<IConsoleMenuItem> MenuItems { get; set; }

    public void AddMenuFunctionItem(string description, Action<IConsoleMenuDataContext> menuAction)
    {
      this.MenuItems.Add(new ConsoleMenuActionItem()
      {
        Context = this.Context,
        Choice = this.MenuItems.Count() + 1,
        Description = description,
        MenuAction = menuAction
      });
    }

    public void AddSubmenuItem(string description, ConsoleMenu submenu)
    {
      this.MenuItems.Add(new ConsoleMenuSubmenuItem()
      {
        Choice = this.MenuItems.Count() + 1,
        Description = description,
        ConsoleMenu = submenu
      });
    }

    public void Show()
    {
      while (true)
      {
        System.Console.WriteLine(this.Context.GetMenuHeader());
        this.MenuItems.ForEach(x => System.Console.WriteLine("{0,-3}: {1}", x.Choice, x.Description));
        System.Console.WriteLine("{0,-3}: {1}", "q", "quit");
        System.Console.Write(this.Context.GetPrompt());

        var choice = System.Console.ReadLine();
        if (choice.ToLower().StartsWith("q"))
        {
          break;
        }

        try
        {
          var choiceNumber = int.Parse(choice.Trim());
          var menuItem = this.MenuItems.FirstOrDefault(x => x.Choice == choiceNumber);
          if (menuItem == null)
          {
            throw new KeyNotFoundException(string.Format("Menu choice {0} is invalid", choice));
          }
          menuItem.OnChoose();
        }
        catch (Exception err)
        {
          System.Console.WriteLine("Error: {0}", err.Message);
        }
      }

    }
  }
}

