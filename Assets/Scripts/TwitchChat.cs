using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Net.Sockets;
using System.IO;
using System.Linq;

public class TwitchChat : MonoBehaviour
{
    private TcpClient twitchClient;
    private StreamReader reader;
    private StreamWriter writer;

    private static TwitchChat _instance;

    [Header("COUNTING")]
    public TextMeshProUGUI countingText;
    public TextMeshProUGUI countingCounter;
    public int counterCount;
    public bool weCounting = false;
    IDictionary<string, int> dictionary = new Dictionary<string, int>();

    [Header("RANDOM")]
    public TextMeshProUGUI randomText;
    public TextMeshProUGUI randomCounter;
    public TextMeshProUGUI drawNumber;
    public int counterRand;
    public bool weRandom = false;
    List<ChatMessage> randMessages = new List<ChatMessage> { };

    [Header("VOTE")]
    public TextMeshProUGUI voteText;
    public TextMeshProUGUI counterA;
    public TextMeshProUGUI counterB;
    public TextMeshProUGUI counterC;
    public Slider sliderA, sliderB, sliderC;
    public TextMeshProUGUI voteCounter;
    public int numA, numB, numC = 0; // Number of votes for A, B, C
    public float targetA, targetB, targetC = 0; // num/counter (target for slider between 0 and 1)
    public int counterVote;
    public bool weVoting = false;
    public float sliderSpeed = 10;

    public struct TwitchCredentials
    {
        public string Username;
        public string ChannelName;
        public string Password;
    }

    public struct ChatMessage
    {
        public string Chatter;
        public string Message;
    }

    public static TwitchChat Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = new TwitchChat();
            }
            return _instance;
        }
    }
    void Awake()
    {
        _instance = this;
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        counterCount = 0;
        counterRand = 0;
        counterVote = 0;
        weCounting = false;
        weRandom = false;
        weVoting = false;
    }

    void Update()
    {
        if(twitchClient != null && twitchClient.Connected)
        {
            ReadChat();
        }
    }

    private void LateUpdate()
    {
        // I made the Vote bars lerp to their new value instead of snapping because why not B)
        if (counterVote > 0)
        {
            sliderA.value = Mathf.Lerp(sliderA.value, targetA, sliderSpeed * Time.deltaTime);
            sliderB.value = Mathf.Lerp(sliderB.value, targetB, sliderSpeed * Time.deltaTime);
            sliderC.value = Mathf.Lerp(sliderC.value, targetC, sliderSpeed * Time.deltaTime);
        }
        else if (counterVote == 0) // This is always running when vote is 0, maybe find a better way to enable/disable this *shrug emoji*
        {
            sliderA.value = Mathf.Lerp(sliderA.value, 0, sliderSpeed * Time.deltaTime);
            sliderB.value = Mathf.Lerp(sliderB.value, 0, sliderSpeed * Time.deltaTime);
            sliderC.value = Mathf.Lerp(sliderC.value, 0, sliderSpeed * Time.deltaTime);
        }
    }

    public bool Connect(TwitchCredentials credentials)
    {
        twitchClient = new TcpClient("irc.chat.twitch.tv", 6667);
        reader = new StreamReader(twitchClient.GetStream());
        writer = new StreamWriter(twitchClient.GetStream());

        writer.WriteLine("PASS " + credentials.Password);
        writer.WriteLine("NICK " + credentials.Username);
        writer.WriteLine("USER " + credentials.Username + " 8 * :" + credentials.Username);
        writer.WriteLine("JOIN #" + credentials.ChannelName);
        writer.Flush();
        return testConnect();
    }

    public bool testConnect()
    {
        string message = reader.ReadLine();
        Debug.Log(message);

        // Need to fix this, sometimes Twitch takes a sec so we return a false negative
        if (message.Contains("Welcome, GLHF!"))
        {
            Debug.Log("Connected! WEEEEEE");
            return true;
        }
        else
        {
            Debug.Log("Didn't connect? uhhhh...");
            return false;
        }
    }

    private void ReadChat()
    {
        if(twitchClient.Available > 0)
        {
            string message = reader.ReadLine();
            Debug.Log(message);

            if (message.Contains("PING"))
            {
                writer.WriteLine("PONG");
                writer.Flush();
                return;
            }

            if (message.Contains("PRIVMSG"))
            {
                var splitPoint = message.IndexOf("!", 1);
                var chatName = message.Substring(0, splitPoint);
                chatName = chatName.Substring(1);

                splitPoint = message.IndexOf(":", 1);
                message = message.Substring(splitPoint + 1);
                string newMessage = message.ToLower().Trim();
                if(weCounting)
                {
                    counterCount++;
                    countingCounter.text = counterCount + " messages";
                    Count(newMessage);
                }
                if(weRandom)
                {
                    counterRand++;
                    randomCounter.text = counterRand + " messages";
                    ChatMessage chat = new ChatMessage { Chatter = chatName, Message = newMessage };
                    Rand(chat);
                }
                if(weVoting)
                {
                    if(newMessage == "a" || newMessage == "b" || newMessage == "c")
                    {
                        counterVote++;
                        voteCounter.text = counterVote + " total votes";
                        Vote(newMessage);
                    }
                }
            }
        }
    }

    //          ***** COUNTING *****

    public void StartCount()
    {
        weCounting = true;
        countingText.text = "Waiting for chat messages...";
    }

    public void PauseCount()
    {
        weCounting = false;
    }

    public void ClearCount()
    {
        dictionary.Clear();
        countingText.text = "Chat cleared!";
        counterCount = 0;
        countingCounter.text = counterCount + " messages";
        weCounting = false;
    }

    void Count(String message)
    {
        // Key is message, value is number of occurrences
        // If message is present in dictionary increment the number, otherwise insert with count of 1

        // Internet says TryGetValue is faster than ContainsKey/Item and I have no reason to assume otherwise
        int value = 0;
        if (dictionary.TryGetValue(message, out value))
        {
            //Debug.Log("Message already in dictionary!");
            dictionary[message] = value + 1;
        }
        else
        {
            //Debug.Log("Message not in dictionary.");
            dictionary.Add(message, 0);
        }

        // Sort dictionary by value, then grab first ten elements
        var mySortedList = dictionary.OrderByDescending(d => d.Value).ToList();
        var topList = mySortedList.Take(10);

        // Display top results
        countingText.text = String.Join("\n",topList.Select((x, n) => $"{n + 1}. {x.Key} : {x.Value + 1}"));

        // This is probably a bad way to display top ten results in real time
        // If you're smarter than me you could use something like K Heavy Hitters to display live view
        // Then hit a Finalize button or something, clear Heavy Hitters and run through/display Dictionary
    }

    //          ***** RANDOM *****

    public void StartRandom()
    {
        randomText.text = "Reading chat...";
        weRandom = true;
    }

    public void PauseRandom()
    {
        weRandom = false;
    }

    public void ClearRandom()
    {
        randMessages.Clear();
        randomText.text = "Chat cleared!";
        counterRand = 0;
        randomCounter.text = counterRand + " messages";
        weRandom = false;
        drawNumber.text = "";
    }

    public void Rand(ChatMessage chat)
    {
        randMessages.Add(chat);
    }

    public void Draw()
    {
        if (counterRand == 0)
        {
            //Debug.Log("Zero random messages!");
            randomText.text = "No messages yet!";
            return;
        }
        int randNum = UnityEngine.Random.Range(0, counterRand);
        //Debug.Log("Random draw is #" + randNum + "!");
        randomText.text = "<size=36> <color=yellow>" + randMessages[randNum].Chatter + "</color> says:</size>\n<color=green><b>\"" + randMessages[randNum].Message + "\"</b></color>";
        drawNumber.text = "Message #" + (randNum + 1) + "!"; // Add 1 to randNum for human readability
    }

    //          ***** VOTE *****

    public void StartVote()
    {
        weVoting = true;
        voteText.text = "Type \"a\" \"b\" or \"c\" to vote.";
    }

    public void PauseVote()
    {
        weVoting = false;
        voteText.text = "Vote paused!";
    }

    public void ClearVote()
    {
        voteText.text = "Enter your three options:";
        counterVote = 0;
        voteCounter.text = counterVote + " total votes";
        weVoting = false;
        counterA.text = "0 votes";
        counterB.text = "0 votes";
        counterC.text = "0 votes";
        numA = 0;
        numB = 0;
        numC = 0;
    }

    public void Vote(string message)
    {
        if(message == "a")
        {
            numA++;
            counterA.text = numA + " votes";
        }
        else if(message == "b")
        {
            numB++;
            counterB.text = numB + " votes";
        }
        else if(message == "c")
        {
            numC++;
            counterC.text = numC + " votes";
        }

        // Calculate targets for sliders to lerp to
        targetA = (float)numA / counterVote;
        targetB = (float)numB / counterVote;
        targetC = (float)numC / counterVote;
    }
}
