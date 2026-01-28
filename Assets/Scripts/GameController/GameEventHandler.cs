using System;
using System.Collections;
using UnityEngine;
using System.Reflection;
using UnityEngine.UI;

public static class EventMethods
{
    public static void Print(string message)
    {
        Debug.Log(message);
    }

    private static CanvasHandler canvasHandler => GameObject.FindWithTag("CanvasHandler")?.GetComponent<CanvasHandler>();
    private static GameManager gameManager => GameObject.FindWithTag("GameController")?.GetComponent<GameManager>();
    private static AudioSourceController audioSource => GameObject.FindWithTag("AudioSource")?.GetComponent<AudioSourceController>();

    public static void PrintSubtitles(string key)
    {
        canvasHandler.SetSubtitles(key);
    }
    public static void RemoveSubtitles()
    {
        canvasHandler.RemoveSubtitles();
    }
    public static void PrintTitle(string key, string subkey)
    {
        canvasHandler.SetTitle(key, subkey);
    }
    public static void RemoveTitle()
    {
        canvasHandler.RemoveTitle();
    }

    public static void PlayAudio(string key)
    {
        AudioClip clip = Resources.Load<AudioClip>("Audio/" + gameManager.getLocale() + "/" + key);
        Debug.Log(clip);
        audioSource.Play(clip);
    }
}

public class GameEventHandler : MonoBehaviour
{
    [System.Serializable]
    public class Event
    {
        public string function;
        public string wait;
    }

    [System.Serializable]
    public class Events
    {
        public Event[] events;
    }

    [SerializeField]
    private TextAsset jsonFile;
    private Events eventsInJson;

    void Start()
    {
        eventsInJson = JsonUtility.FromJson<Events>(jsonFile.text);

        // No need to create an instance, use the static class directly
        // EventMethods eventMethodsInstance = new EventMethods();

        // Start the coroutine with the static EventMethods class
        StartCoroutine(PlayEvent(0));
    }

    IEnumerator PlayEvent(int index)
    {
        Event e = eventsInJson.events[index];

        // Parse the function string and parameters
        ParseFunctionString(e.function);

        if (index >= eventsInJson.events.Length - 1)
        {
            yield return null;
        }
        else
        {
            yield return new WaitForSeconds(float.Parse(e.wait) / 1000f);
            StartCoroutine(PlayEvent(index + 1));
        }
    }

    void ParseFunctionString(string functionString)
    {
        try
        {
            // Parse the function string into function name and arguments
            int openParenIndex = functionString.IndexOf('(');
            int closeParenIndex = functionString.LastIndexOf(')');

            string functionName;
            string argumentsString;

            if (openParenIndex != -1 && closeParenIndex != -1 && closeParenIndex > openParenIndex)
            {
                functionName = functionString.Substring(0, openParenIndex).Trim();
                argumentsString = functionString.Substring(openParenIndex + 1, closeParenIndex - openParenIndex - 1).Trim();
            }
            else
            {
                // No parentheses found, assume the entire string is the function name
                functionName = functionString.Trim();
                argumentsString = string.Empty;
            }

            // Find the method with the specified name using reflection
            MethodInfo methodInfo = typeof(EventMethods).GetMethod(functionName);

            if (methodInfo != null)
            {
                // Validate the number of parameters
                ParameterInfo[] parameters = methodInfo.GetParameters();

                if (parameters.Length == 0 && argumentsString == "")
                {
                    // If the method expects 0 parameters and none are provided, invoke it without arguments
                    methodInfo.Invoke(null, null);
                }
                else if (parameters.Length == argumentsString.Split(',').Length)
                {
                    // Create an array of objects to pass as arguments
                    string[] arguments = argumentsString.Split(',');
                    object[] argumentsAsObjects = new object[arguments.Length];

                    for (int i = 0; i < arguments.Length; i++)
                    {
                        argumentsAsObjects[i] = Convert.ChangeType(arguments[i].Trim(), parameters[i].ParameterType);
                    }

                    // Invoke the method on the static EventMethods class
                    methodInfo.Invoke(null, argumentsAsObjects);
                }
                else
                {
                    Debug.LogWarning($"Method {functionName} expects {parameters.Length} parameters, but {argumentsString.Split(',').Length} provided.");
                }
            }
            else
            {
                Debug.LogWarning("Method not found: " + functionName);
            }
        }
        catch (Exception e)
        {
            // Log the inner exception for more details
            Debug.LogError("Error parsing or executing function: " + e.InnerException.Message);
        }
    }
}