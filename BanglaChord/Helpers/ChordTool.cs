using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;


namespace BanglaChord.Helpers
{
    public class ChordTool
    {
        static string []ValidNotes = {"A","Bb","B","C","C#","D","D#","E","F","F#","G","G#"};
        static Hashtable EqNotes = new Hashtable() { {"Ab","G#"},{ "A#", "Bb" }, { "B#", "C" },
                                                     { "Bb", "Bb" }, { "Cb", "B" },
                                                        {"C#","C#"}, {"Db", "C#"}, {"D#","D#"},
                                                        {"Eb","D#"},{"E#", "F"}, {"Fb", "E"},
                                                        {"F#","F#"}, {"Gb", "F#"}, {"G#","G#"}};



        private static string SanitizeChord(string chord)
        {
            string chordNote, chordType;

            if (chord.Length == 1)
                return chord;

            char []cchord = chord.ToCharArray();
            if (cchord[1] == '#' || cchord[1] == 'b')
            {
                chordNote = chord.Substring(0, 2);
                chordNote = (string)EqNotes[chordNote];
                chordType = chord.Substring(2);

                return chordNote + chordType;
            }

            return chord;
        }


        private static int AdjustCircle5(int i)
        {
            if(i >= 12)
                return i - 12;

            if(i<0)
                return i+12;

            return i;
        }

        //transposes a chord based on half steps
        public static string Transpose(string chord, int nHsteps) {

            chord = SanitizeChord(chord);
            string chord1, chord2, tchord;

            int SlashPos = chord.IndexOf('/');
            if (SlashPos > -1) {
                chord1 = chord.Substring(0, SlashPos);
                chord2 = chord.Substring(SlashPos + 1);

                chord1 = DoTransposition(chord1, nHsteps);
                chord2 = DoTransposition(chord2, nHsteps);

                tchord = chord1 + "/" + chord2;
            }
            else {

                tchord = DoTransposition(chord, nHsteps);
            }

            return tchord;
        
        }

        private static string DoTransposition(string chord, int nHsteps) {
            string chordNote, chordType;
            char[] cchord = chord.ToCharArray();

            if (chord.Length <= 1)
            {
                chordNote = chord;
                chordType = "";
            }
            else
            {
                if (cchord[1] == '#' || cchord[1] == 'b')
                {
                    chordNote = chord.Substring(0, 2);
                    chordType = chord.Substring(2);
                }
                else
                {
                    chordNote = chord.Substring(0, 1);
                    chordType = chord.Substring(1);
                }
            }

            int cindex = Array.FindIndex(ValidNotes, item => item == chordNote);
            cindex += nHsteps;

            cindex = AdjustCircle5(cindex);
            chordNote = ValidNotes[cindex];

            return chordNote + chordType;        
        }

    }
}