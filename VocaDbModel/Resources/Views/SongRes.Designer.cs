﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace VocaDb.Model.Resources.Views
{
	using System;


	/// <summary>
	///   A strongly-typed resource class, for looking up localized strings, etc.
	/// </summary>
	// This class was auto-generated by the StronglyTypedResourceBuilder
	// class via a tool like ResGen or Visual Studio.
	// To add or remove a member, edit your .ResX file then rerun ResGen
	// with the /str option, or rebuild your VS project.
	[global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
	[global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
	[global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
	internal class SongRes
	{
		private static global::System.Resources.ResourceManager resourceMan;

		private static global::System.Globalization.CultureInfo resourceCulture;

		[global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		internal SongRes()
		{
		}

		/// <summary>
		///   Returns the cached ResourceManager instance used by this class.
		/// </summary>
		[global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
		internal static global::System.Resources.ResourceManager ResourceManager
		{
			get
			{
				if (object.ReferenceEquals(resourceMan, null))
				{
					global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("VocaDb.Model.Resources.Views.SongRes", typeof(SongRes).Assembly);
					resourceMan = temp;
				}
				return resourceMan;
			}
		}

		/// <summary>
		///   Overrides the current thread's CurrentUICulture property for all
		///   resource lookups using this strongly typed resource class.
		/// </summary>
		[global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
		internal static global::System.Globalization.CultureInfo Culture
		{
			get
			{
				return resourceCulture;
			}
			set
			{
				resourceCulture = value;
			}
		}

		/// <summary>
		///   Looks up a localized string similar to Please make sure the song you&apos;re about to submit hasn&apos;t been added before. You need to enter at least one name and artist. Link to the original PV is very desirable.
		///
		///Please note that a song without Vocaloid vocals is allowed only if it&apos;s released on an album together with Vocaloid songs, or there&apos;s multiple Vocaloid covers of that song. UTAU and other voice synthesizers are allowed as well, but human-sung covers are not..
		/// </summary>
		internal static string NewSongInfo
		{
			get
			{
				return ResourceManager.GetString("NewSongInfo", resourceCulture);
			}
		}

		/// <summary>
		///   Looks up a localized string similar to VocaDB song rankings.
		/// </summary>
		internal static string RankingsTitle
		{
			get
			{
				return ResourceManager.GetString("RankingsTitle", resourceCulture);
			}
		}
	}
}
