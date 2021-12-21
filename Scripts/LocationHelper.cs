using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Linq;
using UnityEngine;
using DaggerfallWorkshop;
using DaggerfallWorkshop.Game.Utility.ModSupport;
using DaggerfallWorkshop.Utility;
using DaggerfallWorkshop.Game;
using DaggerfallWorkshop.Utility.AssetInjection;
using DaggerfallWorkshop.Game.Items;
using System.Globalization;
using DaggerfallWorkshop.Game.Serialization;

namespace LocationLoader
{
    public static class LocationHelper
    {
        public const string locationInstanceFolder = "StreamingAssets/Locations/";
        public const string locationPrefabFolder = "StreamingAssets/Locations/LocationPrefab/";

        const float animalSoundMaxDistance = 768 * MeshReader.GlobalScale; //Used for the objects with sound

        //List of objects

        /*
            { "woodPillar", new string[] { "100.14", "100.15", "100.16" } },
        
            { "gems", new string[] { "216.10", "216.11", "216.12", "216.13", "216.14", "216.15", "216.16", "216.17", "216.18", "216.19" } },
            { "hammers", new string[] { "214.2", "214.3" } },
            { "hats", new string[] { "204.3", "204.4", "204.5" } },
            { "loomtables", new string[] { "200.15", "200.16" } },
            { "magnifyingglasses", new string[] { "208.1", "208.5" } },
            { "miniaturehouses", new string[] { "211.37", "211.38", "211.39" } },
            { "parchments", new string[] { "209.5", "209.6", "209.7", "209.8", "209.9", "209.10" } },
            { "piecesofarmor", new string[] { "207.9", "207.10", "207.11", "207.12", "207.13", "207.14" } },
            { "pillows", new string[] { "200.11", "200.13" } },
            { "rings", new string[] { "216.4", "216.5" } },
            { "rollsofcloth", new string[] { "204.6", "204.7", "204.8" } },
            { "scoops", new string[] { "214.4", "214.11" } },
            { "shovels", new string[] { "214.0", "214.1" } },
            { "smokingpipes", new string[] { "211.24", "211.25" } },
            { "statuejulianos", new string[] { "97.6", "97.7", "97.8" } },
            { "statuekynareth", new string[] { "97.13", "97.14", "97.15", "97.16", "97.17" } },
            { "statuemen", new string[] { "97.0", "97.9" } },
            { "statuewomen", new string[] { "97.2", "97.4", "97.5", "97.10", "97.11" } },
            { "statuezenithar", new string[] { "97.1", "97.18", "97.19", "97.20", "97.21" } },
            { "statuettes", new string[] { "202.5", "202.6" } },
            { "tablets", new string[] { "209.11", "209.12", "209.13", "209.14", "209.15" } },
            { "treasure", new string[] { "216.0", "216.2", "216.22", "216.23", "216.24", "216.25", "216.26", "216.27", "216.28", "216.29", "216.30", "216.31", "216.32", "216.33", "216.34", "216.35", "216.37", "216.38", "216.39", "216.41", "216.42", "216.43", "216.44", "216.45", "216.46", "216.47" } },
            { "wagonwheels", new string[] { "211.15", "211.16" } },
            { "weapons", new string[] { "207.0", "207.1", "207.2", "207.3", "207.4", "207.5", "207.6", "207.7", "207.8", "207.15", "207.16" } },
            { "wheelbarrows", new string[] { "205.27", "205.28" } },
            { "woodlandrocks", new string[] { "504.4", "504.3", "504.5", "504.6" } },
            { "woodlandtrees", new string[] { "504.11", "504.12", "504.13", "504.14", "504.15", "504.16", "504.17", "504.18", "504.19", "504.20", "504.25", "504.30", "504.31" } },
            { "woodlandplants", new string[] { "504.21", "504.22", "504.23", "504.26", "504.2", "504.1", "504.7", "504.8", "504.9", "504.10", "504.24", "504.27", "504.28", "504.29" } },
            { "ceilingroots", new string[] { "213.7", "213.8", "213.9", "213.10" } },
            { "columns", new string[] { "203.2", "203.3", "203.4", "203.0", "203.1", "203.5", "203.6" } },
            { "corpsepiles", new string[] { "206.22", "206.23", "206.24" } },
            { "crucifiedcorpse", new string[] { "206.15", "206.16" } },
            { "hangingchains", new string[] { "211.4", "211.5", "211.6", "211.7" } },
            { "headspikes", new string[] { "206.17", "206.18" } },
            { "impaledcorpses", new string[] { "206.11", "206.12", "206.13" } },
            { "ironmaidens", new string[] { "211.26", "211.27" } },
            { "skulls", new string[] { "206.0", "206.1", "206.3", "206.4", "206.5", "206.6" } },
            { "skullsonpikes", new string[] { "206.2", "206.7", "206.9" } },
            { "stalactites", new string[] { "300.0", "300.1", "300.2", "300.3", "300.4", "300.5" } },
            { "stalagmites", new string[] { "300.6", "300.7", "300.8", "300.9", "300.10", "300.11", "300.12", "300.13", "300.14", "300.15" } },
            { "statuesmonsters", new string[] { "98.0", "98.1", "98.2", "98.3", "98.4", "98.5", "98.6", "98.7", "98.8", "98.9", "98.10", "98.11", "98.12", "98.13", "98.14", "202.0", "202.1", "202.2", "202.3", "202.4" } },
            { "stocks", new string[] { "211.18", "211.19" } },
            { "tombstones", new string[] { "206.19", "206.20", "206.21" } },
            { "underwateranimated", new string[] { "106.0", "106.1", "106.2", "106.3", "106.4", "106.5", "106.6" } },
            { "underwater", new string[] { "105.0", "105.1", "105.2", "105.3", "105.4", "105.5", "105.6", "105.7", "105.8", "105.9", "105.10" } },
            { "cats", new string[] { "201.7", "201.8" } },
            { "cows", new string[] { "201.3", "201.4" } },
            { "dogs", new string[] { "201.9", "201.10" } },
            { "horses", new string[] { "201.0", "201.1" } },
            { "pigs", new string[] { "201.5", "201.6" } },
            { "crops", new string[] { "301.0", "301.1", "301.2", "301.3", "301.4", "301.5", "301.6", "301.7", "301.8", "301.9", "301.10", "301.11", "301.12", "301.13", "301.14", "301.15", "301.16", "301.17", "301.18", "301.19", "301.20", "301.21", "301.22", "301.23" } },
            { "fountains", new string[] { "212.2", "212.3" } },
            { "hayricks", new string[] { "212.15", "212.16" } },
            { "shrubs", new string[] { "213.15", "213.16", "213.17" } },
            { "signposts", new string[] { "212.4", "212.5", "212.6" } },
            { "standingstones", new string[] { "212.17", "212.18" } },
            { "wellpumps", new string[] { "212.8", "212.9", "212.10" } },
            { "woodposts", new string[] { "212.13", "212.14" } },
            { "candles", new string[] { "210.2", "210.3", "210.4" } },
            { "chandeliers", new string[] { "210.7", "210.9", "210.10", "210.23" } },
            { "hanginglamps", new string[] { "210.8", "210.11", "210.12", "210.13" } },
            { "hanginglanterns", new string[] { "210.22", "210.24", "210.25", "210.26", "210.27" } },
            { "mountedtorches", new string[] { "210.16", "210.17", "210.18" } },
            { "standinglanterns", new string[] { "210.14", "210.15" } },
            { "streetlamps", new string[] { "210.28", "210.29" } },
            { "plantsdesert", new string[] { "503.1", "503.7", "503.8", "503.9", "503.10", "503.14", "503.15", "503.16", "503.17", "503.23", "503.24", "503.25", "503.26", "503.27", "503.29", "503.31" } },
            { "plantshaunted", new string[] { "508.2", "508.7", "508.8", "508.9", "508.11", "508.14", "508.21", "508.22", "508.23", "508.26", "508.27", "508.28", "508.29" } },
            { "plantsmountains", new string[] { "510.2", "510.7", "510.8", "510.9", "510.10", "510.21", "510.22", "510.23", "510.26", "510.29" } },
            { "plantsrainforest", new string[] { "500.1", "500.2", "500.5", "500.6", "500.7", "500.8", "500.9", "500.10", "500.11", "500.20", "500.21", "500.22", "500.23", "500.24", "500.26", "500.27", "500.29", "500.31" } },
            { "plantssteppes", new string[] { "5.7", "5.8", "5.9", "5.10", "5.17", "5.23", "5.24", "5.25", "5.26", "5.27", "5.29", "5.31" } },
            { "plantssubtropical", new string[] { "501.1", "501.2", "501.7", "501.8", "501.9", "501.14", "501.18", "501.20", "501.21", "501.22", "501.25", "501.26", "501.27", "501.28", "501.29", "501.31" } },
            { "plantsswamp", new string[] { "502.1", "502.7", "502.8", "502.9", "502.11", "502.14", "502.20", "502.21", "502.22", "502.23", "502.26", "502.27", "502.28", "502.29", "502.31" } },
            { "plantswoodlandhills", new string[] { "506.2", "506.7", "506.8", "506.9", "506.21", "506.22", "506.23", "506.26", "506.27", "506.29", "506.31" } },
            { "rocksdesert", new string[] { "503.2", "503.3", "503.4", "503.6", "503.18", "503.19", "503.20", "503.2", "503.22" } },
            { "rockshaunted", new string[] { "508.1", "508.3", "508.4", "508.5", "508.6", "508.10", "508.12", "508.17" } },
            { "rocksmountains", new string[] { "510.1", "510.3", "510.4", "510.6", "510.14", "510.17", "510.18", "510.27", "510.28", "510.31" } },
            { "rocksrainforest", new string[] { "500.4", "500.17", "500.19", "500.28" } },
            { "rockssteppes", new string[] { "5.1", "5.2", "5.3", "5.4", "5.6", "5.18", "5.19", "5.20", "5.21", "5.22" } },
            { "rockssubtropical", new string[] { "501.3", "501.4", "501.5", "501.6", "501.10", "501.23" } },
            { "rocksswamp", new string[] { "502.2", "502.3", "502.4", "502.5", "502.6", "502.10" } },
            { "rockswoodlandhills", new string[] { "506.1", "506.3", "506.4", "506.6", "506.17", "506.18", "506.28" } },
            { "treesdesert", new string[] { "503.5", "503.11", "503.12", "503.13", "503.28", "503.30" } },
            { "treeshaunted", new string[] { "508.13", "508.15", "508.16", "508.18", "508.19", "508.20", "508.24", "508.25", "508.30", "508.31" } },
            { "treesmountains", new string[] { "510.5", "510.11", "510.12", "510.13", "510.15", "510.16", "510.19", "510.20", "510.24", "510.25", "510.30" } },
            { "treesrainforest", new string[] { "500.3", "500.12", "500.13", "500.14", "500.15", "500.16", "500.18", "500.25", "500.30" } },
            { "treessteppes", new string[] { "5.5", "5.11", "5.12", "5.13", "5.14", "5.15", "5.16", "5.28", "5.30" } },
            { "treessubtropical", new string[] { "501.11", "501.12", "501.13", "501.15", "501.16", "501.17", "501.19", "501.24", "501.30" } },
            { "treesswamp", new string[] { "502.12", "502.13", "502.15", "502.16", "502.17", "502.18", "502.19", "502.24", "502.25", "502.30" } },
            { "treeswoodlandhills", new string[] { "506.5", "506.10", "506.11", "506.12", "506.13", "506.14", "506.15", "506.16", "506.19", "506.20", "506.24", "506.25", "506.30" } },
        */

        /*
            { "coffinwood", new string[] { "41315", "41317", "41316", "41318", "41322", "41323", "41325", "41326" } },
            { "coffinstone", new string[] { "41319", "41320", "41321", "41324", "41327" } },
            { "statue", new string[] { "62323", "62325", "62327", "62329" } },
            { "statuelarg", new string[] { "62324", "62326", "62328", "62330" } },
            { "drawer", new string[] { "41034", "41050", "41037", "41036", "41035" } },
            { "lectern", new string[] { "41024", "41020", "41021", "41022" } },
            { "stool", new string[] { "41113", "41114" } },
            { "throne", new string[] { "41122", "41123", "41104" } },
            { "shelfalchemy", new string[] { "41042", "41043", "41041", "41044" } },
            { "shelfbooks", new string[] { "41006", "41019", "41026", "41015", "41018", "41025", "41014" } },
            { "shelfclothes", new string[] { "41013", "41011", "41012", "41010" } },
            { "shelffood", new string[] { "41040", "41029", "41039", "41027", "41046" } },
            { "shelfutility", new string[] { "41005", "41045" } },
            { "shelfweapons", new string[] { "41031", "41028", "41048", "41049", "41047" } },
            { "tombroken", new string[] { "43129", "43130", "43131", "43132", "43206" } },
            { "graveyardgates", new string[] { "43003", "43004", "43005", "43006", "43007", "43008", "43009", "43010", "62310", "62312", "62317" } },
            { "graveyardmonuments", new string[] { "43079", "43080", "43081", "43082", "43109", "43110", "43111", "43112", "43202", "43204", "62314" } },
            { "tombpillardark", new string[] { "43071", "43105", "43125" } },
            { "tombpillargray", new string[] { "43073", "43107", "43127" } },
            { "tombpillarred", new string[] { "43072", "43106", "43126" } },
            { "tombpillarwhite", new string[] { "43074", "43108", "43128", "43303" } },
            { "ankh", new string[] { "43083", "43084", "43085", "43086", "43121", "43122", "43123", "43124" } },
            { "casket", new string[] { "43075", "43076", "43077", "43078", "43304", "43305", "43306" } },
            { "slabdark", new string[] { "43027", "43031", "43035" } },
            { "slabgreen", new string[] { "43029", "43033", "43037" } },
            { "slabred", new string[] { "43028", "43032", "43036" } },
            { "slabwhite", new string[] { "43030", "43034", "43038" } },
            { "tomblargedark", new string[] { "43133", "43142", "43200", "43201", "43203", "43205" } },
            { "tomblargegray", new string[] { "43135", "43144" } },
            { "tomblargered", new string[] { "43134", "43143" } },
            { "tomblargewhite", new string[] { "43136", "43145" } },
            { "tombmediumdark", new string[] { "43055", "43059", "43063", "43067", "43101", "43117" } },
            { "tombmediumgray", new string[] { "43057", "43061", "43065", "43069", "43103", "43119" } },
            { "tombmediumred", new string[] { "43056", "43060", "43064", "43068", "43102", "43118" } },
            { "tombmediumwhite", new string[] { "43058", "43062", "43066", "43070", "43104", "43120" } },
            { "tombsmalldark", new string[] { "43011", "43015", "43019", "43023", "43039", "43043", "43047", "43051" } },
            { "tombsmallgray", new string[] { "43013", "43017", "43021", "43025", "43041", "43045", "43049", "43053", "43300", "43301" } },
            { "tombsmallred", new string[] { "43012", "43016", "43020", "43024", "43040", "43044", "43048", "43052", "43302" } },
            { "tombsmallwhite", new string[] { "43014", "43018", "43022", "43026", "43042", "43046", "43050", "43054" } },
        */
                
        public struct ModelSet
        {
            public string Name;
            public string[] Ids;
        }

        public static ModelSet[] modelsStructure = new ModelSet[]
        {
            new ModelSet { Name = "Buildings - Barracks", Ids = new string[] { "516" } },
            new ModelSet { Name = "Buildings - Fighters Guild", Ids = new string[] { "300" } },
            new ModelSet { Name = "Buildings - Knightly Orders", Ids = new string[] { "343" } },
            new ModelSet { Name = "Buildings - Mages Guild", Ids = new string[] { "223" } },
            new ModelSet { Name = "Buildings - Medium", Ids = new string[] { "109" } },
            new ModelSet { Name = "Buildings - Medium - Fenced", Ids = new string[] { "323" } },
            new ModelSet { Name = "Buildings - Medium - Flat", Ids = new string[] { "526" } },
            new ModelSet { Name = "Buildings - Medium - Flat - Fenced", Ids = new string[] { "605" } },
            new ModelSet { Name = "Buildings - Medium - Flat - L Shape", Ids = new string[] { "560" } },
            new ModelSet { Name = "Buildings - Medium - Flat - Round", Ids = new string[] { "603" } },
            new ModelSet { Name = "Buildings - Medium - Flat - Semi-Detached", Ids = new string[] { "707" } },
            new ModelSet { Name = "Buildings - Medium - L Shape", Ids = new string[] { "126" } },
            new ModelSet { Name = "Buildings - Medium - Noble", Ids = new string[] { "759" } },
            new ModelSet { Name = "Buildings - Medium - Round", Ids = new string[] { "502" } },
            new ModelSet { Name = "Buildings - Medium - Tower", Ids = new string[] { "501" } },
            new ModelSet { Name = "Buildings - Large", Ids = new string[] { "112" } },
            new ModelSet { Name = "Buildings - Large - Flat", Ids = new string[] { "121" } },
            new ModelSet { Name = "Buildings - Large - Flat - Round", Ids = new string[] { "617" } },
            new ModelSet { Name = "Buildings - Large - Flat - Terraced", Ids = new string[] { "658" } },
            new ModelSet { Name = "Buildings - Large - L Shape", Ids = new string[] { "0" } },
            new ModelSet { Name = "Buildings - Large - Noble", Ids = new string[] { "758" } },
            new ModelSet { Name = "Buildings - Large - Semi-Detached", Ids = new string[] { "843" } },
            new ModelSet { Name = "Buildings - Large - Semi-Detached - Noble", Ids = new string[] { "822" } },
            new ModelSet { Name = "Buildings - Large - Terraced - Noble", Ids = new string[] { "828" } },
            new ModelSet { Name = "Buildings - Palaces", Ids = new string[] { "407" } },
            new ModelSet { Name = "Buildings - Small", Ids = new string[] { "107" } },
            new ModelSet { Name = "Buildings - Small - Fenced", Ids = new string[] { "329" } },
            new ModelSet { Name = "Buildings - Small - Flat", Ids = new string[] { "527" } },
            new ModelSet { Name = "Buildings - Small - Flat - Fenced", Ids = new string[] { "607" } },
            new ModelSet { Name = "Buildings - Small - Flat - L Shape", Ids = new string[] { "554" } },
            new ModelSet { Name = "Buildings - Small - Flat - Round", Ids = new string[] { "655" } },
            new ModelSet { Name = "Buildings - Small - L SHape", Ids = new string[] { "154" } },
            new ModelSet { Name = "Buildings - Small - Tower", Ids = new string[] { "263" } },
            new ModelSet { Name = "Buildings - Taverns - Large", Ids = new string[] { "220" } },
            new ModelSet { Name = "Buildings - Taverns - Large - Flat", Ids = new string[] { "113" } },
            new ModelSet { Name = "Buildings - Taverns - Medium - Flat", Ids = new string[] { "625" } },
            new ModelSet { Name = "Buildings - Taverns - Medium", Ids = new string[] { "249" } },
            new ModelSet { Name = "Buildings - Taverns - Small", Ids = new string[] { "110" } },
            new ModelSet { Name = "Buildings - Templar Orders", Ids = new string[] { "103" } },
            new ModelSet { Name = "Buildings - Temples", Ids = new string[] { "100" } },
            new ModelSet { Name = "Buildings - Two Houses", Ids = new string[] { "324" } },
            new ModelSet { Name = "Brown Stone Fence Corner", Ids = new string[] { "517", "21105" } },
            new ModelSet { Name = "Brown Stone Fence Straight", Ids = new string[] { "518", "21104" } },
            new ModelSet { Name = "Brown Stone Fence Gateway", Ids = new string[] { "21106" } },
            new ModelSet { Name = "Brown Stone Fence End Cap", Ids = new string[] { "21107" } },
            new ModelSet { Name = "City Walls - Corner Tower", Ids = new string[] { "444" } },
            new ModelSet { Name = "City Walls - Gateway Closed", Ids = new string[] { "447" } },
            new ModelSet { Name = "City Walls - Gateway Opened", Ids = new string[] { "446" } },
            new ModelSet { Name = "City Walls - Straight", Ids = new string[] { "445" } },
            new ModelSet { Name = "Column", Ids = new string[] { "41900" } },
            new ModelSet { Name = "Dungeon - Cairn Entrance", Ids = new string[] { "42000" } },
            new ModelSet { Name = "Dungeon - Castle 00", Ids = new string[] { "512" } },
            new ModelSet { Name = "Dungeon - Castle 01", Ids = new string[] { "644" } },
            new ModelSet { Name = "Dungeon - Castle 02", Ids = new string[] { "711" } },
            new ModelSet { Name = "Dungeon - Castle 03", Ids = new string[] { "647" } },
            new ModelSet { Name = "Dungeon - Castle 04", Ids = new string[] { "649" } },
            new ModelSet { Name = "Dungeon - Castle 05", Ids = new string[] { "712" } },
            new ModelSet { Name = "Dungeon - Castle 06", Ids = new string[] { "717" } },
            new ModelSet { Name = "Dungeon - Castle 07", Ids = new string[] { "719" } },
            new ModelSet { Name = "Dungeon - Castle 08", Ids = new string[] { "726" } },
            new ModelSet { Name = "Dungeon - Castle 09", Ids = new string[] { "723" } },
            new ModelSet { Name = "Dungeon - Castle 10", Ids = new string[] { "852" } },
            new ModelSet { Name = "Dungeon - Castle 11", Ids = new string[] { "853" } },
            new ModelSet { Name = "Dungeon - Castle 12", Ids = new string[] { "854" } },
            new ModelSet { Name = "Dungeon - Castle 13", Ids = new string[] { "855" } },
            new ModelSet { Name = "Dungeon - Castle 14", Ids = new string[] { "856" } },
            new ModelSet { Name = "Dungeon - Castle 15", Ids = new string[] { "857" } },
            new ModelSet { Name = "Dungeon - Castle 16", Ids = new string[] { "858" } },
            new ModelSet { Name = "Dungeon - Castle 17", Ids = new string[] { "859" } },
            new ModelSet { Name = "Dungeon - Castle 18", Ids = new string[] { "860" } },
            new ModelSet { Name = "Dungeon - Castle 19", Ids = new string[] { "861" } },
            new ModelSet { Name = "Dungeon - Ground Entrance", Ids = new string[] { "43600" } },
            new ModelSet { Name = "Dungeon - Mound Entrance", Ids = new string[] { "43601" } },
            new ModelSet { Name = "Dungeon - Ruin Entrance", Ids = new string[] { "43604" } },
            new ModelSet { Name = "Dungeon - Stone Entrance", Ids = new string[] { "40012" } },
            new ModelSet { Name = "Dungeon - Tree Entrance", Ids = new string[] { "42001" } },
            new ModelSet { Name = "Fortifications - Corner", Ids = new string[] { "448" } },
            new ModelSet { Name = "Fortifications - Gateway", Ids = new string[] { "450" } },
            new ModelSet { Name = "Fortifications - S Shape", Ids = new string[] { "39003" } },
            new ModelSet { Name = "Fortifications - Straight", Ids = new string[] { "449" } },
            new ModelSet { Name = "Fountains", Ids = new string[] { "41220", "41221", "41222" } },
            new ModelSet { Name = "Guard Tower", Ids = new string[] { "522" } },
            new ModelSet { Name = "Guard Tower - Top", Ids = new string[] { "20027" } },
            new ModelSet { Name = "Hedge Maze", Ids = new string[] { "735" } },
            new ModelSet { Name = "Hedgerow - Corner - Entrance", Ids = new string[] { "745" } },
            new ModelSet { Name = "Hedgerow - Corner", Ids = new string[] { "744" } },
            new ModelSet { Name = "Hedgerow - 3 Way", Ids = new string[] { "751" } },
            new ModelSet { Name = "Hedgerow - 4 Way", Ids = new string[] { "40017" } },
            new ModelSet { Name = "Hedgerow - End", Ids = new string[] { "739" } },
            new ModelSet { Name = "Hedgerow - End Cap", Ids = new string[] { "753" } },
            new ModelSet { Name = "Hedgerow - Entrance", Ids = new string[] { "746" } },
            new ModelSet { Name = "Hedgerow - Single", Ids = new string[] { "41236" } },
            new ModelSet { Name = "Hedgerow - Straight", Ids = new string[] { "747" } },
            new ModelSet { Name = "Large Shrines", Ids = new string[] { "74094" } },
            new ModelSet { Name = "Mounds", Ids = new string[] { "41215" } },
            new ModelSet { Name = "Mounds - Carved", Ids = new string[] { "41216" } },
            new ModelSet { Name = "Pillars - Wood", Ids = new string[] { "41901" } },
            new ModelSet { Name = "Pillars - Marble", Ids = new string[] { "62315" } },
            new ModelSet { Name = "Pyramid", Ids = new string[] { "74121" } },
            new ModelSet { Name = "Ruins - Columns", Ids = new string[] { "41722" } },
            new ModelSet { Name = "Ruins - Buildings", Ids = new string[] { "41720" } },
            new ModelSet { Name = "Ships / Boats", Ids = new string[] { "41502", "41504", "41501", "909", "910" } },
            new ModelSet { Name = "Shipwreck", Ids = new string[] { "41509" } },
            new ModelSet { Name = "Signs - Akatosh Temple", Ids = new string[] { "43733", "43752", "43714" } },
            new ModelSet { Name = "Signs - Alchemist Shop", Ids = new string[] { "43720", "43739", "43702" } },
            new ModelSet { Name = "Signs - Arkay Temple", Ids = new string[] { "43718", "43737", "43700" } },
            new ModelSet { Name = "Signs - Armor Shop", Ids = new string[] { "43725", "43744" } },
            new ModelSet { Name = "Signs - Bank", Ids = new string[] { "43730", "43749", "43711" } },
            new ModelSet { Name = "Signs - Clothes Shop", Ids = new string[] { "43721", "43740", "43703" } },
            new ModelSet { Name = "Signs - Dibella Temple", Ids = new string[] { "43724", "43743", "43706" } },
            new ModelSet { Name = "Signs - General Store", Ids = new string[] { "43731", "43750", "43712" } },
            new ModelSet { Name = "Signs - Jewelry Shop", Ids = new string[] { "43726", "43745", "43707" } },
            new ModelSet { Name = "Signs - Julianos Temple", Ids = new string[] { "43719", "43738", "43701" } },
            new ModelSet { Name = "Signs - Kynareth Temple", Ids = new string[] { "43734", "43753", "43715" } },
            new ModelSet { Name = "Signs - Library", Ids = new string[] { "43735", "43754", "43716" } },
            new ModelSet { Name = "Signs - Mages Guild", Ids = new string[] { "43722", "43741", "43704" } },
            new ModelSet { Name = "Signs - Mara Temple", Ids = new string[] { "43728", "43747", "43709" } },
            new ModelSet { Name = "Signs - Pawn Shop", Ids = new string[] { "43732", "43751", "43713" } },
            new ModelSet { Name = "Signs - Stendarr Temple", Ids = new string[] { "43729", "43748", "43710" } },
            new ModelSet { Name = "Signs - Tavern", Ids = new string[] { "43727", "43746", "43708" } },
            new ModelSet { Name = "Signs - Weapon Shop", Ids = new string[] { "43736", "43755", "43717" } },
            new ModelSet { Name = "Signs - Zenithar Temple", Ids = new string[] { "43723", "43742", "43705" } },
            new ModelSet { Name = "Stone Bridges", Ids = new string[] { "41211" } },
            new ModelSet { Name = "Towers - Hexagonal", Ids = new string[] { "41223", "41224", "41225", "41226", "41227", "41228", "41229" } },
            new ModelSet { Name = "Towers - Pentagonal", Ids = new string[] { "41230", "41231", "41232", "41233", "41234" } },
            new ModelSet { Name = "Unique - Castle Daggerfall", Ids = new string[] { "521" } },
            new ModelSet { Name = "Unique - Castle Llugwych", Ids = new string[] { "457" } },
            new ModelSet { Name = "Unique - Castle Wayrest", Ids = new string[] { "900" } },
            new ModelSet { Name = "Unique - Direnni Tower", Ids = new string[] { "500" } },
            new ModelSet { Name = "Unique - Lysandus' Tomb", Ids = new string[] { "454" } },
            new ModelSet { Name = "Unique - Orsinium Palace", Ids = new string[] { "455" } },
            new ModelSet { Name = "Unique - Scourg Barrow", Ids = new string[] { "456" } },
            new ModelSet { Name = "Unique - Sentinel Palace", Ids = new string[] { "633" } },
            new ModelSet { Name = "Unique - Sentinel Palace - Walls?", Ids = new string[] { "628" } },
            new ModelSet { Name = "Unique - Shedungent", Ids = new string[] { "452" } },
            new ModelSet { Name = "Unique - Woodborne Hall", Ids = new string[] { "453" } },
            new ModelSet { Name = "Windmill", Ids = new string[] { "41600" } },
            new ModelSet { Name = "Wooden Fence - Broken", Ids = new string[] { "21103" } },
            new ModelSet { Name = "Wooden Fence 01 - End Cap", Ids = new string[] { "41200" } },
            new ModelSet { Name = "Wooden Fence 01 - Mid", Ids = new string[] { "41201" } },
            new ModelSet { Name = "Wooden Fence 02 - End Cap", Ids = new string[] { "41203" } },
            new ModelSet { Name = "Wooden Fence 02 - Mid", Ids = new string[] { "41204" } },
            new ModelSet { Name = "Wooden Fence 03 - End Cap", Ids = new string[] { "41206" } },
            new ModelSet { Name = "Wooden Fence 03 - Mid", Ids = new string[] { "41207" } },
        };

        public static ModelSet[] modelsClutter = new ModelSet[]
        {
            new ModelSet { Name = "Anvil", Ids = new string[] { "41118" } },
            new ModelSet { Name = "Armor", Ids = new string[] { "74226" } },
            new ModelSet { Name = "Arrow", Ids = new string[] { "99800" } },
            new ModelSet { Name = "Axe", Ids = new string[] { "74225" } },
            new ModelSet { Name = "Banners (Daggerfall)", Ids = new string[] { "42536", "42548", "42560", "42500", "42512", "42524" } },
            new ModelSet { Name = "Banners (Direnni)", Ids = new string[] { "42540", "42552", "42564", "42504", "42516", "42528" } },
            new ModelSet { Name = "Banners (Dwynnen)", Ids = new string[] { "42541", "42553", "42565", "42505", "42517", "42529" } },
            new ModelSet { Name = "Banners (Order of the Flame)", Ids = new string[] { "42543", "42507", "42519", "42531", "42555", "42567" } },
            new ModelSet { Name = "Banners (Order of the Lamp)", Ids = new string[] { "42542", "42554", "42566", "42506", "42518", "42530" } },
            new ModelSet { Name = "Banners (Sentinel)", Ids = new string[] { "42537", "42549", "42561", "42501", "42513", "42525" } },
            new ModelSet { Name = "Banners (Wayrest)", Ids = new string[] { "42538", "42550", "42562", "42502", "42514", "42526" } },
            new ModelSet { Name = "Banners (Large)", Ids = new string[] { "42558", "42570", "42546", "42557", "42569", "42545", "42544", "42547", "42556", "42568", "42559", "42571", "42539", "42551", "42563" } },
            new ModelSet { Name = "Banners (Small)", Ids = new string[] { "42510", "42522", "42534", "42533", "42509", "42521", "42508", "42520", "42532", "42511", "42523", "42503", "42515", "42527", "42535" } },
            new ModelSet { Name = "Block Noose Support", Ids = new string[] { "41703" } },
            new ModelSet { Name = "Bulletin Board", Ids = new string[] { "41739" } },
            new ModelSet { Name = "Carpets", Ids = new string[] { "74800", "74801", "74802", "74803", "74804", "74805", "74806", "74807", "74808" } },
            new ModelSet { Name = "Cart - Empty", Ids = new string[] { "41214" } },
            new ModelSet { Name = "Cart - Full", Ids = new string[] { "41241" } },
            new ModelSet { Name = "Catapult", Ids = new string[] { "41407" } },
            new ModelSet { Name = "Corner Shelf", Ids = new string[] { "41128" } },
            new ModelSet { Name = "Crossbow", Ids = new string[] { "74228" } },
            new ModelSet { Name = "Fireplace", Ids = new string[] { "41116" } },
            new ModelSet { Name = "Fireplace - Corner", Ids = new string[] { "41117" } },
            new ModelSet { Name = "Ladder", Ids = new string[] { "41409" } },
            new ModelSet { Name = "Large Raised Wooden Platform", Ids = new string[] { "41405" } },
            new ModelSet { Name = "Mantlet", Ids = new string[] { "41402" } },
            new ModelSet { Name = "Mantlet - Large", Ids = new string[] { "41401" } },
            new ModelSet { Name = "Organ", Ids = new string[] { "41120" } },
            new ModelSet { Name = "Paintings", Ids = new string[] { "51115", "51116", "51117", "51118", "51119", "51120" } },
            new ModelSet { Name = "Rocks - Small", Ids = new string[] { "41704", "41710", "41712", "60711", "60712", "60713", "60714", "60715", "60716", "60717", "60718", "60719", "60720" } },
            new ModelSet { Name = "Rocks - Medium", Ids = new string[] { "41705", "41706", "41707", "41708", "41709", "41711", "41713" } },
            new ModelSet { Name = "Rocks - Large", Ids = new string[] { "41714", "41715", "41716", "41717", "41718", "41719", "60710" } },
            new ModelSet { Name = "Sawhorse", Ids = new string[] { "41125" } },
            new ModelSet { Name = "Scaffolding 1", Ids = new string[] { "41403" } },
            new ModelSet { Name = "Scaffolding 2", Ids = new string[] { "41404" } },
            new ModelSet { Name = "Spinning Wheel", Ids = new string[] { "41009" } },
            new ModelSet { Name = "Stocks 1", Ids = new string[] { "41700" } },
            new ModelSet { Name = "Stocks 2", Ids = new string[] { "41701" } },
            new ModelSet { Name = "Support Beam", Ids = new string[] { "62319" } },
            new ModelSet { Name = "Support Beam - Arched", Ids = new string[] { "62321" } },
            new ModelSet { Name = "Support Beam - Diagonal", Ids = new string[] { "62318" } },
            new ModelSet { Name = "Swords", Ids = new string[] { "74224", "74227" } },
            new ModelSet { Name = "Sword - Large", Ids = new string[] { "74095" } },
            new ModelSet { Name = "Tents", Ids = new string[] { "41606" } },
            new ModelSet { Name = "Trebuchet", Ids = new string[] { "41406" } },
            new ModelSet { Name = "Trellis", Ids = new string[] { "41240" } },
            new ModelSet { Name = "Water Trough", Ids = new string[] { "41209" } },
            new ModelSet { Name = "Water Trough - Empty", Ids = new string[] { "41210" } },
            new ModelSet { Name = "Wood Stake", Ids = new string[] { "41400" } },
            new ModelSet { Name = "Wooden Plank", Ids = new string[] { "41242" } },
            new ModelSet { Name = "Wooden Tree Log", Ids = new string[] { "41734", "41735", "41736", "41737", "41738" } },
        };

        public static ModelSet[] modelsDungeon = new ModelSet[]
        {
            new ModelSet { Name = "Altars", Ids = new string[] { "51110", "51111", "41304", "41307", "41309", "41310", "41311", "41305", "51112", "51113", "51114" } },
            new ModelSet { Name = "Cage - Medium", Ids = new string[] { "41313" } },
            new ModelSet { Name = "Cage - Small", Ids = new string[] { "41312" } },
            new ModelSet { Name = "Coffins - Stone", Ids = new string[] { "41319" } },
            new ModelSet { Name = "Coffins - Wood", Ids = new string[] { "41315" } },
            new ModelSet { Name = "Statues", Ids = new string[] { "62323" } },
            new ModelSet { Name = "Statues - Large", Ids = new string[] { "62324" } },
            new ModelSet { Name = "Pedestal - Stone", Ids = new string[] { "74091" } },
            new ModelSet { Name = "Pedestal - Wood", Ids = new string[] { "74082" } },
            new ModelSet { Name = "Torture Recliner - Knives", Ids = new string[] { "41300" } },
            new ModelSet { Name = "Torture Table - Knives", Ids = new string[] { "41301" } },
            new ModelSet { Name = "Torture Table - Rack", Ids = new string[] { "41303" } },
            new ModelSet { Name = "Torture Table - Spikes", Ids = new string[] { "41302" } },
        };

        public static ModelSet[] modelsFurniture = new ModelSet[]
        {
            new ModelSet { Name = "Beds", Ids = new string[] { "41000", "41001", "41002" } },
            new ModelSet { Name = "Benches", Ids = new string[] { "41105", "41106", "41107", "41126", "51108", "51109", "43307" } },
            new ModelSet { Name = "Cabinet", Ids = new string[] { "41007" } },
            new ModelSet { Name = "Cabinet - Double", Ids = new string[] { "41051" } },
            new ModelSet { Name = "Chairs", Ids = new string[] { "41100", "41101", "41102", "41103" } },
            new ModelSet { Name = "Chests", Ids = new string[] { "41811", "41812", "41813" } },
            new ModelSet { Name = "Crates", Ids = new string[] { "41815", "41816", "41817", "41818", "41819", "41820", "41821", "41822", "41823", "41824", "41825", "41826", "41827", "41828", "41829", "41830", "41831", "41832", "41833", "41834" } },
            new ModelSet { Name = "Cupboard", Ids = new string[] { "41003" } },
            new ModelSet { Name = "Cupboard - Double", Ids = new string[] { "41004" } },
            new ModelSet { Name = "Drawers", Ids = new string[] { "41034" } },
            new ModelSet { Name = "Dresser", Ids = new string[] { "41032" } },
            new ModelSet { Name = "Lecterns", Ids = new string[] { "41024" } },
            new ModelSet { Name = "Shelves - Alchemy", Ids = new string[] { "41042" } },
            new ModelSet { Name = "Shelves - Books", Ids = new string[] { "41006" } },
            new ModelSet { Name = "Shelves - Clothes", Ids = new string[] { "41013" } },
            new ModelSet { Name = "Shelves - Drinks", Ids = new string[] { "41124" } },
            new ModelSet { Name = "Shelves - Empty", Ids = new string[] { "41030" } },
            new ModelSet { Name = "Shelves - Food", Ids = new string[] { "41040" } },
            new ModelSet { Name = "Shelves - Utility", Ids = new string[] { "41005" } },
            new ModelSet { Name = "Shelves - Weapons", Ids = new string[] { "41031" } },
            new ModelSet { Name = "Tables", Ids = new string[] { "41108", "41109", "41110", "41111", "41112", "41130", "51103", "51104" } },
            new ModelSet { Name = "Thrones", Ids = new string[] { "41122" } },
        };

        public static ModelSet[] modelsGraveyard = new ModelSet[]
        {
            new ModelSet { Name = "Graveyard Gate Door Right", Ids = new string[] { "43000" } },
            new ModelSet { Name = "Graveyard Gate Door Mid", Ids = new string[] { "43001" } },
            new ModelSet { Name = "Graveyard Gate Door Left", Ids = new string[] { "43002" } },
            new ModelSet { Name = "Graveyard Gates", Ids = new string[] { "43003" } },
            new ModelSet { Name = "Graveyard Monuments", Ids = new string[] { "43079" } },
            new ModelSet { Name = "Mausoleum Dark", Ids = new string[] { "43138" } },
            new ModelSet { Name = "Mausoleum Gray", Ids = new string[] { "43140" } },
            new ModelSet { Name = "Mausoleum Red 1", Ids = new string[] { "43139" } },
            new ModelSet { Name = "Mausoleum Red 2", Ids = new string[] { "41619" } },
            new ModelSet { Name = "Mausoleum White 1", Ids = new string[] { "43141" } },
            new ModelSet { Name = "Mausoleum White 2", Ids = new string[] { "43137" } },
            new ModelSet { Name = "Pillar Tombs (Dark)", Ids = new string[] { "43071" } },
            new ModelSet { Name = "Pillar Tombs (Gray)", Ids = new string[] { "43073" } },
            new ModelSet { Name = "Pillar Tombs (Red)", Ids = new string[] { "43072" } },
            new ModelSet { Name = "Pillar Tombs (White)", Ids = new string[] { "43074" } },
            new ModelSet { Name = "Slab - Marble", Ids = new string[] { "62322" } },
            new ModelSet { Name = "Slabs - Stone (Dark)", Ids = new string[] { "43027" } },
            new ModelSet { Name = "Slabs - Stone (Gray)", Ids = new string[] { "43029" } },
            new ModelSet { Name = "Slabs - Stone (Red)", Ids = new string[] { "43028" } },
            new ModelSet { Name = "Slabs - Stone (White)", Ids = new string[] { "43030" } },
            new ModelSet { Name = "Stone Ankhs", Ids = new string[] { "43083" } },
            new ModelSet { Name = "Stone Caskets", Ids = new string[] { "43075" } },
            new ModelSet { Name = "Tombstone Wall Dark", Ids = new string[] { "43113" } },
            new ModelSet { Name = "Tombstone Wall Red", Ids = new string[] { "43114" } },
            new ModelSet { Name = "Tombstone Wall Gray", Ids = new string[] { "43115" } },
            new ModelSet { Name = "Tombstone Wall White", Ids = new string[] { "43116" } },
            new ModelSet { Name = "Tombstones - Broken", Ids = new string[] { "43129" } },
            new ModelSet { Name = "Tombstones - Large (Dark)", Ids = new string[] { "43133" } },
            new ModelSet { Name = "Tombstones - Large (Gray)", Ids = new string[] { "43135" } },
            new ModelSet { Name = "Tombstones - Large (Red)", Ids = new string[] { "43134" } },
            new ModelSet { Name = "Tombstones - Large (White)", Ids = new string[] { "43136" } },
            new ModelSet { Name = "Tombstones - Medium (Dark)", Ids = new string[] { "43055" } },
            new ModelSet { Name = "Tombstones - Medium (Gray)", Ids = new string[] { "43057" } },
            new ModelSet { Name = "Tombstones - Medium (Red)", Ids = new string[] { "43056" } },
            new ModelSet { Name = "Tombstones - Medium (White)", Ids = new string[] { "43058" } },
            new ModelSet { Name = "Tombstones - Small (Dark)", Ids = new string[] { "43011" } },
            new ModelSet { Name = "Tombstones - Small (Gray)", Ids = new string[] { "43013" } },
            new ModelSet { Name = "Tombstones - Small (Red)", Ids = new string[] { "43012" } },
            new ModelSet { Name = "Tombstones - Small (White)", Ids = new string[] { "43014" } },
        };

        public static ModelSet[] models = modelsStructure
            .Concat(modelsClutter)
            .Concat(modelsDungeon)
            .Concat(modelsFurniture)
            .Concat(modelsGraveyard)
            .OrderBy(modelSet => modelSet.Name)
            .ToArray();

        public struct BillboardSet
        {
            public string Name;
            public string[] Ids;
        }

        public static BillboardSet[] billboardsPeople = new BillboardSet[]
        {
            new BillboardSet { Name = "Beggars", Ids = new string[] { "182.30", "182.21", "182.31", "182.44", "183.15", "183.17", "182.29", "184.27" } },
            new BillboardSet { Name = "Children", Ids = new string[] { "182.4", "184.15", "182.38", "182.42", "182.43", "182.53", "182.52" } },
            new BillboardSet { Name = "Commoners - Men", Ids = new string[] { "182.20", "197.1", "357.1", "184.17", "182.46", "182.17", "182.16", "182.24", "182.23", "184.20", "184.24", "184.25", "182.25", "334.0", "182.39", "182.18", "182.13", "182.14", "182.19" } },
            new BillboardSet { Name = "Commoners - Women", Ids = new string[] { "184.30", "184.32", "182.47", "182.26", "184.28", "184.29", "197.7", "184.33", "334.5", "182.45", "184.18", "184.23", "184.1", "184.22", "184.26" } },
            new BillboardSet { Name = "Cooks", Ids = new string[] { "182.7", "182.8" } },
            new BillboardSet { Name = "Daedric Princes", Ids = new string[] { "175.0", "175.1", "175.2", "175.3", "175.4", "175.5", "175.6", "175.7", "175.8", "175.9", "175.10", "175.11", "175.12", "175.13", "175.14", "175.15" } },
            new BillboardSet { Name = "Dark Brotherhood", Ids = new string[] { "176.6", "176.5", "176.4", "176.3", "176.2", "176.1", "176.0" } },
            new BillboardSet { Name = "Elders", Ids = new string[] { "184.21" } },
            new BillboardSet { Name = "Horse Rider", Ids = new string[] { "184.34" } },
            new BillboardSet { Name = "Innkeepers", Ids = new string[] { "346.1", "184.16", "182.1", "182.2", "182.3" } },
            new BillboardSet { Name = "Jesters", Ids = new string[] { "182.5", "182.6", "182.49" } },
            new BillboardSet { Name = "Knights", Ids = new string[] { "183.2", "183.3", "183.4" } },
            new BillboardSet { Name = "Mages", Ids = new string[] { "177.4", "177.3", "177.2", "177.1", "177.0", "182.41", "334.1", "182.40" } },
            new BillboardSet { Name = "Minstrels", Ids = new string[] { "182.37", "184.3", "182.50", "182.51" } },
            new BillboardSet { Name = "Necromancers", Ids = new string[] { "178.5", "178.6", "178.1", "178.0", "178.4", "178.2", "178.3" } },
            new BillboardSet { Name = "Noblemen", Ids = new string[] { "183.5", "183.16", "180.3", "183.0", "183.10", "183.11", "183.13", "183.20", "183.6", "197.10", "197.4", "197.9", "182.15", "184.0", "184.4", "195.11", "334.17", "334.18", "346.7", "357.3", "180.2", "183.7" } },
            new BillboardSet { Name = "Noblewomen", Ids = new string[] { "180.0", "197.8", "182.27", "184.19", "182.9", "182.10", "184.5", "334.4", "346.0", "180.1", "183.1", "183.14", "183.18", "183.21", "183.8", "183.9" } },
            new BillboardSet { Name = "Orc King", Ids = new string[] { "183.19" } },
            new BillboardSet { Name = "Prisoner", Ids = new string[] { "184.31" } },
            new BillboardSet { Name = "Prostitutes", Ids = new string[] { "184.6", "184.7", "184.8", "184.9", "184.10", "184.11", "184.12", "184.13", "184.14", "182.34", "182.48" } },
            new BillboardSet { Name = "Serving Girl", Ids = new string[] { "182.11", "182.12" } },
            new BillboardSet { Name = "Shopkeeper", Ids = new string[] { "182.0" } },
            new BillboardSet { Name = "Smiths", Ids = new string[] { "177.5", "182.59" } },
            new BillboardSet { Name = "Snake Charmer", Ids = new string[] { "182.36" } },
            new BillboardSet { Name = "Temple", Ids = new string[] { "183.12", "182.33", "182.57", "181.7", "181.6", "181.5", "182.58", "181.4", "182.32", "182.28", "182.22", "181.3", "181.2", "181.1", "181.0" } },
            new BillboardSet { Name = "Vampires", Ids = new string[] { "182.56", "182.54", "182.55" } },
            new BillboardSet { Name = "Witch Covens", Ids = new string[] { "179.3", "179.2", "179.1", "179.0", "179.4" } },
        };

        public static BillboardSet[] billboardsInterior = new BillboardSet[] 
        {
            new BillboardSet { Name = "Clothing - Boots", Ids = new string[] { "204.1", "204.2" } },
            new BillboardSet { Name = "Clothing - Pile of Clothes", Ids = new string[] { "204.0" } },
            new BillboardSet { Name = "Clothing - Hats", Ids = new string[] { "204.3" } },
            new BillboardSet { Name = "Clothing - Rolls of Cloth", Ids = new string[] { "204.6" } },
            new BillboardSet { Name = "Containers - Barrel", Ids = new string[] { "205.0" } },
            new BillboardSet { Name = "Containers - Baskets", Ids = new string[] { "205.8", "205.9", "205.10" } },
            new BillboardSet { Name = "Containers - Buckets", Ids = new string[] { "205.29", "205.30" } },
            new BillboardSet { Name = "Containers - Chests", Ids = new string[] { "205.21", "205.22", "205.23", "205.24", "205.25", "205.26" } },
            new BillboardSet { Name = "Containers - Grain Sacks", Ids = new string[] { "205.17", "205.18", "205.19", "205.20" } },
            new BillboardSet { Name = "Containers - Pots", Ids = new string[] { "218.0", "218.1", "218.2", "218.3", "211.2", "205.41" } },
            new BillboardSet { Name = "Containers - Pouch", Ids = new string[] { "205.36" } },
            new BillboardSet { Name = "Containers - Sack", Ids = new string[] { "205.44" } },
            new BillboardSet { Name = "Equipment - Lance Rack", Ids = new string[] { "211.13" } },
            new BillboardSet { Name = "Equipment - Armor", Ids = new string[] { "207.9" } },
            new BillboardSet { Name = "Equipment - Quiver", Ids = new string[] { "205.42" } },
            new BillboardSet { Name = "Equipment - Saddle", Ids = new string[] { "204.9" } },
            new BillboardSet { Name = "Equipment - Spear Rack", Ids = new string[] { "211.14" } },
            new BillboardSet { Name = "Equipment - Sword Rack", Ids = new string[] { "211.12" } },
            new BillboardSet { Name = "Equipment - Weapons", Ids = new string[] { "207.0" } },
            new BillboardSet { Name = "Food - Apple", Ids = new string[] { "213.1" } },
            new BillboardSet { Name = "Food - Bread", Ids = new string[] { "211.31" } },
            new BillboardSet { Name = "Food - Fish Fillets", Ids = new string[] { "211.41", "211.42" } },
            new BillboardSet { Name = "Food - Fishes", Ids = new string[] { "211.8", "211.9", "211.10", "211.11" } },
            new BillboardSet { Name = "Food - Meat", Ids = new string[] { "211.40" } },
            new BillboardSet { Name = "Food - Orange", Ids = new string[] { "213.0" } },
            new BillboardSet { Name = "Housing - Bottles", Ids = new string[] { "205.11", "205.12", "205.13", "205.14", "205.15", "205.16" } },
            new BillboardSet { Name = "Housing - Chair", Ids = new string[] { "200.14" } },
            new BillboardSet { Name = "Housing - Cooking Pan", Ids = new string[] { "218.4" } },
            new BillboardSet { Name = "Housing - Cradle", Ids = new string[] { "200.18" } },
            new BillboardSet { Name = "Housing - Cups", Ids = new string[] { "200.0", "200.1", "200.2", "200.3", "200.4", "200.5", "200.6" } },
            new BillboardSet { Name = "Housing - Drapes", Ids = new string[] { "211.43", "211.44", "211.45", "211.46" } },
            new BillboardSet { Name = "Housing - Flowers", Ids = new string[] { "254.26", "254.27", "254.28", "254.29", "432.19" } },
            new BillboardSet { Name = "Housing - Hanging Spoon", Ids = new string[] { "218.6" } },
            new BillboardSet { Name = "Housing - Pillows", Ids = new string[] { "200.11" } },
            new BillboardSet { Name = "Housing - Rocking Horse", Ids = new string[] { "211.21" } },
            new BillboardSet { Name = "Laboratory - Boiling Potions", Ids = new string[] { "208.2", "253.41" } },
            new BillboardSet { Name = "Laboratory - Alchemy Bottles", Ids = new string[] { "205.1", "205.2", "205.3", "205.4", "205.5", "205.6", "205.7" } },
            new BillboardSet { Name = "Laboratory - Flasks", Ids = new string[] { "205.31", "205.32", "205.33", "205.34", "205.35", "205.43" } },
            new BillboardSet { Name = "Laboratory - Globe", Ids = new string[] { "208.0" } },
            new BillboardSet { Name = "Laboratory - Hourglass", Ids = new string[] { "208.6" } },
            new BillboardSet { Name = "Laboratory - Magnifying Glasses", Ids = new string[] { "208.1" } },
            new BillboardSet { Name = "Laboratory - Scales", Ids = new string[] { "208.3" } },
            new BillboardSet { Name = "Laboratory - Telescope", Ids = new string[] { "208.4" } },
            new BillboardSet { Name = "Library - Books", Ids = new string[] { "209.0", "209.1", "209.2", "209.3", "209.4" } },
            new BillboardSet { Name = "Library - Parchments", Ids = new string[] { "209.5" } },
            new BillboardSet { Name = "Library - Quill", Ids = new string[] { "211.1" } },
            new BillboardSet { Name = "Library - Tablets", Ids = new string[] { "209.11" } },
            new BillboardSet { Name = "Misc. - Bandages", Ids = new string[] { "211.0" } },
            new BillboardSet { Name = "Misc. - Bell", Ids = new string[] { "211.47" } },
            new BillboardSet { Name = "Misc. - Candle Snuffer", Ids = new string[] { "211.23" } },
            new BillboardSet { Name = "Misc. - Coal Pile", Ids = new string[] { "200.17" } },
            new BillboardSet { Name = "Misc. - Holy Water", Ids = new string[] { "211.49" } },
            new BillboardSet { Name = "Misc. - Icon", Ids = new string[] { "211.51" } },
            new BillboardSet { Name = "Misc. - Meat Hanger", Ids = new string[] { "211.34" } },
            new BillboardSet { Name = "Misc. - Miniature Houses", Ids = new string[] { "211.37" } },
            new BillboardSet { Name = "Misc. - Painting", Ids = new string[] { "211.57" } },
            new BillboardSet { Name = "Misc. - Smoking Pipes", Ids = new string[] { "211.24" } },
            new BillboardSet { Name = "Misc. - Statuettes", Ids = new string[] { "202.5" } },
            new BillboardSet { Name = "Misc. - Training Dummy", Ids = new string[] { "211.20" } },
            new BillboardSet { Name = "Misc. - Training Pole", Ids = new string[] { "211.30" } },
            new BillboardSet { Name = "Plants - Hanged", Ids = new string[] { "213.13", "213.14" } },
            new BillboardSet { Name = "Plants - Potted", Ids = new string[] { "213.2", "213.3", "213.4", "213.5", "213.6" } },
            new BillboardSet { Name = "Statues - Dibella", Ids = new string[] { "097.12" } },
            new BillboardSet { Name = "Statues - Julianos", Ids = new string[] { "097.6" } },
            new BillboardSet { Name = "Statues - Kynareth", Ids = new string[] { "097.13" } },
            new BillboardSet { Name = "Statues - Man", Ids = new string[] { "097.0" } },
            new BillboardSet { Name = "Statues - Stendarr", Ids = new string[] { "097.3" } },
            new BillboardSet { Name = "Statues - Women", Ids = new string[] { "097.2" } },
            new BillboardSet { Name = "Statues - Zenithar", Ids = new string[] { "097.1" } },
            new BillboardSet { Name = "Tools - Anvil", Ids = new string[] { "211.35" } },
            new BillboardSet { Name = "Tools - Bellows", Ids = new string[] { "214.9" } },
            new BillboardSet { Name = "Tools - Broom", Ids = new string[] { "214.10" } },
            new BillboardSet { Name = "Tools - Brush", Ids = new string[] { "214.12" } },
            new BillboardSet { Name = "Tools - Butter Churn", Ids = new string[] { "214.5" } },
            new BillboardSet { Name = "Tools - Fish Net", Ids = new string[] { "212.7" } },
            new BillboardSet { Name = "Tools - Hammers", Ids = new string[] { "214.3" } },
            new BillboardSet { Name = "Tools - Hoe", Ids = new string[] { "214.6" } },
            new BillboardSet { Name = "Tools - Iron", Ids = new string[] { "214.15" } },
            new BillboardSet { Name = "Tools - Loom Tables", Ids = new string[] { "200.15" } },
            new BillboardSet { Name = "Tools - Meat Hook", Ids = new string[] { "211.36" } },
            new BillboardSet { Name = "Tools - Rope", Ids = new string[] { "214.8" } },
            new BillboardSet { Name = "Tools - Scoops", Ids = new string[] { "214.4" } },
            new BillboardSet { Name = "Tools - Scythe", Ids = new string[] { "214.7" } },
            new BillboardSet { Name = "Tools - Shears", Ids = new string[] { "214.14" } },
            new BillboardSet { Name = "Tools - Shovels", Ids = new string[] { "214.0" } },
            new BillboardSet { Name = "Tools - Tongs", Ids = new string[] { "214.13" } },
        };

        public static BillboardSet[] billboardsNature = new BillboardSet[]
        {
            new BillboardSet { Name = "Animals - Camel", Ids = new string[] { "201.2" } },
            new BillboardSet { Name = "Animals - Cats", Ids = new string[] { "201.7" } },
            new BillboardSet { Name = "Animals - Cows", Ids = new string[] { "201.3" } },
            new BillboardSet { Name = "Animals - Dogs", Ids = new string[] { "201.9" } },
            new BillboardSet { Name = "Animals - Horses", Ids = new string[] { "201.0" } },
            new BillboardSet { Name = "Animals - Pigs", Ids = new string[] { "201.5" } },
            new BillboardSet { Name = "Animals - Seagull", Ids = new string[] { "201.11" } },
            new BillboardSet { Name = "Crops", Ids = new string[] { "301.0" } },
            new BillboardSet { Name = "Dead Wood", Ids = new string[] { "213.11", "213.12" } },
            new BillboardSet { Name = "Fountains", Ids = new string[] { "212.2" } },
            new BillboardSet { Name = "Hay Ricks", Ids = new string[] { "212.15" } },
            new BillboardSet { Name = "Hay Stack", Ids = new string[] { "212.1" } },
            new BillboardSet { Name = "Manure", Ids = new string[] { "253.21" } },
            new BillboardSet { Name = "Plants - Desert", Ids = new string[] { "503.1" } },
            new BillboardSet { Name = "Plants - Haunted Woodland", Ids = new string[] { "508.2" } },
            new BillboardSet { Name = "Plants - Mountains", Ids = new string[] { "510.2" } },
            new BillboardSet { Name = "Plants - Rain Forest", Ids = new string[] { "500.1" } },
            new BillboardSet { Name = "Plants - Steppes", Ids = new string[] { "005.7" } },
            new BillboardSet { Name = "Plants - Subtropical", Ids = new string[] { "501.1" } },
            new BillboardSet { Name = "Plants - Swamp", Ids = new string[] { "502.1" } },
            new BillboardSet { Name = "Plants - Woodland", Ids = new string[] { "504.21" } },
            new BillboardSet { Name = "Plants - Woodland Hills", Ids = new string[] { "506.2" } },
            new BillboardSet { Name = "Rocks - Desert", Ids = new string[] { "503.2" } },
            new BillboardSet { Name = "Rocks - Haunted Woodland", Ids = new string[] { "508.1" } },
            new BillboardSet { Name = "Rocks - Mountains", Ids = new string[] { "510.1" } },
            new BillboardSet { Name = "Rocks - Rain Forest", Ids = new string[] { "500.4" } },
            new BillboardSet { Name = "Rocks - Steppes", Ids = new string[] { "005.1" } },
            new BillboardSet { Name = "Rocks - Subtropical", Ids = new string[] { "501.3" } },
            new BillboardSet { Name = "Rocks - Swamp", Ids = new string[] { "502.2" } },
            new BillboardSet { Name = "Rocks - Woodland", Ids = new string[] { "504.4" } },
            new BillboardSet { Name = "Rocks - Woodland Hills", Ids = new string[] { "506.1" } },
            new BillboardSet { Name = "Shrubs", Ids = new string[] { "213.15" } },
            new BillboardSet { Name = "Signposts", Ids = new string[] { "212.4" } },
            new BillboardSet { Name = "Standing Stones", Ids = new string[] { "212.17" } },
            new BillboardSet { Name = "Trees - Desert", Ids = new string[] { "503.5" } },
            new BillboardSet { Name = "Trees - Haunted Woodland", Ids = new string[] { "508.13" } },
            new BillboardSet { Name = "Trees - Mountains", Ids = new string[] { "510.5" } },
            new BillboardSet { Name = "Trees - Rain Forest", Ids = new string[] { "500.3" } },
            new BillboardSet { Name = "Trees - Steppes", Ids = new string[] { "005.5" } },
            new BillboardSet { Name = "Trees - Subtropical", Ids = new string[] { "501.11" } },
            new BillboardSet { Name = "Trees - Swamp", Ids = new string[] { "502.12" } },
            new BillboardSet { Name = "Trees - Woodland", Ids = new string[] { "504.12" } },
            new BillboardSet { Name = "Trees - Woodland Hills", Ids = new string[] { "506.5" } },
            new BillboardSet { Name = "Wagon Wheels", Ids = new string[] { "211.15" } },
            new BillboardSet { Name = "Well", Ids = new string[] { "212.0" } },
            new BillboardSet { Name = "Well Pumps", Ids = new string[] { "212.8" } },
            new BillboardSet { Name = "Wheelbarrows", Ids = new string[] { "205.27" } },
            new BillboardSet { Name = "Wood Pile", Ids = new string[] { "212.11" } },
            new BillboardSet { Name = "Wood Posts", Ids = new string[] { "212.13" } },
        };

        public static BillboardSet[] billboardsLights = new BillboardSet[]
        {
            new BillboardSet { Name = "Brazier", Ids = new string[] { "210.0" } },
            new BillboardSet { Name = "Brazier - Pillar", Ids = new string[] { "210.19" } },
            new BillboardSet { Name = "Camp Fire", Ids = new string[] { "210.1" } },
            new BillboardSet { Name = "Candles", Ids = new string[] { "210.2" } },
            new BillboardSet { Name = "Candlestick", Ids = new string[] { "210.5" } },
            new BillboardSet { Name = "Chandleliers", Ids = new string[] { "210.7" } },
            new BillboardSet { Name = "Hanging Lamps", Ids = new string[] { "210.8" } },
            new BillboardSet { Name = "Hanging Lantern", Ids = new string[] { "210.22" } },
            new BillboardSet { Name = "Mounted Torches", Ids = new string[] { "210.16" } },
            new BillboardSet { Name = "Standing Candle", Ids = new string[] { "210.21" } },
            new BillboardSet { Name = "Standing Lanterns", Ids = new string[] { "210.14" } },
            new BillboardSet { Name = "Torch - Skull", Ids = new string[] { "210.6" } },
            new BillboardSet { Name = "Torch - Standing", Ids = new string[] { "210.20" } },
            new BillboardSet { Name = "Street Lamps", Ids = new string[] { "210.28" } },
        };

        public static BillboardSet[] billboardsTreasure = new BillboardSet[]
        {
                new BillboardSet { Name = "Goldpile", Ids = new string[] { "216.0", "216.1", "216.2" } },
                new BillboardSet { Name = "Gold Casket", Ids = new string[] { "216.3" } },
                new BillboardSet { Name = "Gold Coin", Ids = new string[] { "216.4" } },
                new BillboardSet { Name = "Silver Coin", Ids = new string[] { "216.5" } },
                new BillboardSet { Name = "Crowns", Ids = new string[] { "216.6", "216.7", "216.8", "216.9" } },
                new BillboardSet { Name = "Silver plate", Ids = new string[] { "216.10" } },
                new BillboardSet { Name = "Treasure", Ids = new string[] { "216.11", "216.12", "216.13", "216.14", "216.15", "216.16", "216.17"
                    , "216.18", "216.19", "216.20", "216.21", "216.22", "216.23", "216.24", "216.25", "216.26", "216.27", "216.28", "216.29", "216.30"
                    , "216.31", "216.32", "216.33", "216.34", "216.35", "216.36", "216.37", "216.38", "216.39", "216.40", "216.41", "216.42", "216.43"
                    , "216.44", "216.45", "216.46", "216.47"
                } },
        };

        public static BillboardSet[] billboardsDungeon = new BillboardSet[]
        {
            new BillboardSet { Name = "Blood", Ids = new string[] { "206.33", "206.34" } },
            new BillboardSet { Name = "Bloody Pike", Ids = new string[] { "206.27" } },
            new BillboardSet { Name = "Bones", Ids = new string[] { "206.29", "206.30", "206.31", "206.32", "206.8" } },
            new BillboardSet { Name = "Ceiling Roots", Ids = new string[] { "213.7" } },
            new BillboardSet { Name = "Chained Woman", Ids = new string[] { "211.33" } },
            new BillboardSet { Name = "Columns", Ids = new string[] { "203.2" } },
            new BillboardSet { Name = "Corpse Pile", Ids = new string[] { "206.22" } },
            new BillboardSet { Name = "Cross", Ids = new string[] { "211.32" } },
            new BillboardSet { Name = "Crucified Corpse", Ids = new string[] { "206.15" } },
            new BillboardSet { Name = "Eviscerated Animal", Ids = new string[] { "206.35" } },
            new BillboardSet { Name = "Hanged Person", Ids = new string[] { "206.36" } },
            new BillboardSet { Name = "Hanging Chains", Ids = new string[] { "211.4" } },
            new BillboardSet { Name = "Hanging Skeleton", Ids = new string[] { "206.10" } },
            new BillboardSet { Name = "Head Pile", Ids = new string[] { "206.14" } },
            new BillboardSet { Name = "Heads on Pikes", Ids = new string[] { "206.17" } },
            new BillboardSet { Name = "Impaled Corpses", Ids = new string[] { "206.11" } },
            new BillboardSet { Name = "Iron Maidens", Ids = new string[] { "211.26" } },
            new BillboardSet { Name = "Noose", Ids = new string[] { "211.22" } },
            new BillboardSet { Name = "Skeleton in Cage", Ids = new string[] { "206.26" } },
            new BillboardSet { Name = "Skulls", Ids = new string[] { "206.0" } },
            new BillboardSet { Name = "Skulls on Pikes", Ids = new string[] { "206.2" } },
            new BillboardSet { Name = "Stalactites", Ids = new string[] { "300.0" } },
            new BillboardSet { Name = "Stalagmites", Ids = new string[] { "300.6" } },
            new BillboardSet { Name = "Statues - Monsters", Ids = new string[] { "098.0" } },
            new BillboardSet { Name = "Stocks", Ids = new string[] { "211.18" } },
            new BillboardSet { Name = "Tombstones", Ids = new string[] { "206.19" } },
            new BillboardSet { Name = "Torture Rack", Ids = new string[] { "211.28" } },
            new BillboardSet { Name = "Underwater", Ids = new string[] { "105.0" } },
            new BillboardSet { Name = "Underwater - Animated", Ids = new string[] { "106.0" } },
        };

        public static BillboardSet[] billboards = billboardsPeople
            .Concat(billboardsInterior)
            .Concat(billboardsNature)
            .Concat(billboardsLights)
            .Concat(billboardsTreasure)
            .Concat(billboardsDungeon)
            .OrderBy(billboardSet => billboardSet.Name)
            .ToArray();

        public static Dictionary<string, string> editor = new Dictionary<string, string>()
        {
            { "199.4", "Rest Marker" },
            { "199.8", "Enter Marker" },
            { "199.10", "Start Marker" },
            { "199.11", "Quest Marker" },
            { "199.15", "Random Monster" },
            { "199.16", "Monster" },
            { "199.18", "Quest Item" },
            { "199.19", "Random Treasure" },
            { "199.21", "Ladder Bottom" },
            { "199.22", "Ladder Top" },
        };

        public static ModelSet[] houseParts = new ModelSet[]
        {
            new ModelSet { Name = "3 Way", Ids = new string[] { "31024" } },
            new ModelSet { Name = "3 Way - Exit", Ids = new string[] { "31030" } },
            new ModelSet { Name = "4 Way", Ids = new string[] { "31025" } },
            new ModelSet { Name = "Corner", Ids = new string[] { "31006" } },
            new ModelSet { Name = "Corner - 1 Door", Ids = new string[] { "31026", "31027" } },
            new ModelSet { Name = "Corner - 2 Doors", Ids = new string[] { "31028" } },
            new ModelSet { Name = "Corner - Diagonal", Ids = new string[] { "31031" } },
            new ModelSet { Name = "Dead End", Ids = new string[] { "31018" } },
            new ModelSet { Name = "Dead End - Exit", Ids = new string[] { "31019" } },
            new ModelSet { Name = "Dead End - 1 Door", Ids = new string[] { "31003", "31004", "31005" } },
            new ModelSet { Name = "Dead End - 2 Doors", Ids = new string[] { "31008", "31009", "31010" } },
            new ModelSet { Name = "Dead End - 2 Doors - Exit", Ids = new string[] { "31020", "31021" } },
            new ModelSet { Name = "Dead End - 3 Doors", Ids = new string[] { "31017" } },
            new ModelSet { Name = "Hall", Ids = new string[] { "31000" } },
            new ModelSet { Name = "Hall - Exit", Ids = new string[] { "31029" } },
            new ModelSet { Name = "Hall - 1 Door", Ids = new string[] { "31007" } },
            new ModelSet { Name = "Hall - 2 Doors", Ids = new string[] { "31016" } },
            new ModelSet { Name = "Single Floor", Ids = new string[] { "1000" } },
            new ModelSet { Name = "Single Ceiling", Ids = new string[] { "2000" } },
            new ModelSet { Name = "Single Doorway", Ids = new string[] { "3000" } },
            new ModelSet { Name = "Single Exit", Ids = new string[] { "3002" } },
            new ModelSet { Name = "Single Wall", Ids = new string[] { "3004" } },
            new ModelSet { Name = "Single Pillar", Ids = new string[] { "4004" } },
            new ModelSet { Name = "Stairwell", Ids = new string[] { "5000", "5001", "5002", "5003", "5004", "5005", "5006", "5007", "31022", "31023" } },
            new ModelSet { Name = "Room 1x1 - 1 Door", Ids = new string[] { "8000" } },
            new ModelSet { Name = "Room 1x1 - 2 Doors", Ids = new string[] { "31011" } },
            new ModelSet { Name = "Room 1x1 - 2 Doors - Exit", Ids = new string[] { "31014" } },
            new ModelSet { Name = "Room 1x2 - 1 Door", Ids = new string[] { "8001", "10001" } },
            new ModelSet { Name = "Room 1x3 - 1 Door", Ids = new string[] { "8002", "10002" } },
            new ModelSet { Name = "Room 1x4 - 1 Door", Ids = new string[] { "8003", "10003" } },
            new ModelSet { Name = "Room 1x5 - 1 Door", Ids = new string[] { "8004", "10004" } },
            new ModelSet { Name = "Room 1x6 - 1 Door", Ids = new string[] { "8005", "10005" } },
            new ModelSet { Name = "Room 2x1 - 1 Door", Ids = new string[] { "8006", "10006" } },
            new ModelSet { Name = "Room 2x2 - 1 Door", Ids = new string[] { "8007", "10007" } },
            new ModelSet { Name = "Room 2x2 - 2 Doors", Ids = new string[] { "34000", "34004", "34008" } },
            new ModelSet { Name = "Room 2x2 - 3 Doors", Ids = new string[] { "35000" } },
            new ModelSet { Name = "Room 2x2 - 4 Doors", Ids = new string[] { "35001" } },
            new ModelSet { Name = "Room 2x3 - 1 Door", Ids = new string[] { "8008", "10008" } },
            new ModelSet { Name = "Room 2x3 - 3 Doors", Ids = new string[] { "35003", "35004" } },
            new ModelSet { Name = "Room 2x4 - 1 Door", Ids = new string[] { "8009", "10009" } },
            new ModelSet { Name = "Room 2x4 - 2 Doors", Ids = new string[] { "34005" } },
            new ModelSet { Name = "Room 2x5 - 1 Door", Ids = new string[] { "8010", "10010" } },
            new ModelSet { Name = "Room 2x6 - 1 Door", Ids = new string[] { "8011", "10011" } },
            new ModelSet { Name = "Room 3x1 - 1 Door", Ids = new string[] { "8012", "10012" } },
            new ModelSet { Name = "Room 3x2 - 1 Door", Ids = new string[] { "8013" } },
            new ModelSet { Name = "Room 3x2 - 2 Doors", Ids = new string[] { "34002", "34006" } },
            new ModelSet { Name = "Room 3x2 - 3 Doors", Ids = new string[] { "34009" } },
            new ModelSet { Name = "Room 3x3 - 1 Door", Ids = new string[] { "8014" } },
            new ModelSet { Name = "Room 3x3 - 4 Doors", Ids = new string[] { "35002", "35009" } },
            new ModelSet { Name = "Room 3x4 - 1 Door", Ids = new string[] { "8015" } },
            new ModelSet { Name = "Room 3x5 - 1 Door", Ids = new string[] { "8016" } },
            new ModelSet { Name = "Room 3x6 - 1 Door", Ids = new string[] { "8017" } },
            new ModelSet { Name = "Room 4x1 - 1 Door", Ids = new string[] { "8018" } },
            new ModelSet { Name = "Room 4x2 - 1 Door", Ids = new string[] { "8019" } },
            new ModelSet { Name = "Room 4x2 - 2 Doors", Ids = new string[] { "34001", "34003" } },
            new ModelSet { Name = "Room 4x3 - 1 Door", Ids = new string[] { "8020" } },
            new ModelSet { Name = "Room 4x4 - 1 Door", Ids = new string[] { "8021" } },
            new ModelSet { Name = "Room 4x4 - 2 Doors", Ids = new string[] { "34007" } },
            new ModelSet { Name = "Room 4x5 - 1 Door", Ids = new string[] { "8022" } },
            new ModelSet { Name = "Room 4x6 - 1 Door", Ids = new string[] { "8023" } },
            new ModelSet { Name = "Room 5x1 - 1 Door", Ids = new string[] { "8024" } },
            new ModelSet { Name = "Room 5x2 - 1 Door", Ids = new string[] { "8025" } },
            new ModelSet { Name = "Room 5x3 - 1 Door", Ids = new string[] { "8026" } },
            new ModelSet { Name = "Room 5x4 - 1 Door", Ids = new string[] { "8027" } },
            new ModelSet { Name = "Room 5x5 - 1 Door", Ids = new string[] { "8028" } },
            new ModelSet { Name = "Room 5x6 - 1 Door", Ids = new string[] { "8029" } },
            new ModelSet { Name = "Room 6x1 - 1 Door", Ids = new string[] { "8030" } },
            new ModelSet { Name = "Room 6x2 - 1 Door", Ids = new string[] { "8031" } },
            new ModelSet { Name = "Room 6x3 - 1 Door", Ids = new string[] { "8032" } },
            new ModelSet { Name = "Room 6x4 - 1 Door", Ids = new string[] { "8033" } },
            new ModelSet { Name = "Room 6x5 - 1 Door", Ids = new string[] { "8034" } },
            new ModelSet { Name = "Room 6x6 - 1 Door", Ids = new string[] { "8035" } },
            new ModelSet { Name = "Room 3x2 - Angled 1 Door", Ids = new string[] { "35005" } },
            new ModelSet { Name = "Room 3x3 - Angled - 1 Door", Ids = new string[] { "32000" } },
            new ModelSet { Name = "Room 5x4 - Angled - 1 Door", Ids = new string[] { "32001" } },
            new ModelSet { Name = "Room 3x2 - Closet - 1 Door", Ids = new string[] { "32003" } },
            new ModelSet { Name = "Room 3x2 - Closet - 2 Doors", Ids = new string[] { "34010" } },
            new ModelSet { Name = "Room 3x3 - Closet - 1 Door", Ids = new string[] { "32002" } },
            new ModelSet { Name = "Room 3x3 - Closet - 2 Doors", Ids = new string[] { "35012" } },
            new ModelSet { Name = "Room 4x4 - Closet - 4 doors", Ids = new string[] { "34011" } },
            new ModelSet { Name = "Room 4x4 - L Shape - 3 Doors", Ids = new string[] { "35008" } },
            new ModelSet { Name = "Room 3x3 - M Shape - 1 Door", Ids = new string[] { "33001" } },
            new ModelSet { Name = "Room 3x3 - M Shape - 3 Doors", Ids = new string[] { "35010", "35011" } },
            new ModelSet { Name = "Room 3x2 - P Shape - 3 Doors", Ids = new string[] { "35007" } },
            new ModelSet { Name = "Room 4x5 - Splitted - 1 Door", Ids = new string[] { "33002" } },
            new ModelSet { Name = "Room 2x3 - Vaulted - 1 Door", Ids = new string[] { "33003" } },
            new ModelSet { Name = "Room 3x3 - Vaulted - 1 Door", Ids = new string[] { "33004" } },
            new ModelSet { Name = "Room 3x4 - Vaulted - 1 Door", Ids = new string[] { "33005" } },
            new ModelSet { Name = "Room 3x5 - Vaulted - 1 Door", Ids = new string[] { "33006" } },
            new ModelSet { Name = "Room 4x6 - Vaulted - 1 Door", Ids = new string[] { "33007" } },
        };

        /*
            { "room2x2door2", new string[] { "58038", "62028", "62128", "62228", "64305", "71101", "71106", "71201", "71206", "71301", "71306" } },
            { "room2x2door2closet", new string[] { "71409", "71509", "71609", "71700", "71800", "71900" } },
            { "room2x2door3closets", new string[] { "71702", "71802", "71902" } },
            { "room1x2door1", new string[] { "70001", "70037", "70101", "70137" } },
            { "room2x4door1arched", new string[] { "62306", "62406", "62506" } },
            { "room1x3door1", new string[] { "70038", "70102", "70138" } },
            { "room1x4door1", new string[] { "70039", "70103", "70139", "70003" } },
            { "room2x4door2", new string[] { "71107", "71207", "71307" } },
            { "room2x1door1", new string[] { "70006", "70106", "70042", "70142" } },
            { "room2x5door1", new string[] { "70010", "70046", "70110", "70146" } },
            { "room1x5door1", new string[] { "70004", "70040", "70104", "70140" } },
            { "room1x6door1", new string[] { "70005", "70041", "70105", "70141" } },
            { "room4x10door1", new string[] { "62011", "62111", "62211" } },
            { "room2x6door1", new string[] { "70011", "70047", "70111", "70147" } },
            { "room4x2door1", new string[] { "62009", "62109", "62209", "70019", "70055", "70119", "70155", "71100", "71200", "71300" } },
            { "room4x2door1arched", new string[] { "62305", "62405", "62505" } },
            { "room4x3door1shapeJ", new string[] { "70501", "70601", "70701" } },
            { "room4x4door1", new string[] { "62010", "62110", "62210", "68001", "68101", "72010", "68201", "70021", "70057", "70121", "70157" } },
            { "room4x4door1arched", new string[] { "62307", "62407", "62507" } },
            { "room4x4door1archedledgegap", new string[] { "62029", "62129", "62229" } },
            { "room4x4door2arched", new string[] { "62308", "62309", "62408", "62409", "62508", "62509" } },
            { "room2x2door3", new string[] { "71400", "71402", "71500", "71502", "71600" } },
            { "room2x2door4", new string[] { "71403", "71503", "71603", } },
            { "room4x4door1closet", new string[] { "71407", "71507", "71607", "71708", "71808", "71908", } },
            { "room2x3door1", new string[] { "70008", "70108", "70144", "70044" } },
            { "room4x6door2", new string[] { "58039", "72001", "72002" } },
            { "room4x6door3", new string[] { "72006", "72007", "72008" } },
            { "room3x2door3", new string[] { "71405", "71406", "71505", "71506", "71605", "71606", "71401", "71501", "71601" } },
            { "room4x6door1angled", new string[] { "70506", "70706", "70606" } },
            { "room4x6door1vaulted", new string[] { "70802", "70806", "71002", "70902", "70906", "71006" } },
            { "room2x4door1", new string[] { "70009", "70045", "70109", "70145", } },
            { "room4x8door2vaulted", new string[] { "70807", "70907", "71007" } },
            { "room3x1door1", new string[] { "70012", "70048", "70112", "70148" } },
            { "room6x3door1angled", new string[] { "71408", "71508", "71608" } },
            { "room3x2door1", new string[] { "70113", "70013", "70049", "70149" } },
            { "room3x2door2", new string[] { "71104", "71108", "71204", "71208", "71304", "71308" } },
            { "room6x4door1shapeB", new string[] { "70509", "70609", "70709" } },
            { "room6x4door1closet", new string[] { "70504", "70604", "70704" } },
            { "room6x4door2closet", new string[] { "71103", "71203", "71303" } },
            { "room6x4door3shapeP", new string[] { "71703", "71803", "71903" } },
            { "room6x4door3arched", new string[] { "62005", "62105", "62205" } },
            { "room3x3door1", new string[] { "70014", "70114", "70050", "70150" } },
            { "room3x3door2", new string[] { "71710", "71810", "71910" } },
            { "room6x6door2", new string[] { "58037", "72003", "72004" } },
            { "room6x6door2arched", new string[] { "62003", "62006", "62103", "62106", "62203", "62206" } },
            { "room6x6door1angled", new string[] { "70608", "70708", "70507", "70508", "70607", "70707" } },
            { "room6x6door1closet", new string[] { "70503", "70703", "70603" } },
            { "room6x6door1shapeM", new string[] { "70800", "71000", "70900" } },
            { "room6x6door3shapeM", new string[] { "71706", "71707", "71806", "71807", "71906", "71907" } },
            { "room6x6door1vaulted", new string[] { "70803", "70903", "71003" } },
            { "room6x6door2archedshapeM", new string[] { "62007", "62107", "62207" } },
            { "room6x6door2closet", new string[] { "71709", "71809", "71909" } },
            { "room6x6door2closets", new string[] { "70505", "70605", "70705" } },
            { "room6x6door3shapeL", new string[] { "71704", "71804", "71904" } },
            { "room3x3door4", new string[] { "71404", "71504", "71604", "71705", "71805", "71905" } },
            { "room6x6door4arched", new string[] { "62301", "62401", "62501" } },
            { "room6x6door4archedbridge", new string[] { "62302", "62402", "62502" } },
            { "room3x4door1", new string[] { "70015", "70051", "70115", "70151" } },
            { "room6x8door1vaulted", new string[] { "70804", "70904", "71004" } },
            { "room3x5door4", new string[] { "70016", "70052", "70116", "70152" } },
            { "room6x10door1vaulted", new string[] { "70805", "70905", "71005" } },
            { "room6x10door2vaulted", new string[] { "70808", "70908", "71008" } },
            { "room3x6door1", new string[] { "70017", "70053", "70117", "70153" } },
            { "room4x1door1", new string[] { "70018", "70054", "70118", "70154" } },
            { "room4x2door2", new string[] { "71102", "71202", "71302", "71105", "71205", "71305", "71701", "71801", "71901" } },
            { "room8x4door1shapeB", new string[] { "70500", "70600", "70700" } },
            { "room4x3door1", new string[] { "70020", "70056", "70120", "70156" } },
            { "room8x6door1angled", new string[] { "70502", "70602", "70702" } },
            { "room4x4door3", new string[] { "71109", "71209", "71309", "72016" } },
            { "room4x5door1", new string[] { "70022", "70058", "70122", "70158" } },
            { "room8x10door2vaulted", new string[] { "70809", "70909", "71009" } },
            { "room4x6door1", new string[] { "70023", "70059", "70123", "70159", "72011" } },
            { "room5x1door1", new string[] { "70024", "70060", "70124", "70160" } },
            { "room5x2door1", new string[] { "70025", "70061", "70125", "70161" } },
            { "room5x3door1", new string[] { "70026", "70062", "70126", "70162" } },
            { "room5x4door1", new string[] { "70027", "70063", "70127", "70163" } },
            { "room10x8door1split", new string[] { "70901", "71001", "70801" } },
            { "room5x5door1", new string[] { "70028", "70064", "70128", "70164" } },
            { "room10x10door2arched", new string[] { "62004", "62104", "62204" } },
            { "room10x12door2arched", new string[] { "70029", "70065", "70129", "70165" } },
            { "room6x1door1", new string[] { "70030", "70066", "70130", "70166" } },
            { "room6x2door1", new string[] { "70031", "70067", "70131", "70167" } },
            { "room6x3door1", new string[] { "70032", "70068", "70132", "70168" } },
            { "room6x4door1", new string[] { "70033", "70069", "70133", "70169", "72012" } },
            { "room6x5door1", new string[] { "70034", "70070", "70134", "70170" } },
            { "room6x6door1", new string[] { "70035", "70071", "70135", "70171" } },
            { "corrhex2way2corrniches", new string[] { "63050", "63150", "63250" } },
            { "corrhex2way", new string[] { "63000", "63024", "63100", "63124", "63200", "63224", "63001", "63101", "63201", "63002", "63102", "63202" } },
            { "corrhex2waydoor1", new string[] { "63008", "63009", "63010", "63011", "63030", "63037", "63108", "63109", "63110", "63111", "63130", "63137", "63208", "63209", "63210", "63211", "63230", "63237" } },
            { "corrhex2waydoor2", new string[] { "63012", "63013", "63018", "63019", "63020", "63021", "63038", "63112", "63113", "63118", "63119", "63120", "63121", "63138", "63212", "63213", "63218", "63219", "63220", "63221", "63238" } },
            { "corrhex2waydoor3", new string[] { "63014", "63015", "63016", "63017", "63114", "63115", "63116", "63117", "63214", "63215", "63216", "63217" } },
            { "corrhex2waydoor4", new string[] { "63025", "63125", "63225" } },
            { "corrhex2waydiag", new string[] { "63035", "63036", "63135", "63136", "63235", "63236" } },
            { "corrhex2wayniches", new string[] { "63049", "63149", "63249" } },
            { "corrhex2wayS", new string[] { "63039", "63139", "63239" } },
            { "corrhex2waywindow", new string[] { "63053", "63153", "63253", "63054", "63154", "63254", "63055", "63155", "63255" } },
            { "corrhex2waybeams", new string[] { "63022", "63023", "63059", "63122", "63123", "63159", "63222", "63223", "63259" } },
            { "corrhex2waydiagdoor1", new string[] { "63056", "63156", "63256" } },
            { "corrhex3way", new string[] { "63028", "63128", "63228" } },
            { "corrhex3waydoor1", new string[] { "63029", "63129", "63229" } },
            { "corrhex3waydiag", new string[] { "63032", "63132", "63232" } },
            { "corrhex4way", new string[] { "63042", "63142", "63242" } },
            { "corrhex4waydiag", new string[] { "63031", "63131", "63231" } },
            { "corrhexcorner", new string[] { "63033", "63034", "63133", "63134", "63233", "63234" } },
            { "corrhexcornerdoor1", new string[] { "63051", "63052", "63151", "63152", "63251", "63252" } },
            { "corrhexdeadend", new string[] { "63003", "63103", "63203" } },
            { "corrhexdeadenddoor1", new string[] { "63004", "63005", "63007", "63104", "63105", "63107", "63204", "63205", "63207" } },
            { "corrhexdeadenddoor2", new string[] { "63006", "63058", "63106", "63158", "63206", "63258" } },
            { "corrhextranscave", new string[] { "63047", "63147", "63247" } },
            { "corrhextransnarrow", new string[] { "63045", "63145", "63245" } },
            { "corrhextranssquare", new string[] { "63048", "63148", "63248" } },
            { "corrhextransarched", new string[] { "63046", "63146", "63246" } },
            { "corrhexramp", new string[] { "63040", "63041", "63140", "63141", "63240", "63241" } },
            { "corrhexrampcorridor", new string[] { "63043", "63044", "63143", "63144", "63143", "63244" } },
            { "corrhexrampdiagonal", new string[] { "63057", "63157", "63257" } },
            { "corrhexstairs", new string[] { "63026", "63027", "63126", "63127", "63226", "63227" } },
            { "corrnarrow2way", new string[] { "69010", "69007" } },
            { "corrnarrow3way", new string[] { "69002", "69004" } },
            { "corrnarrowcorner", new string[] { "69005", "69006" } },
            { "corrsquare2waydoor1", new string[] { "66007", "66107", "66207", "67011", "67111", "67211" } },
            { "corrsquare2waydoor2", new string[] { "66021", "66121", "66221", "67012", "67013", "67112", "67113", "67212", "67213" } },
            { "corrsquare2waychuteceiling", new string[] { "67325", "67425", "67525" } },
            { "corrsquare2waychuteceilingfloor", new string[] { "67335", "67435", "67535" } },
            { "corrsquare2waychutefloor", new string[] { "67302", "67402", "67502" } },
            { "corrsquare2way", new string[] { "66000", "66100", "66200", "67000", "67007", "67100", "67107", "67200", "67207", "66001", "66101", "66201", "67001", "67101", "67201", "66002", "66102", "66202", "67002", "67003", "67102", "67103", "67202", "67203" } },
            { "corrsquare3waydoor1", new string[] { "67032", "67132", "67232" } },
            { "corrsquare3way", new string[] { "67005", "67006", "67105", "67106", "67205", "67206" } },
            { "corrsquare4way", new string[] { "67004", "67104", "67204" } },
            { "corrsquarecornerdoor1", new string[] { "67123", "67022", "67023", "67122", "67123", "67222", "67223" } },
            { "corrsquarecornerchuteceiling", new string[] { "67328", "67428", "67528" } },
            { "corrsquarecornerchuteceilingfloor", new string[] { "67330", "67430", "67530" } },
            { "corrsquarecornerchutefloor", new string[] { "67300", "67400", "67500" } },
            { "corrsquarecornerporticullis", new string[] { "67031", "67131", "67231" } },
            { "corrsquarecorner", new string[] { "66006", "66106", "66206", "67009", "67109", "67209", "67008", "67108", "67208", "67010", "67110", "67210" } },
            { "corrsquaredeadenddoor1", new string[] { "66003", "66004", "66005", "66103", "66104", "66105", "66203", "66204", "66205", "67017", "67018", "67019", "67117", "67118", "67119", "67217", "67218", "67219" } },
            { "corrsquaredeadenddoor2", new string[] { "66008", "66009", "66020", "66108", "66109", "66120", "66208", "66209", "66220", "67020", "67021", "67120", "67121", "67220", "67221" } },
            { "corrsquaredeadenddoor3", new string[] { "66022", "66122", "66222" } },
            { "corrsquaredeadendchuteceiling", new string[] { "67327", "67427", "67527" } },
            { "corrsquaredeadendchuteceilingfloor", new string[] { "67329", "67429", "67529" } },
            { "corrsquaredeadendchutefloor", new string[] { "67301", "67401", "67501" } },
            { "corrsquaredeadend", new string[] { "66023", "66123", "66223", "67014", "67114", "67214" } },
            { "corrsquarerampdoor1", new string[] { "67030", "67130", "67230" } },
            { "corrsquareramp", new string[] { "65017", "67015", "67024", "67026", "67115", "67124", "67126", "67215", "67224", "67226" } },
            { "corrsquareslope", new string[] { "67027", "67028", "67029", "67127", "67128", "67129", "67227", "67228", "67229" } },
            { "corrsquarestairs", new string[] { "65018", "67016", "67025", "67116", "67125", "67216", "67225" } },
            { "corrarched2waydoor1", new string[] { "61011", "61111", "61211" } },
            { "corrarched2waydoor2", new string[] { "61009", "61010", "61109", "61110", "61209", "61210" } },
            { "corrarched2waychutefloor", new string[] { "61016", "61116", "61216", "61302", "61402", "61502" } },
            { "corrarched2waywindow", new string[] { "61029", "61129", "61229" } },
            { "corrarched2waybeams", new string[] { "60509", "60609", "60709" } },
            { "corrarched2way", new string[] { "61003", "61103", "61203", "61008", "61108", "61208", "61006", "61106", "61206", "61007", "61107", "61207" } },
            { "corrarched3waydoor1", new string[] { "61000", "61100", "61200" } },
            { "corrarched3way", new string[] { "61012", "61112", "61212" } },
            { "corrarched4way", new string[] { "61001", "61101", "61201" } },
            { "corrarchedcornerdoor1", new string[] { "61023", "61024", "61123", "61124", "61223", "61224" } },
            { "corrarchedcornerchutefloor", new string[] { "61300", "61400", "61500" } },
            { "corrarchedcornerchuteceilingfloor", new string[] { "60501", "60601", "60701" } },
            { "corrarchedcornerporticullis", new string[] { "60504", "60604", "60704" } },
            { "corrarchedcornerporticullisdoor1", new string[] { "60505", "60605", "60705" } },
            { "corrarchedcorner", new string[] { "61002", "61102", "61202", "61004", "61104", "61204", "61005", "61105", "61205" } },
            { "corrarcheddeadenddoor1", new string[] { "61014", "61019", "61020", "61114", "61119", "61120", "61219", "61220" } },
            { "corrarcheddeadenddoor2", new string[] { "61021", "61022", "61121", "61122", "61221", "61222" } },
            { "corrarcheddeadendchutefloor", new string[] { "61301", "61401", "61501" } },
            { "corrarcheddeadend", new string[] { "61013", "61113", "61213" } },
            { "corrarchedjunction2way", new string[] { "62303", "62403", "62503" } },
            { "corrarchedjunction3way", new string[] { "62300", "62400", "62500" } },
            { "corrarchedjunction4way", new string[] { "62000", "62100", "62200" } },
            { "corrarchedjunctiondeadend", new string[] { "62304", "62404", "62504" } },
            { "corrarchedjunction2x4way4", new string[] { "62001", "62101", "62201" } },
            { "corrarchedjunction2x6way4", new string[] { "62002", "62102", "62202" } },
            { "corrarchedramp", new string[] { "60500", "60600", "60700", "61015", "61031", "61115", "61131", "61215", "61231" } },
            { "corrarchedslope", new string[] { "61030", "61130", "61230" } },
            { "corrarchedstairsledge", new string[] { "59011", "59013" } },
            { "corrarchedstairs", new string[] { "61017", "61018", "61117", "61118", "61217", "61218" } },
            { "sewers2waydoor1", new string[] { "74535", "74536", "74537" } },
            { "sewers2way", new string[] { "74523", "74524", "74525", "74526", "74527", "74528", "74529", "74533", "74534" } },
            { "sewers3way", new string[] { "74541", "74543" } },
            { "sewers4way", new string[] { "74540", "74542" } },
            { "sewerscorner", new string[] { "74532", "74538", "74539" } },
            { "sewersdeep2waydoor1", new string[] { "74513", "74514", "74515", "74516" } },
            { "sewersdeep2way", new string[] { "74501", "74502", "74503", "74504", "74506", "74507" } },
            { "sewersdeep3way", new string[] { "74520", "74522" } },
            { "sewersdeep4way", new string[] { "74519", "74521" } },
            { "sewersdeepcorner", new string[] { "74510", "74511", "74517", "74518" } },
            { "sewersramp", new string[] { "74530", "74531" } },
            { "sewersrampdeep", new string[] { "74508", "74509" } },
            { "door", new string[] { "55000", "55005", "9003", "9004" } },
            { "secretdoorhexagonallarge", new string[] { "55019", "55025" } },
            { "secretdoorhexagonal", new string[] { "55007", "55018", "55024" } },
            { "secretdoorlarge", new string[] { "55010", "55021", "55027", "55032", "55011", "55022", "55028" } },
            { "secretdoorstandard", new string[] { "55006", "55009", "55012", "55017", "55020", "55023", "55026", "55029", "55030", "55031" } },
            { "chasm2waypath", new string[] { "62026", "74112" } },
            { "chasm2way", new string[] { "62014", "62015" } },
            { "chasmceiling", new string[] { "62017", "62018", "62021", "74111" } },
            { "chasmledge2waydoor", new string[] { "62022", "62122", "62222" } },
            { "chasmledge2waygap", new string[] { "62030", "62130", "62230" } },
            { "chasmledge2way", new string[] { "62023", "62123", "62223" } },
            { "chasmledgedeadend", new string[] { "62024", "62025", "62124", "62125", "62224", "62225" } },
            { "chasmledgewall", new string[] { "62027", "62127", "62227" } },
            { "cave2way", new string[] { "60100", "60101", "60102", "60103", "60107", "60108", "60109" } },
            { "cavecorner", new string[] { "60104", "60105", "60110" } },            
            { "stonebridgemid", new string[] { "61608", "61609" } },            
            { "stonebridge", new string[] { "62012", "62112", "62212" } },            
            { "stonebridgeend", new string[] { "61600", "61601" } },            
            { "chutes", new string[] { "58004", "58012" } },            
            { "circularstaircasebottom", new string[] { "56000", "56300" } },            
            { "circularstaircaselanding", new string[] { "56002", "56303", "56305" } },            
            { "circularstaircasemid", new string[] { "56001", "56301" } },            
            { "circularstaircaseroommid", new string[] { "56006", "56007", "56008", "56009" } },            
            { "largehallcornerdoor1", new string[] { "58048", "58049" } },            
            { "largehallcornerledgegaps", new string[] { "58020", "58023" } },            
            { "largehall", new string[] { "58009", "58050" } },            
            { "largehallcornerfloor", new string[] { "58030", "58028" } },            
            { "largehallcornerwallsceiling", new string[] { "58026", "58025", "58027" } },            
            { "ledgearched2waydoor1", new string[] { "61308", "61309", "61310", "61324", "61408", "61409", "61410", "61424", "61508", "61509", "61510", "61524" } },            
            { "ledgearched2waydoor2", new string[] { "61305", "61306", "61307", "61320", "61405", "61406", "61407", "61420", "61505", "61506", "61507", "61520" } },            
            { "ledgearched2way", new string[] { "61304", "61316", "61317", "61404", "61416", "61417", "61504", "61516", "61517" } },            
            { "ledgearchedcornerinner", new string[] { "61313", "61314", "61315", "61322", "61413", "61414", "61415", "61422", "61513", "61514", "61515", "61522" } },            
            { "ledgearchedcorner", new string[] { "61303", "61311", "61312", "61321", "61323", "61403", "61411", "61412", "61421", "61423", "61503", "61511", "61512", "61521", "61523" } },            
            { "ledgearcheddeadenddoor1", new string[] { "61318", "61319", "61418", "61419", "61518", "61519" } },            
            { "ledgesquare2waydoor1", new string[] { "67304", "67305", "67306", "67318", "67323" } },            
            { "ledgesquare2waydoor2", new string[] { "67307", "67308", "67309", "67324" } },            
            { "ledgesquare2way", new string[] { "67303", "67317", "67319", "67320", "67331" } },            
            { "ledgesquarecornerinner", new string[] { "67314", "67315", "67316", "67322" } },            
            { "ledgesquarecorner", new string[] { "67311", "67312", "67313", "67321" } },            
            { "ledgesquaredeadend", new string[] { "67332", "67333", "67334", "67336", "67337" } },            
            { "platformmid2x2gap", new string[] { "58046", "58047" } },            
            { "platformmid", new string[] { "59014", "59015" } },            
            { "pyramidroom", new string[] { "58013", "58014", "58015", "58016" } },            
            { "rampsmall", new string[] { "59008", "59009" } },            
            { "stairscorner", new string[] { "59004", "59005" } },            
            { "stairs", new string[] { "59000", "59001", "59002", "59003", "59007", "59012", "59017" } },            
            { "switchlevers", new string[] { "61027", "61028" } },
        */

        public static ModelSet[] dungeonPartsRooms = new ModelSet[]
        {
            new ModelSet { Name = "Large Hall", Ids = new string[] { "58009" } },
            new ModelSet { Name = "Large Hall - Corner", Ids = new string[] { "58022" } },
            new ModelSet { Name = "Large Hall - Corner - 1 Door", Ids = new string[] { "58048" } },
            new ModelSet { Name = "Large Hall - Corner - Floor - 1 Door", Ids = new string[] { "58029" } },
            new ModelSet { Name = "Large Hall - Corner - Floor - 4 Doors", Ids = new string[] { "58031" } },
            new ModelSet { Name = "Large Hall - Corner - Floor - 5 Doors", Ids = new string[] { "58053" } },
            new ModelSet { Name = "Lrg Hall - Corner - Flr. - Pltfrm - 1 Chute", Ids = new string[] { "58030" } },
            new ModelSet { Name = "Large Hall - Corner - Ledge Gaps - 1 Door", Ids = new string[] { "58021" } },
            new ModelSet { Name = "Large Hall - Corner - Ledge Gaps", Ids = new string[] { "58020" } },
            new ModelSet { Name = "Large Hall - Corner - Walls/Ceiling", Ids = new string[] { "58024" } },
            new ModelSet { Name = "Lrg Hall - Corner - Walls/Ceiling - Chute", Ids = new string[] { "58026" } },
            new ModelSet { Name = "Large Hall - Diagonal Wall - Platform", Ids = new string[] { "58052" } },
            new ModelSet { Name = "Large Hall - External Corridor", Ids = new string[] { "58054" } },
            new ModelSet { Name = "Ledge - Arched - 2 Way", Ids = new string[] { "61304" } },
            new ModelSet { Name = "Ledge - Arched - 2 Way - 1 Door", Ids = new string[] { "61308" } },
            new ModelSet { Name = "Ledge - Arched - 2 Way - 2 Doors", Ids = new string[] { "61305" } },
            new ModelSet { Name = "Ledge - Arched - Corner", Ids = new string[] { "61303" } },
            new ModelSet { Name = "Ledge - Arched - Corner - Inner", Ids = new string[] { "61313" } },
            new ModelSet { Name = "Ledge - Arched - Dead End - 1 Door", Ids = new string[] { "61318" } },
            new ModelSet { Name = "Ledge - Square - 2 Way", Ids = new string[] { "67303" } },
            new ModelSet { Name = "Ledge - Square - 2 Way - 1 Door", Ids = new string[] { "67304" } },
            new ModelSet { Name = "Ledge - Square - 2 Way - 2 Doors", Ids = new string[] { "67307" } },
            new ModelSet { Name = "Ledge - Square - Corner", Ids = new string[] { "67311" } },
            new ModelSet { Name = "Ledge - Square - Corner - Inner", Ids = new string[] { "67314" } },
            new ModelSet { Name = "Ledge - Square - Dead End", Ids = new string[] { "67332" } },
            new ModelSet { Name = "Ledge - Square - Dead End - 1 Door", Ids = new string[] { "67310" } },
            new ModelSet { Name = "Pyramid Room", Ids = new string[] { "58013" } },
            new ModelSet { Name = "Room 1x1 - 1 Door", Ids = new string[] { "67033", "67133", "67233", "70000", "70100", "70036", "70136" } },
            new ModelSet { Name = "Room 1x1 - 2 Doors", Ids = new string[] { "60502", "60602", "60702" } },
            new ModelSet { Name = "Room 1x2 - 1 Door", Ids = new string[] { "70001" } },
            new ModelSet { Name = "Room 1x3 - 1 Door", Ids = new string[] { "70038" } },
            new ModelSet { Name = "Room 1x4 - 1 Door", Ids = new string[] { "70039" } },
            new ModelSet { Name = "Room 1x5 - 1 Door", Ids = new string[] { "70004" } },
            new ModelSet { Name = "Room 1x6 - 1 Door", Ids = new string[] { "70005" } },
            new ModelSet { Name = "Room 2x1 - 1 Door", Ids = new string[] { "70006" } },
            new ModelSet { Name = "Room 2x2 - 1 Door", Ids = new string[] { "62008", "62108", "62208", "64300", "64301", "64302", "64303", "68000", "68100", "68200", "70007", "70043", "70107", "70143" } },
            new ModelSet { Name = "Room 2x2 - 2 Doors", Ids = new string[] { "58038" } },
            new ModelSet { Name = "Room 2x2 - 3 Doors", Ids = new string[] { "71400" } },
            new ModelSet { Name = "Room 2x3 - 1 Door", Ids = new string[] { "70008" } },
            new ModelSet { Name = "Room 2x2 - 4 Doors", Ids = new string[] { "71403" } },
            new ModelSet { Name = "Room 2x4 - 1 Door", Ids = new string[] { "70009" } },
            new ModelSet { Name = "Room 2x4 - 2 Doors", Ids = new string[] { "71107" } },
            new ModelSet { Name = "Room 2x5 - 1 Door", Ids = new string[] { "70010" } },
            new ModelSet { Name = "Room 2x6 - 1 Door", Ids = new string[] { "70011" } },
            new ModelSet { Name = "Room 3x1 - 1 Door", Ids = new string[] { "70012" } },
            new ModelSet { Name = "Room 3x2 - 1 Door", Ids = new string[] { "70113" } },
            new ModelSet { Name = "Room 3x2 - 3 Doors", Ids = new string[] { "71405" } },
            new ModelSet { Name = "Room 3x2 - 2 Doors", Ids = new string[] { "71104" } },
            new ModelSet { Name = "Room 3x3 - 1 Doors", Ids = new string[] { "70014" } },
            new ModelSet { Name = "Room 3x3 - 2 Doors", Ids = new string[] { "71710" } },
            new ModelSet { Name = "Room 3x3 - 4 Doors", Ids = new string[] { "71404" } },
            new ModelSet { Name = "Room 3x4 - 1 Door", Ids = new string[] { "70015" } },
            new ModelSet { Name = "Room 3x5 - 4 Doors", Ids = new string[] { "70016" } },
            new ModelSet { Name = "Room 3x6 - 1 Door", Ids = new string[] { "70017" } },
            new ModelSet { Name = "Room 4x1 - 1 Door", Ids = new string[] { "70018" } },
            new ModelSet { Name = "Room 4x2 - 1 Door", Ids = new string[] { "62009" } },
            new ModelSet { Name = "Room 4x2 - 2 Doors", Ids = new string[] { "71102" } },
            new ModelSet { Name = "Room 4x3 - 1 Door", Ids = new string[] { "70020" } },
            new ModelSet { Name = "Room 4x4 - 1 Door", Ids = new string[] { "62010" } },
            new ModelSet { Name = "Room 4x4 - 2 Doors", Ids = new string[] { "72000" } },
            new ModelSet { Name = "Room 4x4 - 3 Doors", Ids = new string[] { "71109" } },
            new ModelSet { Name = "Room 4x4 - 8 Doors", Ids = new string[] { "58007" } },
            new ModelSet { Name = "Room 4x5 - 1 Door", Ids = new string[] { "70022" } },
            new ModelSet { Name = "Room 4x6 - 1 Door", Ids = new string[] { "70023" } },
            new ModelSet { Name = "Room 4x6 - 2 Doors", Ids = new string[] { "58039" } },
            new ModelSet { Name = "Room 4x6 - 3 Doors", Ids = new string[] { "72006" } },
            new ModelSet { Name = "Room 4x10 - 1 Door", Ids = new string[] { "62011" } },
            new ModelSet { Name = "Room 5x1 - 1 Door", Ids = new string[] { "70024" } },
            new ModelSet { Name = "Room 5x2 - 1 Door", Ids = new string[] { "70025" } },
            new ModelSet { Name = "Room 5x3 - 1 Door", Ids = new string[] { "70026" } },
            new ModelSet { Name = "Room 5x4 - 1 Door", Ids = new string[] { "70027" } },
            new ModelSet { Name = "Room 5x5 - 1 Door", Ids = new string[] { "70028" } },
            new ModelSet { Name = "Room 5x6 - 1 Door", Ids = new string[] { "70029" } },
            new ModelSet { Name = "Room 6x1 - 1 Door", Ids = new string[] { "70030" } },
            new ModelSet { Name = "Room 6x2 - 1 Door", Ids = new string[] { "70031" } },
            new ModelSet { Name = "Room 6x3 - 1 Door", Ids = new string[] { "70032" } },
            new ModelSet { Name = "Room 6x4 - 1 Door", Ids = new string[] { "70033" } },
            new ModelSet { Name = "Room 6x5 - 1 Door", Ids = new string[] { "70034" } },
            new ModelSet { Name = "Room 6x6 - 1 Door", Ids = new string[] { "70035" } },
            new ModelSet { Name = "Room 6x4 - 1 Door ", Ids = new string[] { "72012" } },
            new ModelSet { Name = "Room 6x6 - 1 Door", Ids = new string[] { "72009" } },
            new ModelSet { Name = "Room 6x6 - 2 Doors", Ids = new string[] { "58037" } },
            new ModelSet { Name = "Room 6x6 - 3 Doors", Ids = new string[] { "72005" } },
            new ModelSet { Name = "Room 2x4 - 1 Door Arched", Ids = new string[] { "62306" } },
            new ModelSet { Name = "Room 3x5 - 1 Door Arched", Ids = new string[] { "64304" } },
            new ModelSet { Name = "Room 4x2 - 1 Door Arched", Ids = new string[] { "62305" } },
            new ModelSet { Name = "Room 4x4 - 1 Door Arched", Ids = new string[] { "62307" } },
            new ModelSet { Name = "Room 4x4 - 2 Doors Arched", Ids = new string[] { "62308" } },
            new ModelSet { Name = "Room 4x4 - Ledge Gap - 1 Door Arched", Ids = new string[] { "62029" } },
            new ModelSet { Name = "Room 6x4 - 3 Doors Arched", Ids = new string[] { "62005" } },
            new ModelSet { Name = "Room 6x6 - 2 Doors Arched", Ids = new string[] { "62003" } },
            new ModelSet { Name = "Room 6x6 - 4 Doors Arched", Ids = new string[] { "62301" } },
            new ModelSet { Name = "Room 10x10 - 2 Doors Arched", Ids = new string[] { "62004" } },
            new ModelSet { Name = "Room 2x3 - Angled - 1 Door", Ids = new string[] { "70506" } },
            new ModelSet { Name = "Room 3x2 - Angled - 1 Door", Ids = new string[] { "71408" } },
            new ModelSet { Name = "Room 3x3 - Angled - 1 Door", Ids = new string[] { "70608" } },
            new ModelSet { Name = "Room 4x3 - Angled - 1 Door", Ids = new string[] { "70502" } },
            new ModelSet { Name = "Room 6x10 - Angled - 1 Door", Ids = new string[] { "58006" } },
            new ModelSet { Name = "Room 6x6 - Bridge - 4 Doors Arched", Ids = new string[] { "62302" } },
            new ModelSet { Name = "Room 1x1 - Chute Ceiling - 1 Door", Ids = new string[] { "67326", "67426", "67526" } },
            new ModelSet { Name = "Room 3x2 - Chute Ceiling - 2 Doors", Ids = new string[] { "72013" } },
            new ModelSet { Name = "Room 3x2 - Chute Floor - 2 Doors", Ids = new string[] { "72015" } },
            new ModelSet { Name = "Room 3x2 - Chute Floor/Celing - 1 Door", Ids = new string[] { "72014" } },
            new ModelSet { Name = "Room 3x3 - Chute Floor - 3 Doors", Ids = new string[] { "72017" } },
            new ModelSet { Name = "Room 4x3 - Chute Ceiling - 2 Doors", Ids = new string[] { "72018" } },
            new ModelSet { Name = "Room 1x1 - Closet - 2 Doors", Ids = new string[] { "71409" } },
            new ModelSet { Name = "Room 2x2 - Closet - 1 Door", Ids = new string[] { "71407" } },
            new ModelSet { Name = "Room 3x2 - Closet - 1 Door", Ids = new string[] { "70504" } },
            new ModelSet { Name = "Room 3x2 - Closet - 2 Doors", Ids = new string[] { "71103" } },
            new ModelSet { Name = "Room 3x3 - Closet - 1 Door", Ids = new string[] { "70503" } },
            new ModelSet { Name = "Room 3x3 - Closet - 2 Doors", Ids = new string[] { "71709" } },
            new ModelSet { Name = "Room 1x1 - Closets - 3 Doors", Ids = new string[] { "71702" } },
            new ModelSet { Name = "Room 3x3 - Closets - 1 Door", Ids = new string[] { "70505" } },
            new ModelSet { Name = "Room 2x1 - J Shape - 1 Door", Ids = new string[] { "70501" } },
            new ModelSet { Name = "Room 3x3 - M Shape - 1 Door", Ids = new string[] { "71012" } },
            new ModelSet { Name = "Room 3x2 - B Shape - 1 Door", Ids = new string[] { "70509" } },
            new ModelSet { Name = "Room 3x2 - P Shape - 3 Doors", Ids = new string[] { "71703" } },
            new ModelSet { Name = "Room 3x3 - L Shape - 3 Doors", Ids = new string[] { "71704" } },
            new ModelSet { Name = "Room 3x3 - M Shape - 1 Door", Ids = new string[] { "70800" } },
            new ModelSet { Name = "Room 3x3 - M Shape - 2 Doors Arched", Ids = new string[] { "62007" } },
            new ModelSet { Name = "Room 3x3 - M Shape - 3 Doors", Ids = new string[] { "71706" } },
            new ModelSet { Name = "Room 4x2 - B Shape - 1 Door", Ids = new string[] { "70500" } },
            new ModelSet { Name = "Room 4x5 - Splitted - 1 Door", Ids = new string[] { "70901" } },
            new ModelSet { Name = "Room 4x5 - Splitted - 1 Door", Ids = new string[] { "71010" } },
            new ModelSet { Name = "Room 10x8 - 2 Stories - Ledge - 4 Doors", Ids = new string[] { "58010" } },
            new ModelSet { Name = "Room 10x8 - 2 Stories - Ramp - 2 Doors", Ids = new string[] { "58011" } },
            new ModelSet { Name = "Room 2x3 - Vaulted - 1 Door", Ids = new string[] { "70802" } },
            new ModelSet { Name = "Room 2x4 - Vaulted - 1 Door", Ids = new string[] { "71011" } },
            new ModelSet { Name = "Room 2x4 - Vaulted - 2 Doors", Ids = new string[] { "70807" } },
            new ModelSet { Name = "Room 3x3 - Vaulted - 1 Door", Ids = new string[] { "70803" } },
            new ModelSet { Name = "Room 3x4 - Vaulted - 1 Door", Ids = new string[] { "70804" } },
            new ModelSet { Name = "Room 3x5 - Vaulted - 1 Door", Ids = new string[] { "70805" } },
            new ModelSet { Name = "Room 3x5 - Vaulted - 2 Door", Ids = new string[] { "70808" } },
            new ModelSet { Name = "Room 4x5 - Vaulted - 2 Doors", Ids = new string[] { "70809" } },
        };

        public static ModelSet[] dungeonPartsCorridors = new ModelSet[]
        {
            new ModelSet { Name = "Arched - 2 Way", Ids = new string[] { "61003" } },
            new ModelSet { Name = "Arched - 2 Way - 1 Door", Ids = new string[] { "61011" } },
            new ModelSet { Name = "Arched - 2 Way - 2 Doors", Ids = new string[] { "61009" } },
            new ModelSet { Name = "Arched - 2 Way - Chute - Floor", Ids = new string[] { "61016" } },
            new ModelSet { Name = "Arched - 2 Way - Window", Ids = new string[] { "61029" } },
            new ModelSet { Name = "Arched - 2 Way - Wooden Beams", Ids = new string[] { "60509" } },
            new ModelSet { Name = "Arched - 3 Way", Ids = new string[] { "61012" } },
            new ModelSet { Name = "Arched - 3 Way - 1 Door", Ids = new string[] { "61000" } },
            new ModelSet { Name = "Arched - 4 Way", Ids = new string[] { "61001" } },
            new ModelSet { Name = "Arched - Corner - 1 Door", Ids = new string[] { "61023" } },
            new ModelSet { Name = "Arched - Corner - Chute - Floor", Ids = new string[] { "61300" } },
            new ModelSet { Name = "Arched - Corner - Chute - Ceiling/Floor", Ids = new string[] { "60501" } },
            new ModelSet { Name = "Arched - Corner - Porticullis", Ids = new string[] { "60504" } },
            new ModelSet { Name = "Arched - Corner - Porticullis - 1 Door", Ids = new string[] { "60505" } },
            new ModelSet { Name = "Arched - Corner", Ids = new string[] { "61002" } },
            new ModelSet { Name = "Arched - Dead End", Ids = new string[] { "61013" } },
            new ModelSet { Name = "Arched - Dead End - 1 Door", Ids = new string[] { "61014" } },
            new ModelSet { Name = "Arched - Dead End - 2 Doors", Ids = new string[] { "61021" } },
            new ModelSet { Name = "Arched - Dead End - Chute - Floor", Ids = new string[] { "61301" } },
            new ModelSet { Name = "Arched - Junction 2x2 - 2 Way", Ids = new string[] { "62303" } },
            new ModelSet { Name = "Arched - Junction 2x2 - 3 Way", Ids = new string[] { "62300" } },
            new ModelSet { Name = "Arched - Junction 2x2 - 4 Way", Ids = new string[] { "62000" } },
            new ModelSet { Name = "Arched - Junction 2x2 - Dead End", Ids = new string[] { "62304" } },
            new ModelSet { Name = "Arched - Junction 2x4 - 4 Way", Ids = new string[] { "62001" } },
            new ModelSet { Name = "Arched - Junction 2x6 - 4 Way", Ids = new string[] { "62002" } },
            new ModelSet { Name = "Arched - Ramp", Ids = new string[] { "60500" } },
            new ModelSet { Name = "Arched - Slope", Ids = new string[] { "61030" } },
            new ModelSet { Name = "Arched - Stairs", Ids = new string[] { "61017" } },
            new ModelSet { Name = "Arched - Stairs - Ledge", Ids = new string[] { "59011" } },
            new ModelSet { Name = "Hexagon - 2 Way", Ids = new string[] { "63000" } },
            new ModelSet { Name = "Hexagon - 2 Way - 1 Door", Ids = new string[] { "63008" } },
            new ModelSet { Name = "Hexagon - 2 Way - 2 Doors", Ids = new string[] { "63012" } },
            new ModelSet { Name = "Hexagon - 2 Way - 3 Doors", Ids = new string[] { "63014" } },
            new ModelSet { Name = "Hexagon - 2 Way - 4 Doors", Ids = new string[] { "63025" } },
            new ModelSet { Name = "Hexagon - 2 Way - Beams", Ids = new string[] { "63022" } },
            new ModelSet { Name = "Hexagon - 2 Way - Diagonal", Ids = new string[] { "63035" } },
            new ModelSet { Name = "Hexagon - 2 Way - Diagonal - 1 Door", Ids = new string[] { "63056" } },
            new ModelSet { Name = "Hexagon - 2 Way - 2 Corrs. - Niches", Ids = new string[] { "63050" } },
            new ModelSet { Name = "Hexagon - 2 Way - Niches", Ids = new string[] { "63049" } },
            new ModelSet { Name = "Hexagon - 2 Way - S", Ids = new string[] { "63039" } },
            new ModelSet { Name = "Hexagon - 2 Way - Window", Ids = new string[] { "63053" } },
            new ModelSet { Name = "Hexagon - 3 Way", Ids = new string[] { "63028" } },
            new ModelSet { Name = "Hexagon - 3 Way - 1 Door", Ids = new string[] { "63029" } },
            new ModelSet { Name = "Hexagon - 3 Way - Diagonal", Ids = new string[] { "63032" } },
            new ModelSet { Name = "Hexagon - 4 Way", Ids = new string[] { "63042" } },
            new ModelSet { Name = "Hexagon - 4 Way - Diagonal", Ids = new string[] { "63031" } },
            new ModelSet { Name = "Hexagon - Corner", Ids = new string[] { "63033" } },
            new ModelSet { Name = "Hexagon - Corner - 1 Door", Ids = new string[] { "63051" } },
            new ModelSet { Name = "Hexagon - Dead End", Ids = new string[] { "63003" } },
            new ModelSet { Name = "Hexagon - Dead End - 1 Door", Ids = new string[] { "63004" } },
            new ModelSet { Name = "Hexagon - Dead End - 2 Doors", Ids = new string[] { "63006" } },
            new ModelSet { Name = "Hexagon - Ramp", Ids = new string[] { "63040" } },
            new ModelSet { Name = "Hexagon - Ramp - Diagonal", Ids = new string[] { "63057" } },
            new ModelSet { Name = "Hexagon - Ramp/Corridor", Ids = new string[] { "63043" } },
            new ModelSet { Name = "Hexagon - Stairs", Ids = new string[] { "63026" } },
            new ModelSet { Name = "Hexagon - Transition - Cave", Ids = new string[] { "63047" } },
            new ModelSet { Name = "Hexagon - Transition - Narrow", Ids = new string[] { "63045" } },
            new ModelSet { Name = "Hexagon - Transition - Square", Ids = new string[] { "63048" } },
            new ModelSet { Name = "Hexagon - Transition - Arched", Ids = new string[] { "63046" } },
            new ModelSet { Name = "Narrow - 2 Way", Ids = new string[] { "69010" } },
            new ModelSet { Name = "Narrow - 2 Way - 1 Door", Ids = new string[] { "69008" } },
            new ModelSet { Name = "Narrow - 2 Way - 2 Doors", Ids = new string[] { "69009" } },
            new ModelSet { Name = "Narrow - 3 Way", Ids = new string[] { "69002" } },
            new ModelSet { Name = "Narrow - 3 Way - 1 Door", Ids = new string[] { "69000" } },
            new ModelSet { Name = "Narrow - 4 Way", Ids = new string[] { "69003" } },
            new ModelSet { Name = "Narrow - Corner", Ids = new string[] { "69005" } },
            new ModelSet { Name = "Narrow - Dead End", Ids = new string[] { "69011" } },
            new ModelSet { Name = "Narrow - Dead End - 1 Door", Ids = new string[] { "69012" } },
            new ModelSet { Name = "Narrow - Ramped Stairwell", Ids = new string[] { "59016" } },
            new ModelSet { Name = "Narrow - Stairs", Ids = new string[] { "69001" } },
            new ModelSet { Name = "Sewers - 2 Way", Ids = new string[] { "74523" } },
            new ModelSet { Name = "Sewers - 2 Way - Door", Ids = new string[] { "74535" } },
            new ModelSet { Name = "Sewers - 3 Way", Ids = new string[] { "74541" } },
            new ModelSet { Name = "Sewers - 4 Way", Ids = new string[] { "74540" } },
            new ModelSet { Name = "Sewers - Corner", Ids = new string[] { "74532" } },
            new ModelSet { Name = "Sewers - Dead End", Ids = new string[] { "74527" } },
            new ModelSet { Name = "Sewers - Ramp", Ids = new string[] { "74530" } },
            new ModelSet { Name = "Sewers - Transition - Deep", Ids = new string[] { "74500" } },
            new ModelSet { Name = "Sewers - Deep - 2 Way", Ids = new string[] { "74501" } },
            new ModelSet { Name = "Sewers - Deep - 2 Way - 1 Door", Ids = new string[] { "74513" } },
            new ModelSet { Name = "Sewers - Deep - 3 Way", Ids = new string[] { "74520" } },
            new ModelSet { Name = "Sewers - Deep - 4 Way", Ids = new string[] { "74519" } },
            new ModelSet { Name = "Sewers - Deep - Corner", Ids = new string[] { "74510" } },
            new ModelSet { Name = "Sewers - Deep - Dead End", Ids = new string[] { "74505" } },
            new ModelSet { Name = "Sewers - Deep - Ramp", Ids = new string[] { "74508" } },
            new ModelSet { Name = "Square - 2 Way", Ids = new string[] { "66000" } },
            new ModelSet { Name = "Square - 2 Way - 1 Door", Ids = new string[] { "66007" } },
            new ModelSet { Name = "Square - 2 Way - 2 Doors", Ids = new string[] { "66021" } },
            new ModelSet { Name = "Square - 3 Way - 1 Door", Ids = new string[] { "67032" } },
            new ModelSet { Name = "Square - 2 Way - Chute - Ceiling", Ids = new string[] { "67325" } },
            new ModelSet { Name = "Square - 2 Way - Chute - Floor", Ids = new string[] { "67302" } },
            new ModelSet { Name = "Square - 2 Way - Chute - Ceiling/Floor", Ids = new string[] { "67335" } },
            new ModelSet { Name = "Square - 3 Way", Ids = new string[] { "67005" } },
            new ModelSet { Name = "Square - 4 Way", Ids = new string[] { "67004" } },
            new ModelSet { Name = "Square - Corner", Ids = new string[] { "66006" } },
            new ModelSet { Name = "Square - Corner - 1 Door", Ids = new string[] { "67123" } },
            new ModelSet { Name = "Square - Corner - Chute - Ceiling", Ids = new string[] { "67328" } },
            new ModelSet { Name = "Square - Corner - Chute - Floor", Ids = new string[] { "67300" } },
            new ModelSet { Name = "Square - Corner - Chute - Ceiling/Floor", Ids = new string[] { "67330" } },
            new ModelSet { Name = "Square - Corner - Porticullis Slot", Ids = new string[] { "67031" } },
            new ModelSet { Name = "Square - Dead End", Ids = new string[] { "66023" } },
            new ModelSet { Name = "Square - Dead End - 1 Door", Ids = new string[] { "66003" } },
            new ModelSet { Name = "Square - Dead End - 2 Doors", Ids = new string[] { "66008" } },
            new ModelSet { Name = "Square - Dead End - 3 Doors", Ids = new string[] { "66022" } },
            new ModelSet { Name = "Square - Dead End - Chute - Ceiling", Ids = new string[] { "67327" } },
            new ModelSet { Name = "Square - Dead End - Chute - Floor", Ids = new string[] { "67301" } },
            new ModelSet { Name = "Square - Dead End - Chute - Ceiling/Floor", Ids = new string[] { "67329" } },
            new ModelSet { Name = "Square - Junction 2x2 - 2 Way", Ids = new string[] { "68003" } },
            new ModelSet { Name = "Square - Junction 2x2 - 3 Way", Ids = new string[] { "68004" } },
            new ModelSet { Name = "Square - Junction 2x2 - 4 Way", Ids = new string[] { "68005" } },
            new ModelSet { Name = "Square - Junction 2x2 - Dead End", Ids = new string[] { "68002" } },
            new ModelSet { Name = "Square - Ramp", Ids = new string[] { "65017" } },
            new ModelSet { Name = "Square - Ramp - 1 Door", Ids = new string[] { "67030" } },
            new ModelSet { Name = "Square - Slope", Ids = new string[] { "67027" } },
            new ModelSet { Name = "Square - Stairs", Ids = new string[] { "65018" } },
            new ModelSet { Name = "Square - Stairs - 2 Doors", Ids = new string[] { "58008" } },
        };

        public static ModelSet[] dungeonPartsMisc = new ModelSet[]
        {
            new ModelSet { Name = "Bridge - Rope", Ids = new string[] { "62031" } },
            new ModelSet { Name = "Bridge - Stone", Ids = new string[] { "62012" } },
            new ModelSet { Name = "Bridge - Stone - Mid", Ids = new string[] { "61608" } },
            new ModelSet { Name = "Bridge - Stone - End", Ids = new string[] { "61600" } },
            new ModelSet { Name = "Ceiling 2x2", Ids = new string[] { "64000" } },
            new ModelSet { Name = "Ceiling 6x6", Ids = new string[] { "58019" } },
            new ModelSet { Name = "Chute - Ceiling - Hole", Ids = new string[] { "58003" } },
            new ModelSet { Name = "Chute - Dirt Floor", Ids = new string[] { "58002" } },
            new ModelSet { Name = "Chutes", Ids = new string[] { "58004" } },
            new ModelSet { Name = "Circular Staircase - Bottom", Ids = new string[] { "56000" } },
            new ModelSet { Name = "Circular Staircase - Landing", Ids = new string[] { "56002" } },
            new ModelSet { Name = "Circular Staircase - Mid", Ids = new string[] { "56001" } },
            new ModelSet { Name = "Circular Staircase - Room - Ceiling", Ids = new string[] { "63700" } },
            new ModelSet { Name = "Circular Staircase - Room - Mid", Ids = new string[] { "56006" } },
            new ModelSet { Name = "Circular Staircase - Room - Top", Ids = new string[] { "56010" } },
            new ModelSet { Name = "Doorway", Ids = new string[] { "58005" } },
            new ModelSet { Name = "Floor 6x3", Ids = new string[] { "64001" } },
            new ModelSet { Name = "Floor 6x6", Ids = new string[] { "64003" } },
            new ModelSet { Name = "Floor 7x3", Ids = new string[] { "64004" } },
            new ModelSet { Name = "Floor 7x6", Ids = new string[] { "64005" } },
            new ModelSet { Name = "Floor/Ceiling 2x2 - Chute 1x0.5", Ids = new string[] { "58001" } },
            new ModelSet { Name = "Floor/Ceiling 2x2 - Chute 1x1", Ids = new string[] { "58000" } },
            new ModelSet { Name = "Pit - Floor 4x4 - 4 Chutes", Ids = new string[] { "58017" } },
            new ModelSet { Name = "Pit - Half-Room - 1 Door", Ids = new string[] { "58018" } },            
            new ModelSet { Name = "Platform - 1x1", Ids = new string[] { "54000" } },
            new ModelSet { Name = "Platform - Block Stairs", Ids = new string[] { "59010" } },
            new ModelSet { Name = "Platform - Bridge - Rectangle", Ids = new string[] { "58041" } },
            new ModelSet { Name = "Platform - Bridge - Spike", Ids = new string[] { "58040" } },
            new ModelSet { Name = "Platform - Chipped Beam", Ids = new string[] { "58042" } },
            new ModelSet { Name = "Platform - Corner Beam", Ids = new string[] { "58055" } },
            new ModelSet { Name = "Platform - Marble", Ids = new string[] { "62322" } },
            new ModelSet { Name = "Platform - Mid - 1x1", Ids = new string[] { "58034" } },
            new ModelSet { Name = "Platform - Mid - 2x2", Ids = new string[] { "58035" } },
            new ModelSet { Name = "Platform - Mid - 2x2 - 1 Door", Ids = new string[] { "58036" } },
            new ModelSet { Name = "Platform - Mid - 2x2 - Gap", Ids = new string[] { "58046" } },
            new ModelSet { Name = "Platform - Mid", Ids = new string[] { "59014" } },
            new ModelSet { Name = "Platform - Side", Ids = new string[] { "58043" } },
            new ModelSet { Name = "Platform - Spike", Ids = new string[] { "58044" } },
            new ModelSet { Name = "Platform - Top - 2x2 - 1 Door", Ids = new string[] { "58033" } },
            new ModelSet { Name = "Platform 1x1x0.5", Ids = new string[] { "72019" } },
            new ModelSet { Name = "Platform 1x3 - With 1x1 Floor", Ids = new string[] { "58045" } },
            new ModelSet { Name = "Platform 3x3x1 - Bridges", Ids = new string[] { "58032" } },           
            new ModelSet { Name = "Portcullis", Ids = new string[] { "60506" } },
            new ModelSet { Name = "Ramp - Big", Ids = new string[] { "59006" } },
            new ModelSet { Name = "Ramp - Small", Ids = new string[] { "59008" } },
            new ModelSet { Name = "Secret Wall Block", Ids = new string[] { "61025" } },
            new ModelSet { Name = "Stairs", Ids = new string[] { "59000" } },
            new ModelSet { Name = "Stairs - Corner", Ids = new string[] { "59004" } },
            new ModelSet { Name = "Switch - Lever Base", Ids = new string[] { "61026" } },
            new ModelSet { Name = "Switch - Levers", Ids = new string[] { "61027" } },         
            new ModelSet { Name = "Switch - Wheel", Ids = new string[] { "61032" } },
            new ModelSet { Name = "Trapdoor", Ids = new string[] { "54001" } },
        };

        public static ModelSet[] dungeonPartsCaves = new ModelSet[]
        {
            new ModelSet { Name = "Cave - 2 Way", Ids = new string[] { "60100" } },
            new ModelSet { Name = "Cave - 3 Way", Ids = new string[] { "60106" } },
            new ModelSet { Name = "Cave - 4 Way", Ids = new string[] { "60111" } },
            new ModelSet { Name = "Cave - Corner", Ids = new string[] { "60104" } },
            new ModelSet { Name = "Cave - Dead End", Ids = new string[] { "60113" } },
            new ModelSet { Name = "Cave - Ramp", Ids = new string[] { "60112" } },
            new ModelSet { Name = "Cave - Room - 1 Way", Ids = new string[] { "60200" } },
            new ModelSet { Name = "Cave - Room - 2 Way", Ids = new string[] { "60201" } },
            new ModelSet { Name = "Cave - Room - 2 Way - No Ceiling", Ids = new string[] { "60202" } },
            new ModelSet { Name = "Cave - Transition - Hexagon", Ids = new string[] { "63047" } },
            new ModelSet { Name = "Chasm - 2 Way", Ids = new string[] { "62014" } },
            new ModelSet { Name = "Chasm - 2 Way - Bridge Gap", Ids = new string[] { "62013" } },
            new ModelSet { Name = "Chasm - 2 Way - Pathway Up", Ids = new string[] { "62026" } },
            new ModelSet { Name = "Chasm - 3 Way", Ids = new string[] { "60203" } },
            new ModelSet { Name = "Chasm - Ceiling", Ids = new string[] { "62017" } },
            new ModelSet { Name = "Chasm - Corner", Ids = new string[] { "62019" } },
            new ModelSet { Name = "Chasm - Dead End", Ids = new string[] { "62016" } },
            new ModelSet { Name = "Chasm - Dead End - 1 Way", Ids = new string[] { "62020" } },
            new ModelSet { Name = "Chasm - Ledge - 2 Way", Ids = new string[] { "62023" } },
            new ModelSet { Name = "Chasm - Ledge - 2 Way - 1 Arched Door", Ids = new string[] { "62022" } },
            new ModelSet { Name = "Chasm - Ledge - 2 Way - Gap", Ids = new string[] { "62030" } },
            new ModelSet { Name = "Chasm - Ledge - Dead End", Ids = new string[] { "62024" } },
            new ModelSet { Name = "Chasm - Ledge - Wall", Ids = new string[] { "62027" } },
            new ModelSet { Name = "Chasm - Ramp", Ids = new string[] { "74110" } },
        };

        public static ModelSet[] dungeonPartsDoors = new ModelSet[]
        {
            new ModelSet { Name = "Door", Ids = new string[] { "55000" } },
            new ModelSet { Name = "Entrance/Exit - Standalone", Ids = new string[] { "70300" } },
            new ModelSet { Name = "Entrance/Exit - Crypt Entry Room", Ids = new string[] { "58051" } },
            new ModelSet { Name = "Red Brick Door", Ids = new string[] { "72100" } },
            new ModelSet { Name = "Secret Door - Hexagonal", Ids = new string[] { "55007" } },
            new ModelSet { Name = "Secret Door - Hexagonal - Large", Ids = new string[] { "55019" } },
            new ModelSet { Name = "Secret Door - Large", Ids = new string[] { "55011" } },
            new ModelSet { Name = "Secret Door - Narrow", Ids = new string[] { "55008" } },
            new ModelSet { Name = "Secret Door - Standard", Ids = new string[] { "55006" } },
        };

        public static ModelSet[] interiorParts = houseParts
            .Concat(dungeonPartsRooms)
            .Concat(dungeonPartsCorridors)
            .Concat(dungeonPartsMisc)
            .Concat(dungeonPartsCaves)
            .Concat(dungeonPartsDoors)
            .OrderBy(modelSet => modelSet.Name)
            .ToArray();

        /// <summary>
        /// Returns list of locations based on file path. Return null if not found or wrong file
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static IEnumerable<LocationInstance> LoadLocationInstance(string path)
        {
            if (!File.Exists(path))
            {
                Debug.LogWarning($"Location instance file could not be found at '{path}'");
                return null;
            }


            if (path.EndsWith(".csv"))
            {
                TextReader reader = File.OpenText(path);

                return LoadLocationInstanceCsv(reader, $"file={path}");
            }
            else
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(path);

                return LoadLocationInstance(xmlDoc, $"file={path}");
            }
        }

        public static IEnumerable<LocationInstance> LoadLocationInstance(Mod mod, string assetName)
        {
            TextAsset asset = mod.GetAsset<TextAsset>(assetName);
            if (asset == null)
            {
                Debug.LogWarning($"Asset '{assetName}' could not be found in mod '{mod.Title}'");
                return Enumerable.Empty<LocationInstance>();
            }

            TextReader reader = new StringReader(asset.text);
            if (assetName.EndsWith(".csv"))
            {
                return LoadLocationInstanceCsv(reader, $"mod={mod.Title}, asset={assetName}");
            }
            else
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(reader);

                return LoadLocationInstance(xmlDoc, $"mod={mod.Title}, asset={assetName}");
            }
        }

        public static IEnumerable<LocationInstance> LoadLocationInstance(XmlDocument xmlDoc, string contextString)
        {
            if (xmlDoc.SelectSingleNode("//locations") == null)
            {
                Debug.LogWarning("Wrong file format");
                yield return null;
            }

            CultureInfo cultureInfo = new CultureInfo("en-US");

            XmlNodeList instanceNodes = xmlDoc.GetElementsByTagName("locationInstance");
            for (int i = 0; i < instanceNodes.Count; i++)
            {
                XmlNode node = instanceNodes[i];
                if (node["prefab"].InnerXml == "")
                {
                    Debug.LogWarning("Locationinstance must have a assigned prefab to be valid");
                    continue;
                }

                LocationInstance tmpInst = new LocationInstance();
                try
                {
                    tmpInst.name = node["name"].InnerXml;
                    tmpInst.locationID = ulong.Parse(node["locationID"].InnerXml);
                    tmpInst.type = int.Parse(node["type"].InnerXml);
                    tmpInst.prefab = node["prefab"].InnerXml;
                    tmpInst.worldX = int.Parse(node["worldX"].InnerXml);
                    tmpInst.worldY = int.Parse(node["worldY"].InnerXml);
                    tmpInst.terrainX = int.Parse(node["terrainX"].InnerXml);
                    tmpInst.terrainY = int.Parse(node["terrainY"].InnerXml);

                    XmlNode child = node["rotW"];
                    if (child != null)
                    {
                        tmpInst.rot.w = float.Parse(child.InnerXml, cultureInfo);
                        tmpInst.rot.x = float.Parse(node["rotX"].InnerXml, cultureInfo);
                        tmpInst.rot.y = float.Parse(node["rotY"].InnerXml, cultureInfo);
                        tmpInst.rot.z = float.Parse(node["rotZ"].InnerXml, cultureInfo);
                    }
                    else
                    {
                        child = node["rotYAxis"];
                        if (child != null)
                        {
                            float yRot = float.Parse(child.InnerXml, cultureInfo);
                            tmpInst.rot.eulerAngles = new Vector3(0, yRot, 0);
                        }
                    }

                    child = node["heightOffset"];
                    if (child != null)
                    {
                        tmpInst.heightOffset = float.Parse(child.InnerXml, cultureInfo);
                    }
                }
                catch (Exception e)
                {
                    Debug.LogWarning($"Error while parsing location instance: {e.Message}");
                    continue;
                }

                if (tmpInst.terrainX < 0 || tmpInst.terrainY < 0 || tmpInst.terrainX >= 128 || tmpInst.terrainY >= 128)
                {
                    Debug.LogWarning($"Invalid location instance '{tmpInst.name}' ({contextString}): terrainX and terrainY must be higher than 0 and lower than 128");
                    continue;
                }

                yield return tmpInst;
            }
        }

        public static IEnumerable<LocationInstance> LoadLocationInstanceCsv(TextReader csvStream, string contextString)
        {
            string header = csvStream.ReadLine();
            string[] fields = header.Split(';', ',');

            bool GetIndex(string fieldName, out int index)
            {
                index = -1;
                for (int i = 0; i < fields.Length; ++i)
                {
                    if (fields[i].Equals(fieldName, StringComparison.InvariantCultureIgnoreCase))
                    {
                        index = i;
                        break;
                    }
                }
                if (index == -1)
                {
                    Debug.LogError($"Location instance file failed ({contextString}): could not find field '{fieldName}' in header");
                    return false;
                }
                return true;
            }

            int? GetIndexOpt(string fieldName)
            {
                int index = -1;
                for (int i = 0; i < fields.Length; ++i)
                {
                    if (fields[i].Equals(fieldName, StringComparison.InvariantCultureIgnoreCase))
                    {
                        index = i;
                        break;
                    }
                }
                if (index == -1)
                {
                    return null;
                }
                return index;
            }

            if (!GetIndex("name", out int nameIndex)) yield break;
            if (!GetIndex("type", out int typeIndex)) yield break;
            if (!GetIndex("prefab", out int prefabIndex)) yield break;
            if (!GetIndex("worldX", out int worldXIndex)) yield break;
            if (!GetIndex("worldY", out int worldYIndex)) yield break;
            if (!GetIndex("terrainX", out int terrainXIndex)) yield break;
            if (!GetIndex("terrainY", out int terrainYIndex)) yield break;
            if (!GetIndex("locationID", out int locationIDIndex)) yield break;
            int? rotWIndex = GetIndexOpt("rotW");
            int? rotXIndex = GetIndexOpt("rotX");
            int? rotYIndex = GetIndexOpt("rotY");
            int? rotZIndex = GetIndexOpt("rotZ");
            int? rotXAxisIndex = GetIndexOpt("rotXAxis");
            int? rotYAxisIndex = GetIndexOpt("rotYAxis");
            int? rotZAxisIndex = GetIndexOpt("rotZAxis");
            int? heightOffsetIndex = GetIndexOpt("heightOffset");

            CultureInfo cultureInfo = new CultureInfo("en-US");
            int lineNumber = 1;
            while (csvStream.Peek() >= 0)
            {
                ++lineNumber;
                string line = csvStream.ReadLine();
                string[] tokens = line.Split(';', ',');

                tokens = tokens.Select(token => token.Trim('"')).ToArray();

                LocationInstance tmpInst = new LocationInstance();

                try
                {
                    tmpInst.name = tokens[nameIndex];
                    tmpInst.type = int.Parse(tokens[typeIndex]);
                    tmpInst.prefab = tokens[prefabIndex];
                    tmpInst.worldX = int.Parse(tokens[worldXIndex]);
                    tmpInst.worldY = int.Parse(tokens[worldYIndex]);
                    tmpInst.terrainX = int.Parse(tokens[terrainXIndex]);
                    tmpInst.terrainY = int.Parse(tokens[terrainYIndex]);
                    tmpInst.locationID = ulong.Parse(tokens[locationIDIndex]);

                    if (rotWIndex.HasValue) tmpInst.rot.w = float.Parse(tokens[rotWIndex.Value], cultureInfo);
                    if (rotXIndex.HasValue) tmpInst.rot.x = float.Parse(tokens[rotXIndex.Value], cultureInfo);
                    if (rotYIndex.HasValue) tmpInst.rot.y = float.Parse(tokens[rotYIndex.Value], cultureInfo);
                    if (rotZIndex.HasValue) tmpInst.rot.z = float.Parse(tokens[rotZIndex.Value], cultureInfo);
                    if (rotXAxisIndex.HasValue) tmpInst.rot.eulerAngles = new Vector3(float.Parse(tokens[rotXAxisIndex.Value], cultureInfo), tmpInst.rot.eulerAngles.y, tmpInst.rot.eulerAngles.z);
                    if (rotYAxisIndex.HasValue) tmpInst.rot.eulerAngles = new Vector3(tmpInst.rot.eulerAngles.x, float.Parse(tokens[rotYAxisIndex.Value], cultureInfo), tmpInst.rot.eulerAngles.z);
                    if (rotZAxisIndex.HasValue) tmpInst.rot.eulerAngles = new Vector3(tmpInst.rot.eulerAngles.x, tmpInst.rot.eulerAngles.y, float.Parse(tokens[rotZAxisIndex.Value], cultureInfo));
                    if (heightOffsetIndex.HasValue) tmpInst.heightOffset = float.Parse(tokens[heightOffsetIndex.Value], cultureInfo);
                }
                catch (Exception e)
                {
                    Debug.LogError($"Failed to parse a location instance ({contextString}, line {lineNumber}): {e.Message}");
                    continue;
                }

                yield return tmpInst;

            }
        }

        public static LocationInstance LoadSingleLocationInstanceCsv(string line, string[] fields, string contextString)
        {
            bool GetIndex(string fieldName, out int index)
            {
                index = -1;
                for (int i = 0; i < fields.Length; ++i)
                {
                    if (fields[i].Equals(fieldName, StringComparison.InvariantCultureIgnoreCase))
                    {
                        index = i;
                        break;
                    }
                }
                if (index == -1)
                {
                    Debug.LogError($"Location instance file failed ({contextString}): could not find field '{fieldName}' in header");
                    return false;
                }
                return true;
            }

            int? GetIndexOpt(string fieldName)
            {
                int index = -1;
                for (int i = 0; i < fields.Length; ++i)
                {
                    if (fields[i].Equals(fieldName, StringComparison.InvariantCultureIgnoreCase))
                    {
                        index = i;
                        break;
                    }
                }
                if (index == -1)
                {
                    return null;
                }
                return index;
            }

            if (!GetIndex("name", out int nameIndex)) return null;
            if (!GetIndex("type", out int typeIndex)) return null;
            if (!GetIndex("prefab", out int prefabIndex)) return null;
            if (!GetIndex("worldX", out int worldXIndex)) return null;
            if (!GetIndex("worldY", out int worldYIndex)) return null;
            if (!GetIndex("terrainX", out int terrainXIndex)) return null;
            if (!GetIndex("terrainY", out int terrainYIndex)) return null;
            if (!GetIndex("locationID", out int locationIDIndex)) return null;
            int? rotWIndex = GetIndexOpt("rotW");
            int? rotXIndex = GetIndexOpt("rotX");
            int? rotYIndex = GetIndexOpt("rotY");
            int? rotZIndex = GetIndexOpt("rotZ");
            int? rotXAxisIndex = GetIndexOpt("rotXAxis");
            int? rotYAxisIndex = GetIndexOpt("rotYAxis");
            int? rotZAxisIndex = GetIndexOpt("rotZAxis");
            int? heightOffsetIndex = GetIndexOpt("heightOffset");

            CultureInfo cultureInfo = new CultureInfo("en-US");

            string[] tokens = line.Split(';', ',');

            LocationInstance tmpInst = new LocationInstance();

            try
            {
                tmpInst.name = tokens[nameIndex];
                tmpInst.type = int.Parse(tokens[typeIndex]);
                tmpInst.prefab = tokens[prefabIndex];
                tmpInst.worldX = int.Parse(tokens[worldXIndex]);
                tmpInst.worldY = int.Parse(tokens[worldYIndex]);
                tmpInst.terrainX = int.Parse(tokens[terrainXIndex]);
                tmpInst.terrainY = int.Parse(tokens[terrainYIndex]);
                tmpInst.locationID = ulong.Parse(tokens[locationIDIndex]);

                if (rotWIndex.HasValue) tmpInst.rot.w = float.Parse(tokens[rotWIndex.Value], cultureInfo);
                if (rotXIndex.HasValue) tmpInst.rot.x = float.Parse(tokens[rotXIndex.Value], cultureInfo);
                if (rotYIndex.HasValue) tmpInst.rot.y = float.Parse(tokens[rotYIndex.Value], cultureInfo);
                if (rotZIndex.HasValue) tmpInst.rot.z = float.Parse(tokens[rotZIndex.Value], cultureInfo);
                if (rotXAxisIndex.HasValue) tmpInst.rot.eulerAngles = new Vector3(float.Parse(tokens[rotXAxisIndex.Value], cultureInfo), tmpInst.rot.eulerAngles.y, tmpInst.rot.eulerAngles.z);
                if (rotYAxisIndex.HasValue) tmpInst.rot.eulerAngles = new Vector3(tmpInst.rot.eulerAngles.x, float.Parse(tokens[rotYAxisIndex.Value], cultureInfo), tmpInst.rot.eulerAngles.z);
                if (rotZAxisIndex.HasValue) tmpInst.rot.eulerAngles = new Vector3(tmpInst.rot.eulerAngles.x, tmpInst.rot.eulerAngles.y, float.Parse(tokens[rotZAxisIndex.Value], cultureInfo));
                if (heightOffsetIndex.HasValue) tmpInst.heightOffset = float.Parse(tokens[heightOffsetIndex.Value], cultureInfo);
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to parse a location instance ({contextString}): {e.Message}");
                return null;
            }

            return tmpInst;
        }

        /// <summary>
        /// Save location list to path
        /// </summary>
        /// <param name="locationInstance"></param>
        /// <param name="path"></param>
        public static void SaveLocationInstance(LocationInstance[] locationInstance, string path)
        {
            CultureInfo cultureInfo = new CultureInfo("en-US");

            StreamWriter writer = new StreamWriter(path, false);
            writer.WriteLine("<locations>");

            foreach (LocationInstance inst in locationInstance)
            {
                writer.WriteLine("\t<locationInstance>");
                writer.WriteLine("\t\t<name>" + inst.name + "</name>");
                writer.WriteLine("\t\t<locationID>" + inst.locationID + "</locationID>");
                writer.WriteLine("\t\t<type>" + inst.type + "</type>");
                writer.WriteLine("\t\t<prefab>" + inst.prefab + "</prefab>");
                writer.WriteLine("\t\t<worldX>" + inst.worldX + "</worldX>");
                writer.WriteLine("\t\t<worldY>" + inst.worldY + "</worldY>");
                writer.WriteLine("\t\t<terrainX>" + inst.terrainX + "</terrainX>");
                writer.WriteLine("\t\t<terrainY>" + inst.terrainY + "</terrainY>");
                if(inst.rot != Quaternion.identity)
                {
                    writer.WriteLine("\t\t<rotW>" + inst.rot.w.ToString(cultureInfo) + "</rotW>");
                    writer.WriteLine("\t\t<rotX>" + inst.rot.x.ToString(cultureInfo) + "</rotX>");
                    writer.WriteLine("\t\t<rotY>" + inst.rot.y.ToString(cultureInfo) + "</rotY>");
                    writer.WriteLine("\t\t<rotZ>" + inst.rot.z.ToString(cultureInfo) + "</rotZ>");
                }
                if(inst.heightOffset != 0f)
                {
                    writer.WriteLine("\t\t<heightOffset>" + inst.heightOffset.ToString(cultureInfo) + "</heightOffset>");
                }
                writer.WriteLine("\t</locationInstance>");
            }

            writer.WriteLine("</locations>");
            writer.Close();
        }

        /// <summary>
        /// Returns locationprefab based on file path. Return null if not found or wrong file
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static LocationPrefab LoadLocationPrefab(string path)
        {
            if (!File.Exists(path))
                return null;

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(path);

            return LoadLocationPrefab(xmlDoc);
        }

        public static LocationPrefab LoadLocationPrefab(Mod mod, string assetName)
        {
            TextAsset asset = mod.GetAsset<TextAsset>(assetName);
            if (asset == null)
                return null;

            TextReader reader = new StringReader(asset.text);

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(reader);

            return LoadLocationPrefab(xmlDoc);
        }

        public static LocationPrefab LoadLocationPrefab(XmlDocument xmlDoc)
        {
            XmlNode prefabNode = xmlDoc.SelectSingleNode("//locationPrefab");
            if (prefabNode == null)
            {
                Debug.LogWarning("Wrong file format");
                return null;
            }

            CultureInfo cultureInfo = new CultureInfo("en-US");

            LocationPrefab locationPrefab = new LocationPrefab();

            try
            {
                locationPrefab.height = int.Parse(prefabNode["height"].InnerXml);
                locationPrefab.width = int.Parse(prefabNode["width"].InnerXml);
            }
            catch (Exception e)
            {
                Debug.LogWarning($"Error while parsing location prefab: {e.Message}");
                return null;
            }

            var objects = xmlDoc.GetElementsByTagName("object");
            for (int i = 0; i < objects.Count; i++)
            {
                XmlNode objectNode = objects[i];

                var obj = new LocationObject();

                try
                {
                    obj.type = int.Parse(objectNode["type"].InnerXml);
                    obj.name = objectNode["name"].InnerXml;

                    obj.objectID = int.Parse(objectNode["objectID"].InnerXml);

                    obj.pos.x = float.Parse(objectNode["posX"].InnerXml, cultureInfo);
                    obj.pos.y = float.Parse(objectNode["posY"].InnerXml, cultureInfo);
                    obj.pos.z = float.Parse(objectNode["posZ"].InnerXml, cultureInfo);

                    obj.scale.x = float.Parse(objectNode["scaleX"].InnerXml, cultureInfo);
                    obj.scale.y = float.Parse(objectNode["scaleY"].InnerXml, cultureInfo);
                    obj.scale.z = float.Parse(objectNode["scaleZ"].InnerXml, cultureInfo);

                    if (obj.type == 0)
                    {
                        obj.rot.w = float.Parse(objectNode["rotW"].InnerXml, cultureInfo);
                        obj.rot.x = float.Parse(objectNode["rotX"].InnerXml, cultureInfo);
                        obj.rot.y = float.Parse(objectNode["rotY"].InnerXml, cultureInfo);
                        obj.rot.z = float.Parse(objectNode["rotZ"].InnerXml, cultureInfo);
                    }

                    var extraDataNode = objectNode["extraData"];
                    if (extraDataNode != null)
                    {
                        obj.extraData = extraDataNode.InnerXml;
                    }

                    if (!ValidateValue(obj.type, obj.name))
                        continue;
                }
                catch (Exception e)
                {
                    Debug.LogWarning($"Error while parsing location prefab object: {e.Message}");
                    continue;
                }

                locationPrefab.obj.Add(obj);
            }
            return locationPrefab;
        }

        /// <summary>
        /// Save locationprefab to path
        /// </summary>
        /// <param name="locationPrefab"></param>
        /// <param name="path"></param>
        public static void SaveLocationPrefab(LocationPrefab locationPrefab, string path)
        {
            CultureInfo cultureInfo = new CultureInfo("en-US");

            //Write some text to the test.txt file
            StreamWriter writer = new StreamWriter(path, false);

            writer.WriteLine("<locationPrefab>");
            writer.WriteLine("\t<height>" + locationPrefab.height + "</height>");
            writer.WriteLine("\t<width>" + locationPrefab.width + "</width>");

            foreach (LocationObject obj in locationPrefab.obj)
            {
                writer.WriteLine("\t<object>");
                writer.WriteLine("\t\t<type>" + obj.type + "</type>");
                writer.WriteLine("\t\t<objectID>" + obj.objectID + "</objectID>");
                writer.WriteLine("\t\t<name>" + obj.name + "</name>");

                writer.WriteLine("\t\t<posX>" + obj.pos.x.ToString(cultureInfo) + "</posX>");
                writer.WriteLine("\t\t<posY>" + obj.pos.y.ToString(cultureInfo) + "</posY>");
                writer.WriteLine("\t\t<posZ>" + obj.pos.z.ToString(cultureInfo) + "</posZ>");

                writer.WriteLine("\t\t<scaleX>" + obj.scale.x.ToString(cultureInfo) + "</scaleX>");
                writer.WriteLine("\t\t<scaleY>" + obj.scale.y.ToString(cultureInfo) + "</scaleY>");
                writer.WriteLine("\t\t<scaleZ>" + obj.scale.z.ToString(cultureInfo) + "</scaleZ>");

                if (!string.IsNullOrEmpty(obj.extraData))
                {
                    writer.WriteLine("\t\t<extraData>" + obj.extraData + "</extraData>");
                }

                if (obj.type == 0)
                {
                    writer.WriteLine("\t\t<rotW>" + obj.rot.w.ToString(cultureInfo) + "</rotW>");
                    writer.WriteLine("\t\t<rotX>" + obj.rot.x.ToString(cultureInfo) + "</rotX>");
                    writer.WriteLine("\t\t<rotY>" + obj.rot.y.ToString(cultureInfo) + "</rotY>");
                    writer.WriteLine("\t\t<rotZ>" + obj.rot.z.ToString(cultureInfo) + "</rotZ>");
                }

                writer.WriteLine("\t</object>");
            }

            writer.WriteLine("</locationPrefab>");
            writer.Close();
        }

        /// <summary>
        /// Validate the name of the object
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool ValidateValue(int type, string name)
        {
            if (type == 0)
            {
                try
                {
                    int.Parse(name);
                }
                catch (FormatException)
                {
                    Debug.LogWarning("Object type is set incorrectly: 0 = Model, 1 = Flat");
                    return false;
                }
                catch (OverflowException)
                {
                    Debug.LogWarning("Object type is set incorrectly: 0 = Model, 1 = Flat");
                    return false;
                }

                return true;
            }

            else if (type == 1)
            {
                string[] arg = name.Split('.');

                if (arg.Length == 2)
                {
                    try
                    {
                        int.Parse(arg[0]);
                        int.Parse(arg[1]);
                    }
                    catch (FormatException)
                    {
                        Debug.LogWarning("Billboard string format is invalid, use ARCHIVEID.RECORDID");
                        return false;
                    }
                    catch (OverflowException)
                    {
                        Debug.LogWarning("Billboard string format is invalid, use ARCHIVEID.RECORDID");
                        return false;
                    }

                    return true;
                }

                Debug.LogWarning("Billboard string format is invalid, use ARCHIVEID.RECORDID");
                return false;

            }
            else if (type == 2)
            {
                string[] arg = name.Split('.');

                if (arg.Length == 2)
                {
                    try
                    {
                        if (int.Parse(arg[0]) != 199)
                        {
                            Debug.LogWarning("Editor marker name format is invalid, use 199.RECORDID");
                            return false;
                        }
                        int.Parse(arg[1]);
                    }
                    catch (FormatException)
                    {
                        Debug.LogWarning("Editor marker name format is invalid, use 199.RECORDID");
                        return false;
                    }
                    catch (OverflowException)
                    {
                        Debug.LogWarning("Editor marker name format is invalid, use 199.RECORDID");
                        return false;
                    }

                    return true;
                }

                Debug.LogWarning("Editor marker name format is invalid, use 199.RECORDID");
                return false;
            }
            else
            {
                Debug.LogWarning($"Invalid obj type found: {type}");
                return false;
            }
        }

        /// <summary>
        /// Load a Game Object
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <param name="parent"></param>
        /// <param name="pos"></param>
        /// <param name="rot"></param>
        /// <param name="scale"></param>
        /// <returns></returns>
        public static GameObject LoadStaticObject(int type, string name, Transform parent, Vector3 pos, Quaternion rot, Vector3 scale, ulong locationID, int objID, ModelCombiner modelCombiner = null)
        {
            GameObject go = null;
            //Model
            if (type == 0)
            {
                if (rot.x == 0 && rot.y == 0 && rot.z == 0 && rot.w == 0)
                {
                    Debug.LogWarning($"Object {name} inside prefab has invalid rotation: {rot}");
                    rot = Quaternion.identity;
                }

                Matrix4x4 mat = Matrix4x4.TRS(pos, rot, scale);

                uint modelId = uint.Parse(name);

                go = MeshReplacement.ImportCustomGameobject(modelId, parent, mat);

                if (go == null) //if no mesh replacment exist
                {
                    if (modelCombiner != null && !PlayerActivate.HasCustomActivation(modelId))
                    {
                        ModelData modelData;
                        DaggerfallUnity.Instance.MeshReader.GetModelData(modelId, out modelData);

                        modelCombiner.Add(ref modelData, mat);
                    }
                    else
                    {
                        go = GameObjectHelper.CreateDaggerfallMeshGameObject(modelId, parent);
                        if (go != null)
                        {
                            go.transform.localPosition = pos;
                            go.transform.localRotation = rot;
                            go.transform.localScale = scale;
                        }
                    }
                }
            }

            //Flat
            else if (type == 1)
            {
                string[] arg = name.Split('.');

                go = MeshReplacement.ImportCustomFlatGameobject(int.Parse(arg[0]), int.Parse(arg[1]), pos, parent);

                if (go == null)
                {
                    go = GameObjectHelper.CreateDaggerfallBillboardGameObject(int.Parse(arg[0]), int.Parse(arg[1]), parent);

                    if (go != null)
                    {
                        go.transform.localPosition = pos;

                        if (arg[0] == "210")
                            AddLight(int.Parse(arg[1]), go.transform);

                        if (arg[0] == "201")
                            AddAnimalAudioSource(int.Parse(arg[1]), go);
                    }
                }

                if (go != null)
                {
                    go.transform.localScale = new Vector3(go.transform.localScale.x * scale.x, go.transform.localScale.y * scale.y, go.transform.localScale.z * scale.z);
                }
            }

            return go;
        }

        /// <summary>
        /// Adds a light to a flat. This is a modified copy of a method with the same name, found in DaggerfallInterior.cs
        /// </summary>
        /// <param name="id"></param>
        /// <param name="parent"></param>
        public static void AddLight(int textureRecord, Transform parent)
        {
            Debug.Log("Add Light");

            GameObject go = GameObjectHelper.InstantiatePrefab(DaggerfallUnity.Instance.Option_InteriorLightPrefab.gameObject, string.Empty, parent, parent.position);
            Vector2 size = DaggerfallUnity.Instance.MeshReader.GetScaledBillboardSize(210, textureRecord) * MeshReader.GlobalScale;
            Light light = go.GetComponent<Light>();
            switch (textureRecord)
            {
                case 0:         // Bowl with fire
                    go.transform.localPosition += new Vector3(0, -0.1f, 0);
                    break;
                case 1:         // Campfire
                                // todo
                    break;
                case 2:         // Skull candle
                    go.transform.localPosition += new Vector3(0, 0.1f, 0);
                    break;
                case 3:         // Candle
                    go.transform.localPosition += new Vector3(0, 0.1f, 0);
                    break;
                case 4:         // Candle in bowl
                                // todo
                    break;
                case 5:         // Candleholder with 3 candles
                    go.transform.localPosition += new Vector3(0, 0.15f, 0);
                    break;
                case 6:         // Skull torch
                    go.transform.localPosition += new Vector3(0, 0.6f, 0);
                    break;
                case 7:         // Wooden chandelier with extinguished candles
                                // todo
                    break;
                case 8:         // Turkis lamp
                                // do nothing
                    break;
                case 9:        // Metallic chandelier with burning candles
                    go.transform.localPosition += new Vector3(0, 0.4f, 0);
                    break;
                case 10:         // Metallic chandelier with extinguished candles
                                 // todo
                    break;
                case 11:        // Candle in lamp
                    go.transform.localPosition += new Vector3(0, -0.4f, 0);
                    break;
                case 12:         // Extinguished lamp
                                 // todo
                    break;
                case 13:        // Round lamp (e.g. main lamp in mages guild)
                    go.transform.localPosition += new Vector3(0, -0.35f, 0);
                    break;
                case 14:        // Standing lantern
                    go.transform.localPosition += new Vector3(0, size.y / 2, 0);
                    break;
                case 15:        // Standing lantern round
                    go.transform.localPosition += new Vector3(0, size.y / 2, 0);
                    break;
                case 16:         // Mounted Torch with thin holder
                                 // todo
                    break;
                case 17:        // Mounted torch 1
                    go.transform.localPosition += new Vector3(0, 0.2f, 0);
                    break;
                case 18:         // Mounted Torch 2
                                 // todo
                    break;
                case 19:         // Pillar with firebowl
                                 // todo
                    break;
                case 20:        // Brazier torch
                    go.transform.localPosition += new Vector3(0, 0.6f, 0);
                    break;
                case 21:        // Standing candle
                    go.transform.localPosition += new Vector3(0, size.y / 2.4f, 0);
                    break;
                case 22:         // Round lantern with medium chain
                    go.transform.localPosition += new Vector3(0, -0.5f, 0);
                    break;
                case 23:         // Wooden chandelier with burning candles
                                 // todo
                    break;
                case 24:        // Lantern with long chain
                    go.transform.localPosition += new Vector3(0, -1.85f, 0);
                    break;
                case 25:        // Lantern with medium chain
                    go.transform.localPosition += new Vector3(0, -1.0f, 0);
                    break;
                case 26:        // Lantern with short chain
                                // todo
                    break;
                case 27:        // Lantern with no chain
                    go.transform.localPosition += new Vector3(0, -0.02f, 0);
                    break;
                case 28:        // Street Lantern 1
                                // todo
                    break;
                case 29:        // Street Lantern 2
                    go.transform.localPosition += new Vector3(0, size.y / 2, 0);
                    break;
            }
            switch (textureRecord)
            {
                case 0:         // Bowl with fire
                    light.intensity = 1.2f;
                    light.range = 15f;
                    light.color = new Color32(255, 147, 41, 255);
                    break;
                case 1:         // Campfire
                                // todo
                    break;
                case 2:         // Skull candle
                    light.range /= 3f;
                    light.intensity = 0.6f;
                    light.color = new Color(1.0f, 0.99f, 0.82f);
                    break;
                case 3:         // Candle
                    light.range /= 3f;
                    break;
                case 4:         // Candle with base
                    light.range /= 3f;
                    break;
                case 5:         // Candleholder with 3 candles
                    light.range = 7.5f;
                    light.intensity = 0.33f;
                    light.color = new Color(1.0f, 0.89f, 0.61f);
                    break;
                case 6:         // Skull torch
                    light.range = 15.0f;
                    light.intensity = 0.75f;
                    light.color = new Color(1.0f, 0.93f, 0.62f);
                    break;
                case 7:         // Wooden chandelier with extinguished candles
                                // todo
                    break;
                case 8:         // Turkis lamp
                    light.color = new Color(0.68f, 1.0f, 0.94f);
                    break;
                case 9:        // metallic chandelier with burning candles
                    light.range = 15.0f;
                    light.intensity = 0.65f;
                    light.color = new Color(1.0f, 0.92f, 0.6f);
                    break;
                case 10:         // Metallic chandelier with extinguished candles
                                 // todo
                    break;
                case 11:        // Candle in lamp
                    light.range = 5.0f;
                    light.intensity = 0.5f;
                    break;
                case 12:         // Extinguished lamp
                                 // todo
                    break;
                case 13:        // Round lamp (e.g. main lamp in mages guild)
                    light.range *= 1.2f;
                    light.intensity = 1.1f;
                    light.color = new Color(0.93f, 0.84f, 0.49f);
                    break;
                case 14:        // Standing lantern
                                // todo
                    break;
                case 15:        // Standing lantern round
                                // todo
                    break;
                case 16:         // Mounted Torch with thin holder
                                 // todo
                    break;
                case 17:        // Mounted torch 1
                    light.intensity = 0.8f;
                    light.color = new Color(1.0f, 0.97f, 0.87f);
                    break;
                case 18:         // Mounted Torch 2
                                 // todo
                    break;
                case 19:         // Pillar with firebowl
                                 // todo
                    break;
                case 20:        // Brazier torch
                    light.range = 12.0f;
                    light.intensity = 0.75f;
                    light.color = new Color(1.0f, 0.92f, 0.72f);
                    break;
                case 21:        // Standing candle
                    light.range /= 3f;
                    light.intensity = 0.5f;
                    light.color = new Color(1.0f, 0.95f, 0.67f);
                    break;
                case 22:         // Round lantern with medium chain
                    light.intensity = 1.5f;
                    light.color = new Color(1.0f, 0.95f, 0.78f);
                    break;
                case 23:         // Wooden chandelier with burning candles
                                 // todo
                    break;
                case 24:        // Lantern with long chain
                    light.intensity = 1.4f;
                    light.color = new Color(1.0f, 0.98f, 0.64f);
                    break;
                case 25:        // Lantern with medium chain
                    light.intensity = 1.4f;
                    light.color = new Color(1.0f, 0.98f, 0.64f);
                    break;
                case 26:        // Lantern with short chain
                    light.intensity = 1.4f;
                    light.color = new Color(1.0f, 0.98f, 0.64f);
                    break;
                case 27:        // Lantern with no chain
                    light.intensity = 1.4f;
                    light.color = new Color(1.0f, 0.98f, 0.64f);
                    break;
                case 28:        // Street Lantern 1
                                // todo
                    break;
                case 29:        // Street Lantern 2
                                // todo
                    break;
                default:
                    light.intensity = 1.2f;
                    light.range = 15f;
                    light.color = new Color32(255, 147, 41, 255);
                    break;
            }
        }
        /// <summary>
        /// Add audioSource to animals. Is a modified copy version of a method with the same name, found in RMBLayout.cs
        /// </summary>
        /// <param name="id"></param>
        /// <param name="go"></param>
        public static void AddAnimalAudioSource(int textureRecord, GameObject go)
        {
            DaggerfallAudioSource source = go.AddComponent<DaggerfallAudioSource>();
            source.AudioSource.maxDistance = animalSoundMaxDistance;

            SoundClips sound = SoundClips.None;
            switch (textureRecord)
            {
                case 0:
                case 1:
                    sound = SoundClips.AnimalHorse;
                    break;
                case 3:
                case 4:
                    sound = SoundClips.AnimalCow;
                    break;
                case 5:
                case 6:
                    sound = SoundClips.AnimalPig;
                    break;
                case 7:
                case 8:
                    sound = SoundClips.AnimalCat;
                    break;
                case 9:
                case 10:
                    sound = SoundClips.AnimalDog;
                    break;
                default:
                    sound = SoundClips.None;
                    break;
            }

            source.SetSound(sound, AudioPresets.PlayRandomlyIfPlayerNear);
        }

        /// <summary>
        /// Creates a loot container. Is a modified copy version of a method with the same name, found in GameObjectHelper.cs
        /// </summary>
        /// <param name="billboardPosition"></param>
        /// <param name="parent"></param>
        /// <param name="locationID"></param>
        /// <param name="objID"></param>
        /// <param name="textureArchive"></param>
        /// <param name="textureRecord"></param>
        public static GameObject CreateLootContainer(ulong locationID, int objID, int textureArchive, int textureRecord, Transform parent)
        {
            GameObject go = GameObject.Instantiate(DaggerfallUnity.Instance.Option_LootContainerPrefab.gameObject);

            // We use our own serializer, get rid of the DFU one
            SerializableLootContainer serializableLootContainer = go.GetComponent<SerializableLootContainer>();
            if (serializableLootContainer != null)
            {
                GameObject.Destroy(serializableLootContainer);
            }

            
            // Setup DaggerfallLoot component to make lootable
            DaggerfallLoot loot = go.GetComponent<DaggerfallLoot>();
            if (loot)
            {
                ulong v = (uint)objID;
                loot.LoadID = (locationID << 16) | v;
                loot.WorldContext = WorldContext.Exterior;
                loot.ContainerType = LootContainerTypes.RandomTreasure;
                loot.TextureArchive = textureArchive;
                loot.TextureRecord = textureRecord;

                LocationLootSerializer serializer = go.AddComponent<LocationLootSerializer>();
                if (!serializer.TryLoadSavedData())
                {
                    // We had no existing save, generate new loot
                    if (!LootTables.GenerateLoot(loot, 2))
                        DaggerfallUnity.LogMessage(string.Format("DaggerfallInterior: Location type {0} is out of range or unknown.", 0, true));

                    var billboard = go.GetComponent<DaggerfallBillboard>();
                    if (billboard != null)
                        go.GetComponent<DaggerfallBillboard>().SetMaterial(textureArchive, textureRecord);

                    loot.stockedDate = DaggerfallLoot.CreateStockedDate(DaggerfallUnity.Instance.WorldTime.Now);
                }
            }
            
            go.transform.parent = parent;

            return go;
        }

        const byte Road_N = 128;//0b_1000_0000;
        const byte Road_NE = 64; //0b_0100_0000;
        const byte Road_E = 32; //0b_0010_0000;
        const byte Road_SE = 16; //0b_0001_0000;
        const byte Road_S = 8;  //0b_0000_1000;
        const byte Road_SW = 4;  //0b_0000_0100;
        const byte Road_W = 2;  //0b_0000_0010;
        const byte Road_NW = 1;  //0b_0000_0001;

        /// <summary>
        /// Checks if a location overlaps with a BasicRoad road.
        /// </summary>
        /// <param name="loc"></param>
        /// <param name="locationPrefab"></param>
        /// <param name="pathsDataPoint">Bytefield representing which cardinal directions have roads on the terrain</param>
        /// <returns></returns>
        public static bool OverlapsRoad(LocationInstance loc, LocationPrefab locationPrefab, byte pathsDataPoint)
        {
            RectInt locationRect = new RectInt(loc.terrainX, loc.terrainY, locationPrefab.width, locationPrefab.height);
            return OverlapsRoad(locationRect, pathsDataPoint);
        }

        public static bool OverlapsRoad(RectInt locationRect, byte pathsDataPoint)
        {
            Vector2Int locationTopLeft = new Vector2Int(locationRect.xMin, locationRect.yMax);
            Vector2Int locationTopRight = new Vector2Int(locationRect.xMax, locationRect.yMax);
            Vector2Int locationBottomLeft = new Vector2Int(locationRect.xMin, locationRect.yMin);
            Vector2Int locationBottomRight = new Vector2Int(locationRect.xMax, locationRect.yMin);

            const int TERRAIN_SIZE = LocationLoader.TERRAIN_SIZE;
            const int HALF_TERRAIN_SIZE = LocationLoader.TERRAIN_SIZE / 2;
            const int ROAD_WIDTH = LocationLoader.ROAD_WIDTH;
            const int HALF_ROAD_WIDTH = LocationLoader.ROAD_WIDTH / 2;

            if ((pathsDataPoint & Road_N) != 0)
            {
                if (locationRect.Overlaps(new RectInt(HALF_TERRAIN_SIZE - HALF_ROAD_WIDTH, HALF_TERRAIN_SIZE, ROAD_WIDTH, HALF_TERRAIN_SIZE)))
                    return true;
            }

            if ((pathsDataPoint & Road_E) != 0)
            {
                if (locationRect.Overlaps(new RectInt(HALF_TERRAIN_SIZE, HALF_TERRAIN_SIZE - HALF_ROAD_WIDTH, HALF_TERRAIN_SIZE, ROAD_WIDTH)))
                    return true;
            }

            if ((pathsDataPoint & Road_S) != 0)
            {
                if (locationRect.Overlaps(new RectInt(HALF_TERRAIN_SIZE - HALF_ROAD_WIDTH, 0, ROAD_WIDTH, HALF_TERRAIN_SIZE)))
                    return true;
            }

            if ((pathsDataPoint & Road_W) != 0)
            {
                if (locationRect.Overlaps(new RectInt(0, HALF_TERRAIN_SIZE - HALF_ROAD_WIDTH, HALF_TERRAIN_SIZE, ROAD_WIDTH)))
                    return true;
            }

            if ((pathsDataPoint & Road_NE) != 0)
            {
                // Location can only overlap if anywhere in the top-right quadrant
                if (locationTopRight.x >= HALF_TERRAIN_SIZE && locationTopRight.y >= HALF_TERRAIN_SIZE)
                {
                    float topLeftDiff = locationTopLeft.x - locationTopLeft.y;
                    float bottomRightDiff = locationBottomRight.x - locationBottomRight.y;

                    // Corner overlaps the path
                    if (Mathf.Abs(topLeftDiff) <= HALF_ROAD_WIDTH || Mathf.Abs(bottomRightDiff) <= HALF_ROAD_WIDTH)
                    {
                        return true;
                    }

                    // If corners are on different sides of the path, we have an overlap
                    if (Mathf.Sign(topLeftDiff) != Mathf.Sign(bottomRightDiff))
                    {
                        return true;
                    }
                }
            }

            if ((pathsDataPoint & Road_SE) != 0)
            {
                // Location can only overlap if anywhere in the bottom-right quadrant
                if (locationBottomRight.x >= HALF_TERRAIN_SIZE && locationBottomRight.y <= HALF_TERRAIN_SIZE)
                {
                    float bottomLeftDiff = locationBottomLeft.x + locationBottomLeft.y - TERRAIN_SIZE;
                    float topRightDiff = locationTopRight.x + locationTopRight.y - TERRAIN_SIZE;

                    // Corner overlaps the path
                    if (Mathf.Abs(bottomLeftDiff) <= HALF_ROAD_WIDTH || Mathf.Abs(topRightDiff) <= HALF_ROAD_WIDTH)
                    {
                        return true;
                    }

                    // If corners are on different sides of the path, we have an overlap
                    if (Mathf.Sign(bottomLeftDiff) != Mathf.Sign(topRightDiff))
                    {
                        return true;
                    }
                }
            }

            if ((pathsDataPoint & Road_SW) != 0)
            {
                // Location can only overlap if anywhere in the bottom-left quadrant
                if (locationBottomLeft.x <= HALF_TERRAIN_SIZE && locationBottomLeft.y <= HALF_TERRAIN_SIZE)
                {
                    float topLeftDiff = locationTopLeft.x - locationTopLeft.y;
                    float bottomRightDiff = locationBottomRight.x - locationBottomRight.y;

                    // Corner overlaps the path
                    if (Mathf.Abs(topLeftDiff) <= HALF_ROAD_WIDTH || Mathf.Abs(bottomRightDiff) <= HALF_ROAD_WIDTH)
                    {
                        return true;
                    }

                    // If corners are on different sides of the path, we have an overlap
                    if (Mathf.Sign(topLeftDiff) != Mathf.Sign(bottomRightDiff))
                    {
                        return true;
                    }
                }
            }

            if ((pathsDataPoint & Road_NW) != 0)
            {
                // Location can only overlap if anywhere in the bottom-right quadrant
                if (locationTopLeft.x <= HALF_TERRAIN_SIZE && locationTopLeft.y >= HALF_TERRAIN_SIZE)
                {
                    float bottomLeftDiff = locationBottomLeft.x + locationBottomLeft.y - TERRAIN_SIZE;
                    float topRightDiff = locationTopRight.x + locationTopRight.y - TERRAIN_SIZE;

                    // Corner overlaps the path
                    if (Mathf.Abs(bottomLeftDiff) <= HALF_ROAD_WIDTH || Mathf.Abs(topRightDiff) <= HALF_ROAD_WIDTH)
                    {
                        return true;
                    }

                    // If corners are on different sides of the path, we have an overlap
                    if (Mathf.Sign(bottomLeftDiff) != Mathf.Sign(topRightDiff))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}