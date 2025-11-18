using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Mscc.GenerativeAI;

namespace OnlineEduTaskServices.Services
{
    public class GeminiAIService
    {
        private readonly string _apiKey;
        private readonly GoogleAI _googleAI;

        // Cache đơn giản prompt -> kết quả
        private readonly ConcurrentDictionary<string, (double score, string feedback)> _gradingCache = new();
        private readonly ConcurrentDictionary<string, (string title, string description)> _taskCache = new();
        private readonly ConcurrentDictionary<string, string[]> _ideasCache = new();

        public GeminiAIService()
        {
            // Read API key from environment variable for security
            _apiKey = Environment.GetEnvironmentVariable("GEMINI_API_KEY") 
                ?? "AIzaSyC_14FO0WH8Y1jvfDuVjnsBT5Ns33wN9Go"; // Fallback for local dev only
            _googleAI = new GoogleAI(_apiKey);
        }

        public GeminiAIService(string apiKey)
        {
            _apiKey = apiKey ?? throw new ArgumentNullException(nameof(apiKey));
            _googleAI = new GoogleAI(_apiKey);
        }

        public async Task<(double suggestedScore, string feedback)> GenerateGradingSuggestion(
            string taskTitle,
            string taskDescription,
            string submissionContent,
            int maxScore)
        {
            string cacheKey = $"{taskTitle}|{taskDescription}|{submissionContent}|{maxScore}";
            if (_gradingCache.TryGetValue(cacheKey, out var cached))
                return cached;

            var truncatedSubmission = submissionContent.Length > 2000
                ? submissionContent.Substring(0, 2000) + "..."
                : submissionContent;

            var truncatedDescription = taskDescription.Length > 500
                ? taskDescription.Substring(0, 500) + "..."
                : taskDescription;

            var prompt = $@"Grade this assignment. Max: {maxScore}

Task: {taskTitle}
Requirements: {truncatedDescription}

Submission: {truncatedSubmission}

Respond EXACTLY as:
SCORE: [0-{maxScore}]
FEEDBACK: [Vietnamese feedback: strengths, improvements, encouragement]";

            int retry = 0;
            const int maxRetry = 2;
            while (true)
            {
                try
                {
                    var model = _googleAI.GenerativeModel("gemini-2.5-flash");
                    var responseTask = model.GenerateContent(prompt);
                    var timeoutTask = Task.Delay(30000);
                    var completed = await Task.WhenAny(responseTask, timeoutTask);

                    if (completed == timeoutTask)
                        throw new TimeoutException("AI request timed out after 30 seconds");

                    var response = await responseTask;
                    if (response?.Text == null)
                        throw new Exception("AI response is empty");

                    var result = ParseGradingResponse(response.Text, maxScore);
                    _gradingCache[cacheKey] = result;
                    return result;
                }
                catch (TimeoutException)
                {
                    retry++;
                    if (retry > maxRetry) throw;
                }
            }
        }

        public async Task<string> EnhanceFeedback(string draftFeedback, double score, int maxScore)
        {
            try
            {
                var prompt = $@"You are a compassionate teacher assistant. A teacher has written this feedback for a student who scored {score}/{maxScore}:

{draftFeedback}

Please enhance this feedback by:
1. Making it more detailed and constructive
2. Adding specific encouragement appropriate for the score level
3. Keeping a supportive and motivating tone
4. Writing in Vietnamese
5. Keeping it concise (2-4 sentences)

Return ONLY the enhanced feedback, no labels or extra text.";

                var model = _googleAI.GenerativeModel("gemini-2.5-flash");
                var response = await model.GenerateContent(prompt);

                if (string.IsNullOrEmpty(response?.Text))
                    return draftFeedback;

                return response.Text.Trim();
            }
            catch
            {
                return draftFeedback;
            }
        }

        public async Task<(string title, string description)> GenerateTaskAssignment(
            string subjectName,
            string topic,
            string difficultyLevel = "medium",
            string additionalRequirements = "")
        {
            string cacheKey = $"{subjectName}|{topic}|{difficultyLevel}|{additionalRequirements}";
            if (_taskCache.TryGetValue(cacheKey, out var cached))
                return cached;

            var prompt = $@"Create a learning assignment for students.

Subject: {subjectName}
Topic: {topic}
Difficulty: {difficultyLevel}
{(string.IsNullOrEmpty(additionalRequirements) ? "" : $"Requirements: {additionalRequirements}")}

Generate:
1. A clear, engaging title (in Vietnamese)
2. Detailed description with:
   - Learning objectives
   - What students need to do
   - Expected deliverables
   - Evaluation criteria

Format EXACTLY as:
TITLE: [assignment title]
DESCRIPTION: [detailed description in Vietnamese]";

            int retry = 0;
            const int maxRetry = 2;
            while (true)
            {
                try
                {
                    var model = _googleAI.GenerativeModel("gemini-2.5-flash");
                    var responseTask = model.GenerateContent(prompt);
                    var timeoutTask = Task.Delay(40000);
                    var completed = await Task.WhenAny(responseTask, timeoutTask);

                    if (completed == timeoutTask)
                        throw new TimeoutException("AI request timed out");

                    var response = await responseTask;

                    if (response?.Text == null)
                        throw new Exception("AI response is empty");

                    var result = ParseTaskResponse(response.Text);
                    _taskCache[cacheKey] = result;
                    return result;
                }
                catch (TimeoutException)
                {
                    retry++;
                    if (retry > maxRetry) throw;
                }
            }
        }

        public async Task<string[]> GenerateTaskIdeas(string subjectName, string topic, int count = 3)
        {
            string cacheKey = $"{subjectName}|{topic}|{count}";
            if (_ideasCache.TryGetValue(cacheKey, out var cached))
                return cached;

            var prompt = $@"Suggest {count} assignment ideas for:

Subject: {subjectName}
Topic: {topic}

Give {count} brief, creative assignment ideas in Vietnamese. Each on a new line starting with number.
Format:
1. [idea]
2. [idea]
3. [idea]";

            try
            {
                var model = _googleAI.GenerativeModel("gemini-2.5-flash");
                var response = await model.GenerateContent(prompt);

                if (response?.Text == null)
                {
                    return new[] { "Bài tập nghiên cứu và trình bày", "Bài tập thực hành", "Bài tập áp dụng kiến thức" };
                }

                var lines = response.Text.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
                var ideas = new System.Collections.Generic.List<string>();

                foreach (var line in lines)
                {
                    var trimmed = line.Trim();
                    if (trimmed.Length > 0)
                    {
                        var cleaned = System.Text.RegularExpressions.Regex.Replace(trimmed, @"^\d+\.\s*", "");
                        if (!string.IsNullOrEmpty(cleaned))
                        {
                            ideas.Add(cleaned);
                        }
                    }
                }

                var result = ideas.Count > 0 ? ideas.ToArray() : new[] { "Bài tập nghiên cứu và trình bày" };
                _ideasCache[cacheKey] = result;
                return result;
            }
            catch
            {
                return new[] { "Bài tập nghiên cứu và trình bày", "Bài tập thực hành", "Bài tập áp dụng kiến thức" };
            }
        }

        private (string title, string description) ParseTaskResponse(string response)
        {
            try
            {
                var lines = response.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

                string title = "";
                string description = "";
                bool foundTitle = false;
                bool foundDescription = false;

                foreach (var line in lines)
                {
                    if (line.StartsWith("TITLE:", StringComparison.OrdinalIgnoreCase))
                    {
                        title = line.Substring(6).Trim();
                        foundTitle = true;
                    }
                    else if (line.StartsWith("DESCRIPTION:", StringComparison.OrdinalIgnoreCase))
                    {
                        description = line.Substring(12).Trim();
                        foundDescription = true;
                    }
                    else if (foundDescription)
                    {
                        description += "\n" + line;
                    }
                }

                if (!foundTitle || !foundDescription)
                {
                    throw new Exception("Invalid response format");
                }

                return (title.Trim(), description.Trim());
            }
            catch
            {
                return ("Bài tập cần tạo thủ công", "Vui lòng tự viết mô tả bài tập.");
            }
        }

        private (double score, string feedback) ParseGradingResponse(string response, int maxScore)
        {
            try
            {
                var lines = response.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

                double score = 0;
                string feedback = "";

                bool foundScore = false;
                bool foundFeedback = false;

                foreach (var line in lines)
                {
                    if (line.StartsWith("SCORE:", StringComparison.OrdinalIgnoreCase))
                    {
                        var scoreText = line.Substring(6).Trim();
                        if (double.TryParse(scoreText, out double parsedScore))
                        {
                            score = Math.Min(Math.Max(parsedScore, 0), maxScore); // Clamp between 0 and maxScore
                            foundScore = true;
                        }
                    }
                    else if (line.StartsWith("FEEDBACK:", StringComparison.OrdinalIgnoreCase))
                    {
                        feedback = line.Substring(9).Trim();
                        foundFeedback = true;
                    }
                    else if (foundFeedback)
                    {
                        feedback += "\n" + line;
                    }
                }

                if (!foundScore || !foundFeedback)
                {
                    throw new Exception("Invalid AI response format");
                }

                return (score, feedback.Trim());
            }
            catch
            {
                return (maxScore / 2.0, "Bài làm cần được đánh giá thêm. Hãy xem lại yêu cầu và hoàn thiện thêm.");
            }
        }
    }
}
