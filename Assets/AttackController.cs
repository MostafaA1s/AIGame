using LLMUnity;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class AttackController : MonoBehaviour
{
    public LLMCharacter llmCharacter;
    public AttackTypes attackTypes;
    public GameObject attackPrefab;
    public GameObject spawnPoint;

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

    public async void OnCommandSubmit(string message)
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

        string commandName = await llmCharacter.Chat(ConstructPrompt<GameFunctions>(message));
        Debug.Log(commandName);


        attackPrefab = (GameObject)typeof(AttackTypes).GetMethod(commandName).Invoke(attackTypes, null);
        if (attackPrefab != null)
            SpawnAttack();

    }



    void SpawnAttack()
    {
        Instantiate(attackPrefab, spawnPoint.transform.position, spawnPoint.transform.rotation);
    }

    private RectTransform GetObjectByColor(Color color)
    {
        if (color == Color.blue) ;
        //return blueSquare;
        else if (color == Color.red) ;
        //  return redSquare;

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
