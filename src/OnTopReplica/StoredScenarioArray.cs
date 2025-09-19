using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Xml.Serialization;
using System.Xml;
using System.Xml.Linq;
using System.Globalization;

namespace OnTopReplica
{

    /// <summary>
    /// Strongly typed array of StoredScenario elements.
    /// </summary>
    /// <remarks>
    /// Handles XML serialization.
    /// </remarks>
    public class StoredScenarioArray : List<StoredScenario>, IXmlSerializable
    {

        #region IXmlSerializable Members

        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(System.Xml.XmlReader reader)
        {
            this.Clear();

            var doc = XDocument.Load(reader);
            foreach (var xmlScenario in doc.Descendants("StoredScenario"))
            {
                StoredScenario parsedScenario = ParseStoredScenario(xmlScenario);
                if (parsedScenario != null)
                {
                    this.Add(parsedScenario);
                }
            }
        }

        private StoredScenario ParseStoredScenario(XElement xmlScenario)
        {
            var scenario = new StoredScenario();

            var xName = xmlScenario.Attribute("name");
            if (xName == null || string.IsNullOrWhiteSpace(xName.Value))
            {
                System.Diagnostics.Debug.Fail("Parsed stored scenario has no name attribute.");
                return null;
            }
            scenario.Name = xName.Value;

            //Window match
            var xWindowMatch = xmlScenario.Element("WindowMatch");
            if(xWindowMatch != null){
                var xTitle = xWindowMatch.Element("Title");
                if (xTitle != null)
                    scenario.WindowTitle = xTitle.Value;

                var xClass = xWindowMatch.Element("Class");
                if (xClass != null)
                    scenario.WindowClass = xClass.Value;
            }

            //Visuals
            var xVisuals = xmlScenario.Element("Visuals");
            if (xVisuals != null)
            {
                var xRegion = xVisuals.Element("Region");
                if (xRegion != null)
                    scenario.Region = ParseRegion(xRegion);

                var xOpacity = xVisuals.Element("Opacity");
                if (xOpacity != null)
                    scenario.Opacity = byte.Parse(xOpacity.Value, CultureInfo.InvariantCulture);

                var xChrome = xVisuals.Element("Chrome");
                if (xChrome != null)
                    scenario.IsChromeVisible = bool.Parse(xChrome.Value);
            }

            return scenario;
        }

        private ThumbnailRegion ParseRegion(XElement xmlRegion)
        {
            var xRectangle = xmlRegion.Element("Rectangle");
            if (xRectangle != null)
            {
                System.Drawing.Rectangle rectangle = ParseRectangle(xRectangle);
                return new ThumbnailRegion(rectangle);
            }

            var xPadding = xmlRegion.Element("Padding");
            if (xPadding != null)
            {
                System.Windows.Forms.Padding padding = ParsePadding(xPadding);
                return new ThumbnailRegion(padding);
            }

            return null;
        }

        private System.Windows.Forms.Padding ParsePadding(XElement xPadding)
        {
            var p = new System.Windows.Forms.Padding();
            try
            {
                p.Left = Int32.Parse(xPadding.Element("Left").Value);
                p.Top = Int32.Parse(xPadding.Element("Top").Value);
                p.Right = Int32.Parse(xPadding.Element("Right").Value);
                p.Bottom = Int32.Parse(xPadding.Element("Bottom").Value);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Fail("Failure while parsing padding data.", ex.ToString());
            }
            return p;
        }

        private System.Drawing.Rectangle ParseRectangle(XElement xRectangle)
        {
            var r = new System.Drawing.Rectangle();
            try
            {
                r.X = Int32.Parse(xRectangle.Element("X").Value);
                r.Y = Int32.Parse(xRectangle.Element("Y").Value);
                r.Width = Int32.Parse(xRectangle.Element("Width").Value);
                r.Height = Int32.Parse(xRectangle.Element("Height").Value);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Fail("Failure while parsing rectangle data.", ex.ToString());
            }
            return r;
        }

        public void WriteXml(System.Xml.XmlWriter writer)
        {
            foreach (var scenario in this)
            {
                WriteScenario(writer, scenario);
            }
        }

        private void WriteScenario(XmlWriter writer, StoredScenario scenario)
        {
            writer.WriteStartElement("StoredScenario");
            writer.WriteAttributeString("name", scenario.Name);

            //Window match
            writer.WriteStartElement("WindowMatch");
            if(scenario.WindowTitle != null)
                writer.WriteElementString("Title", scenario.WindowTitle);
            if(scenario.WindowClass != null)
                writer.WriteElementString("Class", scenario.WindowClass);
            writer.WriteEndElement();

            //Visuals
            writer.WriteStartElement("Visuals");
            if (scenario.Region != null)
            {
                writer.WriteStartElement("Region");
                if (scenario.Region.Relative)
                {
                    WriteRelativeRegion(writer, scenario.Region);
                }
                else
                {
                    WriteAbsoluteRegion(writer, scenario.Region);
                }
                writer.WriteEndElement();
            }
            writer.WriteElementString("Opacity", scenario.Opacity.ToString(CultureInfo.InvariantCulture));
            writer.WriteElementString("Chrome", scenario.IsChromeVisible.ToString());
            writer.WriteEndElement();

            writer.WriteEndElement();
        }

        private void WriteAbsoluteRegion(XmlWriter writer, ThumbnailRegion region)
        {
            writer.WriteStartElement("Rectangle");

            var bounds = region.Bounds;
            writer.WriteElementString("X", bounds.X.ToString());
            writer.WriteElementString("Y", bounds.Y.ToString());
            writer.WriteElementString("Width", bounds.Width.ToString());
            writer.WriteElementString("Height", bounds.Height.ToString());

            writer.WriteEndElement();
        }

        private void WriteRelativeRegion(XmlWriter writer, ThumbnailRegion region)
        {
            writer.WriteStartElement("Padding");

            var padding = region.BoundsAsPadding;
            writer.WriteElementString("Left", padding.Left.ToString());
            writer.WriteElementString("Top", padding.Top.ToString());
            writer.WriteElementString("Right", padding.Right.ToString());
            writer.WriteElementString("Bottom", padding.Bottom.ToString());

            writer.WriteEndElement();
        }

        #endregion

    }
}
