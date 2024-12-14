using System;
using UnityEngine.Events;

public class Option
{
    public string Value { get; set; }

    public Action OnOptionSelected;

    public Option(string value, Action onOptionSelected)
    {
        Value = value;
        OnOptionSelected = onOptionSelected;
    }
}