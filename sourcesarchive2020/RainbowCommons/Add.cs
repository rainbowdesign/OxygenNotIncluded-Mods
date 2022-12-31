using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Co
{
    public class Add
    {
        static public void Recipe(string ID, float recipetime, ComplexRecipe.RecipeElement[] ingredients, ComplexRecipe.RecipeElement[] results, string description = null, string ingredientsstr = null, string resultsstr = null){
        Recipe(ID,  ingredients,results , recipetime);
    }
            static public void Recipe(string ID, ComplexRecipe.RecipeElement[] ingredients, ComplexRecipe.RecipeElement[] results, float recipetime=40, string description = null, string ingredientsstr = null, string resultsstr = null)
        {
        
            /*Tag ingredientsstri; Tag resultsstri;
			try {
				ingredientsstri = ingredients[0].material;
				resultsstri = results[0].material;
			}
			catch {
				ingredientsstri = ingredientsstr.ToTag();
				resultsstri = resultsstr.ToTag();
			}

			string obsolete_id = ComplexRecipeManager.MakeObsoleteRecipeID(ID, ingredientsstri);*/
            string str = ComplexRecipeManager.MakeRecipeID(ID, (IList<ComplexRecipe.RecipeElement>)ingredients, (IList<ComplexRecipe.RecipeElement>)results);

            string recipename = "";
            foreach (var i in ingredients)
            {
                recipename += i.material.Name + " ";
                break;
            }
            recipename += " to ";
            foreach (var i in results)
            {
                recipename += i.material.Name + " ";
                break;
            }


            ComplexRecipeManager.Get().AddObsoleteIDMapping(recipename, str);

            ComplexRecipe complexRecipe1 = new ComplexRecipe(str, ingredients, results);
            complexRecipe1.time = recipetime;
            //complexRecipe1.useResultAsDescription = true;
            if (description == null)
            {
                complexRecipe1.description = "Combine ";
                int counter = 1;
                foreach (var i in ingredients)
                {
                    if (ingredients.Length > counter++) complexRecipe1.description += i.material.Name + " and ";
                    else complexRecipe1.description += i.material.Name;
                }
                complexRecipe1.description += " to get ";
                counter = 1;
                foreach (var i in results)
                {
                    if (results.Length > counter++) complexRecipe1.description += i.material.Name + " and ";
                    else complexRecipe1.description += i.material.Name;
                };
            }
            else complexRecipe1.description = description;

            complexRecipe1.nameDisplay = ComplexRecipe.RecipeNameDisplay.ResultWithIngredient;
            complexRecipe1.fabricators = new List<Tag>() { TagManager.Create(ID) };
        }
        static public void BuildingTech(string ID, string TECH)
        {

            List<string> ls = new List<string>((string[])Database.Techs.TECH_GROUPING[TECH]);
            ls.Add(ID);
            Database.Techs.TECH_GROUPING[TECH] = (string[])ls.ToArray();
        }
        static public void BuildingPlan(string ID, string NAME,string DESCRIPTION,string EFFECT,string PLANCATEGORY)
        {
            Strings.Add("STRINGS.BUILDINGS.PREFABS." + ID.ToUpper() + ".NAME", NAME);
            Strings.Add("STRINGS.BUILDINGS.PREFABS." + ID.ToUpper() + ".DESC", DESCRIPTION);
            Strings.Add("STRINGS.BUILDINGS.PREFABS." + ID.ToUpper() + ".EFFECT", EFFECT);

            List<string> category = (List<string>)TUNING.BUILDINGS.PLANORDER.First(po => ((HashedString)PLANCATEGORY).Equals(po.category)).data;
            category.Add(ID);

            //TUNING.BUILDINGS.COMPONENT_DESCRIPTION_ORDER.Add(typeof(PressureLiquidReservoirConfig)); }
        }
    }
}
