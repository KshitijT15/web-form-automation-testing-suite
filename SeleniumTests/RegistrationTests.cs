using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using Newtonsoft.Json;

public class TestUser {
    public string? Name, Email, Phone, Password, FailField;
    public bool IsValid;
}

[TestFixture]
public class RegistrationTests {
    IWebDriver driver = null!;
    WebDriverWait wait = null!;
    const string URL = "http://localhost:5148";

    [SetUp]
    public void Setup() {
        driver = new ChromeDriver();
        wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
    }

    [TearDown]
    public void Cleanup() => driver.Quit();

    void Fill(string name, string email, string phone, string pass) {
        driver.Navigate().GoToUrl(URL);
        wait.Until(d => d.FindElement(By.Id("Name")));
        driver.FindElement(By.Id("Name")).SendKeys(name);
        driver.FindElement(By.Id("Email")).SendKeys(email);
        driver.FindElement(By.Id("Phone")).SendKeys(phone);
        driver.FindElement(By.Id("Password")).SendKeys(pass);
        driver.FindElement(By.Id("submit-btn")).Click();
    }

    IWebElement WaitForText(string id) =>
        wait.Until(d => {
            var el = d.FindElement(By.Id(id));
            return el.Text.Trim().Length > 0 ? el : null;
        })!;

    static List<TestUser> LoadUsers() {
        var path = Path.Combine(
            TestContext.CurrentContext.TestDirectory,
            "TestData", "test_data.json");
        return JsonConvert.DeserializeObject<List<TestUser>>(
            File.ReadAllText(path))!;
    }

    // Runs one test per valid user in the JSON
    [Test, TestCaseSource(nameof(ValidUsers))]
    public void ValidUser_Succeeds(TestUser u) {
        Fill(u.Name!, u.Email!, u.Phone!, u.Password!);
        Assert.That(WaitForText("success-message").Text, Does.Contain("successful"));
    }

    // Runs one test per invalid user in the JSON
    [Test, TestCaseSource(nameof(InvalidUsers))]
    public void InvalidUser_ShowsError(TestUser u) {
    Fill(u.Name!, u.Email!, u.Phone!, u.Password!);

    // Map failField to the correct error span id
    var errorId = (u.FailField ?? "").ToLower() switch {
        "email"    => "email-error",
        "password" => "password-error",
        "name"     => "name-error",
        // Old JSON has no failField — detect from data
        _ when string.IsNullOrEmpty(u.Name)     => "name-error",
        _ when !u.Email!.Contains("@")          => "email-error",
        _ when u.Password!.Length < 8           => "password-error",
        _                                       => "name-error"
    };

    Assert.That(WaitForText(errorId).Text.Length, Is.GreaterThan(0),
        $"Expected error on: {errorId} for user: {u.Email}");
    }

    // Data sources — NUnit calls these to build test cases
    static IEnumerable<TestCaseData> ValidUsers() =>
        LoadUsers().Where(u => u.IsValid)
                   .Select(u => new TestCaseData(u).SetName($"Valid: {u.Name} ({u.Email})"));

    static IEnumerable<TestCaseData> InvalidUsers() =>
        LoadUsers().Where(u => !u.IsValid)
                   .Select(u => new TestCaseData(u).SetName($"Invalid [{u.FailField}]: {u.Email}"));
}