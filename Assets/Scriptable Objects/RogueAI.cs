using UnityEngine;

[CreateAssetMenu(fileName = "New AI", menuName = "Artificial Intelligence")]
public class RogueAI : ScriptableObject
{
	public void Act(Entity entity, RogueMap roguemap)
	{
		Debug.Log("The " + entity.data.name + " is itching for a fight.");
	}
}