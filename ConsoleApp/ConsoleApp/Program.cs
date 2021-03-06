﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenDataImport.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml.Linq;

namespace YC.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            //var nodes = findOpenData();
            //showOpenData(nodes);

            //var waterData = findWaterLevelRealTimeData();
            //showWaterData(waterData);


            var stations = findStations();
            showStations(stations);
            Console.ReadKey();

        }
        static List<OpenData> findOpenData()
        {
            List<OpenData> result = new List<OpenData>();



            var xml = XElement.Load(@"App_Data/datagovtw_dataset_20181005.xml");


            //XNamespace gml = @"http://www.opengis.net/gml/3.2";
            //XNamespace twed = @"http://twed.wra.gov.tw/twedml/opendata";
            List<XElement> nodes = xml.Descendants("node").ToList();

            for (var i = 0; i < nodes.Count; i++)
            {
                var node = nodes[i];


                OpenData item = new OpenData();

                item.id = int.Parse(getValue(node, "id"));
                item.資料集名稱 = getValue(node, "資料集名稱");
                item.主要欄位說明 = getValue(node, "主要欄位說明");
                item.服務分類 = getValue(node, "服務分類");
                result.Add(item);
            }
            return result;

        }

        static Models.WaterLevelDatas findWaterLevelRealTimeData()
        {
            Models.WaterLevelDatas result;
            WebClient wc = new WebClient();
            wc.Encoding = Encoding.UTF8;
            string jsonStr = wc.DownloadString("http://odata.wra.gov.tw/v4/RealtimeWaterLevel?$top=1000");
            result= JsonConvert.DeserializeObject<Models.WaterLevelDatas>(jsonStr);
            return result;
        }

        static Models.Stations findStations()
        {
            Models.Stations result;
            WebClient wc = new WebClient();
            wc.Encoding = Encoding.UTF8;
            string jsonStr = wc.DownloadString(@"https://data.wra.gov.tw/Service/OpenData.aspx?format=json&id=28E06316-FE39-40E2-8C35-7BF070FD8697");
            result = JsonConvert.DeserializeObject<Models.Stations>(jsonStr);
            return result;
        }



        private static string getValue(XElement node, string propertyName)
        {
            return node.Element(propertyName)?.Value?.Trim();

        }


        private static void showOpenData(List<OpenData> nodes)
        {

            Console.WriteLine(string.Format("共收到{0}筆的資料", nodes.Count));
            //nodes.GroupBy(node => node.服務分類).ToList()
            //    .ForEach(group =>
            //    {

            //        var key = group.Key;
            //        var groupDatas = group.ToList();
            //        var message = $"服務分類:{key},共有{groupDatas.Count()}筆資料";
            //        Console.WriteLine(message);
            //    });
            nodes.ToList()
                .ForEach(node =>
                {
                    var message = $"服務:{node.資料集名稱}";

                    Console.WriteLine(message);
                });
            Console.WriteLine($"共 : {nodes.Count} 筆");

        }

        private static void showWaterData(Models.WaterLevelDatas data)
        {
            data.Value.ForEach(item =>
            {
                Console.WriteLine($"Id:{item.StationIdentifier},Value:{item.WaterLevel}");


            });

        }
        private static void showStations(Models.Stations data)
        {
            data.StationInfo.ForEach(item =>
            {
                Console.WriteLine($"Id:{item.BasinIdentifier},RiverName:{item.RiverName}");


            });

        }
    }


}
