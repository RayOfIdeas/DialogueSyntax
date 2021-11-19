#pragma warning disable CS8604 // Possible null reference argument.

using DialogueSyntaxSamples;

#region Tokens
const string BRANCH_OPEN_TOKEN = "<";
const string BRANCH_CLOSE_TOKEN = ">";
const string COMMAND_OPEN_TOKEN = "[";
const string COMMAND_CLOSE_TOKEN = "]";
const string PARAMETER_OPEN_TOKEN = "{";
const string PARAMETER_CLOSE_TOKEN = "}";
const string FIRST_BRANCH_NAME = "START";
#endregion

#region Parsing

List<Branch> GetBranches(string text)
{
    List<Branch> branchesData = new List<Branch>();

    #region Separate Branches

    var branchesText = StringUtility.StringUtility.SeparateToListAfterToken(text, BRANCH_OPEN_TOKEN);

    foreach (var branch in branchesText)
    {
        var branchTuple = StringUtility.StringUtility.ExtractPairByNameTokens(branch, BRANCH_OPEN_TOKEN, BRANCH_CLOSE_TOKEN);
        var branchName = branchTuple.Item1;
        var commandsText = branchTuple.Item2;
        List<Command> commandsData = new List<Command>();

        #region Separate Nodes
        var commandsList = StringUtility.StringUtility.SeparateToListAfterToken(commandsText, COMMAND_OPEN_TOKEN);

        foreach (var commandText in commandsList)
        {
            var commandTuple = StringUtility.StringUtility.ExtractPairByNameTokens(commandText, COMMAND_OPEN_TOKEN, COMMAND_CLOSE_TOKEN);
            var commandName = commandTuple.Item1;
            var parametersText = commandTuple.Item2;

            Command commandData = GetSpecializedCommand(new Command(commandName, parametersText));
            commandsData.Add(commandData);
        }

        #endregion

        Branch branchData = new Branch(branchName, commandsData);
        branchesData.Add(branchData);
    }

    #endregion

    return branchesData;

    Command GetSpecializedCommand(Command command)
    {
        if (command.Name == CommandType.GOTO.ToString())
        {
            var targetBranch = StringUtility.StringUtility.ExtractByTokens(command.ParameterRaw, PARAMETER_OPEN_TOKEN, PARAMETER_CLOSE_TOKEN);
            CommandGoTo commandGoTo = new CommandGoTo(command.Name, command.ParameterRaw, targetBranch);
            return commandGoTo;
        }
        else if (command.Name == CommandType.CHOICES.ToString())
        {
            List<Choice> choicesData = new List<Choice>();
            var choicesText = StringUtility.StringUtility.SeparateToListBeforeToken(command.ParameterRaw, PARAMETER_CLOSE_TOKEN);
            foreach (var choiceText in choicesText)
            {
                var choiceTuple = StringUtility.StringUtility.ExtractPairByParameterTokens(choiceText, PARAMETER_OPEN_TOKEN, PARAMETER_CLOSE_TOKEN);
                Choice choiceData = new Choice(optionText: choiceTuple.Item1, targetBranch: choiceTuple.Item2);
                choicesData.Add(choiceData);
            }

            CommandChoice commandChoice = new CommandChoice(command.Name, command.ParameterRaw, choicesData);
            return commandChoice;
        }
        else // Command that's not a special name is an actor name
        {
            CommandSay commandSay = new CommandSay(command.Name, command.ParameterRaw, command.ParameterRaw);
            return commandSay;
        }
    }
}

#endregion

#region Testing

string text = DialogueSyntaxSamples.Properties.Resources.sample_text;
List<Branch> branches = GetBranches(text);

// In this example, the branch with the name of FIRST_BRANCH_NAME will always be displayed first
Branch startBranch = branches.Find(branch => branch.Name == FIRST_BRANCH_NAME);
DisplayDialogue(startBranch, branches);

void DisplayDialogue(Branch branchToDisplay, List<Branch> branches)
{
    foreach (var command in branchToDisplay.Commands)
    {
        if (command is CommandSay)
        {
            CommandSay commandSay = (CommandSay)command;
            Console.WriteLine("{0}: {1}", commandSay.Name, commandSay.Say);
        }

        else if (command is CommandGoTo)
        {
            CommandGoTo commandGoTo = (CommandGoTo)command;
            Branch targetBranch = branches.Find(branch => branch.Name == commandGoTo.TargetBranch);
            DisplayDialogue(targetBranch, branches);
        }

        else if (command is CommandChoice)
        {
            CommandChoice commandChoice = (CommandChoice)command;

            #region Console display
            Console.WriteLine("\nChoices:");
            int index = 0;
            foreach (var choice in commandChoice.Choices)
            {
                Console.WriteLine("{0}. {1}", index, choice.OptionText);
                index++;
            }
            Console.Write("Type: ");
            for (int i = 0; i < index; i++)
            {
                Console.Write("{0}", i);
                if(i != index - 1)
                {
                    Console.Write(", or ", i);
                }
                else
                {
                    Console.Write(" : ");
                }
            }
            #endregion

            var input = Console.ReadLine(); Console.WriteLine();
            Branch targetBranch = branches.Find(branch => branch.Name == commandChoice.Choices[int.Parse(input)].TargetBranch);
            DisplayDialogue(targetBranch, branches);
        }
    }
}

#endregion
