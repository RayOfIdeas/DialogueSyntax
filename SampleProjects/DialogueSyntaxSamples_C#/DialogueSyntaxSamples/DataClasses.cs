using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DialogueSyntaxSamples;

public class Branch
{
    public string Name { get; private set; }
    public List<Command> Commands { get; private set; }

    public Branch (string name, List<Command> commands)
    {
        Name = name;
        Commands = commands;
    }
}

public enum CommandType { GOTO, CHOICES }

public class Command
{
    public string Name { get; protected set; }
    public string ParameterRaw { get; private set; }

    public Command (string name, string parameterRaw)
    {
        Name = name;
        ParameterRaw = parameterRaw;
    }
}

public class CommandSay : Command
{
    public string Say { get; private set; }

    public CommandSay (string name, string parameterRaw, string say) : base (name, parameterRaw)
    {
        Say = say;
    }
}

public class CommandGoTo : Command
{
    public string TargetBranch { get; private set; }
    public CommandGoTo (string name, string parameterRaw, string targetBranch) : base (name, parameterRaw)
    {
        TargetBranch = targetBranch;
    }
}

public class CommandChoice : Command
{
    public List<Choice> Choices { get; private set; }
    public CommandChoice (string name, string parameterRaw, List<Choice> choices) : base (name, parameterRaw)
    {
        Choices = choices;
    }
}

public struct Choice 
{ 
    public string OptionText; 
    public string TargetBranch; 

    public Choice(string optionText, string targetBranch)
    {
        this.OptionText = optionText;
        this.TargetBranch = targetBranch;
    }
}

