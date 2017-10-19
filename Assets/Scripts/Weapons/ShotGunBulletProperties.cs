using UnityEngine;

namespace Main.Scripts.BulletProperties
{
	public class ShotGunBulletProperties : MonoBehaviour {

		public float Lifetime = 2.0f;           //Time untill bullets clear
		public int Damage = 1;
		public int Penetration = 5;
		int _health;
	
		void Start () {
			Destroy(gameObject, Lifetime);
		}
	
		void OnTriggerEnter(Collider other)
		{
			if (other.gameObject.CompareTag("Hitbox"))
			{
				other.gameObject.GetComponent<EntityHealth>().Damage(Damage);
				if (other.gameObject.GetComponent<EntityHealth>().Health >= 1)
					Penetration = 0;
				//Select Sounds
				if (other.gameObject.CompareTag("Hitbox") && other.gameObject.layer != 13)
					FindObjectOfType<AudioManager>().Play("hitTarget");
				if (other.gameObject.layer == 13)
					FindObjectOfType<AudioManager>().Play("codHit");
				if (Penetration <= 0) {
					Destroy(gameObject);
				}
			}
			if (other.gameObject.CompareTag("Matter") || other.gameObject.CompareTag("Hazard"))
			{
                FindObjectOfType<AudioManager>().Play("hitMatter");
                Destroy(gameObject);
			}
		}
	}
}