using ADProject.JsonObjects;
using ADProject.Models;
using ADProject.Service;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ADProject.GenerateTagsClass
{
    public class GenerateTag
    {
        private readonly IRecipeService _recipesService;
        private readonly ADProjContext _context;

        public GenerateTag(IRecipeService recipeService, ADProjContext context)
        {
            _recipesService = recipeService;
            _context = context;
        }

        public string GetAllergenTag(List<RecipeIngredient> recipeIngredients)
        {
            string uirWebAPI = "http://recipe4menlp-env.eba-zsgvuj5a.ap-southeast-1.elasticbeanstalk.com/api/allergentags";


            /*            string uirWebAPI = "http://127.0.0.1:5000/api/allergentags";
            */
            string webResponse = string.Empty;


            Debug.WriteLine(recipeIngredients[0]);

            List<string> ingredients = new List<string>();
            foreach (RecipeIngredient ri in recipeIngredients)
            {
                ingredients.Add(ri.Ingredient);
            }

            recipeIngredientsJson toSend = new recipeIngredientsJson();
            toSend.ingredients = ingredients;

            List<Tag> allTags = _context.Tags.ToList();
            List<string> corpus_allergen = new List<string>();
            List<string> corpus_ingredient = new List<string>();
            foreach(Tag tag in allTags)
            {
                corpus_allergen.Add(tag.Warning);
                corpus_ingredient.Add(tag.TagName);
            }
            toSend.corpus_allergen = corpus_allergen;
            toSend.corpus_ingredient = corpus_ingredient;

            try
            {
                Uri uri = new Uri(uirWebAPI);
                WebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(uri);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";
                using (StreamWriter streamWriter =
                    new StreamWriter(httpWebRequest.GetRequestStream()))
                {

                    streamWriter.Write(JsonConvert.SerializeObject(toSend));
                }
                HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream()))
                {
                    webResponse = streamReader.ReadToEnd();

                }

            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
            Debug.WriteLine(webResponse);

            tempAllergenTags tempAlTags = JsonConvert.DeserializeObject<tempAllergenTags>(webResponse);
/*            if (tempAlTags.allergens != null)
            {
                Debug.WriteLine(tempAlTags.allergens[0]);
            }*/

            return webResponse;

        }
    }
}
