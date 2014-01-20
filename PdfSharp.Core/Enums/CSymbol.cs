namespace PdfSharp.Core.Enums
{
	/// <summary>
	/// Terminal symbols recognized by PDF content stream lexer.
	/// </summary>
	public enum CSymbol
	{
		None,
		Comment, Integer, Real, /*Boolean?,*/ String, HexString, UnicodeString, UnicodeHexString,
		Name, Operator,
		BeginArray, EndArray,
		Eof,
	}
}