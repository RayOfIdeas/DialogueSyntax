# DialogueSyntax
 Description about DialogueSyntax, its principles and samples

## Definition
DialogueSyntax is a syntax designed to contain a non-linear dialogue data which can be easily read and edited by humans and computers.

---

## Purposes
DialogueSyntax is used to mainly solve these problems:
1. Structuring a simple readable non-linear dialogue.
2. Converting dialogues written by a writer to a useable data in a game.
3. Editing dialogue data from any device without any dependencies.

---

## Structure
DialogueSyntax uses two methods to contain dialogue data: Separation and Encapsulation, which construct the hierarchy of the data.

### Separation

Separation is a method to separate a collection of data that has the same context with another collection of data by using a token (special characters) between each data collection.

Example: 
> Separator token: ";"
>
> Input: "data1; data2; data3"
>
> Output: "data1", "data2", "data3"

### Encapsulation
Encapsulation is a method to separate a collection of data that has the same context with another collection of data by using an opening token and a closing token for each data collection.

Example:
> Opening token: "["
>
> Closing token: "]"
>
> Input: "[data1][data2][data3]"
>
> Output: "data1","data2","data3"

### Hierarchy
DialogueSyntax allow recursive nested data by using some tokens to mark a collection of data that contains a lower level collection of data.

Example:
> Branches data contains commands data which contains parameter data



## Best Practices
To fulfill the purposes listed above, it's encouraged to observe these guidelines when writing in DialogueSyntax if deemed possible:
- Only uses readily available characters in any device as tokens.
- Separation method is only used to contain a long list of data, whilst Encapsulation method is only used to contain one short data.
- Separator token uses either the opening token or the closing token of the Encapsulation method.
- Each data collection may only allow one time of Separation and one time of Encapsulation at its level.
- Keep the types of data collection to the maximum of three, by using three sets of tokens.
- Avoid using tokens if unnecessary.

---

## Samples
This DialogueSyntax sample conform to the listed best practices. 

These samples use the terminology of "Branch". "Command", and "Parameter". A file contains a list of branches, each branch contains a list of commands, each commands contains a list of parameters
> Branch tokens: opening: "<", closing: ">"
>
> Command tokens: opening: "[", closing: "]"
>
> Parameter tokens: opening: "{", closing: "}"

### Simple Dialogue Sample

#### Code:
`DialogueSyntaxSample.txt`
```
<START>
[Bob] Hello, Rick!
[Rick] Hi there, Bob!
```
#### Explanation:
- A branch data called `START` contains two command data, named: `Bob` and `Rick`. 
- `Bob` contains a parameter data of `Hello, Rick`. 
- `Rick` contains a parameter data of `Hi there, Bob`.

**Note:** the separator token of commands is the opening token of command's encapsulation method, which is "["

**Note:** Using Separation and Encapsulation methods, the encoder can retrieve these data:
- `Branches` has one element of branches:
    + The first element has two properties:
        * `BranchName` : `START`
        * `Commands` : consists of two commands:
            - The first command has two properties:
                + `CommandName` : `Bob`
                + `Parameter` : `Hello, Rick!`
            - The second command has two properties:
                + `CommandName` : `Rick`
                + `Parameter` : `Hi there, Bob!`

### Multiple Branches Dialogue Sample

#### Code:
`DialogueSyntaxSample.txt`
```
<START>
[Bob] Hello, Rick!
[Rick] Hi there, Bob!
[GOTO] {BRANCH_1}

<BRANCH_1>
[Bob] How are you doing?
[Rick] I'm fine!
```
#### Explanation:
- A branch data called `START` contains three command data, named: `Bob` , `Rick` , and `GOTO`. 
    + `Bob` contains a parameter data of `Hello, Rick`. 
    + `Rick` contains a parameter data of `Hi there, Bob`.
    + `GOTO` contains a parameter data of `BRANCH_1`.
- A branch data called `BRANCH_1` contains three command data, named: `Bob` and `Rick` . 
    + `Bob` contains a parameter data of `How are you doing?`. 
    + `Rick` contains a parameter data of `I'm fine!`.

**Note:** The usage of parameter tokens in `GOTO` are to help ensure both the writer and the encoder that the data contained inside the tokens must be retrieved as is. It also reminds the write that `GOTO` is not part of the spoken dialogue.

### Branching Dialogue Sample

#### Code:
`DialogueSyntaxSample.txt`
```
<START>
[Bob] Hello, Rick!
[Rick] Hi there, Bob!
[CHOICES] 
Long time no see, Rick {BRANCH_1}
I don't want to talk to you, Rick {BRANCH_2}

<BRANCH_1>
[Bob] Long time no see, Rick
[Rick] Yeah!

<BRANCH_2>
[Bob] Hey... Look I don't want to talk to you
[Rick] But, why?
```
#### Explanation:
- A branch data called `START` contains three command data, named: `Bob` , `Rick` , and `CHOICES`. 
    + `Bob` contains a parameter data of `Hello, Rick`. 
    + `Rick` contains a parameter data of `Hi there, Bob`.
    + `CHOICES` contains multiple types of parameter data:
        * First element contains `Long time no see, Rick` and `BRANCH_1`
        * Second element contains `I don't want to talk to you, Rick` and `BRANCH_2`


**Note:** The parameter tokens in `CHOICES` serves two purposes: to encapsulate the target branch, and to separate the spoken dialogue and the target branch. It is always valid to write like so:
```
[CHOICES] 
{Long time no see, Rick} {BRANCH_1}
{I don't want to talk to you, Rick} {BRANCH_2}
```

However, for the sake of simplicity and to prevent human errors, parameter tokens are not used for long data.

---

## Encoder
To retrieve data from a `.txt` file that uses DialogueSyntax, simply create some functions in your project using your preferred programming language by observing the Separation and Encapsulation methods.

### Retrieve data after the separator token

This simple flow of algorithm can retrieve a collection of data that is separated by Separation method:

1. Get the string of the whole text.
2. Create a loop which goes through each character inside the string of the whole text.
3. If the loop encounters the first separator token, save the index into a variable
4. If the loop encounter the second separator token, add the characters from the first encounter to this point of the loop to a list.
5. Remove the those characters from the whole text.
6. Repeat step 2 to 5 until the loop can no longer find the first separator token inside the whole text.
    + if the loop finds the first, but not the second separator token, add the character from the first encounter to the end of the whole text.

### Retrieve data before the separator token

This simple flow of algorithm can retrieve a collection of data that is separated by Separation method:

1. Get the string of the whole text.
2. Create a loop which goes through each character inside the string of the whole text.
3. If the loop encounters the separator token, add the characters from the start of the loop to this point of the loop to a list.
4. Remove the those characters from the whole text.
5. Repeat step 2 to 4 until the loop can no longer find the separator token inside the whole text.

### Retrieve data by the opening and closing token

This simple flow of algorithm can retrieve a collection of data that is separated by Encapsulation method:

1. Get the string of the whole text.
2. Create a loop which goes through each character inside the string of the whole text.
3. If the loop encounters the opening token, save the index into a variable
4. If the loop encounter the closing token, add the characters from the opening token to this point of the loop to a list.
5. Remove the those characters from the whole text.
6. Repeat step 2 to 5 until the loop can no longer find the opening token inside the whole text.

---

## Sample Projects
More examples can be found in https://github.com/RayOfIdeas/DialogueSyntax 
