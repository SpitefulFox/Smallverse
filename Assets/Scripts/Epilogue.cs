using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class Epilogue : MonoBehaviour {

	public static List<World> saved;
	
	public GameObject worldPrefab;
	public GameObject[] stars;
	public Camera cam;
	
	public Text desc;
	public Text stats;
	
	public int military;
	public int art;
	public int life;
	public int population;
	public string theme = "";
	public List<string> races = new List<string>();
	public Dictionary<string, int> themes = new Dictionary<string, int>();
	
	public int punk = 0;
	public int evil = 0;
	public int magic = 0;
	public int scifi = 0;
	public int anime = 0;
	public int neutron = 0;
	public int nosex = 0;
	public int racist = 0;
	
	// Use this for initialization
	void Start () {
		MakePlanets();
		CheckVerse();
		JudgeVerse();
	}
	
	public void MakePlanets(){
		int x = 0;
		int reach = Mathf.Max(60, (15 * saved.Count) / 2);
		foreach(World world in saved){
			GameObject current = (Instantiate(worldPrefab, worldPrefab.transform.position - new Vector3(x * 15, 0, 0), worldPrefab.transform.rotation) as GameObject);
			SpriteRenderer render = current.GetComponent<SpriteRenderer>();
			render.sprite = world.sprite;
			render.color = world.color;
			//print("Color:" + world.color);
			//print("Seen:" + render.color);
			Starfield drift = current.GetComponent<Starfield>();
			drift.beginning = new Vector3(-reach, drift.beginning.y, drift.beginning.z);
			drift.end = new Vector3(reach, drift.end.y, drift.end.z);
			x++;
		}
	}
	
	public void CheckVerse(){
		int mil = 0;
		int ar = 0;
		int lif = 0;
		foreach(World world in saved){
			mil += world.military * world.population;
			ar += world.art * world.population;
			lif += world.life * world.population;
			population += world.population;
			foreach(string race in world.races){
				if(!races.Contains(race))
					races.Add(race);
			}
			foreach(string trait in world.traits){
				if(trait == "Anime is real")
					anime++;
				if(trait == "Every person owns a neutron bomb")
					neutron++;
				if(trait == "Sex is illegal")
					nosex++;
				if(trait == "Everyone is absurdly racist")
					racist++;
			}
			Tally(world.theme);
			if(Punk(world.theme))
				punk++;
			if(Evil(world.theme))
				evil++;
			if(Magical(world.theme))
				magic++;
			if(SciFi(world.theme))
				scifi++;
		}
		if(population > 0){
			military = mil / population;
			art = ar / population;
			life = lif / population;
			int max = 0;
			foreach(KeyValuePair<string, int> entry in themes){
				if(entry.Value > max){
					theme = entry.Key;
					max = entry.Value;
				}
			}
			
			if(((float)punk / (float)saved.Count) >= 0.75f){
				military += 1;
				art += 1;
			}
			else if(((float)scifi / (float)saved.Count) > 0.5f){
				military += 1;
				life += 1;
			}
			
			if(races.Count == 1 && races[0] == "Lloigor")
				evil += 1;
			if(((float)evil / (float)saved.Count) >= 0.75f){
				military += 3;
				life -= 3;
			}
			else if(((float)magic / (float)saved.Count) > 0.5f && military > 0){
				military += 2;
			}
			
			if(((float)neutron / (float)saved.Count) >= 0.75f){
				military += 3;
			}
			if(((float)racist / (float)saved.Count) >= 0.75f){
				military += 1;
				life -= 3;
			}
		}
		else
			theme = "Genocide";
		
		stats.text = "Military: " + MathHelper.HiLo(military) +
			"\nArt: " + MathHelper.HiLo(art) +
			"\nQuality of Life: " + MathHelper.HiLo(life) +
			"\n\nPopulation: " + population + "k" +
			"\nOverall Theme: " + theme;
	}
	
	public void JudgeVerse(){
		string end = "You did okay, kinda?";
		if(saved.Count <= 0){
			end = "You failed to save a single world. Was no world good enough for you, or did you just want to watch the multiverse burn?";
			for(int x = 0;x < stars.Length;x++){
				Destroy(stars[x]);
			}
			cam.backgroundColor = Color.black;
		}
		else if(saved.Count == 1 && saved[0].name == "Earth"){
			end = "Earth is the only inhabited world. There is no magic. There is no fantastic technology. There is no Destroyer. ";
		}
		else {
			if(art <= 0){
				end = "In your haste to save as many worlds as possible, you somehow didn't save ANY worlds with any significant art or culture. The new universe is a soulless husk, unfit for even The Destroyer.";
			}
			else if(saved.Count < 4){
				end = "The Destroyer emerged again to find an absurdly unpopulated universe. It never stood a chance.";
			}
			else if(military <= 0 && life > 8){
				end = "The new universe is so utopian and devoted to peace that they convince The Destroyer to spare them and change its ways. ";
				if(art > 5){
					end = end + "The feat will be celebrated in song and art for aeons to come. ";
				}
			}
			else if(military <= 6){
				if(life >= 7){
					end = "The new universe is home to many advanced and happy utopian societies. ";
					if(art > 6)
						end = end + "It becomes a beacon of art and culture. ";
					end = end + "But all good things must come to an end eventually. The Destroyer emerges again and wipes this universe as easily as it did the last one. This too has come to pass.";
				}
				else if(life <= 3){
					end = "The new universe is a miserable hellhole. ";
					if(art > 5)
						end = end + "The ambient unhappiness provides the angst to fuel a plethora of depressing writing and artwork. ";
					else
						end = end + "With not even art to relieve life's futile existence. ";
					end = end + "Mercifully, The Destroyer emerges again and puts the new universe out of its misery. ";
				}
				else {
					end = "The new universe is a mixed bag, just as the last one. Sometimes life is okay, sometimes it isn't. The circle continues to turn. ";
					end = end + "The Destroyer emerges again and wipes this universe clean just like the last. ";
				}
			}
			else {
				if(((float)evil / (float)saved.Count) >= 0.75f){
					end = "The Destroyer emerges to find a universe more twisted, violent, and nightmarish than itself. It never stood a chance. ";
					if(art > 5)
						end = end + "The slaughter is depicted in bloody paintings for generations to come. ";
					else
						end = end + "The slaughter is memorialized in the silent screaming of the bitter stars and unseen moons. ";
				}
				else {
					end = "The Destroyer emerges again only to find a universe armed to the teeth and ready for it. After a brutal war across many worlds, The Destroyer is put down, ending the cycle for good. ";
					if(life <=3){
						end = end + "But at what cost? The universe is a bleak place where daily life is suffering. ";
						if(art > 5)
							end = end + "At least the artwork is pretty metal. ";
					}
					else if(life >= 7){
						end = end + "A shining new age begins as life flourishes in this brave new universe. ";
						if(art > 5)
							end = end + "In this idyllic utopia, the arts reach new heights never imagined. ";
					}
				}
			}
			
			end = end + "\n";
			
			if(races.Count == 1 && races[0] == "Humans")
				end = end + "\nYou helped wipe the multiverse of everything but humans. Are you proud of yourself? ";
			else if(Bro()){
				end = end + "\nYou created a universe of ponies, but real friends don't let friends die. ";
			}
			else if(Furry()){
				end = end + "\nYou did knot save any races except fur your furry furiends. Howl could you?! ";
			}
			else if(races.Count == 1)
				end = end + "\n" + races[0] + " were the only race you saved. ";
			else {
				end = end + "\nYou rescued " + races.Count + " different races from destruction. ";
			}
			
			if(saved.Count >=4){
				end = end + "\nYou rescued " + saved.Count + " worlds from destruction. ";
				if(saved.Count >=20)
					end = end + "You indiscriminately saved every world you saw. ";
			}
			
			if(((float)punk / (float)saved.Count) >= 0.75f){
				end = end + "\n\nThe new universe is proof that punk isn't dead. ";
			}
			if(((float)scifi / (float)saved.Count) > 0.5f){
				end = end + "\n\nThe new universe is full of advanced science and technology. ";
			}
			if(((float)magic / (float)saved.Count) > 0.5f){
				end = end + "\n\nThe new universe is a very magical place. ";
			}
			if(((float)anime / (float)saved.Count) >= 0.75f){
				end = end + "\n\nEveryone in the new universe has a waifu. ";
			}
			
			if(((float)racist / (float)saved.Count) >= 0.75f && military > 6){
				end = end + "\n\nThe universe later destroyed itself anyway in a massive racial cleansing due to the high level of xenophobia. ";
			}
			else if(((float)neutron / (float)saved.Count) >= 0.75f && military > 6){
				end = end + "\n\nThe universe later destroyed itself anyway due to everyone owning their own neutron bombs. ";
			}
			else if(((float)nosex / (float)saved.Count) >= 0.75f && military > 6){
				end = end + "\n\nThe universe later died out anyway due to nobody reproducing. ";
			}
		}
		
		desc.text = end;
	}
	
	public void NewGame(){
		SceneManager.LoadScene("Multiverse");
	}
	
	private bool Furry(){
		foreach(string race in races){
			if(race != "Furries" && race != "Ponies" && race != "Foxes" && race != "Talking Dogs" && race != "Unicorns" && race != "Dragons" && race != "Lizard People")
				return false;
		}
		return true;
	}
	
	private bool Bro(){
		bool pony = false;
		foreach(string race in races){
			if(race != "Ponies" && race != "Dragons" && race != "Unicorns")
				return false;
			if(race == "Ponies")
				pony = true;
		}
		return pony;
	}
	
	private void Tally(string theme){
		int tally = 0;
		themes.TryGetValue(theme, out tally);
		themes[theme] = tally + 1;
	}
	
	private bool Punk(string theme){
		return theme.Contains("punk") || theme == "Punk";
	}
	
	private bool Evil(string theme){
		return theme == "Netherworld" || theme == "Eldritch" || theme == "Grimdark" || theme == "Occult" || theme == "Capitalism" || theme == "Dark Fantasy";
	}
	
	private bool Magical(string theme){
		return theme.Contains("Fantasy") || theme == "Elemental Plane" || theme == "Occult" || theme == "Eldritch" || theme == "Netherworld" || theme == "Weird West";
	}
	
	private bool SciFi(string theme){
		return theme.Contains("punk") || theme == "Raygun Gothic" || theme == "Space Opera" || theme == "Simulation";
	}
}
