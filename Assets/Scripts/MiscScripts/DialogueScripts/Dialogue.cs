using System;
using UnityEngine;

[Serializable]
public struct Dialogue
{
	public string name;

	[TextArea(3, 10)]
	public string[] sentences;
}
