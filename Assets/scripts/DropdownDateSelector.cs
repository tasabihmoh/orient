using UnityEngine;
using TMPro;
using System;
using System.Collections.Generic;
using UnityEngine.UI;

public class DropdownDateSelector : MonoBehaviour
{
    public TMP_Dropdown checkInDayDropdown;
    public TMP_Dropdown checkInMonthDropdown;
    public TMP_Dropdown checkInYearDropdown;
    public TMP_Dropdown checkOutDayDropdown;
    public TMP_Dropdown checkOutMonthDropdown;
    public TMP_Dropdown checkOutYearDropdown;
    public TMP_Dropdown RoomDropdown; // For room selection
    public TMP_Text statusText;
    public Button confirmButton; // Reference to the confirm button

    private string[] monthNames = { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
    private string[] roomOptions = { "Deluxe Room", "Premium Room", "Family Connected Room", "Executive Jacuzzi" };

    void Start()
    {
        // Hide status text initially
        statusText.gameObject.SetActive(false);

        // Set up initial dropdowns
        PopulateYears(checkInYearDropdown);
        PopulateYears(checkOutYearDropdown);
        PopulateRooms(RoomDropdown);

        // Set up event listeners to react to dropdown changes
        checkInYearDropdown.onValueChanged.AddListener(delegate { OnCheckInYearChanged(); ValidateDates(); });
        checkInMonthDropdown.onValueChanged.AddListener(delegate { OnCheckInMonthChanged(); ValidateDates(); });
        checkInDayDropdown.onValueChanged.AddListener(delegate { ValidateDates(); });
        checkOutYearDropdown.onValueChanged.AddListener(delegate { OnCheckOutYearChanged(); ValidateDates(); });
        checkOutMonthDropdown.onValueChanged.AddListener(delegate { OnCheckOutMonthChanged(); ValidateDates(); });
        checkOutDayDropdown.onValueChanged.AddListener(delegate { ValidateDates(); });
        RoomDropdown.onValueChanged.AddListener(delegate { ValidateRoomSelection(); });

        // Populate initial values
        PopulateMonths(checkInMonthDropdown, isCheckIn: true);
        PopulateMonths(checkOutMonthDropdown, isCheckIn: false);
        PopulateDays(checkInDayDropdown, isCheckIn: true);
        PopulateDays(checkOutDayDropdown, isCheckIn: false);

        // Connect the confirm button click to the ConfirmBooking function
        if (confirmButton != null)
        {
            confirmButton.onClick.AddListener(ConfirmBooking);
        }
        else
        {
            Debug.LogError("Confirm Button not assigned in the inspector!");
        }

        // Initial validations
        ValidateDates();
        ValidateRoomSelection();
    }

    void PopulateRooms(TMP_Dropdown dropdown)
    {
        dropdown.ClearOptions();

        // First add an empty/placeholder option
        List<string> options = new List<string>();
        options.Add("Please select a room");

        // Then add the actual room options
        options.AddRange(roomOptions);

        dropdown.AddOptions(options);
        dropdown.RefreshShownValue();
    }

    void ValidateRoomSelection()
    {
        // Check if a room is selected (index 0 is the placeholder "Please select a room")
        if (RoomDropdown.value == 0)
        {
            statusText.gameObject.SetActive(true);
            statusText.text = "\u274C Please select a room type.";
            statusText.color = Color.red;
            confirmButton.interactable = false;
        }
        else
        {
            // Room is selected, now validate dates as well
            ValidateDates(showRoomMessage: false);
        }
    }

    void OnCheckInYearChanged()
    {
        PopulateMonths(checkInMonthDropdown, isCheckIn: true);
        PopulateDays(checkInDayDropdown, isCheckIn: true);
    }

    void OnCheckInMonthChanged()
    {
        PopulateDays(checkInDayDropdown, isCheckIn: true);
    }

    void OnCheckOutYearChanged()
    {
        PopulateMonths(checkOutMonthDropdown, isCheckIn: false);
        PopulateDays(checkOutDayDropdown, isCheckIn: false);
    }

    void OnCheckOutMonthChanged()
    {
        PopulateDays(checkOutDayDropdown, isCheckIn: false);
    }

    void PopulateDays(TMP_Dropdown dropdown, bool isCheckIn)
    {
        // Store the current selection if any
        int currentValue = dropdown.value;
        string currentSelection = dropdown.options.Count > 0 ? dropdown.options[currentValue].text : "";

        dropdown.ClearOptions();
        List<string> days = new List<string>();

        // Get the actual month index (1-12) from the dropdown value
        TMP_Dropdown monthDropdown = isCheckIn ? checkInMonthDropdown : checkOutMonthDropdown;
        int monthValue = monthDropdown.value;
        // Convert the dropdown index to the actual month index (1-12)
        int selectedYear = GetSelectedYear(isCheckIn);
        int startMonth = selectedYear == 2025 ? 4 : 0; // May (index 4) for 2025, January (index 0) for others
        int selectedMonth = startMonth + monthValue + 1;

        int startDay = 1;
        // If it's 2025 and May, start from today's date (assuming today is in May 2025)
        if (selectedYear == 2025 && selectedMonth == 5)
        {
            startDay = 1; // Set to 1 for testing, or use DateTime.Now.Day in production
        }

        int daysInMonth = DateTime.DaysInMonth(selectedYear, selectedMonth);
        for (int i = startDay; i <= daysInMonth; i++)
            days.Add(i.ToString("D2"));

        dropdown.AddOptions(days);

        // Try to restore previous selection if valid
        if (!string.IsNullOrEmpty(currentSelection))
        {
            for (int i = 0; i < dropdown.options.Count; i++)
            {
                if (dropdown.options[i].text == currentSelection)
                {
                    dropdown.value = i;
                    break;
                }
            }
        }

        dropdown.RefreshShownValue();
    }

    void PopulateMonths(TMP_Dropdown dropdown, bool isCheckIn)
    {
        // Store the current selection if any
        int currentValue = dropdown.value;
        string currentSelection = dropdown.options.Count > 0 ? dropdown.options[currentValue].text : "";

        dropdown.ClearOptions();
        List<string> months = new List<string>();

        int selectedYear = GetSelectedYear(isCheckIn);
        int startMonth = 0; // January (0-indexed)

        // For 2025, start from May (index 4)
        if (selectedYear == 2025)
        {
            startMonth = 4; // May (0-indexed)
        }

        for (int i = startMonth; i < 12; i++)
        {
            months.Add(monthNames[i]);
        }

        dropdown.AddOptions(months);

        // Try to restore previous selection if valid
        if (!string.IsNullOrEmpty(currentSelection))
        {
            for (int i = 0; i < dropdown.options.Count; i++)
            {
                if (dropdown.options[i].text == currentSelection)
                {
                    dropdown.value = i;
                    break;
                }
            }
        }

        dropdown.RefreshShownValue();
    }

    void PopulateYears(TMP_Dropdown dropdown)
    {
        dropdown.ClearOptions();
        List<string> years = new List<string>();

        int currentYear = 2025; // Hardcoded as we want to start from 2025
        for (int i = 0; i < 6; i++)
            years.Add((currentYear + i).ToString());

        dropdown.AddOptions(years);
        dropdown.RefreshShownValue();
    }

    int GetSelectedYear(bool isCheckIn)
    {
        TMP_Dropdown yearDropdown = isCheckIn ? checkInYearDropdown : checkOutYearDropdown;
        return int.Parse(yearDropdown.options[yearDropdown.value].text);
    }

    // Helper method to get the actual month number (1-12) from dropdown value
    int GetSelectedMonth(bool isCheckIn)
    {
        TMP_Dropdown monthDropdown = isCheckIn ? checkInMonthDropdown : checkOutMonthDropdown;
        int yearValue = isCheckIn ? checkInYearDropdown.value : checkOutYearDropdown.value;
        int year = 2025 + yearValue;

        // For 2025, dropdown index 0 = May (month 5)
        // For 2026+, dropdown index 0 = January (month 1)
        int monthOffset = (year == 2025) ? 5 : 1;

        return monthOffset + monthDropdown.value;
    }

    // Validate dates before the confirm button is pressed
    public void ValidateDates(bool showRoomMessage = true)
    {
        try
        {
            // First check if a room is selected
            if (RoomDropdown.value == 0 && showRoomMessage)
            {
                ValidateRoomSelection();
                return;
            }

            // Make sure we have valid dropdowns
            if (checkInDayDropdown.options.Count == 0 || checkOutDayDropdown.options.Count == 0)
            {
                // Not enough data yet to validate
                if (!showRoomMessage)
                {
                    statusText.gameObject.SetActive(false);
                }
                return;
            }

            // Get selected dates
            DateTime checkInDate = GetSelectedDate(true);
            DateTime checkOutDate = GetSelectedDate(false);

            // Check if the dates are valid
            if (checkOutDate < checkInDate)
            {
                statusText.gameObject.SetActive(true);
                statusText.text = "\u274C Check-out date must be after check-in date. Please adjust your selection.";
                statusText.color = Color.red;
                confirmButton.interactable = false;
            }
            else if (checkOutDate == checkInDate)
            {
                statusText.gameObject.SetActive(true);
                statusText.text = "\u274C Check-in and check-out cannot be on the same day. Please book at least one night.";
                statusText.color = Color.red;
                confirmButton.interactable = false;
            }
            else if (RoomDropdown.value == 0)
            {
                // Room not selected
                ValidateRoomSelection();
            }
            else
            {
                // Valid dates and room selected, hide the status text and enable the confirm button
                statusText.gameObject.SetActive(false);
                confirmButton.interactable = true;
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Date validation error: {ex}");
            statusText.gameObject.SetActive(true);
            statusText.text = "\u274C Cannot validate dates at this time. Please try again.";
            statusText.color = Color.red;
            confirmButton.interactable = false;
        }
    }

    // Helper method to get the full DateTime for either check-in or check-out
    DateTime GetSelectedDate(bool isCheckIn)
    {
        int year = GetSelectedYear(isCheckIn);
        int month = GetSelectedMonth(isCheckIn);
        TMP_Dropdown dayDropdown = isCheckIn ? checkInDayDropdown : checkOutDayDropdown;
        int day = int.Parse(dayDropdown.options[dayDropdown.value].text);

        return new DateTime(year, month, day);
    }

    public void ConfirmBooking()
    {
        try
        {
            Debug.Log("ConfirmBooking called"); // Debug log to verify function is called

            // Make sure status text is visible
            statusText.gameObject.SetActive(true);

            // Check if a room is selected
            if (RoomDropdown.value == 0)
            {
                statusText.text = "\u274C Please select a room type.";
                statusText.color = Color.red;
                return;
            }

            // Get selected dates
            DateTime checkInDate = GetSelectedDate(true);
            DateTime checkOutDate = GetSelectedDate(false);

            // Get selected room
            string selectedRoom = RoomDropdown.options[RoomDropdown.value].text;

            // Validate dates again
            if (checkOutDate <= checkInDate)
            {
                statusText.text = "\u274C Invalid date selection. Check-out must be after check-in.";
                statusText.color = Color.red;
                return;
            }

            // Calculate stay duration
            TimeSpan duration = checkOutDate - checkInDate;
            int nights = duration.Days;

            // Format the confirmation message
            statusText.text = $"\u2705 Booking confirmed!\n\nYou chose the {selectedRoom} with check-in date {checkInDate:MMMM dd, yyyy} and check-out on {checkOutDate:MMMM dd, yyyy}.\n\nWe hope you enjoy your {nights}-night stay. Payment can be made when you checkout.";
            statusText.color = new Color(0, 0.7f, 0, 1); // Green color

            Debug.Log("Booking confirmed for " + selectedRoom); // Debug log
        }
        catch (Exception ex)
        {
            // Make sure status text is visible even on error
            statusText.gameObject.SetActive(true);
            statusText.text = $"\u274C Error: {ex.Message}";
            statusText.color = Color.red;
            Debug.LogError($"Date selection error: {ex}"); // Debug log
        }
    }
}