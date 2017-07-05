﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using SemanticWebNPLSearchEngine.Models;
using System;
using System.IO;

namespace SemanticWebNPLSearchEngine.Classes.Tests
{
    [TestClass()]
    public class utilitiesTests
    {
        [TestMethod()]
        public void RemoveLast3Cahracters()
        {
            string test = "Hello World@en";
            string expected = "Hello World";

            string output = utilities.RemoveLast3Cahracters(test);

            Assert.AreEqual(expected, output);
        }

        [TestMethod()]
        public void DateCreatorTest()
        {
            string test = "08/06/1996^^Date";
            string expected = "08/06/1996";

            string output = utilities.DateCreator(test);

            Assert.AreEqual(expected, output);
        }

        [TestMethod()]
        public void ExtractLuisDataTest()
        {
            string limit = $"LIMIT({10})";
            string genreMatch = $"FILTER ( regex (str(?genre), '{"crime"}', 'i'))";
            string dateMatch = $"FILTER ((?releaseDate >= '{2012}-01-01'^^xsd:date) && (?releaseDate < '{2012}-12-31'^^xsd:date))";

            string queryPattern =
                "PREFIX xsd: <http://www.w3.org/2001/XMLSchema#> " +
                "PREFIX db: <http://dbpedia.org/ontology/> " +
                "PREFIX prop: <http://dbpedia.org/property/> " +
                "SELECT ?movieLink ?title ?genreLink ?genre ?releaseDate " +
                "WHERE {{ " +
                    "?movieLink rdf:type db:Film; " +
                               "foaf:name ?title. " +
                    "OPTIONAL {{ ?movieLink prop:genre ?genreLink. " +
                               "?genreLink rdfs:label ?genre. " +
                               "FILTER(lang(?genre) = 'en') }}. " +
                    "OPTIONAL {{ ?movieLink <http://dbpedia.org/ontology/releaseDate> ?releaseDate }}. " +

                    "{0}" +
                    "{1}" +
                    "FILTER(lang(?title) = 'en') " +
                "}}" +
                "ORDER BY DESC(?releaseDate)" +
                "{2}";
            string expected = String.Format(queryPattern, genreMatch, dateMatch, limit);

            LuisJSONModel data = new LuisJSONModel();

            string currDir = Directory.GetCurrentDirectory();
            var file = Path.Combine(@"C:\Users\n773773\Source\Repos\Dissertation-Project\The application\SemanticWebNPLSearchEngine.Tests\Classes\TestItems");
            data = JsonConvert.DeserializeObject<LuisJSONModel>(File.ReadAllText(file));

            string output = utilities.ExtractLuisData(data);

            Assert.AreEqual(expected, output);
        }
    }
}