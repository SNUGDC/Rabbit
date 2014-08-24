using UnityEngine;
using System.Collections;

public class RabbitSprite : MonoBehaviour {
	[System.Serializable]
	public class Spritelist{
		public Sprite[] sprites = new Sprite[5];
		public Sprite this[int i]
		{
			get
			{
				return sprites[i];
			}
			set
			{
				sprites[i] = value;
			}
		}
	}

	public Spritelist[] headList = new Spritelist[3];
	public Spritelist[] bodyList = new Spritelist[4];
	public Sprite[] tailList = new Sprite[5];
	public Sprite[] teethList = new Sprite[2];

	public static RabbitSprite manager;
	// Use this for initialization
	void Awake () {
		manager = this;
	}
}
