using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class WorldBrowser : MonoBehaviour {

	public Sprite[] worldSprites;
	public GameObject worldPrefab;
	
	public World current;
	
	public Image currentSprite;
	public Animator currentAnimator;
	
	public Text stats;
	public Text census;
	public Text desc;
	
	public Text energyBar;
	public int energy = 20;
	private int skipCount = 0;
	public Animator uiAnimator;
	public Animator cameraAnimator;
	
	public List<World> saved;
	public World[] specialChoices;
	private List<World> specials;
	
	private string[] races = {"Humans", "Foxes", "Octopi", "Talking Dogs",
		"Demons", "Svartalfar", "Kobolds", "Lizard People", "Lightning Elementals",
		"Androids", "Greys", "Angel", "Dragons", "Unicorns", "Ponies", "Trolls",
		"Merpeople", "Furries", "Clay Golems", "Lloigor", "Redcaps", "Pixies"};
		
	private string[] themes = {
		"High Fantasy", "Low Fantasy", "Steampunk", "Cyberpunk", "Dieselpunk", "Punk",
		"Yarn", "Eldritch", "Elemental Plane", "Capitalism", "Saturday Morning Cartoon",
		"Grimdark", "Biopunk", "Middle Fantasy", "Kitchen Sink", "Vanilla", "Metal",
		"Netherworld", "Raygun Gothic", "Space Opera", "Dark Fantasy", "Weird West",
		"Solarpunk", "Spooky", "Romantic Comedy", "Noir", "Abstract", "8-Bit"
	};
		
	private string[] good_traits = {"The sun is made of crystal", "All cops have wooden legs",
		"Nobody can see the color purple", "Mayonnaise on french fries is illegal", "Zombies exist, but are friendly",
		"Society has settled on four and a half genders", "The moon changes phases when nobody's looking at it",
		"Is full of kittens", "Same sex marriage is mandatory", "All transactions are done with plastic coins",
		"Everyone has a birthmark of the first line their soul mate will say",
		"Cybernetics is considered a dorky dad science", "Anime is real",
		"Mountains are made of rock candy", "Gumdrops rain from the sky on occasion",
		"People are allergic to right angles", "Alternate history has led to Steampunk never being invented",
		"Minecraft has a modding API", "Differences are settled by guitar duels", "The internet was never invented",
		"Psychopomps are visible, but scared of the color pink", "Sugar is essential to a balanced diet",
		"On-disc DLC is illegal", "There are 25 hours in a day as long as the 25th is used for sleeping",
		"Napping does not progress time", "The concept of Up is 45 degrees to the left"
	};
		
	private string[] bad_traits = {
		"Is on fire", "Everyone is absurdly racist", "Military service is required at the age of four.",
		"Kaijus emerge from the ocean every Thursday.", "Every day of the week is just Monday",
		"Socks go missing at alarmingly high rates", "Everyone is religious and hates every other religion",
		"All physical actions are determined by text prompt", "Soul mates exist, but nobody can meet theirs",
		"Healthcare costs blood sacrifices", "Sex is illegal", "The economy's main commodity is babies",
		"Children screams can reach 130 decibels", "Police brutality is legally required",
		"The spiciest condiment is mayonnaise", "Video games aren't tested for bugs",
		"The afterlife is real and commercialized", "Porn doesn't exist", "Pigs contain tetrodotoxin",
		"Corporations own the rights to everyone's left eyes", "Has way too many eyes", "Is made of voxels",
		"Every person owns a neutron bomb", "Spiders are real", "Dolls talk", "The darkness hungers"
	};
	
	public void Start(){
		specials = new List<World>(specialChoices);
		energy = 20;
		GenerateWorld();
	}
	
	public void Update(){
		energyBar.text = "Energy: " + energy + "/20";
	}
	
	public void DontSave(){
		if(currentAnimator == null)
			return;
		currentAnimator.SetTrigger("Destroy");
		currentAnimator.gameObject.AddComponent<SelfDestruct>();
		if(++skipCount >= 5){
			skipCount = 0;
			energy--;
		}
		if(energy > 0)
			GenerateWorld();
		else
			GetOut();
	}
	
	public void Save(){
		if(currentAnimator == null)
			return;
		currentAnimator.SetTrigger("Save");
		currentAnimator.gameObject.AddComponent<SelfDestruct>();
		saved.Add(current);
		energy--;
		if(energy > 0)
			GenerateWorld();
		else
			GetOut();
	}
	
	public void GetOut(){
		uiAnimator.SetTrigger("Done");
		cameraAnimator.SetTrigger("Done");
		Epilogue.saved = saved;
		Invoke("ReallyGetOut", 1f);
	}
	
	public void ReallyGetOut(){
		SceneManager.LoadScene("Epilogue");
	}
	
	public void GenerateWorld(){
		currentSprite = (Instantiate(worldPrefab) as GameObject).GetComponent<Image>();
		currentAnimator = currentSprite.gameObject.GetComponent<Animator>();
		currentSprite.transform.SetParent(transform, true);
		
		if(specials.Count > 0 && Random.Range(0, 51) < 5 && Random.Range(0, specialChoices.Length * 2) < specials.Count){
			current = specials[Random.Range(0, specials.Count)];
			specials.Remove(current);
			currentSprite.sprite = current.sprite;
		}
		else {
			currentSprite.sprite = worldSprites[Random.Range(0,worldSprites.Length)];
			current = new World();
			if(MathHelper.YesNo()){
				currentSprite.color = Random.ColorHSV(0f, 1f, 0.8f, 1f, 0.8f, 1f, 1f, 1f);
				current.color = currentSprite.color;
			}
			current.sprite = currentSprite.sprite;
			current.theme = themes[Random.Range(0,themes.Length)];
			current.military = MathHelper.WeightLow(0, 11);
			current.art = MathHelper.WeighAgainst(0, 11, current.military);
			current.life = MathHelper.WeighAgainst(1, 11, current.military);
			current.population = Random.Range(1, 26);
			
			current.races = new string[MathHelper.WeightLow(1, 5)];
			for(int x = 0;x < current.races.Length;x++){
				if(x == 0 && Random.Range(0, 11) > 4)
					current.races[x] = "Humans";
				else
					current.races[x] = MathHelper.Unique(current.races, races);
			}
			
			current.traits = new string[MathHelper.WeightLow(1, 5)];
			for(int x = 0;x < current.traits.Length;x++){
				string[] traits = LifeTraits(current.life);
				current.traits[x] = MathHelper.Unique(current.traits, traits);
			}
		}
		DisplayStats();
	}
	
	public void DisplayStats(){
		stats.text = "Theme: " + current.theme +
			"\n\nMilitary: " + MathHelper.HiLo(current.military) +
			"\nArts: " + MathHelper.HiLo(current.art) +
			"\nQuality of Life: " + MathHelper.HiLo(current.life);
			
		census.text = "Population: " + current.population + "k" +
			"\n\nRaces:";
		foreach(string race in current.races){
			census.text = census.text + "\n-" + race;
		}
		
		desc.text = "Other Info:";
		foreach(string trait in current.traits){
			desc.text = desc.text + "\n-" + trait;
		}
	}
	
	private string[] LifeTraits(int life){
		if(life >= 7)
			return good_traits;
		else if(life <= 3)
			return bad_traits;
		else if(MathHelper.YesNo())
			return good_traits;
		else
			return bad_traits;
	}
}
