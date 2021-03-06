﻿using System;
using System.Collections.Generic;
using Khud0.Utility;

namespace StringPlay
{
    public class FirstNonRepeatingCharacter : IStringModifier
    {
        public void Test(string stringToSearchIn)
        {
            MethodTester<char>.TestAllMethods( stringToSearchIn, "first non repeating character", 
                                               new Func<string, char>(TwoPointersSearch),
                                               new Func<string, char>(DictionarySearch),
                                               new Func<string, char>(CountEachCharacterSearch) );
        }

        #region Search Methods

        /// <summary>
        /// The slowest StringPlay method. It uses two loops: 
        /// <para>First loop goes through each character.
        /// Second loop compares char from the first one to each other character in the string.</para>
        /// </summary>
        /// <returns>First non repeating character in the given string.</returns>
        public static char TwoPointersSearch(string stringToSearchIn)
        {
            char foundCharacter = default;
            int stringLength = stringToSearchIn.Length;

            // First loop - go through each single character in a string
            for (int i=0; i<stringLength; i++)
            {
                bool characterRepeats = false;

                // Compare one chosen character to each other character in the string except for itself
                for (int ii=0; ii<stringLength; ii++)
                {
                    if (ii == i) continue; // The character should skip itself (when both pointers are referring to the same position in a string)
                    if (stringToSearchIn[i] == stringToSearchIn[ii])
                    {
                        characterRepeats = true;
                        break; // The character is definitely repeated at least once, there is no need to keep seaching for another duplicate
                    }
                }

                if (!characterRepeats)
                {
                    foundCharacter = stringToSearchIn[i];
                    break;
                }
            }

            return foundCharacter;
        }   

        /// <summary>
        /// Adds characters to a Dictionary and increments their value if it is found again.
        /// <para>In the second loop, if the value of a certain char is 1 - it is non repeating.</para>
        /// </summary>
        /// <returns>First non repeating character in the given string.</returns>
        public static char DictionarySearch(string stringToSearchIn)
        {
            char foundCharacter = default;
            int stringLength = stringToSearchIn.Length;

            Dictionary<char, int> existingCharacters = new Dictionary<char, int>();

            for(int i=0; i<stringLength; i++)
            {
                char currentKey = stringToSearchIn[i];
                int currentNumber = default;
                // If the character already exists on the Dictionary - increase corresponding number
                // TryGetValue returns if the key exists or not
                if (existingCharacters.TryGetValue(currentKey, out currentNumber))
                {
                    currentNumber++;
                    existingCharacters[currentKey] = currentNumber;
                }
                // If the character isn't on the Dictionary yet - put it there
                else 
                {
                    existingCharacters.Add(currentKey, 1);
                }
            }

            // Go through the same initial string in the second loop to make it FIRST non repeating character, not just any
            for(int i=0; i<stringLength; i++)
            {
                char currentKey = stringToSearchIn[i];
                int currentNumber = default;

                // TryGetValue also checks if such a key exists or not
                if (existingCharacters.TryGetValue(currentKey, out currentNumber))
                {
                    if (currentNumber == 1) 
                    {
                        foundCharacter = currentKey;
                        break;
                    }
                }
            }

            return foundCharacter;
        }

        /// <summary>
        /// Counts the amount of certain character occasions in a string.
        /// <para>Has a "skip list" which prevents checking a character more than once.
        /// If a character is found more than once => skip current character.</para>
        /// </summary>
        /// <param name="stringToSearchIn"></param>
        /// <returns></returns>
        public static char CountEachCharacterSearch(string stringToSearchIn)
        {
            char foundCharacter = default;
            List<char> checkedCharacters = new List<char>();
            int stringLength = stringToSearchIn.Length;

            for (int i=0; i<stringLength; i++)
            {
                char currentCharacter = stringToSearchIn[i];
                // Don't check the same character twice
                if (checkedCharacters.Contains(currentCharacter)) continue; 

                // If the character count is higher than 1 - add it to "skip list" so that you don't check it again
                if (StringCheck.CharacterRepeats(stringToSearchIn, currentCharacter))
                {
                    checkedCharacters.Add(currentCharacter);
                    continue;
                }
                else 
                // If the count is equal to 1 (can't be lower, because we are checking it from the existing string) => character found
                {
                    foundCharacter = currentCharacter;
                    break;
                }
            }

            return foundCharacter;
        }

        #endregion
    }
}
