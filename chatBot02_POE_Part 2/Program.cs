using System;
using System.Collections.Generic;
using System.Speech.Synthesis;
using System.Threading;

namespace chatBot02_POE_Part_2
{
    class CybersecurityChatbot
    {
        // Speech synthesizer for voice output
        private static SpeechSynthesizer synthesizer = new SpeechSynthesizer();

        // User memory storage
        private static Dictionary<string, string> userMemory = new Dictionary<string, string>();
        private static string currentTopic = null;

        // Random instance for all random choices
        private static Random rand = new Random();

        // Dictionary for keyword recognition with random responses
        private static Dictionary<string, List<string>> keywordResponses = new Dictionary<string, List<string>>()
        {
            {"password", new List<string> {
                "Make sure to use strong, unique passwords for each account. Avoid using personal details in your passwords.",
                "Consider using a password manager to generate and store complex passwords securely.",
                "A good password should be at least 12 characters long and include a mix of letters, numbers, and symbols."
            }},
            {"scam", new List<string> {
                "Be wary of offers that seem too good to be true - they usually are.",
                "Never share personal or financial information with unsolicited callers or emails.",
                "Scammers often create a sense of urgency to pressure you into acting quickly."
            }},
            {"privacy", new List<string> {
                "Regularly review privacy settings on your social media accounts and apps.",
                "Be cautious about what personal information you share online.",
                "Using a VPN can help protect your online privacy by encrypting your internet connection."
            }},
            {"phishing", new List<string> {
                "Be cautious of emails asking for personal information. Scammers often disguise themselves as trusted organisations.",
                "Check the sender's email address carefully - phishing emails often use addresses that look similar to legitimate ones.",
                "Never click on links in suspicious emails. Instead, go directly to the company's website."
            }},
            {"browsing", new List<string> {
                "Use HTTPS websites and avoid clicking on unfamiliar links to stay safe online.",
                "Keep your browser updated to ensure you have the latest security patches.",
                "Consider using browser extensions that block trackers and malicious websites."
            }},
            {"authentication", new List<string> {
                "Two-factor authentication (2FA) is an extra layer of security that helps protect your online accounts.",
                "Enable two-factor authentication wherever possible for better account security.",
                "Authentication apps are more secure than SMS for two-factor authentication."
            }},
            {"malware", new List<string> {
                "Keep your antivirus software updated to protect against malware threats.",
                "Be cautious when downloading files from the internet, especially from unknown sources.",
                "Regularly update your operating system and software to patch security vulnerabilities."
            }}
        };

        static void Main(string[] args)
        {
            // Display welcome message
            DisplayWelcomeScreen();

            // Ask for the user's name and store it in memory
            InitializeUser();

            // Main conversation loop
            RunConversationLoop();

            // Dispose of synthesizer when done
            synthesizer.Dispose();
        }

        private static void DisplayWelcomeScreen()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(@"
        ██████╗ ██╗  ██╗ █████╗ ████████╗██████╗  ██████╗ ██████╗ ██████╗ 
        ██╔══██╗██║  ██║██╔══██╗╚══██╔══╝██╔══██╗██╔═══██╗██╔══██╗██╔══██╗
        ██████╔╝███████║███████║   ██║   ██████╔╝██║   ██║██████╔╝██████╔╝
        ██╔═══╝ ██╔══██║██╔══██║   ██║   ██╔══██╗██║   ██║██╔══██╗██╔══██╗
        ██║     ██║  ██║██║  ██║   ██║   ██║  ██║╚██████╔╝██║  ██║██████╔╝
        ╚═╝     ╚═╝  ╚═╝╚═╝  ╚═╝   ╚═╝   ╚═╝  ╚═╝ ╚═════╝ ╚═╝  ╚═╝╚═════╝ 
        ----------------------------------------------------------------
                 CYBERSECURITY AWARENESS CHATBOT - POE PART 2                  
        ----------------------------------------------------------------
            ");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Green;
            TypeEffect("Welcome to the Cybersecurity Awareness ChatBot02!\n");
            synthesizer.Speak("Welcome to the Cybersecurity Awareness ChatBot02!");
            Console.ResetColor();
        }

        private static void InitializeUser()
        {
            Console.WriteLine("\nHello! What's your name?");
            string userName = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(userName))
            {
                Console.WriteLine("I didn't catch your name. I'll just call you Cyber Hero!");
                synthesizer.Speak("I didn't catch your name. I'll just call you Cyber Hero!");
                userName = "Cyber Hero";
            }
            else
            {
                userMemory["name"] = userName;
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\nWelcome, {userName}, to the Cybersecurity Awareness ChatBot02!");
            synthesizer.Speak($"Welcome, {userName}, to the Cybersecurity Awareness ChatBot02!");
            Console.ResetColor();
        }

        private static void RunConversationLoop()
        {
            // Display initial instructions
            DisplayInstructions();

            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\nYou can:");
                Console.WriteLine("- Ask about cybersecurity topics (type naturally)");
                Console.WriteLine("- Tell me your interests (e.g., 'I'm interested in privacy')");
                Console.WriteLine("- Type 'menu' to see menu options");
                Console.WriteLine("- Type 'exit' to quit");
                Console.ResetColor();

                Console.Write("\nYou: ");
                string userInput = Console.ReadLine();

                if (userInput == null) continue; // Handle null input
                userInput = userInput.ToLower();

                // Exit condition
                if (userInput == "exit" || userInput == "quit")
                {
                    ExitChatbot();
                    return;
                }

                // Show menu if requested
                if (userInput == "menu")
                {
                    ShowMenu();
                    continue;
                }

                // Handle the user input with all features
                ProcessUserInput(userInput);
            }
        }

        private static void DisplayInstructions()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            TypeEffect("\nI can help you with various cybersecurity topics including:\n");
            Console.WriteLine("- Passwords and authentication");
            Console.WriteLine("- Phishing and scams");
            Console.WriteLine("- Online privacy");
            Console.WriteLine("- Safe browsing");
            Console.WriteLine("- Malware protection");
            Console.ResetColor();
        }

        private static void ProcessUserInput(string userInput)
        {
            string response = HandleUserInput(userInput);

            Console.ForegroundColor = ConsoleColor.Cyan;
            TypeEffect("ChatBot02: " + response + "\n");
            synthesizer.Speak(response);
            Console.ResetColor();
        }

        // Main input handler that coordinates all features
        private static string HandleUserInput(string userInput)
        {
            if (string.IsNullOrWhiteSpace(userInput))
            {
                return "I didn't receive your message. Could you please repeat that?";
            }

            // Check for memory storage opportunities first
            string memoryResponse = HandleMemory(userInput);
            if (memoryResponse != null) return memoryResponse;

            // Check sentiment
            string sentimentResponse = DetectSentiment(userInput);
            if (sentimentResponse != null) return sentimentResponse;

            // Handle conversation flow
            string flowResponse = HandleConversationFlow(userInput);
            if (flowResponse != null) return flowResponse;

            // Check for keywords
            string keywordResponse = RecognizeKeywords(userInput);
            if (keywordResponse != null) return keywordResponse;

            // Default response if nothing else matched
            return "I'm not sure I understand. Could you try rephrasing or ask about cybersecurity topics like passwords, scams, or privacy?";
        }

        #region Core Feature Implementations

        private static string HandleMemory(string userInput)
        {
            userInput = userInput.ToLower(); // ensure lowercase for comparison

            // Store user's name if not already stored
            if (userInput.StartsWith("my name is "))
            {
                string name = userInput.Substring("my name is ".Length).Trim();
                userMemory["name"] = name;
                return $"Nice to meet you, {name}! I'll remember that.";
            }

            // Store user's interests
            foreach (var keyword in keywordResponses.Keys)
            {
                if (userInput.Contains($"i'm interested in {keyword}") ||
                    userInput.Contains($"i like {keyword}"))
                {
                    userMemory["interest"] = keyword;
                    return $"Great! I'll remember that you're interested in {keyword}. It's a crucial part of staying safe online.";
                }
            }

            // Use stored information to personalize responses
            if (userMemory.ContainsKey("interest") &&
                (userInput.Contains("suggestion") || userInput.Contains("advice")))
            {
                string interest = userMemory["interest"];
                string name = userMemory.ContainsKey("name") ? userMemory["name"] : "you";
                return $"As someone interested in {interest}, {name} might want to review the security settings on your accounts.";
            }

            return null;
        }

        private static string DetectSentiment(string userInput)
        {
            userInput = userInput.ToLower();

            if (userInput.Contains("worried") || userInput.Contains("concerned") || userInput.Contains("anxious"))
            {
                return "It's completely understandable to feel that way. Cybersecurity can be overwhelming, but I'm here to help you stay safe.";
            }
            else if (userInput.Contains("angry") || userInput.Contains("frustrated") || userInput.Contains("annoyed"))
            {
                return "I understand your frustration. Dealing with cybersecurity issues can be challenging. Let's work through this together.";
            }
            else if (userInput.Contains("happy") || userInput.Contains("excited") || userInput.Contains("great"))
            {
                return "I'm glad you're feeling positive about cybersecurity! It's great to be proactive about online safety.";
            }
            else if (userInput.Contains("confused") || userInput.Contains("unsure") || userInput.Contains("don't understand"))
            {
                return "No worries at all! Cybersecurity can be complex. Let me explain this in a simpler way.";
            }

            return null;
        }

        private static string HandleConversationFlow(string userInput)
        {
            userInput = userInput.ToLower();

            // Check if user is asking for more information
            if (userInput.Contains("more") || userInput.Contains("explain") || userInput.Contains("details"))
            {
                if (currentTopic != null && keywordResponses.ContainsKey(currentTopic))
                {
                    // Provide another random response on the same topic
                    var responses = keywordResponses[currentTopic];
                    return responses[rand.Next(responses.Count)];
                }
            }

            // Check for new topic
            foreach (var keyword in keywordResponses.Keys)
            {
                if (userInput.Contains(keyword))
                {
                    currentTopic = keyword;
                    var responses = keywordResponses[keyword];
                    return responses[rand.Next(responses.Count)];
                }
            }

            return null;
        }

        private static string RecognizeKeywords(string userInput)
        {
            userInput = userInput.ToLower();

            foreach (var keyword in keywordResponses.Keys)
            {
                if (userInput.Contains(keyword))
                {
                    currentTopic = keyword;
                    var responses = keywordResponses[keyword];
                    return responses[rand.Next(responses.Count)];
                }
            }
            return null;
        }

        #endregion

        #region User Interface Methods

        private static void ShowMenu()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n========================================");
            Console.WriteLine("           CHATBOT02 MAIN MENU         ");
            Console.WriteLine("========================================");
            Console.WriteLine("Choose an option:");
            Console.WriteLine("(1) Password security");
            Console.WriteLine("(2) Phishing protection");
            Console.WriteLine("(3) Safe browsing");
            Console.WriteLine("(4) Privacy tips");
            Console.WriteLine("(5) Avoiding scams");
            Console.WriteLine("(6) Two-factor authentication");
            Console.WriteLine("(7) Malware protection");
            Console.WriteLine("(8) What can I ask you about");
            Console.WriteLine("(9) Exit");
            Console.ResetColor();

            Console.Write("\nEnter your choice (1-9): ");
            string choice = Console.ReadLine();

            if (choice == null) return;

            choice = choice.ToLower();

            string response;
            switch (choice)
            {
                case "1":
                    response = GetRandomResponse("password");
                    break;
                case "2":
                    response = GetRandomResponse("phishing");
                    break;
                case "3":
                    response = GetRandomResponse("browsing");
                    break;
                case "4":
                    response = GetRandomResponse("privacy");
                    break;
                case "5":
                    response = GetRandomResponse("scam");
                    break;
                case "6":
                    response = GetRandomResponse("authentication");
                    break;
                case "7":
                    response = GetRandomResponse("malware");
                    break;
                case "8":
                    response = "You can ask me about passwords, phishing, browsing safely, privacy, scams, malware, and more!";
                    break;
                case "9":
                    response = "exit";
                    break;
                default:
                    response = "I didn't quite understand that. Please enter a number between 1 and 9.";
                    break;
            }

            if (response == "exit")
            {
                ExitChatbot();
                Environment.Exit(0);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                TypeEffect("ChatBot02: " + response + "\n");
                synthesizer.Speak(response);
                Console.ResetColor();
            }
        }

        private static string GetRandomResponse(string topic)
        {
            if (keywordResponses.ContainsKey(topic))
            {
                var responses = keywordResponses[topic];
                return responses[rand.Next(responses.Count)];
            }
            return "I'm not sure about that topic. Could you ask something else?";
        }

        private static void TypeEffect(string message)
        {
            foreach (char c in message)
            {
                Console.Write(c);
                Thread.Sleep(30);
            }
        }

        private static void ExitChatbot()
        {
            string namePart = userMemory.ContainsKey("name") ? $"Thank you for chatting, {userMemory["name"]}. Stay safe online! Goodbye." : "Thank you for chatting. Stay safe online! Goodbye.";
            Console.ForegroundColor = ConsoleColor.Magenta;
            TypeEffect(namePart + "\n");
            synthesizer.Speak(namePart);
            Console.ResetColor();
        }

        #endregion
    }
}
