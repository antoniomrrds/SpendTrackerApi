/// <summary>
/// 🔍 Advanced Commit Message Linter
/// Following Conventional Commits specification
/// Specification: https://www.conventionalcommits.org/en/v1.0.0/
/// Angular Commit Guidelines: https://github.com/angular/angular/blob/22b96b9/CONTRIBUTING.md#type
/// </summary>

using System;
using System.IO;
using System.Text.RegularExpressions;

// Regex pattern for Conventional Commits validation
string pattern = 
    @"^(?=.{1,90}$)" +                                  // 📏 Length validation (1-90 chars)
    @"(?:build|chore|ci|docs|feat|fix|perf|refactor|revert|style|test)" + // 🔖 Valid types
    @"(?:\([a-zA-Z0-9\-_]+\))?" +                       // 🎯 Optional scope
    @":\s+" +                                           // ➡️ Colon with required space
    @".{4,}" +                                          // 📝 Subject (min 4 chars)
    @"(?:\s+#\d+)?" +                                   // 🔗 Optional issue number
    @"(?<![\.\s])$";                                    // 🚫 No trailing dot or space

if (Args.Count == 0)
{
    Console.WriteLine("❌ No commit message file provided.");
    return 1;
}

string msg = File.ReadAllLines(Args[0])[0].Trim();
bool hasErrors = false;

if (Regex.IsMatch(msg, pattern))
{
    // ✅ Valid commit message
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine("✅ Valid commit message! 🎉");
    Console.ResetColor();

    var match = Regex.Match(msg, @"^(?<type>\w+)(?:\((?<scope>[^)]+)\))?:\s+(?<subject>.+?)(?:\s+#(?<issue>\d+))?$");

    Console.WriteLine("\n📊 Analysis:");
    Console.WriteLine($"  🔖 Type:    {match.Groups["type"].Value}");
    if (match.Groups["scope"].Success)
        Console.WriteLine($"  🎯 Scope:   {match.Groups["scope"].Value}");
    Console.WriteLine($"  📝 Subject: {match.Groups["subject"].Value}");
    if (match.Groups["issue"].Success)
        Console.WriteLine($"  🔗 Issue:   #{match.Groups["issue"].Value}");
    Console.WriteLine($"  📏 Length:  {msg.Length}/90 characters");
    
    return 0;
}
else
{
    // ❌ Invalid commit message - detailed analysis
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine("❌ Invalid commit message");
    Console.ResetColor();

    Console.WriteLine($"\n📝 Your message: \"{msg}\"");
    Console.WriteLine($"📏 Length: {msg.Length}/90 characters");

    // 🔍 DETAILED ERROR ANALYSIS (only shows if error occurs)
    
    // 1. 📏 SIZE ERROR
    if (msg.Length > 90)
    {
        hasErrors = true;
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"\n🚫 SIZE ERROR: Message too long (+{msg.Length - 90} characters)");
        Console.ResetColor();
        Console.WriteLine("💡 How to fix:");
        Console.WriteLine("  • Remove unnecessary words");
        Console.WriteLine("  • Use abbreviations where possible");
        Console.WriteLine("  • Break into multiple commits if needed");
        Console.WriteLine("📝 Example:");
        Console.WriteLine("  ❌ 'feat(veryLongScope): implement extremely complex business logic that requires...'");
        Console.WriteLine("  ✅ 'feat(scope): implement core business logic'");
    }

    // 2. 🔖 TYPE ERROR
    if (!Regex.IsMatch(msg, @"^(?:build|chore|ci|docs|feat|fix|perf|refactor|revert|style|test)"))
    {
        hasErrors = true;
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"\n🚫 TYPE ERROR: Invalid or missing commit type");
        Console.ResetColor();
        Console.WriteLine("📋 Allowed types:");
        Console.WriteLine("  🎯 feat     - New feature");
        Console.WriteLine("  🐛 fix      - Bug fix");
        Console.WriteLine("  📚 docs     - Documentation");
        Console.WriteLine("  🎨 style    - Code style/formatting");
        Console.WriteLine("  🔧 refactor - Code refactoring");
        Console.WriteLine("  ⚡ perf     - Performance improvements");
        Console.WriteLine("  🧪 test     - Adding tests");
        Console.WriteLine("  🏗️  build    - Build system changes");
        Console.WriteLine("  🔄 ci       - CI configuration");
        Console.WriteLine("  🛠️  chore    - Maintenance tasks");
        Console.WriteLine("  ⏪ revert   - Revert previous commit");
        Console.WriteLine("📝 Examples:");
        Console.WriteLine("  ❌ 'feature: add new thing'");
        Console.WriteLine("  ✅ 'feat: add new thing'");
        Console.WriteLine("  ❌ 'bugfix: resolve issue'");
        Console.WriteLine("  ✅ 'fix: resolve issue'");
    }

    // 3. 🎯 SCOPE ERROR
    if (Regex.IsMatch(msg, @"^[a-z]+\([^)]*[^a-zA-Z0-9\-_][^)]*\)"))
    {
        hasErrors = true;
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"\n🚫 SCOPE ERROR: Invalid characters in scope");
        Console.ResetColor();
        Console.WriteLine("💡 Allowed in scope: letters, numbers, hyphens, underscores");
        Console.WriteLine("📝 Examples:");
        Console.WriteLine("  ❌ 'feat(auth service): login'");
        Console.WriteLine("  ✅ 'feat(auth-service): login'");
        Console.WriteLine("  ❌ 'fix(api.v2): error'");
        Console.WriteLine("  ✅ 'fix(api-v2): error'");
        Console.WriteLine("  ❌ 'feat(user@email): feature'");
        Console.WriteLine("  ✅ 'feat(user-email): feature'");
    }

    // 4. ➡️ FORMAT ERROR
    if (!msg.Contains(": "))
    {
        hasErrors = true;
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"\n🚫 FORMAT ERROR: Missing colon and space after type/scope");
        Console.ResetColor();
        Console.WriteLine("💡 Required format: 'type(scope): subject'");
        Console.WriteLine("📝 Examples:");
        Console.WriteLine("  ❌ 'feat:add login'");
        Console.WriteLine("  ✅ 'feat: add login'");
        Console.WriteLine("  ❌ 'feat(auth):add login'");
        Console.WriteLine("  ✅ 'feat(auth): add login'");
    }

    // 5. 📝 SUBJECT ERROR
    if (Regex.IsMatch(msg, @":\s*.{0,3}$"))
    {
        hasErrors = true;
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"\n🚫 SUBJECT ERROR: Description too short (minimum 4 characters)");
        Console.ResetColor();
        Console.WriteLine("💡 Make your commit message more descriptive");
        Console.WriteLine("📝 Examples:");
        Console.WriteLine("  ❌ 'feat: add'");
        Console.WriteLine("  ✅ 'feat: add user login'");
        Console.WriteLine("  ❌ 'fix: bug'");
        Console.WriteLine("  ✅ 'fix: resolve memory leak'");
    }

    // 6. 🚫 TRAILING CHARACTERS ERROR
    if (msg.EndsWith(".") || msg.EndsWith(" "))
    {
        hasErrors = true;
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"\n🚫 TRAILING ERROR: Ends with dot or space");
        Console.ResetColor();
        Console.WriteLine("💡 Remove the trailing character");
        Console.WriteLine("📝 Examples:");
        Console.WriteLine("  ❌ 'feat: add login. '");
        Console.WriteLine("  ✅ 'feat: add login'");
        Console.WriteLine("  ❌ 'fix: resolve issue. '");
        Console.WriteLine("  ✅ 'fix: resolve issue'");
    }

    // 7. 🔗 ISSUE NUMBER ERROR
    if (Regex.IsMatch(msg, @"#\D") || msg.EndsWith("#"))
    {
        hasErrors = true;
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"\n🚫 ISSUE ERROR: Invalid issue number format");
        Console.ResetColor();
        Console.WriteLine("💡 Issue numbers must be numeric with space before #");
        Console.WriteLine("📝 Examples:");
        Console.WriteLine("  ❌ 'feat: add feature #abc'");
        Console.WriteLine("  ✅ 'feat: add feature #123'");
        Console.WriteLine("  ❌ 'feat: add feature#'");
        Console.WriteLine("  ✅ 'feat: add feature #456'");
        Console.WriteLine("  ❌ 'feat: add feature#123'");
        Console.WriteLine("  ✅ 'feat: add feature #123'");
    }

    // Show general help if no specific errors detected (edge cases)
    if (!hasErrors)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"\n⚠️  General format error detected");
        Console.ResetColor();
        Console.WriteLine("💡 Your message doesn't match the expected format");
    }

    Console.WriteLine($"\n🎯 Valid examples:");
    Console.WriteLine("  feat(auth): add password reset functionality");
    Console.WriteLine("  fix(api): resolve null reference in user service");
    Console.WriteLine("  docs: update installation guide #45");
    Console.WriteLine("  chore(deps): update packages to latest versions");
    Console.WriteLine("  refactor: simplify data access layer");

    Console.WriteLine($"\n📚 Documentation:");
    Console.WriteLine("  📖 Conventional Commits: https://www.conventionalcommits.org");
    Console.WriteLine("  📖 Angular Guidelines: https://github.com/angular/angular/blob/main/CONTRIBUTING.md#type");

    return 1;
}