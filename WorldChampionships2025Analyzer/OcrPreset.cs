using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract;

namespace WorldChampionships2025Analyzer
{
	public class OcrPreset
	{
		public PageSegMode Psm { get; set; }
		public EngineMode Oem { get; set; }
		public string Whitelist { get; set; } = "";
	}
}
