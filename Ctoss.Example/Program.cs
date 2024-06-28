﻿using Ctoss.Configuration;
using Ctoss.Example;
using Ctoss.Extensions;

CtossSettingsBuilder.Create()
    .Entity<ExampleEntity>()
    .Property("Property", x => x.Property + x.Property2, p => { p.IgnoreCase = true; })
    .Apply()
    .Entity<ExampleNumericEntity>()
    .Property("virtual", x => x.A + x.B)
    .Apply();

const string jsonFilter =
    """
    {
        "PrOpErTy": {
            "filterType": "date",
            "condition1": {
                "filterType": "date",
                "type": "inRange",
                "dateFrom": "10/10/2002",
                "dateTo": "10/12/2020"
            },
            "conditions": [
                {
                    "filterType": "date",
                    "type": "inRange",
                    "date": "10/10/2002",
                    "dateTo": "10/12/2020"
                }
            ]
        }
    }
    """;

const string jsonNumericFilter =
    """
    {
        "virtual": {
            "filterType": "number",
            "condition1": {
                "filterType": "number",
                "type": "inRange",
                "filter": "10",
                "filterTo": "70"
            },
            "conditions": [
                {
                    "filterType": "number",
                    "type": "GreaterThan",
                    "filter": "10"
                }
            ]
        }
    }
    """;

/*
 * The CTOSS gives you three overloads of the method WithFilter which evaluates a given filter and provides you with
 * a filtering Expression<Func<T, bool>> fully compatible with IQueryable and EF.
 *
 * Overloads:
 * - WithFilter<T>(this IQueryable<T> query, string jsonFilter)
 * - WithFilter<T>(this IQueryable<T> query, string propertyName, Filter filter)
 * - WithFilter<T>(this IQueryable<T> query, Dictionary<string, Filter> filters)
 */
var entities = ExampleEntityFaker.GetN(100).AsQueryable()
    .WithFilter(jsonFilter) // <-- This is the extension method from the ctoss library
    .ToList();

Console.WriteLine("Filtered entities:");
foreach (var entity in entities) Console.WriteLine(entity.Property);

Console.WriteLine("\nNumeric entities:");

var numericEntities = ExampleNumericEntityFaker.GetN(100).AsQueryable()
    .WithFilter(jsonNumericFilter) // <-- This is the extension method from the ctoss library
    .ToList();

foreach (var entity in numericEntities) Console.WriteLine($"A: {entity.A}, B: {entity.B}");
