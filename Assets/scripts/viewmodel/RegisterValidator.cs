using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class RegisterValidator : MonoBehaviour
{
    public TMP_InputField usernameField;
    public TMP_InputField emailField;
    public TMP_InputField passwordField;
    public TMP_InputField confirmPasswordField;
    public TMP_Text feedbackText;

    public void TryRegister()
    {
        string username = usernameField.text.Trim();
        string email = emailField.text.Trim();
        string password = passwordField.text;
        string confirmPassword = confirmPasswordField.text;

        feedbackText.text = "";

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(email) ||
            string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPassword))
        {
            feedbackText.text = "Please fill in all fields.";
            return;
        }

        if (!IsValidEmail(email))
        {
            feedbackText.text = "Please enter a valid email address.";
            return;
        }

        if (password.Length < 6)
        {
            feedbackText.text = "Password must be at least 6 characters.";
            return;
        }

        if (password != confirmPassword)
        {
            feedbackText.text = "Passwords do not match.";
            return;
        }

        // All validations passed — proceed
        feedbackText.text = "";
        Debug.Log("Registration validated. Loading homepage...");
        SceneManager.LoadScene("homepage");
    }

    private bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }
}
