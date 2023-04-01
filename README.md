
# ImageBot

ImageBot is a customisable C# discord bot that easily allows you to add commands for responding with images.

## Usage

Inside the ImageBot folder execute the command:
```
dotnet run
```

Make sure to have your bot's token in a file named "token.txt" in the same folder.

## Testing

Inside the ImageBotTests folder execute the command:
```
dotnet test
```

## Adding Commands

To add a new command to the bot you must implement a class that inherits the interface ImageCommand.

Then, add an instance of the command class to the bot before starting the bot:
```C#
DiscordBot bot = new();
bot.AddImageCommand(new ExampleImageCommand());
```

See [DogImageCommand](https://github.com/PrestonLTaylor/ImageBot/blob/master/ImageBot/Commands/DogImageCommand.cs) for an example.

## License

[MIT](https://choosealicense.com/licenses/mit/)
