# Twitch Chat Connection Thing

This is a utility I made to read Twitch chat and process it in different ways. If you just want to download it to try it for yourself click on Releases to the right and download the zip. Unzip the folder then run the exe!

To see the program in action [click here to watch my archived stream.](https://youtu.be/zrUTX_6ATck)

**NOTE: this was created for personal use and is probably written very poorly as I have little idea what I'm doing. Do not expect future updates.**

You'll need Unity if you want to mess with the entire project. If you just want to see my crappy code go to Assets > Scripts > and check out the .cs files.

## Instructions

### Connecting to Twitch

Before you can do anything else you need to log in to Twitch chat.

>You can hit the tab key to switch between input fields. _Neat!_

**Username** - The username of your Twitch account.

**Channel Name** - The channel you want to connect to.

**OAuth Key** - After logging in to twitch.tv you need to generate an OAuth Key [using a website like this one](https://twitchapps.com/tmi/) or use Google or whatever. **Do not use your regular Twitch password!**

>Make sure you include the "oauth:" at the beginning of the key, for example: "oauth:123456789" The key is displayed as asterisks and isn't saved anywhere. Do not share or display your OAuth key, it can be used to log in to your account just like a password.
>
>If you intend to stream to Twitch and use this tool I recommend signing in off screen just to be safe. And then copy something random to replace the OAuth key in your clipboard.

Then just click CONNECT. If Twitch is slow to respond you may need to click a second time. _This is probably my bad code whoops._

Once you connect you can switch between the three different modes by using the dropdown menu in the top right. I'll explain each one in order.

### Counting

This mode counts the most-used phrases in chat and displays the top ten messages, along with the number of times that phrase was used. Capitalization is ignored as well as spaces before and after the text. If any messages are longer than one line it'll look gross but should work just fine.

Click START to begin reading chat, PAUSE to stop reading chat, and CLEAR to stop reading and reset.

>I recommend using Twitch's slow mode to prevent users from spamming their own responses. This can only be done by the broadcaster or a moderator. If you're new to chat commands look up "Slow" [on this page.](https://help.twitch.tv/s/article/chat-commands)

### Random

This mode lets you randomly display a message from the chat. Click START to begin reading chat, then click DRAW to randomly grab a message and display it, along with the name of the user responsible so you can publically shame them. Can click DRAW as many times as you want, but make sure to PAUSE if you want to stop allowing for new entries. The CLEAR button clears and resets, yes indeed.

>Oh by the way the program window is resizable so go nuts.

### Vote

I know Twitch has a built-in poll option but this is more visual and fun so leave me alone. There are three text fields to type in whatever you want, then chatters can vote by typing "a" "b" or "c" to vote for that respective choice. Capitalization is ignored, but "AAAAAA" won't count for an "A" vote. Neither will matching the text put into the text fields. I planned to build that feature but was lazy, also single letters are probably more reliable anyway.

START to start reading chat, PAUSE to pause, and CLEAR to clear.

>If you want more or fewer options you're out of luck, sorry. THREE IS FINE, TRUST ME.

## Final Thoughts

If you hit START on one of the modes and switch to another it'll keep running in the background. Useful if you want to, say, run Counting and Random simultaneously, but I'm not responsible for blowing up your computer if you keep all three running for a few days. I only tested up to a couple hundred messages, beyond that is on you.

If you use this and it doesn't brick your computer I'd love to hear about it. Drop by [one of my streams and say hi](https://www.twitch.tv/barryisstreaming) or [tweet at me](https://twitter.com/razzadoop). Feel free to use my code if you want to build a better version of this. Good luck!

#### BANANA ROTAT E

**~** - Toggle on and off **SPINNING FOOD MODE**. The tilde key (~) is left of the number 1 on your keyboard.

**1** - Previous food.

**2** - Next food.

**-** - Add counter-clockwise rotation.

**+** - Add clockwise rotation.