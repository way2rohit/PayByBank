namespace Pokedex.Tests
{
	public static class MockData
	{

		public static string GetErrorResponse()
		{
			return @"{ ""Error"":{ ""Code"":404,""Message"":""Not Found""} }";
		}
		public static string GetPokemonSpeices()
		{
			return @"{
                   ""base_happiness"":0,
                   ""capture_rate"":0,
                   ""color"":null,
                   ""egg_groups"":null,
                   ""evolution_chain"":null,
                   ""evolves_from_species"":null,
                   ""flavor_text_entries"":[
                      {
                         ""flavor_text"":""This is a test flavor_text Value"",
                         ""language"":{
                            ""name"":null,
                            ""url"":null
                         },
                         ""version"":{
                            ""name"":null,
                            ""url"":null
                         }
                      }
                   ],
                   ""form_descriptions"":null,
                   ""forms_switchable"":false,
                   ""gender_rate"":0,
                   ""genera"":null,
                   ""generation"":null,
                   ""growth_rate"":null,
                   ""habitat"":{
                      ""name"":""cave"",
                      ""url"":null
                   },
                   ""has_gender_differences"":false,
                   ""hatch_counter"":0,
                   ""id"":0,
                   ""is_baby"":false,
                   ""is_legendary"":true,
                   ""is_mythical"":false,
                   ""name"":""steelix"",
                   ""names"":null,
                   ""order"":0,
                   ""pal_park_encounters"":null,
                   ""pokedex_numbers"":null,
                   ""shape"":null,
                   ""varieties"":null
                }";
		}
		public static string GetTranslatedDesc()
		{
			return @"{""success"":{""total"":1},""contents"":{""translated"":""A test description,  'this is.'"",""text"":""This is a test description."",""translation"":""yoda""}}";
		}

	}
}
