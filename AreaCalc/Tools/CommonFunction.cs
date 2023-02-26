using Autodesk.Revit.DB;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace AreaCalc.Tools
{
    static class CommonFunction
    {
		public static JObject GetJsonData(string path)
		{
			if (!File.Exists(path))
			{
				return null;
			}
			string value;
			using (StreamReader streamReader = new StreamReader(path))
			{
				value = streamReader.ReadToEnd();
			}
			JsonSerializerSettings settings = new JsonSerializerSettings
			{
				NullValueHandling = NullValueHandling.Ignore,
				DateFormatString = "yyyy-MM-dd HH:m:ss"
			};
			dynamic val = JsonConvert.DeserializeObject<object>(value, settings);
			return val;
		}

		/// <summary>
		/// get the polyline, if the first and last value are the same, the polyline is closed.
		/// </summary>
		/// <param name="inputEdge"></param>
		/// <returns></returns>
		public static List<XYZ> GetPolygonFromRevit(HashSet<KeyValuePair<XYZ, XYZ>> inputEdge)
		{
			List<XYZ> polygon = new List<XYZ>();
			var init = inputEdge.ElementAt(0);
			polygon.Add(init.Key);
			polygon.Add(init.Value);
			while (true)
			{	
				var result = inputEdge.Where(p => init.Value.IsAlmostEqualTo(p.Key,0.0001)).Select(g => g).FirstOrDefault();
				if (result.Value != null)
				{
					polygon.Add(result.Value);
					inputEdge.Remove(init);
					init = result;
				}
				else
				{ 
					inputEdge.Remove(init);
					break;
				}
					
			}
			//polygon.RemoveAt(0);
			return polygon;
		}


		public static List<List<XYZ>> GetMultiPolygonsFromRevit(HashSet<KeyValuePair<XYZ, XYZ>> inputEdge)
		{
			List<List<XYZ>> polygons = new List<List<XYZ>>();
			while (inputEdge.Count != 0)
			{ 
				polygons.Add(GetPolygonFromRevit(inputEdge));
			}
			return polygons;
		}
		public static List<Vector2> ConvertXYZtoVector2(List<XYZ> Polyline)
		{
			var result = new List<Vector2>();
			foreach (var pt in Polyline)
			{
				result.Add(new Vector2((float)pt.X, (float)pt.Y));
			}
			return result;
		}
		public static List<List<Vector2>> ConvertXYZtoVector2List(List<List<XYZ>> polylines)
		{
			List<List<Vector2>> result = new List<List<Vector2>>();
			foreach (var polyline in polylines)
			{
				var polygon =ConvertXYZtoVector2(polyline);
				
				result.Add(CheckOrientation(polygon));
			}
			return result;
		}

		public static bool IsClockwise(List<Vector2> vertices)
		{
			double sum = 0.0;
			var v1 = vertices[vertices.Count - 1];
			for (int i = 0; i < vertices.Count; i++)
			{
				var v2 = vertices[i];
				sum += (v2.X - v1.X) * (v2.Y + v1.Y);
				v1 = v2;
			}
			Console.WriteLine(sum);
			return sum > 0.0;
		}
		public static bool IsClosed(List<Vector2> vertices)
		{
			return vertices.First() == vertices.Last();
		}
		/// <summary>
		/// check orientation and delete duplicated point
		/// </summary>
		/// <param name="vertices"></param>
		/// <returns></returns>
		public static List<Vector2> CheckOrientation(List<Vector2> vertices)
		{
			if (IsClosed(vertices))
			{
				vertices.RemoveAt(0);
				if (!IsClockwise(vertices))
				{
					vertices.Reverse();
				}
				return vertices;
			}
			throw new InvalidDataException("the input polyline is not closed");
		}

		/// <summary>
		/// Determines if the given point is inside the polygon
		/// </summary>
		/// <param name="polygon">the vertices of polygon</param>
		/// <param name="testPoint">the given point</param>
		/// <returns>true if the point is inside the polygon; otherwise, false</returns>
		public static bool IsInPolygon(this Vector2 point, IEnumerable<Vector2> polygon)
		{
			bool result = false;
			var a = polygon.Last();
			foreach (var b in polygon)
			{
				if ((b.X == point.X) && (b.Y == point.Y))
					return true;

				if ((b.Y == a.Y) && (point.Y == a.Y))
				{
					if ((a.X <= point.X) && (point.X <= b.X))
						return true;

					if ((b.X <= point.X) && (point.X <= a.X))
						return true;
				}

				if ((b.Y < point.Y) && (a.Y >= point.Y) || (a.Y < point.Y) && (b.Y >= point.Y))
				{
					if (b.X + (point.Y - b.Y) / (a.Y - b.Y) * (a.X - b.X) <= point.X)
						result = !result;
				}
				a = b;
			}
			return result;
		}

		public static int GetOuterPolygon(List<List<Vector2>> polygons)
		{
			for(int i=1;i<polygons.Count;i++)
			{
				if (IsInPolygon(polygons[0][0], polygons[i]))
				{
					return i;
				}			
			}
			return 0;
		}

	}
}
