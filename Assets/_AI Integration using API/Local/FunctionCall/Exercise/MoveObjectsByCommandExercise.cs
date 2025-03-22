using System.Collections.Generic;
using System.Reflection;
using LLMUnity;
using LLMUnitySamples;
using UnityEngine;
using UnityEngine.UI;

public class MoveObjectsByCommandExercise : MonoBehaviour
{
    public LLMCharacter llmCharacter;
    public InputField playerText;
    public RectTransform blueSquare;
    public RectTransform redSquare;
    public string colorF, dirF;
    void Start()
    {
        playerText.onSubmit.AddListener(onInputFieldSubmit);
        playerText.Select();
    }

    string[] GetFunctionNames<T>()
    {
        List<string> functionNames = new List<string>();
        foreach (var function in typeof(T).GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly))
            functionNames.Add(function.Name);
        return functionNames.ToArray();
    }
    string ConstructPrompt<T>(string message)
    {
        // construct the prompt for the AI to understand direction commands
        string prompt = "Which of the following choices matches best the input?\n\n";
        prompt += "Input:" + message + "\n\n";
        prompt += "Choices:\n";
        foreach (string functionName in GetFunctionNames<T>()) prompt += $"- {functionName}\n";
        prompt += "\nAnswer directly with the choice";
        return prompt;

    }

    async void onInputFieldSubmit(string message)
    {
        /* Example prompts and test cases for students:
         * 
         * Test inputs:
         * - "move the blue square up"
         * - "move red square to the right"
         * - "make the blue square go down"
         * - "move the red square left"
         * 
         * Expected AI responses examples:
         * - Direction: "MoveUp", "MoveRight", "MoveDown", "MoveLeft", "NoDirectionsMentioned"
         * - Color: "BlueColor", "RedColor", "NoColorMentioned"
         */


        Debug.Log(message);
        playerText.text = "";
        playerText.gameObject.SetActive(false);
        string dirFunctionName = await llmCharacter.Chat(ConstructPrompt<DirectionFunctions>(message));
        Debug.Log(dirFunctionName);
        string colorFunctionName = await llmCharacter.Chat(ConstructPrompt<ColorFunctions>(message));
        Debug.Log(colorFunctionName);

        Color color = (Color)typeof(ColorFunctions).GetMethod(colorFunctionName).Invoke(null, null);
        Vector3 direction = (Vector3)typeof(DirectionFunctions).GetMethod(dirFunctionName).Invoke(null, null);
        GetObjectByColor(color).transform.position += direction * 80f;
        playerText.gameObject.SetActive(true);

    }

    private RectTransform GetObjectByColor(Color color)
    {
        if (color == Color.blue)
            return blueSquare;
        else if (color == Color.red)
            return redSquare;

        return null;
    }

    public void CancelRequests()
    {
        llmCharacter.CancelRequests();
    }

    public void ExitGame()
    {
        Debug.Log("Exit button clicked");
        Application.Quit();
    }

    bool onValidateWarning = true;
    void OnValidate()
    {
        if (onValidateWarning && !llmCharacter.remote && llmCharacter.llm != null && llmCharacter.llm.model == "")
        {
            Debug.LogWarning($"Please select a model in the {llmCharacter.llm.gameObject.name} GameObject!");
            onValidateWarning = false;
        }
    }
}