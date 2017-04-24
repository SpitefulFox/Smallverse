using UnityEngine;
using System.Collections;

public class MathHelper {

	public static int WeightLow(int min, int max){
		int first = Random.Range(min, max);
		int second = Random.Range(min, max);
		if(second < first)
			return WeightLow(min, max);
		else
			return first;
	}
	
	public static int WeighAgainst(int min, int max, int against){
		int first = Random.Range(min, max);
		int second = Random.Range(min, max);
		if(second < first && second < against)
			return WeighAgainst(min, max, against);
		else
			return first;
	}
	
	public static string Unique(string[] current, string[] all){
		string token = all[Random.Range(0, all.Length)];
		foreach(string soFar in current){
			if(token == soFar)
				return Unique(current, all);
		}
		return token;
	}
	
	public static bool YesNo(){
		if(Random.Range(1, 101) % 2 == 0)
			return true;
		else
			return false;
	}
	
	public static string HiLo(int value){
		if(value <= 0)
			return "None";
		else if(value >= 10)
			return "Very High";
		else if(value <= 3)
			return "Low";
		else if(value >= 7)
			return "High";
		else
			return "Okay";
	}
}
