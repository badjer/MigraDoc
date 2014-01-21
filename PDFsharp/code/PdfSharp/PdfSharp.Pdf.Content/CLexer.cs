#region PDFsharp - A .NET library for processing PDF

//
// Authors:
//   Stefan Lange (mailto:Stefan.Lange@pdfsharp.com)
//
// Copyright (c) 2005-2009 empira Software GmbH, Cologne (Germany)
//
// http://www.pdfsharp.com
// http://sourceforge.net/projects/pdfsharp
//
// Permission is hereby granted, free of charge, to any person obtaining a
// copy of this software and associated documentation files (the "Software"),
// to deal in the Software without restriction, including without limitation
// the rights to use, copy, modify, merge, publish, distribute, sublicense,
// and/or sell copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included
// in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.

#endregion

using System;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using PdfSharp.Core.Enums;

namespace PdfSharp.Pdf.Content
{
	/// <summary>
	///     Lexical analyzer for PDF content files. Adobe specifies no grammar, but it seems that it
	///     is a simple post-fix notation.
	/// </summary>
	internal class CLexer
	{
		private static readonly double[] PowersOf10 = new double[] {1, 10, 100, 1000, 10000, 100000, 1000000, 10000000, 100000000, 1000000000};
		private readonly byte[] content;
		private readonly StringBuilder token = new StringBuilder();
		private int charIndex;
		private char currChar;
		private char nextChar;
		private CSymbol symbol = CSymbol.None;
		private long tokenAsLong;
		private double tokenAsReal;

		/// <summary>
		///     Initializes a new instance of the Lexer class.
		/// </summary>
		public CLexer(byte[] content)
		{
			this.content = content;
			charIndex = 0;
		}

		/// <summary>
		///     Gets or sets the current symbol.
		/// </summary>
		public CSymbol Symbol
		{
			get { return symbol; }
			set { symbol = value; }
		}

		/// <summary>
		///     Gets the current token.
		/// </summary>
		internal string Token
		{
			get { return token.ToString(); }
		}

		/// <summary>
		///     Interprets current token as integer literal.
		/// </summary>
		internal int TokenToInteger
		{
			get
			{
				Debug.Assert(tokenAsLong == Int32.Parse(token.ToString(), CultureInfo.InvariantCulture));
				//return Int32.Parse(this.token.ToString(), CultureInfo.InvariantCulture);
				return (int) tokenAsLong;
			}
		}

		/// <summary>
		///     Interpret current token as real or integer literal.
		/// </summary>
		internal double TokenToReal
		{
			get
			{
				Debug.Assert(tokenAsReal == Double.Parse(token.ToString(), CultureInfo.InvariantCulture));
				//return Double.Parse(token.ToString(), CultureInfo.InvariantCulture); 
				return tokenAsReal;
			}
		}

		public int ContLength
		{
			get { return content.Length; }
		}

		/// <summary>
		///     Reads the next token and returns its type.
		/// </summary>
		public CSymbol ScanNextToken()
		{
			Again:
			ClearToken();
			char ch = MoveToNonWhiteSpace();
			switch (ch)
			{
				case '%':
					// Eat comments, the parser doesn't handle them
					//return this.symbol = ScanComment();
					ScanComment();
					goto Again;

				case '/':
					return symbol = ScanName();

					//case 'R':
					//  if (Lexer.IsWhiteSpace(this.nextChar))
					//  {
					//    ScanNextChar();
					//    return Symbol.R;
					//  }
					//  break;

				case '+':
				case '-':
					return symbol = ScanNumber();

				case '[':
					ScanNextChar();
					return symbol = CSymbol.BeginArray;

				case ']':
					ScanNextChar();
					return symbol = CSymbol.EndArray;

				case '(':
					return symbol = ScanLiteralString();

				case '<':
					return symbol = ScanHexadecimalString();

				case '.':
					return symbol = ScanNumber();

				case '"':
				case '\'':
					return symbol = ScanOperator();
			}
			if (Char.IsDigit(ch))
				return symbol = ScanNumber();

			if (Char.IsLetter(ch))
				return symbol = ScanOperator();

			if (ch == Chars.EOF)
				return symbol = CSymbol.Eof;

			Debug.Assert(false, "Unexpected character in content stream (maybe not implemented).");
			return symbol = CSymbol.None;
		}

		/// <summary>
		///     Scans a comment line. (Not yet used, comments are skipped by lexer.)
		/// </summary>
		public CSymbol ScanComment()
		{
			Debug.Assert(currChar == Chars.Percent);

			ClearToken();
			char ch;
			while ((ch = AppendAndScanNextChar()) != Chars.LF && ch != Chars.EOF)
			{
			}
			return symbol = CSymbol.Comment;
		}

		/// <summary>
		///     Scans the bytes of an inline image
		/// </summary>
		public CSymbol ScanInlineImage()
		{
			// TODO: 
			while (currChar != 'E' && nextChar != 'I')
				ScanNextChar();
			return CSymbol.None;
		}

		/// <summary>
		///     Scans a name.
		/// </summary>
		public CSymbol ScanName()
		{
			Debug.Assert(currChar == Chars.Slash);

			ClearToken();
			while (true)
			{
				char ch = AppendAndScanNextChar();
				if (IsWhiteSpace(ch) || IsDelimiter(ch))
					return symbol = CSymbol.Name;

				if (ch == '#')
				{
					ScanNextChar();
					char[] hex = new char[2];
					hex[0] = currChar;
					hex[1] = nextChar;
					ScanNextChar();
					// TODO Check syntax
					ch = (char) (ushort) int.Parse(new string(hex), NumberStyles.AllowHexSpecifier);
					currChar = ch;
				}
			}
		}

		/// <summary>
		///     Scans an integer or real number.
		/// </summary>
		public CSymbol ScanNumber()
		{
			long value = 0;
			int decimalDigits = 0;
			bool period = false;
			bool negative = false;

			ClearToken();
			char ch = currChar;
			if (ch == '+' || ch == '-')
			{
				if (ch == '-')
					negative = true;
				token.Append(ch);
				ch = ScanNextChar();
			}
			while (true)
			{
				if (char.IsDigit(ch))
				{
					token.Append(ch);
					if (decimalDigits < 10)
					{
						value = 10*value + ch - '0';
						if (period)
							decimalDigits++;
					}
				}
				else if (ch == '.')
				{
					if (period)
						throw new ContentReaderException("More than one period in number.");
					period = true;
					token.Append(ch);
				}
				else
					break;
				ch = ScanNextChar();
			}

			if (negative)
				value = -value;
			if (period)
			{
				if (decimalDigits > 0)
				{
					tokenAsReal = value/PowersOf10[decimalDigits];
					//this.tokenAsLong = value / PowersOf10[decimalDigits];
				}
				else
				{
					tokenAsReal = value;
					tokenAsLong = value;
				}
				return CSymbol.Real;
			}
			tokenAsLong = value;
			tokenAsReal = Convert.ToDouble(value);

			//long l = Int64.Parse(this.token.ToString(), CultureInfo.InvariantCulture);
			Debug.Assert(Int64.Parse(token.ToString(), CultureInfo.InvariantCulture) == value);

			if (value >= Int32.MinValue && value <= Int32.MaxValue)
				return CSymbol.Integer;
			//if (l > 0 && l <= UInt32.MaxValue)
			//  return Symbol.UInteger;
			throw new ContentReaderException("Number exceeds integer range.");
		}

		/// <summary>
		///     Scans an operator.
		/// </summary>
		public CSymbol ScanOperator()
		{
			ClearToken();
			char ch = currChar;
			// Scan token
			while (IsOperatorChar(ch))
				ch = AppendAndScanNextChar();

			return symbol = CSymbol.Operator;
		}

		// TODO
		public CSymbol ScanLiteralString()
		{
			Debug.Assert(currChar == Chars.ParenLeft);

			ClearToken();
			int parenLevel = 0;
			char ch = ScanNextChar();
			// Test UNICODE string
			if (ch == '\xFE' && nextChar == '\xFF')
			{
				// I'm not sure if the code is correct in any case.
				// ? Can a UNICODE character not start with ')' as hibyte
				// ? What about \# escape sequences
				ScanNextChar();
				char chHi = ScanNextChar();
				if (chHi == ')')
				{
					// The empty unicode string...
					ScanNextChar();
					return symbol = CSymbol.String;
				}
				char chLo = ScanNextChar();
				ch = (char) (chHi*256 + chLo);
				while (true)
				{
					SkipChar:
					switch (ch)
					{
						case '(':
							parenLevel++;
							break;

						case ')':
							if (parenLevel == 0)
							{
								ScanNextChar();
								return symbol = CSymbol.String;
							}
							parenLevel--;
							break;

						case '\\':
							{
								// TODO: not sure that this is correct...
								ch = ScanNextChar();
								switch (ch)
								{
									case 'n':
										ch = Chars.LF;
										break;

									case 'r':
										ch = Chars.CR;
										break;

									case 't':
										ch = Chars.HT;
										break;

									case 'b':
										ch = Chars.BS;
										break;

									case 'f':
										ch = Chars.FF;
										break;

									case '(':
										ch = Chars.ParenLeft;
										break;

									case ')':
										ch = Chars.ParenRight;
										break;

									case '\\':
										ch = Chars.BackSlash;
										break;

									case Chars.LF:
										ch = ScanNextChar();
										goto SkipChar;

									default:
										if (Char.IsDigit(ch))
										{
											// Octal character code
											int n = ch - '0';
											if (Char.IsDigit(nextChar))
											{
												n = n*8 + ScanNextChar() - '0';
												if (Char.IsDigit(nextChar))
													n = n*8 + ScanNextChar() - '0';
											}
											ch = (char) n;
										}
										break;
								}
								break;
							}

							// TODO ???
							//case '#':
							//  Debug.Assert(false, "Not yet implemented");
							//  break;

						default:
							break;
					}
					token.Append(ch);
					chHi = ScanNextChar();
					if (chHi == ')')
					{
						ScanNextChar();
						return symbol = CSymbol.String;
					}
					chLo = ScanNextChar();
					ch = (char) (chHi*256 + chLo);
				}
			}
			else
			{
				// 8-bit characters
				while (true)
				{
					SkipChar:
					switch (ch)
					{
						case '(':
							parenLevel++;
							break;

						case ')':
							if (parenLevel == 0)
							{
								ScanNextChar();
								return symbol = CSymbol.String;
							}
							parenLevel--;
							break;

						case '\\':
							{
								ch = ScanNextChar();
								switch (ch)
								{
									case 'n':
										ch = Chars.LF;
										break;

									case 'r':
										ch = Chars.CR;
										break;

									case 't':
										ch = Chars.HT;
										break;

									case 'b':
										ch = Chars.BS;
										break;

									case 'f':
										ch = Chars.FF;
										break;

									case '(':
										ch = Chars.ParenLeft;
										break;

									case ')':
										ch = Chars.ParenRight;
										break;

									case '\\':
										ch = Chars.BackSlash;
										break;

									case Chars.LF:
										ch = ScanNextChar();
										goto SkipChar;

									default:
										if (Char.IsDigit(ch))
										{
											// Octal character code
											int n = ch - '0';
											if (Char.IsDigit(nextChar))
											{
												n = n*8 + ScanNextChar() - '0';
												if (Char.IsDigit(nextChar))
													n = n*8 + ScanNextChar() - '0';
											}
											ch = (char) n;
										}
										break;
								}
								break;
							}

							// TODO ???
							//case '#':
							//  Debug.Assert(false, "Not yet implemented");
							//  break;

						default:
							break;
					}
					token.Append(ch);
					ch = ScanNextChar();
				}
			}
		}

		// TODO
		public CSymbol ScanHexadecimalString()
		{
			Debug.Assert(currChar == Chars.Less);

			ClearToken();
			char[] hex = new char[2];
			ScanNextChar();
			while (true)
			{
				MoveToNonWhiteSpace();
				if (currChar == '>')
				{
					ScanNextChar();
					break;
				}
				if (char.IsLetterOrDigit(currChar))
				{
					hex[0] = char.ToUpper(currChar);
					hex[1] = char.ToUpper(nextChar);
					int ch = int.Parse(new string(hex), NumberStyles.AllowHexSpecifier);
					token.Append(Convert.ToChar(ch));
					ScanNextChar();
					ScanNextChar();
				}
			}
			string chars = token.ToString();
			int count = chars.Length;
			if (count > 2 && chars[0] == (char) 0xFE && chars[1] == (char) 0xFF)
			{
				Debug.Assert(count%2 == 0);
				token.Length = 0;
				for (int idx = 2; idx < count; idx += 2)
					token.Append((char) (chars[idx]*256 + chars[idx + 1]));
			}
			return symbol = CSymbol.HexString;
		}

		/// <summary>
		///     Move current position one character further in content stream.
		/// </summary>
		internal char ScanNextChar()
		{
			if (ContLength <= charIndex)
			{
				currChar = Chars.EOF;
				nextChar = Chars.EOF;
			}
			else
			{
				currChar = nextChar;
				nextChar = (char) content[charIndex++];
				if (currChar == Chars.CR)
				{
					if (nextChar == Chars.LF)
					{
						// Treat CR LF as LF
						currChar = nextChar;
						if (ContLength <= charIndex)
							nextChar = Chars.EOF;
						else
							nextChar = (char) content[charIndex++];
					}
					else
					{
						// Treat single CR as LF
						currChar = Chars.LF;
					}
				}
			}
			return currChar;
		}

		/// <summary>
		///     Resets the current token to the empty string.
		/// </summary>
		private void ClearToken()
		{
			token.Length = 0;
			tokenAsLong = 0;
			tokenAsReal = 0;
		}

		/// <summary>
		///     Appends current character to the token and reads next one.
		/// </summary>
		internal char AppendAndScanNextChar()
		{
			token.Append(currChar);
			return ScanNextChar();
		}

		/// <summary>
		///     If the current character is not a white space, the function immediately returns it.
		///     Otherwise the PDF cursor is moved forward to the first non-white space or EOF.
		///     White spaces are NUL, HT, LF, FF, CR, and SP.
		/// </summary>
		public char MoveToNonWhiteSpace()
		{
			while (currChar != Chars.EOF)
			{
				switch (currChar)
				{
					case Chars.NUL:
					case Chars.HT:
					case Chars.LF:
					case Chars.FF:
					case Chars.CR:
					case Chars.SP:
						ScanNextChar();
						break;

					default:
						return currChar;
				}
			}
			return currChar;
		}

		/// <summary>
		///     Indicates whether the specified character is a content stream white-space character.
		/// </summary>
		internal static bool IsWhiteSpace(char ch)
		{
			switch (ch)
			{
				case Chars.NUL: // 0 Null
				case Chars.HT: // 9 Tab
				case Chars.LF: // 10 Line feed
				case Chars.FF: // 12 Form feed
				case Chars.CR: // 13 Carriage return
				case Chars.SP: // 32 Space
					return true;
			}
			return false;
		}

		/// <summary>
		///     Indicates whether the specified character is an content operator character.
		/// </summary>
		internal static bool IsOperatorChar(char ch)
		{
			if (Char.IsLetter(ch))
				return true;
			switch (ch)
			{
				case Chars.Asterisk: // *
				case Chars.QuoteSingle: // '
				case Chars.QuoteDbl: // "
					return true;
			}
			return false;
		}

		/// <summary>
		///     Indicates whether the specified character is a PDF delimiter character.
		/// </summary>
		internal static bool IsDelimiter(char ch)
		{
			switch (ch)
			{
				case '(':
				case ')':
				case '<':
				case '>':
				case '[':
				case ']':
					//case '{':
					//case '}':
				case '/':
				case '%':
					return true;
			}
			return false;
		}
	}
}