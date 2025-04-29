using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class dropdownex : MonoBehaviour
{
    [SerializeField] private TMP_Text room_text;

    // This array holds the room type descriptions
    private string[] roomTypes = {
        "Deluxe Room",
        "Premium Room",
        "Family connected Room",
        "Executive Jacuzzi Room"
    };

    public void DropDownSample(int index)
    {
        if (index >= 0 && index < roomTypes.Length)
        {
            string chosenRoom = roomTypes[index];
            room_text.text = $"You have chosen {chosenRoom}. Let's go ahead with the proceedings.";
        }
        else
        {
            room_text.text = "Please choose a room type.";
        }
    }
}