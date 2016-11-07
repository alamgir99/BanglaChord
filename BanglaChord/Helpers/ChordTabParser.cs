using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace BanglaChord.Helpers
{
    //parses a chordtab formatted text
    public class ChordTabParser
    {
        public string Text { get; set; } // original text
        public int  TKey { get; set; }
        public string RText { get; set; } // rendedred text
        public string[] UsedChords { get; set; }

        private bool isParsed; // true if already parsed

        public ChordTabParser(string songText, int tKey=0)
        {
            Text = songText;
            isParsed = false;
            TKey = tKey;
        }

        public string Parse(string font = "", string classID="")
        {
            string[] Lines = Text.Split(new char[] { '\n' });
            int lineCount = Lines.Count();

            int lineNo = 0; // current line number, for showing error etc

            if (lineCount == 0)
            {
                isParsed = false;
                return ""; // cant proceed with parsing
            }
            if (isParsed == true) return RText;


            RText = "";// "<?xml version=\"1.0\" standalone=\"no\"?> <!DOCTYPE svg PUBLIC \"-//W3C//DTD SVG 1.1//EN\"  \"http://www.w3.org/Graphics/SVG/1.1/DTD/svg11.dtd\"> ";
            RText += "<svg id=\"SVGsongView\" preserveAspectRatio=\"xMidYMin\" ";
            if (classID != "" )
                RText += "class = \"" + classID + "\"";

            RText += "  viewBox=\"0 0 530 1400\"";

            RText += " xmlns=\"http://www.w3.org/2000/svg\" version=\"1.1\">";

            if (font != "")
                RText += "<g font-family=\"" + font + "\" >";

            RText += "<g transform=\"translate(0,0)\">";
            //dummy text line to prevent overlap
            //RText += "<text x=\"0\" y=\"0\" dy=\"1em\"> </text>";

            //parse each line
            foreach (string line in Lines)
            {
                lineNo++;
                string parsedLine = ParseLyricLine(line.Trim(), lineNo);
                if (parsedLine != "")
                {
                    RText += "<text x=\"0\" y=\"" + ((lineNo) * 2).ToString() + "em\">";
                    RText += parsedLine;
                    RText += "</text>" +Environment.NewLine;
                }
            }//for each line
            RText += "</g>";
            if (font != "") // close g tag
                RText += "</g>";

            //close svg taag
            RText += "</svg>";

            isParsed = true;

            return RText;
        }

        private string ParseLyricLine(string line, int lineNo)
        {
            //flag if a dy is in action
            bool dyOn = false;
            float dx = 0.0f;

            if (line.Trim().Length == 0)
            {
                return "";
            }

            string parsedString = "";

            char[] cline = line.ToCharArray();
            int pos = Array.IndexOf(cline, '['); // begining of a chord
            if (pos == -1)
            {   // no chords
                return "<tspan>" + line + "</tspan>";
            }

            if (pos > 0)
            {  // there are texts before the chord
                parsedString = "<tspan>" + line.Substring(0, pos).ToString() + "</tspan>";
                //pos = Array.IndexOf(cline, '[', pos+1); // 
            }
            //loop thru all chords
            while (pos > -1)
            {
                int posEnd = Array.IndexOf(cline, ']', pos);
                if (posEnd == -1) return "error in Line " + lineNo.ToString();

                string chord = line.Substring(pos + 1, posEnd - pos - 1);
                if (TKey != 0) {
                    chord = ChordTool.Transpose(chord, TKey);
                }

                if(chord.Length >=5)
                    dx = (float)(chord.Length/1.5 * 0.8f);
                else
                    dx = (float)(chord.Length * 0.8f);
                //parsedString += "<tspan dx=\"" + dx.ToString() + "em\" dy=\"-0.9em\">" + chord + "</tspan>";
                parsedString += "<tspan dx=.8em dy=\"-0.9em\">" + chord + "</tspan>";
                dyOn = true;

                if (posEnd == line.Length - 1)
                    break; // break the loop

                //find next block of lyric
                pos = Array.IndexOf(cline, '[', posEnd);
                if (pos > -1)
                {
                    string lyric = line.Substring(posEnd + 1, pos - posEnd - 1);
                    if (dyOn == true)
                    {
                        parsedString += "<tspan dx=\"-" + (dx).ToString() + "em\" dy=\"0.9em\">" + " "+lyric + "</tspan>";
                        dyOn = false;
                    }
                    else
                        parsedString += "<tspan>" +" " +lyric + "</tspan>";

                    //pos = Array.IndexOf(cline, '[', pos+1); //for next iteration
                }
                else
                {  // no chord found
                    string lyric = line.Substring(posEnd + 1);
                    if (dyOn == true)
                    {
                        parsedString += "<tspan dx=\"-" + dx.ToString() + "em\" dy=\"0.9em\">" + lyric + "</tspan>";
                        dyOn = false;
                    }
                    else
                        parsedString += "<tspan>" + lyric + "</tspan>";
                    break; // no more chord
                }
            }
            return parsedString;
        } // DoParse
    } // end of class
} // end of namespace