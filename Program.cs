﻿using System;
using System.IO;

using Clifton.Core.ExtensionMethods;

using FiddleDock.RouteHandlers;

namespace FiddleDock
{
	class Program
	{
		static void Main(string[] args)
		{
			WebServer ws = new WebServer();
			ws.Logger += Logger;
			InitializeRoutes(ws);
			ws.StartWebServer();
			Console.WriteLine("Web server ready.");
			Console.ReadLine();
		}

		private static void Logger(object sender, LogEventArgs e)
		{
			Console.WriteLine(e.Message);
		}

		private static string GetPath()
		{
			return @"c:\projects\FiddleDock\Website";
		}

		private static void InitializeRoutes(WebServer ws)
		{
			ws.AddRoute("GET", "/", (context, _) => new StringResponse() { Data = File.ReadAllText(Path.Combine(GetPath(), "index.html")), ContentType = "text/html" });
			ws.AddRoute("GET", "/index", (context, _) => new StringResponse() { Data = File.ReadAllText(Path.Combine(GetPath(), "index.html")), ContentType = "text/html" });
			ws.AddRoute("GET", "/index.html", (context, _) => new StringResponse() { Data = File.ReadAllText(Path.Combine(GetPath(), "index.html")), ContentType = "text/html" });
			ws.AddRoute("GET", "*.js", (context, _) => new StringResponse() { Data = File.ReadAllText(Path.Combine(GetPath(), context.Request.Url.LocalPath.WindowsDelimiters().Substring(1))), ContentType = "text/javascript" });
			ws.AddRoute("GET", "*.css", (context, _) => new StringResponse() { Data = File.ReadAllText(Path.Combine(GetPath(), context.Request.Url.LocalPath.WindowsDelimiters().Substring(1))), ContentType = "text/css" });
			ws.AddRoute("GET", "*.jpg", (context, _) => new BinaryResponse() { ByteData = File.ReadAllBytes(Path.Combine(GetPath(), context.Request.Url.LocalPath.WindowsDelimiters().Substring(1))), ContentType = "image/jpg" });
			ws.AddRoute("GET", "*.png", (context, _) => new BinaryResponse() { ByteData = File.ReadAllBytes(Path.Combine(GetPath(), context.Request.Url.LocalPath.WindowsDelimiters().Substring(1))), ContentType = "image/png" });
			ws.AddRoute("GET", "*.bmp", (context, _) => new BinaryResponse() { ByteData = File.ReadAllBytes(Path.Combine(GetPath(), context.Request.Url.LocalPath.WindowsDelimiters().Substring(1))), ContentType = "image/bmp" });
			ws.AddRoute("GET", "*.ico", (context, _) => new BinaryResponse() { ByteData = File.ReadAllBytes(Path.Combine(GetPath(), context.Request.Url.LocalPath.WindowsDelimiters().Substring(1))), ContentType = "image/x-icon" });

			ws.AddRoute("POST", "/runOnHost", (context, _) => new RunOnHost() { ContentType = "text/json" });
			ws.AddRoute("POST", "/runOnDocker", (context, session) => new RunOnDocker(session) { ContentType = "text/json" });
			ws.AddRoute("POST", "/createNewInstance", (context, session) => new CreateNewInstance(session) { ContentType = "text/json" });
			ws.AddRoute("POST", "/deleteInstance", (context, session) => new DeleteInstance(session) { ContentType = "text/json" });
		}
	}
}
