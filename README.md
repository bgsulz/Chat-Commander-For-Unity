# Chat Commander
Integrate Twitch chat with a Unity environment. Easy and extensible.

## Parsing Twitch chat
### Step 1: Receive Twitch chat messages in Unity using the technique of your choice.

### Step 2: Submit messages like so:

```cs
var msg = GetMessageHoweverYouWant();
ChatParser.Submit(msg);
```

## Making a custom command
### Step 1: Make a new script and inherit from `ChatResponder` instead of `MonoBehaviour`.

```cs
using BGSulz.ChatCommander;

public class MyResponder : ChatResponder
{ ...
```

### Step 2: Override the `CommandKeywords` property and the `OnReceive` and `OnEnd` methods.

```cs
protected override IEnumerable<string> CommandKeywords => new[]
{
    // List all the commands that should
    // activate this code.
};

protected override void OnReceive(CommandMessage msg)
{
    // This code will run when the command is received.
}

protected override void OnEnd(CommandMessage msg)
{
    // For a timed command, this code allows you to
    // return the Scene to its previous state.
}
```

### Step 3: Write whatever code you please!

#### Making a !light command
This command should temporarily turn on a light in the Unity Scene.
If a number is given after, the light should remain on for that many seconds.
e.g. !light 5 turns the light on for 5 seconds.

```cs
using System.Collections.Generic;
using UnityEngine;
using BGSulz.ChatCommander;

public class TestResponder : ChatResponder
{
    [SerializeField] private Light overheadLight;

    protected override IEnumerable<string> CommandKeywords => new[]
    {
        "l",
        "light",
        "on"
    };

    protected override void OnReceive(CommandMessage msg)
    {
        overheadLight.intensity = 1;

        // Access "parameters" (words after the command)
        // by indexing the CommandMessage parameter.
        // In other words, write msg[0] for the first parameter,
        // msg[1] for the second, and so on.
        
        // Safely convert between string and common types with
        // ToInt, ToFloat, ToColor, etc.
        // A failed conversion results in the type's default value,
        // but this fallback value can be overridden by calling
        // the method with a parameter (e.g. .ToInt(1000))
        var duration = msg[0].ToInt();
        
        // Safely write timed commands with EndAfter.
        // This will call the OnEnd method
        // after a certain amount of seconds.
        EndAfter(duration);
    }

    protected override void OnEnd(CommandMessage msg)
    {
        overheadLight.intensity = 0;
    }
}
```

### Step 4: Add your new ChatResponder Component to any GameObject in your Scene.
The rest is automatic.
