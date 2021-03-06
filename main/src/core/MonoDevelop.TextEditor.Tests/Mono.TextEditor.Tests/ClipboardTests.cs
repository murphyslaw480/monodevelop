//
// ClipboardTests.cs
//
// Author:
//       Mike Krüger <mkrueger@xamarin.com>
//
// Copyright (c) 2013 Xamarin Inc. (http://xamarin.com)
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
using System;
using NUnit.Framework;
using System.Linq;
using Gtk;

namespace Mono.TextEditor.Tests
{
	[TestFixture]
	public class ClipboardTests : TextEditorTestBase, ITextPasteHandler
	{
		#region ITextPasteHandler implementation

		public string FormatPlainText (int offset, string text, byte[] copyData)
		{
			return "Hello World";
		}

		public byte[] GetCopyData (TextSegment segment)
		{
			return null;
		}

		#endregion

		[Test]
		public void TestTextPasteHandler ()
		{
			var data = Create (
				@"$");

			Clipboard clipboard = Clipboard.Get (Mono.TextEditor.ClipboardActions.CopyOperation.CLIPBOARD_ATOM);
			clipboard.Text = "hello";

			data.TextPasteHandler = this;

			ClipboardActions.Paste (data);

			Check (data, @"Hello World$");
		}

		[Test]
		public void TestUndoSteps ()
		{
			var data = Create (
				@"$");

			Clipboard clipboard = Clipboard.Get (Mono.TextEditor.ClipboardActions.CopyOperation.CLIPBOARD_ATOM);
			clipboard.Text = "hello";

			data.TextPasteHandler = this;

			ClipboardActions.Paste (data);

			Check (data, @"Hello World$");
			MiscActions.Undo (data);
			Check (data, @"hello$");
			MiscActions.Undo (data);
			Check (data, @"$");
		}
	}
}

