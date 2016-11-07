/*
 Metacode
 *
 * Helper class to help generate a code for Bengali string based on double metacode algorithm.
 * Author: Alamgir Mohammed
 * Last edit: Sep 11, 2016
*/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BanglaChord.Helpers;

namespace BanglaChord.Helpers
{
    public class Metacode
    {
        //generates metacode for a bengali string using double-metaphone algoritm
        public static string Generate(string str)
        {
            string encStr=""; // encoded string
            char nextChar, prevChar, curChar; // next, previous and current character
            bool nextHosh = false; // next character is a halant/hashant
            bool prevHosh = false; // previous character was a halant/hasant

            char [] srcStr  = Canonize(str);
            int   len = srcStr.Length;
            int i;
            for( i = 0; i < len; i++){
                curChar = srcStr[i]; // get the current character

                //init the flags
                nextHosh = false;
                prevHosh = false;

                //info about the next character
                if (i < len - 1)
                {
                    nextChar = srcStr[i + 1];
                    if (nextChar == '\u09cd') nextHosh = true;
                }
                else {
                    nextChar = '\u0000';
                }

                //info about previous character
                if (i > 0){
                    prevChar = srcStr[i - 1];
                    if (prevChar == '\u09cd') prevHosh = true;
                }
                else {
                    prevChar = '\u0000';
                }

                //process characters
                // 1. shoro aa and aa kar -> o
                if (curChar == '\u0985' || curChar == '\u0993') {
                    encStr += "o"; 
                }
                
		        // 2. shoro a and a kar -> a
		        // 
		        else if((curChar == '\u0986') || (curChar == '\u09BE')) {
			        encStr +=  "a";
		        }
		        
                // 3. hrosh i, dirgh i and their kar -> i
		        else if((curChar == '\u0987') || (curChar == '\u0988') || (curChar == '\u09BF') || (curChar == '\u09C0')) {
			        encStr +=  "i";
		        }
		        // 4. hrosh u, dirgh u and their kaars -> u
		        else if ((curChar == '\u0989') || (curChar == '\u098A') || (curChar == '\u09C1') || (curChar == '\u09C2')) {
			        encStr +=  "u";
		        }
		        // 5. e or e kar -> e
		        else if((curChar == '\u098F') || (curChar == '\u09C7')) {
			        encStr +=  "e";
		        }
		        // 6. oi and oi kar -> oi
		        else if((curChar == '\u0990') || (curChar == '\u09C8')) {
			        encStr +=  "oi";
		        }
		        // 7. ou and ou kar
		        else if((curChar == '\u0994') || (curChar == '\u09CC')) {
			        encStr +=  "ou";
		        }

                // 8. ka -> k
		        // 8. khio -> kh or kkh
		        else if( curChar == '\u0995') {
			        if(nextHosh == true && i < len-2) {
				        nextChar = srcStr[i+2];

				        if(nextChar ==  '\u09B7') { //mordhuna sha
				            i += 2;
					        if(encStr.Length == 0) { // khio is the first char
					    		encStr =  "kh";
					        }
					        else {
						        encStr +=  "kkh";
					        }
				        } //not a mordhuna sha
				        else {
					        encStr +=  "k";
				        }
			        }// end of if nextHost 
			        else { // not part  of a khio
				        encStr +=  "k";
			        }
		        } // if curChar 0995

		        // 9. kha -> kh
		        else if(curChar == '\u0996') {
			        encStr +=  "kh";
		        }

		        // 10. ga
		        else if(curChar == '\u0997')	{
			        encStr +=  "g";
		        }

		        //11. gha 
		        else if(curChar == '\u0998') {
			        encStr +=  "gh";
		        }

		        /* 12.
			    * ng	? 	0999	NGA	: uwo
			    * 			0982		: onushar	in bangla
			    * 
		        */
	
		        else if((curChar == '\u0999') || (curChar == '\u0982')) {
			        encStr +=  "ng";
		        }

		        // 13. c ? = 099A	: ca
		        else if(curChar == '\u099A') {
			        encStr +=  "c";
		        } 
		        //	14.   ? = 099B 	: cha
		        else if (curChar == '\u099B') {
			        encStr +=  "ch";
		        }

		        // 15. * j ? = 099C 	: JA	borgio ja 
                else if(curChar == '\u099C') {
			        encStr +=  "j";
		        }

		        //	16.  *   ? = 099D 	: JHA	jha
		        else if(curChar == '\u099D') {
			        encStr +=  "jh";
		        }
		        // 17. ya fola
		        else if(curChar == '\u09AF')	{
			        if(i==2  && prevHosh == true) { //the ya fala is just after the first char of the word
			            encStr +=  "e";

				        if( i < len-1) {
					        nextChar = srcStr[i+1];
					        if(nextChar == '\u09BE') // a kar
						            i++; //we skip that a kar
				        }
			        }
			        else if( i > 2  && prevHosh == true) { // ya fala at the middle or end of word
					    prevChar = srcStr[i-3]; 
				        if(prevChar == '\u09CD'){ // ya fala is after a conjunct
							//we dont do anything
				    		// ya fala in NOT coded
					    	//??
                            ;
				        }
				        else { // double the last char, if not after ha
				            prevChar = srcStr[i-2]; 
					        if(prevChar == '\u09B9'){
						        encStr +=  "hh"; //jj
					        }
					        else{	
						        encStr += encStr.Substring(encStr.Length-1,1);
					        }
				        }				
			        }
			        else { // not a fala
			            encStr +=  "j";
			        }
		        } // if 09af
                
		        //18. neo
		        else if(curChar == '\u099E'){
			        if( (i >= 2) && prevHosh == true) {
				        prevChar = srcStr[i-2];
				        if(prevChar == '\u099A'){ // ca (cha)
				            encStr +=  "n";
				        }
				        else if(prevChar == '\u099C') { // borgio ja
				    		if(i==2) {
                                
                                encStr = encStr.ReplaceAt(encStr.Length - 1, 'g'); 
						        encStr +=  "e";

        						nextChar = srcStr[i+1];
		        				if(nextChar == '\u09BE') { // a kar
						            //skip that a kar
							        i++;
						        }
					        }
					        else if( i > 2 ){
                                encStr = encStr.ReplaceAt(encStr.Length - 1, 'g');
						        encStr +=  "g";
					        }
				        }
			        } // if i>2
			        else if( (i+2 < len) && nextHosh == true){
				        nextChar = srcStr[i+2];

				        // nya + cha/ccha/ja/jha 
				        switch(nextChar){
					        case '\u099A':
					        case '\u099B':
					        case '\u099C':
					        case '\u099D':
						        encStr +=  "n";
						        break;

					        case '\u0985':
					        case '\u0987':
						        break; //?? nothing

					        default:
						        encStr +=  "ng";
                                break;
				        } // switch
			        } // if i+2 < len
			        else { //no othe conditions matched
			            encStr +=  "ng";
			        }
		        }// if 99E

		        // 19. * T ? = 099F : TTA	ta 
		        else if(curChar == '\u099F'){
			        encStr +=  "T";
		        }
		
		        // 20.	 * 	 ? = 09A0 : TTHA	tha
		        else if(curChar == '\u09A0') 	{
			        encStr +=  "Th";
		        }

		        // 21. * D ? = 09A1 	: DDA	da
		        else if(curChar == '\u09A1')	{
			        encStr +=  "D";
		        }

		        // 22.  ? = 09A2 	: DDHA	dha
		        else if(curChar == '\u09A2')	{
			        encStr +=  "Dh";
		        }
		
		        // 23.	ri	098B	VOCALIC R	: ri // hrosh ri
		        //	09C3	symbolic VOCALIC R : niche ri // ri kar
		        else if((curChar == '\u098B')||(curChar == '\u09C3')){
			        encStr +=  "ri";
		        }
		
		        // 24. * r		? 	=	09B0	: RA	bo shunno ro
		        // *		? 	=	09DC	: RRA	do shunno ro
		        // *		? 	=	09DD	: DDHA	dho shunno Ro
		        else if((curChar == '\u09B0') || (curChar == '\u09DC') || (curChar == '\u09DD')) {
			        encStr +=  "r"; 
		        }
		
		        // 25.	 * n ? = 09A8 	NA	: donton no  
		        //	 *   ? = 09A3   NNA	: murdhon no
		        else if((curChar == '\u09A8') || (curChar == '\u09A3'))	{
			        encStr +=  "n";
		        }
		
		        // 26. * t ? = 09A4 : TA	ta 
		        else if(curChar == '\u09A4'){
			        encStr +=  "t";
		        }
		        // 27.	 * 	 ? = 09A5 : THA	tha
		        else if(curChar == '\u09A5') {
			        encStr +=  "th";
		        }
		
		        // 28. * d ? = 09A6  : DA		da -> dat (teeth)
		        else if(curChar == '\u09A6')	{
			        encStr +=  "d";
		        }
		        // 29.  *   ? = 09A7  : DHA	dha -> dhan (rice)
		        else if(curChar == '\u09A7') 	{
			        encStr +=  "dh";
		        }
		
		        // 30. * p		? 	=	09AA : 	po	 : pola
		        else if(curChar == '\u09AA')	{
			        encStr +=  "p";
		        }
		        //	31. ?	=	09AB :  fo	 : fol
		        else if(curChar == '\u09AB') 	{
			        encStr +=  "ph";
		        }

		        // 31 * b		?	=	09AD : bho 	: bhola
		        else if(curChar == '\u09AD') 	{
			        encStr +=  "bh";
		        }

		        //32. * b		? 	=	09AC : bo	: bolo
		        else if(curChar == '\u09AC') {
			        if(prevHosh == false && nextHosh==false) { //stand alone ba
							encStr +=  "b";
			        }
			        else if(prevHosh)	{
				        if(i==2){ // @the beginning
				                ; //do nothing ??
				        }
				        else if(i>2){
					        prevChar = srcStr[i-3];
					        
                            if(prevChar == '\u09CD'){
						        //do nothng ??
					        }
					        else{
						        prevChar = srcStr[i-2];
							
                                switch(prevChar) {
						            case '\u09ac': //ba
						            case '\u09ae':
						            case '\u0997':
    							        encStr +=  "b";
							            break;

						            case '\u09a6': //da (udbeg
							            prevChar = srcStr[i-3];
							            if(prevChar == '\u0989')    
                                            encStr +=  "b";
	    						        break;

		    				        default:
			    				        encStr += encStr.Substring(encStr.Length-1,1);
                                        break;
				    		    }//switch
					        } // else if prevchar
				        } // else if i>2
			        }//prevHosh
			        else{
				        encStr +=  "b";
			        }
		        } //end of ba

		        //33. ma
		        // * m		? 	=	09AE: mo
		        else if(curChar == '\u09ae'){
			        if(prevHosh ==false && nextHosh ==false){
				        encStr +=  "m";
			        }
			        else if(prevHosh==true)	{
				        if(i==2) { // @the beginning
									//nothing ??
                            ;
                        }
				        else if(i>2){
					        prevChar = srcStr[i-3];
					        if(prevChar == '\u09cd'){
							    // do nothing ??
                                ;
					        }
					        else{
						        prevChar = srcStr[i-2];
						        
                                switch(prevChar){
							    //KA / GA / NGA / TTA / NA / NNA / MA / LA / S / SSA / SHA
						            case '\u0995':
						            case '\u0997':
						            case '\u0999':
						            case '\u099F':
						            case '\u09A3':
						            case '\u09A8':
						            case '\u09AE':
						            case '\u09B2':
						            case '\u09B6':
						            case '\u09B7':
						            case '\u09B8':
							            encStr +=  "m";
							            break;
						
                                    case '\u09B0': //me
							            encStr +=  "rm";
							            break;

						            default:
                                        encStr += encStr.Substring(encStr.Length - 1, 1);
							        break;
						        } // switch
					        } //else
				        } // i> 2
			        } // end of prevhosh
			        else {
				        encStr +=  "m";
			        }
		        }

		        //34.	y	? 	09DF:  YYA	: ontostho yeo	boyosh
		        else if(curChar == '\u09DF')  {
			        encStr +=  "y";
		        }

		        //35. * l		? 	=	09B2: LA	lo
		        else if(curChar == '\u09B2') 	{
			        encStr +=  "l";
        		}

		        // 36. * s		? 	=	09B8: SA	dontonno sho
		        //	 *			? 	=	09B7: SSA	murdhonno sho
		        //	 * 			?	=	09B6: SHA	tal bosh sho
		        else if((curChar == '\u09B8') || (curChar == '\u09B7') || (curChar == '\u09B6')) {
			        encStr +=  "s";
		        }

		        // 37. * h		? 	=	09B9: ho
		        else if(curChar == '\u09B9') {
			        if(nextHosh==false && prevHosh==false){
				        encStr +=  "h";
			        }
			        else if(nextHosh==true)	{
				        if (i < len - 2)	{
					        nextChar = srcStr[i+2];
					        switch(nextChar)	{
					            case '\u098B':
					            case '\u09B0': // ri, r as fola
						            break; //?? do nothing

        					    case '\u09AE': //M mo
		            				encStr +=  "mm";
					            	i+=2;
						            break;

					            case '\u09AF': // ya as fola
						            if (i>2){
							            encStr +=  "jj";
							            i+=2;
						            }
						            else
							            encStr +=  "h";

						            break;

					            case '\u09B2': // LA
						            if(i==0){
							            encStr +=  "l";
						            }
						            else{
							            encStr +=  "ll";
						            }

						            i+=2;
						            break;

					            case '\u09A3':
					            case '\u09A8':
						            encStr +=  "nn";
						            break;

					            case '\u09AC': // ba
						            encStr +=  "hb";
						            i+=2;
						            break;

					            default:
						            encStr +=  "h";
						            break;
					            } // end of switch
				            } // if i < len -2
			            } // nextHosh true
			            /* else // by me
			            {
				        encStr +=  "h";
                    	} */	
		            }// if 9b9
		
		          //38.	 * 	0983	bishorgo
		        else if(curChar == '\u0983'){
			        if(i == len-1){
				        if((i==1) || (i==2))	{
					        encStr +=  "h";
				        }
				        else{
					        ;//nothing ??
				        }
			        }
		        } // if 0983

		        //39. 		?	=	09CD	(hoshonto)
		        //      	0981	chondrobindu
		        //	        09CB	symbolic O
		        else if((curChar == '\u09CD') || (curChar == '\u0981') || (curChar == '\u09CB'))	{
			        ;    //nothing

			        //encoded[i] = 'v';
			        //			hoshCnt++;
			        //			i--;
		        }
                else{
			        ; // just ignore
		        }
            } // for loop i

            //return the generated code
            return encStr;
        } // end of mehod Generate

        //connonize a bengali string
        //connonization involves combing kaar ( | => o-kar etc
        private static char[] Canonize(string str)
        {
            int i = 0, j = 0; // loop control
            int len = str.Length;
            char[] ostr = new Char [len];

            char [] cstr = str.ToCharArray();

            for (i = 0; i < len - 1; i++) {
                if (cstr[i] == '\u09c7')  {
                    if (cstr[i + 1] == '\u09be') {
                        ostr[j++] = '\u09cb';
                        i++; // extra increment to ignore kaar part
                    }
                    else if (cstr[i + 1] == '\u09d7') { 
                        ostr[j++] = '\u09cc';
                        i++; // extra increment to ignore kaar
                    } else {
                        ostr[j++] = '\u09c7';
                    }
                    continue; // next i
                } // end of if 09c7

                if (cstr[i] == '\u09a1') {
                    if (cstr[i + 1] == '\u09bc')
                    {
                        ostr[j++] = '\u09dc';
                        i++; // extra increment to ignore the kar
                    }
                    else {
                        ostr[j++] = '\u09a1';
                    }
                    continue;
                } // end  of if 09a1

                if (cstr[i] == '\u09a2')
                {
                    if (cstr[i + 1] == '\u09bc')
                    {
                        ostr[j++] = '\u09dd';
                        i++; // extra increment to ignore the kar
                    }
                    else
                    {
                        ostr[j++] = '\u09a2';
                    }
                    continue;
                } // end  of if 09a2

                if (cstr[i] == '\u09af')
                {
                    if (cstr[i + 1] == '\u09bc')
                    {
                        ostr[j++] = '\u09df';
                        i++; // extra increment to ignore the kar
                    }
                    else
                    {
                        ostr[j++] = '\u09af';
                    }
                    continue;
                } // end  of if 09af

                //no other cases to apply
                ostr[j++] = cstr[i];
            } // for i loop
            // copy the last character
            ostr[j] = cstr[i];

            //return the array 
            return ostr; 
        }
        

    
    }


}