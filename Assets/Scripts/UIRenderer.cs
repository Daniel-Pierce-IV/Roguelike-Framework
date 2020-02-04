using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRenderer : MonoBehaviour
{
	[SerializeField] Text curHPText;
	[SerializeField] Text maxHPText;

    void Start()
    {
		UpdateUI();
		EventSystem.Instance.Combat += PlayerStateChange;
    }

	void UpdateUI()
	{
		curHPText.text = RogueGameManager.player.stats.curHp.ToString();
		maxHPText.text = RogueGameManager.player.stats.maxHp.ToString();
	}

	void PlayerStateChange(CombatEventArgs args)
	{
		if (args.Defender == RogueGameManager.player)
		{
			UpdateUI();
		}
	}
}
