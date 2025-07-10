using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// Displays rich-text (e.g., <color>, <i>) one character at a time,
/// with adjustable speed via characters-per-second.
/// </summary>
[RequireComponent(typeof(Text))]
public class TextTyper : MonoBehaviour
{
    public bool isTyping = false;

    [SerializeField] private Text uiText;
    [SerializeField] private float charInterval = 0.03f;

    private struct Token { public bool isTag; public string content; public string tagName; public bool isOpening; }
    private List<Token> tokens;
    string text;

    private void Awake()
    {
        if (uiText == null)
            uiText = GetComponent<Text>();
    }

    public void Play(string text)
    {
        this.text = text;
        tokens = ParseTokens(text);
        StartCoroutine(TypeText());
    }

    public void OnQuickTyping()
    {
        StopAllCoroutines();
        uiText.text = text;
        isTyping = false;
    }

    private IEnumerator TypeText()
    {
        isTyping = true;
        int revealedCount = 0;
        int totalChars = 0;
        foreach (var t in tokens) if (!t.isTag) totalChars++;

        while (revealedCount < totalChars)
        {
            revealedCount++;
            uiText.text = BuildRichString(revealedCount);
            yield return new WaitForSeconds(charInterval);
        }

        isTyping = false;
    }

    private string BuildRichString(int revealChars)
    {
        var sb = new StringBuilder();
        var openStack = new Stack<Token>();
        int count = 0;

        foreach (var token in tokens)
        {
            if (token.isTag)
            {
                sb.Append(token.content);
                if (token.isOpening)
                    openStack.Push(token);
                else
                {
                    var name = token.tagName;
                    var temp = new Stack<Token>();
                    while (openStack.Count > 0 && openStack.Peek().tagName != name)
                        temp.Push(openStack.Pop());
                    if (openStack.Count > 0) openStack.Pop();
                    while (temp.Count > 0) openStack.Push(temp.Pop());
                }
            }
            else
            {
                count++;
                if (count <= revealChars)
                    sb.Append(token.content);
                else
                    break;
            }
        }

        foreach (var open in openStack)
            sb.Append($"</{open.tagName}>");

        return sb.ToString();
    }

    private List<Token> ParseTokens(string input)
    {
        var list = new List<Token>();
        for (int i = 0; i < input.Length;)
        {
            if (input[i] == '<')
            {
                int end = input.IndexOf('>', i);
                if (end > i)
                {
                    string tag = input.Substring(i, end - i + 1);
                    bool opening = !(tag.Length > 2 && tag[1] == '/');
                    string name;
                    if (opening)
                    {
                        int start = 1;
                        int idx = tag.IndexOfAny(new char[] { ' ', '>', '=' }, start);
                        name = idx > 0 ? tag.Substring(start, idx - start) : tag.Substring(start, tag.Length - start - 1);
                    }
                    else
                        name = tag.Substring(2, tag.Length - 3);

                    list.Add(new Token { isTag = true, content = tag, tagName = name, isOpening = opening });
                    i = end + 1;
                    continue;
                }
            }
            list.Add(new Token { isTag = false, content = input[i].ToString() });
            i++;
        }
        return list;
    }
}
