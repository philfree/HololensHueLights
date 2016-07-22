using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HelpMenuVoiceCommands : MonoBehaviour {


    public TextMesh messageBody;
    public float distanceFromUser = 1;

    private string formattedText;


    void Start()
    {
        messageBody = GetComponent<TextMesh>();
        messageBody.text = "";

        //formattedText = "This is a really long line of texts about cats. They are wonderful and adorable. We all know that. Wahooo!";
        setMessageBody();
    }

    // Update is called once per frame
    void Update()
    {
        //setMessageBody();
    }

    public void CreateVoiceControlMenu(Dictionary<string, System.Action> keywords)
    {
        foreach(KeyValuePair<string, System.Action> phrase in keywords)
        {
            formattedText += phrase.Key + "\n";
        }
        setMessageBody();
        ShowVCMenu(false);
    }

    public void ShowVCMenu(bool show)
    {

        this.transform.parent.gameObject.SetActive(show);

        var headPosition = Camera.main.transform.position;
        var gazeDirection = Camera.main.transform.forward;

        this.transform.parent.gameObject.transform.position = headPosition + gazeDirection * distanceFromUser;
        // Rotate this object to face the user.
        Quaternion toQuat = Camera.main.transform.localRotation;
        toQuat.x = 0;
        toQuat.z = 0;
        this.transform.parent.rotation = toQuat;
    }

    void setMessageBody()
    {
        messageBody.text = FormatTextLayout(formattedText, 300);
    }

    private string FormatTextLayout(string input, int lineLength)
    {
        string[] message;
        string result = "";

        if (input != null)
        {
            message = input.Split(" "[0]);

            // Temp line string
            string line = "";

            foreach (string s in message)
            {
                string temp = line + " " + s;

                // If line length is bigger than lineLength
                if (temp.Length > lineLength)
                {
                    result += line + "\n";

                    // Append remaining word to new line
                    line = s;
                }
                else
                {
                    line = temp;
                }
            }
            // Append last line into result
            result += line;

            // Remove first " " char
            result = result.Substring(1, result.Length - 1);
        }
        return result;
    }
}
