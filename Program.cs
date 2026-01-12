// See https://aka.ms/new-console-template for more information
using System.Globalization;
using System.Text.Json;
using RefatoringMartinFowler;
using RefatoringMartinFowler.models;

var playsJson = File.ReadAllText(@"C:\Projetos\RefatoringMartinFowler\jsons\Plays.json");
var invoiceJson = File.ReadAllText(@"C:\Projetos\RefatoringMartinFowler\jsons\Invoices.json");

var plays = JsonSerializer.Deserialize<Dictionary<string, Play>>(playsJson) ?? throw new Exception("Plays deserialization resulted in null");
var invoices = JsonSerializer.Deserialize<List<Invoice>>(invoiceJson) ?? throw new Exception("Invoices deserialization resulted in null");
var generator = new StatementGenerator();

var statement = generator.Statement(invoices.First(), plays);
Console.WriteLine(statement);

