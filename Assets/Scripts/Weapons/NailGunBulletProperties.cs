using UnityEngine;

namespace Main.Scripts.BulletProperties
{
	public class NailGunBulletProperties : MonoBehaviour {

		public float Lifetime = 2.0f;           //Time untill bullets clear
		public int Damage = 1;
        public int Penetration = 1;
        int _health;

		void Start () {
			Destroy(gameObject, Lifetime);
		}
	
		void OnTriggerEnter(Collider other)
		{
			if (other.gameObject.CompareTag("Hitbox"))
			{
                Debug.Log(Penetration);
                other.gameObject.GetComponent<EntityHealth>().Damage(Damage);
                if (other.gameObject.GetComponent<EntityHealth>().Health >= 1)
                    Penetration = 0;
                if (other.gameObject.GetComponent<EntityHealth>().Health <= 1)
                    Penetration -= 1;
                //Select Sounds
                if (other.gameObject.CompareTag("Hitbox") && other.gameObject.layer != 13)
                    FindObjectOfType<AudioManager>().Play("hitTarget");
                if (other.gameObject.layer == 13 && Penetration > 0) {
                    FindObjectOfType<AudioManager>().Play("swarmerPuncture");
                    FindObjectOfType<AudioManager>().Play("swarmerKill");
                }
                if (Penetration <= 0 && other.gameObject.layer == 13)
                    FindObjectOfType<AudioManager>().Play("swarmerKill");
                if (Penetration <= 0) { 
                    Destroy(gameObject);
                }
            }
			if (other.gameObject.CompareTag("Matter"))
			{
           //     FindObjectOfType<AudioManager>().Play("hitMatter");
                Destroy(gameObject);
			}
		}
	}
}