//
// FileTabCommands.cs
//
// Author:
//   Mike Krüger <mkrueger@novell.com>
//
// Copyright (C) 2007 Novell, Inc (http://www.novell.com)
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using System;

using Gtk;

using MonoDevelop.Components.Commands;
using MonoDevelop.Ide.Gui;
using MonoDevelop.Ide.Navigation;
using MonoDevelop.Core;

namespace MonoDevelop.Ide.Commands
{
	public enum FileTabCommands
	{
		CloseAllButThis,
		CopyPathName,
		ToggleMaximize,
		ReopenClosedTab,
		OpenContainingFolder
	}
	
	class CloseAllButThisHandler : CommandHandler
	{
		protected override void Run ()
		{
			IdeApp.Workbench.CloseAllDocuments (true);
		}
	}
	
	class ToggleMaximizeHandler : CommandHandler
	{
		protected override void Run ()
		{
			IdeApp.Workbench.ToggleMaximize ();
		}
	}
	
	class CopyPathNameHandler : CommandHandler
	{
		protected override void Run ()
		{
			Document document = IdeApp.Workbench.ActiveDocument;
			if (document == null)
				return;
			
			Clipboard clipboard = Clipboard.Get (Gdk.Atom.Intern ("CLIPBOARD", false));
			clipboard.Text = document.FileName;
			clipboard = Clipboard.Get (Gdk.Atom.Intern ("PRIMARY", false));
			clipboard.Text = document.FileName;
		}
	}

	class ReopenClosedTabHandler : CommandHandler
	{
		protected override void Run ()
		{
			NavigationHistoryService.OpenLastClosedDocument ();
		}

		protected override void Update (CommandInfo info)
		{
			info.Enabled = NavigationHistoryService.HasClosedDocuments;
		}
	}

	class OpenContainingFolderHandler : CommandHandler
	{
		protected override void Run ()
		{
			// A tab will always hold a file, never a folder.
			FilePath path = System.IO.Path.GetDirectoryName (IdeApp.Workbench.ActiveDocument.FileName);
			DesktopService.OpenFolder (path);
		}

		protected override void Update (CommandInfo info)
		{
			var doc = IdeApp.Workbench.ActiveDocument;
			info.Enabled = doc != null && !doc.FileName.IsNullOrEmpty;
		}
	}
}
