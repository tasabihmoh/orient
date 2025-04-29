using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class SignInValidator : MonoBehaviour
{
    public TMP_InputField emailField;
    public TMP_InputField passwordField;
    public TMP_Text feedbackText;

    public void TrySignIn()
    {
        string email = emailField.text.Trim();
        string password = passwordField.text.Trim();

        // Clear previous feedback
        feedbackText.text = "";

        if ( string.IsNullOrEmpty(email)|| string.IsNullOrEmpty(password) )
        {
            feedbackText.text = "Please fill in all fields.";
            return;
        }
        if (email == "")
        {
            feedbackText.text = "Email is required.";
            return;
        }

        if (password == "")
        {
            feedbackText.text = "Password is required.";
            return;
        }

        // If we got here, both fields are filled correctly
        feedbackText.text = "";
        SceneManager.LoadScene("homepage");
    }
}
